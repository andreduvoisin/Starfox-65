using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace itp380.Objects
{
    class Radar
    {
        // Set THESE to change sizes
        static Point Location = new Point(412, 290);
        static Point Size = new Point(100, 100);
        static int ShipSize = 5;

        // THESE are internal
        static Rectangle BackgroundRect = new Rectangle(Location.X, Location.Y, Size.X, Size.Y);

        Texture2D m_Background;

        public Radar(ContentManager Content)
        {
            m_Background = Content.Load<Texture2D>("HealthBar_border") as Texture2D;
        }

        Rectangle GetRectangleForShip(Vector3 Position)
        {
            float fracX = (Position.X - Objects.Ship.SHIP_X_MIN) / (Objects.Ship.SHIP_X_MAX - Objects.Ship.SHIP_X_MIN) * Size.X;
            float fracY = (Position.Z - Objects.Ship.SHIP_Z_MIN) / (Objects.Ship.SHIP_Z_MAX - Objects.Ship.SHIP_Z_MIN) * Size.Y;
            int RadarX = (int)fracX - ShipSize / 2;
            int RadarY = (int)fracY - ShipSize / 2;

            return new Rectangle(Location.X + RadarX, Location.Y + RadarY, ShipSize, ShipSize);
        }

        void DrawShip(SpriteBatch DrawBatch, Objects.Ship Ship, Objects.Ship Origin, Color Color)
        {
            float Angle;
            Vector3 RelativePosition;
            Rectangle ShipRect;

            Angle = (float)Math.Asin(2 * Origin.Rotation.X * Origin.Rotation.Y + 2 * Origin.Rotation.Z * Origin.Rotation.W);
            RelativePosition = Vector3.Transform(Ship.Position - Origin.Position, Matrix.CreateFromAxisAngle(Vector3.UnitY, Angle));
            ShipRect = GetRectangleForShip(RelativePosition);

            DrawBatch.Draw(m_Background, ShipRect, ShipRect, Color);
        }

        public void Draw(float fDeltaTime, SpriteBatch DrawBatch, Models.Player Player)
        {
            //Draw radar
            DrawBatch.Draw(m_Background, BackgroundRect, BackgroundRect, Color.Gray);

            foreach (Models.Player p in GameState.Get().m_Players)
            {
                if (p != Player)
                {
                    DrawShip(DrawBatch, p.Ship, Player.Ship, Color.Red);
                }
            }
        }
    }
}
