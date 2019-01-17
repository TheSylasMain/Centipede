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
        Texture2D image;

        Rectangle source = new Rectangle(0, 1, 16, 16);

        public Player(float x, float y, Texture2D image)
        {
            this.x = x;
            this.y = y;
            this.image = image;
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
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(image, new Rectangle((int)x, (int)y, (int)(source.Width * scale), (int)(source.Height * scale)), source, Color.White);
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