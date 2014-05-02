using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace itp380.Objects
{
    public class PowerUp : GameObject
    {
        float originalFireSpeed;
        Ship affectedShip;

        public PowerUp(Game game) :
            base(game)
        {
            m_ModelName = "Projectiles/Sphere";
            Scale = 12f;
        }

        public void startPowerUp(Ship ship)
        {
            originalFireSpeed = ship.FireSpeed;
            ship.FireSpeed = originalFireSpeed / 2f;
            m_Timer.AddTimer("Stop PowerUp", 5.0f, stopPowerUp, false);
        }

        public void stopPowerUp()
        {
            affectedShip.FireSpeed = originalFireSpeed;
        }

    }
}
