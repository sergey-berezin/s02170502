/*using System;
using ObjectsImageRecognitionLibrary;

class Program
{
    // Event for output
    public static void EventHandler(object sender, ObjectInImageProbability structureObject)
    {
        Console.WriteLine($"Looks like it is a {structureObject.ClassLabel} at {structureObject.Path} with probability of {structureObject.Probability}%.");
    }

    static void Main(string[] args)
    {
        // Getting directory from console
        Console.WriteLine("Type on the keyboard the name of the existing directory (otherwise, the default directory will be used) with the images to recognize and press \"Enter\":");
        string directory = Console.ReadLine();

        // Library object creation, subscription to the event and recognition of objects in image
        ImageRecognitionLibrary LibraryObject = new ImageRecognitionLibrary();
        LibraryObject.ResultEvent += EventHandler;
        LibraryObject.ProgramStart(directory);
    }
}
*/