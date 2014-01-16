using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WumpusTest
{
    class Hud : Sprite
    {
        public Sprite GoldIcon, ArrowIcon;

        public Hud(Vector2 newPos, string newAsset)
            : base(newPos, newAsset)
        {
            GoldIcon = new Sprite(30, new Vector2(newPos.X + 460, newPos.Y + 10), "Coins\\verticalCoin");
            ArrowIcon = new Sprite(50, new Vector2(newPos.X + 460, newPos.Y + 50), "Arrows\\ArrowCounter");
        }

        public void DrawHUD(SpriteBatch theSpriteBatch, ContentManager theContentManager)
        {
            this.Draw(theSpriteBatch, theContentManager);
            GoldIcon.Draw(theSpriteBatch, theContentManager);
            ArrowIcon.Draw(theSpriteBatch, theContentManager);
        }

        public void LoadContentHUD(ContentManager theContentManager)
        {
            this.LoadContent(theContentManager);
            GoldIcon.LoadContent(theContentManager);
            ArrowIcon.LoadContent(theContentManager);
        }
    }
}
