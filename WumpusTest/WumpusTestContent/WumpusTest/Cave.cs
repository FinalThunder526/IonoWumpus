using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace WumpusTest
{
    interface Cave
    {
        Room[] loadCaveandRoom (double location);

        int getRoomsPerChamber();
        int getTotalChambers();
    }

    class Cave3 : Cave
    {   
        /// <summary>
        /// Class fields
        /// </summary>
        private string address;
        public int NUMB_OF_ROOMS = 9;
        int NUMB_OF_CHAMBERS = 10;
        int totalRooms;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="newAddress"> String representation of cave file address </param>
	    public Cave3(string newAddress)
	    {
            address = newAddress;
	    }

        /// <summary>
        /// Generates array of connected rooms based on current location
        /// </summary>
        /// <param name="location"> Current location represented as a decimal (chamberNumber.roomNumber) </param> 
        /// <returns></returns>
	    public Room[] loadCaveandRoom (double location)
	    {
	        String[] mapInfo = File.ReadAllLines(address);

            int chamberNumber = (int) location;
            int roomNumber = (int)(((location - chamberNumber) * 10.0) + 0.5);

            Room[] myRooms = new Room[9];

            for (int i = 0; i < myRooms.Length; i++)
            {
                myRooms[i] = null;
            }

            String temp1; String[] temp2 = new String[2];
            for (int i = 0; i < mapInfo.Length; i++)
            {
                temp1 = mapInfo[i];
                temp2 = temp1.Split(' ', '\t');
                temp2 = temp2.Where(s => !string.IsNullOrEmpty(s)).ToArray();

                if (Convert.ToDouble(temp2[0]) == location)
                    break;
            }

            if (chamberNumber < 8)
            {
                NUMB_OF_ROOMS = 4;
                for (int i = 1; i < 1 + NUMB_OF_ROOMS; i++)
                {
                    myRooms[i] = new Room(0, 0);
                }

                if (roomNumber == 0)
                {
                    for (int i = 1; i < 1 + NUMB_OF_ROOMS; i++)
                    {
                        myRooms[i] = new Room(chamberNumber, i);
                    }
                }

                else
                {
                    switch (roomNumber)
                    {
                        case 1:
                            myRooms[2] = myRooms[4] = null;
                            myRooms[3].setRoomFromDecimal(chamberNumber);
                            break;
                        case 2:
                            myRooms[1] = myRooms[3] = null;
                            myRooms[4].setRoomFromDecimal(chamberNumber);
                            break;
                        case 3:
                            myRooms[2] = myRooms[4] = null;
                            myRooms[1].setRoomFromDecimal(chamberNumber);
                            break;
                        case 4:
                            myRooms[1] = myRooms[3] = null;
                            myRooms[2].setRoomFromDecimal(chamberNumber);
                            break;
                    }

                    myRooms[roomNumber].setRoomFromDecimal(Convert.ToDouble(temp2[1]));
                }
            }

            if (chamberNumber == 8)
            {
                NUMB_OF_ROOMS = 6;
                for (int i = 1; i < 1 + NUMB_OF_ROOMS; i++)
                {
                    myRooms[i] = new Room(0, 0);
                }

                if (roomNumber == 0)
                {
                    for (int i = 1; i < 1 + NUMB_OF_ROOMS; i++)
                    {
                        myRooms[i] = new Room(chamberNumber, i);
                    }
                }

                else
                {
                    switch (roomNumber)
                    {
                        case 1:
                        case 2:
                            myRooms[1].setRoomFromDecimal(Convert.ToDouble(temp2[1]));
                            myRooms[2] = myRooms[4] = null;
                            myRooms[3].setRoomFromDecimal(chamberNumber);
                            break;
                        case 3:
                            myRooms[2].setRoomFromDecimal(Convert.ToDouble(temp2[1]));
                            myRooms[1] = myRooms[3] = null;
                            myRooms[4].setRoomFromDecimal(chamberNumber);
                            break;
                        case 4:
                        case 5:
                            myRooms[3].setRoomFromDecimal(Convert.ToDouble(temp2[1]));
                            myRooms[2] = myRooms[4] = null;
                            myRooms[1].setRoomFromDecimal(chamberNumber);
                            break;
                        case 6:
                            myRooms[4].setRoomFromDecimal(Convert.ToDouble(temp2[1]));
                            myRooms[1] = myRooms[3] = null;
                            myRooms[2].setRoomFromDecimal(chamberNumber);
                            break;
                    }
                }
            }

            if (chamberNumber == 9 || chamberNumber == 10)
            {
                NUMB_OF_ROOMS = 8;
                for (int i = 1; i < 1 + NUMB_OF_ROOMS; i++)
                {
                    myRooms[i] = new Room(0, 0);
                }

                if (roomNumber == 0)
                {
                    for (int i = 1; i < 1 + NUMB_OF_ROOMS; i++)
                    {
                        myRooms[i] = new Room(chamberNumber, i);
                    }
                }

                else
                {
                    switch (roomNumber)
                    {
                        case 1:
                        case 2:
                        case 3:
                            myRooms[1].setRoomFromDecimal(Convert.ToDouble(temp2[1]));
                            myRooms[2] = myRooms[4] = null;
                            myRooms[3].setRoomFromDecimal(chamberNumber);
                            break;
                        case 4:
                            myRooms[2].setRoomFromDecimal(Convert.ToDouble(temp2[1]));
                            myRooms[1] = myRooms[3] = null;
                            myRooms[4].setRoomFromDecimal(chamberNumber);
                            break;
                        case 5:
                        case 6:
                        case 7:
                            myRooms[3].setRoomFromDecimal(Convert.ToDouble(temp2[1]));
                            myRooms[2] = myRooms[4] = null;
                            myRooms[1].setRoomFromDecimal(chamberNumber);
                            break;
                        case 8:
                            myRooms[4].setRoomFromDecimal(Convert.ToDouble(temp2[1]));
                            myRooms[1] = myRooms[3] = null;
                            myRooms[2].setRoomFromDecimal(chamberNumber);
                            break;
                    }
                }
            }

            return myRooms;
	    }

        /// <summary>
        /// Accessors
        /// </summary>
        public int getRoomsPerChamber()
        {
            return NUMB_OF_ROOMS;
        }
        public int getTotalChambers()
        {
            return totalRooms;
        }

    }

    class Cave2 : Cave
    {
        /// <summary>
        /// Class fields
        /// </summary>
        private string address;
        const int NUMB_OF_ROOMS = 4;
        public int totalRooms = (NUMB_OF_ROOMS + 1) * 9;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="newAddress"> String representation of cave file address </param>
        public Cave2(string newAddress)
        {
            address = newAddress;
        }

        /// <summary>
        /// Generates array of connected rooms based on current location
        /// </summary>
        /// <param name="location"> Current location represented as a decimal (chamberNumber.roomNumber) </param> 
        /// <returns></returns>
        public Room[] loadCaveandRoom (double location)
        {
            String[] mapInfo = File.ReadAllLines(address);

            int chamberNumber = (int) location;
            int roomNumber = (int) ( ( (location - chamberNumber) * 10.0) + 0.5 );

	        Room[] myRooms = new Room[9];

            for (int i = 0; i < myRooms.Length; i++)
            {
                myRooms[i] = null;
            }

            for (int i = 1; i < 1 + NUMB_OF_ROOMS; i++)
            {
                myRooms[i] = new Room(0, 0);
            }

            if (roomNumber == 0)
            {
                for (int i = 1; i < 5; i++)
                {
                    myRooms[i] = new Room(chamberNumber, i);
                }
            }

            else
            {
                switch (roomNumber)
                {
                    case 1:
                        myRooms[2] = myRooms[4] = null;
                        myRooms[3].setRoomFromDecimal(chamberNumber);
                        break;
                    case 2:
                        myRooms[1] = myRooms[3] = null;
                        myRooms[4].setRoomFromDecimal(chamberNumber);
                        break;
                    case 3:
                        myRooms[2] = myRooms[4] = null;
                        myRooms[1].setRoomFromDecimal(chamberNumber);
                        break;
                    case 4:
                        myRooms[1] = myRooms[3] = null;
                        myRooms[2].setRoomFromDecimal(chamberNumber);
                        break;
                }

                string temp1 = mapInfo[(chamberNumber - 1) * 4 + (roomNumber - 1)];
                String[] temp2 = temp1.Split(' ', '\t');
                temp2 = temp2.Where(s => !string.IsNullOrEmpty(s)).ToArray();
                myRooms[roomNumber].setRoomFromDecimal(Convert.ToDouble(temp2[1]));
            }

            return myRooms;
        }

        /// <summary>
        /// Accessors
        /// </summary>
        public int getRoomsPerChamber()
        {
            return NUMB_OF_ROOMS;
        }
        public int getTotalChambers()
        {
            return totalRooms;
        }
    }

    class Cave1 : Cave
    {
        /// <summary>
        /// Class fields
        /// </summary>
        private string address;
        const int NUMB_OF_ROOMS = 4;
        const int NUMB_OF_CHAMBERS = 9;
        public int totalRooms = (NUMB_OF_ROOMS + 1) * NUMB_OF_CHAMBERS;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="newAddress"> String representation of cave file address </param>
        public Cave1 (string newAddress)
        {
            address = newAddress;
        }

        /// <summary>
        /// Generates array of connected rooms based on current location
        /// </summary>
        /// <param name="location"> Current location represented as a decimal (chamberNumber.roomNumber) </param> 
        /// <returns></returns>
        public Room[] loadCaveandRoom (double location)
        {
            String[] mapInfo = File.ReadAllLines(address);

            int chamberNumber = (int) location;
            int roomNumber = (int) ( ( (location - chamberNumber) * 10.0) + 0.5 );

	        Room[] myRooms = new Room[9];

            for (int i = 0; i < myRooms.Length; i++)
            {
                myRooms[i] = null;
            }

            for (int i = 1; i < 1 + NUMB_OF_ROOMS; i++)
            {
                myRooms[i] = new Room(0, 0);
            }

            if (roomNumber == 0)
            {
                for (int i = 1; i < 5; i++)
                {
                    myRooms[i] = new Room(chamberNumber, i);
                }
            }

            else
            {
                switch (roomNumber)
                {
                    case 1:
                        myRooms[2] = myRooms[4] = null;
                        myRooms[3].setRoomFromDecimal(chamberNumber);
                        break;
                    case 2:
                        myRooms[1] = myRooms[3] = null;
                        myRooms[4].setRoomFromDecimal(chamberNumber);
                        break;
                    case 3:
                        myRooms[2] = myRooms[4] = null;
                        myRooms[1].setRoomFromDecimal(chamberNumber);
                        break;
                    case 4:
                        myRooms[1] = myRooms[3] = null;
                        myRooms[2].setRoomFromDecimal(chamberNumber);
                        break;
                }

                string temp1 = mapInfo[(chamberNumber - 1) * 4 + (roomNumber - 1)];
                String[] temp2 = temp1.Split(' ', '\t');
                temp2 = temp2.Where(s => !string.IsNullOrEmpty(s)).ToArray();
                myRooms[roomNumber].setRoomFromDecimal(Convert.ToDouble(temp2[1]));
            }
            
            return myRooms;
        }

        /// <summary>
        /// Generates array of rooms two away from current location
        /// </summary>
        /// <param name="location"> Current location represented as a decimal (chamberNumber.roomNumber) </param>
        /// <returns></returns>
        public Room[] warningRooms(double location)
        {
            (loadCaveandRoom(location)).
        }

        /// <summary>
        /// Accessors
        /// </summary>
        public int getRoomsPerChamber()
        {
            return NUMB_OF_ROOMS;
        }
        public int getTotalChambers()
        {
            return totalRooms;
        }
    }
}
