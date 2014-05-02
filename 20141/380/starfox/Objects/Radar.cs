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
        static float MaxRadarDist = 300;
        static Point Location = new Point(0, 250);
        static Point Size = new Point(100, 100);
        static int ShipSize = 5;

        // THESE are internal
        static Rectangle BackgroundRect = new Rectangle(Location.X, Location.Y, Size.X, Size.Y);

        Texture2D m_Background;

        public Radar(ContentManager Content)
        {
            m_Background = Content.Load<Texture2D>("HealthBar_border") as Texture2D;
        }

        static Vector2 V2Rotate(Vector2 v, float ang)
        {
            float cos = (float)Math.Cos(ang);
            float sin = (float)Math.Sin(ang);

            return new Vector2(
                v.X * cos - v.Y * sin,
                v.Y * sin + v.X * cos);
        }

        Rectangle GetRectangleForShip(Vector2 Position)
        {
            float fracX = Position.X / MaxRadarDist * Size.X;
            float fracY = Position.Y / MaxRadarDist * Size.Y;
            int RadarX = (int)MathHelper.Clamp(fracX, -Size.X/2, Size.X/2);
            int RadarY = (int)MathHelper.Clamp(fracY, -Size.Y/2, Size.Y/2);

            Vector2 Final = Vector2.Zero;
            Final += new Vector2(Location.X, Location.Y);               // For basic radar location on screen
            Final += new Vector2(Size.X/2, Size.Y/2);                   // For location within the radar -- If ship is at origin, the ship should be in the center of radar.
            Final -= new Vector2((float)ShipSize/2, (float)ShipSize/2); // Center the ship
            Final += new Vector2(RadarX, RadarY);                       // Add in ship coordinates.
            return new Rectangle((int)Final.X, (int)Final.Y, ShipSize, ShipSize);
        }

        Vector3 WorldToPlayerCoordinates(Vector3 Target, Objects.Ship Origin)
        {
            return Vector3.Transform(Target - Origin.Position, Matrix.CreateFromYawPitchRoll(-Origin.Yaw, 0, 0));
        }

        void DrawShip(SpriteBatch DrawBatch, Objects.Ship Ship, Objects.Ship Origin, Color Color)
        {
            Rectangle ShipRect;
            Vector3 PlayerCoords;
            Vector2 RelativePosition;

            PlayerCoords = WorldToPlayerCoordinates(Ship.Position, Origin);
            RelativePosition = new Vector2(PlayerCoords.Z, -PlayerCoords.X);
            ShipRect = GetRectangleForShip(RelativePosition);

            DrawBatch.Draw(m_Background, ShipRect, ShipRect, Color);
        }

        public void Draw(float fDeltaTime, SpriteBatch DrawBatch, Models.Player Player)
        {
            //Draw radar
            DrawBatch.Draw(m_Background, BackgroundRect, BackgroundRect, Color.Gray);
            DrawShip(DrawBatch, Player.Ship, Player.Ship, Color.Blue);

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
