using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace WumpusTest
{
    class Arrow : Sprite
    {
        public Vector2 Velocity;

        public Arrow(Vector2 newVel, Vector2 newPos, string newAsset)
            : base(newPos, newAsset)
        {
            Velocity = newVel;                
        }

        public void Fire()
        {
            Position += Velocity;
        }

        public void Draw(SpriteBatch theSpriteBatch, ContentManager theContentManager)
        {
            if (Texture == null)
            {
                LoadContent(theContentManager);
            }

            double thisTheta;

            thisTheta = Math.Atan(Velocity.Y / Velocity.X);
            if (Velocity.X < 0)
            {
                thisTheta += Math.PI;
            }
            thisTheta += Math.PI;
            thisTheta = (thisTheta > 2 * Math.PI) ? thisTheta - 2 * Math.PI : thisTheta;

            theSpriteBatch.Draw(Texture, Position,
                 new Rectangle(0, 0, Texture.Width, Texture.Height), Color.White,
                 (float)thisTheta, Vector2.Zero, 1.0f, SpriteEffects.None, 0);
        }
    }
}
