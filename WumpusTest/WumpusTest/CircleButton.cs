using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace WumpusTest
{
    class CircleButton : Sprite
    {
        public CircleButton(int newHeight, Vector2 newPos, string newAsset): base(newHeight, newPos, newAsset)
        { 
        
        }

        public bool isOver(Vector2 OtherPosition)
        {
            return Math.Sqrt(Math.Pow(OtherPosition.X - Origin.X, 2) + Math.Pow(OtherPosition.Y - Origin.Y, 2)) < (height + 0.5) / 2;
        }
    }
}
