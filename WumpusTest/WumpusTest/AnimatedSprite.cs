using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace WumpusTest
{
    class AnimatedSprite : Sprite
    {
        float TotalElapsedTime;
        float TimePerFrame;
        int FrameCount = 4;
        public int FrameNumber = 1;

        bool isPaused = false;
        
        public AnimatedSprite(double framesPerSec, int frameCount, Vector2 Position, string asset)
        {
            this.AssetName = asset;
            this.TimePerFrame = (float)(1 / framesPerSec);
            this.FrameCount = frameCount;
        }

        public void UpdateFrame(float elapsedTime)
        {
            if (isPaused)
                return;
            TotalElapsedTime += elapsedTime;
            if (TotalElapsedTime > TimePerFrame)
            {
                FrameNumber++;
                FrameNumber = (FrameNumber % FrameCount);
                TotalElapsedTime -= TimePerFrame;
            }
        }

        public void DrawAnimated(SpriteBatch theSpriteBatch, ContentManager theContentManager)
        {
            if (Texture == null)
            {
                this.LoadContent(theContentManager);
            }
            int FrameWidth = Texture.Width / FrameCount;
            Rectangle Source = new Rectangle(FrameWidth * FrameNumber, 0, FrameWidth, Texture.Height);
            theSpriteBatch.Draw(Texture, Position, Source, Color.White);
        }
    }
}
