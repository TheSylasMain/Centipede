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
        Texture2D spriteSheet;
        Boolean spiderOn;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferHeight = 1000;
            graphics.PreferredBackBufferWidth = 800;

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            mushrooms = new List<Mushroom>();
            for (int i = 0; i < 40; i++)
            {
                mushrooms.Add(new Mushroom(Content, this, rand));
            }

            

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

            // TODO: use this.Content to load your game content here
            centipedeSpriteSheet = Content.Load<Texture2D>("Arcade - Centipede - General Sprites");
            spriteSheet = Content.Load<Texture2D>("spiderTrans");
            //spider = new Spider(graphics, spriteSheet);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
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

            // TODO: Add your update logic here
            Boolean added = false;
            for (int i = 0; i < 39; i++)
            {
                
                if (i < mushrooms.Count - 1 && mushrooms[i].mushroom == mushrooms[i + 1].mushroom)
                    mushrooms[i].randShroom(rand);
                else if (i >= mushrooms.Count && i < 40 && gameTime.TotalGameTime.TotalMilliseconds % 5000 < 1 && !added)
                {
                    mushrooms.Add(new Mushroom(Content, this, rand));
                    added = true;
                }


            }

            if (!spiderOn && gameTime.TotalGameTime.TotalMilliseconds % 1000 < 1)
            {
                spider = new Spider(graphics, spriteSheet);
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
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            for (int i = 0; i < mushrooms.Count; i++)
            {
                
                spriteBatch.Draw(mushrooms[i].spriteSheet, mushrooms[i].mushroom, mushrooms[i].s_mushroom, Color.White);
            }

            if(spiderOn)
            spriteBatch.Draw(spriteSheet, spider.getPos(), spider.getSpiderTexture(), Color.White);

            spriteBatch.End();
            base.Draw(gameTime);
        }



        /**Add method to check if spider has been hit, or player has been hit, this'll probably go in the generic Enemy class Rizvee is working on*/
    }
}
