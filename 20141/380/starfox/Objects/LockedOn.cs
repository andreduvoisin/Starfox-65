using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace itp380.Objects
{
    public class LockedOn : GameObject
    {
        Ship m_Ship;
        Camera m_Camera;
        Ship m_TargetShip;

        public LockedOn(Game game, Models.Player player, Camera camera) :
            base(game)
        {
            m_ModelName = "lockedon";
            m_Owner = player;
            m_Ship = player.Ship;
            m_Camera = camera;
            Scale = 1.0f;

            m_DrawOrder = eDrawOrder.Foreground;
            m_lightingType = eLightingType.Ambient;

            m_TargetShip = null;
        }

        public void SetTarget(Ship target)
        {
            m_TargetShip = target;
        }

        public override void Update(float fDeltaTime)
        {
            if (m_TargetShip != null)
            {
                Position = m_TargetShip.Position;

                Rotation = Quaternion.CreateFromRotationMatrix(Matrix.CreateBillboard(
                    Position, m_Camera.Position, Vector3.UnitY, m_Camera.Forward));
                Rotation = Quaternion.Concatenate(Quaternion.CreateFromAxisAngle(Vector3.UnitX, (float)Math.PI / 2), Rotation);
            }
        }

        public override void Draw(float fDeltaTime, Models.Player player)
        {
            if (m_TargetShip != null)
            {
                base.Draw(fDeltaTime, player);
            }
        }
    }
}
