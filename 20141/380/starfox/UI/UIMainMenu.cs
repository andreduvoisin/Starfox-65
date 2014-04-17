//-----------------------------------------------------------------------------
// UIMainMenu is the main menu UI.
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
	public class UIMainMenu : UIScreen
	{
		SpriteFont m_TitleFont;
		SpriteFont m_ButtonFont;
		string m_Title;

		public UIMainMenu(ContentManager Content) :
			base(Content)
		{
			m_TitleFont = m_Content.Load<SpriteFont>("Fonts/QuartzTitle");
			m_ButtonFont = m_Content.Load<SpriteFont>("Fonts/QuartzButton");

			// Create buttons
			Point vPos = new Point();
			vPos.X = (int) (GraphicsManager.Get().Width / 2.0f);
			vPos.Y = (int)(GraphicsManager.Get().Height / 2.0f);

			m_Title = "Starfox 65";

			m_Buttons.AddLast(new Button(vPos, "New Game", 
				m_ButtonFont, Color.DarkBlue, 
				Color.White, NewGame, eButtonAlign.Center));

			vPos.Y += 150;
			m_Buttons.AddLast(new Button(vPos, "Exit",
				m_ButtonFont, Color.DarkBlue,
				Color.White, Exit, eButtonAlign.Center));
		}

		public void NewGame()
		{
			SoundManager.Get().PlaySoundCue("MenuClick");
			GameState.Get().SetState(eGameState.Gameplay);
		}

		public void Options()
		{
		}

		public void Exit()
		{
			SoundManager.Get().PlaySoundCue("MenuClick");
			GameState.Get().Exit();
		}

		public override void Update(float fDeltaTime)
		{
			base.Update(fDeltaTime);
		}

		public override void Draw(float fDeltaTime, SpriteBatch DrawBatch, Models.Player player)
		{
			Vector2 vOffset = Vector2.Zero;
			vOffset.Y = -1.0f * GraphicsManager.Get().Height / 4.0f;
			DrawCenteredString(DrawBatch, m_Title, m_TitleFont, Color.DarkBlue, vOffset);

			base.Draw(fDeltaTime, DrawBatch, player);
		}
	}
}
