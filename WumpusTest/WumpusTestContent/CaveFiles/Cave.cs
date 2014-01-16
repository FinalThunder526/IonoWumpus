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
	public Room[] loadCaveandRoom (double location)
	{
	    String[] mapInfo = File.ReadAllLines(@"C:\Users\jains\Documents\CaveFiles\MapDataCave3.txt");
	    String connectivityInfo = "";

            int chamberNumber = (int) location;
            int roomNumber = (int)(((location - chamberNumber) * 10.0) + 0.5);

            char chamberGate = ' ';
            char externalGate = ' ';

	    Room[] myRooms = new Room[5];
	    myRooms[0] = null;

	    for (int i=0; i<mapInfo.length; i++)
	    {
		String temp1 = mapInfo[i];
		String[] temp2 = temp1.split(' ', '\t');
                temp2 = temp2.Where(s => !string.IsNullOrEmpty(s)).ToArray();

                if (temp2[0] == location)
		  break;
	    }

	    if (chamberNumber < 8)
            {
            	switch (roomNumber)
            	{
                    case 1:
		        myRooms[2] = myRooms[4] = null;
                        myRooms[3].setDecimalNumber(ChamberNumber);
                        break;
                    case 2: 
		        myRooms[1] = myRooms[3] = null;
                        myRooms[4].setDecimalNumber(ChamberNumber);
                        break;
                    case 3: 
		        myRooms[2] = myRooms[4] = null;
                        myRooms[1].setDecimalNumber(ChamberNumber);
                        break;
                    case 4: 
		        myRooms[1] = myRooms[3] = null;
                        myRooms[2].setDecimalNumber(ChamberNumber);
                        break;
                }

		myRooms[roomNumber] = setDecimalNumber(temp2[1]);
	    }

	    if (chamberNumber == 8)
            {
            	switch (roomNumber)
            	{
                    case 1:
		    case 2:
			myRooms[1].setDecimalNumber(temp2[1]);
                    	myRooms[2] = myRooms[4] = null;
                    	myRooms[3].setDecimalNumber(ChamberNumber);
                    	break;
               	    case 3: 
			myRooms[2].setDecimalNumber(temp2[1]);
		        myRooms[1] = myRooms[3] = null;
                        myRooms[4].setDecimalNumber(ChamberNumber);
                        break;
                    case 4:
		    case 5:
			myRooms[3].setDecimalNumber(temp2[1]);
		        myRooms[2] = myRooms[4] = null;
                        myRooms[1].setDecimalNumber(ChamberNumber);
                    case 6:
			myRooms[4].setDecimalNumber(temp2[1]);
		        myRooms[1] = myRooms[3] = null;
                        myRooms[2].setDecimalNumber(ChamberNumber);
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
			myRooms[1].setDecimalNumber(temp2[1]);
                    	myRooms[2] = myRooms[4] = null;
                    	myRooms[3].setDecimalNumber(ChamberNumber);
                    	break;
               	    case 4: 
			myRooms[2].setDecimalNumber(temp2[1]);
		        myRooms[1] = myRooms[3] = null;
                        myRooms[4].setDecimalNumber(ChamberNumber);
                        break;
                    case 5:
		    case 6:
                    case 7:
			myRooms[3].setDecimalNumber(temp2[1]);
		        myRooms[2] = myRooms[4] = null;
                        myRooms[1].setDecimalNumber(ChamberNumber);
                    case 8:
			myRooms[4].setDecimalNumber(temp2[1]);
		        myRooms[1] = myRooms[3] = null;
                        myRooms[2].setDecimalNumber(ChamberNumber);
                        break;
                }
	    }

	}

	
	/*
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
	*/
    }

    class Cave2 
    {
        // Constructor
        public Cave2()
        {
        }

	/*
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
	*/

        // map object passes information about current location to cave object
	// this function generates connectivity info
        public Room[] loadCaveandRoom (double location)
        {
            String[] mapInfo = File.ReadAllLines(@"C:\Users\jains\Documents\CaveFiles\MapDataCave1.txt");
            String connectivityInfo = "";

            int chamberNumber = (int) location;
            int roomNumber = (int) ( ( (location - chamberNumber) * 10.0) + 0.5 );

            char chamberGate = ' ';
            char externalGate = ' ';

	    Room[] myRooms = new Room[5];
	    myRooms[0] = null;

            switch (roomNumber)
            {
                case 1:
		    myRooms[2] = myRooms[4] = null;
                    myRooms[3].setDecimalNumber(ChamberNumber);
                    break;
                case 2: 
		    myRooms[1] = myRooms[3] = null;
                    myRooms[4].setDecimalNumber(ChamberNumber);
                    break;
                case 3: 
		    myRooms[2] = myRooms[4] = null;
                    myRooms[1].setDecimalNumber(ChamberNumber);
                    break;
                case 4: 
		    myRooms[1] = myRooms[3] = null;
                    myRooms[2].setDecimalNumber(ChamberNumber);
                    break;
            }

            string temp1 = mapInfo[ (chamberNumber - 1) * 4 + (roomNumber - 1) ];
            String[] temp2 = temp1.Split(' ', '\t');
            temp2 = temp2.Where(s => !string.IsNullOrEmpty(s)).ToArray();
            myRooms[roomNumber].setDecimalNumber( temp2[1] );

            return myRooms;
        }
    }

    class Cave1 
    {
	// Constructor
        public Cave1 ()
        {
        }

	/*
        // map object passes information about current location to cave object
	// this function generates connectivity info
        public String loadCaveandRoom (double location)
        {
            String[] mapInfo = File.ReadAllLines(@"C:\Users\jains\Documents\CaveFiles\MapDataCave1.txt");
            String connectivityInfo = "";

            int chamberNumber = (int) location;
            int roomNumber = (int) ( ( (location - chamberNumber) * 10.0) + 0.5 );

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

            string temp1 = mapInfo[ (chamberNumber - 1) * 4 + (roomNumber - 1) ];
            String[] temp2 = temp1.Split(' ', '\t');
            temp2 = temp2.Where(s => !string.IsNullOrEmpty(s)).ToArray();
            connectivityInfo += Environment.NewLine + externalGate + " gate: " + temp2[1];

            return connectivityInfo;
        }
	*/

        // map object passes information about current location to cave object
	// this function generates connectivity info
        public Room[] loadCaveandRoom (double location)
        {
            String[] mapInfo = File.ReadAllLines(@"C:\Users\jains\Documents\CaveFiles\MapDataCave1.txt");
            String connectivityInfo = "";

            int chamberNumber = (int) location;
            int roomNumber = (int) ( ( (location - chamberNumber) * 10.0) + 0.5 );

            char chamberGate = ' ';
            char externalGate = ' ';

	    Room[] myRooms = new Room[5];
	    myRooms[0] = null;

            switch (roomNumber)
            {
                case 1:
		    myRooms[2] = myRooms[4] = null;
                    myRooms[3].setDecimalNumber(ChamberNumber);
                    break;
                case 2: 
		    myRooms[1] = myRooms[3] = null;
                    myRooms[4].setDecimalNumber(ChamberNumber);
                    break;
                case 3: 
		    myRooms[2] = myRooms[4] = null;
                    myRooms[1].setDecimalNumber(ChamberNumber);
                    break;
                case 4: 
		    myRooms[1] = myRooms[3] = null;
                    myRooms[2].setDecimalNumber(ChamberNumber);
                    break;
            }

            string temp1 = mapInfo[ (chamberNumber - 1) * 4 + (roomNumber - 1) ];
            String[] temp2 = temp1.Split(' ', '\t');
            temp2 = temp2.Where(s => !string.IsNullOrEmpty(s)).ToArray();
            myRooms[roomNumber].setDecimalNumber( temp2[1] );

            return myRooms;
        }
    }
}
