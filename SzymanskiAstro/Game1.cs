using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;

namespace SzymanskiAstro
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private Rakieta gracz;
        private Meteor wrog, wrog2;
        private Texture2D control, niebo, animRakiety, meteor, pocisk, gameOver;
        private Vector2 pozDotyku;

        //Animacja klatek Rakieta:
        private int nrKlatki_Rakieta;
        private int czasOdOstatniejKlatki_Rakieta = 0;
        private int msNaKlatke_Rakieta = 50;

        //Animacja klatek Meteor:
        private int nrKlatki_Meteor, ileCykli;
        private int czasOdOstatniejKlatki_Meteor = 0;
        private int msNaKlatke_Meteor = 500;
        
        private Song wybuchRaz;
        private int punkty;
        SpriteFont font;
        private bool stanGry;
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 480;
            graphics.PreferredBackBufferHeight = 800;
            //graphics.IsFullScreen = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            stanGry = true;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {            
            spriteBatch = new SpriteBatch(GraphicsDevice);            

            niebo = Content.Load<Texture2D>("niebo");            
            control = Content.Load<Texture2D>("control");
            animRakiety = Content.Load<Texture2D>("AnimRakiety");
            meteor = Content.Load<Texture2D>("meteor");
            pocisk = Content.Load<Texture2D>("pocisk2D");
            gameOver = Content.Load<Texture2D>("gameOver"); 

            wybuchRaz = Content.Load<Song>("boom");

            font = Content.Load<SpriteFont>("czcionka");                       

            nrKlatki_Rakieta = 0;
            nrKlatki_Meteor = 0;
            ileCykli = 0;
            punkty = 0;

            gracz = new Rakieta(animRakiety, pocisk);     
            wrog = new Meteor(meteor);
            wrog2 = new Meteor(meteor);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
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
            TouchCollection mscaDotknięte = new TouchCollection();
            mscaDotknięte = TouchPanel.GetState();            

            if (stanGry)
            {
                //Sterowanie dotykowe
                foreach (TouchLocation dotyk in mscaDotknięte)
                {
                    pozDotyku = dotyk.Position;
                    //Równanie koła (x-a)^2 + (y-b)^2 <= r^2
                    if (dotyk.State == TouchLocationState.Moved)
                    {
                        if (((pozDotyku.X - 60) * (pozDotyku.X - 60)) + ((pozDotyku.Y - 690) * (pozDotyku.Y - 690)) <= 160)
                        {
                            gracz.MoveL();
                        }
                        if (((pozDotyku.X - 160) * (pozDotyku.X - 160)) + ((pozDotyku.Y - 690) * (pozDotyku.Y - 690)) <= 160)
                        {
                            gracz.MoveR();
                        }
                        if (((pozDotyku.X - 110) * (pozDotyku.X - 110)) + ((pozDotyku.Y - 645) * (pozDotyku.Y - 645)) <= 160)
                        {
                            gracz.MoveU();
                        }
                        if (((pozDotyku.X - 110) * (pozDotyku.X - 110)) + ((pozDotyku.Y - 740) * (pozDotyku.Y - 740)) <= 160)
                        {
                            gracz.MoveD();
                        }
                    }
                    else if (dotyk.State == TouchLocationState.Pressed)
                    {
                        if (((pozDotyku.X - 375) * (pozDotyku.X - 375)) + ((pozDotyku.Y - 695) * (pozDotyku.Y - 695)) <= 160)
                        {
                            gracz.Wystrzel();
                            if (!stanGry)
                            {
                                stanGry = true;
                            }
                        }
                    }
                }
                
                //Sterowanie klawiaturą
                if (Keyboard.GetState().IsKeyDown(Keys.S) || Keyboard.GetState().IsKeyDown(Keys.Down))
                {
                    gracz.MoveD();
                }
                if (Keyboard.GetState().IsKeyDown(Keys.W) || Keyboard.GetState().IsKeyDown(Keys.Up))
                {
                    gracz.MoveU();
                }
                if (Keyboard.GetState().IsKeyDown(Keys.A) || Keyboard.GetState().IsKeyDown(Keys.Left))
                {
                    gracz.MoveL();
                }
                if (Keyboard.GetState().IsKeyDown(Keys.D) || Keyboard.GetState().IsKeyDown(Keys.Right))
                {
                    gracz.MoveR();
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    gracz.Wystrzel();
                }

                wrog.Update();
                wrog2.Update();
                DetekcjaKolizji();
                gracz.LotPocisku();
            }
            else
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    stanGry = true;
                    punkty = 0;                    
                }
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
            
            spriteBatch.Begin();

            //Rysowanie nieba
            spriteBatch.Draw(niebo, new Vector2(0, 0), Color.White);     

            if (stanGry)
            {
                //Rysowanie rakiety-----------------------------------------------------------------------------------------------------      
                int szerokoscRakieta = animRakiety.Width / 6;
                czasOdOstatniejKlatki_Rakieta += gameTime.ElapsedGameTime.Milliseconds;
                if (czasOdOstatniejKlatki_Rakieta > msNaKlatke_Rakieta)
                {
                    czasOdOstatniejKlatki_Rakieta -= msNaKlatke_Rakieta;

                    //inkrementacja obecnej klatki
                    nrKlatki_Rakieta++;
                    czasOdOstatniejKlatki_Rakieta = 0;
                    if (nrKlatki_Rakieta > 5)
                    {
                        nrKlatki_Rakieta = 0;
                    }
                }
                Rectangle klatkaRakieta = new Rectangle(nrKlatki_Rakieta * szerokoscRakieta, 0, szerokoscRakieta, animRakiety.Height);
                gracz.Draw(animRakiety, pocisk, spriteBatch, gracz, klatkaRakieta);
                //---------------------------------------------------------------------------------------------------------------------

                //Rysowanie meteorytow-----------------------------------------------------------------------------------------
                int szerokoscMeteor = meteor.Width / 3;
                czasOdOstatniejKlatki_Meteor += gameTime.ElapsedGameTime.Milliseconds;
                if (czasOdOstatniejKlatki_Meteor > msNaKlatke_Meteor)
                {
                    czasOdOstatniejKlatki_Meteor -= msNaKlatke_Meteor;

                    //inkrementacja obecnej klatki
                    nrKlatki_Meteor++;
                    czasOdOstatniejKlatki_Meteor = 0;
                    if (nrKlatki_Meteor > 2)
                    {
                        nrKlatki_Meteor = 0;
                    }
                }
                Rectangle klatkaMeteor = new Rectangle(nrKlatki_Meteor * szerokoscMeteor, 0, szerokoscMeteor, meteor.Height);
                wrog.Draw(meteor, spriteBatch, wrog, klatkaMeteor);
                wrog2.Draw(meteor, spriteBatch, wrog2, klatkaMeteor);
                //--------------------------------------------------------------------------------------------------------------

                //Zliczanie i inkrementacja cykli meteorów
                ileCykli++;
                if (ileCykli >= 10)
                {
                    ileCykli = 0;
                }
                //------------------------------------------
            }
            else
            {
                spriteBatch.Draw(gameOver, new Vector2(0, 200), Color.White);
            }

            //Rysowanie kontrolera
            spriteBatch.Draw(control, new Vector2(0, 583), Color.White);

            //Rysowanie wyniku
            spriteBatch.DrawString(font, "Wynik: " + punkty, new Vector2(200, 770), Color.White);

            spriteBatch.End();        

            base.Draw(gameTime);
        }

        /// <summary>
        /// Uruchamia się, gdy dochodzi do wszelakich kolizji.
        /// </summary>
        public void DetekcjaKolizji()
        {
            Rectangle rectGracza = new Rectangle((int)gracz.GetPosition().X, (int)gracz.GetPosition().Y, (int)gracz.GetSize().X, (int)gracz.GetSize().Y);
            Rectangle rectWrog = new Rectangle((int)wrog.GetPosition().X, (int)wrog.GetPosition().Y, (int)wrog.GetSize().X, (int)wrog.GetSize().Y);
            Rectangle rectWrog2 = new Rectangle((int)wrog2.GetPosition().X, (int)wrog2.GetPosition().Y, (int)wrog2.GetSize().X, (int)wrog2.GetSize().Y);
            Rectangle rectPocisk = new Rectangle((int)gracz.GetPositionPocisk().X, (int)gracz.GetPositionPocisk().Y, 
                                                 (int)gracz.GetSizePocisk().X, (int)gracz.GetSizePocisk().Y);

            //Kolizja rakiety z meteorem
            if (rectGracza.Intersects(rectWrog))
            {
                wrog.Kolizja();
                gracz.Kolizja();
                MediaPlayer.Play(wybuchRaz);
                stanGry = false;
            }
            if (rectGracza.Intersects(rectWrog2))
            {
                wrog2.Kolizja();
                gracz.Kolizja();
                MediaPlayer.Play(wybuchRaz);
                stanGry = false;
            }

            //Kolizja pocisku z meteorem
            if (rectPocisk.Intersects(rectWrog))
            {
                wrog.Kolizja();
                gracz.Trafienie();
                punkty++;
            }
            if (rectPocisk.Intersects(rectWrog2))
            {
                wrog2.Kolizja();
                gracz.Trafienie();
                punkty++;
            }
        }
    }
}
