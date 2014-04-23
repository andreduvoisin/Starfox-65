using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace itp380.Models
{
    public class Player
    {
        uint m_Health;
        public uint Health
        {
            set { m_Health = value; }
            get { return m_Health; }
        }

        Objects.Ship m_Ship;
        public Objects.Ship Ship
        {
            get { return m_Ship; }
        }

        Camera m_Camera;
        public Camera Camera
        {
            get { return m_Camera; }
        }

        public Viewport m_Viewport;

        // player number (e.g. player 1, player 2, player 3, or player 4)
        // helps with input management and stuff
        public int m_PlayerIndex;

        public Player(Game game, int playerIndex, Viewport viewport)
        {
            m_Health = 100;
            m_Ship = new Objects.Ship(game, this);
            m_Ship.Position = new Vector3(0, 20 * playerIndex, 50 * playerIndex);
            m_Camera = new Camera(game, m_Ship);
            GameState.Get().SpawnReticle(this, m_Camera);
            GameState.Get().SpawnGameObject(m_Ship);
            m_Viewport = viewport;
            m_PlayerIndex = playerIndex;
        }
    }
}
