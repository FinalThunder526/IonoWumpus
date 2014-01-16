using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WumpusTest
{
    class Room : Sprite
    {
        public int RoomNumber;
        public int ChamberNumber;

        public static Rectangle RoomSize = new Rectangle(120, 30, 660, 400);
        public static Rectangle ChamberSize = new Rectangle(100, 20, 700, 420);
        public Rectangle thisSize;

        public static Vector2 VerticalDoorSize = new Vector2(45, 150);
        public static Vector2 HorizontalDoorSize = new Vector2(150, 45);

        Sprite pit, bats;

        public int caveType = 1;

        // Change to 9:
        public Sprite[] Doors = new Sprite[5];
        
        // CONSTRUCTORS
        /// <summary>
        /// First constructor. Initializes doors, default asset name, and default position.
        /// </summary>
        /// <param name="Cham">Chamber number</param>
        /// <param name="Room">Room number</param>
        public Room(int Cham, int Room)
        {
            setRoomFromRegular(Cham, Room); 
            Initialize();
            
        }
        /// <summary>
        /// Second constructor. Includes parameter for Pos.
        /// </summary>
        /// <param name="newPosition">New position</param>
        /// <param name="Cham">Chamber number</param>
        /// <param name="Room">Room number</param>
        
        /// <summary>
        /// Third constructor.
        /// </summary>
        /// <param name="location">The decimal form of the room.</param>
        public Room(double location)
        {
            setRoomFromDecimal(location);
            Initialize();
        }
        /// <summary>
        /// Fourth constructor.
        /// </summary>
        /// <param name="location">Svetlana's integer style room.</param>
        public Room(int location)
        {
            setRoomFromInteger(location);
            Initialize();
        }

        private void setAssetName()
        {
            if (RoomNumber == 0)
                this.AssetName = "Rooms\\Chamber1";
            else
                this.AssetName = "Rooms\\Room1";
        }

        private void setRoomPosition()
        {
            this.Position = new Vector2(thisSize.Left, thisSize.Top);
        }

        /// <summary>
        /// Initializes the doors of the room, only for Cave 1.
        /// Based on the room number, the doors are initialized in the right places.
        /// </summary>
        private void Initialize()
        {
            thisSize = (RoomNumber == 0) ? ChamberSize : RoomSize;
            pit = new Sprite(new Vector2(thisSize.Center.X - 200, thisSize.Center.Y - 200), "ObstacleEvents\\Pit2");
            bats = new Sprite(new Vector2(thisSize.Center.X - 20, thisSize.Center.Y - 20), "ObstacleEvents\\Bat");
            
            Doors[0] = null;
            // The doors are placed differently if it's a chamber or if it's a room.
            //if(RoomNumber == 0)
            //{
            Doors[1] = (new Sprite(new Vector2(thisSize.Center.X - HorizontalDoorSize.X / 2, thisSize.Top), "Doors\\Door3Horizontal"));
            Doors[2] = (new Sprite(new Vector2(thisSize.Right - VerticalDoorSize.X, thisSize.Center.Y - VerticalDoorSize.Y/2), "Doors\\Door3Vertical"));
            Doors[3] = (new Sprite(new Vector2(thisSize.Center.X - HorizontalDoorSize.X / 2, thisSize.Bottom - HorizontalDoorSize.Y), "Doors\\Door3Horizontal"));
            Doors[4] = (new Sprite(new Vector2(thisSize.Left, thisSize.Center.Y - VerticalDoorSize.Y / 2), "Doors\\Door3Vertical"));
            
            if (RoomNumber == 0)
            { }
            else if (RoomNumber == 1 || RoomNumber == 3)
            {
                Doors[2] = null;
                Doors[4] = null;
            }
            else if (RoomNumber == 2 || RoomNumber == 4)
            {
                Doors[1] = null;
                Doors[3] = null;
            }

            setRoomPosition();
            setAssetName();
            
        }
        
        /// <summary>
        /// Older style of initializing doors, based on the adjacent rooms.
        /// Not good, because this is dependent on the cave data: there is no connection between Room and Cave.
        /// </summary>
        /// <param name="AdjacentRooms">The array of adjacent rooms.</param>
        /*private void InitializeDoors1(Room[] AdjacentRooms)
        {
            Doors.Add(null);
            //North
            if (AdjacentRooms[1] != null)
                Doors.Add(new Sprite(new Vector2(375, 30), "Doors\\Door3Horizontal"));
            //East
            if (AdjacentRooms[2] != null)
                Doors.Add(new Sprite(new Vector2(735, 155), "Doors\\Door3Vertical"));
            //South
            if (AdjacentRooms[3] != null)
                Doors.Add(new Sprite(new Vector2(375, 385), "Doors\\Door3Horizontal"));
            //West
            if (AdjacentRooms[4] != null)
                Doors.Add(new Sprite(new Vector2(120, 155), "Doors\\Door3Vertical"));
        }
        */
        
        
        // ACCESSORS
        /// <summary>
        /// Decimal form of the room.
        /// </summary>
        /// <returns>The decimal form of the room.</returns>
        public double getDecimalForm()
        {
            return ChamberNumber + (double)(RoomNumber)/10.0;
        }
        /// <summary>
        /// Svetlana's integer form.
        /// </summary>
        /// <returns>The integer translation.</returns>
        public int getIntegerForm()
        {
            int n = 0;
            if (caveType != 3)
            {
                // -1 taken out:
                //n = (ChamberNumber - 1) * 5 + RoomNumber;
                // Numbering starts at 5 = 1-0 = 1.0
                n = (ChamberNumber) * 5 + RoomNumber;
            }
            return n;
                /*
            else
            {
                int integerRoomNumber = 0;
                integerRoomNumber = (ChamberNumber - 1) * 4 + RoomNumber;
                 
                if (ChamberNumber == 7) 
                {
                    integerRoomNumber += 
                }
            }
                */
        }


        
        // MUTATORS
        /// <summary>
        /// Sets room.
        /// </summary>
        /// <param name="location">Of the form C.R</param>
        void setRoomFromDecimal(double location)
        {
            ChamberNumber = (int)location;
            RoomNumber = (int)(((location - ChamberNumber) * 10.0) + 0.5);
        }
        /// <summary>
        /// Sets room
        /// </summary>
        /// <param name="chamber">Chamber number</param>
        /// <param name="room">Room number</param>
        void setRoomFromRegular(int chamber, int room)
        {
            ChamberNumber = chamber;
            RoomNumber = room;
        }
        void setRoomFromInteger(int n)
        {
            // Changed from 4 to 5:
            // I think we messed up. It should be divided and modulus-ed by 5: this is what I changed:
            ChamberNumber = n / 5;
            RoomNumber = n % 5;
        }
        //public void setIsPitted(bool b)
        //{
        //    isPitted = b;
        //}
        
        public override string ToString()
        {
            return ChamberNumber + "-" + RoomNumber;
        }
        public void LoadContentRoom(ContentManager theContentManager)
        {
            LoadContent(theContentManager);
            foreach (Sprite door in Doors)
            {
                if(door != null)
                    door.LoadContent(theContentManager);
            }
        }
        public void DrawRoom(SpriteBatch theSpriteBatch, ContentManager theContentManager, bool isPitted, bool isBatted)
        {
            this.Draw(theSpriteBatch, theContentManager);
            foreach (Sprite door in Doors)
            {
                if(door != null)
                    door.Draw(theSpriteBatch, theContentManager);
            }
            if (isPitted)
            {
                pit.Draw(theSpriteBatch, theContentManager);
            }
            
        }
        public bool Equals(Room other)
        {
            return other.ChamberNumber == this.ChamberNumber && other.RoomNumber == this.RoomNumber;
        }
    }
}
