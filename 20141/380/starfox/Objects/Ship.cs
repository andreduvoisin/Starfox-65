using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace itp380.Objects
{
    public class Ship : GameObject
    {
        const float MAX_PITCH = (float)Math.PI * 3f/8f;
        const float PITCH_SPEED = (float).08f;
        const float YAW_SPEED = (float).07f;

        const float SHIP_SPEED = (float).2f;

        //Ship Velocity
        public Vector3 shipVelocity = Vector3.Zero;

        //Ship Position
        public Vector3 shipPosition = Vector3.Zero;

        // Yaw and Pitch
        float m_Yaw, m_Pitch;

        private bool canFire;
        public bool CanFire
        {
            get { return canFire; }
        }

        public Ship(Game game) :
            base(game)
        {
            m_ModelName = "ship1";
            Scale = 0.4f;
            canFire = true;
        }

        public override void Update(float fDeltaTime)
        {
            base.Update(fDeltaTime);

            // Yaw
            m_Yaw += -InputManager.Get().LeftThumbstick.X * YAW_SPEED;

            // Pitch
            m_Pitch += InputManager.Get().LeftThumbstick.Y * PITCH_SPEED;
            m_Pitch = MathHelper.Clamp(m_Pitch, -MAX_PITCH * 0.7f, MAX_PITCH);

            Rotation = Quaternion.CreateFromYawPitchRoll(m_Yaw, 0, m_Pitch);

            shipVelocity += Forward * SHIP_SPEED * InputManager.Get().RightTrigger;
            shipVelocity -= Forward * SHIP_SPEED * InputManager.Get().LeftTrigger;
        }

        public void fireCannonProjectile()
        {
            GameState.Get().SpawnProjectile(this);
            canFire = false;
            m_Timer.AddTimer("EnableFire", .5f, enableFire, false);
        }

        public void enableFire()
        {
            canFire = true;
        }
    }
}
