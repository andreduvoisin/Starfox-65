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

        const float SHIP_BOOST      = .08f;
        const float SHIP_SPEED      = .8f;
        const float SHIP_FRICTION   = 10f;
        const float SHIP_CEILING    = 150f;
        const float SHIP_FLOOR      = -60f;

        const uint BROLL_ROTS       = 2;
        const float BROLL_TIME      = 1f;
        const float BROLL_TVAL      = (float)BROLL_ROTS / BROLL_TIME;

        public enum ShipMoveState { NORMAL, BROLL };
        ShipMoveState m_MoveState;
        public ShipMoveState MoveState
        {
            get { return m_MoveState; }
        }

        //Ship Velocity
        float m_ShipSpeed = 0;
        Vector3 ShipVelocity
        {
            get { return Forward * (m_ShipSpeed + SHIP_SPEED); }
        }

        //Ship Position
        Vector3 m_ShipPosition = Vector3.Zero;

        // Yaw and Pitch
        float m_Yaw, m_Pitch, m_BRollAng;
        float Roll
        {
            get
            {
                float rv = InputManager.Get(m_Player.m_PlayerIndex).LeftThumbstick.X;
                if (m_MoveState == ShipMoveState.BROLL)
                    rv += m_BRollAng;

                return rv;
            }
        }

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

        List<Objects.Projectile> m_Projectiles;
        public List<Objects.Projectile> Projectiles
        {
            get { return m_Projectiles; }
        }

        // Reference to the ship's player owner.
        Models.Player m_Player;

        public Ship(Game game, Models.Player player) :
            base(game)
        {
            m_ModelName = "ship2";
            Scale = 0.4f;
            canFire = true;
            m_MoveState = ShipMoveState.NORMAL;
            m_Player = player;
            m_Projectiles = new List<Objects.Projectile>();
        }

        public override void Update(float fDeltaTime)
        {
            base.Update(fDeltaTime);

            UpdatePhysics(fDeltaTime);
            ApplyPhysics();

            // Sound
            GameState.Get().updateEngineSound();
        }

        void UpdatePhysics(float fDeltaTime)
        {
            // Yaw
            m_Yaw += -InputManager.Get(m_Player.m_PlayerIndex).LeftThumbstick.X * YAW_SPEED;

            // Pitch
            m_Pitch += InputManager.Get(m_Player.m_PlayerIndex).LeftThumbstick.Y * PITCH_SPEED;
            m_Pitch = MathHelper.Clamp(m_Pitch, -MAX_PITCH, MAX_PITCH);

            // Hack for pitch to force it to return to 0 when player is not going up/down.
            if ((Math.Abs(InputManager.Get(m_Player.m_PlayerIndex).LeftThumbstick.Y) == 0) || Position.Y > SHIP_CEILING)
                m_Pitch *= PITCH_DAMP;

            m_ShipSpeed -= m_ShipSpeed * SHIP_FRICTION * fDeltaTime;

            Position += ShipVelocity;

            if (m_MoveState == ShipMoveState.BROLL)
                UpdateBarrelRoll(fDeltaTime);
        }

        // Apply velocity and rotation.
        void ApplyPhysics()
        {
            Position += ShipVelocity;

            if (Position.Y > SHIP_CEILING)
                Position = new Vector3(Position.X, SHIP_CEILING, Position.Z);
            else if (Position.Y < SHIP_FLOOR)
                Position = new Vector3(Position.X, SHIP_FLOOR, Position.Z);

            Rotation = Quaternion.CreateFromYawPitchRoll(
                m_Yaw,
                0,
                m_Pitch);
            Rotation = Quaternion.Concatenate(Rotation, Quaternion.CreateFromAxisAngle(Forward, Roll));
        }

        public void Boost()
        {
            m_ShipSpeed += SHIP_BOOST;
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
                    m_BRollAng = 0f;
                },
                false);
        }

        void UpdateBarrelRoll(float fDeltaTime)
        {
            float RollDirection = (m_RollSide == BarrelRollSide.RIGHT) ? 1f : -1f;

            m_RollTime += fDeltaTime;

            m_BRollAng = MathHelper.SmoothStep(
                0,
                RollDirection * BROLL_ROTS * (float)MathHelper.TwoPi,
                m_RollTime * BROLL_TVAL);
        }

        public void fireCannonProjectile()
        {
            GameState.Get().SpawnProjectile(this);
            canFire = false;
            m_Timer.AddTimer("EnableFire", .3f, () => { canFire = true; }, false);
        }
    }
}
