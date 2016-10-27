/* Rene Moise Kwibuka
 * Andy Harbert
 * ASSIGNMENT 7.
 * Due Date: October 31st, 2014
*/
using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace _7_CONNECT4
{
    public partial class Form1 : Form
    {     
        Game game1;
        public Form1()
        {
            InitializeComponent();
            game1 = new Game(6,7,80);           
        }

        //panel1 event handler.
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            using (Graphics graphics = panel1.CreateGraphics())
            {
                //Drawing the course.
                game1.DrawCourse(graphics);
              
                //draw the player and show whose turn.
                label1.Text = game1.drawWhoseTurn(graphics);
                
                //Drawing the checkers in playedChecker;               
                game1.drawPlayedCheckers(graphics);
            }
        }

        //mouse down event handler
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
           using (Graphics graphics = panel1.CreateGraphics())
           {
               int colLocation;
              
               //Get the column Location.
               if (e.X > 560)
                   colLocation = 6;
               else
                    colLocation = e.X / 80;
               if (game1.freeSpot(colLocation))
               {
                   //Place checker.          
                   game1.placeChecker(graphics, colLocation);              
                   label1.Text = game1.drawWhoseTurn(graphics);

                   if (game1.isWinner())
                   {
                       game1.DisplayWInner();
                       game1 = new Game(6, 7, 80);                 
                   }
               }
           }    
        }

        //opening the file event handler
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StreamReader fileReader;      //streamReader pointer
            DialogResult result;          //holds dialog result
            string nameOfFile;            //holds the name of a file

            using (OpenFileDialog fileChooser = new OpenFileDialog())
            {
                result = fileChooser.ShowDialog();        //Getting the result from the Dialog box.
                nameOfFile = fileChooser.FileName;        //Getting the name of the file.
            }

            //procceed if the user did not cancel the dialog.
            if (result == DialogResult.OK)
            {
                if (nameOfFile == string.Empty)          //check if the file name is valid
                    MessageBox.Show("Invalid File Name", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    try
                    {
                        panel1.Invalidate();            //invalidate panel

                        //open file
                        FileStream input = new FileStream(nameOfFile, FileMode.Open, FileAccess.Read);
                        fileReader = new StreamReader(input);

                        //get data
                        using (Graphics graphics = panel1.CreateGraphics())
                        {
                            game1.GetData(graphics, fileReader);
                        }

                        //close file
                        fileReader.Close();
                    }
                    catch (Exception)     //catch format exception
                    {
                        MessageBox.Show("There was an error reading a file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }            
    }
        

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string nameOfFile;            //the name of the file
            DialogResult result;          //the pointer to the result from the Dialog
            StreamWriter fileWriter;      //StreamWriter pointer

            using (SaveFileDialog fileChooser = new SaveFileDialog())
            {
                fileChooser.CheckFileExists = false;      //if file exists ask if to owverwrite, otherwise, create one.
                result = fileChooser.ShowDialog();        //Getting the result from the Dialog box.
                nameOfFile = fileChooser.FileName;        //Getting the name of the file.
            }

            //Proceed if the result is OK and exit if the result is Cancel.
            if (result == DialogResult.OK)
            {
                if (nameOfFile == string.Empty)   //Display message error if the name of the file is empty.
                    MessageBox.Show("Invalid File Name", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                else
                {
                    try
                    {
                        //open file
                        FileStream output = new FileStream(nameOfFile, FileMode.Create, FileAccess.Write);
                        fileWriter = new StreamWriter(output);

                        //write data
                        game1.saveData(fileWriter);
                        //Close the file             
                        fileWriter.Close();
                    }
                    catch (Exception)       
                    {
                        MessageBox.Show("There was an error saving data", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        //button handler that reset the game
        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result;
            result = MessageBox.Show("Are you sure you want to start over?", "RESET", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            
            if (result == DialogResult.Yes)                   
               game1 = new Game(6, 7, 80);                    
        }
    }
}