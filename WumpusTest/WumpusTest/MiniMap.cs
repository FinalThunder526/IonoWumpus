using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WumpusTest
{
    class MiniMap : Sprite
    {
        public List<MiniMapBit> ToShow = new List<MiniMapBit>();
        public Sprite smallPlayer;

        public MiniMap(Vector2 newPos, string newAsset)
            : base(newPos, newAsset)
        {
             smallPlayer = new Sprite(newPos + new Vector2(10, 10), "MiniMapFiles\\Player");
        }

        public void AddNew(Room NewRoom)
        {
            ToShow.Add(new MiniMapBit(this.Position, NewRoom));
        }

        public void Draw(SpriteBatch theSpriteBatch, ContentManager theContentManager)
        {
            foreach (MiniMapBit mmb in ToShow)
            {
                mmb.Draw(theSpriteBatch, theContentManager);
            }
            smallPlayer.Draw(theSpriteBatch, theContentManager);
        }
    }
}
