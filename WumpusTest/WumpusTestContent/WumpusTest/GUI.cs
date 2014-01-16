using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace WumpusTest
{
    class GUI : Sprite
    {
        public GUI()
        {
            //GameControl gc = new GameControl();
            //Player p = new Player();
            //Cave c = new Cave();
        }

        //Parameters: none
        //Output: Display the paused Game Menu on the screen
        public void pauseGameMenu()
        {
        }

        //Parameters: None
        //Output: Display the startup game menu on the screen
        public void bootMenu()
        {
        }

        //Parameters: Not Sure
        //Output: Shows volume rocker on screen
        public void updateSoundControl()
        {
        }

        //Display on the game screen a representation of the room, 
        //including the hexagonal form of the room, with each edge illustrated as either a tunnel
        //to an adjacent room or a wall blocking access to an adjacent room.
        //Parameters: Get cave obstacles, size, shape, from cave object
        //Output: Display surrounding rooms on form
        //
        public void updateCave(int caveNumber, int roomNumber)
        {
            //caveNumber: shows terrain, different obstacles
            //roomNumber: current aspects of cave within it
           
        }

        //display score //Display on the game screen the player’s score.
        //Parameters: Updated Score from game control
        //Output: Display score on screen
        public void updateScore(int updatedScore)
        {

        }

        //Include an illustration of the player, any present hazards, the wumpus if present, 
        //and any additional graphics that add to the realism of the cave.
        //Parameters: get player location from player object/cave object
        //Output: Display player within room on form
        public int[] updatePlayerLocation(int[] playerLocation)
        {
            return null;
        }

        //Parameters: Get coins quantity from player object
        //Output: Display quantity of coins on form
        public void updateCoins(int updatedCoins)
        {
        }

        //display number of tries
        //parameters: number of tries from gameControl
        //Output: display tries on form
        public void updateNumberOfTries(int numberOfTries)
        {
        }





        //Parameters: Get wumpus location from Game Control
        //Output: Show where the wumpus is on map
        public void updateWumpusLocation(int[] wumpusLocation)
        {

        }

        

        //Parameters: None
        //Output: Display on the game screen the player’s inventory box
        public void displayInventoryBox()
        {
           
        }
        
        //Parameters: Get arrow quantity from GameControl or player
        //Output: Show arrow quanity on screen
        public void updateArrows(int updatedArrows)
        {
        }

        //Display on the game screen any hints based on the player’s room. (For example: I smell a wumpus).
        //parameters: is it near the wumpus? Get from Player and cave object and wumpus object; game control
                        //ascertains this information by comparing playerLocation to wumpus location
        //Output: Display on form
        public void displayHint(Boolean nearWumpus)
        {
            if(nearWumpus)
            {
            }
        }


        //Display on the game screen all actions the player can take on the current turn.
        //parameters: type is incorrect, but i need to know what I can do at that moment
        //Output: Diplay on form
        public void updateOptions(int[] options)
        {
            //shoot arrow ability
            //purchase arrow ability
            //move
            //purchase a secret capability
        }

    }
}
