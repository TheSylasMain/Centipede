﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Centipede
{
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


    class Missile : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        Texture2D full;
        Rectangle newMissle;
        public Missile()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

        }
        public void build(float x, float y)
        {
            newMissle = new Rectangle((int)x, (int)y, 16, 16);
        }

        public Rectangle getNewMissle()
        {
            return newMissle;
        }


    }
}

