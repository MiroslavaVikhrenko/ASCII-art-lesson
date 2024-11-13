using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ASCII_art_lesson
{
    public class BitmapToASCIIConverter
    {
        //add an _asciiTable char array to store the grey gradients represented in char symbols
        private readonly char[] _asciiTable = { '.', ',', ':', '+', '*', '?', '%', 'S', '#', '@' };
        private readonly char[] _asciiTableNegative = { '@', '#', 'S', '%', '?', '*', '+', ':', ',', '.' };

        //add a _bitmap field as reference to the bitmap which we will get in constructor
        private readonly Bitmap _bitmap;

        //add constructor which will take as parameter a Bitmap object which we will be converting
        public BitmapToASCIIConverter(Bitmap bitmap)
        {
            _bitmap = bitmap;
        }

        public char[][] Convert()
        {
            return Convert(_asciiTable);
        }

        public char[][] ConvertAsNegative()
        {
            return Convert(_asciiTableNegative);
        }

        //Method to convert a bw image into char symbols
        //returns 2-dimensional array => 2-dimensional array is a more convenient way to work with pixels
        //each row in an array will represent a single line that we will display in console
        //looping through one dimension of the array, the other dimension we will pass to Console.WriteLine()
        //and the entire line of symbols will be drawn
        private char[][] Convert(char[] asciiTable)
        {
            //declare an array, 0 dimension size will be the height of the image
            var result = new char[_bitmap.Height][];
            //convert the image logic
            //external loop going through the image vertically
            for (int y = 0; y < _bitmap.Height; y++)
            {
                //for each row in the 2-dimensional array we will create a nested array which will be as the width of bitmap
                //meaning result[y] represents one line that we will draw in console
                result[y] = new char[_bitmap.Width];

                //loop through elements of the image by its width
                //this way by external loop we go through the image by rows
                //and by nested internal loop we go through each row of the image by width, by each pixel
                //and all pizels we go through we will form into a ready row line which would consists of the char symbols from _asciiTable
                for (int x = 0; x < _bitmap.Width; x++)
                {
                    //how to figure out which pixel should be represented with which _asciiTable symbol?
                    //in ToGrayscale() method we already averaged the values for RGB and now these values are the same
                    //moreover the value type is an integer
                    //depending on what number it is the pixel may be either brighter or more faded
                    //the brightness span that the pixel can take in bitmap is from 0 to 255
                    //So, we can take a brightness value from any of RGB values and choose from _asciiTable the most suitable symbol
                    //In _asciiTable the span is from 0 to 9 (= 10 symbols)
                    //we need to take a value from span 0-255 and find a corresponding position in a different span (0-9)
                    //after we find the correct value - we can take a symbol stored by this index from _asciiTable and
                    //display this symbol instead of the pixel and we will use Map() method for this
                    //we can use this method to get an index of the array so that we can take a symbol for drawing

                    int mapIndex = (int)Map(_bitmap.GetPixel(x, y).R,0,255,0,asciiTable.Length - 1);

                    //in the result array we will have char symbols which we will display in console

                    result[y][x] = asciiTable[mapIndex];
                }
            }
           
            return result;
        }

        //Method to map from one span to another
        //valueToMap - original value which we want to map another span
        //start1, stop1 - original values for original span (0-255 in our case) = span for pixel brightness in bitmap
        //start2, stop2 - same for the other span (0-9) for _asciiTable array

        private float Map(float valueToMap, float start1, float stop1, float start2, float stop2)
        {
            return ((valueToMap - start1) / (stop1 - start1)) * (stop2 - start2) + start2;
        }
    }
}
