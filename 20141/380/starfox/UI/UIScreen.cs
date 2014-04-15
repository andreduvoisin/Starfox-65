//-----------------------------------------------------------------------------
// Base UIScreen class that all other UIScreens inherit from.
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
	public class UIScreen
	{
		protected LinkedList<Button> m_Buttons = new LinkedList<Button>();
        protected LinkedListNode<Button> selectedButton = null;
		protected ContentManager m_Content;
		protected float m_fLiveTime = 0.0f;
		// Determines whether or not you can press ESC to leave the screen
		protected bool m_bCanExit = false;
		protected Utils.Timer m_Timer = new Utils.Timer();

		public UIScreen(ContentManager Content)
		{
			m_Content = Content;
		}

		public virtual void Update(float fDeltaTime)
		{
			m_fLiveTime += fDeltaTime;

            IEnumerable<Button> mouseButtons = m_Buttons
                .Where(b => b.Enabled && b.m_Bounds.Contains(InputManager.Get(0).MousePosition));
            if (mouseButtons.Any() && GameState.Get().GetCurrentUI() == this)
            {
                foreach (Button b in m_Buttons)
			    {
				    // If the button is enabled, the mouse is pointing to it, and the UI is the top one
                    b.HasFocus = mouseButtons.Contains(b);
                    selectedButton = m_Buttons.Find(b);
			    }
            }

			m_Timer.Update(fDeltaTime);
		}

		public virtual void Draw(float fDeltaTime, SpriteBatch DrawBatch)
		{
			DrawButtons(fDeltaTime, DrawBatch);
		}

		public virtual bool MouseClick(Point Position)
		{
            bool success = false;
			foreach (Button b in m_Buttons)
			{
                if (b.Enabled && b.m_Bounds.Contains(Position))
                {
                    b.Click();
                    b.HasFocus = true;
                    selectedButton = m_Buttons.Find(b);
                    success = true;
                }
                else
                {
                    b.HasFocus = false;
                    selectedButton = null;
                }
			}

			return success;
		}

		protected void DrawButtons(float fDeltaTime, SpriteBatch DrawBatch)
		{
			foreach (Button b in m_Buttons)
			{
				if (b.Enabled)
				{
					b.Draw(fDeltaTime, DrawBatch);
				}
			}
		}

		public void DrawCenteredString(SpriteBatch DrawBatch, string sText, 
			SpriteFont font, Color color, Vector2 vOffset)
		{
			Vector2 pos = new Vector2(GraphicsManager.Get().Width / 2.0f, GraphicsManager.Get().Height / 2.0f);
			pos -= font.MeasureString(sText) / 2.0f;
			pos += vOffset;
			DrawBatch.DrawString(font, sText, pos, color);
		}

		public virtual void GamepadInput(SortedList<eBindings, BindInfo> binds)
		{
			if (binds.ContainsKey(eBindings.UI_Exit))
			{
				if (m_bCanExit)
				{
					GameState.Get().PopUI();
				}

				binds.Remove(eBindings.UI_Exit);
			}

            if (binds.ContainsKey(eBindings.UI_Okay))
            {
                if (selectedButton != null)
                    selectedButton.Value.Click();
                binds.Remove(eBindings.UI_Okay);
            }

            if (binds.ContainsKey(eBindings.UI_Up))
            {
                if (selectedButton != null)
                {
                    selectedButton.Value.HasFocus = false;
                    if ((selectedButton = selectedButton.Previous) == null)
                        selectedButton = m_Buttons.Last;
                    selectedButton.Value.HasFocus = true;
                }
                else
                {
                    selectedButton = m_Buttons.Last;
                }
                binds.Remove(eBindings.UI_Up);
            }

            if (binds.ContainsKey(eBindings.UI_Down))
            {
                if (selectedButton != null)
                {
                    selectedButton.Value.HasFocus = false;
                    if ((selectedButton = selectedButton.Next) == null)
                        selectedButton = m_Buttons.First;
                    selectedButton.Value.HasFocus = true;
                }
                else
                {
                    selectedButton = m_Buttons.First;
                }
                binds.Remove(eBindings.UI_Down);
            }
		}

		public virtual void OnExit()
		{

		}
	}
}
