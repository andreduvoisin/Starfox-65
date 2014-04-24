//-----------------------------------------------------------------------------
// UIGameplay is UI while in the main game state.
// Because there are so many aspects to the UI, this class is relatively large.
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
using Microsoft.Xna.Framework.Content;

namespace itp380.UI
{
	public class UIGameplay : UIScreen
	{
		SpriteFont m_FixedFont;
		SpriteFont m_FixedSmall;
		SpriteFont m_StatusFont;

        Texture2D m_HealthBar;

		public UIGameplay(ContentManager Content) :
			base(Content)
		{
			m_FixedFont = Content.Load<SpriteFont>("Fonts/FixedText");
			m_FixedSmall = Content.Load<SpriteFont>("Fonts/FixedSmall");
			m_StatusFont = Content.Load<SpriteFont>("Fonts/FixedTitle");

            //Healthbar
            m_HealthBar = Content.Load<Texture2D>("HealthBar_border") as Texture2D;
		}

		public override void Update(float fDeltaTime)
		{
			base.Update(fDeltaTime);
		}

		public override void Draw(float fDeltaTime, SpriteBatch DrawBatch, Models.Player player)
		{	
			base.Draw(fDeltaTime, DrawBatch, player);
            //Draw Healthbar
            DrawBatch.Draw(m_HealthBar, new Rectangle(20, 30, m_HealthBar.Width, 44), new Rectangle(0, 45, m_HealthBar.Width, 44), Color.Gray);
            DrawBatch.Draw(m_HealthBar, new Rectangle(20, 30, (int)(m_HealthBar.Width * ((double)player.Health / 100)), 44), new Rectangle(0, 45, m_HealthBar.Width, 44), Color.Red);
            DrawBatch.Draw(m_HealthBar, new Rectangle(20, 30, m_HealthBar.Width, 44), new Rectangle(0, 0, m_HealthBar.Width, 44), Color.White);

            //Draw radar
            //DrawBatch.Draw(m_HealthBar, new Rectangle(410, 300, 100, 100), new Rectangle(410, 300, 100, 100), Color.Gray);
            //DrawBatch.Draw(m_HealthBar, new Rectangle(410 + ((int)player.Ship.m_WorldBounds.Center.X / 5), 340 + ((int)player.Ship.m_WorldBounds.Center.Y / 5), 5, 5), new Rectangle(410 + ((int)player.Ship.m_WorldBounds.Center.X / 5), 340 +((int)player.Ship.m_WorldBounds.Center.Y / 5), 5, 5), Color.Red);

            // Draw GOC (Game Object Count) (only for player 1)
            if (player.m_PlayerIndex == 1)
            {
                DrawBatch.DrawString(
                    m_FixedFont,
                    String.Format("GOC: {0}", GameState.Get().GOC),
                    new Vector2(0, 50),
                    Color.White);
            }
		}

		public override void GamepadInput(SortedList<eBindings, BindInfo> binds)
		{
			GameState g = GameState.Get();
			if (binds.ContainsKey(eBindings.UI_Exit))
			{
				g.ShowPauseMenu();
				binds.Remove(eBindings.UI_Exit);
			}

			// Handle any input before the gameplay screen can look at it

			base.GamepadInput(binds);
		}
	}
}
