using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SzymanskiAstro
{
    class Meteor //Moje meteory latają tylko pionowo od góry do dołu, wtedy ta gra ma dla mnie większy sens, 
                 //ponieważ meteory lecące na bok prawie nigdy nas nie trafią. Jeśli to problem mogę zrobić, żeby latały również na boki. 
                 //Zależne jest to od zmieniania pozycji X a nie tylko Y.
    {
        private Texture2D texture;
        private Vector2 position;
        Random generujLL = new Random();
        private Vector2 predkosc;

        public Meteor(Texture2D texture2D)
        {
            position = new Vector2(generujLL.Next(0, 370), 0);  //1 pozycja meteoru
            predkosc = new Vector2(0, generujLL.Next(2, 5));   //ten Y jest uzywany tylko raz na poczatku            
            texture = texture2D;
        }

        public Vector2 GetPosition() { return position; }

        public Vector2 GetSize() { return new Vector2(texture.Width / 3, texture.Height); }

        /// <summary>
        /// Odpowiada za predkosc Meteoru i resetowanie go.
        /// </summary>
        public void Update()          
        {            
            position.Y += predkosc.Y;
            if (position.Y > 580)
            {
                Startuj(); 
            }
        } 

        /// <summary>
        /// Uruchamia się, gdy meteor zderzy się z rakietą.
        /// </summary>
        public void Kolizja() { Startuj(); }

        /// <summary>
        /// Uruchamia się, gdy meteor wyjdzie poza ekran lub zostanie trafiony.
        /// </summary>
        public void Startuj()
        {
            position = new Vector2(generujLL.Next(0, 370), 0);  //nowa pozycja meteoru
            predkosc = new Vector2(0, generujLL.Next(2, 6)); //Y to predkosc, ktorą dostaje nowo utworzony meteor
        }

        public void Draw(Texture2D texture2D, SpriteBatch spriteBatch, Meteor meteor, Rectangle rectangle)
        {
            Rectangle rectMeteor = new Rectangle((int)meteor.GetPosition().X, (int)meteor.GetPosition().Y, rectangle.Width, rectangle.Height);
            spriteBatch.Draw(texture2D, rectMeteor, rectangle, Color.White);
        }
    }
}
