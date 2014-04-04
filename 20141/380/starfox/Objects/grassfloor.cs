using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace itp380.Objects
{
    public class grassfloor : GameObject
    {
        public grassfloor(Game game) :
            base(game)
        {
            m_ModelName = "grassy_floor";
            Scale = 30f;
        }


    }
}