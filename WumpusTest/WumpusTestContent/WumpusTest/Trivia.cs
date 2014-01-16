using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// Yo dawg I heard you like exceptions so we put an exception
// in your exception so you can debug while you debug.


/* 
 * This object stores a question and answer based off of an index
 * drawn from a database of questions and answers.  To change the question
 * and answer set currently "active", you need to randomize the index.
 *
 * Current databases for questions/answers include:
 * chemTriviaAnswers.txt
 * chemTriviaQuestions.txt
 * 
 * These must be in the same folder as the main project
 * in order for the program to work.
 * 
 * Updated 4/9/2012 (added sealed ints dealing with the total correct needed to "win")
 * Updated 4/10/2012 (class variables for lifetimeCorrect and lifetimeAttempted added as well
 *                      as a method, getInfo, that relays information about the game)
 *                      *note: currently, 5 questions asked, 3 correct ones needed to "win"
 * Updated 4/11/2012 (chem trivia questions expanded to include through element 86, no lanthanides)
 * 
 */

namespace WumpusTest {


    /* Categories of questions available:
     * 
     * Chemistry
     * Algebra
     * 
    */

    class Trivia {
        // random number generator
        Random gen;

        // String array of all questions' text
        private String[] questions;
        // String array of all answers' text
        private String[] textDocAnswers;
        // if "usedIndexes" element is "true", then it can't be used again
        private bool[] usedIndexes;

        private Question[] allQuestions;

        private String type;
        private int finalWins, finalLosses, 
            winCount, lossCount, totalCount, 
            index;

        // amount of questions that must be answered correctly to "win"
        public const int correctLength = 3;

        // amount of questions in a sequence of questions
        public const int questionSequenceLength = 5;

        // variables to keep track of the total correct/incorrect
        // during an entire session of the game
        public static int lifetimeCorrect = 0;
        public static int lifetimeAttempted = 0;

        // constructor, accepts a question type as a string
        public Trivia(String qtype)
        {
            gen = new Random();
            if (qtype.Equals("Chemistry") || qtype.Equals("chemistry"))
            {
                this.type = qtype;
                questions = System.IO.File.ReadAllLines("chemTriviaQuestions.txt");
                textDocAnswers = System.IO.File.ReadAllLines("chemTriviaAnswers.txt");
            }
            else if (qtype.Equals("Algebra") || qtype.Equals("algebra"))
            {
                this.type = qtype;
                questions = System.IO.File.ReadAllLines("algTriviaQuestions.txt");
                textDocAnswers = System.IO.File.ReadAllLines("algTriviaAnswers.txt");
            }
            else
            {
                questions = System.IO.File.ReadAllLines("chemTriviaQuestions.txt");
                textDocAnswers = System.IO.File.ReadAllLines("chemTriviaAnswers.txt");
            }

            usedIndexes = new bool[questions.Length];
            allQuestions = new Question[questions.Length];
            for (int i = 0; i < allQuestions.Length; i++)
            {
                //allQuestions[i].processQuestionTextLine(questions[i]);
                //allQuestions[i].processAnswerTextLine(textDocAnswers[i]);
            }
            this.randomizeIndex();
            totalCount = 0;
            winCount = 0;
        }

        // sets a random question and answer:
        // doesn't check if question has been used
        public void randomizeIndex()
        {
            int prevIndex;
            usedIndexes[index] = true;
            prevIndex = index;
            index = gen.Next(this.getAmountOfQuestions());
            while (prevIndex == index)
            {
                index = gen.Next(this.getAmountOfQuestions());
            }
        }

        // selects a random question and answer that haven't been used yet
        public void safeRandomizeIndex()
        {

            usedIndexes[index] = true;
            
            // checks to see if all answers have been used, and, if so
            // resets so that all questions may be used again
            if (this.completelyUsed())
            {
                this.resetUsedIndexes();
            }

            index = gen.Next(this.getAmountOfQuestions());

            while (usedIndexes[index] == true)
            {
                index = gen.Next(this.getAmountOfQuestions());
            }
        }

        // checks to see if all answers have been used
        public bool completelyUsed()
        {
            for (int i = 0; i < usedIndexes.Length; i++)
            {
                if (usedIndexes[i] == false)
                {
                    return false;
                }
            }
            return true;
        }

        // resets all bool indexes of question/answer to false
        // so that they can be reused
        public void resetUsedIndexes()
        {
            for (int i = 0; i < usedIndexes.Length; i++)
            {
                usedIndexes[i] = false;
            }
        }

        // returns chemical symbol of answer
        public string getAnswer(int searchIndex)
        {
            return allQuestions[searchIndex].getAnswer();
        }

        public string getAnswer()
        {
            return allQuestions[index].getAnswer();
        }

        // returns question in "What is the chemical element for ____?" format
        public string getQuestion()
        {
            return questions[index];
        }

        // sends back a random answer string
        // POSTCONDITION: the answer will not be the same as the index's answer
        public string getRandomAnswer()
        {
            int ind = gen.Next(this.getAmountOfQuestions());
            while (ind == index)
            {
                ind = gen.Next(this.getAmountOfQuestions());
            }
            return textDocAnswers[ind];
        }

        // returns an array of strings that have answers that aren't the same, and
        // aren't equal to the index
        public string[] getDistractors()
        {
            return allQuestions[index].getRandomizedAnswerChoices();
        }

        // checks if answer is right
        public bool checkAnswer(String ans)
        {
            return(ans.Equals(textDocAnswers[index]));
        }

        public Question getQuestion(int ind)
        {
            return allQuestions[ind];
        }

        // returns total amount of questions
        public int getAmountOfQuestions()
        {
            return questions.Length;
        }

        // resets win/loss count to zero
        public void resetWinLossInfo()
        {
            winCount = 0;
            totalCount = 0;
        }

        // adds one to total questions asked, as well as one to correct
        // *used in line with "checkWin" and "checkLoss" methods
        public void gotCorrect()
        {
            winCount++;
            totalCount++;

        }

        // adds one to total questions asked
        // *used in line with "checkWin" and "checkLoss" methods
        public void gotIncorrect()
        {
            lossCount++;
            totalCount++;
        }

        // returns if win
        // returns false if not
        public bool checkWin()
        {
            return winCount >= 3;
        }

        // returns true if loss
        // returns false if not
        public bool checkLoss()
        {
            return lossCount >= 3;
        }

        // returns a string regarding whether the user
        // has won, lost, or neither yet
        public String getWinLoss()
        {
            if (this.checkWin())
            {
                return "win";
            }
            if (this.checkLoss())
            {
                return "loss";
            }
            return "neither";
        }

        // returns basics: current trivia category,
        // questions correct, and questions attempted
        public String getInfo()
        {
            String str = "";
            str += "Current Trivia Category: " + type +
                "\nQuestions correct: " + winCount +
                "\nTotal Questions: " + totalCount;
            return str;
        }

        ///returns string containing all info about
        // the Trivia class
        public String toString()
        {
            String str = this.getInfo();
            str += "Current Index: " + index +
                "\nCurrent Question: " + this.getQuestion() +
                "\nCurrent Answer: " + this.getAnswer() +
                "\nAll Questions Used: " + this.completelyUsed() +
                "Lifetime Correct: " + lifetimeCorrect +
                "Lifetime Attempted: " + lifetimeAttempted;
            return str;
        }

    }

}