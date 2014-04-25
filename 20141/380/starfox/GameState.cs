﻿//-----------------------------------------------------------------------------
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
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

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
        // Main, top, bottom, upperleft, upperright, bottomleft, bottomright viewports.
        public Viewport mainViewport, topViewport, bottomViewport, ulViewport, urViewport, blViewport, brViewport;

        SoundEffectInstance flightSoundInstance;

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
        public uint GOC
        {
            get { return (uint)m_GameObjects.Count; }
        }

        List<Objects.building> m_Buildings = new List<Objects.building>(); // Building Array
        Objects.building m_building;
        Objects.grassfloor m_Terrain;

		// Timer class for the global GameState
		Utils.Timer m_Timer = new Utils.Timer();

		UI.UIGameplay m_UIGameplay;

        //Players
        public List<Models.Player> m_Players = new List<Models.Player>();

        //List<Objects.Projectile> m_Projectiles_P1 = new List<Objects.Projectile>(); // Asteroid Belt
		
		public void Start(Game game)
		{
			m_Game = game;
			m_State = eGameState.None;
			m_UIStack = new Stack<UI.UIScreen>();

            // Initialize Viewports
            InitializeViewports();
		}

        void InitializeViewports()
        {
            mainViewport = GraphicsManager.Get().GraphicsDevice.Viewport;

            topViewport = mainViewport;
            bottomViewport = mainViewport;
            ulViewport = mainViewport;
            urViewport = mainViewport;
            blViewport = mainViewport;
            brViewport = mainViewport;

            topViewport.Height = topViewport.Height / 2;

            bottomViewport.Height = bottomViewport.Height / 2;
            bottomViewport.Y = --topViewport.Height + 1;

            ulViewport.Height = ulViewport.Height / 2;
            ulViewport.Width = ulViewport.Width / 2;

            urViewport.Height = urViewport.Height / 2;
            urViewport.Width = urViewport.Width / 2;
            urViewport.X = --ulViewport.Width + 1;

            blViewport.Height = blViewport.Height / 2;
            blViewport.Width = blViewport.Width / 2;
            blViewport.Y = --ulViewport.Height + 1;

            brViewport.Height = brViewport.Height / 2;
            brViewport.Width = brViewport.Width / 2;
            brViewport.X = ulViewport.Width + 1;
            brViewport.Y = ulViewport.Height + 1;
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

            flightSoundInstance = SoundManager.Get().GetSoundEffect("FlyingSound").CreateInstance();

			// TODO: Add any gameplay setup here
            // UNCOMMENT THIS FOR 1 PLAYER
            //m_Players.Add(new Models.Player(m_Game, 1, mainViewport));
            // UNCOMMENT THESE FOR 2 PLAYERS
            //m_Players.Add(new Models.Player(m_Game, 1, topViewport));
            //m_Players.Add(new Models.Player(m_Game, 2, bottomViewport));
            // UNCOMMENT THESE FOR 3 OR 4 PLAYERS
            m_Players.Add(new Models.Player(m_Game, 1, ulViewport));
            m_Players.Add(new Models.Player(m_Game, 2, urViewport));
            m_Players.Add(new Models.Player(m_Game, 3, blViewport));
            m_Players.Add(new Models.Player(m_Game, 4, brViewport));

            //[JEAN] Spawn Level
            m_Terrain = new Objects.grassfloor(m_Game);
            m_Terrain.Position = new Vector3(200, -70, 100);
            m_Terrain.Rotation = Quaternion.Identity;
            SpawnGameObject(m_Terrain);
            m_building = new Objects.building(m_Game);
            m_building.Position = new Vector3(200, 0, 50);
            m_building.Rotation = Quaternion.Identity;
            SpawnGameObject(m_building);
            //[JEAN] Spawn buildings
            //Objects.building obj_building1 = new Objects.building(m_Game);
            //obj_building1.Position = new Vector3(200, -70, 100);
            //obj_building1.Rotation = Quaternion.Identity;
            //m_Buildings.Add(obj_building1);
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
                foreach (Models.Player player in m_Players)
                {
                    player.Camera.Update(fDeltaTime);
                }

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
                foreach (Models.Player player in m_Players)
                {
                    int projectileCount = player.Ship.Projectiles.Count;
                    for (int i = 0; i < projectileCount; i++ )
                    {
                        player.Ship.Projectiles.ElementAt(i).Position += player.Ship.Projectiles.ElementAt(i).projectileVelocity;
                        detectProjectileCollision(player.Ship.Projectiles.ElementAt(i));
                        if (projectileCount != player.Ship.Projectiles.Count)
                        {
                            projectileCount = player.Ship.Projectiles.Count;
                            i--;
                        }
                    }
                    detectBuildingCollision();
                    detectPlayerCollisions();
                }

                // Prints out the position of a ship.
                //System.Console.WriteLine("pos: " + m_Players[0].Ship.Position.X + " " + m_Players[0].Ship.Position.Y + " " + m_Players[0].Ship.Position.Z);
			}
		}

        public void detectProjectileCollision(Objects.Projectile projectile)
        {
            foreach (Models.Player player in m_Players)
            {
                if (projectile.m_WorldBounds.Intersects(player.Ship.m_WorldBounds) && projectile.projectileOwner != player.Ship)
                {
                    RemoveProjectile(projectile);
                    player.Health -= 10;
                    if (player.Health <= 0)
                    {
                        RemoveGameObject(player.Ship);
                        SoundManager.Get().PlaySoundCue("Snared");
                    }
                    break;
                }
            }
        }

        public void detectBuildingCollision()
        {
            foreach (Models.Player player in m_Players)
            {
                if (player.Ship.m_WorldBounds.Intersects(m_building.m_WorldBounds))
                {
                    RemoveGameObject(player.Ship);
                    SoundManager.Get().PlaySoundCue("Snared");
                }
                    
            }
        }

        public void detectPlayerCollisions()
        {
            foreach (Models.Player mainPlayer in m_Players)
            {
                foreach(Models.Player otherPlayer in m_Players)
                {
                    if (!(mainPlayer.Equals(otherPlayer)) && mainPlayer.Ship.WorldBounds.Intersects(otherPlayer.Ship.WorldBounds))
                    {
                        RemoveGameObject(mainPlayer.Ship);
                        RemoveGameObject(otherPlayer.Ship);
                    }
                }
            }
        }

		public void SpawnGameObject(GameObject o)
		{
			o.Load();
			m_GameObjects.AddLast(o);
			GraphicsManager.Get().AddGameObject(o);
		}

        public void SpawnProjectile(Objects.Ship ship)
        {
            //Find player with this ship.  Not necessary yet.
            Objects.Projectile cannonShot = new Objects.Projectile(m_Game, ship);
            cannonShot.Position = ship.Position;
            ship.Projectiles.Add(cannonShot);
            SpawnGameObject(cannonShot);
        }

        public void RemoveProjectile(Objects.Projectile projectile)
        {
            //Find player with this projectile.  Not necessary yet.  Use ship to search(max search of 4).
            foreach(Models.Player player in m_Players)
            {
                player.Ship.Projectiles.Remove(projectile);
            }
            RemoveGameObject(projectile);
        }

        public void SpawnReticle(Models.Player player, Camera camera)
        {
            player.Ship.m_Reticle = new Objects.Reticle(m_Game, player, camera);
            SpawnGameObject(player.Ship.m_Reticle);
        }

        public void updateEngineSound()
        {
            //Play engine sound only when the engine is on.
            foreach (Models.Player player in m_Players)
            {
                flightSoundInstance.Pitch = InputManager.Get(player.m_PlayerIndex).RightTrigger / 4;
                if (InputManager.Get(player.m_PlayerIndex).RightTrigger > 0)
                {
                    flightSoundInstance.Volume = .65f;
                    if (flightSoundInstance.State == SoundState.Stopped)
                    {
                        flightSoundInstance.Volume = 0.75f;
                        flightSoundInstance.IsLooped = true;
                        flightSoundInstance.Play();
                    }
                    else
                        flightSoundInstance.Resume();
                }
                else if (InputManager.Get(player.m_PlayerIndex).RightTrigger == 0)
                {
                    if (flightSoundInstance.State == SoundState.Playing)
                        flightSoundInstance.Volume = .4f;
                }
            }
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
		public void GamepadInput(SortedList<eBindings, BindInfo> binds, PlayerIndex pIndex, float fDeltaTime)
		{
            if (m_State == eGameState.MainMenu)
            {

            }
            else if (m_State == eGameState.Gameplay && !IsPaused)
			{
                Models.Player Player = m_Players[(int)pIndex];

                if (binds.ContainsKey(eBindings.FireCannon))
                {
                    //Fire cannon.
                    if (Player.Ship.CanFire)
                    {
                        Player.Ship.fireCannonProjectile();
                    }

                    binds.Remove(eBindings.FireCannon);
                }

                if (binds.ContainsKey(eBindings.Boost))
                {
                    Player.Ship.Boost();
                    binds.Remove(eBindings.Boost);
                }

                if (binds.ContainsKey(eBindings.BRollLeft))
                {
                    if (Player.Ship.MoveState == Objects.Ship.ShipMoveState.NORMAL)
                    {
                        Player.Ship.PerformRoll(Objects.Ship.BarrelRollSide.LEFT);
                        SoundManager.Get().GetSoundEffect("BarrelRoll").Play(.1f, 0, 0);
                    }
                    binds.Remove(eBindings.BRollLeft);
                }

                if (binds.ContainsKey(eBindings.BRollRight))
                {
                    if (Player.Ship.MoveState == Objects.Ship.ShipMoveState.NORMAL)
                    {
                        Player.Ship.PerformRoll(Objects.Ship.BarrelRollSide.RIGHT);
                        SoundManager.Get().GetSoundEffect("BarrelRoll").Play(.1f, 0, 0);
                    }
                    binds.Remove(eBindings.BRollRight);
                }
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
		public void DrawUI(float fDeltaTime, SpriteBatch batch, Models.Player player)
		{
			// We draw in reverse so the items at the TOP of the stack are drawn after those on the bottom
			foreach (UI.UIScreen u in m_UIStack.Reverse())
			{
                u.Draw(fDeltaTime, batch, player);
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
	}
}
