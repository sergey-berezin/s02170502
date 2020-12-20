using System;
using System.Collections.Generic;
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
        public List<RecognizedImage> Post([FromBody] List<StringPathAndImage> strings)
        {
            using var LibraryObject = new ModelContext();
            List<RecognizedImage> RecognizedImagesList = new List<RecognizedImage>();
            ObjectsImageRecognitionLibrary.ImageRecognitionLibrary ImageLibraryObject = new ObjectsImageRecognitionLibrary.ImageRecognitionLibrary();
            ImageLibraryObject.ProgramStart(strings);
            foreach (var directory in strings)
            {
                var ByteImage = from item in LibraryObject.ImagesInformation
                                where item.Path == directory.Path
                                select item.ImageContext.ImageContext;
                var StringImage = Convert.ToBase64String(ByteImage.First());
                ImageObject result = LibraryObject.DatabaseCheck(directory.Path);
                RecognizedImagesList.Add(new RecognizedImage()
                {
                    Path = directory.Path,
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
            foreach (var item in LibraryObject.ClassLabels)
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
