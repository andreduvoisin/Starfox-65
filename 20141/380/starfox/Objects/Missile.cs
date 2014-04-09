using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace itp380.Objects
{
    //Homing missile class, model should turn and follow its target.
    public class Missile : GameObject
    {
        //Projectile Velocity
        public Vector3 projectileVelocity = Vector3.Zero;
        Ship firingShip;
        Ship targetShip;

        public Missile(Game game, Ship projectileOwner, Ship target) :
            base(game)
        {
            this.firingShip = projectileOwner;
            this.targetShip = target;
            m_ModelName = "miner/Miner";
            Scale = 0.7f;
            Rotation = Quaternion.Identity;
            projectileVelocity = projectileOwner.Forward * 8f;
        }

        //Once a frame:

//Get vector spanning from missile to target
//Vector2 vectorToTarget = target.Position - missile.Position;

//Convert to Vector3 to do cross-product
//Vector3 vectorToTarget3 = new Vector3(vectorToTarget, 0);
//Vector3 missileVelocity3 = new Vector3(missile.Velocity, 0);

//Rotate clockwise/counter-clockwise is determined by sign of cross-product
//int crossProductSign = Vector3.Cross(missileVelocity3, vectorToTarget3).Z;

//Positive cross-product means rotate counter-clockwise, negative is clockwise
//double rotationAngle = 0;
//if(crossProductSign > 0)
  //  rotationAngle = -0.05;
//else if(crossProductSign < 0)
  //  rotationAngle = 0.05;

//I'm not sure how to do rotation in XNA, but the internets tell me it's something like this:
//missile.velocity = Vector2.Transform(missile.velocity, Matrix.CreateRotationZ(rotationAngle))
    }

    }