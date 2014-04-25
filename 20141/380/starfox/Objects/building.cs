using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace itp380.Objects
{
    public class building : GameObject
    {
        public building(Game game) :
            base(game)
        {
            m_ModelName = "building1";
            Scale = 8f;
        }
    }
}
