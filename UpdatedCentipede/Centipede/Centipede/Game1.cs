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

        SpriteFont font1;
        Texture2D spriteSheetText;
        Rectangle fullrect, stillMissleRect, shootingMissleRect, shotMissleRect, destrect, destrect2;
        bool shot = false;
        List<Rectangle> lazers = new List<Rectangle>();

        KeyboardState key, keyi;

        Player player;

        Boolean endGame;

        Centipede c1;

        Vector2 text;

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
            IsMouseVisible = true;
            mushrooms = new List<Mushroom>();
            for (int i = 0; i < 25; i++)
            {
                mushrooms.Add(new Mushroom(Content, this, rand));
            }

            keyi = Keyboard.GetState();
            fullrect = new Rectangle(0, 0, 207, 105);
            shootingMissleRect = new Rectangle(0, 3, 100, 100);
            stillMissleRect = new Rectangle(104, 52, 100, 50);
            shotMissleRect = new Rectangle(105, 0, 100, 48);
            destrect = new Rectangle(100, 400, 100, 100);
            destrect2 = new Rectangle(100, 300, 100, 100);

            endGame = false;

            text = new Vector2(100, 300);

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
            spriteSheetText = this.Content.Load<Texture2D>("full");
            player = new Player(GraphicsDevice.Viewport.Width/2, GraphicsDevice.Viewport.Height / 2, centipedeSpriteSheet, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            c1 = new Centipede(centipedeSpriteSheet, 3, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, mushrooms);
            font1 = Content.Load<SpriteFont>("SpriteFont1");
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
            if (!endGame)
            {
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

                if (!spiderOn && gameTime.TotalGameTime.TotalMilliseconds % 5000 < 1)
                {
                    spider = new Spider(graphics, spriteSheet);
                    spiderOn = true;
                }
                else if (spiderOn)
                {
                    if (player.Rect.Intersects(spider.getPos()))
                    {
                        endGame = true;
                    }

                    for (int i = 0; i < lazers.Count(); i++)
                    {
                        if (lazers.ElementAt(i).Intersects(spider.getPos()))
                        {
                            spider = null;
                            spiderOn = false;                            
                            break;
                        }
                    }
          
                }


                if (spiderOn)
                    spider.Update(graphics, gameTime, mushrooms);

                keyi = key;
                key = Keyboard.GetState();

                player.Update(gameTime, key, keyi);
                if (key.IsKeyDown(Keys.Space) && keyi.IsKeyDown(Keys.Space))
                {
                    Missile newMissile = new Missile();

                    newMissile.build(player.X + 15, player.Y - 16);

                    lazers.Add(newMissile.getNewMissle());
                }
                if (lazers.Count > 0)
                {
                    for (int i = 0; i < lazers.Count; i++)
                    {

                        Rectangle hold = lazers.ElementAt(i);
                        hold.Y -= 3;
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

                c1.Move();
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            if (!endGame)
            {
                for (int i = 0; i < mushrooms.Count; i++)
                {

                    spriteBatch.Draw(mushrooms[i].spriteSheet, mushrooms[i].mushroom, mushrooms[i].s_mushroom, Color.White);
                }

                if (spiderOn)
                    spriteBatch.Draw(spriteSheet, spider.getPos(), spider.getSpiderTexture(), Color.White);

                spriteBatch.End();

                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);
                player.Draw(gameTime, spriteBatch);


                foreach (Rectangle missile in lazers)
                {
                    spriteBatch.Draw(spriteSheetText, missile, shotMissleRect, Color.White);
                }

                c1.Draw(spriteBatch);
            }
            else
            {
                spriteBatch.DrawString(font1, "Game Over! Press R to restart!", text, Color.White);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }



        /**Add method to check if spider has been hit, or player has been hit, this'll probably go in the generic Enemy class Rizvee is working on*/
    }
}
