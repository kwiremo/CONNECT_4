/* Rene Moise Kwibuka
 * Andy Harbert
 * ASSIGNMENT 7.
 * Due Date: October 31st, 2014
*/

using System;
using System.Drawing;

class Checker
{
        //Defining properties.
    public int X { get; set; }      // X circle location
    public int Y {get; set;}        //y circle location
    public int Diameter{get;set;}   //Diameter of the circle
    public Color ColorOfCircle { get; set;}     //colot of the circle.
        
    //constructor
    
    public Checker(Color colorToUse, int x, int y, int diameter)
    {
        X = x;
        Y = y; 
        Diameter = diameter;
        ColorOfCircle = colorToUse;
    }
        
    //Draw method
    public void DrawCheckerCircles(Graphics newGraph)
    {           
        newGraph.FillEllipse(new SolidBrush(ColorOfCircle), X, Y, Diameter, Diameter);
    }

    //To string method
    public override string ToString()
    {
        string stringToWrite;
        stringToWrite = Diameter + "," + X + "," + Y  + "," + ColorOfCircle.Name;      
        return stringToWrite;
    }
}