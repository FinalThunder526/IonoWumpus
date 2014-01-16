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
//using Microsoft.Kinect;

namespace WumpusTest
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        GameControl MyGameControl;
        Sprite SettingsButton,  BG;//, Door;
        AnimatedSprite CursorRight;
        Room CurrentRoom;
        Hud HUD1;
        Menu MyMenu;
        KeyboardState KBState;
        MouseState MState;
        SpriteFont standardFont;
        
        System.Diagnostics.Stopwatch MyWatch = new System.Diagnostics.Stopwatch(), KinectWatch = new System.Diagnostics.Stopwatch();

        const float MaxDepthDistance = 4095; // max distance
        const float MinDepthDistance = 850; // min distance
        const float MaxDepthDistanceOffset = MaxDepthDistance - MinDepthDistance;

        private int thresholdDistance = 50;
        private int movementBuffer = 30;

        enum Controls { Keyboard, Mouse, /*Xbox,*/ Kinect };
        Controls MyControls = Controls.Mouse;
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            InitGraphicsMode(900, 600);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            //KinectSensor.KinectSensors.StatusChanged += new EventHandler<StatusChangedEventArgs>(KinectSensors_StatusChanged);
            //DiscoverKinectSensor();
            
            MyGameControl = new GameControl(new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight));
            HUD1 = new Hud(new Vector2(0, 460), "ScreenLayout\\HUD1_52313");
            SettingsButton = new Sprite(new Vector2(graphics.PreferredBackBufferWidth - 60, 0), "Menu\\SettingsMenuButton");
            CursorRight = new AnimatedSprite(6, 6, Vector2.Zero, "ScreenPointers\\AnimatedCursor6");
            CurrentRoom = new Room(new Vector2(120, 30), 4, 1);//(new Vector2(120, 30), "Rooms\\" + MyGameControl.MyMap.getPlayerRoom().ChamberNumber + "-" + MyGameControl.MyMap.getPlayerRoom().RoomNumber);
            MyGameControl.MyMap.setPlayerRoom(CurrentRoom);
            BG = new Sprite(Vector2.Zero, "ScreenLayout\\BackdropSpace");
            MyMenu = new Menu(Vector2.Zero, "ScreenLayout\\BackdropSpace");
            MyWatch.Reset();

            base.Initialize();
        }
        
        // Kinect Stuff
        /*
        KinectSensor MyKinect;
        Skeleton[] allSkeletons = new Skeleton[6];
        string ConnectedStatus = "";
        
        public bool isScaled = true;
        
        private void DiscoverKinectSensor()
        {
            foreach (KinectSensor sensor in KinectSensor.KinectSensors)
            {
                if (sensor.Status == KinectStatus.Connected)
                {
                    // YAY! We found a connected Kinect! (excuse the loopy pronounciation)
                    MyKinect = sensor;
                    break;
                }
            }

            if (this.MyKinect == null)
            {
                ConnectedStatus = "No Kinect Sensors connected. Bah.";
                return;
            }

            // switch is a thingyjing that is used to do stuff based on what the thing in the brackets is.
            // Sort of like ?:

            switch (MyKinect.Status)
            {
                case KinectStatus.Connected:
                    {
                        ConnectedStatus = "Status: Connected";
                        break;
                    }
                case KinectStatus.Disconnected:
                    {
                        ConnectedStatus = "Status: Disconnected";
                        break;
                    }
                case KinectStatus.NotPowered:
                    {
                        ConnectedStatus = "Status: CONNECT THE DAMN POWER";
                        break;
                    }
                default:
                    {
                        ConnectedStatus = "Status: ERROR. IDIOT.";
                        break;
                    }
            }

            if (MyKinect.Status == KinectStatus.Connected)
            {
                InitializeKinect();
            }
        }
        private bool InitializeKinect()
        {
            var parameters = new TransformSmoothParameters
            {
                Smoothing = 0.5f,
                Correction = 0.5f,
                Prediction = 0.5f,
                JitterRadius = 0.5f,
                MaxDeviationRadius = 0.4f
            };

            MyKinect.AllFramesReady += new EventHandler<AllFramesReadyEventArgs>(MyKinect_AllFramesReady);
            MyKinect.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);
            MyKinect.DepthStream.Enable(DepthImageFormat.Resolution640x480Fps30);
            MyKinect.SkeletonStream.Enable(parameters);

            try
            {
                MyKinect.Start();
                return true;
            }
            catch (MissingMemberException e)
            {
                ConnectedStatus = "UNABLE TO START THE SENSOR. Get A LIfE.";
                return false;
            }    
        }
        void MyKinect_AllFramesReady(object sender, AllFramesReadyEventArgs e)
        {
            Skeleton first = GetFirstSkeleton(e);
            if (first == null)
                return;

            if (MyControls == Controls.Kinect)
            {
                GetCameraPoint(first, e);
                if (isScaled)
                {
                    ScalePosition(CursorRight);
                }
            }
        }
        private void ScalePosition(Sprite Cursor)
        {
            Vector2 CursorPosition = Cursor.Position;
            double x = CursorPosition.X * 1300 / 640 - 115;
            double y = CursorPosition.Y * 800 / 480 - 250;
            Cursor.Origin = new Vector2((float)x, (float)y);
            Cursor.SetPosition();
        }
        private void GetCameraPoint(Skeleton first, AllFramesReadyEventArgs e)
        {
            using (DepthImageFrame depth = e.OpenDepthImageFrame())
            {
                if (depth == null || MyKinect == null)
                {
                    return;
                }

                DepthImagePoint headDepthPoint = depth.MapFromSkeletonPoint(first.Joints[JointType.Head].Position);
                DepthImagePoint leftHandDepthPoint = depth.MapFromSkeletonPoint(first.Joints[JointType.HandLeft].Position);
                DepthImagePoint rightHandDepthPoint = depth.MapFromSkeletonPoint(first.Joints[JointType.HandRight].Position);

                ColorImagePoint headColorPoint = depth.MapToColorImagePoint(headDepthPoint.X, headDepthPoint.Y, ColorImageFormat.RgbResolution640x480Fps30);
                ColorImagePoint leftHandColorPoint = depth.MapToColorImagePoint(leftHandDepthPoint.X, leftHandDepthPoint.Y, ColorImageFormat.RgbResolution640x480Fps30);
                ColorImagePoint rightHandColorPoint = depth.MapToColorImagePoint(rightHandDepthPoint.X, rightHandDepthPoint.Y, ColorImageFormat.RgbResolution640x480Fps30);

                CameraPosition(CursorRight, rightHandColorPoint);
            }
        }
        private void CameraPosition(Sprite MySprite, ColorImagePoint MyColorImagePoint)
        {
            MySprite.Position = new Vector2(MyColorImagePoint.X - MySprite.height / 2, MyColorImagePoint.Y - MySprite.height / 2);
            //MySprite.SetPositionAndCenter();
        }
        private Skeleton GetFirstSkeleton(AllFramesReadyEventArgs e)
        {
            using (SkeletonFrame skeletonFrameData = e.OpenSkeletonFrame())
            {
                if (skeletonFrameData == null)
                {
                    return null;
                }

                skeletonFrameData.CopySkeletonDataTo(allSkeletons);

                Skeleton first = (from s in allSkeletons where s.TrackingState == SkeletonTrackingState.Tracked select s).FirstOrDefault();

                return first;
            }
        }
        

        void KinectSensors_StatusChanged(object sender, StatusChangedEventArgs e)
        {
            if (this.MyKinect == e.Sensor)
            {
                if (e.Status == KinectStatus.Disconnected || e.Status == KinectStatus.NotPowered)
                {
                    this.MyKinect = null;
                    this.DiscoverKinectSensor();
                }
            }
        }
        */
        // End Kinect Stuff

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            MyGameControl.LoadContent(this.Content);
            CurrentRoom.LoadContentRoom(this.Content);
            HUD1.LoadContentHUD(this.Content);
            standardFont = this.Content.Load<SpriteFont>("Font");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        { }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            KBState = Keyboard.GetState();
            //MState = Mouse.GetState();
            if (MyGameControl.GameState == GameControl.Gstate.Menu)
            {
                if (MyControls == Controls.Keyboard || MyControls == Controls.Mouse)
                {
                    if (KBState.IsKeyDown(Keys.A))
                    {
                        CursorRight.AssetName = "ScreenPointers\\AnimatedCursor6";
                        CursorRight.UpdateFrame((float)gameTime.ElapsedGameTime.TotalSeconds);
                    }
                    else
                    {
                        CursorRight.AssetName = "ScreenPointers\\Cursor";
                        CursorRight.FrameNumber = 0;
                    }
                    UpdateMouseClicks(MState);
                }
                else if (MyControls == Controls.Kinect)
                {
                    UpdateKinectClicks();
                }

            }

            if (MyGameControl.GameState == GameControl.Gstate.Game)
            {
                // Input:
                //      Keyboard
                if (MyControls == Controls.Keyboard)
                {
                    UpdateKBMovement(KBState);
                }
                //      Keyboard and Mouse
                if (MyControls != Controls.Kinect)
                {
                    UpdateMouseCursor();
                }
                //      Kinect and Mouse
                if (MyControls != Controls.Keyboard)
                {
                    UpdateMovement();
                }
                //      Mouse, Kinect, and Keyboard
                UpdateAngle();
                
                
                
                // Entering rooms
                if (isInNSThreshold() || isInEWThreshold())
                {
                    CurrentRoom = MyGameControl.moveInDirection((GameControl.Direction)(int)(getDirectionFromXY(MyGameControl.MyPlayer.Position)));
                    /*CurrentRoom.AssetName = "Rooms\\" + MyGameControl.MyMap.getPlayerRoom().ChamberNumber + "-" + MyGameControl.MyMap.getPlayerRoom().RoomNumber;
                    CurrentRoom.LoadContent(this.Content);*/
                }
                // Full Screen mode
                if (KBState.IsKeyDown(Keys.F11))
                {
                    if (graphics.IsFullScreen)
                        graphics.IsFullScreen = false;
                    else
                        graphics.IsFullScreen = true;
                    graphics.ApplyChanges();
                }
            }
            base.Update(gameTime);

            float myTheta = MyGameControl.MyPlayer.newTheta;
        }

        /// <summary>
        /// Checks whether the cursor is over a given set of buttons.
        /// </summary>
        /// <param name="p">The current menu.</param>
        /// <returns>Whether the cursor is over the buttons or not.</returns>
        private bool isOverButtons(string p)
        {
            Vector2 MyVec = CursorRight.Origin;
            if (p == "Startup")
            {
                return MyMenu.PlayButton.isOver(MyVec) || MyMenu.ExitGameButton.isOver(MyVec) || MyMenu.OptionsButton.isOver(MyVec)
                    || MyMenu.HighScoresButton.isOver(MyVec);
            }
            else if (p == "Options")
            {
                return MyMenu.Cave1.isOver(MyVec) || MyMenu.Cave2.isOver(MyVec) || MyMenu.Cave3.isOver(MyVec)
                    || MyMenu.MusicOn.isOver(MyVec) || MyMenu.MusicOff.isOver(MyVec) || MyMenu.SoundOn.isOver(MyVec)
                    || MyMenu.SoundOff.isOver(MyVec) || MyMenu.Debug.isOver(MyVec) || MyMenu.Cheats.isOver(MyVec)
                    || MyMenu.Back.isOver(MyVec);
            }
            else
                return false;
        }
        
        
        /// <summary>
        /// Checks if the player is within the threshold for N or S. Does NOT check Y value, only checks X.
        /// Also, has an IF-block for situations where there are no N or S doors.
        /// </summary>
        /// <returns>Whether the player is in the threshold.</returns>
        private bool isInNSThreshold()
        {
            if (CurrentRoom.RoomNumber == 0 || CurrentRoom.RoomNumber == 1 || CurrentRoom.RoomNumber == 3)
                return MyGameControl.MyPlayer.Position.X > CurrentRoom.Doors[1].Position.X && MyGameControl.MyPlayer.Position.X /*+ CenteredSprite.height*/ < CurrentRoom.Doors[1].Position.X + CurrentRoom.Doors[1].Texture.Width && MyGameControl.isPlayerAtWall;
            else
                return false;
        }
        /// <summary>
        /// Similar to NS, only E or W. Does not check X value, only checks Y. Also includes the IF-block.
        /// </summary>
        /// <returns>Whether the player is in the threshold.</returns>
        private bool isInEWThreshold()
        {
            if (CurrentRoom.RoomNumber == 0 || CurrentRoom.RoomNumber == 2 || CurrentRoom.RoomNumber == 4)
                return MyGameControl.MyPlayer.Position.Y > CurrentRoom.Doors[4].Position.Y && MyGameControl.MyPlayer.Position.Y /*+ CenteredSprite.height*/ < CurrentRoom.Doors[4].Position.Y + CurrentRoom.Doors[4].Texture.Height && MyGameControl.isPlayerAtWall;
            else
                return false;
        }

        /// <summary>
        /// Method that handles the angle of the player based on the current position of CursorRight.
        /// Integrates Mouse and Kinect movement. ;) Bazinga.
        /// </summary>
        private void UpdateAngle()
        {
            float myTheta = MyGameControl.MyPlayer.theta;
            
            // Direction is the vector representing the x and y difference between the player's current position and
            // the mouse's position.
            
            Vector2 Direction = new Vector2(
                MyGameControl.MyPlayer.Position.X + CenteredSprite.height / 2 - (CursorRight.Position.X + Sprite.stdHeight / 2),
                MyGameControl.MyPlayer.Position.Y + CenteredSprite.height / 2 - (CursorRight.Position.Y + Sprite.stdHeight / 2));

            
            // Setting the angle of rotation of the player to ArcTan of the difference.
            MyGameControl.MyPlayer.theta = (float)Math.Atan(Direction.Y / Direction.X);
            // This basically makes the value of theta range from -PI/2 to 3PI/2.
            if (Direction.X > 0)
            {
                MyGameControl.MyPlayer.theta += (float)Math.PI;
            }
            MyGameControl.MyPlayer.theta = (MyGameControl.MyPlayer.theta > (float)Math.PI * 2) ? MyGameControl.MyPlayer.theta - 2 * (float)Math.PI : MyGameControl.MyPlayer.theta;
            // newTheta ranges from 0 to 2PI.
            MyGameControl.MyPlayer.newTheta = (float)((MyGameControl.MyPlayer.theta + (2 * Math.PI)) % (2 * Math.PI));

        }

        /// <summary>
        /// Handles the movement of the player based on the current position of CursorRight.
        /// Integrates Mouse and Kinect too.
        /// </summary>
        private void UpdateMovement()
        {
            // This eradicates the jittery jerks of the player when it actually reaches the mouse's position.
            // If the player is within a radius of a certain number of pixels, it will stop moving.

            // TODO: Edit so that the thingamadoodle goes to the center, not the corner.
            if (Math.Abs((int)CursorRight.Position.X - (int)MyGameControl.MyPlayer.Position.X) > thresholdDistance || Math.Abs((int)CursorRight.Position.Y - (int)MyGameControl.MyPlayer.Position.Y) > thresholdDistance)
            {
                // The version of MovePlayer involving the mouse.
                MyGameControl.MoveNow();
            }
        }


        /// <summary>
        /// Handles all clicks by Kinect.
        /// </summary>
        private void UpdateKinectClicks()
        {
            if(MyGameControl.GameState == GameControl.Gstate.Menu && MyMenu.MyMenuState == Menu.MenuState.Startup)
            {
                if (isOverButtons("Startup"))
                {
                    if (!KinectWatch.IsRunning)
                        KinectWatch.Start();
                    if (KinectWatch.ElapsedMilliseconds > 1200)
                    {
                        StartupClickSequence(CursorRight.Position);
                        MyWatch.Reset();
                    }
                }
                else
                    if (KinectWatch.IsRunning)
                        KinectWatch.Reset();
            }
            else if (MyGameControl.GameState == GameControl.Gstate.Menu && MyMenu.MyMenuState == Menu.MenuState.Options)
            {
                if (isOverButtons("Options"))
                {
                    if (!KinectWatch.IsRunning)
                        KinectWatch.Start();
                    if (KinectWatch.ElapsedMilliseconds > 1200)
                    {
                        OptionsClickSequence(CursorRight.Position);
                        MyWatch.Reset();
                    }
                }
                else
                    if (KinectWatch.IsRunning)
                        KinectWatch.Reset();
            }
        }

        /// <summary>
        /// Handles the clicking at the Options menu
        /// </summary>
        /// <param name="Pos">Position of the cursor.</param>
        private void OptionsClickSequence(Vector2 Pos)
        {
            // Debug Button
            if (MyMenu.Debug.isOver(Pos))
            {
                DebugButtonClick();
            }
            // Music Off
            else if (MyMenu.MusicOff.isOver(Pos))
            {
                MusicOffClick();
            }
            // Music On
            else if (MyMenu.MusicOn.isOver(Pos))
            {
                MusicOnClick();
            }
            // Sound Off
            else if (MyMenu.SoundOff.isOver(Pos))
            {
                SoundOffClick();
            }
            // Sound On
            else if (MyMenu.SoundOn.isOver(Pos))
            {
                SoundOnClick();
            }
            // Cave 1
            else if (MyMenu.Cave1.isOver(Pos))
            {
                Cave1Click();
            }
            // Cave 2
            else if (MyMenu.Cave2.isOver(Pos))
            {
                Cave2Click();
            }
            // Cave 3
            else if (MyMenu.Cave3.isOver(Pos))
            {
                Cave3Click();
            }
            // Back to Main Menu
            else if (MyMenu.Back.isOver(Pos))
            {
                MyMenu.MyMenuState = Menu.MenuState.Startup;
            }
        }

        /// <summary>
        /// Handles everything to do with mouse clicks. Mostly the startup and other menus.
        /// Includes a call to UpdateMouseCursor().
        /// </summary>
        /// <param name="OldState">The old state of the mouse.</param>
        private void UpdateMouseClicks(MouseState OldState)
        {
            MouseState NewState = Mouse.GetState();
            UpdateMouseCursor();
            Vector2 MousePos = new Vector2(NewState.X, NewState.Y);
            if (NewState.LeftButton == ButtonState.Pressed && OldState.LeftButton == ButtonState.Released)
            {
                if(MyMenu.MyMenuState == Menu.MenuState.Startup)
                {
                    StartupClickSequence(MousePos);
                }
                else if (MyMenu.MyMenuState == Menu.MenuState.Options)
                {
                    OptionsClickSequence(MousePos);
                }
            }
        }

        private void StartupClickSequence(Vector2 Pos)
        {
            // Play Button
            if (MyMenu.PlayButton.isOver(Pos))
            {
                PlayButtonClick();
            }
            // Exit Button
            else if (MyMenu.ExitGameButton.isOver(Pos))
            {
                ExitButtonClick();
            }
            // Options Button
            else if (MyMenu.OptionsButton.isOver(Pos))
            {
                OptionsButtonClick();
            }
        }

        /// <summary>
        /// All the click method sequences
        /// </summary>
        private void PlayButtonClick()
        {
            MyGameControl.GameState = GameControl.Gstate.Game;
            MyGameControl.InitializeGameMode();
            MyWatch.Start();
        }
        private void ExitButtonClick()
        {
            Exit();
        }
        private void OptionsButtonClick()
        {
            MyMenu.MyMenuState = Menu.MenuState.Options;
        }
        private void DebugButtonClick()
        {
            if (MyGameControl.isDebugMode)
            {
                MyGameControl.isDebugMode = false;
                MyMenu.Debug.AssetName = "Menu\\Startup\\Options\\DebugMode_Disabled";
                MyMenu.Debug.LoadContent(this.Content);
            }
            else
            {
                MyGameControl.isDebugMode = true;
                MyMenu.Debug.AssetName = "Menu\\Startup\\Options\\DebugMode_Enabled";
                MyMenu.Debug.LoadContent(this.Content);
            }
        }
        private void MusicOffClick()
        {
            if (MyGameControl.isMusicOn)
            {
                MyGameControl.isMusicOn = false;
                MyMenu.MusicOff.AssetName = "Menu\\Startup\\Options\\Off_Enabled";
                MyMenu.MusicOff.LoadContent(this.Content);
                MyMenu.MusicOn.AssetName = "Menu\\Startup\\Options\\On_Disabled";
                MyMenu.MusicOn.LoadContent(this.Content);
            }
        }
        private void MusicOnClick()
        {
            if (!MyGameControl.isMusicOn)
            {
                MyGameControl.isMusicOn = true;
                MyMenu.MusicOn.AssetName = "Menu\\Startup\\Options\\On_Enabled";
                MyMenu.MusicOn.LoadContent(this.Content);
                MyMenu.MusicOff.AssetName = "Menu\\Startup\\Options\\Off_Disabled";
                MyMenu.MusicOff.LoadContent(this.Content);
            }
        }
        private void SoundOffClick()
        {
            if (MyGameControl.isSoundOn)
            {
                MyGameControl.isSoundOn = false;
                MyMenu.SoundOff.AssetName = "Menu\\Startup\\Options\\Off_Enabled";
                MyMenu.SoundOff.LoadContent(this.Content);
                MyMenu.SoundOn.AssetName = "Menu\\Startup\\Options\\On_Disabled";
                MyMenu.SoundOn.LoadContent(this.Content);
            }
        }
        private void SoundOnClick()
        {
            if (!MyGameControl.isSoundOn)
            {
                MyGameControl.isSoundOn = true;
                MyMenu.SoundOn.AssetName = "Menu\\Startup\\Options\\On_Enabled";
                MyMenu.SoundOn.LoadContent(this.Content);
                MyMenu.SoundOff.AssetName = "Menu\\Startup\\Options\\Off_Disabled";
                MyMenu.SoundOff.LoadContent(this.Content);
            }
        }
        private void Cave1Click()
        {
            if (MyGameControl.caveN != 1)
            {
                MyGameControl.caveN = 1;
                MyMenu.Cave1.AssetName = "Menu\\Startup\\Options\\Cave1_Enabled";
                MyMenu.Cave1.LoadContent(this.Content);
                MyMenu.Cave2.AssetName = "Menu\\Startup\\Options\\Cave2_Disabled";
                MyMenu.Cave3.AssetName = "Menu\\Startup\\Options\\Cave3_Disabled";
                MyMenu.Cave2.LoadContent(this.Content);
                MyMenu.Cave3.LoadContent(this.Content);
            }
        }
        private void Cave2Click()
        {
            if (MyGameControl.caveN != 2)
            {
                MyGameControl.caveN = 2;
                MyMenu.Cave2.AssetName = "Menu\\Startup\\Options\\Cave2_Enabled";
                MyMenu.Cave2.LoadContent(this.Content);
                MyMenu.Cave1.AssetName = "Menu\\Startup\\Options\\Cave1_Disabled";
                MyMenu.Cave3.AssetName = "Menu\\Startup\\Options\\Cave3_Disabled";
                MyMenu.Cave1.LoadContent(this.Content);
                MyMenu.Cave3.LoadContent(this.Content);
            }
        }
        private void Cave3Click()
        {
            if (MyGameControl.caveN != 3)
            {
                MyGameControl.caveN = 3;
                MyMenu.Cave3.AssetName = "Menu\\Startup\\Options\\Cave3_Enabled";
                MyMenu.Cave3.LoadContent(this.Content);
                MyMenu.Cave2.AssetName = "Menu\\Startup\\Options\\Cave2_Disabled";
                MyMenu.Cave1.AssetName = "Menu\\Startup\\Options\\Cave1_Disabled";
                MyMenu.Cave2.LoadContent(this.Content);
                MyMenu.Cave1.LoadContent(this.Content);
            }
        }


        /// <summary>
        /// Just updates the cursor to the position of the mouse.
        /// </summary>
        private void UpdateMouseCursor()
        {
            MState = Mouse.GetState();
            CursorRight.Origin = new Vector2(MState.X, MState.Y);
            CursorRight.SetPosition();
            CursorRight.LoadContent(this.Content);
        }

        
        /// <summary>
        /// Updates the position of the player based on the up, down, left, and right keys.
        /// </summary>
        /// <param name="theKBState">The current state of the Keyboard.</param>
        private void UpdateKBMovement(KeyboardState theKBState)
        {
            if (theKBState.IsKeyDown(Keys.Up))
                MyGameControl.MovePlayer(0, -(int)(MyGameControl.MyPlayer.Velocity));
            if (theKBState.IsKeyDown(Keys.Right))
                MyGameControl.MovePlayer((int)(MyGameControl.MyPlayer.Velocity), 0);
            if (theKBState.IsKeyDown(Keys.Down))
                MyGameControl.MovePlayer(0, (int)(MyGameControl.MyPlayer.Velocity));
            if (theKBState.IsKeyDown(Keys.Left))
                MyGameControl.MovePlayer(-(int)(MyGameControl.MyPlayer.Velocity), 0);
        }

        public enum Direction { None, North, East, South, West };

        /// <summary>
        /// Used to determine which wall the player is at (when the playerIsAtWall is true)
        /// </summary>
        /// <param name="Position">The Position of the Player</param>
        /// <returns>Which direction the Player is going to move.</returns>
        public Direction getDirectionFromXY(Vector2 Position)
        {
            if (Position.X < MyGameControl.RoomSize.Left + thresholdDistance)
                return Direction.West;
            else if (Position.X > MyGameControl.RoomSize.Right - thresholdDistance)
                return Direction.East;
            else if (Position.Y < MyGameControl.RoomSize.Top + thresholdDistance)
                return Direction.North;
            else if (Position.Y > MyGameControl.RoomSize.Bottom - thresholdDistance)
                return Direction.South;
            else
                return Direction.None;
        }
        
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();

            // All the sprites
            BG.Draw(spriteBatch, this.Content);
            if (MyGameControl.GameState == GameControl.Gstate.Game)
            {
                CurrentRoom.DrawRoom(spriteBatch, this.Content);
                //Door.Draw(spriteBatch, this.Content); 
                MyGameControl.Draw(spriteBatch, this.Content);
                HUD1.DrawHUD(spriteBatch, this.Content);
                SettingsButton.Draw(spriteBatch, this.Content);
                
            }
            else if (MyGameControl.GameState == GameControl.Gstate.Menu)
            {
                MyMenu.Draw(spriteBatch, this.Content);
            }
            
            // Debugging purposes: Text drawing
            DrawText();
            
            // And, of course, the cursor
            if(KBState.IsKeyDown(Keys.A))
                CursorRight.DrawAnimated(spriteBatch, this.Content);
            else
                CursorRight.Draw(spriteBatch, this.Content);

            spriteBatch.End();
            base.Draw(gameTime);
        }

        private void DrawText()
        {
            if (MyGameControl.isDebugMode)
            {
                string Debug = "Cursor: " + CursorRight.Origin;
                Debug += "\nMouse: " + MState;
                Debug += "\nPlayer: " + MyGameControl.MyPlayer.Position;
                Debug += "\nIsOverButton: " + this.isOverButtons("Startup");
                Debug += "\nKinectWatch: " + KinectWatch.ElapsedMilliseconds;
                Debug += "\nWumpusRoom: " + MyGameControl.MyMap.getWumpusRoom();
                spriteBatch.DrawString(standardFont, Debug, new Vector2(10, 470), Color.White);
            }

            if (MyGameControl.GameState == GameControl.Gstate.Game)
            {
                spriteBatch.DrawString(standardFont, MyGameControl.MyMap.getPlayerRoom().ToString(), new Vector2(MyGameControl.RoomSize.Center.X, MyGameControl.RoomSize.Center.Y), Color.White);

                // ADD SCORE
                spriteBatch.DrawString(standardFont, "Time: " + ((MyWatch.Elapsed.TotalSeconds)) /*+ "." +  (MyWatch.ElapsedMilliseconds % 1000)/100*/, new Vector2(460, 550), Color.White);
                spriteBatch.DrawString(standardFont, "Moves: " + MyGameControl.MyMap.moves, new Vector2(660, 550), Color.White);
                spriteBatch.DrawString(standardFont, MyGameControl.MyPlayer.getGold() + "", new Vector2(HUD1.GoldIcon.Position.X + HUD1.GoldIcon.Texture.Width + 15, HUD1.GoldIcon.Position.Y), Color.White);
                spriteBatch.DrawString(standardFont, MyGameControl.MyPlayer.getArrows() + "", new Vector2(HUD1.ArrowIcon.Position.X + HUD1.ArrowIcon.Texture.Width + 15, HUD1.ArrowIcon.Position.Y), Color.White);
            }

            //else if (MyMenu.MyMenuState == Menu.MenuState.Options && MyGameControl.GameState == GameControl.Gstate.Menu)
                
        }

        /// <summary>
        /// Sets the game window to the specified height and width
        /// </summary>
        /// <param name="iWidth">New width</param>
        /// <param name="iHeight">New height</param>
        private void InitGraphicsMode(int iWidth, int iHeight)
        {
            graphics.PreferredBackBufferHeight = iHeight;
            graphics.PreferredBackBufferWidth = iWidth;
            graphics.ApplyChanges();
        }

        
    }
}
