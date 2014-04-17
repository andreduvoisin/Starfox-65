//-----------------------------------------------------------------------------
// The GraphicsManager handles all lower-level aspects of rendering.
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
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace itp380
{
	public enum eDrawOrder
	{
		Default,
		Background,
		Foreground
	}

	public class GraphicsManager : itp380.Patterns.Singleton<GraphicsManager>
	{
		GraphicsDeviceManager m_Graphics;
		Game m_Game;
		SpriteBatch m_SpriteBatch;
		Texture2D m_Blank;

		SpriteFont m_FPSFont;

		LinkedList<GameObject> m_DefaultObjects = new LinkedList<GameObject>();
		LinkedList<GameObject> m_BGObjects = new LinkedList<GameObject>();
		LinkedList<GameObject> m_FGObjects = new LinkedList<GameObject>();

		public Matrix Projection;
		
		public bool IsFullScreen
		{
			get { return m_Graphics.IsFullScreen; }
			set { m_Graphics.IsFullScreen = value; }
		}

		public bool IsVSync
		{
			get { return m_Graphics.SynchronizeWithVerticalRetrace; }
			set { m_Graphics.SynchronizeWithVerticalRetrace = value; }
		}

		public int Width
		{
			get { return m_Graphics.PreferredBackBufferWidth; }
		}

		public int Height
		{
			get { return m_Graphics.PreferredBackBufferHeight; }
		}

		public GraphicsDevice GraphicsDevice
		{
			get { return m_Graphics.GraphicsDevice; }
		}

		float m_fZoom = GlobalDefines.fCameraZoom;
		
		public void Start(Game game)
		{
			m_Graphics = new GraphicsDeviceManager(game);
            m_Graphics.PreferMultiSampling = true;
			m_Game = game;
			IsVSync = GlobalDefines.bVSync;
			
			// TODO: Set resolution to what's saved in the INI, or default full screen
			if (!GlobalDefines.bFullScreen)
			{
				SetResolution(GlobalDefines.WindowedWidth, GlobalDefines.WindowHeight);
			}
			else
			{
				SetResolutionToCurrent();
				ToggleFullScreen();
			}
		}

		public void LoadContent()
		{
			//InitializeRenderTargets();
			m_SpriteBatch = new SpriteBatch(m_Graphics.GraphicsDevice);
						
			// Load FPS font
			m_FPSFont = m_Game.Content.Load<SpriteFont>("Fonts/FixedText");

			// Debug stuff for line drawing
			m_Blank = new Texture2D(m_Graphics.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
			m_Blank.SetData(new[] { Color.White });
		}

		public void SetResolutionToCurrent()
		{
			m_Graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
			m_Graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

			m_fZoom = GlobalDefines.fCameraZoom;
			SetProjection((float)Width / Height);

			if (m_Graphics.GraphicsDevice != null)
			{
				m_Graphics.ApplyChanges();
			}
		}

		public void SetResolution(int Width, int Height)
		{
			m_Graphics.PreferredBackBufferWidth = Width;
			m_Graphics.PreferredBackBufferHeight = Height;

			m_fZoom = GlobalDefines.fCameraZoom;
			SetProjection((float)Width / Height);

			if (m_Graphics.GraphicsDevice != null)
			{
				m_Graphics.ApplyChanges();
			}
		}

		public void SetProjection(float fAspectRatio)
		{
            Projection = Matrix.CreatePerspectiveFieldOfView((float)Math.PI / 2, fAspectRatio, 0.1f, 1200.0f);
		}

		public void ResetProjection()
		{
			m_fZoom = GlobalDefines.fCameraZoom;
			SetProjection((float)Width / Height);
		}

		public void ToggleFullScreen()
		{
			m_Graphics.ToggleFullScreen();
		}

		public void Draw(float fDeltaTime)
		{
            // Clear back buffer
            m_Graphics.GraphicsDevice.Clear(Color.LightSkyBlue);

            if (GameState.Get().State == eGameState.MainMenu)
            {
                // Now draw all 2D components
                m_SpriteBatch.Begin();

                // Draw the UI screens
                GameState.Get().DrawUI(fDeltaTime, m_SpriteBatch, null);

                // Draw FPS counter
                Vector2 vFPSPos = Vector2.Zero;
                if (DebugDefines.bShowBuildString)
                {
                    m_SpriteBatch.DrawString(m_FPSFont, DebugDefines.DebugName, vFPSPos, Color.White);
                    vFPSPos.Y += 25.0f;
                }
                if (DebugDefines.bShowFPS)
                {
                    string sFPS = String.Format("FPS: {0}", (int)(1 / fDeltaTime));
                    m_SpriteBatch.DrawString(m_FPSFont, sFPS, vFPSPos, Color.White);
                }

                m_SpriteBatch.End();
            }
            else if (GameState.Get().State == eGameState.Gameplay)
            {
                foreach (Models.Player player in GameState.Get().m_Players)
                {
                    // Set viewport and aspect ratio projection for this player.
                    GraphicsDevice.Viewport = player.m_Viewport;
                    SetProjection((float)player.m_Viewport.Width / player.m_Viewport.Height);

                    // First draw all 3D components
                    m_Game.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
                    // For background objects, disabled Z-Buffer
                    m_Game.GraphicsDevice.DepthStencilState = DepthStencilState.None;
                    foreach (GameObject o in m_BGObjects)
                    {
                        if (o.Enabled && (o.Owner == null || o.Owner == player))
                        {
                            o.Draw(fDeltaTime, player);
                        }
                    }

                    m_Game.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                    foreach (GameObject o in m_DefaultObjects)
                    {
                        if (o.Enabled && (o.Owner == null || o.Owner == player))
                        {
                            o.Draw(fDeltaTime, player);
                        }
                    }

                    // Also disabled Z-Buffer for background objects
                    m_Game.GraphicsDevice.DepthStencilState = DepthStencilState.None;
                    foreach (GameObject o in m_FGObjects)
                    {
                        if (o.Enabled && (o.Owner == null || o.Owner == player))
                        {
                            o.Draw(fDeltaTime, player);
                        }
                    }

                    // Now draw all 2D components
                    m_SpriteBatch.Begin();

                    // Draw the UI screens
                    GameState.Get().DrawUI(fDeltaTime, m_SpriteBatch, player);

                    // Draw FPS counter (only for player 1)
                    if (player.m_PlayerIndex == 1)
                    {
                        Vector2 vFPSPos = Vector2.Zero;
                        if (DebugDefines.bShowBuildString)
                        {
                            m_SpriteBatch.DrawString(m_FPSFont, DebugDefines.DebugName, vFPSPos, Color.White);
                            vFPSPos.Y += 25.0f;
                        }
                        if (DebugDefines.bShowFPS)
                        {
                            string sFPS = String.Format("FPS: {0}", (int)(1 / fDeltaTime));
                            m_SpriteBatch.DrawString(m_FPSFont, sFPS, vFPSPos, Color.White);
                        }
                    }

                    m_SpriteBatch.End();
                }
            }
		}

		public void AddGameObject(GameObject o)
		{
			if (o.DrawOrder == eDrawOrder.Background)
			{
				m_BGObjects.AddLast(o);
			}
			else if (o.DrawOrder == eDrawOrder.Default)
			{
				m_DefaultObjects.AddLast(o);
			}
			else
			{
				m_FGObjects.AddLast(o);
			}
		}

		public void RemoveGameObject(GameObject o)
		{
			if (o.DrawOrder == eDrawOrder.Background)
			{
				m_BGObjects.Remove(o);
			}
			else if (o.DrawOrder == eDrawOrder.Default)
			{
				m_DefaultObjects.Remove(o);
			}
			else
			{
				m_FGObjects.Remove(o);
			}
		}

		public void ClearAllObjects()
		{
			m_BGObjects.Clear();
			m_DefaultObjects.Clear();
			m_FGObjects.Clear();
		}

		// Draws a line
		public void DrawLine(SpriteBatch batch, float width, Color color, 
			Vector2 point1, Vector2 point2)
		{
			float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
			float length = Vector2.Distance(point1, point2);

			batch.Draw(m_Blank, point1, null, color,
					   angle, Vector2.Zero, new Vector2(length, width),
					   SpriteEffects.None, 0);
		}

		public void DrawLine3D(SpriteBatch batch, float width, Color color, Vector3 point1, Vector3 point2)
		{
            foreach (Models.Player player in GameState.Get().m_Players)
            {
                GraphicsDevice.Viewport = player.m_Viewport;

                // Convert the 3D points into screen space points
                Vector3 point1_screen = GraphicsDevice.Viewport.Project(point1, Projection,
                    player.Camera.CameraMatrix, Matrix.Identity);
                Vector3 point2_screen = GraphicsDevice.Viewport.Project(point2, Projection,
                    player.Camera.CameraMatrix, Matrix.Identity);

                // Now draw a 2D line with the appropriate points
                DrawLine(batch, width, color, new Vector2(point1_screen.X, point1_screen.Y),
                    new Vector2(point2_screen.X, point2_screen.Y));
            }
		}

		public void DrawFilled(SpriteBatch batch, Rectangle rect, Color color, float outWidth, Color outColor)
		{
			// Draw the background
			batch.Draw(m_Blank, rect, color);

			// Draw the outline
			DrawLine(batch, outWidth, outColor, new Vector2(rect.Left, rect.Top),
				new Vector2(rect.Right, rect.Top));
			DrawLine(batch, outWidth, outColor, new Vector2(rect.Left, rect.Top),
				new Vector2(rect.Left, rect.Bottom + (int)outWidth));
			DrawLine(batch, outWidth, outColor, new Vector2(rect.Left, rect.Bottom),
				new Vector2(rect.Right, rect.Bottom));
			DrawLine(batch, outWidth, outColor, new Vector2(rect.Right, rect.Top),
				new Vector2(rect.Right, rect.Bottom));
		}
	}
}
