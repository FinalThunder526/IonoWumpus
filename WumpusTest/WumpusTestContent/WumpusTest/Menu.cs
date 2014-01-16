using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace WumpusTest
{
    class Menu : Sprite
    {
        // Startup
        public CircleButton PlayButton, OptionsButton, HighScoresButton, ExitGameButton;
        Sprite Logo;
        // Options
        public Sprite MusicOn, MusicOff, SoundOn, SoundOff, Cheats, Debug, Cave1, Cave2, Cave3, Back;
        Sprite MusicLabel, SoundFXLabel, OptionsLabel, CaveLabel;

        public enum MenuState { Startup, Options1, Options2, HighScores, Credits };
        public MenuState MyMenuState = MenuState.Startup;

        public Menu(Vector2 newPos, string newAsset) : base(newPos, newAsset)
        {
            InitializeStartup();
            InitializeOptions1();
        }

        public void InitializeStartup()
        {
            PlayButton = new CircleButton(300, new Vector2(140, 70), "Menu\\Startup\\playButtonNew");
            OptionsButton = new CircleButton(200, new Vector2(25, 380), "Menu\\Startup\\optionsButtonNew");
            HighScoresButton = new CircleButton(300, new Vector2(460, 70), "Menu\\Startup\\highScoresButtonNew");
            ExitGameButton = new CircleButton(200, new Vector2(675, 380), "Menu\\Startup\\exitButtonNew");
            Logo = new Sprite(new Vector2(275, 325), "Menu\\Startup\\GameIcon3");
        }
        public void InitializeOptions1()
        {
            MusicOn = new Sprite(new Vector2(310, 120), "Menu\\Startup\\Options\\On_Disabled");
            MusicOff = new Sprite(new Vector2(580, 120), "Menu\\Startup\\Options\\Off_Enabled");
            SoundOn = new Sprite(new Vector2(310, 220), "Menu\\Startup\\Options\\On_Disabled");
            SoundOff = new Sprite(new Vector2(580, 220), "Menu\\Startup\\Options\\Off_Enabled");
            Cheats = new Sprite(new Vector2(180, 320), "Menu\\Startup\\Options\\CheatCodesButton");
            Debug = new Sprite(new Vector2(470, 320), "Menu\\Startup\\Options\\DebugMode_Enabled");
            Cave1 = new Sprite(new Vector2(55, 500), "Menu\\Startup\\Options\\Cave1_Enabled");
            Cave2 = new Sprite(new Vector2(325, 500), "Menu\\Startup\\Options\\Cave2_Disabled");
            Cave3 = new Sprite(new Vector2(595, 500), "Menu\\Startup\\Options\\Cave3_Disabled");
            Back = new Sprite(new Vector2(10, 10), "Menu\\Startup\\Options\\BackToMenu");

            OptionsLabel = new Sprite(new Vector2(300, 20), "Menu\\Startup\\Options\\OptionsLabel2");
            MusicLabel = new Sprite(new Vector2(40, 120), "Menu\\Startup\\Options\\Music_Label");
            SoundFXLabel = new Sprite(new Vector2(40, 220), "Menu\\Startup\\Options\\SoundFX_Label");
            CaveLabel = new Sprite(new Vector2(300, 420), "Menu\\Startup\\Options\\Cave_Label");

        }
        public void InitializeOptions2()
        {

        }

        public void Draw(SpriteBatch theSpriteBatch, ContentManager theContentManager)
        {
            if(MyMenuState == MenuState.Startup)
            {
                Logo.Draw(theSpriteBatch, theContentManager);
                PlayButton.Draw(theSpriteBatch, theContentManager);
                OptionsButton.Draw(theSpriteBatch, theContentManager);
                HighScoresButton.Draw(theSpriteBatch, theContentManager);
                ExitGameButton.Draw(theSpriteBatch, theContentManager);
            }
            else if (MyMenuState == MenuState.Options1)
            {
                MusicOn.Draw(theSpriteBatch, theContentManager);
                MusicOff.Draw(theSpriteBatch, theContentManager);
                SoundOn.Draw(theSpriteBatch, theContentManager);
                SoundOff.Draw(theSpriteBatch, theContentManager);
                Cheats.Draw(theSpriteBatch, theContentManager);
                Debug.Draw(theSpriteBatch, theContentManager);
                Cave1.Draw(theSpriteBatch, theContentManager);
                Cave2.Draw(theSpriteBatch, theContentManager);
                Cave3.Draw(theSpriteBatch, theContentManager);
                Back.Draw(theSpriteBatch, theContentManager);

                OptionsLabel.Draw(theSpriteBatch, theContentManager);
                MusicLabel.Draw(theSpriteBatch, theContentManager);
                SoundFXLabel.Draw(theSpriteBatch, theContentManager);
                CaveLabel.Draw(theSpriteBatch, theContentManager);
            }
        }
    }
}

