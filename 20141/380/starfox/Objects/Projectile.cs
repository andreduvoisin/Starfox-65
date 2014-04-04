using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace itp380.Objects
{
    class Projectile : GameObject
    {
        //Projectile Velocity
        public Vector3 projectileVelocity = Vector3.Zero;


         public Projectile(Game game) :
            base(game)
        {
            m_ModelName = "ship_bullet";
            Scale = 0.4f;
            projectileVelocity = Forward * 10f;
        }




    }
}
