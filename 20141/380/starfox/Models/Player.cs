using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace itp380.Models
{
    public class Player
    {
        uint m_Health;
        public uint Health
        {
            get { return m_Health; }
        }

        Objects.Ship m_Ship;
        public Objects.Ship Ship
        {
            get { return m_Ship; }
        }

        List<Objects.Projectile> m_Projectiles;
        public List<Objects.Projectile> Projectiles
        {
            get { return m_Projectiles; }
        }
                
        public Player(Game game)
        {
            m_Health = 100;
            m_Ship = new Objects.Ship(game);
            m_Projectiles = new List<Objects.Projectile>();
            GameState.Get().SpawnGameObject(m_Ship);
        }
    }
}
