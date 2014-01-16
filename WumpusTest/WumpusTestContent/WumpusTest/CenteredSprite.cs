using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace WumpusTest
{
    class CenteredSprite : Sprite
    {
        public const int height = 45;

        public CenteredSprite(Vector2 Position, string theAssetName)
        {
            this.Position = Position;
            this.AssetName = theAssetName;
            SetOrigin();
        }
        public CenteredSprite()
        {
            this.Position = new Vector2(0, 0);
            this.AssetName = "";
        }

        
        public void Draw(SpriteBatch theSpriteBatch, ContentManager theContentManager)
        {
            if (Texture == null)
            {
                LoadContent(theContentManager);
            }

            theSpriteBatch.Draw(Texture, Position,
                 new Rectangle(0, 0, Texture.Width, Texture.Height), Color.White,
                 theta, new Vector2(height/2, height/2), 1.0f, SpriteEffects.None, 0);
        }
    }
}
