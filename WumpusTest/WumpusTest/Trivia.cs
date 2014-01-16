using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Design;
using Microsoft.Xna.Framework.Content;

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

    class Trivia : Sprite {
        // random number generator
        Random gen;

        public SpriteFont TriviaFont;
        public Sprite[] AnswerButtons;

        private Question[] allQuestions;
        private bool[] usedIndexes;
        private string[] randomizedAnswersFinal;

        private String type;
        private int finalWins, finalLosses, 
            //winCount, lossCount, totalCount, 
            index;

        /// <summary>
        /// accepts a question type as the parameter
        /// </summary>
        /// <param name="qtype"></param>
        public Trivia(String qtype, Vector2 newPos, string newAsset) : base(newPos, newAsset)
        {
            String[] questions;
            String[] textDocAnswers;
            gen = new Random();
            //if (qtype.Equals("Chemistry") || qtype.Equals("chemistry"))
            //{
            //    this.type = qtype;
            //    questions = System.IO.File.ReadAllLines("chemTriviaQuestions.txt");
            //    textDocAnswers = System.IO.File.ReadAllLines("chemTriviaAnswers.txt");
            //} else
            if (qtype.Equals("Algebra") || qtype.Equals("algebra"))
            {
                this.type = qtype;
                questions = System.IO.File.ReadAllLines("algTriviaQuestions.txt");
                textDocAnswers = System.IO.File.ReadAllLines("algTriviaAnswers.txt");
            }
            else
            {
                this.type = qtype;
                questions = System.IO.File.ReadAllLines("chemTriviaQuestions.txt");
                textDocAnswers = System.IO.File.ReadAllLines("chemTriviaAnswers.txt");
            }
            allQuestions = new Question[questions.Length];
            usedIndexes = new bool[questions.Length];
            for (int i = 0; i < allQuestions.Length; i++)
            {
                allQuestions[i] = new Question(i, questions[i], textDocAnswers[i]);

            }
            this.safeRandomizeIndex();
            AnswerButtons = new Sprite[4];
            AnswerButtons[0] = new Sprite(new Vector2(125, 260), "Trivia\\Buttons\\RegularTemplate");
            AnswerButtons[1] = new Sprite(new Vector2(450, 260), "Trivia\\Buttons\\RegularTemplate");
            AnswerButtons[2] = new Sprite(new Vector2(125, 360), "Trivia\\Buttons\\RegularTemplate");
            AnswerButtons[3] = new Sprite(new Vector2(450, 360), "Trivia\\Buttons\\RegularTemplate");

        }

        /// <summary>
        /// resets the object index to one which hasn't been used yet
        /// </summary>
        public void safeRandomizeIndex()
        {

            usedIndexes[index] = true;
            
            /* checks to see if all answers have been used, and, if so
            resets so that all questions may be used again */
            if (this.completelyUsed())
            {
                this.resetUsedIndexes();
            }

            index = gen.Next(this.getAmountOfQuestions());

            while (usedIndexes[index] == true)
            {
                index = gen.Next(this.getAmountOfQuestions());
            }

            randomizedAnswersFinal = allQuestions[index].setRandomizedAnswerChoices();
        }

        /// <summary>
        /// checks if all questions have been asked at least once in the current set
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// makes every single question "not used"
        /// </summary>
        public void resetUsedIndexes()
        {
            for (int i = 0; i < usedIndexes.Length; i++)
            {
                usedIndexes[i] = false;
            }
        }

        /// <summary>
        /// returns the question's answer as a string
        /// </summary>
        /// <returns></returns>
        public string getAnswer()
        {
            return allQuestions[index].getAnswer();
        }

        /// <summary>
        /// returns question's question as a string
        /// </summary>
        /// <returns></returns>
        public string getQuestion()
        {
            return allQuestions[index].getQuestion();
        }

        /// <summary>
        /// returns an string array of length 4
        /// containing all answer choices for the question at "index"
        /// </summary>
        /// <returns></returns>
        public string[] getDistractors()
        {
            return randomizedAnswersFinal;
        }

        /// <summary>
        /// returns true if the inputed answer string equals
        /// the one of the correct answer
        /// </summary>
        /// <param name="ans"></param>
        /// <returns></returns>
        public bool isAnswerCorrect(String ans)
        {
            return(ans.Equals(allQuestions[index].getAnswer()));
        }

        /// <summary>
        /// returns the entire quesiton object at "index"
        /// </summary>
        /// <param name="ind"></param>
        /// <returns></returns>
        public Question getQuestionObject(int ind)
        {
            return allQuestions[ind];
        }

        /// <summary>
        /// Gets questions and answers.
        /// </summary>
        /// <returns>0 = question, 1-4 = answers</returns>
        public string[] lolBeavers()
        {
            String[] superBeavers = new String[5];
            superBeavers[0] = this.getQuestion();
            String[] otherBeaver = this.getDistractors();
            for (int i = 1; i < 4; i++)
            {
                superBeavers[i] = otherBeaver[i - 1];
            }
            return superBeavers;
        }

        /// <summary>
        ///  returns total amount of questions
        /// </summary>
        /// <returns></returns>
        public int getAmountOfQuestions()
        {
            return allQuestions.Length;
        }

        /// <summary>
        /// returns basics: current trivia category,
        /// questions correct, and questions attempted
        /// </summary>

        public String getInfo()
        {
            String str = "";
            str += "Current Trivia Category: " + type;
                //+ "\nQuestions correct: " + winCount +
                //"\nTotal Questions: " + totalCount;
            return str;
        }

        /// <summary>
        /// returns info concerning the Trivia class
        /// </summary>
        /// <returns></returns>
        public String toString()
        {
            String str = this.getInfo();
            str += "Current Index: " + index +
                "\nCurrent Question: " + this.getQuestion() +
                "\nCurrent Answer: " + this.getAnswer() +
                "\nAll Questions Used: " + this.completelyUsed();
            return str;
        }

        /// <summary>
        /// accessor to get current index
        /// </summary>
        /// <returns>current index</returns>
        public int getIndex()
        {
            return index;
        }

        public void LoadTriviaContent(ContentManager theContentManager)
        {
            this.LoadContent(theContentManager);
            TriviaFont = theContentManager.Load<SpriteFont>("TriviaFont");
            foreach (Sprite s in AnswerButtons)
            {
                s.LoadContent(theContentManager);
            }
        }
        public void DrawTrivia(SpriteBatch theSpriteBatch, ContentManager theContentManager)
        {
            this.Draw(theSpriteBatch, theContentManager);
            foreach (Sprite s in AnswerButtons)
            {
                s.Draw(theSpriteBatch, theContentManager);
            }
            // question
            theSpriteBatch.DrawString(TriviaFont, getQuestion(), new Vector2(150, 100), Color.White);
            // answers
            string[] answers = getDistractors();
            for(int i = 0; i < answers.Length; i++)
            {
                theSpriteBatch.DrawString(TriviaFont, answers[i], AnswerButtons[i].Position + new Vector2(10, 10), Color.White);
                
            }
            
        }
    }

}