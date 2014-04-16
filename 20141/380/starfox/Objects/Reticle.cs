using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace itp380.Objects
{
    public class Reticle : GameObject
    {
        Ship m_Ship;
        Camera m_Camera;

        public Reticle(Game game, Models.Player player, Camera camera) :
            base(game)
        {
            m_ModelName = "reticle";
            m_Owner = player;
            m_Ship = player.Ship;
            m_Camera = camera;
            Scale = 0.5f;
        }

        public override void Update(float fDeltaTime)
        {
            Position = m_Ship.Position + m_Ship.Forward * 25;

            Rotation = Quaternion.CreateFromRotationMatrix(Matrix.CreateBillboard(
                Position, m_Camera.Position, Vector3.UnitY, m_Camera.Forward));
            Rotation = Quaternion.Concatenate(Quaternion.CreateFromAxisAngle(Vector3.UnitX, (float)Math.PI / 2), Rotation);
        }
    }
}
