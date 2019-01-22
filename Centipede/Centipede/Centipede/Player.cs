using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace Centipede
{
    class Player
    {
        float x, y;
        float speed = 5; //spped of player movement
        float scale = 3; //scale of player
        int windowWidth, windowHeight;
        Texture2D image;

        Rectangle source = new Rectangle(0, 1, 16, 16);

        /// <summary>
        /// note: player area is at the bottom 1/6.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="image"></param>
        /// <param name="windowWidth"></param>
        /// <param name="windowHeight"></param>
        public Player(float x, float y, Texture2D image, int windowWidth, int windowHeight)
        {
            this.x = x;
            this.y = y;
            this.image = image;
            this.windowWidth = windowWidth;
            this.windowHeight = windowHeight;
        }

        /// <summary>
        /// Called when the Player fires a bullet
        /// </summary>
        void fireBullet()
        {
            //TODO bullet firing code
        }

        public void Update(GameTime gameTime, KeyboardState key, KeyboardState keyOld)
        {
            //movement
            if (key.IsKeyDown(Keys.Right) && key.IsKeyUp(Keys.Left))
            {
                x += speed;
            }
            if (key.IsKeyDown(Keys.Left) && key.IsKeyUp(Keys.Right))
            {
                x -= speed;
            }
            if (key.IsKeyDown(Keys.Up) && key.IsKeyUp(Keys.Down))
            {
                y -= speed;
            }
            if (key.IsKeyDown(Keys.Down) && key.IsKeyUp(Keys.Up))
            {
                y += speed;
            }

            //wall collision
            Rectangle rect = Rect;
            if (rect.Right>windowWidth)
            {
                x = windowWidth - rect.Width;
            }
            else if(rect.Left<0)
            {
                x = 0;
            }
            if(rect.Bottom>windowHeight)
            {
                y = windowHeight - rect.Height;
            }
            else if(rect.Top<windowHeight/6*5)//can't move above bottom 1/6 of the screen
            {
                y = windowHeight / 6 * 5;
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(image, Rect, source, Color.White);
        }

        public float X
        {
            get
            {
                return x;
            }

            set
            {
                x = value;
            }
        }

        public float Y
        {
            get
            {
                return y;
            }

            set
            {
                y = value;
            }
        }

        public Rectangle Rect
        {
            get
            {
                return new Rectangle((int)x, (int)y, (int)(source.Width * scale), (int)(source.Height * scale));
            }
        }

        public float Scale
        {
            get
            {
                return scale;
            }

            set
            {
                scale = value;
            }
        }

        public float Speed
        {
            get
            {
                return speed;
            }

            set
            {
                speed = value;
            }
        }
    }
}