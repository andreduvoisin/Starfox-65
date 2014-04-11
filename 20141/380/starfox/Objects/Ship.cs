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

        const float MAX_PITCH       = (float)Math.PI * 5f/16f;
        const float PITCH_SPEED     = .08f;
        const float PITCH_DAMP      = .91f;
        const float YAW_SPEED       = .07f;

        const float SHIP_SPEED      = .1f;
        const float SHIP_FRICTION   = 3f;

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
            m_Pitch = MathHelper.Clamp(m_Pitch, -MAX_PITCH, MAX_PITCH);

            // Hack for pitch to force it to return to 0 when player is not going up/down.
            if (Math.Abs(InputManager.Get().LeftThumbstick.Y) == 0)
                m_Pitch *= PITCH_DAMP;

            Rotation = Quaternion.CreateFromYawPitchRoll(m_Yaw, 0, m_Pitch);

            shipVelocity += Forward * SHIP_SPEED * InputManager.Get().RightTrigger;
            shipVelocity -= Forward * SHIP_SPEED * InputManager.Get().LeftTrigger;

            Position += shipVelocity;
            shipVelocity -= shipVelocity * SHIP_FRICTION * fDeltaTime;
            GameState.Get().updateEngineSound();
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
            m_Timer.AddTimer("EnableFire", .3f, () => { canFire = true; }, false);
        }
    }
}
