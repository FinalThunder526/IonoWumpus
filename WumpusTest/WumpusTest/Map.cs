using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace WumpusTest
{
    class Map : Sprite
    {

        /* 
         * keeps track of where hazards, Wumpus, player are
         * give warnings
         * shooting arrows
         * obtain secrets to help player
         */

        public Random r;

        private bool won = false;
        public enum roomType { pit, bat, wumpus, none };
        private roomType PlayerInRoomWith;

        // represent room numbers
        private int[] pitRooms;
        private int[] batRooms;
        private int[] goldRooms;
        private int wumpusRoomInt;
        private Room wumpusRoom;

        public int[] GoldRooms
        {
            get
            {
                return goldRooms;
            }
            set
            {
                goldRooms = value;
            }
        }
        
        // based on cave type
        private int numberOfRooms;
        private int totalPits = 2;
        private int totalBats = 2;
        private int totalGold = 20;
        
        private List<Room> explored = new List<Room>();

        private Room playerRoom;
        private Room[] currentAdjacentRooms;

        public int moves = 0;

        public MiniMap MyMiniMap;

        public Map(int totalNumberOfRooms)
        {
            // error check
            if (totalPits < 2)
            {
                totalPits = 2;
            }

            if (totalBats < 2)
            {
                totalBats = 2;
            }
            
            r = new Random();
            pitRooms = new int[totalPits];
            batRooms = new int[totalBats];
            goldRooms = new int[totalGold];
            setNumberOfRooms(totalNumberOfRooms);

            MyMiniMap = new MiniMap(new Vector2(0, 460), "MiniMapFiles\\BG");
        }

        public Room getPlayerRoom()
        {
            return playerRoom;
        }

        public void setPlayerRoom(Room playerRoom)
        {
            this.playerRoom = playerRoom;
            addToExplored(playerRoom);
        }

        public int getWumpusRoom()
        {
            return wumpusRoomInt;
        }

        public void setWumpusRoom(int wumpusRoomInt)
        {
            this.wumpusRoomInt = wumpusRoomInt;
            wumpusRoom = new Room(wumpusRoomInt);
        }

        public int getNumberOfRooms()
        {
            return numberOfRooms;
        }

        public void setNumberOfRooms(int numberOfRooms)
        {
            this.numberOfRooms = numberOfRooms;
        }

        // accessors
        public int getPitRoomAt(int n)
        {
            return pitRooms[n];
        }

        public int getBatRoomAt(int n)
        {
            return batRooms[n];
        }

        public int[] getAllPitRooms()
        {
            return pitRooms;
        }

        public int[] getAllBatRooms()
        {
            return batRooms;
        }

        public bool getWon()
        {
            return won;
        }

        public Room[] getAdjacentRooms()
        {
            return currentAdjacentRooms;
        }

        // mutators
        public void setPitRooms(int n, int pitRoom)
        {
            pitRooms[n] = pitRoom;
        }

        public void setBatRooms(int n, int batRoom)
        {
            batRooms[n] = batRoom;
        }

         public void setAdjacentRooms(Room[] newRooms)
        {
            this.currentAdjacentRooms = newRooms;
        }


        // randomize locations of various rooms/characters
        // player and wumpus randomization independent
        public void randomizePitAndBatRooms()
        {
            // can combine the following 2 loops into 1 if pitRooms.Length == batRooms.Length for all caves
            for (int i = 0; i < pitRooms.Length; i++)
            {
                do
                {
                    pitRooms[i] = getRandomRoom();
                } while (!pitIsOkay(i));
                
            }

            for (int i = 0; i < batRooms.Length; i++)
            {
                do
                {
                    batRooms[i] = getRandomRoom();
                } while (!batIsOkay(i));

            }


        }
        public void randomizePitBatAndGoldRooms()
        {
            randomizePitAndBatRooms();
            for (int i = 0; i < goldRooms.Length; i++)
            {
                do
                {
                    goldRooms[i] = getRandomRoom();
                } while (!goldIsOkay(i));
            }
        }

        private bool goldIsOkay(int index)
        {
            for (int j = 0; j < pitRooms.Length; j++)
            {
                if (j != index)
                {
                    if (batRooms.Contains(goldRooms[index]) || pitRooms.Contains(goldRooms[index]) || (goldRooms[index] == wumpusRoomInt) || (goldRooms[index] == playerRoom.getIntegerForm()) || (goldRooms[index] == goldRooms[j]))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        // returns true if a PIT is ok to assign to the given room number
        private bool pitIsOkay(int index)
        {
            for (int j = 0; j < pitRooms.Length; j++)
            {
                if (j != index)
                {
                    if (batRooms.Contains(pitRooms[index]) || (pitRooms[index] == wumpusRoomInt) || (pitRooms[index] == playerRoom.getIntegerForm()) || (pitRooms[index] == pitRooms[j]))
                    {
                        return false;
                    }
                }    
            }
            return true;
        }

        // returns true if a BAT is ok to assign to the given room number
        private bool batIsOkay(int index)
        {
            for (int j = 0; j < batRooms.Length; j++)
            {
                if (j != index)
                {
                    if (pitRooms.Contains(batRooms[index]) || (batRooms[index] == wumpusRoomInt) || (batRooms[index] == playerRoom.getIntegerForm()) || (batRooms[index] == batRooms[j]))
                    {
                        return false;
                    }
                }     
            }
            return true;
        }

        /// <summary>
        /// for player: use for bats and for beginning
        /// for wumpus: use for teleport and beginning
        /// </summary>
        /// <param name="temp"></param>
        /// <param name="adjacentRooms"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public bool randomizeCharacterRoom(int temp, Room[] adjacentRooms, char c)
        {
            for (int i = 0; i < adjacentRooms.Length; i++)
            {
                if (adjacentRooms[i] != null)
                {
                    if (isRoomNotOkay(adjacentRooms[i], c))
                    {
                        return true; 
                    }
                }
            }
            return false;
        }

        // returns true if no room is passed or the room already has a wumpus, pit, bat, player (and therefore isn't valid), else false 
        public bool isRoomNotOkay(Room MyRoom, char p)
        {
            if (MyRoom == null)
                return true;
            if (p == 'p')
                return (MyRoom.getIntegerForm() == wumpusRoomInt) || (pitRooms.Contains(MyRoom.getIntegerForm())) || (batRooms.Contains(MyRoom.getIntegerForm()));
            else if (p == 'w')
                return (MyRoom.getIntegerForm() == playerRoom.getIntegerForm()) || pitRooms.Contains(MyRoom.getIntegerForm()) || batRooms.Contains(MyRoom.getIntegerForm()) || (MyRoom.RoomNumber == 0);
            else
                return false;
            
        }

        public int getRandomRoom()
        {
            return r.Next(numberOfRooms) + 5;
        }

        /// <summary>
        /// setting gold locations
        /// </summary>
        /// <param name="roomNumber"></param>
        public int updateExploredAndReturnGold(int roomNumber)
        {
            Room rm = new Room(roomNumber);

            // explored prob doesn't work because can't compare 2 objects using 'contains' accurately
            // will fix later, once get enough willpower to write a frustratingly redundant 'for' loop
            
            if (!explored.Contains(rm))
            {
                explored.Add(rm);
                return r.Next(3) + 3;
            }
            explored.Add(rm);
            return 0;
        }

        public string GiveWarnings(Room[] adjacentRooms)
        {
            // if player is adjacent to hazard or wumpus, returns the proper warning
            bool air = false, growl = false, wings = false;
            String str = "";

            // this group of if loops isnt integrated with the next group so that the warnings are given in order
            // if they were added differently, the player would get excessive hints about the location of the obstacles
            // WARNING FOR WUMPUS 2 ROOM RADIUS - method from cave object coming soon
            for(int i = 0; i < adjacentRooms.Length; i++) {
                if (pitRooms.Contains(adjacentRooms[i].getIntegerForm())) {
                    air = true;
                }
                if (wumpusRoomInt.Equals(adjacentRooms[i].getIntegerForm()))
                {
                    growl = true;
                }
                if (batRooms.Contains(adjacentRooms[i].getIntegerForm()))
                {
                    wings = true;
                }
            }

            // if(thewarnings are needed (true) then add them to string in order)
            if (air)
            {
                str += "You hear a whoosh of air... \n";
            }
            if (growl)
            {
                str += "You hear a low growl... \n";
            }
            if (wings)
            {
                str += "You hear wings... \n";
            }
             
            return str;
        }

        public roomType getPlayerInRoomWith(int givenRoom)
        {
            if ((givenRoom == pitRooms[0]) || (givenRoom == pitRooms[1]))
            {
                PlayerInRoomWith = roomType.pit;
            }
            else if ((givenRoom == batRooms[0]) || (givenRoom == batRooms[1]))
            {
                PlayerInRoomWith = roomType.bat;
            }
            else if (givenRoom == wumpusRoomInt)
            {
                PlayerInRoomWith = roomType.wumpus;
            }
            else
            {
                PlayerInRoomWith = roomType.none;
            }
            return PlayerInRoomWith;
        }

        public bool ShootArrow(Room roomNumber)
        {
            // returns true if won (arrow room = wumpus room), else false
            if (wumpusRoom.Equals(roomNumber))
            {
                return true;
            }    

            return false;
        }

        /// <summary>
        /// Updates the List Explored by adding the room the player has currently moved into.
        /// </summary>
        /// <param name="myR">The room the player has just moved into.</param>
        public void addToExplored(Room myR)
        {
            MyMiniMap.smallPlayer.Position = MiniMapBit.SetBitPosition(myR) + MyMiniMap.Position + new Vector2(15, 15);
            if (!explored.Contains(myR))
            {
                explored.Add(myR);
                MyMiniMap.AddNew(myR);
            }
        }
    }
}
