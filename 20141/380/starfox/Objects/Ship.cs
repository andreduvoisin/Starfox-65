using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace itp380.Objects
{
    class Ship : GameObject
    {
        public Matrix[] Transforms;

        //Ship Velocity
        public Vector3 shipVelocity = Vector3.Zero;

        //Ship Position
        public Vector3 shipPosition = Vector3.Zero;

        public Matrix RotationMatrix = Matrix.Identity;

        private float rotation;
        public float Rotation
        {
            get { return rotation; }
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
                if (rotation != val)
                {
                    rotation = val;
                    RotationMatrix = Matrix.CreateRotationY(rotation);
                }
            }
        }

        public Ship(Game game) :
            base(game)
        {
            m_ModelName = "Miner/Miner";
            
        }

        public override void Update(float fDeltaTime)
        {
            base.Update(fDeltaTime);
            Angle -= InputManager.Get().LeftThumbstick.X * .10f;

            shipVelocity += Forward * .1f * InputManager.Get().RightTrigger;
        }


    }
}
