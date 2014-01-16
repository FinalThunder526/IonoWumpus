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
        SoundEffect Attack, Retriet;
        Song Bg, gold, EncWumpus, EncPit, EnterRoom, LeaveRoom, Trivia, ArrowShot, Shoots, highscore, Menu, bats;


        public void LoadContent(ContentManager CM)
        {
            //Bg = CM.Load<Song>("Sound/Background");
            Bg = CM.Load<Song>("Sound/Demons");

            gold = CM.Load<Song>("Sound/coin");
            EncWumpus = CM.Load<Song>("Sound/EncWumpus");
            EncPit = CM.Load<Song>("Sound/EncPit");
            EnterRoom = CM.Load<Song>("Sound/EnterRoom");
            LeaveRoom = CM.Load<Song>("Sound/LeaveRoom");
            Trivia = CM.Load<Song>("Sound/Trivia");
            ArrowShot = CM.Load<Song>("Sound/ArrowShot");
            highscore = CM.Load<Song>("Sound/highscore");
            Shoots = CM.Load<Song>("Sound/Shoot");
            Menu = CM.Load<Song>("Sound/MenuScreen");
            bats = CM.Load<Song>("Sound/Bats");

            Attack = CM.Load<SoundEffect>("Sound/attack");
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

        public void PlayRetriet()
        {
            Retriet.Play();
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
            MediaPlayer.Play(Menu);
        }

        public void PlayBats()
        {
            MediaPlayer.Play(bats);
        }
    }
}