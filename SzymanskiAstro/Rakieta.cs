using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SzymanskiAstro
{
    class Rakieta
    {
        private Texture2D texture, pocisk2D;
        private Vector2 position;
        private Vector2 ruch; 
        private Pocisk strzal;        

        public Rakieta(Texture2D texture2D, Texture2D texture2D_2)
        {
            position = new Vector2(210, 480);
            ruch = new Vector2(5, 5);   //Rakieta miala zmieniac polozenie o 5
            texture = texture2D;        //textura dla Rakiety

            pocisk2D = texture2D_2;     //textura dla Pocisku
            strzal.position = position;
        }

        public Vector2 GetPosition() { return position; }

        public Vector2 GetSize() { return new Vector2(texture.Width / 6, texture.Height); }

        public Vector2 GetSizePocisk() { return new Vector2(pocisk2D.Width, pocisk2D.Height); }

        public Vector2 GetPositionPocisk() { return strzal.position; }

        private struct Pocisk
        {
            public Vector2 position;
            public bool wystrzelony;            
        }

        /// <summary>
        /// Pocisk zostaje wystrzelony z Rakiety.
        /// </summary>
        public void Wystrzel()
        {
            if (strzal.wystrzelony == false)
            {
                strzal.wystrzelony = true;  //jesli jest true to jest wystrzelony
                strzal.position = position;
            }            
        }

        /// <summary>
        /// Odpowiada za dane pocisku podczas lotu.
        /// </summary>
        public void LotPocisku()
        {
            if (strzal.wystrzelony)
            {                
                if (strzal.position.Y > 0)
                {
                    strzal.position.Y -= 10;                    
                }
                else
                {
                    strzal.wystrzelony = false;
                }
            }
        }

        /// <summary>
        /// Uruchamia się, gdy pocisk trafi meteor.
        /// </summary>
        public void Trafienie()
        {
            strzal.wystrzelony = false;
            strzal.position = new Vector2(0, 583); //nieaktywny strzal zostaje umieszczony poza mapą
        }

        /// <summary>
        /// Uruchamia się, gdy rakieta zderzy się z meteorem.
        /// </summary>
        public void Kolizja()
        {
            position = new Vector2(210, 480);  //Przywraca rakiete w miejsce startowe.
        }

        public void MoveL()
        {
            if (position.X >= 5) { position.X -= ruch.X; }
        }

        public void MoveR()
        {
            if (position.X <= 395) { position.X += ruch.X; }
        }
        public void MoveU()
        {
            if (position.Y >= 5) { position.Y -= ruch.Y; }
        }
        public void MoveD()
        {
            if (position.Y <= 475) { position.Y += ruch.Y; }
        }

        public void Draw(Texture2D texture2D, Texture2D texture2D_2, SpriteBatch spriteBatch, Rakieta rakieta, Rectangle rectangle)
        {
            Rectangle rectGracza = new Rectangle((int)rakieta.GetPosition().X, (int)rakieta.GetPosition().Y, rectangle.Width, rectangle.Height);            

            spriteBatch.Draw(texture2D, rectGracza, rectangle, Color.White);

            if (strzal.wystrzelony) { spriteBatch.Draw(texture2D_2, strzal.position, Color.White); }
        }
    }
}
