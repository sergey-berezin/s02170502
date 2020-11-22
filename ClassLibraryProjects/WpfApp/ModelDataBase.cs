using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ObjectsImageRecognitionLibrary;

namespace WpfApp
{
    public class Blob
    {
        public int BlobId { get; set; }
        public byte[] ImageContext { get; set; }
    }

    public class ClassLabel
    {
        public int ClassLabelId { get; set; }
        public string StringClassLabel { get; set; }
        public int ClassLabelImagesNumber { get; set; }
        public ICollection<ImageInformation> ImagesInformation { get; set; }
    }

    public class ImageInformation
    {
        public int ImageInformationId { get; set; }
        public string Path { get; set; }
        public float Probability { get; set; }
        public Blob ImageContext { get; set; }
        public ClassLabel ClassLabel { get; set; }
    }

    public class ImageObject
    {
        public string ClassLabel { get; set; }
        public float Probability { get; set; }
        public int Number { get; set; }
        public ImageObject(string label, float probability, int number)
        {
            ClassLabel = label;
            Probability = probability;
            Number = number;
        }
    }

    public class ModelContext: DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseSqlite("Data Source=model.db");
        public DbSet<Blob> ImageContext { get; set; }
        public DbSet<ClassLabel> ClassLabels { get; set; }
        public DbSet<ImageInformation> ImagesInformation { get; set; } 

        // Checking if the result is written in the Database, if so - return it
        public ImageObject DatabaseCheck(string path)
        {
            ImageObject image = null;
            bool flag = true;
            byte[] BinaryFile = File.ReadAllBytes(path);
            
            foreach(var item in ImagesInformation.Include(obj => obj.ClassLabel).Where(obj => obj.Path == path))
            {
                Entry(item).Reference(obj => obj.ImageContext).Load();
                
                if (BinaryFile.Length == item.ImageContext.ImageContext.Length)
                {
                    flag = true;
                    for (int i = 0; i <= BinaryFile.Length - 1; i++)
                    {
                        if (BinaryFile[i] != item.ImageContext.ImageContext[i])
                        {
                            flag = false;
                            break;
                        }
                    }
                }

                if (flag == true)
                {
                    image = new ImageObject(item.ClassLabel.StringClassLabel, item.Probability, item.ClassLabel.ClassLabelImagesNumber);
                    SaveChanges();
                    break;
                }
            }
            return image;
        }

        public void DatabaseAdding(ObjectInImageProbability image)
        {
            ImageInformation AddedImage = new ImageInformation
            {
                Path = image.Path,
                ImageContext = new Blob() { ImageContext = File.ReadAllBytes(image.Path)},
                Probability = image.Probability
            };

            var query = ClassLabels.Include(item => item.ImagesInformation).Where(item => item.StringClassLabel == image.ClassLabel);
            
            if (query.Count() == 0)
            {
                AddedImage.ClassLabel = new ClassLabel()
                {
                    StringClassLabel = image.ClassLabel,
                    ClassLabelImagesNumber = 1
                };
                AddedImage.ClassLabel.ImagesInformation = new List<ImageInformation>
                {
                    AddedImage
                };
                ImagesInformation.Add(AddedImage);
                ClassLabels.Add(AddedImage.ClassLabel);
                ImageContext.Add(AddedImage.ImageContext);
            }
            else
            {
                AddedImage.ClassLabel = query.First();
                AddedImage.ClassLabel.ImagesInformation.Add(AddedImage);
                AddedImage.ClassLabel.ClassLabelImagesNumber++;
                ImagesInformation.Add(AddedImage);
                ImageContext.Add(AddedImage.ImageContext);
            }
            SaveChanges();
        }

        public void DatabaseCleanup()
        {
            foreach (var context in ImageContext)
            {
                ImageContext.Remove(context);
            }

            foreach (var label in ClassLabels)
            {
                ClassLabels.Remove(label);
            }

            foreach (var information in ImagesInformation)
            {
                ImagesInformation.Remove(information);
            }

            SaveChanges();
        }
    }
}
