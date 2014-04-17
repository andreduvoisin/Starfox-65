//-----------------------------------------------------------------------------
// UIPauseMenu comes up when you hit Escape during gameplay.
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
	public class UIPauseMenu : UIScreen
	{
		SpriteFont m_TitleFont;
		SpriteFont m_ButtonFont;
		string m_PausedText;

		public UIPauseMenu(ContentManager Content) :
			base(Content)
		{
			m_bCanExit = true;

			m_TitleFont = m_Content.Load<SpriteFont>("Fonts/FixedTitle");
			m_ButtonFont = m_Content.Load<SpriteFont>("Fonts/FixedButton");

			m_PausedText = "Paused";
			// Create buttons
			Point vPos = new Point();
			vPos.X = (int) (GraphicsManager.Get().Width / 2.0f);
			vPos.Y = (int)(GraphicsManager.Get().Height / 2.0f);

			m_Buttons.AddLast(new Button(vPos, "Resume",
				m_ButtonFont, new Color(0, 0, 200), 
				Color.White, Resume, eButtonAlign.Center));

			vPos.Y += 50;
			m_Buttons.AddLast(new Button(vPos, "Quit",
				m_ButtonFont, new Color(0, 0, 200),
				Color.White, Quit, eButtonAlign.Center));

			SoundManager.Get().PlaySoundCue("MenuClick");
		}

		public void Resume()
		{
			GameState.Get().PopUI();
			SoundManager.Get().PlaySoundCue("MenuClick");
		}

		public void Quit()
		{
			GameState.Get().SetState(eGameState.MainMenu);
			SoundManager.Get().PlaySoundCue("MenuClick");
		}

		public override void Update(float fDeltaTime)
		{
			base.Update(fDeltaTime);
		}

		public override void Draw(float fDeltaTime, SpriteBatch DrawBatch, Models.Player player)
		{
            // Quick hacky fix to multiple players + pause menu, but better than what it looks like without this line.
            GraphicsManager.Get().GraphicsDevice.Viewport = GameState.Get().mainViewport;

            // Draw background
			GraphicsManager g = GraphicsManager.Get();

			Rectangle rect = new Rectangle(g.Width / 2 - 200, g.Height / 2 - 115,
				400, 250);
			g.DrawFilled(DrawBatch, rect, Color.Black, 4.0f, Color.DarkBlue);

			Vector2 vOffset = Vector2.Zero;
			vOffset.Y -= 75;
			DrawCenteredString(DrawBatch, m_PausedText, m_TitleFont, Color.White, vOffset);
						
			base.Draw(fDeltaTime, DrawBatch, player);
		}

		public override void OnExit()
		{
			GameState.Get().IsPaused = false;
		}
	}
}
