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
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        
        Texture2D centipedeSpriteSheet;
        Random rand = new Random();

        List<Mushroom> mushrooms;
        Spider spider;
        Boolean spiderOn;

        Rectangle shotMissleRect;
        bool shot = false; //why does this exist?
        List<Rectangle> lazers = new List<Rectangle>();
        
        KeyboardState key, keyi;

        Player player;

        Centipede c1;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            mushrooms = new List<Mushroom>();
            for (int i = 0; i < 25; i++)
            {
                mushrooms.Add(new Mushroom(Content, this, rand));
            }
            
            keyi = Keyboard.GetState();
            shotMissleRect = new Rectangle(24, 2, 1, 6);
            
            graphics.PreferredBackBufferHeight = 1000;
            graphics.PreferredBackBufferWidth = 800;
            graphics.ApplyChanges();
            
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            centipedeSpriteSheet = Content.Load<Texture2D>("Arcade - Centipede - General Sprites");
            player = new Player(centipedeSpriteSheet, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            c1 = new Centipede(centipedeSpriteSheet, 3, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, mushrooms);
            //spider = new Spider(graphics, spriteSheet);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            keyi = key;
            key = Keyboard.GetState();

            //update player
            player.Update(gameTime, key, keyi);

            //update lazers
            if (key.IsKeyDown(Keys.Space) && keyi.IsKeyUp(Keys.Space))
            {
                Missile newMissile = new Missile();

                newMissile.build(player.X+player.Rect.Width/2-shotMissleRect.Width*5/2, player.Y-shotMissleRect.Height*5);

                lazers.Add(newMissile.getNewMissle());
            }
            if (lazers.Count > 0)
            {
                for (int i = 0; i < lazers.Count; i++)
                {

                    Rectangle hold = lazers.ElementAt(i);
                    hold.Y -= 10;
                    lazers.Remove(lazers.ElementAt(i));
                    if (lazers.Count > 0)
                    {
                        lazers.Insert(i, hold);
                    }
                    else
                    {
                        lazers.Add(hold);
                    }
                }
            }
            
            //update mushrooms
            Boolean added = false;
            for (int i = 0; i < 25; i++)
            {
                
                if (i < mushrooms.Count - 1 && mushrooms[i].mushroom == mushrooms[i + 1].mushroom)
                    mushrooms[i].randShroom(rand);
                else if (i >= mushrooms.Count && i < 40 && gameTime.TotalGameTime.TotalMilliseconds % 5000 < 1 && !added)
                {
                    mushrooms.Add(new Mushroom(Content, this, rand));
                    added = true;
                }
            }

            //update spider
            if (!spiderOn && gameTime.TotalGameTime.TotalMilliseconds % 5000 < 1)
            {
                spider = new Spider(graphics, centipedeSpriteSheet);
                spiderOn = true;
            }
            else if (spiderOn && gameTime.TotalGameTime.TotalMilliseconds % 25000 < 1)
            {
                spider = null;
                spiderOn = false;
            }

            if (spiderOn)
            spider.Update(graphics, gameTime, mushrooms);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);
            
            //draw lazers
            foreach (Rectangle missile in lazers)
            {
                spriteBatch.Draw(centipedeSpriteSheet, missile, shotMissleRect, Color.White);
            }

            //draw player
            player.Draw(gameTime, spriteBatch);

            //draw centipede
            c1.Draw(spriteBatch);
            
            //draw mushrooms
            for (int i = 0; i < mushrooms.Count; i++)
            {
                spriteBatch.Draw(mushrooms[i].spriteSheet, mushrooms[i].getPosition(), mushrooms[i].s_mushroom, Color.White);
            }

            //draw spider
            if(spiderOn)
            spriteBatch.Draw(centipedeSpriteSheet, spider.getPos(), spider.getSpiderTexture(), Color.White);
            
            spriteBatch.End();

            base.Draw(gameTime);
        }
        
        //Add method to check if spider has been hit, or player has been hit, this'll probably go in the generic Enemy class Rizvee is working on
    }
}
