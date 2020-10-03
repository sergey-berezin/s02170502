using System;
using ObjectsImageRecognitionLibrary;

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Type on the keyboard the name of the directory with the images to recognize and press \"Enter\": ");
            string directory = Console.ReadLine();
            ObjectsImageRecognitionLibrary.ImageRecognitionLibrary.ProgramStart(directory);
        }
    }
