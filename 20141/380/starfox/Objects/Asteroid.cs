using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace itp380.Objects
{
    public class Asteroid : GameObject
    {
        public Asteroid(Game game) :
            base(game)
        {
            m_ModelName = "Asteroid";
            Scale = 0.5f;
        }


    }
}