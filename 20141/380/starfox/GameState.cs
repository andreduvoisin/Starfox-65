//-----------------------------------------------------------------------------
// The main GameState Singleton. All actions that change the game state,
// as well as any global updates that happen during gameplay occur in here.
// Because of this, the file is relatively lengthy.
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
using Microsoft.Xna.Framework.Graphics;

namespace itp380
{
	public enum eGameState
	{
		None = 0,
		MainMenu,
		Gameplay,
	}

	public class GameState : itp380.Patterns.Singleton<GameState>
	{
		Game m_Game;
		eGameState m_State;
        Random m_Random = new Random();
		public eGameState State
		{
			get { return m_State; }
		}

		eGameState m_NextState;
		Stack<UI.UIScreen> m_UIStack;
		bool m_bPaused = false;
		public bool IsPaused
		{
			get { return m_bPaused; }
			set	{ m_bPaused = value; }
		}

		// Keeps track of all active game objects
		LinkedList<GameObject> m_GameObjects = new LinkedList<GameObject>();
        List<Objects.Asteroid> m_Asteroids = new List<Objects.Asteroid>(); // Asteroid Belt

		// Camera Information
		Camera m_Camera;
		public Camera Camera
		{
			get { return m_Camera; }
		}

		public Matrix CameraMatrix
		{
			get { return m_Camera.CameraMatrix; }
		}

		// Timer class for the global GameState
		Utils.Timer m_Timer = new Utils.Timer();

		UI.UIGameplay m_UIGameplay;

        //Player ships
        Objects.Ship ship_P1;
		
		public void Start(Game game)
		{
			m_Game = game;
			m_State = eGameState.None;
			m_UIStack = new Stack<UI.UIScreen>();

			m_Camera = new Camera(m_Game);
		}

		public void SetState(eGameState NewState)
		{
			m_NextState = NewState;
		}

		private void HandleStateChange()
		{
			if (m_NextState == m_State)
				return;

			switch (m_NextState)
			{
				case eGameState.MainMenu:
					m_UIStack.Clear();
					m_UIGameplay = null;
					m_Timer.RemoveAll();
					m_UIStack.Push(new UI.UIMainMenu(m_Game.Content));
					ClearGameObjects();
					break;
				case eGameState.Gameplay:
					SetupGameplay();
					break;
			}

			m_State = m_NextState;
		}

		protected void ClearGameObjects()
		{
			// Clear out any and all game objects
			foreach (GameObject o in m_GameObjects)
			{
				RemoveGameObject(o, false);
			}
			m_GameObjects.Clear();
		}

		public void SetupGameplay()
		{
			ClearGameObjects();
			m_UIStack.Clear();
			m_UIGameplay = new UI.UIGameplay(m_Game.Content);
			m_UIStack.Push(m_UIGameplay);

			m_bPaused = false;
			GraphicsManager.Get().ResetProjection();
			
			m_Timer.RemoveAll();
					
			// TODO: Add any gameplay setup here
            ship_P1 = new Objects.Ship(m_Game);
            SpawnGameObject(ship_P1);

            m_Camera.CameraShipTarget = ship_P1;
            m_Camera.ComputeMatrix();

            //JEAN Spawn Asteroid Belt
            SpawnAsteroidBelt();
        }

		public void Update(float fDeltaTime)
		{
			HandleStateChange();

			switch (m_State)
			{
				case eGameState.MainMenu:
					UpdateMainMenu(fDeltaTime);
					break;
				case eGameState.Gameplay:
					UpdateGameplay(fDeltaTime);
					break;
			}

			foreach (UI.UIScreen u in m_UIStack)
			{
				u.Update(fDeltaTime);
			}
		}

		void UpdateMainMenu(float fDeltaTime)
		{

		}

		void UpdateGameplay(float fDeltaTime)
		{
			if (!IsPaused)
			{
				m_Camera.Update(fDeltaTime);

				// Update objects in the world
				// We have to make a temp copy in case the objects list changes
				LinkedList<GameObject> temp = new LinkedList<GameObject>(m_GameObjects);
				foreach (GameObject o in temp)
				{
					if (o.Enabled)
					{
						o.Update(fDeltaTime);
					}
				}
				m_Timer.Update(fDeltaTime);

				// TODO: Any update code not for a specific game object should go here
                ship_P1.Position += ship_P1.shipVelocity;
                ship_P1.shipVelocity *= .95f;

                // Calculate camera matrix to follow the ship.
                m_Camera.ComputeMatrix();

                //[JEAN] Spawn Asteroid Belt
			}
		}

		public void SpawnGameObject(GameObject o)
		{
			o.Load();
			m_GameObjects.AddLast(o);
			GraphicsManager.Get().AddGameObject(o);
		}

		public void RemoveGameObject(GameObject o, bool bRemoveFromList = true)
		{
			o.Enabled = false;
			o.Unload();
			GraphicsManager.Get().RemoveGameObject(o);
			if (bRemoveFromList)
			{
				m_GameObjects.Remove(o);
			}
		}

		public void MouseClick(Point Position)
		{
			if (m_State == eGameState.Gameplay && !IsPaused)
			{
				// TODO: Respond to mouse clicks here
			}
		}

		// I'm the last person to get keyboard input, so don't need to remove
		public void GamepadInput(SortedList<eBindings, BindInfo> binds, float fDeltaTime)
		{
			if (m_State == eGameState.Gameplay && !IsPaused)
			{
				// TODO: Add keyboard input handling for Gameplay
			}
		}

		public UI.UIScreen GetCurrentUI()
		{
			return m_UIStack.Peek();
		}

		public int UICount
		{
			get { return m_UIStack.Count; }
		}

		// Has to be here because only this can access stack!
		public void DrawUI(float fDeltaTime, SpriteBatch batch)
		{
			// We draw in reverse so the items at the TOP of the stack are drawn after those on the bottom
			foreach (UI.UIScreen u in m_UIStack.Reverse())
			{
				u.Draw(fDeltaTime, batch);
			}
		}

		// Pops the current UI
		public void PopUI()
		{
			m_UIStack.Peek().OnExit();
			m_UIStack.Pop();
		}

		public void ShowPauseMenu()
		{
			IsPaused = true;
			m_UIStack.Push(new UI.UIPauseMenu(m_Game.Content));
		}

		public void Exit()
		{
			m_Game.Exit();
		}

		void GameOver(bool victorious)
		{
			IsPaused = true;
			m_UIStack.Push(new UI.UIGameOver(m_Game.Content, victorious));
		}

        //JEAN code
        public void SpawnAsteroid(float x, float y, float z)
        {
            

            //Create Asteroid
            Objects.Asteroid obj_Asteroid; obj_Asteroid = new Objects.Asteroid(m_Game);
            //Add Asteroid to m_Asteroids
            m_Asteroids.Add(obj_Asteroid);
            //Set random rotation
            float asteroid_angle = (float)m_Random.Next(0, 628) / 100;
            //Set position and rotation of Asteroid
            obj_Asteroid.Position = new Vector3(x, y + (float)m_Random.Next(-100, 100) , z - 10 + (float)m_Random.Next(-100, 100));
            obj_Asteroid.Angle = asteroid_angle;
            SpawnGameObject(obj_Asteroid);
        }

        //Jean Code
        void SpawnAsteroidBelt()
        {
            for (int i = -100; i < 300; i++)
            {
                SpawnAsteroid(i, i, i);
            }
            for (int i = 300; i < 600; i++)
            {
                SpawnAsteroid(-i, -i, -i);
            }
        }
	}
}
