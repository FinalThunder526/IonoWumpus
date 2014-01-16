using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WumpusTest
{
    class WHighScores
    {
        
        /*private const int initialTurnBonus = 20;
        private const int initialTimeBonus = 1000;
        private const int coinMultiplier = 10;
        private const int batMultiplier = 50;*/

        //private int turn = 0, batNumber = 0, coins = 0, timeTotal = 0;
        List<int> myList = new List<int>();
        
        /// <summary>
        /// Accesses the scores from file, and updates the scores.
        /// </summary>
        /// "Score" is the score to be saved in the high score list
        public void saveHighScores(int Score)
        {
            // create reader & open file
            //TextReader tr = new StreamReader(@"highScores.txt");

            // read a line of text and store it in an ArrayList
            string[] highscores = File.ReadAllLines(@"highScores.txt");

            myList = new List<int>();

            foreach (string s in highscores)
            {
                myList.Add(Convert.ToInt32(s));
            }

            //int x = 0, current;
            //while (x < 10)
            //{
            //    current = Convert.ToInt32(tr.ReadLine());
            //    if (!myList.Contains(current))
            //    {
            //        myList.Add(current);
            //    }
            //    x++;    
            //}
            // close the stream
            //tr.Close();
            
            myList.Add(Score);
            myList.Sort();
            myList.Reverse();

            string str = "";
            foreach (int i in myList)
            {
                str += i + "\n";
            }
            // create a writer and open the file
            //TextWriter tw = new StreamWriter(@"highScores.txt");
            
            // write a line of text to the file
            //for (int j = 0; j < myList.Count; j++)
            //{
            //    tw.WriteLine(myList[j]);
            //}

            File.WriteAllText(@"highscores.txt", str);

            // close the stream
            //tw.Close();

        }

        //mutators
        /*public void TurnCounter()
        {
            turn++;
        }

        public void batEncounter()
        {
            batNumber++;
        }

        public void coin()
        {
            coins++;
        }

        public void time(int tm)
        {
            if (tm >= initialTimeBonus)
            {
                tm = initialTimeBonus;
            }
            tm = timeTotal;

        }*/

        //accesses the list of scores
        public List<int> getMyList()
        {
            return myList;
        }

        

        //High Score Calculator
        /*public int currentScore(int coins, int batNumber)
        {
            int coinBonus = coins*coinMultiplier;
            int batBonus = batNumber*batMultiplier;

            int cScore = coinBonus + batBonus;
            return cScore;
        }

        public int finalScore(int coins, int batNumber, int turn, int timeTotal)
        {

            int turnBonus = initialTurnBonus - turn;
            int timeBonus = initialTimeBonus - timeTotal;
            int coinBonus = coins * coinMultiplier;
            int batBonus = batNumber * batMultiplier;

            int fScore = turnBonus + timeBonus + coinBonus + batBonus;
            return fScore;
        }*/

        /// <summary>
        /// This Calculates the score according to the specifications given.  May be changed for added variability to the game.
        /// </summary>
        /// Number of turns refers to the total number of turns that the player made during the game.
        /// Number of coins refers to the total number of coins that the player collected during the game.
        /// Number of Arrows refers to the number of arrows that the player has at the end of the game.
        /// <returns >calculated high score</returns>
        public int scoreCalculator(int numberOfTurns, int numberOfCoins, int numberOfArrows)
        {
            return 100 - numberOfTurns + numberOfCoins + (10 * numberOfArrows);
        }
    }
}

