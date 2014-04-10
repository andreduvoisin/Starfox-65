using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace itp380.Objects
{
    public class Projectile : GameObject
    {
        //Projectile Velocity
        public Vector3 projectileVelocity = Vector3.Zero;
        public Ship projectileOwner;

         public Projectile(Game game, Ship projectileOwner) :
            base(game)
        {
            m_ModelName = "ship_bullet";
            Scale = 0.7f;
            Rotation = projectileOwner.Rotation;
            this.projectileOwner = projectileOwner;
            projectileVelocity = projectileOwner.Forward * 8f;
            m_Timer.AddTimer("BulletDeath", 2f, lifetimeReached, false);
        }

         public void lifetimeReached()
         {
             GameState.Get().RemoveProjectile(this);
         }
    }
}
