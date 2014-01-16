using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WumpusTest
{
    class Question
    {
        Random lol;
        int index;
        String question;
        String[] answerChoices;

        /*
         * Every "Question" object has an index, the string of its question, and an array of four answer choices
         * The answer in answerChoices[0] will always be the correct answer
         */

        // stores index of question (from question array), string of the question text,
        // and array of four answer choices
        public Question(int ind, String q, String[] a)
        {
            index = ind;
            question = q;
            answerChoices = new String[4];
            for (int i = 0; i < a.Length; i++)
            {
                answerChoices[i] = a[i];
            }
        }

        // returns the question text of the question
        public String getQuestion()
        {
            return question;
        }

        // returns the answer text of the question
        public String getAnswer()
        {
            return answerChoices[0];
        }

        // returns an array of all answer choices, with the correct one in cell 0
        public String[] getAnswerChoices()
        {
            return answerChoices;
        }

        // returns an array of all answer choices, random cell order
        public String[] getRandomizedAnswerChoices()
        {
            String[] randomized = new String[4];
            bool[] finished = new bool[4];
            for (int i = 0; i < 4; i++)
            {
                int nextIndex = lol.Next(4);
                while(finished[nextIndex] == true) {
                    nextIndex = lol.Next(4);
                }
                randomized[i] = answerChoices[nextIndex];
                finished[nextIndex] = true;
            }
            return randomized;
        }

        // returns true if input string matches answer string, false otherwise
        public bool checkCorrect(String ans)
        {
            if(ans.Equals(answerChoices[0]))
                return true;
            return false;
        }

        public void processQuestionTextLine(String q)
        {
            question = q;
        }

        // takes a line of text document as a String and stores its info
        public void processAnswerTextLine(String text)
        {
            String ans = "";
            int filledIndexes = 0;
            while (filledIndexes <= 3)
            {
                for (int i = 0; i < text.Length; i++)
                {
                    if (text.Substring(i, i + 1) != "~")
                    {
                        ans += text.Substring(i, i + 1);
                    }
                    else
                    {
                        answerChoices[filledIndexes] = ans;
                        ans = "";
                    }

                }
            }
        }

    }
}
