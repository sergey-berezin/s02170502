using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ObjectsImageRecognitionLibrary;

namespace WebApplication.Controllers
{
    public class RecognizedImage
    {
        public string Path { get; set; }
        public string Image { get; set; }
        public string ClassLabel { get; set; }
        public string Probability { get; set; }
    }

    [ApiController]
    [Route("[controller]")]
    public class RecognitionController : ControllerBase
    {
        [HttpPost]
        public List<RecognizedImage> Post([FromBody] string path)
        {
            List<RecognizedImage> RecognizedImagesList = new List<RecognizedImage>();
            ObjectsImageRecognitionLibrary.ImageRecognitionLibrary ImageLibraryObject = new ObjectsImageRecognitionLibrary.ImageRecognitionLibrary();
            ImageLibraryObject.ProgramStart(path);
            using var LibraryObject = new ModelContext();
            foreach (var directory in Directory.GetFiles(path, "*.jpg"))
            {
                var ByteImage = from item in LibraryObject.ImagesInformation
                              where item.Path == directory
                              select item.ImageContext.ImageContext;
                var StringImage = Convert.ToBase64String(ByteImage.First());
                ImageObject result = LibraryObject.DatabaseCheck(path);
                RecognizedImagesList.Add(new RecognizedImage()
                {
                    Path = directory,
                    Image = StringImage,
                    ClassLabel = result.ClassLabel,
                    Probability = result.Probability.ToString()
                });
            }

            return RecognizedImagesList;
        }
        [HttpGet]
        public IEnumerable<string> Get()
        {
            using var LibraryObject = new ModelContext();
            var queue = from item in LibraryObject.ClassLabels
                        select "Class label: " + item.StringClassLabel + ". Database number of times: " + item.ClassLabelImagesNumber.ToString();
            return queue.ToArray();
        }


        [HttpDelete]
        public void Delete()
        {
            using var LibraryObject= new ModelContext();
            LibraryObject.DatabaseCleanup();
        }
    }
}
