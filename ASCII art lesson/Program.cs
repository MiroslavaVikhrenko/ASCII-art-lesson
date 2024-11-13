using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Text;
using System.IO;

namespace ASCII_art_lesson
{
    internal class Program
    {
        //constant to ensure correct proportions are protected when we resize an image 
        private const double WIDTH_OFFSET = 1.5;
        //set max image width 
        private const int MAX_WIDTH = 600;

        //In order we can convert an image into ASCII symbols - we need to get the original image first
        //We can achieve this in different ways - it would be inconvenient to type the image path manually in console
        //That's why in sonsole I connect winforms library and will use open file dialog to be able to open files via graphical interface
        //We can do that even though the app will work from the console (Assemblies - Framework - System.Windows.Forms)

        //[STAThread] attribute is needed so that we could call ShowDialog() method from openFileDialog

        [STAThread]
        static void Main(string[] args)
        {
            //create open file dialog object when started the program
            var openFileDialog = new OpenFileDialog
            {
                //specify the filter for file types that we can open wil this graphical interface
                Filter = "Images | *.bmp; *.png; *.jpg; *.JPEG"
            };

            Console.WriteLine("Press enter to start...\n");



            //add an endless loop while to ensure that we do not need to re-start the program in case we want to add a new image
            while (true)
            {
                //before any logic will be executed, we call method ReadLine() from Console class
                //until this method has worked - the code after that will not be executed
                //method ReadLine() will work only after we enter anything into console or press enter
                //so this way we will 'pause' the execution of the program

                Console.ReadLine();

                //OPEN AN IMAGE

                //ShowDialog() method result is represented with enum 'DialogResult'
                //we are interested only in 'OK' as it's the only result that would lead to successful opening
                //if it was not 'OK' then the later code won't execute and we will start in the beginning of the loop

                if (openFileDialog.ShowDialog() != DialogResult.OK)
                    continue;

                //if we successfully opened the file, it means that we are going to open a new image 
                //we need to remember that before that we can already have some other image opened and displayed in console.
                //That's why we need to clear the console.
                Console.Clear();

                //at this point we already got access to the file via class OpenFileDialog and we can work with an image
                //in order to be able to work with an image we will need Bitmap class
                //this class is located in the library System.Drawing => we need to connect it in references

                //once the reference is added to the project we can create an object of Bitmap class
                //where we can store the data we read from the file


                //in the Bitmap() constructor we pass a path to the file that is stored in the property 'FileName' of the object of class OpenFileDialog 
                //of course it will only be there IF we successfully opened the file - but unsuccessful scenarios are handled above in the if condition
                //if we got so far => then it means that we already have an opened image and we can proceed with convertion

                var bitmap = new Bitmap(openFileDialog.FileName);

                //by its essense the image can be represented as a 2-dimensional array where each element is a pixel of a certain color
                //we can consider even our console as a 2-dim array with coordinates by X axis and Y axis

                //our goal is to somehow convert a pixel in the picture into a symbol we want and display it in the correct position in the console
                //this would be the core logic of this program

                //there are a few points to consider:

                //---> #1 FONT
                //depending on what font/font size we use, we can fit different amount of symbols in the console
                //simply put, font and fontsize settings decides the resolution of the image
                //for picture to look nice we need a small font => that's why our top message Console.WriteLine("Press enter to start...\n"); might not be visible due to small font
                //some fonts are brighter or more faded, wider or slimmer - it would also effect the way how the image is shown
                //so far I set up the font size to 8 and marked as bold => will adjust further if needed

                //---> #2 ORIGINAL IMAGE RESOLUTION
                //we can open any image and the images we open can have very different resolutions
                //theoretically we can face an issue when an opened image would not fit the console width
                //to handle it we would need to add functionality to re-size/re-scale the image that we open to ensure it fits
                

                //make sure that the size fits - calling ResizedBitmap() method where the image will be resized IF needed
                bitmap = ResizedBitmap(bitmap);

                //**************** CONVERT TO MONOCHROME ****************
                //Why do we need to turn an image into bw?
                //We will draw an image using the follwing table:
                //char[] _asciiTable = { '.', ',', ':', '+', '*', '?', '%', 'S', '#', '@' };
                //if we look closer to these simbols - they represent different bright levels like a gradient from 'black' to 'white'
                //where the most on the right is the brightest, and the most on the left is the least bright
                //we do not have room for colors in this scheme
                //so when we draw an image in console we can only operate with the brightness level of the pixel

                bitmap.ToGrayscale();

                //now bitmap ready to convert into ASCII symbols, for this purpose we create a separate class BitmapToASCIIConverter

                //create an object of BitmapToASCIIConverter class and pass a bitmap 
                var converter = new BitmapToASCIIConverter(bitmap);
                //call Convert() method and reseive as result the rows with ASCII symbols
                var rows = converter.Convert();

                //now we just need to display those rows line by line in the console
                foreach ( var row in rows )
                    Console.WriteLine(row);

                var rowNegative = converter.ConvertAsNegative();

                //connect System.IO so that we could add save file functionality
                File.WriteAllLines("image.txt", rowNegative.Select(r => new string(r)));

                //after an image is displayed in the console we need to replace the coursor of the console in the top left corner
                //so that we will see not the last lines from the drawn image but see it from the top
                Console.SetCursorPosition(0, 0);
            }

        }

        //Method to re-size the image
        private static Bitmap ResizedBitmap(Bitmap bitmap)
        {
            //we cannot change only image width otherwise the proportions will be distored
            //so if we change the width, we also need to change height
            //the below line ensures the proportions are kept the same
            var newHeight = bitmap.Height / WIDTH_OFFSET * MAX_WIDTH / bitmap.Width;


            //if the opened image by its width or height is bigger than the values we specified above, then we resize it
            if (bitmap.Width > MAX_WIDTH || bitmap.Height > newHeight)
                bitmap = new Bitmap(bitmap, new Size(MAX_WIDTH, (int)newHeight));

            //if the opened image width and height are fine, we do nothing, just return it as it is

            return bitmap;
        }
    }
}
