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

	public enum eBindings
	{
		UI_Exit = 0,
		// TODO: Add more bindings to the enum
		NUM_BINDINGS
	}

	public class BindInfo
	{
		public BindInfo(Keys Key, eBindType Type)
		{
			m_Key = Key;
			m_Type = Type;
		}

		public Keys m_Key;
		public eBindType m_Type;
	}

	public class InputManager : itp380.Patterns.Singleton<InputManager>
	{
		// Keyboard binding map
		private SortedList<eBindings, BindInfo> m_Bindings;
		private void InitializeBindings()
		{
			m_Bindings = new SortedList<eBindings, BindInfo>();
			// UI Bindings
			m_Bindings.Add(eBindings.UI_Exit, new BindInfo(Keys.Escape, eBindType.JustPressed));
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

		// Keyboard Data
		private KeyboardState m_PrevKey;
		private KeyboardState m_CurrKey;

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

			m_PrevKey = Keyboard.GetState();
			m_CurrKey = Keyboard.GetState();
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

		public void UpdateKeyboard(float fDeltaTime)
		{
			m_PrevKey = m_CurrKey;
			m_CurrKey = Keyboard.GetState();
			m_ActiveBinds.Clear();

			// Build the list of bindings which were triggered this frame
			foreach (KeyValuePair<eBindings, BindInfo> k in m_Bindings)
			{
				Keys Key = k.Value.m_Key;
				eBindType Type = k.Value.m_Type;
				switch (Type)
				{
					case (eBindType.Held):
						if ((m_PrevKey.IsKeyDown(Key) &&
							m_CurrKey.IsKeyDown(Key)) ||
							(!m_PrevKey.IsKeyDown(Key) &&
							m_CurrKey.IsKeyDown(Key)))
						{
							m_ActiveBinds.Add(k.Key, k.Value);
						}
						break;
					case (eBindType.JustPressed):
						if (!m_PrevKey.IsKeyDown(Key) &&
							m_CurrKey.IsKeyDown(Key))
						{
							m_ActiveBinds.Add(k.Key, k.Value);
						}
						break;
					case (eBindType.JustReleased):
						if (m_PrevKey.IsKeyDown(Key) &&
							!m_CurrKey.IsKeyDown(Key))
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
					GameState.Get().GetCurrentUI().KeyboardInput(m_ActiveBinds);
				}

				GameState.Get().KeyboardInput(m_ActiveBinds, fDeltaTime);
			}
		}

		public void Update(float fDeltaTime)
		{
			UpdateMouse(fDeltaTime);
			UpdateKeyboard(fDeltaTime);
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
		
		// Convert key binding to string representing the name
		// TODO: THIS IS NOT LOCALIZED
		public string GetBinding(eBindings binding)
		{
			Keys k = m_Bindings[binding].m_Key;
			string name = Enum.GetName(typeof(Keys), k);
			if (k == Keys.OemPlus)
			{
				name = "+";
			}
			else if (k == Keys.OemMinus)
			{
				name = "-";
			}

			return name;
		}
	}
}
