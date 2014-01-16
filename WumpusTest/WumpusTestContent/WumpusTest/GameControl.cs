using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

/*
 * Updated 5/16/2012: Player is now unable to move off of the purple screen
 **/


namespace WumpusTest
{
    class GameControl
    {
        
        public Player MyPlayer;
        public Map MyMap;
        public Wumpus MyWumpus;
        public Trivia MyTrivia;
        //WHighScores hs = new WHighScores();
        public Cave MyCave;
        GUI MyUI;

        Vector2 GameDimensions;
        //public Rectangle RoomSize = new Rectangle(120, 30, 660, 400);
        Sound MySound;
        public bool isPlayerAtWall = false;
        public bool isPlayerAtDoor = false;
        public bool isDebugMode = true, isMusicOn = false, isSoundOn = false ;
        // The user of the game right now: s-sjoshi, s-kbowers, Sarang, etc.
        public string username = "s-sjoshi";
        public int caveN = 1;

        public GameControl(Vector2 WindowSize)
        {
            // GState.Game loads the game
            // GState.Menu loads the menu
            GameState = Gstate.Menu;
            MyWumpus = new Wumpus(new Vector2(Room.thisSize.Center.X - 35, Room.thisSize.Center.Y - 35), "Wumpus2_Small");
            MyTrivia = new Trivia("Chemistry");
            MyCave = new Cave1("C:\\Users\\" + username + "\\Dropbox\\WumpusTest\\WumpusTest\\WumpusTestContent\\CaveFiles\\MapDataCave1.txt");
            MyPlayer = new Player(new Vector2(WindowSize.X / 2, WindowSize.Y / 2), "Player\\Player5", 0, 10, 0, 0, 3.5f);
            MyMap = new Map(MyCave.getTotalChambers());
            MySound = new Sound();
            
            MyUI = new GUI();
            GameDimensions = WindowSize;
        }

        // 0 = up, 1 = down, 2 = right, 3 = left
        enum roomType { pit, bat, wumpus, none };
        public enum Direction { None, North, East, South, West };

        public enum Gstate { Menu, Game, Trivia, Paused, EndGame };
        public Gstate GameState;

        public enum PlayerState { Movement, Arrow };
        public PlayerState PlayerMode = PlayerState.Movement;
        

        public void InitializeGameMode()
        {
            MyMap.randomizePitAndBatRooms();
            RandomizeRoom('p');
            RandomizeRoom('w');

            //MyMap.setPlayerRoom(new Room(4, 1));
            MyMap.setAdjacentRooms(MyCave.loadCaveandRoom(MyMap.getPlayerRoom().getDecimalForm()));
            if(isMusicOn)
                MySound.PlayBg();
            
        }

        private void RandomizeRoom(char p)
        {
            Room temp;
            Room[] myAdjacentRooms;
            Random r = MyMap.r;
            int count;
            do
            {
                count = 0;
                temp = new Room(MyMap.getRandomRoom());
                myAdjacentRooms = MyCave.loadCaveandRoom(temp.getDecimalForm());

                if (MyMap.randomizeCharacterRoom(temp.getIntegerForm(), myAdjacentRooms, p))
                    count++;


            } while (MyMap.isRoomNotOkay(temp, p));

            if(p == 'p')
            {
                MyMap.setPlayerRoom(temp);
                MyMap.addToExplored(temp);
            }
            else if (p == 'w')
            {
                MyMap.setWumpusRoom(temp.getIntegerForm());
            }
        }

        
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
                MyMap.setAdjacentRooms(MyCave.loadCaveandRoom(PlayerRoom.getDecimalForm()));
                // Call cave to see what room is adjacent, in given direction.
                adjacentRoom = MyMap.getAdjacentRooms()[(int)d]; /*c.getConnectedExteralRoom(1, room);*/;

                if (adjacentRoom != null)
                {
                    //adjacentRoom.LoadContent(theContentManager);
                    // Call map to see if the room in given direction is hazardous.
                    //roomType AdjacentRoomType = MyMap.getPlayerInRoomWith(MyRoom);
                    //MyMap.GiveWarnings(chamber);
                    //      If is pitted:
                    if (adjacentRoom.isRoomPitted())
                        EncounterPit();
                    //      If it contains Wumpus:
                    EncounterWumpus();
                    // Actually move to the room.
                    MyMap.setPlayerRoom(adjacentRoom);
                    MyMap.moves++;
                    // Move Wumpus around
                    MoveWumpus(false);
                    // Returns player to center:
                    ////MyPlayer.Position = new Vector2(RoomSize.Center.X, RoomSize.Center.Y);
                    // OR, more realistically, the opposite side:
                    MyPlayer.Position = getEntryLocationFrom((Direction)((((int)d + 1) % 4) + 1), MyPlayer.Position);
                }
                else
                    return new Room(0, 0);
            }
            
            return adjacentRoom;
        }

        public void MoveWumpus(bool trivia)
        {
            // Get stuff from Map
            Room WumpusRoom = new Room(MyMap.getWumpusRoom());
            Random myR = MyMap.r;
            Room NewRoom = WumpusRoom;
            // Get adj rooms
            Room[] wAdjRooms = MyCave.loadCaveandRoom(WumpusRoom.getDecimalForm());
            // obtain number of rooms to move per turn from Wumpus
            // CHECK TRIVIA
            int n = MyWumpus.Move(false);
            // loop
            for (int i = 0; i < n; i++)
            {
                // (get all adjacent rooms
                wAdjRooms = MyCave.loadCaveandRoom(WumpusRoom.getDecimalForm());
                // Eliminate all chambers from wAdjRooms
                for (int k = 1; k < wAdjRooms.Length; k++)
                {
                    if (wAdjRooms[k] != null)
                    {
                        if (i != n - 1)
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
                do
                {
                    j = myR.Next(wAdjRooms.Length - 1) + 1;
                    NewRoom = wAdjRooms[j];
                } while (MyMap.isRoomNotOkay(NewRoom, 'w'));
                // set wumpus room to the new room)
                MyMap.setWumpusRoom(NewRoom.getIntegerForm());
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

        public void shoot(int direction)
        {
            // Check user's arrow inventory
            // Shoot arrow in given direction.
            // Check if wumpy is there
        }
        public void die()
        {
            GameState = Gstate.EndGame;
            // show endgame graphics
            // Call high score and display high score
        }
        private void EncounterWumpus()
        {
            // Call player
            // Call Trivia
            
        }
        private void EncounterPit()
        {
            // Call trivia for questions
        }
        public void pause()
        {
            if (GameState == Gstate.Game)
            {
                // Call UI for menu graphics
            }
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
            //MyUI.LoadContent(theContentManager);
            
            //BG Sound
            MySound.LoadContent(theContentManager);
            
        }

        public void Draw(SpriteBatch spriteBatch, ContentManager theContentManager)
        {
            MyPlayer.Draw(spriteBatch, theContentManager);
            //MyWumpus.Draw(spriteBatch, theContentManager);
        }

        // Moves player on the screen by x and y.
        // checks to make sure player is still on the screen
        public void MovePlayer(float x, float y)
        {
            if (canMove(x, y))
            {
                MyPlayer.Position.X += x;
                MyPlayer.Position.Y += y;
                //MyPlayer.Origin.X += x;
                //MyPlayer.Origin.Y += y;
                isPlayerAtWall = false;
            }
            else
            {
                if(!isPlayerAtWall && isSoundOn && !isPlayerAtDoor)
                    MySound.PlayAttack();
                isPlayerAtWall = true;
            }
        }
        public void MoveNow()
        {
            MovePlayer((float)(MyPlayer.Velocity * Math.Cos(MyPlayer.newTheta)), (float)(MyPlayer.Velocity * Math.Sin(MyPlayer.newTheta))); 
            //MyPlayer.Position.Y += (float)(MyPlayer.Velocity * Math.Sin(MyPlayer.newTheta));
            //this.MyPlayer.Position.X += (float)(MyPlayer.Velocity * Math.Cos(MyPlayer.newTheta));
            MyPlayer.SetOrigin();
        }

        public bool canMove(float x, float y)
        {
            return (MyPlayer.Position.X + x) > Room.thisSize.Left + (CenteredSprite.height / 2) && MyPlayer.Position.X + x < Room.thisSize.Right - (CenteredSprite.height / 2) &&
                    (MyPlayer.Position.Y + y) > Room.thisSize.Top + (CenteredSprite.height / 2) && MyPlayer.Position.Y + y < Room.thisSize.Bottom - (CenteredSprite.height / 2);
        }

        public Room getRoom()
        {
            return MyMap.getPlayerRoom();
        }
    }
    
}