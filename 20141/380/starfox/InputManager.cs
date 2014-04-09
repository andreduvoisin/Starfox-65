//-----------------------------------------------------------------------------
// InputManager checks for key binds and adds them to the active binds list
// as appropriate.
// The implementation is similar to the one discussed later in Chapter 5.
//
// __Defense Sample for Game Programming Algorithms and Techniques
// Copyright (C) Sanjay Madhav. All rights reserved.
//
// Released under the Microsoft Permissive License.
// See LICENSE.txt for full details.
//-----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace itp380
{
	public enum eBindType
	{
		JustPressed, // Was just pressed
		JustReleased, // Was just released
		Held // Was just pressed OR being held
	}

    public enum eButton
    {
        A,
        B,
        X,
        Y,
        LeftShoulder,
        RightShoulder,
        Back,
        Start,
        LeftStick,
        RightStick,
        DUp,
        DLeft,
        DRight,
        DDown
    }

    public enum eBindings
    {
        // UI bindings
        UI_Exit = 0,
        UI_Okay,
        UI_Up,
        UI_Down,
        // Game bindings
        FireCannon,
        BRollLeft,
        BRollRight,
        NUM_BINDINGS
    }


	public class BindInfo
	{
		public BindInfo(eButton Button, eBindType Type)
		{
			m_Button = Button;
			m_Type = Type;
		}

		public eButton m_Button;
		public eBindType m_Type;
	}

	public class InputManager
	{
		// Gamepad binding map
		private SortedList<eBindings, BindInfo> m_Bindings;
		private void InitializeBindings()
		{
			m_Bindings = new SortedList<eBindings, BindInfo>();
			// UI Bindings
            m_Bindings.Add(eBindings.UI_Exit, new BindInfo(eButton.Start, eBindType.JustPressed));
            m_Bindings.Add(eBindings.UI_Okay, new BindInfo(eButton.A, eBindType.JustPressed));
            m_Bindings.Add(eBindings.UI_Up, new BindInfo(eButton.DUp, eBindType.JustPressed));
            m_Bindings.Add(eBindings.UI_Down, new BindInfo(eButton.DDown, eBindType.JustPressed));
            // Game Bindings
            m_Bindings.Add(eBindings.FireCannon, new BindInfo(eButton.A, eBindType.Held));
            m_Bindings.Add(eBindings.BRollLeft, new BindInfo(eButton.LeftShoulder, eBindType.JustPressed));
            m_Bindings.Add(eBindings.BRollRight, new BindInfo(eButton.RightShoulder, eBindType.JustPressed));
			// TODO: Add any additional bindings here
		}

		private SortedList<eBindings, BindInfo> m_ActiveBinds = new SortedList<eBindings, BindInfo>();

		// Mouse Data
		private MouseState m_PrevMouse;
		private MouseState m_CurrMouse;

		// The mouse position according to Windows
		private Point m_DeviceMousePos = Point.Zero;
		// The mouse position taking into account deltas, no clamping
		private Point m_ActualMousePos = Point.Zero;
		// Mouse position with clamping
		private Point m_MousePos = Point.Zero;
		
		public Point MousePosition
		{
			get { return m_MousePos; }
		}

        // Gamepad Data
        private PlayerIndex m_Index;
        private Dictionary<eButton, Boolean> m_PrevState;
        private Dictionary<eButton, Boolean> m_CurrState;

        public Vector2 LeftThumbstick
        {
            get { return GamePad.GetState(m_Index, GamePadDeadZone.IndependentAxes).ThumbSticks.Left; }
        }

        public Vector2 RightThumbstick
        {
            get { return GamePad.GetState(m_Index, GamePadDeadZone.Circular).ThumbSticks.Right; }
        }

        public float LeftTrigger
        {
            get { return GamePad.GetState(m_Index, GamePadDeadZone.Circular).Triggers.Left; }
        }

        public float RightTrigger
        {
            get { return GamePad.GetState(m_Index, GamePadDeadZone.Circular).Triggers.Right; }
        }

        // Singleton stuff
        private static Dictionary<PlayerIndex, InputManager> m_Managers = new Dictionary<PlayerIndex,InputManager>();

        public static InputManager Get(PlayerIndex Index)
        {
            if (!m_Managers.ContainsKey(Index))
            {
                m_Managers[Index] = new InputManager(Index);
            }

            return m_Managers[Index];
        }

        public static InputManager Get()
        {
            return Get(PlayerIndex.One);
        }

        private InputManager(PlayerIndex Index)
        {
            m_Index = Index;
        }

		public void Start()
		{
			InitializeBindings();

			m_PrevMouse = Mouse.GetState();
			m_CurrMouse = Mouse.GetState();

			m_DeviceMousePos.X = m_CurrMouse.X;
			m_DeviceMousePos.Y = m_CurrMouse.Y;

			m_ActualMousePos = m_DeviceMousePos;
			m_MousePos = m_ActualMousePos;
			ClampMouse();

            m_PrevState = m_CurrState = GetGamepadState();
		}

		private void ClampMouse()
		{
			if (m_MousePos.X < 0)
			{
				m_MousePos.X = 0;
			}
			if (m_MousePos.Y < 0)
			{
				m_MousePos.Y = 0;
			}
			if (m_MousePos.X > GraphicsManager.Get().Width)
			{
				m_MousePos.X = GraphicsManager.Get().Width - GlobalDefines.iMouseCursorSize / 4;
			}
			if (m_MousePos.Y > GraphicsManager.Get().Height)
			{
				m_MousePos.Y = GraphicsManager.Get().Height - GlobalDefines.iMouseCursorSize / 4;
			}
		}

		public void UpdateMouse(float fDeltaTime)
		{
			m_PrevMouse = m_CurrMouse;
			m_CurrMouse = Mouse.GetState();

			m_DeviceMousePos.X = m_CurrMouse.X;
			m_DeviceMousePos.Y = m_CurrMouse.Y;

			m_ActualMousePos = m_DeviceMousePos;
			m_MousePos = m_ActualMousePos;
						
			ClampMouse();

			// Check for click
			if (JustPressed(m_PrevMouse.LeftButton, m_CurrMouse.LeftButton))
			{
				// If the UI doesn't handle it, send it to GameState
				if (GameState.Get().UICount == 0 ||
					!GameState.Get().GetCurrentUI().MouseClick(m_MousePos))
				{
					GameState.Get().MouseClick(m_MousePos);
				}
			}
		}

		public void UpdateGamepad(float fDeltaTime)
		{
			m_PrevState = m_CurrState;
            m_CurrState = GetGamepadState();
			m_ActiveBinds.Clear();

			// Build the list of bindings which were triggered this frame
			foreach (KeyValuePair<eBindings, BindInfo> k in m_Bindings)
			{
                eButton Key = k.Value.m_Button;
				eBindType Type = k.Value.m_Type;
				switch (Type)
				{
					case (eBindType.Held):
						if ((m_PrevState[Key] &&
							m_CurrState[Key]) ||
							(!m_PrevState[Key] &&
							m_CurrState[Key]))
						{
							m_ActiveBinds.Add(k.Key, k.Value);
						}
						break;
					case (eBindType.JustPressed):
						if (!m_PrevState[Key] &&
							m_CurrState[Key])
						{
							m_ActiveBinds.Add(k.Key, k.Value);
						}
						break;
					case (eBindType.JustReleased):
						if (m_PrevState[Key] &&
							!m_CurrState[Key])
						{
							m_ActiveBinds.Add(k.Key, k.Value);
						}
						break;
				}
			}

			if (m_ActiveBinds.Count > 0)
			{
				// Send the list to the UI first, then any remnants to the game
				if (GameState.Get().UICount != 0)
				{
					GameState.Get().GetCurrentUI().GamepadInput(m_ActiveBinds);
				}

				GameState.Get().GamepadInput(m_ActiveBinds, fDeltaTime);
			}
		}

		public void Update(float fDeltaTime)
		{
			UpdateMouse(fDeltaTime);
			UpdateGamepad(fDeltaTime);
		}

		protected bool JustPressed(ButtonState Previous, ButtonState Current)
		{
			if (Previous == ButtonState.Released &&
				Current == ButtonState.Pressed)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

        protected Dictionary<eButton, Boolean> GetGamepadState()
        {
            Dictionary<eButton, Boolean> state =
                new Dictionary<eButton, Boolean>();
            GamePadState GPState = GamePad.GetState(m_Index);
            GamePadButtons Buttons = GPState.Buttons;
            GamePadDPad DPad = GPState.DPad;

            // All the buttons from GamePadButtons. Verbose but there aren't very many better
            // ways to do it.
            state[eButton.A]                = Buttons.A.Equals(ButtonState.Pressed);
            state[eButton.B]                = Buttons.B.Equals(ButtonState.Pressed);
            state[eButton.X]                = Buttons.X.Equals(ButtonState.Pressed);
            state[eButton.Y]                = Buttons.Y.Equals(ButtonState.Pressed);
            state[eButton.LeftShoulder]     = Buttons.LeftShoulder.Equals(ButtonState.Pressed);
            state[eButton.RightShoulder]    = Buttons.RightShoulder.Equals(ButtonState.Pressed);
            state[eButton.Back]             = Buttons.Back.Equals(ButtonState.Pressed);
            state[eButton.Start]            = Buttons.Start.Equals(ButtonState.Pressed);
            state[eButton.LeftStick]        = Buttons.LeftStick.Equals(ButtonState.Pressed);
            state[eButton.RightStick]       = Buttons.RightStick.Equals(ButtonState.Pressed);

            state[eButton.DUp]              = DPad.Up.Equals(ButtonState.Pressed);
            state[eButton.DLeft]            = DPad.Left.Equals(ButtonState.Pressed);
            state[eButton.DRight]           = DPad.Right.Equals(ButtonState.Pressed);
            state[eButton.DDown]            = DPad.Down.Equals(ButtonState.Pressed);

            return state;
        }
		
		// Convert key binding to string representing the name
		// TODO: THIS IS NOT LOCALIZED
        public string GetBinding(eBindings binding)
        {
            return m_Bindings[binding].m_Button.ToString();
        }
	}
}
