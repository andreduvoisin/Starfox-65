using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace itp380.Objects
{
    public class Ship : GameObject
    {
        public Matrix[] Transforms;

        //Ship Velocity
        public Vector3 shipVelocity = Vector3.Zero;

        //Ship Position
        public Vector3 shipPosition = Vector3.Zero;

        public Matrix RotationMatrix = Matrix.Identity;

       

        private float rotation1;
        public float Rotation2
        {
            get { return rotation1; }
            set
            {
                float val = value;
                while (val >= MathHelper.TwoPi)
                {
                    val -= MathHelper.TwoPi;
                }
                while (val < 0)
                {
                    val += MathHelper.TwoPi;
                }
                if (rotation1 != val)
                {
                    rotation1 = val;
                    RotationMatrix = Matrix.CreateRotationY(rotation1);
                }
            }
        }

        public Ship(Game game) :
            base(game)
        {
            //m_ModelName = "Miner/Miner";
            m_ModelName = "ship1";
            Scale = 0.4f;
            m_Rotation = Quaternion.CreateFromAxisAngle(new Vector3(1.0f, -0.2f, 0.2f), 1.57f);
            Angle = 1.57f;
        }

        public override void Update(float fDeltaTime)
        {
            base.Update(fDeltaTime);
            //Rotation = Quaternion.CreateFromAxisAngle(new Vector3(0,0,1), Angle);
            Angle -= InputManager.Get().LeftThumbstick.X * .10f;
            
            shipVelocity += Forward * .1f * InputManager.Get().RightTrigger;
            shipVelocity -= Forward * .1f * InputManager.Get().LeftTrigger;

        }


    }
}
