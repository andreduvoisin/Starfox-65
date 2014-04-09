using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace itp380.Objects
{
    public class Ship : GameObject
    {
        enum ShipMoveState { NORMAL, BROLL };
        ShipMoveState m_MoveState;

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

        // BarrelRoll Side and time
        enum BarrelRollSide { LEFT, RIGHT };
        BarrelRollSide m_RollSide;
        float m_RollTime;

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
            m_MoveState = ShipMoveState.NORMAL;
        }

        public override void Update(float fDeltaTime)
        {
            base.Update(fDeltaTime);

            switch (m_MoveState)
            {
                case ShipMoveState.NORMAL:
                    UpdateNormal(fDeltaTime);
                    break;
                case ShipMoveState.BROLL:
                    UpdateBarrelRoll(fDeltaTime);
                    break;
            }
        }

        void UpdateNormal(float fDeltaTime)
        {
            // Yaw
            m_Yaw += -InputManager.Get().LeftThumbstick.X * YAW_SPEED;

            // Pitch
            m_Pitch += InputManager.Get().LeftThumbstick.Y * PITCH_SPEED;
            m_Pitch = MathHelper.Clamp(m_Pitch, -MAX_PITCH * 0.7f, MAX_PITCH);

            Rotation = Quaternion.CreateFromYawPitchRoll(m_Yaw, 0, m_Pitch);

            shipVelocity += Forward * SHIP_SPEED * InputManager.Get().RightTrigger;
            shipVelocity -= Forward * SHIP_SPEED * InputManager.Get().LeftTrigger;
        }

        void UpdateBarrelRoll(float fDeltaTime)
        {
            float RollDirection = (m_RollSide == BarrelRollSide.RIGHT) ? 1f : -1f;
            m_RollTime += fDeltaTime;

            Rotation *= Quaternion.CreateFromAxisAngle(Vector3.UnitY, RollDirection * m_RollTime * (MathHelper.TwoPi / 2.0f));

            if (m_RollTime >= 2.0f)
            {
                m_MoveState = ShipMoveState.NORMAL;
                Rotation = Quaternion.CreateFromYawPitchRoll(m_Yaw, 0, m_Pitch);
            }
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
