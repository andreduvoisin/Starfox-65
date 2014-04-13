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

        public Reticle(Game game, Ship ship, Camera camera) :
            base(game)
        {
            m_ModelName = "reticle";
            m_Ship = ship;
            m_Camera = camera;
            Scale = 1f;
        }

        public override void Update(float fDeltaTime)
        {
            Position = m_Ship.Position + m_Ship.Forward * 25;
            // Need to change model orientation or function won't work.
            /*Rotation = Quaternion.CreateFromRotationMatrix(
                Matrix.CreateBillboard(Position, m_Camera.Position, Vector3.UnitY, null));*/
        }

        public override void Draw(float fDeltaTime)
        {
            //GraphicsDevice.BlendState = BlendState.Additive;
            base.Draw(fDeltaTime);

        }
    }
}
