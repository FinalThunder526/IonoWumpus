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
        Map m;

        enum state {asleep, moving};
        private state WumpusState;

        // turns when Wumpus isn't moving
        private int countAsleep;

        // number rooms moves each turn while moving
        private int numberOfMoves;

        // r.Next(3) + 1   when need to reset it
        private int randomCount;

        public Wumpus()
        {
            m = new Map();
            WumpusState = state.asleep;
            countAsleep = 0;
        }
        public Wumpus(Vector2 NewPos, string NewAsset)
            : base(NewPos, NewAsset)
        { }
        /*
         * moveWumpus method should be called by Game Control (?, maybe called by Map class) whenever player moves/finishes move
         * parameters: trivia = true if Wumpus was defeated through trivia, else false- can be changed and new method made for trivia
         */
        public int Move(bool trivia)
        {
            if (shouldTeleport())
            {
                // teleport to random room, not including one that was just in
                int currentWumpusRoom = 0;//m.getWumpusRoomInt();
                do
                {
                    //m.randomizeCharacterRoom(new Room[1]);
                } while (currentWumpusRoom == 0);//m.getWumpusRoomInt());

                numberOfMoves = -1;
                WumpusState = state.asleep;
            }
            else if (trivia)
            {
                // moves 2 rooms at a time for 1 - 3 turns
                numberOfMoves = 2;
                randomCount = r.Next(3) + 1;
                WumpusState = state.moving;
            }
            else if (WumpusState == state.moving)
            {
                // if the Wumpus was in the middle of a moving sequence and it didn't finish it, numberOfMoves doesnt change
                // if it was moving but is supposed to stop, spends at least 1 move asleep
                if (randomCount > 1)
                {
                    randomCount--;
                }
                else
                {
                    // same code as below- simplify?
                    WumpusState = state.asleep;
                    numberOfMoves = 0;
                    countAsleep++;
                }
            }
            else if (countAsleep > 4)
            {
                WumpusState = state.moving;
                numberOfMoves = 1;
                randomCount = r.Next(3) + 1;
                countAsleep = 0;
            }
            else
            {
                countAsleep++;
                numberOfMoves = 0;
                WumpusState = state.asleep;
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
