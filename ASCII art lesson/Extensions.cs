using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASCII_art_lesson
{
    public static class Extensions
    {
        //add extension method for Bitmap class to convert the entire image into bw
        //note - in fact it would be not pure black and white, but instead in varios grades of gray

        //In order to convert an image from color to monochrome we need somehow each pixel convert into
        //an equivalent grey gradation
        //each pizel consists of 3 colors RGB (Red, Green, Blue) and these values are represented in numbers
        //To turn a pixel into gray we need to sum up all these values and divide by their count - meaning we need to find an average
        //and we need to perform this on each pixel for our image
        //to handle it in OOP style we need to create an extension method for Bitmap (we will add class Extensions)
        public static void ToGrayscale(this Bitmap bitmap)
        {
            //loop through all the image pixels as it's in essense a 2-dimensional array
            for (int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    //GetPixel() method takes the pixel from certain coordinates which we pass to the method as arguments
                    var pixel = bitmap.GetPixel(x, y);
                    int avg = (pixel.R + pixel.G + pixel.B) / 3;
                    //send back to this coordinates values where instead of red, green, blue colors we set these average values
                    //pixel.A - it's alpha channel for opacity - we do not change that
                    bitmap.SetPixel(x,y,Color.FromArgb(pixel.A, avg,avg,avg));
                }
            }
        }
    }
}
