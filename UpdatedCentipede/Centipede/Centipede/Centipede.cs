﻿
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
    class Centipede : Game1
    {
        Texture2D spriteSheet;
        List<Rectangle> body = new List<Rectangle>();
        List<Rectangle> source = new List<Rectangle>();
        List<int> incrementValues = new List<int>();
        List<Mushroom> mushrooms;
        int length;
        int width;
        int heigth;
        int headPoint;
        int rotation = 180;
        int timer = 0;



        public Centipede(Texture2D spritesheet, int l, int w, int h, List<Mushroom> shrooms)
        {

            spriteSheet = spritesheet;
            length = l;
            this.width = w;
            this.heigth = h;
            headPoint = length * 80;
            mushrooms = shrooms;

            makeCentipede();
        }

        public void makeCentipede()
        {
            for (int i = 0; i < length; i++)
            {
                if (i == 0)
                {
                    //body.Add(new Rectangle(headPoint, 4, 48, 24));
                    body.Add(new Rectangle(headPoint, 4, 80, 40));
                    source.Add(new Rectangle(0, 18, 16, 8));
                    incrementValues.Add(1);
                }
                else
                {
                    //body.Add(new Rectangle(headPoint - (i * 48), 4, 48, 24));
                    body.Add(new Rectangle(headPoint - (i * 40), 4, 80, 40));
                    source.Add(new Rectangle(0, 36, 16, 8));
                    incrementValues.Add(1);
                }
            }
        }

        public void Move()
        {
            if (body.Count != 0)
            {
                Rectangle far = new Rectangle(width, 0, 20, heigth);
                Rectangle close = new Rectangle(-20, 0, 20, heigth);

                timer++;

                //movement
                for (int i = 0; i < body.Count; i++)
                {
                    body[i] = new Rectangle(body[i].X + incrementValues[i], body[i].Y, body[i].Width, body[i].Height);
                }

                //turn
                for (int i = 0; i < body.Count; i++)
                {
                    if (body[i].Intersects(close) || body[i].Intersects(far))
                    {
                        incrementValues[i] *= -1;
                        body[i] = new Rectangle(body[i].X, body[i].Y + body[i].Height, body[i].Width, body[i].Height);
                    }
                }

                //mushroom collision
                for (int i = 0; i < body.Count; i++)
                {
                    for (int j = 0; j < mushrooms.Count; j++)
                    {
                        if (body[i].Intersects(mushrooms[j].mushroom))
                        {
                            incrementValues[i] *= -1;
                            body[i] = new Rectangle(body[i].X, body[i].Y + body[i].Height, body[i].Width, body[i].Height);
                        }
                    }
                }

                //animation
                for (int i = 0; i < body.Count; i++)
                {

                    if (source[i].X < 119)
                        source[i] = new Rectangle(source[i].X + 17, source[i].Y, source[i].Width, source[i].Height);
                    else
                        source[i] = new Rectangle(0, source[i].Y, source[i].Width, source[i].Height);
                }
            }
        }

        public List<Mushroom> hitHead(ContentManager content, List<Rectangle> m)
        {
            if (body.Count != 0)
            {
                for (int i = 0; i < m.Count; i++)
                {
                    if (m[i].Intersects(body[0]))
                    {
                        while (body.Count > 0)
                        {
                            mushrooms.Add(new Mushroom(content, this, body[0].X, body[0].Y));
                            body.RemoveAt(0);
                        }

                    }
                }
            }
            return mushrooms;
        }

        public void UpdateMushroom(List<Mushroom> mushroom)
        {
            mushrooms = mushroom;
        }

        public List<Mushroom> hitTail(ContentManager content, List<Rectangle> m)
        {
            int tail = body.Count - 1;
            if (body.Count != 0)
            {
                for (int i = 0; i < m.Count; i++)
                {
                    if (m[i].Intersects(body[body.Count - 1]))
                    {
                        mushrooms.Add(new Mushroom(content, this, body[tail].X, body[tail].Y));
                        body.RemoveAt(tail);

                    }
                }
            }
            return mushrooms;
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            if (body.Count != 0)
            {
                for (int i = 0; i < body.Count; i++)
                    spriteBatch.Draw(spriteSheet, new Rectangle(body[i].X + body[i].Width / 2, body[i].Y + body[i].Height / 2, body[i].Width, body[i].Height), source[i], Color.White, MathHelper.ToRadians(rotation), new Vector2(source[i].Width / 2, source[i].Height / 2), SpriteEffects.None, 0);
            }
        }
    }
}
