using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Akordy
{
    /// <summary> Class that handles drawing the piano (with or without chord) </summary>
    public class Draw
    {
        private readonly MainWindow mainWindow;
        private int widthWhite; //width of white keys
        private int heightWhite; // height of white keys
        private int widthBlack; //width of black keys 
        private int heightBlack; //height of black keys
        private readonly SolidColorBrush brushRight = Brushes.DeepSkyBlue;
        private readonly SolidColorBrush brushLeft = Brushes.DeepPink;
        
        public Draw(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
        }

        /// <summary> Set width and height of piano and keys </summary>
        private void SetVariables()
        {
            if (mainWindow.WindowState == WindowState.Maximized)
            {
                mainWindow.piano.Width = mainWindow.ActualWidth;
                mainWindow.piano.Height = mainWindow.piano.Width/Values.Ratio;
            }
            else
            {
                mainWindow.piano.Width = mainWindow.Width;
                mainWindow.piano.Height = mainWindow.piano.Width/Values.Ratio;
            }
            widthWhite = (int)mainWindow.piano.Width / Values.Keys;
            heightWhite = (int)mainWindow.piano.Height; 
            widthBlack = widthWhite / 2; 
            heightBlack = heightWhite / 5 * 3;
        }

        /// <summary> draw piano without chords </summary>
        public void PlainPiano()
        {
            SetVariables();
            //draw white keys
            for (int i = 0; i < Values.Keys; i++)
            {
                Rectangle rect = WhiteKey(i, Brushes.White);
                mainWindow.piano.Children.Add(rect);
            }
            //draw black keys
            for (int i = 0; i < Values.Keys; i++)
            {
                if (i % 7 != 2 && i % 7 != 6)
                {
                    Rectangle rect = BlackKey(i, Brushes.Black);
                    mainWindow.piano.Children.Add(rect);
                }
            }
        }
        
        /// <summary> draw piano with chord (left + right hand) </summary>
        public void Chord(int left, int[]right)
        {
            SetVariables();
            int id = 0;
            //draw white keys and color those which are part of the chord
            for (int i = 0; i < Values.Keys; i++)
            {
                Rectangle rect;
                //check if key is played by left hand
                if(id == left || id == left+Values.OctaveAll)
                    rect = WhiteKey(i, brushLeft);
                
                //check if key is played by right hand
                else if(right.Contains(id))
                    rect = WhiteKey(i, brushRight);
                
                else
                    rect = WhiteKey(i, Brushes.White);

                mainWindow.piano.Children.Add(rect);
                
                //if E or B is drawn, id is increased by 1 (because there is no black key between E, F and B, C)
                if (i % Values.Octave == 2 || i % Values.Octave == 6)
                    id++;
                
                //otherwise id is increased by 2 (because there is a black key between those two white keys)
                else
                    id += 2;
            }

            id = 0;
            //draw black keys and color those which are part of the chord
            for (int i = 0; i < Values.Keys; i++)
            {
                id++;
                
                //if key with this id is black
                if (i % 7 != 2 && i % 7 != 6)
                {
                    Rectangle rect;
                    //check if key is played by left hand
                    if (id == left || id == left + Values.OctaveAll)
                        rect = BlackKey(i, brushLeft);
                    
                    //check if key is played by right hand
                    else if(right.Contains(id))
                        rect = BlackKey(i, brushRight);
                    
                    else
                        rect = BlackKey(i, Brushes.Black);

                    mainWindow.piano.Children.Add(rect);
                    id++;
                }
            }
        }
       
        /// <summary> draw one white key </summary>
        /// <param name="id"> id of this key </param>
        /// <param name="color"> color of the key (white if it's not played, colored if it's part of the current chord </param>
        private Rectangle WhiteKey(int id, SolidColorBrush color)
        {
            Rectangle rect = new Rectangle
            {
                Stroke = Brushes.Black, Fill = color, StrokeThickness = 1, Width = widthWhite, Height = heightWhite,
            };
            Canvas.SetLeft(rect, id * widthWhite);
            Canvas.SetTop(rect, 0);
            return rect;
        }
        
        /// <summary> draw one black key </summary>
        /// <param name="id"> id of this key </param>
        /// <param name="color"> color of the key (black if it's not played, colored if it's part of the current chord </param>
        private Rectangle BlackKey(int id, SolidColorBrush color)
        {
            Rectangle rect = new Rectangle
            {
                Stroke = Brushes.Black, Fill = color, StrokeThickness = 1, Width = widthBlack, Height = heightBlack,
            };
            int shift = widthWhite / 2 + widthBlack / 2; //shift of black keys
            Canvas.SetLeft(rect, id * widthWhite + shift);
            Canvas.SetTop(rect, 0);
            return rect;
        }
    }
}