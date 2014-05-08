using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace itp380.Objects
{
    public class Ship : GameObject
    {
        const float MAX_PITCH       = (float)Math.PI * 4f / 16f;
        const float PITCH_SPEED     = .08f;
        const float PITCH_DAMP      = .89f;
        const float YAW_SPEED       = .07f;
        const float ROLL_SCALE      = 1.1f;

        const float SHIP_BOOST      = .4f;
        const float SHIP_SPEED      = 1f;
        const float SHIP_FRICTION   = 10f;
        const float SHIP_CEILING    = 145f;
        const float SHIP_FLOOR      = -55f;
        public const float SHIP_X_MIN      = -165f;
        public const float SHIP_X_MAX      = 600f;
        public const float SHIP_Z_MIN      = -375f;
        public const float SHIP_Z_MAX      = 475f;

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
                float rv = ROLL_SCALE * InputManager.Get(m_Player.m_PlayerIndex).LeftThumbstick.X;
                if (m_MoveState == ShipMoveState.BROLL)
                    rv += m_BRollAng;

                return rv;
            }
        }

        public float Yaw
        {
            set { m_Yaw = value; }
            get { return m_Yaw; }
        }

        // BarrelRoll Side and time
        public enum BarrelRollSide { LEFT, RIGHT };
        BarrelRollSide m_RollSide;
        float m_RollTime;

        // Reticle
        public Reticle m_Reticle;
        public LockedOn m_LockedOn;

        private bool canFire;
        public bool CanFire
        {
            get { return canFire; }
        }

        public bool fireHoming;
        public bool homingTimerActive;

        List<Objects.Projectile> m_Projectiles;
        public List<Objects.Projectile> Projectiles
        {
            get { return m_Projectiles; }
        }

        private float fireSpeed;
        public float FireSpeed
        {
            get { return fireSpeed; }
            set { fireSpeed = value; }
        }

        private float homingFireDelay;
        public float HomingFireDelay
        {
            get { return homingFireDelay; }
            set { homingFireDelay = value; }
        }

        public bool collidingAtBounds;

        // Reference to the ship's player owner.
        Models.Player m_Player;

        public Ship ShipUnderReticle;

        public Ship(Game game, Models.Player player) :
            base(game)
        {
            m_ModelName = "ship2";
            Scale = 0.9f;
            canFire = true;
            fireHoming = false;
            homingTimerActive = false;
            m_MoveState = ShipMoveState.NORMAL;
            m_Player = player;
            m_Projectiles = new List<Objects.Projectile>();
            fireSpeed = .1f;
            homingFireDelay = 1.5f;
            collidingAtBounds = false;
        }

        public override void Update(float fDeltaTime)
        {
            base.Update(fDeltaTime);
            UpdatePhysics(fDeltaTime);
            ApplyPhysics();

            // Reticle cover
            ShipUnderReticle = ClosestShipUnderReticle();
            m_LockedOn.SetTarget(ShipUnderReticle);
            if (ShipUnderReticle != null)
            {
                m_Reticle.ShowRedReticle();
            }
            else
            {
                m_Reticle.ShowGreenReticle();
            }

            // Sound
            GameState.Get().UpdateEngineSound();
        }

        void UpdatePhysics(float fDeltaTime)
        {
            // Yaw
            m_Yaw += -InputManager.Get(m_Player.m_PlayerIndex).LeftThumbstick.X * YAW_SPEED;
            if (m_Yaw < 0)
                m_Yaw += MathHelper.TwoPi;
            if (m_Yaw > MathHelper.TwoPi)
                m_Yaw -= MathHelper.TwoPi;

            // Pitch
            float PitchDelta = InputManager.Get(m_Player.m_PlayerIndex).LeftThumbstick.Y * PITCH_SPEED;
            if ((Position.Y > SHIP_CEILING && PitchDelta >= 0) || (Position.Y < SHIP_FLOOR && PitchDelta <= 0))
                m_Pitch *= PITCH_DAMP;
            else
                m_Pitch = MathHelper.Clamp(m_Pitch + PitchDelta, -MAX_PITCH, MAX_PITCH * 0.8f);

            // Hack for pitch to force it to return to 0 when player is not going up/down.
            if ((Math.Abs(InputManager.Get(m_Player.m_PlayerIndex).LeftThumbstick.Y) == 0) || Position.Y > SHIP_CEILING || Position.Y < SHIP_FLOOR)
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

            collidingAtBounds = false;

            // Check x/z max.
            if (Position.X > SHIP_X_MAX)
            {
                Position = new Vector3(SHIP_X_MAX, Position.Y, Position.Z);
                collidingAtBounds = true;
            }
            else if (Position.X < SHIP_X_MIN)
            {
                Position = new Vector3(SHIP_X_MIN, Position.Y, Position.Z);
                collidingAtBounds = true;
            }

            if (Position.Z > SHIP_Z_MAX)
            {
                Position = new Vector3(Position.X, Position.Y, SHIP_Z_MAX);
                collidingAtBounds = true;
            }
            else if (Position.Z < SHIP_Z_MIN)
            {
                Position = new Vector3(Position.X, Position.Y, SHIP_Z_MIN);
                collidingAtBounds = true;
            }

            Rotation = Quaternion.CreateFromYawPitchRoll(
                m_Yaw,
                0,
                m_Pitch);
            Rotation = Quaternion.Concatenate(Rotation, Quaternion.CreateFromAxisAngle(Forward, Roll));
        }

        float CheckReticleCover(Ship Other)
        {
            return Vector3.Cross(Forward, Other.Position - Position).Length();
        }

        Ship ClosestShipUnderReticle()
        {
            float best = float.PositiveInfinity;
            float current;
            Ship rv = null;

            foreach (Models.Player player in GameState.Get().m_Players)
            {
                if (this.m_Player == player)
                    continue;

                if (GameState.Get().m_GameObjects.Contains(player.Ship))
                {
                    current = CheckReticleCover(player.Ship);

                    if (current < best)
                    {
                        best = current;
                        rv = player.Ship;
                    }
                }
            }

            if (best < Reticle.LOCK_RADIUS &&
                (rv.Position - Position).Length() <= Reticle.MAX_LOCK_DIST)
                return rv;
            else
                return null;
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
            if (fireHoming)
            {
                GameState.Get().SpawnHomingProjectile(this);
                fireHoming = false;
                homingTimerActive = false;
            }
            else
            {
                GameState.Get().SpawnProjectile(this);
                canFire = false;
                m_Timer.AddTimer("EnableFire", FireSpeed, () => { canFire = true; }, false);
            }
        }

        public void chargeHomingCannonProjectile()
        {
            if (!homingTimerActive)
            {
                m_Timer.AddTimer("EnableHomingFire", HomingFireDelay, () => { fireHoming = true; }, false);
                homingTimerActive = true;
            }
        }

        public void handleFireButtonReleased()
        {
            if (m_Timer.GetRemainingTime("EnableHomingFire") > 0)
            {
                m_Timer.RemoveTimer("EnableHomingFire");
                homingTimerActive = false;
            }
        }
    }
}
