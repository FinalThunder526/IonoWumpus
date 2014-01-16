using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace WumpusTest
{
    interface Cave
    {
        Room[] getConnectedRooms (double location);
        Room[] getConnectedRooms (Room myRoom);
        List<Room> warningRooms(double location);

        int getRoomsPerChamber();
        int getTotalRooms();
    }

    class Cave3 : Cave
    {   
        /// <summary>
        /// Class fields
        /// </summary>
        private string address;
        public int NUMB_OF_ROOMS = 9;
        int NUMB_OF_CHAMBERS = 10;
        int totalRooms = 50;

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
	    public Room[] getConnectedRooms (double location)
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
                            myRooms[3] = new Room(chamberNumber);
                            break;
                        case 2:
                            myRooms[1] = myRooms[3] = null;
                            myRooms[4] = new Room(chamberNumber);
                            break;
                        case 3:
                            myRooms[2] = myRooms[4] = null;
                            myRooms[1] = new Room(chamberNumber);
                            break;
                        case 4:
                            myRooms[1] = myRooms[3] = null;
                            myRooms[2] = new Room(chamberNumber);
                            break;
                    }

                    myRooms[roomNumber] = new Room(Convert.ToDouble(temp2[1]));
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
                            myRooms[1] = new Room(Convert.ToDouble(temp2[1]));
                            myRooms[2] = myRooms[4] = null;
                            myRooms[3] = new Room(chamberNumber);
                            break;
                        case 3:
                            myRooms[2] = new Room(Convert.ToDouble(temp2[1]));
                            myRooms[1] = myRooms[3] = null;
                            myRooms[4] = new Room(chamberNumber);
                            break;
                        case 4:
                        case 5:
                            myRooms[3] = new Room(Convert.ToDouble(temp2[1]));
                            myRooms[2] = myRooms[4] = null;
                            myRooms[1] = new Room(chamberNumber);
                            break;
                        case 6:
                            myRooms[4] = new Room(Convert.ToDouble(temp2[1]));
                            myRooms[1] = myRooms[3] = null;
                            myRooms[2] = new Room(chamberNumber);
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
                            myRooms[1] = new Room(Convert.ToDouble(temp2[1]));
                            myRooms[2] = myRooms[4] = null;
                            myRooms[3] = new Room(chamberNumber);
                            break;
                        case 4:
                            myRooms[2] = new Room(Convert.ToDouble(temp2[1]));
                            myRooms[1] = myRooms[3] = null;
                            myRooms[4] = new Room(chamberNumber);
                            break;
                        case 5:
                        case 6:
                        case 7:
                            myRooms[3] = new Room(Convert.ToDouble(temp2[1]));
                            myRooms[2] = myRooms[4] = null;
                            myRooms[1] = new Room(chamberNumber);
                            break;
                        case 8:
                            myRooms[4] = new Room(Convert.ToDouble(temp2[1]));
                            myRooms[1] = myRooms[3] = null;
                            myRooms[2] = new Room(chamberNumber);
                            break;
                    }
                }
            }

            return myRooms;
	    }

        /// <summary>
        /// Generates array of connected rooms based on current location
        /// </summary>
        /// <param name="location"> Current location represented as a decimal (chamberNumber.roomNumber) </param> 
        /// <returns></returns>
        public Room[] getConnectedRooms (Room myRoom)
        {
            double location = myRoom.getDecimalForm();

            String[] mapInfo = File.ReadAllLines(address);

            int chamberNumber = (int)location;
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
                            myRooms[3] = new Room(chamberNumber);
                            break;
                        case 2:
                            myRooms[1] = myRooms[3] = null;
                            myRooms[4] = new Room(chamberNumber);
                            break;
                        case 3:
                            myRooms[2] = myRooms[4] = null;
                            myRooms[1] = new Room(chamberNumber);
                            break;
                        case 4:
                            myRooms[1] = myRooms[3] = null;
                            myRooms[2] = new Room(chamberNumber);
                            break;
                    }

                    myRooms[roomNumber] = new Room(Convert.ToDouble(temp2[1]));
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
                            myRooms[1] = new Room(Convert.ToDouble(temp2[1]));
                            myRooms[2] = myRooms[4] = null;
                            myRooms[3] = new Room(chamberNumber);
                            break;
                        case 3:
                            myRooms[2] = new Room(Convert.ToDouble(temp2[1]));
                            myRooms[1] = myRooms[3] = null;
                            myRooms[4] = new Room(chamberNumber);
                            break;
                        case 4:
                        case 5:
                            myRooms[3] = new Room(Convert.ToDouble(temp2[1]));
                            myRooms[2] = myRooms[4] = null;
                            myRooms[1] = new Room(chamberNumber);
                            break;
                        case 6:
                            myRooms[4] = new Room(Convert.ToDouble(temp2[1]));
                            myRooms[1] = myRooms[3] = null;
                            myRooms[2] = new Room(chamberNumber);
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
                            myRooms[1] = new Room(Convert.ToDouble(temp2[1]));
                            myRooms[2] = myRooms[4] = null;
                            myRooms[3] = new Room(chamberNumber);
                            break;
                        case 4:
                            myRooms[2] = new Room(Convert.ToDouble(temp2[1]));
                            myRooms[1] = myRooms[3] = null;
                            myRooms[4] = new Room(chamberNumber);
                            break;
                        case 5:
                        case 6:
                        case 7:
                            myRooms[3] = new Room(Convert.ToDouble(temp2[1]));
                            myRooms[2] = myRooms[4] = null;
                            myRooms[1] = new Room(chamberNumber);
                            break;
                        case 8:
                            myRooms[4] = new Room(Convert.ToDouble(temp2[1]));
                            myRooms[1] = myRooms[3] = null;
                            myRooms[2] = new Room(chamberNumber);
                            break;
                    }
                }
            }

            return myRooms;
        }

        /// <summary>
        /// Generates array of rooms two away from current location
        /// </summary>
        /// <param name="location"> Current location represented as a decimal (chamberNumber.roomNumber) </param>
        /// <returns></returns>
        public List<Room> warningRooms (double location)
        {
            Room[] oneAwayArr = getConnectedRooms(location);
            List<Room> oneAway = oneAwayArr.ToList<Room>();
            List<Room> twoAway = new List<Room>();

            for (int i = 0; i < oneAway.Count; i++)
            {
                if (oneAway[i] == null)
                {
                    oneAway.RemoveAt(i);
                    i--;
                }

                twoAway.AddRange(getConnectedRooms(oneAway[i]));
            }

            for (int i = 0; i < twoAway.Count; i++)
            {
                if (twoAway[i] == null)
                {
                    twoAway.RemoveAt(i);
                    i--;
                }
            }

            return twoAway;
        }

        /// <summary>
        /// Accessors
        /// </summary>
        public int getRoomsPerChamber()
        {
            return NUMB_OF_ROOMS;
        }
        public int getTotalRooms()
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
        public Cave2 (string newAddress)
        {
            address = newAddress;
        }

        /// <summary>
        /// Generates array of connected rooms based on current location
        /// </summary>
        /// <param name="location"> Current location represented as a decimal (chamberNumber.roomNumber) </param> 
        /// <returns></returns>
        public Room[] getConnectedRooms (double location)
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
                        myRooms[3] = new Room(chamberNumber);
                        break;
                    case 2:
                        myRooms[1] = myRooms[3] = null;
                        myRooms[4] = new Room(chamberNumber);
                        break;
                    case 3:
                        myRooms[2] = myRooms[4] = null;
                        myRooms[1] = new Room(chamberNumber);
                        break;
                    case 4:
                        myRooms[1] = myRooms[3] = null;
                        myRooms[2] = new Room(chamberNumber);
                        break;
                }

                string temp1 = mapInfo[(chamberNumber - 1) * 4 + (roomNumber - 1)];
                String[] temp2 = temp1.Split(' ', '\t');
                temp2 = temp2.Where(s => !string.IsNullOrEmpty(s)).ToArray();
                myRooms[roomNumber] = new Room(Convert.ToDouble(temp2[1]));
            }

            return myRooms;
        }

        /// <summary>
        /// Generates array of connected rooms based on current location
        /// </summary>
        /// <param name="location"> Current location represented as a decimal (chamberNumber.roomNumber) </param> 
        /// <returns></returns>
        public Room[] getConnectedRooms (Room myRoom)
        {
            double location = myRoom.getDecimalForm();

            String[] mapInfo = File.ReadAllLines(address);

            int chamberNumber = (int)location;
            int roomNumber = (int)(((location - chamberNumber) * 10.0) + 0.5);

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
                        myRooms[3] = new Room(chamberNumber);
                        break;
                    case 2:
                        myRooms[1] = myRooms[3] = null;
                        myRooms[4] = new Room(chamberNumber);
                        break;
                    case 3:
                        myRooms[2] = myRooms[4] = null;
                        myRooms[1] = new Room(chamberNumber);
                        break;
                    case 4:
                        myRooms[1] = myRooms[3] = null;
                        myRooms[2] = new Room(chamberNumber);
                        break;
                }

                string temp1 = mapInfo[(chamberNumber - 1) * 4 + (roomNumber - 1)];
                String[] temp2 = temp1.Split(' ', '\t');
                temp2 = temp2.Where(s => !string.IsNullOrEmpty(s)).ToArray();
                myRooms[roomNumber] = new Room(Convert.ToDouble(temp2[1]));
            }

            return myRooms;
        }

        /// <summary>
        /// Generates array of rooms two away from current location
        /// </summary>
        /// <param name="location"> Current location represented as a decimal (chamberNumber.roomNumber) </param>
        /// <returns></returns>
        public List<Room> warningRooms (double location)
        {
            Room[] oneAwayArr = getConnectedRooms(location);
            List<Room> oneAway = oneAwayArr.ToList<Room>();
            List<Room> twoAway = new List<Room>();

            for (int i = 0; i < oneAway.Count; i++)
            {
                if (oneAway[i] == null)
                {
                    oneAway.RemoveAt(i);
                    i--;
                }

                twoAway.AddRange(getConnectedRooms(oneAway[i]));
            }

            for (int i = 0; i < twoAway.Count; i++)
            {
                if (twoAway[i] == null)
                {
                    twoAway.RemoveAt(i);
                    i--;
                }
            }

            return twoAway;
        }
       
        /// <summary>
        /// Accessors
        /// </summary>
        public int getRoomsPerChamber()
        {
            return NUMB_OF_ROOMS;
        }
        public int getTotalRooms()
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
        public Room[] getConnectedRooms (double location)
        {
            String[] mapInfo = File.ReadAllLines(address);

            int chamberNumber = (int) location;
            int roomNumber = (int) ( ( (location - chamberNumber) * 10.0) + 0.5 );

	        Room[] myRooms = new Room[NUMB_OF_ROOMS + 1];

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
                        myRooms[3] = new Room(chamberNumber, 0);
                        break;
                    case 2:
                        myRooms[1] = myRooms[3] = null;
                        myRooms[4] = new Room(chamberNumber, 0);
                        break;
                    case 3:
                        myRooms[2] = myRooms[4] = null;
                        myRooms[1] = new Room(chamberNumber, 0);
                        break;
                    case 4:
                        myRooms[1] = myRooms[3] = null;
                        myRooms[2] = new Room(chamberNumber, 0);
                        break;
                }

                string temp1 = mapInfo[(chamberNumber - 1) * 4 + (roomNumber - 1)];
                String[] temp2 = temp1.Split(' ', '\t');
                temp2 = temp2.Where(s => !string.IsNullOrEmpty(s)).ToArray();
                myRooms[roomNumber] = new Room(Convert.ToDouble(temp2[1]));
            }
            
            return myRooms;
        }

        /// <summary>
        /// Generates array of connected rooms based on current location
        /// </summary>
        /// <param name="location"> Current room </param> 
        /// <returns></returns>
        public Room[] getConnectedRooms (Room myRoom)
        {
            double location = myRoom.getDecimalForm();

            String[] mapInfo = File.ReadAllLines(address);

            int chamberNumber = (int)location;
            int roomNumber = (int)(((location - chamberNumber) * 10.0) + 0.5);

            Room[] myRooms = new Room[NUMB_OF_ROOMS + 1];

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
                        myRooms[3] = new Room(chamberNumber, 0);
                        break;
                    case 2:
                        myRooms[1] = myRooms[3] = null;
                        myRooms[4] = new Room(chamberNumber, 0);
                        break;
                    case 3:
                        myRooms[2] = myRooms[4] = null;
                        myRooms[1] = new Room(chamberNumber, 0);
                        break;
                    case 4:
                        myRooms[1] = myRooms[3] = null;
                        myRooms[2] = new Room(chamberNumber, 0);
                        break;
                }

                string temp1 = mapInfo[(chamberNumber - 1) * 4 + (roomNumber - 1)];
                String[] temp2 = temp1.Split(' ', '\t');
                temp2 = temp2.Where(s => !string.IsNullOrEmpty(s)).ToArray();
                myRooms[roomNumber] = new Room(Convert.ToDouble(temp2[1]));
            }

            return myRooms;
        }

        /// <summary>
        /// Generates array of rooms two away from current location
        /// </summary>
        /// <param name="location"> Current location represented as a decimal (chamberNumber.roomNumber) </param>
        /// <returns></returns>
        public List<Room> warningRooms (double location)
        {
            Room[] oneAwayArr = getConnectedRooms(location);
            List<Room> oneAway = removeNulls(oneAwayArr);
            List<Room> twoAway = new List<Room>();

            for (int i = 0; i < oneAway.Count; i++)
            {
                if (oneAway[i] != null)
                {
                    twoAway.Add(oneAway[i]);
                    foreach (Room r in removeNulls(getConnectedRooms(oneAway[i])))
                    {
                        if (!r.Equals(new Room(location)))
                        {
                            twoAway.Add(r);
                        }
                    }
                }
            }

            //for (int i = 0; i < twoAway.Count; i++)
            //{
            //    if (twoAway[i] == null)
            //    {
            //        twoAway.RemoveAt(i);
            //        i--;
            //    }
            //}

            return twoAway;
        }

        private List<Room> removeNulls(Room[] room)
        {
            List<Room> withoutNulls = new List<Room>();
            for (int i = 0; i < room.Length; i++)
            {
                if (room[i] != null)
                {
                    withoutNulls.Add(room[i]);
                }
            }
            return withoutNulls;
        }

        /*    
        var oneAwayWithoutNulls = oneAway.Take(oneAway.Length - numbOfNulls);
        var twoAwayWithRepeats = oneAwayWithoutNulls.SelectMany(n => loadCaveandRoom(n)).ToArray();
        var twoAway = twoAwayWithRepeats.Distinct().ToArray();
         
        static void Main(string[] args)
        {
            int one = 1;
            int[] stuff = DoStuff(one);
            var stuffModded = stuff.SelectMany(n => DoStuff(n)).ToArray();
            var result = stuffModded.Distinct().ToArray();
        }
        static int[] DoStuff(int num)
        {
            return new int[] { PlusThreeModFive(num), PlusThreeModFive(num + 1) };        
        }
        static int PlusThreeModFive(int num)
        {
            return (num + 3) % 5;
        }
        */

        /// <summary>
        /// Accessors
        /// </summary>
        public int getRoomsPerChamber()
        {
            return NUMB_OF_ROOMS;
        }
        public int getTotalRooms()
        {
            return totalRooms;
        }
    }
}