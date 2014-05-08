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
        public Game m_Game;

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

        float[] rots;
        public int m_Lives;
        public float respawnTime = 0.5f;
        public bool respawnTimerRunning = false;

        public Player(Game game, int playerIndex, Viewport viewport)
        {
            int pi = playerIndex - 1;
            bool a, b;
            float x, z;

            if (rots == null)
            {
                rots = new float[4];

                rots[0] = 3 * MathHelper.PiOver4;
                rots[1] = 1 * MathHelper.PiOver4;
                rots[2] = 5 * MathHelper.PiOver4;
                rots[3] = 7 * MathHelper.PiOver4;
            }

            a = (pi+1) % 2 == 0;
            b = pi == 2 || pi == 3;

            x = a ? Objects.Ship.SHIP_X_MIN + 20 : Objects.Ship.SHIP_X_MAX - 20;
            z = b ? Objects.Ship.SHIP_Z_MIN + 20 : Objects.Ship.SHIP_Z_MAX - 20;

            m_Health = 100;
            m_Ship = new Objects.Ship(game, this);
            m_Ship.Position = new Vector3(x, 25, z);
            m_Ship.Yaw = rots[pi];
            m_Camera = new Camera(game, m_Ship);
            GameState.Get().SpawnReticle(this, m_Camera);
            GameState.Get().SpawnLockedOn(this, m_Camera);
            GameState.Get().SpawnGameObject(m_Ship);
            m_Viewport = viewport;
            m_PlayerIndex = playerIndex;
            m_Lives = 3;
            m_Game = game;
        }

        public void SetRespawnTimer()
        {
            respawnTimerRunning = true;
            GameState.Get().m_Timer.AddTimer("RespawnPlayer" + m_PlayerIndex, respawnTime, Respawn, false);
        }

        public void Respawn()
        {
            int pi = m_PlayerIndex - 1;
            bool a, b;
            float x, z;

            a = (pi + 1) % 2 == 0;
            b = pi == 2 || pi == 3;

            x = a ? Objects.Ship.SHIP_X_MIN + 20 : Objects.Ship.SHIP_X_MAX - 20;
            z = b ? Objects.Ship.SHIP_Z_MIN + 20 : Objects.Ship.SHIP_Z_MAX - 20;

            m_Health = 100;
            m_Ship = new Objects.Ship(m_Game, this);
            m_Ship.Position = new Vector3(x, 25, z);
            m_Ship.Yaw = rots[pi];
            m_Camera.m_ShipTarget = m_Ship;
            GameState.Get().SpawnReticle(this, m_Camera);
            GameState.Get().SpawnLockedOn(this, m_Camera);
            GameState.Get().SpawnGameObject(m_Ship);

            respawnTimerRunning = false;
        }
    }
}
