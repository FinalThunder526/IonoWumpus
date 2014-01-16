using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WumpusTest
{
    class Player : CenteredSprite
    {
        private int gold;
        private int arrows;
        private int turns;
        private int score;

        public float Velocity;
        public Vector2 Origin;
        private bool canMove;
        public float newTheta;

        public Player()
        {
            gold = 0;
            arrows = 0;
            turns = 0;
            score = 0;
        }
        public Player(Vector2 NewPos, string NewAsset, int gold, int arrows, int turns, int score, float NewVel)
            : base(NewPos, NewAsset)
        {
            this.gold = gold;
            this.arrows = arrows;
            this.turns = turns;
            this.score = score;
            this.Velocity = NewVel;
        }

        //accessors
        public int getArrows()
        {
            return arrows;
        }
        public int getGold()
        {
            return gold;
        }
        public int getTurns()
        {
            return turns;
        }

        // calculates score based on turns, gold, arrows
        public int getScore()
        {
            score = 100 - turns + gold + (10 * arrows);
            return score;
        }

        //mutators
        public void setArrows(int arrows)
        {
            this.arrows = arrows;
        }
        public void setGold(int gold)
        {
            this.gold = gold;
        }
        public void setTurns(int turns)
        {
            this.turns = turns;
        }
        public void setScore(int score)
        {
            this.score = score;
        }

        // called from GC when player moves
        public void addTurn()
        {
            turns++;
        }

        public void LoadContent(ContentManager theContentManager)
        {
            Origin.X = height / 2;
            Origin.Y = height / 2;
            
            Texture = theContentManager.Load<Texture2D>(AssetName);
        }
        public void Draw(SpriteBatch theSpriteBatch, ContentManager theContentManager)
        {
            if (Texture == null)
            {
                LoadContent(theContentManager);
            }

            theSpriteBatch.Draw(Texture, Position,
                 new Rectangle(0, 0, Texture.Width, Texture.Height), Color.White,
                 theta, Origin, 1.0f, SpriteEffects.None, 0);
        }


        

        public bool getCanMove()
        {
            return canMove;
        }
        public void setCanMove(bool newCanMove)
        {
            canMove = newCanMove;
        }
    }
}
