using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace itp380.Objects
{
    class Radar : GameObject
    {
        private SpriteBatch spriteBatch;
        GraphicsDevice graphicsDevice;

        private bool enabled = false;

        private float scale;
        private int dimension;

        private Vector2 position;

        private Texture2D backgroundImage;

        public float currentAngle = 0;

        private Vector3[] objectPositions;
        private Vector3 myPosition;
        private int highlight;

        /// <summary>
        /// Creates a new RadarComponent for the HUD.
        /// </summary>
        /// <param name="position">Component position on the screen.</param>
        /// <param name="backgroundImage">Image for the background of the radar.</param>
        /// <param name="spriteBatch">SpriteBatch that is required to draw the sprite.</param>
        /// <param name="scale">Factor to scale the graphics.</param>
        /// <param name="dimension">Dimension of the world.</param>
        /// <param name="graphicsDevice">Graphicsdevice that is required to create the textures for the objects.</param>
        public Radar(Vector2 position, Texture2D backgroundImage, SpriteBatch spriteBatch, float scale, int dimension, GraphicsDevice graphicsDevice, Game game) :
            base(game)
        {

            this.position = position;

            this.backgroundImage = backgroundImage;

            this.spriteBatch = spriteBatch;
            this.graphicsDevice = graphicsDevice;

            this.scale = scale;
            this.dimension = dimension;
        }

        /// <summary>
        /// Sets whether the component should be drawn.
        /// </summary>
        /// <param name="enabled">enable the component</param>
        public void Enable(bool enabled)
        {
            this.enabled = enabled;
        }

        /// <summary>
        /// Updates the positions of the objects to be drawn and the angle for the rotation of the radar.
        /// </summary>
        /// <param name="objectPositions">Position of all objects to be drawn.</param>
        /// <param name="highlight">Index of the object to be highlighted. Object with a smaller or a 
        /// greater index will be displayed in a smaller size and a different color.</param>
        /// <param name="currentAngle">Angle for the rotation of the radar.</param>
        /// <param name="myPosition">Position of the player.</param>
        
        public void update(Vector3[] objectPositions, int highlight, float currentAngle, Vector3 myPosition)
        {
            this.objectPositions = objectPositions;
            this.highlight = highlight;
            this.currentAngle = currentAngle;
            this.myPosition = myPosition;
        }

        /// <summary>
        /// Draws the RadarComponent with the values set before.
        /// </summary>
        public void Draw()
        {
            if (enabled)
            {
                spriteBatch.Draw(backgroundImage, position, null, Color.White, 0, new Vector2(backgroundImage.Width / 2, backgroundImage.Height / 2), scale, SpriteEffects.None, 0);

                for (int i = 0; i < objectPositions.Length; i++)
                {
                    Color myTransparentColor = new Color(255, 0, 0);
                    if (highlight == i)
                    {
                        myTransparentColor = new Color(255, 255, 0);
                    }
                    else if (highlight > i)
                    {
                        myTransparentColor = new Color(0, 255, 0);
                    }

                    Vector3 temp = objectPositions[i];
                    temp.X = temp.X / dimension * backgroundImage.Width / 2 * scale;
                    temp.Z = temp.Z / dimension * backgroundImage.Height / 2 * scale;

                    temp = Vector3.Transform(temp, Matrix.CreateRotationY(MathHelper.ToRadians(currentAngle)));

                    Rectangle backgroundRectangle = new Rectangle();
                    backgroundRectangle.Width = 2;
                    backgroundRectangle.Height = 2;
                    backgroundRectangle.X = (int)(position.X + temp.X);
                    backgroundRectangle.Y = (int)(position.Y + temp.Z);

                    Texture2D dummyTexture = new Texture2D(graphicsDevice, 1, 1);
                    dummyTexture.SetData(new Color[] { myTransparentColor });

                    spriteBatch.Draw(dummyTexture, backgroundRectangle, myTransparentColor);
                }

                myPosition.X = myPosition.X / dimension * backgroundImage.Width / 2 * scale;
                myPosition.Z = myPosition.Z / dimension * backgroundImage.Height / 2 * scale;

                myPosition = Vector3.Transform(myPosition, Matrix.CreateRotationY(MathHelper.ToRadians(currentAngle)));

                Rectangle backgroundRectangle2 = new Rectangle();
                backgroundRectangle2.Width = 5;
                backgroundRectangle2.Height = 5;
                backgroundRectangle2.X = (int)(position.X + myPosition.X);
                backgroundRectangle2.Y = (int)(position.Y + myPosition.Z);

                Texture2D dummyTexture2 = new Texture2D(graphicsDevice, 1, 1);
                dummyTexture2.SetData(new Color[] { Color.Pink });

                spriteBatch.Draw(dummyTexture2, backgroundRectangle2, Color.Pink);
            }
        }
    }
}
