/* Rene Moise Kwibuka
 * Andy Harbert
 * ASSIGNMENT 7.
 * Due Date: October 31st, 2014
*/
using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

class Game
{   
    private int rowsNumber{  get; set;}      //the number of rows
    private int columnsNumber { get; set; }  //the number of columns
    private int CircleDiameter { get; set; } //the diameter of a typical game checkers
    private int currentPlayer { get; set; }  //knows the current player
    private Checker[,] playedCheckers { get; set; }  //it contains the played checkers
    private int CurrentRow { get; set; }            //contains the last row played 
    private int currentColumn { get; set; }         //contains the last column played         
    private Checker saved { get; set; }              //it remembers the saved checker.
    private enum players { Black = 1, Red = 2};
   
    public Game(int rowsNum, int columnNum, int Diameter)
    {
        rowsNumber = rowsNum;
        columnsNumber = columnNum;
        CircleDiameter = Diameter;
        currentPlayer = (int)players.Black;               //Initialize current player
        //Creating a new array of that will hold the played checkers.         
        playedCheckers = new Checker[rowsNumber, columnsNumber];       
    }

    //draw the course
    public void DrawCourse(Graphics graphics)
    {
        for (int rowCounter = 0; rowCounter < rowsNumber; rowCounter++)
        {
            for (int colCounter = 0; colCounter < columnsNumber; colCounter++)
            {
                //Draw grids
                Checker grid = new Checker(Color.White, colCounter * CircleDiameter, rowCounter * CircleDiameter, CircleDiameter);                  
                grid.DrawCheckerCircles(graphics);                          
            }
        }     
    }

    //place checker in the appropriate location
    public void placeChecker(Graphics graphics, int colLocation)
    {
        if (currentPlayer == (int)players.Black)
        {
            for (int row = (rowsNumber - 1); row >= 0; row--)
            {
                if (playedCheckers[row, colLocation] == null)
                {                     
                    Checker black = new Checker(Color.Black, (colLocation * CircleDiameter), 
                        (row * CircleDiameter), CircleDiameter);    //make a checker
                    black.DrawCheckerCircles(graphics);             //Draw black checker.                                                  
                    playedCheckers[row, colLocation] = black;       //assigning the played checkers                                      
                    currentColumn = colLocation;                    //get current location
                    CurrentRow = row;                               //get current row.
                    switchPlayer();                                 //switch player.
                    break;
                }
            }
        }
        else if (currentPlayer == (int)players.Red)
        {
            for (int row = (rowsNumber - 1); row >= 0; row--)
            {
                if (playedCheckers[row, colLocation] == null)
                {
                    Checker Red = new Checker(Color.Red, (colLocation * CircleDiameter), (row * CircleDiameter), CircleDiameter);
                    Red.DrawCheckerCircles(graphics);                         
                    playedCheckers[row, colLocation] = Red;                   
                    currentColumn = colLocation;            //get current location
                    CurrentRow = row;                        //get current row.   
                    switchPlayer();                           //switch player.
                    break;
                }
            }
        }
    }

    //switch players
    private void switchPlayer()
    {
        if (currentPlayer == (int)players.Black)
            currentPlayer = (int)players.Red;    
        else
            currentPlayer = (int)players.Black;
    }

    //draw the checker of the next player
    public string drawWhoseTurn(Graphics graphics)
    {
        //draw the player's checker
        if (currentPlayer == (int)players.Black)
        {
            Checker black = new Checker(Color.Black, 560, 0, CircleDiameter);        
            black.DrawCheckerCircles(graphics);
            saved = black;
            return "Black's turn";
        }
        else
        {
            Checker Red = new Checker(Color.Red, 560, 0, CircleDiameter);
            Red.DrawCheckerCircles(graphics);
            saved = Red;
            return "Red's turn";
        }
    }

    //Display who is the winner
    public void DisplayWInner()
    {
        if (currentPlayer == (int)players.Red)
            MessageBox.Show("The Black Player Won", "Winner", MessageBoxButtons.OK, MessageBoxIcon.Information);
        else
            MessageBox.Show("The Red Player Won", "Winner", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    //check if there is the winner
    public bool isWinner()
    {
        if (checkVertical())
            return true;
        if (checkHorizontal())
            return true;
        if (CheckDiagonalUpLeftToDownRight())
            return true;
        if (CheckDiagonalUpRightToLeftDown())
            return true;
        return false;
    }

    //check the vertical won
    private bool checkVertical()
    {
        if (CurrentRow >= rowsNumber - 3)
            return false;
  
        for(int i = CurrentRow; i<=CurrentRow+3; i++)
        {
            if(playedCheckers[i,currentColumn].ColorOfCircle != playedCheckers[CurrentRow,currentColumn].ColorOfCircle)
                return false;             
        }
        return true;
    }
         
    //check if the horizontal won
    private bool checkHorizontal()
    {
        int counter = 1;
        for(int i = currentColumn-1; i>=0; i--)
        {     
            if(playedCheckers[CurrentRow, i] != null)
            {
                if((playedCheckers[CurrentRow,currentColumn].ColorOfCircle != playedCheckers[CurrentRow,i].ColorOfCircle))                   
                    break;

                counter++;
            }
            else
                break;
        }

        for(int i = currentColumn+1; i<=columnsNumber-1; i++)
        {
            if (playedCheckers[CurrentRow, i] != null)
            {
                if (playedCheckers[CurrentRow, currentColumn].ColorOfCircle != playedCheckers[CurrentRow, i].ColorOfCircle)
                    break;
                counter++;
            }
            else
                break;
        }
          
        if (counter >= 4)
            return true;
        else
            return false;    
    }

    //check if the diagonal from up left toward down right won
    private bool CheckDiagonalUpLeftToDownRight()
    {
        int counter = 1;
        int i = CurrentRow -1;          //Assign i counter with the next upper row
        int j = currentColumn - 1;      //Assign j counter with the next left column  
            
        while (i >= 0 && j >= 0)
        {
            if (playedCheckers[i, j] != null)
            {
                if (playedCheckers[CurrentRow, currentColumn].ColorOfCircle == playedCheckers[i, j].ColorOfCircle)
                {
                    counter++;
                    i--;
                    j--;
                }
                else
                    break;
            }
            else
                break;
        }
            

        i = CurrentRow + 1;          //Assign i counter with the next lower row
        j = currentColumn + 1;      //Assign j counter with the next right column

        while (i <rowsNumber && j < columnsNumber)
        {
            if (playedCheckers[i, j] != null)
            {
                if (playedCheckers[CurrentRow, currentColumn].ColorOfCircle == playedCheckers[i, j].ColorOfCircle)
                {
                    counter++;
                    i++;
                    j++;
                }
                else
                    break;
            }
            else
                break;
        }

        if (counter >= 4)
            return true;
        else        
            return false;         
    }

    //check if the diagonal from up right to left down won
    private bool CheckDiagonalUpRightToLeftDown()
    {
        int counter = 1;
        int i = CurrentRow - 1;          //Assign i counter with the next upper row
        int j = currentColumn + 1;      //Assign j counter with the next left column
      
            while (i >= 0 && j < columnsNumber)
            {
                if (playedCheckers[i, j] != null)
                {
                    if (playedCheckers[CurrentRow, currentColumn].ColorOfCircle == playedCheckers[i, j].ColorOfCircle)
                    {
                        counter++;
                        i--;
                        j++;
                    }
                    else
                        break;
                }
                else
                    break;
            }

            i = CurrentRow + 1;          //Assign i counter with the next lower row
            j = currentColumn - 1;      //Assign j counter with the next right column

            while (i < rowsNumber && j >= 0)
            {
                if (playedCheckers[i, j] != null)
                {
                    if (playedCheckers[CurrentRow, currentColumn].ColorOfCircle == playedCheckers[i, j].ColorOfCircle)
                    {
                        counter++;
                        i++;
                        j--;
                    }
                    else
                        break;
                }
                else
                    break;
            } 

        if (counter >= 4)
            return true;
        else
            return false;
    }

    //Get data from the saved file
    public void GetData(Graphics graphics, StreamReader fileReader)
    {
            int Diameter;
            int X, Y;
            Color colorToUse;

            string inputRecord = fileReader.ReadLine();
            string[] inputFields;

            //writting to the circle list.
            if (inputRecord != null)
            {
                int row = 0;
                int column = 0;
                while (inputRecord != null)
                {
                    inputFields = inputRecord.Split(',');                            
                    Diameter = Convert.ToInt32(inputFields[0]);
                    X = Convert.ToInt32(inputFields[1]);
                    Y = Convert.ToInt32(inputFields[2]);
                    colorToUse = Color.FromName(inputFields[3]);
                    Checker Saved = new Checker(colorToUse, X, Y, Diameter);                          
                    row = Y / CircleDiameter;
                    column = X / CircleDiameter;

                    if (column > 6)
                    {
                        saved = Saved;
                    }
                    else
                    {
                        playedCheckers[row, column] = Saved;      //put back the saved checker in playedchecker array
                    }

                    inputRecord = fileReader.ReadLine();
                }

                if (saved.ColorOfCircle == Color.Black)
                {
                    currentPlayer = (int)players.Black;
                    Checker black = new Checker(Color.Black, 560, 0, CircleDiameter);
                    black.DrawCheckerCircles(graphics);                                              
                }
                else
                {
                    currentPlayer = (int)players.Red;            
                    Checker Red = new Checker(Color.Red, 560, 0, CircleDiameter);
                    Red.DrawCheckerCircles(graphics);                                              
                }
            }               
    }

    //write data to the file
    public void saveData(StreamWriter fileWriter)
    {
        //save the saved checker.
        fileWriter.WriteLine(saved.ToString());

        for (int rowCounter = 0; rowCounter < rowsNumber; rowCounter++)
        {
            for (int colCounter = 0; colCounter < columnsNumber; colCounter++)
            {
                if (playedCheckers[rowCounter, colCounter] != null)
                {
                    fileWriter.WriteLine(playedCheckers[rowCounter, colCounter].ToString());
                }
            }
        }
    }
        
    //check if there is a free spot
    public bool freeSpot(int location)
    {
        if (playedCheckers[0, location] == null)
            return true;
        return false;
    }

    //redraw played checkers after reopening from the file.
    public void drawPlayedCheckers(Graphics graphics)
    {
        for (int rowCounter = 0; rowCounter < rowsNumber; rowCounter++)
        {
            for (int colCounter = 0; colCounter < columnsNumber; colCounter++)
            {
                if (playedCheckers[rowCounter, colCounter] != null)
                {
                    playedCheckers[rowCounter, colCounter].DrawCheckerCircles(graphics);
                }
            }
        }
    }
}