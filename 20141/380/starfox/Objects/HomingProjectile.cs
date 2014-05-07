using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace itp380.Objects
{
    class HomingProjectile : Projectile
    {
        public Ship projectileTarget;

        public HomingProjectile(Game game, Ship projectileOwner, Ship projectileTarget) :
            base(game, projectileOwner)
        {
            m_ModelName = "homing_bullet1";
            this.projectileTarget = projectileTarget;
            if (projectileTarget != null)
            {

            }
        }
    }
}
