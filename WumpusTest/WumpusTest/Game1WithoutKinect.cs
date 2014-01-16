using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
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
    public class Game1WithoutKinect : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // Various instance variables
        GameControl MyGameControl;
        Sprite SettingsButton, BG, Arrows, Move, Skip, Story;
        AnimatedSprite CursorRight, CursorLeft;
        Room CurrentRoom;
        Hud HUD1;
        Menu MyMenu;
        KeyboardState KBState;
        MouseState MState;
        SpriteFont DebugFont, HUDFont;

        private bool isAnimated = false;
        private bool winningMessage;
        public bool correctDraw;

        // Three watches for timed results:
        Stopwatch MyWatch = new Stopwatch(), KinectWatch = new Stopwatch(), TempWatch = new Stopwatch(), StoryWatch = new Stopwatch();

        const float MaxDepthDistance = 4095; // max mm
        const float MinDepthDistance = 850; // min mm
        const float MaxDepthDistanceOffset = MaxDepthDistance - MinDepthDistance;

        private int thresholdDistance = 50;
        private int movementBuffer = 30;

        private bool DebugModeChanged;
        
        enum Controls { Keyboard, Mouse, /*Xbox,*/ Kinect };
        Controls MyControls = Controls.Mouse;

        public Game1WithoutKinect()
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
            SettingsButton = new Sprite(Vector2.Zero, "Menu\\SettingsMenuButton");
            Arrows = new Sprite(new Vector2(0, 460 - 50), "Arrows\\ArrowButton2");
            Move = new Sprite(new Vector2(0, 460 - 250), "Arrows\\MoveButton");
            Skip = new Sprite(new Vector2(0, 0), "Skip");
            CursorRight = new AnimatedSprite(16/1.2, 16, Vector2.Zero, "ScreenPointers\\AnimatedCursor16");
            CursorLeft = new AnimatedSprite(16 / 1.2, 16, Vector2.Zero, "ScreenPointers\\AnimatedCursor16");
            Story = new Sprite();
            MyGameControl.RandomizeRoom('p');
            MyGameControl.StartingRoom = MyGameControl.MyMap.getPlayerRoom();
            CurrentRoom = MyGameControl.StartingRoom;//(new Vector2(120, 30), "Rooms\\" + MyGameControl.MyMap.getPlayerRoom().ChamberNumber + "-" + MyGameControl.MyMap.getPlayerRoom().RoomNumber);
            //CurrentRoom.setIsPitted(true);

            MyGameControl.RandomizeRoom('w');
            
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
            // Smoothing
            var parameters = new TransformSmoothParameters
            {
                Smoothing = 0.5f,
                Correction = 0.7f,
                Prediction = 0.5f,
                JitterRadius = 0.5f,
                MaxDeviationRadius = 0.4f
            };
            // Events
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
                
                ConnectedStatus = "UNABLE TO START THE SENSOR. Lol.";
                return false;
            }    
        }
        void MyKinect_AllFramesReady(object sender, AllFramesReadyEventArgs e)
        {
            // Get first player.
            Skeleton first = GetFirstSkeleton(e);
            if (first == null)
                return;

            if (MyControls == Controls.Kinect)
            {
                GetCameraPoint(first, e);
                if (isScaled)
                {
                    ScalePosition(CursorRight);
                    ScalePosition(CursorLeft);
                }
            }
        }
        */
        /// <summary>
        /// Scales 640x480 to desired mapping
        /// </summary>
        /// <param name="Cursor">Sprite to scale.</param>
        private void ScalePosition(Sprite Cursor)
        {
            Vector2 CursorPosition = Cursor.Position;
            double x = CursorPosition.X * 1500 / 640 - 115;
            double y = CursorPosition.Y * 900 / 480 - 250;
            Cursor.Origin = new Vector2((float)x, (float)y);
            Cursor.SetPosition();
        }
        /// <summary>
        /// The actual "Kinect" logic.
        /// </summary>
        /// <param name="first">Player controlling program.</param>
        /// <param name="e">All frames ready.</param>
        /*
        private void GetCameraPoint(Skeleton first, AllFramesReadyEventArgs e)
        {
            using (DepthImageFrame depth = e.OpenDepthImageFrame())
            {
                if (depth == null || MyKinect == null)
                {
                    return;
                }

                // Tracks head, just in case.
                DepthImagePoint headDepthPoint = depth.MapFromSkeletonPoint(first.Joints[JointType.Head].Position);
                DepthImagePoint leftHandDepthPoint = depth.MapFromSkeletonPoint(first.Joints[JointType.HandLeft].Position);
                DepthImagePoint rightHandDepthPoint = depth.MapFromSkeletonPoint(first.Joints[JointType.HandRight].Position);
                // Depth to color
                ColorImagePoint headColorPoint = depth.MapToColorImagePoint(headDepthPoint.X, headDepthPoint.Y, ColorImageFormat.RgbResolution640x480Fps30);
                ColorImagePoint leftHandColorPoint = depth.MapToColorImagePoint(leftHandDepthPoint.X, leftHandDepthPoint.Y, ColorImageFormat.RgbResolution640x480Fps30);
                ColorImagePoint rightHandColorPoint = depth.MapToColorImagePoint(rightHandDepthPoint.X, rightHandDepthPoint.Y, ColorImageFormat.RgbResolution640x480Fps30);

                CameraPosition(CursorRight, rightHandColorPoint);
                CameraPosition(CursorLeft, leftHandColorPoint);
            }
        }
        private void CameraPosition(Sprite MySprite, ColorImagePoint MyColorImagePoint)
        {
            // Centering
            MySprite.Position = new Vector2(MyColorImagePoint.X - MySprite.height / 2, MyColorImagePoint.Y - MySprite.height / 2);
        }
        private Skeleton GetFirstSkeleton(AllFramesReadyEventArgs e)
        {
            // Getting the first player among those playing.
            // To avoid confusion.
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
        /// <summary>
        /// If a new Kinect is introduced.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        
        // End Kinect Stuff
        */
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
            DebugFont = this.Content.Load<SpriteFont>("Debug");
            HUDFont = this.Content.Load<SpriteFont>("HUDFont");
            //TriviaFont = this.Content.Load<SpriteFont>("TriviaFont");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            MyGameControl.MySound.EndAll();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            KBState = Keyboard.GetState();

            // In Menu
            if (MyGameControl.GameState == GameControl.Gstate.Menu)
            {
                //if (MyGameControl.isMusicOn)
                //    MyGameControl.MySound.PlayMenu();
                //else
                //    MediaPlayer.Stop();
                if (MyControls == Controls.Keyboard || MyControls == Controls.Mouse)
                {
                    // Animating
                    if (KBState.IsKeyDown(Keys.A))
                    {
                        CursorRight.AssetName = "ScreenPointers\\AnimatedCursor16";
                        CursorRight.UpdateFrame((float)gameTime.ElapsedGameTime.TotalSeconds);
                    }
                    else
                    {
                        CursorRight.AssetName = "ScreenPointers\\Cursor2";
                        CursorRight.FrameNumber = 0;
                    }
                    // All mouse clicks related to the menu
                    UpdateMouseClicks(MState);
                }
                    /*
                else if (MyControls == Controls.Kinect)
                {
                    CursorRight.AssetName = "ScreenPointers\\Cursor2";
                    UpdateKinectClicks(gameTime);
                }
                     */

            }
            else if (MyGameControl.GameState == GameControl.Gstate.PreGame)
            {
                UpdateMouseClicks(MState);
            }
            // In Game
            if (MyGameControl.GameState == GameControl.Gstate.Game)
            {
                // Input:
                //      Mouse
                if (MyControls == Controls.Mouse)
                {
                    UpdateMouseClicks(MState);

                    if (KBState.IsKeyDown(Keys.Space))
                    {
                        MyGameControl.PlayerMode = GameControl.PlayerState.Arrow;
                    }
                    else if (!MyGameControl.isTrivia)
                    {
                        MyGameControl.PlayerMode = GameControl.PlayerState.Movement;
                    }

                    // Shooting
                    if (MyGameControl.PlayerMode == GameControl.PlayerState.Arrow)
                    {
                        MyGameControl.MyPlayer.MyArrow.theta = MyGameControl.MyPlayer.theta;
                        if(MyGameControl.isShooting)
                            MyGameControl.MyPlayer.ShootArrow();
                        if (!MyGameControl.canMove(MyGameControl.MyPlayer.MyArrow, 0, 0, CurrentRoom) && MyGameControl.isShooting)
                        {
                            MyGameControl.shoot(getDirectionFromMouse(new Vector2(MState.X, MState.Y)));
                            MyGameControl.isShooting = false;
                        }
                    }
                    if(!MyGameControl.isShooting || !KBState.IsKeyDown(Keys.Space))
                        MyGameControl.MyPlayer.ResetArrow();
                }
                //      Keyboard
                if (MyControls == Controls.Keyboard)
                {
                    if (MyGameControl.PlayerMode == GameControl.PlayerState.Movement)
                    {
                        UpdateKBMovement(KBState);
                    }
                    if (!MyGameControl.isShooting || !KBState.IsKeyDown(Keys.Space))
                        MyGameControl.MyPlayer.ResetArrow();
                    
                    UpdateMouseClicks(MState);
                }
                //      Kinect and Mouse
                if (MyControls != Controls.Keyboard)
                {
                    if (MyGameControl.PlayerMode == GameControl.PlayerState.Movement)
                    {
                        UpdateMovement();
                    }
                }
                // Just Kinect
                /*
                if (MyControls == Controls.Kinect)
                {
                    if (MyGameControl.PlayerMode == GameControl.PlayerState.Arrow)
                    {
                        MyGameControl.MyPlayer.MyArrow.theta = MyGameControl.MyPlayer.theta;
                        if (MyGameControl.isShooting)
                            MyGameControl.MyPlayer.ShootArrow();
                        if (!MyGameControl.canMove(MyGameControl.MyPlayer.MyArrow, 0, 0, CurrentRoom) && MyGameControl.isShooting)
                        {
                            MyGameControl.shoot(getDirectionFromArrow());
                            MyGameControl.isShooting = false;
                        }
                    }
                    if (!MyGameControl.isShooting)
                        MyGameControl.MyPlayer.ResetArrow();

                    UpdateKinectClicks(gameTime);
                }
                 */
                
                //      Mouse, Kinect, and Keyboard
                UpdateAngle();
                if (MyGameControl.PlayerMode == GameControl.PlayerState.Trivia)
                {
                    MyWatch.Stop();
                }
                else if(!MyWatch.IsRunning)
                {
                    MyWatch.Start();
                }


                // Entering rooms
                if ((isInNSThreshold() || isInEWThreshold()) && MyGameControl.PlayerMode != GameControl.PlayerState.Trivia)
                {
                    MyGameControl.isPlayerAtDoor = true;
                    CurrentRoom = MyGameControl.moveInDirection((GameControl.Direction)(int)(getDirectionFromXY(MyGameControl.MyPlayer.Position)));
                    MyGameControl.MyPlayer.setScore(MyGameControl.hs.scoreCalculator(MyGameControl.MyMap.moves, MyGameControl.MyPlayer.getGold(), MyGameControl.MyPlayer.getArrows()));
                    /*CurrentRoom.AssetName = "Rooms\\" + MyGameControl.MyMap.getPlayerRoom().ChamberNumber + "-" + MyGameControl.MyMap.getPlayerRoom().RoomNumber;
                    CurrentRoom.LoadContent(this.Content);*/
                }
                else
                {
                    MyGameControl.isPlayerAtDoor = false;
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

                if (MyGameControl.shouldResetRoom)
                {
                    CurrentRoom = MyGameControl.MyMap.getPlayerRoom();
                    //MyGameControl.shouldResetRoom = false;
                }
            }
            else if (MyGameControl.GameState == GameControl.Gstate.EndGame)
            {
                UpdateMouseCursor();
                if (MyMenu.ExitGameButton.isOver(new Vector2(MState.X, MState.Y)) && MState.LeftButton == ButtonState.Pressed)
                    ExitButtonClick();
            }
            base.Update(gameTime);

            float myTheta = MyGameControl.MyPlayer.newTheta;
        }

        
        //private void UpdateIngameMouseClicks(MouseState OldState)
        //{
        //    MouseState NewState = Mouse.GetState();
        //    if (NewState.LeftButton == ButtonState.Pressed && OldState.LeftButton == ButtonState.Released)
        //    {
        //        MyGameControl.shoot(getDirectionFromMouse(NewState));
        //    }
        //}

        /// <summary>
        /// Returns which direction the mouse is currently pointing.
        /// </summary>
        /// <param name="State">Mouse state</param>
        /// <returns>Direction to currently go</returns>
        private Direction getDirectionFromMouse(Vector2 State)
        {
            for (int i = 0; i < CurrentRoom.Doors.Length; i++)
            {
                if (CurrentRoom.Doors[i] != null)
                {
                    if (CurrentRoom.Doors[i].isOver(new Vector2(State.X, State.Y)))
                    {
                        return (Direction)i;
                    }
                }
            }
            return (Direction)0;
        }
        /// <summary>
        /// Obtains direction in which arrow has shot, based on its position.
        /// </summary>
        /// <returns></returns>
        private Direction getDirectionFromArrow()
        {
            for (int i = 0; i < CurrentRoom.Doors.Length; i++)
            {
                if (CurrentRoom.Doors[i] != null)
                {
                    if (CurrentRoom.Doors[i].isOver(MyGameControl.MyPlayer.MyArrow.Position))
                    {
                        return (Direction)i;
                    }
                }
            }
            return (Direction)0;
        }

        /// <summary>
        /// Algorithm to detect whether the cursor is over any of the buttons.
        /// </summary>
        /// <param name="p">Menu style.</param>
        /// <returns></returns>
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
                return MyMenu.Mouse.isOver(MyVec) || MyMenu.Keyboard.isOver(MyVec) || MyMenu.Kinect.isOver(MyVec)
                    || MyMenu.MusicOn.isOver(MyVec) || MyMenu.MusicOff.isOver(MyVec) || MyMenu.SoundOn.isOver(MyVec)
                    || MyMenu.SoundOff.isOver(MyVec) || MyMenu.Debug.isOver(MyVec) || MyMenu.Cheats.isOver(MyVec)
                    || MyMenu.Back.isOver(MyVec);
            }
            else if (p == "Pause")
                return MyMenu.Resume.isOver(MyVec) || MyMenu.Quit.isOver(MyVec) || MyMenu.MainMenu.isOver(MyVec);
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
            // Ask for a Cave method(s): northDoors(), which returns north connections, etc.
            // But what if hex? Not NESW. Either make sure you can have only 4 per room? Or change the system.
            if (CurrentRoom.RoomNumber == 0 || CurrentRoom.RoomNumber == 1 || CurrentRoom.RoomNumber == 3)
                return MyGameControl.MyPlayer.Position.X > CurrentRoom.Doors[1].Position.X && MyGameControl.MyPlayer.Position.X /*+ CenteredSprite.height*/ < CurrentRoom.Doors[1].Position.X + Room.HorizontalDoorSize.X && MyGameControl.isPlayerAtWall;
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
                return MyGameControl.MyPlayer.Position.Y > CurrentRoom.Doors[4].Position.Y && MyGameControl.MyPlayer.Position.Y /*+ CenteredSprite.height*/ < CurrentRoom.Doors[4].Position.Y + Room.VerticalDoorSize.Y && MyGameControl.isPlayerAtWall;
            else
                return false;
        }

        /// <summary>
        /// Method that handles the angle of the player based on the current position of CursorRight.
        /// Integrates Mouse and Kinect movement. ;) Bazinga.
        /// </summary>
        private void UpdateAngle()
        {
            if (MyGameControl.PlayerMode != GameControl.PlayerState.Trivia)
            {
                float myTheta = MyGameControl.MyPlayer.theta;

                // Direction is the vector representing the x and y difference between the player's current position and
                // the mouse's position.

                Vector2 Direction = new Vector2(
                    MyGameControl.MyPlayer.Position.X + CenteredSprite.height / 2 - (CursorRight.Position.X + Sprite.stdHeight/* / 2*/),
                    MyGameControl.MyPlayer.Position.Y + CenteredSprite.height / 2 - (CursorRight.Position.Y + Sprite.stdHeight/* / 2*/));

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
            if (MyGameControl.PlayerMode != GameControl.PlayerState.Trivia)
            {
                if (Math.Abs((int)CursorRight.Position.X - (int)MyGameControl.MyPlayer.Position.X) > movementBuffer || Math.Abs((int)CursorRight.Position.Y - (int)MyGameControl.MyPlayer.Position.Y) > movementBuffer)
                {
                    // The version of MovePlayer involving the mouse.
                    MyGameControl.MoveNow(CurrentRoom);
                }
            }
        }

        /*
        /// <summary>
        /// Handles all clicks by Kinect.
        /// </summary>
        /// 
        private void UpdateKinectClicks(GameTime gameTime)
        {
            if(MyGameControl.GameState == GameControl.Gstate.Menu)
            {
                if (isOverButtons("Startup") || isOverButtons("Options") || isOverButtons("Pause"))
                {
                    if (!KinectWatch.IsRunning)
                    {
                        KinectWatch.Start();
                    }
                    if (KinectWatch.ElapsedMilliseconds > 1200)
                    {
                        if(isOverButtons("Startup") && MyMenu.MyMenuState == Menu.MenuState.Startup)
                            ClickSequence(CursorRight.Position, "Startup");
                        if (isOverButtons("Options") && MyMenu.MyMenuState == Menu.MenuState.Options)
                            ClickSequence(CursorRight.Position, "Options");
                        if (isOverButtons("Pause") && MyMenu.MyMenuState == Menu.MenuState.Pause)
                            ClickSequence(CursorRight.Position, "Pause");
                        MyWatch.Reset();
                        StopAnimateCursor(CursorRight);
                    }
                    else
                    {
                        AnimateCursor(CursorRight, gameTime);
                    }
                }
                else
                {
                    KinectWatch.Reset();
                    StopAnimateCursor(CursorRight);
                }
            }
            else if(MyGameControl.GameState == GameControl.Gstate.Game)
            {
                // Pause Menu
                if (SettingsButton.isOver(CursorLeft.Position))
                {
                    if (!KinectWatch.IsRunning)
                    {
                        KinectWatch.Start();
                    }
                    if (KinectWatch.ElapsedMilliseconds > 1200)
                    {
                        if (MyGameControl.GameState == GameControl.Gstate.Game && MyMenu.MyMenuState != Menu.MenuState.Pause)
                        {
                            MyGameControl.GameState = GameControl.Gstate.Menu;
                            MyMenu.MyMenuState = Menu.MenuState.Pause;
                            KinectWatch.Stop();
                        }
                        else
                        {
                            MyGameControl.GameState = GameControl.Gstate.Game;
                            MyMenu.MyMenuState = Menu.MenuState.Startup;
                            KinectWatch.Start();
                        }
                        KinectWatch.Reset();
                        StopAnimateCursor(CursorLeft);
                    }
                    else
                    {
                        AnimateCursor(CursorLeft, gameTime);
                    }
                }
                    // Arrow mode
                else if (Arrows.isOver(CursorLeft.Position))
                {
                    if (!KinectWatch.IsRunning)
                    {
                        KinectWatch.Start();
                    }
                    if (KinectWatch.ElapsedMilliseconds > 1200)
                    {
                        if (MyGameControl.PlayerMode == GameControl.PlayerState.Arrow && MyGameControl.MyPlayer.getArrows() >= 1)
                            MyGameControl.isShooting = true;
                        else
                        {
                            MyGameControl.PlayerMode = GameControl.PlayerState.Arrow;
                            MyGameControl.isShooting = false;
                        }
                        KinectWatch.Reset();
                        StopAnimateCursor(CursorLeft);
                    }
                    else
                    {
                        AnimateCursor(CursorLeft, gameTime);
                    }
                }
                    // Switching to move
                else if (Move.isOver(CursorLeft.Position))
                {
                    if (!KinectWatch.IsRunning)
                    {
                        KinectWatch.Start();
                    }
                    if (KinectWatch.ElapsedMilliseconds > 1200)
                    {
                        MyGameControl.isShooting = false;
                        if(MyGameControl.PlayerMode != GameControl.PlayerState.Trivia)
                            MyGameControl.PlayerMode = GameControl.PlayerState.Movement;
                        KinectWatch.Reset();
                        StopAnimateCursor(CursorLeft);
                    }
                    else
                    {
                        AnimateCursor(CursorLeft, gameTime);
                    }
                }
                else if (MyGameControl.PlayerMode != GameControl.PlayerState.Trivia)
                {
                    StopAnimateCursor(CursorRight);
                    StopAnimateCursor(CursorLeft);
                }
                // Trivia
                if (MyGameControl.PlayerMode == GameControl.PlayerState.Trivia)
                {
                    Sprite s;
                    for (int i = 0; i < MyGameControl.MyTrivia.AnswerButtons.Length; i++)
                    {
                        s = MyGameControl.MyTrivia.AnswerButtons[i];
                        if (s.isOver(CursorRight.Position))
                        {
                            if (!KinectWatch.IsRunning)
                                KinectWatch.Start();
                            if (KinectWatch.ElapsedMilliseconds > 1200)
                            {
                                MyGameControl.drawTrivia = false;
                                bool wump = MyGameControl.MyMap.getWumpusRoom() == CurrentRoom.getIntegerForm();
                                isCorrect = MyGameControl.AnswerQuestion(i);
                                if (isCorrect)
                                {
                                    WonTrivia(wump);
                                }
                                else if (MyGameControl.questionN >= nOfQuestionsFor('w'))
                                {
                                    EndTrivia(wump);
                                }
                                else if (MyGameControl.questionN >= nOfQuestionsFor('p'))
                                {
                                    EndTrivia(!wump);
                                }
                                else
                                {
                                    NextQuestion(false);
                                }
                                KinectWatch.Reset();
                                StopAnimateCursor(CursorRight);
                            }
                            else
                            {
                                AnimateCursor(CursorLeft, gameTime);
                            }
                            
                        }
                    }
                }
            }
                // Pre-game
            else if (MyGameControl.GameState == GameControl.Gstate.PreGame)
            {
                if (Skip.isOver(CursorLeft.Position) || Skip.isOver(CursorRight.Position))
                {
                    if (!KinectWatch.IsRunning)
                        KinectWatch.Start();
                    if (KinectWatch.ElapsedMilliseconds > 1200)
                    {
                        MyGameControl.GameState = GameControl.Gstate.Game;
                        KinectWatch.Reset();
                        StopAnimateCursor(CursorLeft);
                    }
                    else
                    {
                        AnimateCursor(CursorLeft, gameTime);
                    }

                }
            }
        }
         * */

        /// <summary>
        /// Kinect functions: to animate the circle around the cursor to demonstrate the time it hovers.
        /// </summary>
        /// <param name="Cursor">Which cursor to animate.</param>
        /// <param name="gameTime">Game Time.</param>
        /*
        private void AnimateCursor(AnimatedSprite Cursor, GameTime gameTime)
        {
            isAnimated = true;
            Cursor.UpdateFrame((float)(gameTime.ElapsedGameTime.TotalSeconds));
        }
        private void StopAnimateCursor(AnimatedSprite Cursor)
        {
            Cursor.FrameNumber = 1;
            isAnimated = false;
        }
        */
        bool isCorrect;

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
                if (MyGameControl.GameState == GameControl.Gstate.Menu)
                {
                    if (MyMenu.MyMenuState == Menu.MenuState.Startup)
                    {
                        ClickSequence(MousePos, "Startup");
                    }
                    else if (MyMenu.MyMenuState == Menu.MenuState.Options)
                    {
                        ClickSequence(MousePos, "Options");
                    }
                    else if (MyMenu.MyMenuState == Menu.MenuState.Pause)
                    {
                        //MyGameControl.GameState = GameControl.Gstate.Game;
                        //MyMenu.MyMenuState = Menu.MenuState.Startup;
                        ClickSequence(MousePos, "Pause");
                    }
                }
                else if (MyGameControl.GameState == GameControl.Gstate.Game)
                {
                    if (MyGameControl.PlayerMode == GameControl.PlayerState.Arrow)
                    {
                        if(MyGameControl.MyPlayer.getArrows() >= 1)
                        {
                            MyGameControl.isShooting = true;
                            //MyGameControl.shoot(getDirectionFromMouse(NewState));
                        }

                    }
                    if (SettingsButton.isOver(MousePos))
                    {
                        if (MyMenu.MyMenuState != Menu.MenuState.Pause)
                        {
                            MyGameControl.GameState = GameControl.Gstate.Menu;
                            MyMenu.MyMenuState = Menu.MenuState.Pause;
                            MyWatch.Stop();
                        }
                        else
                        {
                            MyWatch.Start();
                        }
                        
                    }
                    // Trivia answering
                    if (MyGameControl.PlayerMode == GameControl.PlayerState.Trivia)
                    {
                        Sprite s;
                        for (int i = 0; i < MyGameControl.MyTrivia.AnswerButtons.Length; i++)
                        {
                            s = MyGameControl.MyTrivia.AnswerButtons[i];
                            if (s.isOver(MousePos))
                            {
                                MyGameControl.drawTrivia = false;
                                bool wump = MyGameControl.MyMap.getWumpusRoom() == CurrentRoom.getIntegerForm();
                                isCorrect = MyGameControl.AnswerQuestion(i);
                                if (isCorrect)
                                {
                                    WonTrivia(wump);
                                }
                                else if (MyGameControl.questionN >= nOfQuestionsFor('w'))
                                {
                                    EndTrivia(wump);
                                }
                                else if (MyGameControl.questionN >= nOfQuestionsFor('p'))
                                {
                                    EndTrivia(!wump);
                                }
                                else
                                {
                                    NextQuestion(false);
                                }
                                
                            }
                        }
                    }
                }
                if (MyGameControl.GameState == GameControl.Gstate.PreGame)
                {
                    if (Skip.isOver(CursorRight.Position) || Skip.isOver(CursorLeft.Position))
                    {
                        if (MState.LeftButton == ButtonState.Pressed)
                        {
                            MyGameControl.GameState = GameControl.Gstate.Game;
                        }
                    }
                } 
                
            }
            else if (MyControls == Controls.Keyboard)
            {
                MyGameControl.MyPlayer.MyArrow.theta = MyGameControl.MyPlayer.theta;
                if (MyGameControl.isShooting)
                    MyGameControl.MyPlayer.ShootArrow();
                if (!MyGameControl.canMove(MyGameControl.MyPlayer.MyArrow, 0, 0, CurrentRoom) && MyGameControl.isShooting)
                {
                    MyGameControl.shoot(getDirectionFromArrow());
                    MyGameControl.isShooting = false;
                }

                if (!MyGameControl.isShooting)
                    MyGameControl.MyPlayer.ResetArrow();

                if (MyGameControl.MyPlayer.getArrows() >= 1)
                {
                    MyGameControl.isShooting = true;
                    //MyGameControl.shoot(getDirectionFromMouse(NewState));
                }

            }
            else
            {
                DebugModeChanged = false;
                //MyGameControl.isShooting = false;
            }
        }

        /// <summary>
        /// Final method to terminate Trivia method sequence
        /// </summary>
        /// <param name="wump">Whether the trivia is Wumpus or not.</param>
        private void EndTrivia(bool wump)
        {
            if (wump)
            {
                if (MyGameControl.questionsAnsweredCorrectly >= 3)
                {
                    WonWumpusBattle();
                }
                else
                {
                    LostTrivia();
                }
            }
            else
            {
                if (MyGameControl.questionsAnsweredCorrectly >= 2)
                    WonPitBattle();
                else
                    LostTrivia();
            }
            MyGameControl.isTrivia = false;
        }

        /// <summary>
        /// Gets # of questions each type of trivia requires.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private int nOfQuestionsFor(char p)
        {
            if (p == 'w')
                return 5;
            else
                return 3;
        }

        // Various trivia methods.
        private void WonTrivia(bool wumpus)
        {
            if (MyGameControl.questionN <= nOfQuestionsFor((wumpus) ? 'w' : 'p'))
            {
                correctDraw = true;
                NextQuestion(true);
            }
            else
                EndTrivia(wumpus);
            
            //spriteBatch.End();
        }
        private void WonWumpusBattle()
        {
            MyGameControl.RandomizeRoom('w');
            MyGameControl.PlayerMode = GameControl.PlayerState.Movement;
            winningMessage = true;
            TempWatch.Start();
        }
        private void WonPitBattle()
        {
            MyGameControl.setInitialRoom();
            MyGameControl.moveTo(MyGameControl.MyMap.getPlayerRoom(), GameControl.Direction.North);
            CurrentRoom = MyGameControl.MyMap.getPlayerRoom();
            MyGameControl.PlayerMode = GameControl.PlayerState.Movement;
            winningMessage = true;
            TempWatch.Start();
        }
        private void NextQuestion(bool correct)
        {
            isCorrect = correct;
            correctDraw = true;
            TempWatch.Start();
            MyGameControl.questionN++;
            if(correct)
                MyGameControl.questionsAnsweredCorrectly++;
            MyGameControl.InitializeTrivia();
        }
        private void LostTrivia()
        {
            MyGameControl.won = false;
            correctDraw = false;
            EndGame();
        }

        List<int> HighScores = new List<int>();
        private int storyIndex;
        private void EndGame()
        {
            MyGameControl.GameState = GameControl.Gstate.EndGame;
            HighScores = MyGameControl.EndGame();
        }
        
        /// <summary>
        /// Handles the various click sequences of the various menus.
        /// </summary>
        /// <param name="Pos"></param>
        /// <param name="name"></param>
        private void ClickSequence(Vector2 Pos, string name)
        {
            if (name == "Options")
            {
                // Debug Button
                if (MyMenu.Debug.isOver(Pos) && !DebugModeChanged)
                {
                    DebugButtonClick();
                }
                else if (MyMenu.Cheats.isOver(Pos))
                {
                    CheatCodesClick();
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
                else if (MyMenu.Mouse.isOver(Pos))
                {
                    MouseClick();
                }
                // Cave 2
                else if (MyMenu.Keyboard.isOver(Pos))
                {
                    KeyboardClick();
                }
                // Cave 3
                else if (MyMenu.Kinect.isOver(Pos))
                {
                    KinectClick();
                }
                // Back to Main Menu
                else if (MyMenu.Back.isOver(Pos))
                {
                    MyMenu.MyMenuState = Menu.MenuState.Startup;
                }
            }
            else if (name == "Startup")
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
                // High Scores Button
                else if (MyMenu.HighScoresButton.isOver(Pos))
                {
                    HighScoreClick();
                }
            }
            else if (name == "Pause")
            {
                if (MyMenu.Resume.isOver(Pos))
                {
                    MyGameControl.GameState = GameControl.Gstate.Game;
                    MyGameControl.PlayerMode = GameControl.PlayerState.Movement;
                }
                else if (MyMenu.Quit.isOver(Pos))
                {
                    ExitButtonClick();
                }
                else if (MyMenu.MainMenu.isOver(Pos))
                {
                    MyMenu.MyMenuState = Menu.MenuState.Startup;
                    MyGameControl.MyPlayer.Reset();
                    MyWatch.Reset();
                }
            }
        }

        /// <summary>
        /// All the click method sequences
        /// </summary>
        private void PlayButtonClick()
        {
            MyGameControl.GameState = GameControl.Gstate.PreGame;
            MyGameControl.InitializeGameMode();
            MyWatch.Start();
        }
        private void HighScoreClick()
        {
            DrawHighScores();
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
                DebugModeChanged = true;
                MyMenu.Debug.AssetName = "Menu\\Startup\\Options\\DebugMode_Disabled";
                MyMenu.Debug.LoadContent(this.Content);
            }
            else
            {
                MyGameControl.isDebugMode = true;
                DebugModeChanged = true;
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
        private void MouseClick()
        {
            if (MyControls != Controls.Mouse)
            {
                MyControls = Controls.Mouse;
                MyMenu.Mouse.AssetName = "Menu\\Startup\\Options\\Mouse_Enabled";
                MyMenu.Keyboard.AssetName = "Menu\\Startup\\Options\\Keyboard_Disabled";
                MyMenu.Kinect.AssetName = "Menu\\Startup\\Options\\Kinect_Disabled";
                MyMenu.Mouse.LoadContent(this.Content);
                MyMenu.Keyboard.LoadContent(this.Content);
                MyMenu.Kinect.LoadContent(this.Content);
            }
        }
        
        private void KeyboardClick()
        {
            if (MyControls != Controls.Keyboard)
            {
                MyControls = Controls.Keyboard;
                MyMenu.Mouse.AssetName = "Menu\\Startup\\Options\\Mouse_Disabled";
                MyMenu.Keyboard.AssetName = "Menu\\Startup\\Options\\Keyboard_Enabled";
                MyMenu.Kinect.AssetName = "Menu\\Startup\\Options\\Kinect_Disabled";
                MyMenu.Mouse.LoadContent(this.Content);
                MyMenu.Keyboard.LoadContent(this.Content);
                MyMenu.Kinect.LoadContent(this.Content);
            }
        }
        
        private void KinectClick()
        {
            if (MyControls != Controls.Kinect)
            {
                MyControls = Controls.Kinect;
                MyMenu.Mouse.AssetName = "Menu\\Startup\\Options\\Mouse_Disabled";
                MyMenu.Keyboard.AssetName = "Menu\\Startup\\Options\\Keyboard_Disabled";
                MyMenu.Kinect.AssetName = "Menu\\Startup\\Options\\Kinect_Enabled";
                MyMenu.Mouse.LoadContent(this.Content);
                MyMenu.Keyboard.LoadContent(this.Content);
                MyMenu.Kinect.LoadContent(this.Content);
            }
        }
         
        private void CheatCodesClick()
        {
            MyGameControl.MyPlayer.Velocity *= 2.0f;
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
                MyGameControl.MovePlayer(0, -(int)(MyGameControl.MyPlayer.Velocity), CurrentRoom);
            if (theKBState.IsKeyDown(Keys.Right))
                MyGameControl.MovePlayer((int)(MyGameControl.MyPlayer.Velocity), 0, CurrentRoom);
            if (theKBState.IsKeyDown(Keys.Down))
                MyGameControl.MovePlayer(0, (int)(MyGameControl.MyPlayer.Velocity), CurrentRoom);
            if (theKBState.IsKeyDown(Keys.Left))
                MyGameControl.MovePlayer(-(int)(MyGameControl.MyPlayer.Velocity), 0, CurrentRoom);
        }

        public enum Direction { None, North, East, South, West };

        /// <summary>
        /// Used to determine which wall the player is at (when the playerIsAtWall is true)
        /// </summary>
        /// <param name="Position">The Position of the Player</param>
        /// <returns>Which direction the Player is going to move.</returns>
        public Direction getDirectionFromXY(Vector2 Position)
        {
            // Changed RoomSize to thisSize
            if (Position.X < CurrentRoom.thisSize.Left + thresholdDistance)
                return Direction.West;
            else if (Position.X > CurrentRoom.thisSize.Right - thresholdDistance)
                return Direction.East;
            else if (Position.Y < CurrentRoom.thisSize.Top + thresholdDistance)
                return Direction.North;
            else if (Position.Y > CurrentRoom.thisSize.Bottom - thresholdDistance)
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

            if (MyControls != Controls.Kinect)
            {
                CursorLeft.Position = new Vector2(-40, -40);
            }

            // All the sprites
            BG.Draw(spriteBatch, this.Content);
            if (MyGameControl.GameState == GameControl.Gstate.Game || (MyMenu.MyMenuState == Menu.MenuState.Pause))
            {
                //Door.Draw(spriteBatch, this.Content); 
                CurrentRoom.DrawRoom(spriteBatch, this.Content, MyGameControl.isRoomPitted(CurrentRoom), MyGameControl.isRoomBatted(CurrentRoom));
                HUD1.DrawHUD(spriteBatch, this.Content);
                
                if (CurrentRoom.getIntegerForm() == MyGameControl.MyMap.getWumpusRoom())
                    MyGameControl.MyWumpus.Draw(spriteBatch, this.Content);

                if (MyGameControl.PlayerMode == GameControl.PlayerState.Trivia)
                    MyGameControl.drawTrivia = true;


                MyGameControl.Draw(spriteBatch, this.Content);
                SettingsButton.Draw(spriteBatch, this.Content);
                if (MyControls == Controls.Kinect)
                {
                    Arrows.Draw(spriteBatch, this.Content);
                    Move.Draw(spriteBatch, this.Content);
                }

                //Buy.Draw(spriteBatch, this.Content);

                if (MyMenu.MyMenuState == Menu.MenuState.Pause && MyGameControl.GameState == GameControl.Gstate.Menu)
                {
                    MyMenu.Draw(spriteBatch, this.Content);
                }
            }
            else if (MyGameControl.GameState == GameControl.Gstate.Menu)
            {
                MyMenu.Draw(spriteBatch, this.Content);
            }
            else if (MyGameControl.GameState == GameControl.Gstate.EndGame)
            {
                MyMenu.ExitGameButton.Draw(spriteBatch, this.Content);
            }
            else if (MyGameControl.GameState == GameControl.Gstate.PreGame)
            {
                StoryWatch.Start();
                Story.AssetName = "StorylineFiles\\" + Convert.ToString(((GUI.Storyline)storyIndex));
                Story.LoadContent(this.Content);
                Story.Draw(spriteBatch, this.Content);
                if (StoryWatch.Elapsed.TotalMilliseconds > 1000)
                {
                    storyIndex++;
                    StoryWatch.Restart();
                }
                if (storyIndex >= 7)
                {
                    MyGameControl.GameState = GameControl.Gstate.Game;
                }
                Skip.Draw(spriteBatch, this.Content);
            }
            
            // Text drawing
            DrawText();
            
            // And, of course, the cursor
            if (isAnimated)
            {
                CursorRight.AssetName = "ScreenPointers\\AnimatedCursor16";
                CursorRight.LoadContent(this.Content);
                CursorRight.DrawAnimated(spriteBatch, this.Content);
                CursorLeft.AssetName = "ScreenPointers\\AnimatedCursor16";
                CursorLeft.LoadContent(this.Content);
                CursorLeft.DrawAnimated(spriteBatch, this.Content);
            }
            else
            {
                CursorRight.AssetName = "ScreenPointers\\cursor2";
                CursorRight.LoadContent(this.Content);
                CursorRight.Draw(spriteBatch, this.Content);
                CursorLeft.AssetName = "ScreenPointers\\cursor2";
                CursorLeft.LoadContent(this.Content);
                CursorLeft.Draw(spriteBatch, this.Content);
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }

        /// <summary>
        /// Draws all text. All of it.
        /// Including Debug, HUD, and others.
        /// </summary>
        private void DrawText()
        {
            if (MyGameControl.isDebugMode)
            {
                string Debug = "Cursor: " + CursorRight.Origin;
                //Debug += "\nMouse: " + MState;
                //Debug += "\nPlayer: " + MyGameControl.MyPlayer.Position;
                //Debug += "\nKinectWatch: " + KinectWatch.ElapsedMilliseconds;
                //Debug += "\nConnectedStatus: " + ConnectedStatus;
                Debug += "\nPlayer: " + CurrentRoom;
                Debug += "\nWumpus: " + new Room(MyGameControl.MyMap.getWumpusRoom());
                Debug += "\nPit0: " + new Room(MyGameControl.MyMap.getAllPitRooms()[0]);
                Debug += "\nAns: " + MyGameControl.correctAns;

                //Debug += "\nTwoAway: " + MyGameControl.MyCave.warningRooms(CurrentRoom.getDecimalForm())[1];
                spriteBatch.DrawString(DebugFont, Debug, new Vector2(10, 470), Color.White);
            }

            if (MyGameControl.GameState == GameControl.Gstate.Game)
            {
                spriteBatch.DrawString(DebugFont, MyGameControl.MyMap.getPlayerRoom().ToString(), new Vector2(Room.RoomSize.Center.X, Room.RoomSize.Center.Y), Color.White);
                
                decimal time = Math.Round((decimal)(MyWatch.Elapsed.TotalSeconds), 3);
                // ADD SCORE
                float TimeMovesY = HUD1.ArrowIcon.Position.Y + 50 + 7;
                spriteBatch.DrawString(HUDFont, "Time: " + (time), new Vector2(460, TimeMovesY), Color.White);
                spriteBatch.DrawString(HUDFont, "Moves: " + MyGameControl.MyMap.moves, new Vector2(650, TimeMovesY), Color.White);
                spriteBatch.DrawString(HUDFont, MyGameControl.MyPlayer.getGold() + "", new Vector2(HUD1.GoldIcon.Position.X + HUD1.GoldIcon.Texture.Width + 15, HUD1.GoldIcon.Position.Y), Color.White);
                spriteBatch.DrawString(HUDFont, MyGameControl.MyPlayer.getArrows() + "", new Vector2(HUD1.ArrowIcon.Position.X + HUD1.ArrowIcon.Texture.Width + 15, HUD1.ArrowIcon.Position.Y), Color.White);
                spriteBatch.DrawString(HUDFont, "Score: " + MyGameControl.MyPlayer.getScore(), new Vector2(HUD1.ArrowIcon.Position.X + HUD1.ArrowIcon.Texture.Width + 85, HUD1.ArrowIcon.Position.Y), Color.White);
                
                spriteBatch.DrawString(HUDFont, MyGameControl.MyMap.GiveWarnings(MyGameControl.MyCave.warningRooms(MyGameControl.MyMap.getPlayerRoom().getDecimalForm()).ToArray()), new Vector2(50, 40), Color.White);

                // On-screen text messages
                if(winningMessage)
                {
                    spriteBatch.DrawString(MyGameControl.MyTrivia.TriviaFont, "Well done! You're safe for now...", 
                        new Vector2(Room.RoomSize.Center.X - 150, Room.RoomSize.Center.Y - 80), Color.White);
                }
                else if (correctDraw && isCorrect)
                {
                    spriteBatch.DrawString(MyGameControl.MyTrivia.TriviaFont, "You got it! Keep going!",
                        new Vector2(Room.RoomSize.Center.X - 150, Room.RoomSize.Center.Y - 80), Color.White);
                }
                else if (correctDraw && !isCorrect)
                {
                    spriteBatch.DrawString(MyGameControl.MyTrivia.TriviaFont, "Oh no! That's wrong!",
                        new Vector2(Room.RoomSize.Center.X - 150, Room.RoomSize.Center.Y - 80), Color.White);
                }
                else if (MyGameControl.shouldResetRoom)
                {
                    spriteBatch.DrawString(MyGameControl.MyTrivia.TriviaFont, "The bats have teleported you to a new room!\nLook out next time...",
                        new Vector2(Room.RoomSize.Center.X - 250, Room.RoomSize.Center.Y - 80), Color.White);
                }
                // Timing the textmessages
                if (TempWatch.IsRunning && TempWatch.ElapsedMilliseconds > 1500)
                {
                    winningMessage = false;
                    correctDraw = false;
                    TempWatch.Reset();
                }
                if (MyGameControl.BatTextWatch.IsRunning && MyGameControl.BatTextWatch.ElapsedMilliseconds > 2200)
                {
                    MyGameControl.shouldResetRoom = false;
                    MyGameControl.BatTextWatch.Reset();
                }
            }
            else if (MyGameControl.GameState == GameControl.Gstate.EndGame)
            {
                string str = (MyGameControl.won) ? "YOU WON!" : "GAME OVER!";
                str += "\nThanks for playing!\nHere are the high scores.";
                
                spriteBatch.DrawString(HUDFont, "Your score:\n" + MyGameControl.MyPlayer.getScore(), new Vector2(650, 100), Color.White);
                string highs = "";
                MyGameControl.hs.saveHighScores(0);
                HighScores = MyGameControl.hs.getMyList();
                foreach (int i in HighScores)
                {
                    if(i != 0)
                        highs += "\n" + i;
                }
                str += highs;
                spriteBatch.DrawString(HUDFont, str, new Vector2(200, 100), Color.White);
                
            }

            //else if (MyMenu.MyMenuState == Menu.MenuState.Options && MyGameControl.GameState == GameControl.Gstate.Menu)
                
        }

        private void DrawHighScores()
        {
            MyGameControl.hs.saveHighScores(0);
            HighScores = MyGameControl.hs.getMyList();
            string highs = "";
            foreach (int i in HighScores)
            {
                if (i != 0)
                    highs += "\n" + i;
            }
            spriteBatch.Begin();
            spriteBatch.DrawString(HUDFont, highs, new Vector2(200, 100), Color.White);
            spriteBatch.End();
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
