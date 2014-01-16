using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace WumpusTest
{
    
    class Cave3
    {
	// Constructor
	public Cave3()
	{
	}

	// map object passes information about current location to cave object
	// this function generates connectivty info
	public String loadCaveandRoom (double location)
	{
	    String[] mapInfo = File.ReadAllLines(@"C:\Users\jains\Documents\CaveFiles\MapDataCave3.txt");
	    String connectivityInfo = "";

            int chamberNumber = (int) location;
            int roomNumber = (int)(((location - chamberNumber) * 10.0) + 0.5);

            char chamberGate = ' ';
            char externalGate = ' ';

	    if (chamberNumber < 8)
            {
            	switch (roomNumber)
            	{
                    case 1:
                    	chamberGate = 'S'; externalGate = 'N';
                    	break;
               	    case 2: 
 			chamberGate = 'W'; externalGate = 'E';
                        break;
                    case 3:
                        chamberGate = 'N'; externalGate = 'S';
                        break;
                    case 4:
                        chamberGate = 'E'; externalGate = 'W';
                        break;
                }
	    }

	    if (chamberNumber == 8)
            {
            	switch (roomNumber)
            	{
                    case 1:
		    case 2:
                    	chamberGate = 'S'; externalGate = 'N';
                    	break;
               	    case 3: 
 			chamberGate = 'W'; externalGate = 'E';
                        break;
                    case 4:
		    case 5:
                        chamberGate = 'N'; externalGate = 'S';
                        break;
                    case 6:
                        chamberGate = 'E'; externalGate = 'W';
                        break;
                }
	    }

	    if (chamberNumber == 9 || chamberNumber == 10)
            {
            	switch (roomNumber)
            	{
                    case 1:
		    case 2:
                    case 3:
                    	chamberGate = 'S'; externalGate = 'N';
                    	break;
               	    case 4: 
 			chamberGate = 'W'; externalGate = 'E';
                        break;
                    case 5:
		    case 6:
                    case 7:
                        chamberGate = 'N'; externalGate = 'S';
                        break;
                    case 8:
                        chamberGate = 'E'; externalGate = 'W';
                        break;
                }
	    }

            connectivityInfo = chamberGate + " gate: Chamber " + chamberNumber;

            string temp1 = mapInfo[(chamberNumber - 1) * 4 + (roomNumber - 1)];
            String[] temp2 = temp1.Split(' ', '\t');
            temp2 = temp2.Where(s => !string.IsNullOrEmpty(s)).ToArray();
            connectivityInfo += Environment.NewLine + externalGate + " gate: " + temp2[1];

            return connectivityInfo;
	}
    }

    class Cave2 
    {
        // Constructor
        public Cave2()
        {
        }

        // map object passes information about current location to cave object
        // this function generates connectivity info
        public String loadCaveandRoom (double location)
        {
            String[] mapInfo = File.ReadAllLines(@"C:\Users\jains\Documents\CaveFiles\MapDataCave2.txt");
            String connectivityInfo = "";


            int chamberNumber = (int)location;
            int roomNumber = (int)(((location - chamberNumber) * 10.0) + 0.5);

            char chamberGate = ' ';
            char externalGate = ' ';

            switch (roomNumber)
            {
                case 1:
                    chamberGate = 'S'; externalGate = 'N';
                    break;
                case 2:
                    chamberGate = 'W'; externalGate = 'E';
                    break;
                case 3:
                    chamberGate = 'N'; externalGate = 'S';
                    break;
                case 4:
                    chamberGate = 'E'; externalGate = 'W';
                    break;
            }

            connectivityInfo = chamberGate + " gate: Chamber " + chamberNumber;

            string temp1 = mapInfo[(chamberNumber - 1) * 4 + (roomNumber - 1)];
            String[] temp2 = temp1.Split(' ', '\t');
            temp2 = temp2.Where(s => !string.IsNullOrEmpty(s)).ToArray();
            connectivityInfo += Environment.NewLine + externalGate + " gate: " + temp2[1];

            return connectivityInfo;
        }
    }

    class Cave1 
    {
        private string address;

	    // Constructor
        public Cave1 (string newAddress)
        {
            this.address = newAddress;
        }

        // map object passes information about current location to cave object
	    // this function generates connectivity info
        public String loadCaveandRoom (double location)
        {
            String[] mapInfo = File.ReadAllLines(address);
            String connectivityInfo = "";

            /* /int chamberNumber = (int) location;
            int roomNumber = (int) ( ( (location - chamberNumber) * 10.0) + 0.5 );
             * 
             */

            Room MyRoom = new Room(location);

            char chamberGate = ' ';
            char externalGate = ' ';

            switch (MyRoom.RoomNumber)
            {
                case 1:
                    chamberGate = 'S'; externalGate = 'N';
                    break;
                case 2: 
                    chamberGate = 'W'; externalGate = 'E';
                    break;
                case 3: 
                    chamberGate = 'N'; externalGate = 'S';
                    break;
                case 4: 
                    chamberGate = 'E'; externalGate = 'W';
                    break;
            }

            connectivityInfo = chamberGate + " gate: Chamber " + MyRoom.ChamberNumber;

            string temp1 = mapInfo[(MyRoom.ChamberNumber - 1) * 4 + (MyRoom.RoomNumber - 1)];
            String[] temp2 = temp1.Split(' ', '\t');
            temp2 = temp2.Where(s => !string.IsNullOrEmpty(s)).ToArray();
            connectivityInfo += Environment.NewLine + externalGate + " gate: " + temp2[1];

            return connectivityInfo;
        }
    }
}
