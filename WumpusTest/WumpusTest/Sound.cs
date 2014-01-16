using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace WumpusTest
{
    class Sound
    {
        SoundEffect Attack, Retreat;
        public Song Bg, gold, EncWumpus, EncPit, EnterRoom, LeaveRoom, Trivia, ArrowShot, Shoots, highscore, Menu, bats;


        public void LoadContent(ContentManager CM)
        {
            //Bg = CM.Load<Song>("SoundFiles/Background");
            Bg = CM.Load<Song>("SoundFiles/Demons");

            gold = CM.Load<Song>("SoundFiles/CoinMusic");
            EncWumpus = CM.Load<Song>("SoundFiles/EncWumpusMusic");
            EncPit = CM.Load<Song>("SoundFiles/EncPitMusic");
            EnterRoom = CM.Load<Song>("SoundFiles/EnterRoom");
            LeaveRoom = CM.Load<Song>("SoundFiles/LeaveRoom");
            Trivia = CM.Load<Song>("SoundFiles/Trivia");
            ArrowShot = CM.Load<Song>("SoundFiles/ArrowShotMusic");
            highscore = CM.Load<Song>("SoundFiles/highscore");
            Shoots = CM.Load<Song>("SoundFiles/Shoot");
            Menu = CM.Load<Song>("SoundFiles/MenuScreen");
            bats = CM.Load<Song>("SoundFiles/BatsMusic");

            Attack = CM.Load<SoundEffect>("SoundFiles/attack");
            //retreat sound fx is combined with attack
        }

        public void PlayBg()
        {
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(Bg);
        }

        public void Playcoin()
        {
            MediaPlayer.Play(gold);
        }

        public void PlayAttack()
        {
            Attack.Play();
        }

        public void PlayEnterRoom()
        {
            MediaPlayer.Play(EnterRoom);
        }

        public void PlayRetreat()
        {
            Retreat.Play();
        }

        public void PlayTrivia()
        {
            MediaPlayer.Play(Trivia);
        }

        public void PlayEncWumpus()
        {
            MediaPlayer.Play(EncWumpus);
        }

        public void PlayShoots()
        {
            MediaPlayer.Play(Shoots);
        }

        public void PlayEncPit()
        {
            MediaPlayer.Play(EncPit);
        }

        public void PlayLeaveRoom()
        {
            MediaPlayer.Play(LeaveRoom);
        }

        public void PlayHighScore()
        {
            MediaPlayer.Play(highscore);
        }

        public void PlayMenu()
        {
            if(MediaPlayer.State != MediaState.Playing)
                MediaPlayer.Play(Menu);
        }

        public void PlayBats()
        {
            MediaPlayer.Play(bats);
        }

        public void EndAll()
        {
            Bg.Dispose();
            gold.Dispose();
            EncWumpus.Dispose();
            EncPit.Dispose();
            EnterRoom.Dispose();
            LeaveRoom.Dispose();
            Trivia.Dispose();
            ArrowShot.Dispose();
            Shoots.Dispose();
            highscore.Dispose();
            Menu.Dispose();
            bats.Dispose();

            Attack.Dispose();
        }
    }
}