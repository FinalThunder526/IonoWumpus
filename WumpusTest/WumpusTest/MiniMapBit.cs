using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace WumpusTest
{
    class MiniMapBit : Sprite
    {
        public Room ThisRoom;
        public Vector2 MiniMapPos;

        public Vector2 buffer = new Vector2(10, 10);

        public MiniMapBit(Vector2 theMiniPos, Room NewRoom)
        {
            this.MiniMapPos = theMiniPos;
            this.ThisRoom = NewRoom;
            if (ThisRoom.RoomNumber == 0)
            {
                AssetName = "MiniMapFiles\\Chamber";
            }
            else
            {
                AssetName = "MiniMapFiles\\Room";
            }
            Position = SetBitPositionNonStatic(ThisRoom);
        }

        public static Vector2 SetBitPosition(Room MyRoom)
        {
            int x, y;

            x = 10 + (40 * ((MyRoom.ChamberNumber - 1) % 3));
            y = 10 + (40 * ((MyRoom.ChamberNumber - 1) / 3));

            switch (MyRoom.RoomNumber)
            {
                case 0:
                    break;
                case 1:
                    x += 5; y -= 10;
                    break;

                case 2:
                    x += 20; y += 5;
                    break;

                case 3:
                    x += 5; y += 20;
                    break;

                case 4:
                    x -= 10; y += 5;
                    break;
            }

            return new Vector2(x, y);

        }
        public Vector2 SetBitPositionNonStatic(Room MyRoom)
        {
            int x, y;

            x = 10 + (40 * ((MyRoom.ChamberNumber - 1) % 3));
            y = 10 + (40 * ((MyRoom.ChamberNumber - 1) / 3));

            switch (MyRoom.RoomNumber)
            {
                case 0:
                    break;
                case 1:
                    x += 5; y -= 10;
                    break;

                case 2:
                    x += 20; y += 5;
                    break;

                case 3:
                    x += 5; y += 20;
                    break;

                case 4:
                    x -= 10; y += 5;
                    break;
            }

            return new Vector2(MiniMapPos.X + buffer.X + x, MiniMapPos.Y + buffer.Y + y);


        }
    }
}
