using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace itp380.Objects
{
    class HomingProjectile : Projectile
    {
        public float timePassed;
        public float speedFactor;
        public Vector3 sourceLocation;
        public Ship projectileTarget;

        public HomingProjectile(Game game, Ship projectileOwner, Ship projectileTarget) :
            base(game, projectileOwner)
        {
            m_ModelName = "homing_bullet1";
            this.projectileTarget = projectileTarget;
            sourceLocation = projectileOwner.Position;
            speedFactor = 1.0f;
        }

        public override void Update(float fDeltaTime)
        {
            if (projectileTarget != null)
            {
                timePassed += fDeltaTime;
                Position = Vector3.Lerp(sourceLocation, projectileTarget.Position, timePassed * speedFactor);
            }

            base.Update(fDeltaTime);
        }
    }
}
