using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ObjectsImageRecognitionLibrary;
using LibraryContracts;

namespace WebApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RecognitionController : ControllerBase
    {
        [HttpPost]
        public List<RecognizedImage> Post([FromBody] string path)
        {
            using var LibraryObject = new ModelContext();
            List<RecognizedImage> RecognizedImagesList = new List<RecognizedImage>();
            ObjectsImageRecognitionLibrary.ImageRecognitionLibrary ImageLibraryObject = new ObjectsImageRecognitionLibrary.ImageRecognitionLibrary();
            ImageLibraryObject.ProgramStart(path);
            foreach (var directory in Directory.GetFiles(path, "*.jpg"))
            {
                var ByteImage = from item in LibraryObject.ImagesInformation
                                where item.Path == directory
                                select item.ImageContext.ImageContext;
                var StringImage = Convert.ToBase64String(ByteImage.First());
                ImageObject result = LibraryObject.DatabaseCheck(directory);
                RecognizedImagesList.Add(new RecognizedImage()
                {
                    Path = directory,
                    Image = StringImage,
                    ClassLabel = result.ClassLabel,
                    Probability = result.Probability
                });
            }

            return RecognizedImagesList;
        }

        [HttpGet]
        public List<DatabaseGet> Get()
        {
            using var LibraryObject = new ModelContext();
            List<DatabaseGet> images = new List<DatabaseGet>();
            foreach(var item in LibraryObject.ClassLabels)
            {
                images.Add(new DatabaseGet(item.StringClassLabel, item.ClassLabelImagesNumber));
            }
            return images;
        }


        [HttpDelete]
        public void Delete()
        {
            using var LibraryObject= new ModelContext();
            LibraryObject.DatabaseCleanup();
        }
    }
}
