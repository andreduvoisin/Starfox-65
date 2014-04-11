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
        public Ship m_Ship;

        public Reticle(Game game, Ship ship) :
            base(game)
        {
            m_ModelName = "reticle";
            m_Ship = ship;
            Scale = 1f;
        }

        public override void Update(float fDeltaTime)
        {
            Position = m_Ship.Position + m_Ship.Forward * 25;
        }

        public override void Draw(float fDeltaTime)
        {
            //GraphicsDevice.BlendState = BlendState.Additive;
            base.Draw(fDeltaTime);

        }
    }
}
