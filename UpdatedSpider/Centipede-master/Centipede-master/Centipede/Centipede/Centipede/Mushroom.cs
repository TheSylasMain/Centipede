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
    class Mushroom 
    {
        public Texture2D spriteSheet;
        public Rectangle s_mushroom; // source rectangle
        public Rectangle mushroom; // location on screen

        public Mushroom(ContentManager content, Game game, Random r) 
        {
            spriteSheet = content.Load<Texture2D>("Arcade - Centipede - General Sprites");
            s_mushroom = new Rectangle(68, 72, 8, 8);
            mushroom = new Rectangle(r.Next(800) + 1, r.Next(1000) + 1, 40, 40);
        }

        public void randShroom(Random r)
        {
            mushroom = new Rectangle(r.Next(800) + 1, r.Next(1000) + 1, 40, 40);
        }

        public Rectangle getPosition()
        {
            return mushroom;
        }
    }
}
