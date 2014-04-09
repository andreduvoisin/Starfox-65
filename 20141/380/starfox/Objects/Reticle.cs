using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace itp380.Objects
{
    public class Reticle : GameObject
    {
        public Reticle(Game game) :
            base(game)
        {
            m_ModelName = "reticle";
            Scale = 0.75f;
        }
    }
}
