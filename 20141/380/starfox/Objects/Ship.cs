using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace itp380.Objects
{
    public class Ship : GameObject
    {
        const float MAX_PITCH       = (float)Math.PI * 5f / 16f;
        const float PITCH_SPEED     = .08f;
        const float PITCH_DAMP      = .91f;
        const float YAW_SPEED       = .07f;

        const float SHIP_SPEED      = .1f;
        const float SHIP_FRICTION   = 3f;
        const int SHIP_CEILING      = 150;

        const uint BROLL_ROTS       = 2;
        const float BROLL_TIME      = 1.0f;
        const float BROLL_TVAL      = (float)BROLL_ROTS / BROLL_TIME;

        public enum ShipMoveState { NORMAL, BROLL };
        ShipMoveState m_MoveState;
        public ShipMoveState MoveState
        {
            get { return m_MoveState; }
        }

        //Ship Velocity
        public Vector3 shipVelocity = Vector3.Zero;

        //Ship Position
        public Vector3 shipPosition = Vector3.Zero;

        // Camera Ship Up
        Vector3 m_CameraUp;
        public Vector3 CameraUp
        {
            get
            {
                if (m_MoveState == ShipMoveState.BROLL)
                    return m_CameraUp;
                else
                    return Up;
            }
        }

        // Yaw and Pitch
        float m_Yaw, m_Pitch;

        // BarrelRoll Side and time
        public enum BarrelRollSide { LEFT, RIGHT };
        BarrelRollSide m_RollSide;
        float m_RollTime;

        // Reticle
        public Reticle m_Reticle;

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
            GameState.Get().SpawnReticle(this);
        }

        public override void Update(float fDeltaTime)
        {
            base.Update(fDeltaTime);

            UpdateSpeed(fDeltaTime);
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

        void UpdateSpeed(float fDeltaTime)
        {
            // Update position, hold speed.
            Position += shipVelocity;
            GameState.Get().updateEngineSound();
        }

        void UpdateNormal(float fDeltaTime)
        {
            m_CameraUp = Up;

            // Yaw
            m_Yaw += -InputManager.Get().LeftThumbstick.X * YAW_SPEED;

            // Pitch
            m_Pitch += InputManager.Get().LeftThumbstick.Y * PITCH_SPEED;
            m_Pitch = MathHelper.Clamp(m_Pitch, -MAX_PITCH, MAX_PITCH);

            // Hack for pitch to force it to return to 0 when player is not going up/down.
            if ((Math.Abs(InputManager.Get().LeftThumbstick.Y) == 0) || Position.Y > SHIP_CEILING)
                m_Pitch *= PITCH_DAMP;

            Rotation = Quaternion.CreateFromYawPitchRoll(m_Yaw, 0, m_Pitch);

            // Update velocity, don't change position.
            if (Position.Y > SHIP_CEILING)
            {
                Vector3 tempShipVel = Forward * SHIP_SPEED * InputManager.Get().RightTrigger;
                if (tempShipVel.Y > 0)
                {
                    tempShipVel.Y = 0;
                }
                shipVelocity += tempShipVel;
                shipVelocity -= Forward * SHIP_SPEED * InputManager.Get().LeftTrigger;
            }
            else
            {
                shipVelocity += Forward * SHIP_SPEED * InputManager.Get().RightTrigger;
                shipVelocity -= Forward * SHIP_SPEED * InputManager.Get().LeftTrigger;
            }

            // Only reduce speed in normal mode.
            shipVelocity -= shipVelocity * SHIP_FRICTION * fDeltaTime;
        }

        public void PerformRoll(BarrelRollSide Side)
        {
            m_RollSide = Side;
            m_MoveState = ShipMoveState.BROLL;
            m_RollTime = 0;

            m_Timer.AddTimer(
                "EndRoll",
                BROLL_TIME,
                () =>
                {
                    m_MoveState = ShipMoveState.NORMAL;
                    Rotation = Quaternion.CreateFromYawPitchRoll(m_Yaw, 0, m_Pitch);
                },
                false);
        }

        void UpdateBarrelRoll(float fDeltaTime)
        {
            float RollDirection = (m_RollSide == BarrelRollSide.RIGHT) ? 1f : -1f;
            float RollAng;

            m_RollTime += fDeltaTime;

            RollAng = MathHelper.SmoothStep(
                0,
                RollDirection * (float)MathHelper.TwoPi,
                m_RollTime * BROLL_TVAL);

            Rotation = Quaternion.CreateFromYawPitchRoll(m_Yaw, 0, m_Pitch) *
                Quaternion.CreateFromAxisAngle(
                    Vector3.UnitX,
                    RollAng);
        }

        public void fireCannonProjectile()
        {
            GameState.Get().SpawnProjectile(this);
            canFire = false;
            m_Timer.AddTimer("EnableFire", .3f, () => { canFire = true; }, false);
        }
    }
}
