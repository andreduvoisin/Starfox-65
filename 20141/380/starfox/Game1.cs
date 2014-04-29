//-----------------------------------------------------------------------------
// Main Game Class. This basically just creates some Singletons and then
// hands off all other logic to them.
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
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace itp380
{
	/// <summary>
	/// This is the main type for your game
	/// </summary>
    /// 

	public class Game1 : Microsoft.Xna.Framework.Game
	{

        ParticleSystem projectileTrailParticles;
        ParticleSystem smokePlumeParticles;
        List<Projectile> projectiles = new List<Projectile>();


		public Game1()
		{
            Content.RootDirectory = "Content";

            // Construct our particle system components.
            projectileTrailParticles = new ProjectileTrailParticleSystem(this, Content);
            smokePlumeParticles = new SmokePlumeParticleSystem(this, Content);


            // Set the draw order so the explosions and fire
            // will appear over the top of the smoke.
            smokePlumeParticles.DrawOrder = 100;
            projectileTrailParticles.DrawOrder = 300;

            // Register the particle system components.
            Components.Add(projectileTrailParticles);
            Components.Add(smokePlumeParticles);
            

			IsFixedTimeStep = true;
            TargetElapsedTime = TimeSpan.FromMilliseconds(1000.0 / 30.0);
			GraphicsManager.Get().Start(this);
			if (DebugDefines.bShowWindowsMouseCursor)
			{
				IsMouseVisible = true;
			}
			else
			{
				IsMouseVisible = false;
			}
			Window.Title = "Starfox 65";

            


		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			GameState.Get().Start(this);
			GameState.Get().SetState(eGameState.MainMenu);
            for (int i = 0; i < 4; i++)
            {
                InputManager.Get(i + 1).Start();
            }
			PhysicsManager.Get().Start(this);
			base.Initialize();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			GraphicsManager.Get().LoadContent();
			SoundManager.Get().LoadContent(Content);
		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// all content.
		/// </summary>
		protected override void UnloadContent()
		{
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			float fDeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
			if (fDeltaTime > 0.1f)
			{
				fDeltaTime = 0.1f;
			}

			// If the game doesn't have focus don't update anything
			if (IsActive)
			{
                for (int i = 0; i < 4; i++)
                {
                    InputManager.Get(i + 1).Update(fDeltaTime);
                }
				GameState.Get().Update(fDeltaTime);
                
			}
            UpdateSmokePlume();
            UpdateProjectiles(gameTime);
			base.Update(gameTime);
		}

        void UpdateSmokePlume()
        {
            // This is trivial: we just create one new smoke particle per frame.
            foreach (Models.Player player in GameState.Get().m_Players)
            {
                smokePlumeParticles.AddParticle(new Vector3(player.Ship.Position.X, player.Ship.Position.Y, player.Ship.Position.Z), new Vector3(player.Ship.Position.X , player.Ship.Position.Y, player.Ship.Position.Z));
                    // player.Ship.Position
            }
        }

        void UpdateProjectiles(GameTime gameTime)
        {
            int i = 0;

            while (i < projectiles.Count)
            {
                if (!projectiles[i].Update(gameTime))
                {
                    // Remove projectiles at the end of their life.
                    projectiles.RemoveAt(i);
                }
                else
                {
                    // Advance to the next projectile.
                    i++;
                }
            }
        }
		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			GraphicsManager.Get().Draw((float)gameTime.ElapsedGameTime.TotalSeconds);
            //GraphicsManager.Get().Projection
            //GameState.Get().m_Players[0].Camera.CameraMatrix
            foreach (Models.Player player in GameState.Get().m_Players)
            {
                smokePlumeParticles.SetCamera(player.Camera.CameraMatrix, GraphicsManager.Get().Projection);
            }

			base.Draw(gameTime);
		}
	}
}
