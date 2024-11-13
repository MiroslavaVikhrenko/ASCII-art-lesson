﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ASCII_art_lesson
{
    internal class Program
    {
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
                Filter = "Images | *.bmp, *.png, *.jpg, *.JPEG"
            };

            //open the dialog when program starts
            openFileDialog.ShowDialog();
        }
    }
}
