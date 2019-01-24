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
        float scale = 5; //scale of player
        int windowWidth, windowHeight;
        Texture2D image;

        Rectangle source;
        Animation animation;
        Rectangle alive;
        Rectangle[] dead;
        int animationTimer;

        /// <summary>
        /// note: player area is at the bottom 1/6.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="image"></param>
        /// <param name="windowWidth"></param>
        /// <param name="windowHeight"></param>
        public Player(Texture2D image, int windowWidth, int windowHeight)
        {
            x = windowWidth / 2 - Rect.Width / 2;
            y = windowHeight / 6 * 5 + windowHeight / 6 / 2 - Rect.Height / 2;
            this.image = image;
            this.windowWidth = windowWidth;
            this.windowHeight = windowHeight;

            Animation1 = Animation.Alive;
            animationTimer = 0;
            alive = new Rectangle(4, 7, 7, 10);
            dead = new Rectangle[8];
            for (int i = 0; i < dead.Length; i++)
            {
                dead[i] = new Rectangle(34 + i * 17, 0, 16, 8);
            }
            source = alive;
        }

        public void Update(GameTime gameTime, KeyboardState key, KeyboardState keyOld)
        {
            if(Animation1==Animation.Alive)
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
                if (rect.Right > windowWidth)
                {
                    x = windowWidth - rect.Width;
                }
                else if (rect.Left < 0)
                {
                    x = 0;
                }
                if (rect.Bottom > windowHeight)
                {
                    y = windowHeight - rect.Height;
                }
                else if (rect.Top < windowHeight / 6 * 5)//can't move above bottom 1/6 of the screen
                {
                    y = windowHeight / 6 * 5;
                }
            }
            else if(animation==Animation.Dead)
            {
                animationTimer++;
                if (animationTimer < dead.Length * 4)
                {
                    source = dead[animationTimer / 4];
                }
                else
                {
                    source = new Rectangle(0, 1, 1, 1);
                }
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(image, Rect, source, Color.White);
        }

        public void Die()
        {
            Animation1 = Animation.Dead;
        }

        enum Animation
        {
            Alive,
            Dead
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

        /// <summary>
        /// The box surrounding the player.
        /// </summary>
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

        private Animation Animation1
        {
            get
            {
                return animation;
            }

            set
            {
                if (value == Animation.Alive)
                {
                    source = alive;
                }
                if (value == Animation.Dead && animation==Animation.Alive)
                {
                    source = dead[0];
                    animationTimer = 0;
                    x -= 4 * scale;
                    Console.WriteLine("test");
                }

                animation = value;
            }
        }
    }
}