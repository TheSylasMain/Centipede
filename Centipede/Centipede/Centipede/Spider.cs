using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
namespace Centipede
{
    class Spider
    {
        //screen is 1000 * 800
        private SpriteBatch spriteBatch;
        private double x, y, changeX, changeY, oldChangeX;
        private Rectangle pos, spiderRect;
        private int width, height;
        private Texture2D spriteSheet;//Sprites are 15*8, with a one pixel barrier in between;
                                      //The sheet is an entire 135*8;

        private Random r;

        public Spider(GraphicsDeviceManager graphics, Texture2D texture)
        {
            /**There are mutliple cases of randomized movement and placement**/
            r = new Random();


            /**These are for changing the width and height of the spider, in case it needs resizing**/
            width = 15 * 5;
            height = 8 * 5;


            /**This  controls whether or not the spider will spawn on the left side, or right side**/
            if (r.Next(2) == 0)
            {

                x = 0;
                y = graphics.GraphicsDevice.Viewport.Height * (4 / 5.0) - height;
            }
            else
            {
                x = graphics.GraphicsDevice.Viewport.Width - width;
                y = graphics.GraphicsDevice.Viewport.Height * (4 / 5.0) - height;
            }


            /**This is the rectangle that is displayed with the spider**/
            pos = new Rectangle((int)x, (int)y, width, height);

            /**This is the main rectangle for the spider's textures**/
            spiderRect = new Rectangle(0, 54, 15, 8);

            changeX = 2;
            changeY = 3;


            /*Load variables**/
            spriteBatch = new SpriteBatch(graphics.GraphicsDevice);
            spriteSheet = texture;

        }

        public void Update(GraphicsDeviceManager graphics, GameTime gameTime, List<Mushroom> mushrooms)
        {
            double time = gameTime.TotalGameTime.TotalMilliseconds; /**This is my timer, to let me see how long the game has been running**/

            /**This controls the texture of the spider, animating it**/
            if (time % 100 < 1)
            {
                spiderRect.X += 17;
                if (spiderRect.X > 130)
                    spiderRect.X = 0;
            }


            /**This is what makes the spider randomly change from moving across the screen, to a period of time where it strictly moves up and down**/
            if (time % 400 < 1)
            {
                if (r.Next(2) == 0)
                {
                    if (changeX == 0)
                        changeX = oldChangeX;
                }
            }
            if (time % 2500 < 1)
            {
                if (r.Next(2) == 0)
                {
                    if (changeX != 0)
                        oldChangeX = changeX;
                    changeX = 0;
                }

            }



            /**This part of the code controls the base movement, in order to bounce off walls, change direction, etc.**/
            if (pos.X < 0 || pos.X > graphics.GraphicsDevice.Viewport.Width - pos.Width)
                changeX *= -1;
            if (pos.Y < graphics.GraphicsDevice.Viewport.Height * (3 / 5.0) - height || pos.Y > graphics.GraphicsDevice.Viewport.Height - pos.Height)
                changeY *= -1;
            x += changeX;
            y += changeY;
            pos.X = (int)x;
            pos.Y = (int)y;

            if (gameTime.TotalGameTime.TotalMilliseconds % 750 < 1)
                checkMushroom(mushrooms);

        }

        //public void Draw()
        //{
        //    spriteBatch.Begin();
        //    spriteBatch.Draw(spriteSheet, pos, spiderRect, Color.White);
        //    spriteBatch.End();
        //}

        public Rectangle getPos()
        {
            return pos;
        }

        public Rectangle getSpiderTexture()
        {
            return spiderRect;
        }

        /**This method is for eating mushrooms**/
        public void checkMushroom(List<Mushroom> mushrooms)
        {
            for (int i = 0; i < mushrooms.Count; i++)
            {
                if (pos.Intersects(mushrooms.ElementAt(i).getPosition()))
                {
                    mushrooms.RemoveAt(i);
                }
            }
        }
    }
}