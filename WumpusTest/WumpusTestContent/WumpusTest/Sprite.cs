using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace WumpusTest
{
    class Sprite
    {
        public Vector2 Position;
        public string AssetName;
        public Texture2D Texture;
        public static int stdHeight = 40;
        public int height = 40;
        // radians
        public float theta = 0;
        public Vector2 Origin;
        public int width;

        public Sprite(Vector2 Position, string theAssetName)
        {
            this.Position = Position;
            this.AssetName = theAssetName;
            SetOrigin();
        }
        public Sprite(int newHeight, Vector2 newPos, string newAsset)
        {
            height = newHeight;
            this.Position = newPos;
            this.AssetName = newAsset;
            SetOrigin();
        }
        public Sprite()
        {
            this.Position = new Vector2(0, 0);
            this.AssetName = "";
        }

        public void SetOrigin()
        {
            this.Origin = new Vector2(Position.X + height / 2, Position.Y + height / 2);
        }
        public void SetPosition()
        {
            this.Position = new Vector2(Origin.X - height / 2, Origin.Y - height / 2);
        }

        public void LoadContent(ContentManager theContentManager)
        {
            Texture = theContentManager.Load<Texture2D>(AssetName);
            height = Texture.Height;
            width = Texture.Width;
        }
        public bool isOver(Vector2 OtherPosition)
        {
            if (OtherPosition.X >= Position.X && OtherPosition.X <= (Position.X + width))
            {
                if (OtherPosition.Y >= Position.Y && OtherPosition.Y <= (Position.Y + height))
                {
                    return true;
                }
            }
            return false;
        }
        public void Draw(SpriteBatch theSpriteBatch, ContentManager theContentManager)
        {
            if (Texture == null)
            {
                LoadContent(theContentManager);
            }

            theSpriteBatch.Draw(Texture, Position,
                 new Rectangle(0, 0, Texture.Width, Texture.Height), Color.White,
                 theta, Vector2.Zero, 1.0f, SpriteEffects.None, 0);
        }
        
    }
}
