using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WumpusTest
{
    class Map : Sprite
    {
        public Random r;

        private bool won = false;
        public enum roomType { pit, bat, wumpus, none };
        private roomType PlayerInRoomWith;

        // represent room numbers
        private int[] pitRooms;
        private int[] batRooms;
        private int wumpusRoomInt;
        private Room wumpusRoom;
        
        // based on cave type
        private int numberOfRooms;
        
        private List<Room> explored = new List<Room>();

        private Room playerRoom;// = new Room(4, 0);
        private Room[] currentAdjacentRooms;

        Player p;
        public int moves = 0;

        public Map(int totalNumberOfRooms)
        {
            p = new Player();
            r = new Random();
            pitRooms = new int[10];
            batRooms = new int[10];
            setNumberOfRooms(totalNumberOfRooms);
        }

        /* 
         * keeps track of where hazards, Wumpus, player are
         * give warnings
         * shooting arrows
         * obtain secrets to help player
         */

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

        public Room getPlayerRoom()
        {
            return playerRoom;
        }

        public int getWumpusRoom()
        {
            return wumpusRoomInt;
        }

        public bool getWon()
        {
            return won;
        }

        public int getNumberOfRooms()
        {
            return numberOfRooms;
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

        public void setPlayerRoom(Room newPlayerRoom)
        {
            playerRoom = newPlayerRoom;
            explored.Add(playerRoom);
        }

        public void setAdjacentRooms(Room[] newRooms)
        {
            this.currentAdjacentRooms = newRooms;
        }

        public void setWumpusRoom(int theWumpusRoom)
        {
            this.wumpusRoomInt = theWumpusRoom;
            this.wumpusRoom = new Room(theWumpusRoom);
        }

        public void setNumberOfRooms(int numberOfRooms)
        {
            this.numberOfRooms = numberOfRooms;
        }

        
        // randomize locations of various rooms/characters
        // player and wumpus randomization independent
        public void randomizePitAndBatRooms()
        {
            pitRooms[0] = getRandomRoom();

            do
            {
                pitRooms[1] = getRandomRoom();
            } while (pitRooms[1] == pitRooms[0]);

            do
            {
                batRooms[0] = getRandomRoom();
            } while (pitRooms.Contains(batRooms[0]));

            do
            {
                batRooms[1] = getRandomRoom();
            } while (pitRooms.Contains(batRooms[1]) || (batRooms[1] == batRooms[0]));
        }

        public bool randomizeRoom(int temp, Room[] adjacentRooms, char p)
        {
            return true;
        }

        // for player: use for bats and for beginning
        // for wumpus: use for teleport and beginning
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

        

        public bool isRoomNotOkay(Room MyRoom, char p)
        {
            if (MyRoom == null)
                return true;
            if (p == 'p')
                return (MyRoom.getIntegerForm() == wumpusRoomInt) || (pitRooms.Contains(MyRoom.getIntegerForm())) || (batRooms.Contains(MyRoom.getIntegerForm()));
            else if (p == 'w')
                return (MyRoom.getIntegerForm() == playerRoom.getIntegerForm()) || pitRooms.Contains(MyRoom.getIntegerForm()) || batRooms.Contains(MyRoom.getIntegerForm());
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
        public void updateExploredAndGold(int roomNumber)
        {
            int gold = r.Next(2) + 3;
            Room rm = new Room(roomNumber);
            explored.Add(rm);
                        
            if (!explored.Contains(rm))
            {
                p.setGold(p.getGold() + gold);
            }
        }

        public bool ShootArrow(int roomNumber)
        {
            // check if have arrow
            // check if can shoot arrow in that room (to test for errors, basically)
            // updates "won" variable if roomNumber == wumpus;
            if (p.getArrows() < 1)
            {
                // print error
            }
            else if (roomNumber == wumpusRoomInt)
            {
                p.setArrows(p.getArrows() - 1);
                won = true;
            }
            else
            {
                p.setArrows(p.getArrows() - 1);
                // print message & access trivia
            }
            return won;
        }

        public string GiveWarnings(Room[] adjacentRooms)
        {
            // if player is adjacent to hazard or wumpus, returns the proper warning
            bool air = false, growl = false, wings = false;
            String str = "";

            // this group of if loops isnt integrated with the next group so that the warnings are given in order
            // if they were added differently, the player would get excessive hints about the location of the obstacles
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

        /*
        public bool ObtainSecret()
        {
            // unsure about this method, but perhaps returns true if player should get a secret this move
            return false;
        }
        */

        public void addToExplored(Room myR)
        {
            explored.Add(myR);
        }
    }
}
