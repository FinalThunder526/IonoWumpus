using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WumpusTest
{
    class GameControl
    {
        
        public Player MyPlayer;
        public Map MyMap;
        public Wumpus MyWumpus;
        public Trivia MyTrivia;
        public WHighScores hs = new WHighScores();
        public Cave MyCave;
        public GUI MyUI;
        public Sound MySound;

        Vector2 GameDimensions;
        //public Rectangle RoomSize = new Rectangle(120, 30, 660, 400);
        
        public bool isPlayerAtWall = false, isPlayerAtDoor = false, won = false, isTrivia = false;
        public bool isDebugMode = false, isMusicOn = false, isSoundOn = false, isShooting = false;
        // The user of the game right now: s-sjoshi, s-kbowers, Sarang, etc.
        public string username = "Sarang";
        public int caveN = 1;
        public int questionN = 1;
        public Stopwatch BatTextWatch = new Stopwatch();

        public Room StartingRoom;

        public GameControl(Vector2 WindowSize)
        {
            // GState.Game loads the game
            // GState.Menu loads the menu
            GameState = Gstate.Menu;
            MyWumpus = new Wumpus(new Vector2(Room.RoomSize.Center.X - 200, Room.RoomSize.Center.Y - 200), "WumpusFiles\\Wumpus5_Small");
            MyTrivia = new Trivia("Chemistry", new Vector2(), "Trivia\\backgroundTrivia");
            MyCave = new Cave1("CaveFiles\\MapDataCave1.txt");
            MyPlayer = new Player(new Vector2(WindowSize.X / 2, WindowSize.Y / 2), "Player\\Player5", 0, 10, 0, 0, 5f);
            MyMap = new Map(MyCave.getTotalRooms());
            MySound = new Sound();
            MyUI = new GUI();
            GameDimensions = WindowSize;

        }

        // 0 = up, 1 = down, 2 = right, 3 = left
        enum roomType { pit, bat, wumpus, none };
        public enum Direction { None, North, East, South, West };

        public enum Gstate { Menu, PreGame, Game, Paused, EndGame };
        public Gstate GameState;

        public enum PlayerState { Movement, Arrow, Trivia };
        public PlayerState PlayerMode = PlayerState.Movement;
        

        public void InitializeGameMode()
        {
            //MyMap.randomizePitAndBatRooms();
            MyMap.randomizePitBatAndGoldRooms();

            //MyMap.setPlayerRoom(new Room(4, 1));
            MyMap.setAdjacentRooms(MyCave.getConnectedRooms(MyMap.getPlayerRoom().getDecimalForm()));
            if(isMusicOn)
                MySound.PlayBg();
            
        }

        public void RandomizeRoom(char p)
        {
            Room temp;
            Room[] myAdjacentRooms;
            Random r = MyMap.r;
            int count;
            do
            {
                count = 0;
                temp = new Room(MyMap.getRandomRoom());
                myAdjacentRooms = MyCave.getConnectedRooms(temp.getDecimalForm());

                if (MyMap.randomizeCharacterRoom(temp.getIntegerForm(), myAdjacentRooms, p))
                    count++;


            } while (MyMap.isRoomNotOkay(temp, p));

            if(p == 'p')
            {
                MyMap.setPlayerRoom(temp);
            }
            else if (p == 'w')
            {
                MyMap.setWumpusRoom(temp.getIntegerForm());
            }
        }

        public void InitializeTrivia()
        {
            PlayerMode = PlayerState.Trivia;
            isTrivia = true;
            //questionN = 1;
            MyPlayer.Position = new Vector2(Room.ChamberSize.Center.X, Room.ChamberSize.Center.Y);
            MyTrivia.safeRandomizeIndex();
        }

        /// <summary>
        /// Moving to a room.
        /// </summary>
        /// <param name="d">The direction to move in.</param>
        /// <returns>The room just moved into.</returns>
        public Room moveInDirection(Direction d)
        {
            Room adjacentRoom = new Room(0, 0);
            
            if (GameState == Gstate.Menu)
            {
                // The user is in-menu.
                //Console.Out.WriteLine("");
            }
            else if (GameState == Gstate.Game)
            {
                // The user is in-game.
                // Call map to see what room and chamber player is in.
                Room PlayerRoom = MyMap.getPlayerRoom();
                // Call map to set adjacent rooms
                MyMap.setAdjacentRooms(MyCave.getConnectedRooms(PlayerRoom.getDecimalForm()));
                // Call cave to see what room is adjacent, in given direction.
                adjacentRoom = MyMap.getAdjacentRooms()[(int)d]; /*c.getConnectedExteralRoom(1, room);*/;

                if (adjacentRoom != null)
                {
                    moveTo(adjacentRoom, d);
                    //MyPlayer.setGold(MyPlayer.getGold() + MyMap.updateExploredAndReturnGold(adjacentRoom.getIntegerForm()));
                }
                else
                    return new Room(0, 0);
            }
            
            return adjacentRoom;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="adjRoom"></param>
        /// <param name="d"></param>
        /// <returns>True if batted.</returns>
        public bool moveTo(Room adjRoom, Direction d)
        {
            MoveWumpus(false);
            //adjacentRoom.LoadContent(theContentManager);
            // Call map to see if the room in given direction is hazardous.
            //roomType AdjacentRoomType = MyMap.getPlayerInRoomWith(MyRoom);
            //MyMap.GiveWarnings(chamber);
            
            //updateGold(adjRoom);
            
            MyPlayer.setTurns(MyPlayer.getTurns() + 1);
            //      If is pitted:            
            if (isRoomPitted(adjRoom))
                EncounterPit();
            //      If it contains Wumpus:
            if (adjRoom.Equals(new Room(MyMap.getWumpusRoom())))
                EncounterWumpus();
            //      If is batted (=D)
            if (isRoomBatted(adjRoom))
                adjRoom = EncounterBat();
            // Actually move to the room.
            MyMap.setPlayerRoom(adjRoom);
            MyMap.moves++;
            MyPlayer.setTurns(MyMap.moves);
            // Move Wumpus around
            // Returns player to center:
            ////MyPlayer.Position = new Vector2(RoomSize.Center.X, RoomSize.Center.Y);
            // OR, more realistically, the opposite side:
            MyPlayer.Position = getEntryLocationFrom((Direction)((((int)d + 1) % 4) + 1), MyPlayer.Position);
            return (isRoomBatted(adjRoom));
        }

        private Room EncounterBat()
        {
            RandomizeRoom('p');
            shouldResetRoom = true;
            BatTextWatch.Start();
            return MyMap.getPlayerRoom();
        }

        public bool isRoomBatted(Room aNewRoom)
        {
            return (MyMap.getAllBatRooms().Contains(aNewRoom.getIntegerForm()));
        }

        public bool isRoomPitted(Room aNewRoom)
        {
            return (MyMap.getAllPitRooms().Contains(aNewRoom.getIntegerForm()));
        }

        public void updateGold(Room room)
        {
            MyPlayer.setGold(MyPlayer.getGold() + MyMap.updateExploredAndReturnGold(room.getIntegerForm()));
        }

        public void MoveWumpus(bool trivia)
        {
            // Get stuff from Map
            Room WumpusRoom = new Room(MyMap.getWumpusRoom());
            Random myR = MyMap.r;
            Room NewRoom = WumpusRoom;
            // Get adj rooms
            Room[] wAdjRooms = MyCave.getConnectedRooms(WumpusRoom.getDecimalForm());
            // obtain number of rooms to move per turn from Wumpus
            // CHECK TRIVIA
            int n = MyWumpus.Move(false);
            // loop
            for (int i = 0; i < n; i++)
            {
                // (get all adjacent rooms
                wAdjRooms = MyCave.getConnectedRooms(WumpusRoom.getDecimalForm());
                // Eliminate all chambers from wAdjRooms
                for (int k = 1; k < wAdjRooms.Length; k++)
                {
                    if (wAdjRooms[k] != null)
                    {
                        if (i != n - 1 && n != 1)
                        {
                            if (wAdjRooms[k].RoomNumber == 0)
                            {
                                wAdjRooms[k] = null;
                            }
                        }
                    }
                }
                // loop: choose randomly one of the rooms and check to make sure it is valid
                int j;
                bool temp = false;
                for (int k = 0; k < 15; k++)
                {
                    j = myR.Next(wAdjRooms.Length - 1) + 1;
                    NewRoom = wAdjRooms[j];
                    if (!MyMap.isRoomNotOkay(NewRoom, 'w'))
                    {
                        temp = true;
                        break;
                    }
                }
                if (temp)
                    MyMap.setWumpusRoom(NewRoom.getIntegerForm());
                else
                    RandomizeRoom('w');
                //do
                //{
                //    j = myR.Next(wAdjRooms.Length - 1) + 1;
                //    NewRoom = wAdjRooms[j];
                //} while (MyMap.isRoomNotOkay(NewRoom, 'w'));
                // set wumpus room to the new room)
                
            }
        }

        private Vector2 getEntryLocationFrom(Direction d, Vector2 Pos)
        {
            if (d.Equals(Direction.North))
                return new Vector2(Pos.X, Room.RoomSize.Top + 30);
            else if (d.Equals(Direction.East))
                return new Vector2(Room.RoomSize.Right - 30, Pos.Y);
            else if (d.Equals(Direction.South))
                return new Vector2(Pos.X, Room.RoomSize.Bottom - 30);
            else if (d.Equals(Direction.West))
                return new Vector2(Room.RoomSize.Left + 30, Pos.Y);
            else
                return new Vector2(Room.RoomSize.Center.X, Room.RoomSize.Center.Y);
        }

        public bool shoot(Game1WithoutKinect.Direction dir)
        {
            if (MyPlayer.getArrows() >= 1)
            {
                Room NextRoom = MyCave.getConnectedRooms(MyMap.getPlayerRoom().getDecimalForm())[(int)(dir)];
                MyPlayer.setArrows(MyPlayer.getArrows() - 1);
                if (NextRoom != null)
                {
                    won = MyMap.ShootArrow(NextRoom);
                    if(won)
                        GameState = Gstate.EndGame;

                    return true;
                }
                else
                    return false;
            }
            else
            {
                return false;
            }
        }
        public bool shoot(Game1.Direction dir)
        {
            if (MyPlayer.getArrows() >= 1)
            {
                Room NextRoom = MyCave.getConnectedRooms(MyMap.getPlayerRoom().getDecimalForm())[(int)(dir)];
                MyPlayer.setArrows(MyPlayer.getArrows() - 1);
                if (NextRoom != null)
                {
                    won = MyMap.ShootArrow(NextRoom);
                    if (won)
                        GameState = Gstate.EndGame;

                    return true;
                }
                else
                    return false;
            }
            else
            {
                return false;
            }
        }
        
        public void die()
        {
            GameState = Gstate.EndGame;
            // show endgame graphics
            // Call high score and display high score
        }
        
        private void EncounterWumpus()
        {
            // Call Trivia
            InitializeTrivia();
        }
        private void EncounterPit()
        {
            // Call trivia for questions
            InitializeTrivia();
        }

        public string correctAns;
        public bool AnswerQuestion(int index)
        {
            string[] answers = MyTrivia.getDistractors();
            correctAns = MyTrivia.getAnswer();
            MyPlayer.setGold(MyPlayer.getGold() - 1);
            return (MyTrivia.isAnswerCorrect(answers[index]));
        }
        
        // When the user presses the "Start Game" button, the state of the game is set to 1 (in game).
        // This happens only when the user is NOT ALREADY in the game.
        public bool startGame()
        {
            if(GameState != Gstate.Game)
                GameState = Gstate.Game;

            return true;
        }

        public void LoadContent(ContentManager theContentManager)
        {
            MyPlayer.LoadContent(theContentManager);
            MyTrivia.LoadTriviaContent(theContentManager);
            MySound.LoadContent(theContentManager);
            //MyUI.LoadContent(theContentManager);
            
            //BG Sound
            //          MySound.LoadContent(theContentManager);
            
        }

        public void Draw(SpriteBatch theSpriteBatch, ContentManager theContentManager)
        {
            MyPlayer.Draw(theSpriteBatch, theContentManager);
            MyMap.MyMiniMap.Draw(theSpriteBatch, theContentManager);
            if (isShooting)
                MyPlayer.MyArrow.Draw(theSpriteBatch, theContentManager);
            if (drawTrivia)
                MyTrivia.DrawTrivia(theSpriteBatch, theContentManager);
            //MyWumpus.Draw(spriteBatch, theContentManager);
        }

        // Moves player on the screen by x and y.
        // checks to make sure player is still on the screen
        public void MovePlayer(float x, float y, Room CurrentRoom)
        {
            Vector2 Target = new Vector2(MyPlayer.Position.X + x, MyPlayer.Position.Y + y);
            if (canMove(MyPlayer, x, y, CurrentRoom))
            {
                MyPlayer.Position.X += x;
                MyPlayer.Position.Y += y;
                isPlayerAtWall = false;
            }
            //else if (canMove(MyPlayer, x, 0, CurrentRoom))
            //{
            //    MyPlayer.Position.X += x;
            //    isPlayerAtWall = false;
            //}
            //else if (canMove(MyPlayer, 0, y, CurrentRoom))
            //{
            //    MyPlayer.Position.Y += y;
            //    isPlayerAtWall = false;
            //}
            else
            {
                if (!isPlayerAtWall && isSoundOn && !isPlayerAtDoor)
                    MySound.PlayAttack();
                isPlayerAtWall = true;
            }
        }
        public void MoveNow(Room CurrentRoom)
        {
            MovePlayer((float)(MyPlayer.Velocity * Math.Cos(MyPlayer.newTheta)), (float)(MyPlayer.Velocity * Math.Sin(MyPlayer.newTheta)), CurrentRoom); 
            //MyPlayer.Position.Y += (float)(MyPlayer.Velocity * Math.Sin(MyPlayer.newTheta));
            //this.MyPlayer.Position.X += (float)(MyPlayer.Velocity * Math.Cos(MyPlayer.newTheta));
            MyPlayer.SetOrigin();
        }

        public bool canMove(Sprite s, float x, float y, Room CurrentRoom)
        {
            return (s.Position.X + x) > CurrentRoom.thisSize.Left + (CenteredSprite.height / 2) && s.Position.X + x < CurrentRoom.thisSize.Right - (CenteredSprite.height / 2) &&
                    (s.Position.Y + y) > CurrentRoom.thisSize.Top + (CenteredSprite.height / 2) && s.Position.Y + y < CurrentRoom.thisSize.Bottom - (CenteredSprite.height / 2);
        }

        public Room getRoom()
        {
            return MyMap.getPlayerRoom();
        }

        public bool drawTrivia;
        public bool shouldResetRoom;
        public int questionsAnsweredCorrectly = 0;

        public List<int> EndGame()
        {
            GameState = Gstate.EndGame;
            hs.saveHighScores(MyPlayer.getScore());
            return hs.getMyList();
        }

        public void setInitialRoom()
        {
            MyMap.setPlayerRoom(StartingRoom);
        }
    }
    
}