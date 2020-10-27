using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using System.IO;
using ObjectsImageRecognitionLibrary;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Threading;

namespace WpfApp
{

    public partial class MainWindow : Window
    {
        private ObservableCollection<ImageInformation> ImageCollection;
        private ObservableCollection<SingleClassLabel> SingleClassLabelCollection;
        private ObservableCollection<AllClassLabels> AllClassLabelsCollection;
        private ImageRecognitionLibrary LibraryObject = new ImageRecognitionLibrary();
        private bool RecognitionStatus = false;
        private string ChosenDirectoryPath { get; set; }

        private void EventHandler(object sender, ObjectInImageProbability StructureObject)
        {
            ImageInformation image = ImageCollection.First(picture => picture.ImagePath == StructureObject.Path);
            image.ImageClassLabel = StructureObject.ClassLabel;
            image.ClassLabelProbability = StructureObject.Probability;
            AllClassLabels ClassLabel = AllClassLabelsCollection.First(picture => picture.ClassLabel == image.ImageClassLabel);
            ClassLabel.NumberOfTimes++;
        }

        private void ImageRecognitionInWPF()
        { 
            ImageRecognitionLibrary LibraryObject = new ImageRecognitionLibrary();
            LibraryObject.ResultEvent += EventHandler;
            RecognitionStatus = true;
            LibraryObject.ProgramStart(ChosenDirectoryPath);
        }

        private void InterruptButton_Click(object sender, RoutedEventArgs e)
        {
            LibraryObject.RecognitionStop();
        }

        private void FolderOpeningButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog();
            if (dialog.ShowDialog(this).GetValueOrDefault())
            {
                DirectoryName.Text = dialog.SelectedPath;
                ChosenDirectoryPath = dialog.SelectedPath;
            }

            ImageCollection.Clear();
            for (int i = 0; i <= 999; i++)
            {
                AllClassLabelsCollection[i].NumberOfTimes = 0;
            }

            SingleClassLabelCollection.Clear();

            string[] filePaths = Directory.GetFiles(@ChosenDirectoryPath, "*.jpg");
            for (int i = 0; i <= filePaths.Count() - 1; i++)
            {
                ImageCollection.Add(new ImageInformation()
                {
                    Image = new BitmapImage(new Uri(filePaths[i])),
                    ImagePath = filePaths[i],
                    ImageClassLabel = "null",
                    ClassLabelProbability = -1
                });
            }
            ImageRecognitionInWPF();
        }

        private void AllClassLabelsListboxChanged(object sender, EventArgs e)
        {
            SingleClassLabelCollection.Clear();
            AllClassLabels ClassLabelElement = AllClassLabelsListBox.SelectedItem as AllClassLabels;
            if (ClassLabelElement != null)
            {
                var queue = from image in ImageCollection
                            where image.ImageClassLabel == ClassLabelElement.ClassLabel
                            select image.Image;

                foreach (BitmapImage image in queue)
                {
                    SingleClassLabelCollection.Add(new SingleClassLabel()
                    {
                        ImageOfClassLabel = image
                    });
                }
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            ImageCollection = new ObservableCollection<ImageInformation>();
            Binding ImageCollectionBinding = new Binding
            {
                Source = ImageCollection
            };
            ImagesAndInformationListBox.SetBinding(ItemsControl.ItemsSourceProperty, ImageCollectionBinding);

            SingleClassLabelCollection = new ObservableCollection<SingleClassLabel>();
            Binding SingleClassLabelCollectionBinding = new Binding
            {
                Source = SingleClassLabelCollection
            };
            SingleClassLabelImagesListBox.SetBinding(ItemsControl.ItemsSourceProperty, SingleClassLabelCollectionBinding);

            AllClassLabelsCollection = new ObservableCollection<AllClassLabels>();
            Binding AllClassLabelsCollectionBinding = new Binding
            {
                Source = AllClassLabelsCollection
            };
            AllClassLabelsListBox.SetBinding(ItemsControl.ItemsSourceProperty, AllClassLabelsCollectionBinding);
            for (int i = 0; i <= 999; i++)
            {
                AllClassLabelsCollection.Add(new AllClassLabels()
                { 
                    ClassLabel = ObjectsImageRecognitionLibrary.ImageRecognitionLibrary.classLabels[i],
                    NumberOfTimes = 0
                });
            }
    }

    }

    public class ImageInformation: INotifyPropertyChanged
    {
        private string label;
        private float probability;

        public string ImagePath { get; set; }

        public BitmapImage Image { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public string ImageClassLabel
        {
            get
            {
                return label;
            }

            set
            {
                label = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ImageClassLabel"));
            }
        }

        public float ClassLabelProbability
        {
            get
            {
                return probability;
            }

            set
            {
                probability = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ClassLabelProbability"));
            }
        }
    }

    public class SingleClassLabel: INotifyPropertyChanged
    {
        private BitmapImage image;

        public event PropertyChangedEventHandler PropertyChanged;

        public BitmapImage ImageOfClassLabel
        {
            get
            {
                return image;
            }

            set
            {
                image = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ImageOfClassLabel"));
            }
        }
    }

    public class AllClassLabels: INotifyPropertyChanged
    {
        private int times;
        public string ClassLabel { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public int NumberOfTimes
        {
            get
            {
                return times;
            }

            set
            {
                times = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("NumberOfTimes"));
            }
        }
    }
}
