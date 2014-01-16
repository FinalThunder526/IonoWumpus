using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace WumpusTest
{
    // Active Wumpus rules

    class Wumpus : Sprite
    {
        private Random r = new Random();

        // turns when Wumpus isn't moving
        private int countAsleep;

        // number rooms that the wumpus should move in that turn 
        private int numberOfMoves;

        public Wumpus()
        {
            countAsleep = 0;
        }
        public Wumpus(Vector2 NewPos, string NewAsset)
            : base(NewPos, NewAsset)
        { }
        /*
         * parameters: trivia = true if Wumpus was defeated through trivia, else false- can be changed and new method made for trivia
         */
        public int Move(bool trivia)
        {
            // if should teleport, returns -1
            if (shouldTeleport())
            {
                countAsleep = 0;
                return -1;
            }
                // What happens if player wins trivia against wumpus
            else if (trivia)
            {
                // moves 2-4 rooms at a time for 1 turn
                countAsleep = 0;
                numberOfMoves = r.Next(3) + 2;
            }
                // if wumpus wasn't moving for more than 4 moves, it moves 1 room
            else if (countAsleep > 4)
            { 
                numberOfMoves = 1;
                countAsleep = 0;
            }
            else
            {
                countAsleep++;
                numberOfMoves = 0;
            }

            return numberOfMoves;
        }

        public bool shouldTeleport()
        {
            // determines if wumpus should teleport- 5% chance
            int n = r.Next(20);
            if (n == 0)
                return true;
            else 
                return false;
        }
    }
}
