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

         public Projectile(Game game, Ship projectileOwner) :
            base(game)
        {
            m_ModelName = "ship_bullet";
            Scale = 0.4f;
            Rotation = Quaternion.Identity;
            projectileVelocity = projectileOwner.Forward * 10f;
        }

         public void lifetimeReached()
         {
             GameState.Get().RemoveProjectile(this);
         }
    }
}
