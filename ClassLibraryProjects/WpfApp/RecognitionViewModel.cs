using System;
using System.Linq;
using ObjectsImageRecognitionLibrary;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Threading;
using System.Threading;

namespace WpfApp
{
    public class RecognitionViewModel: INotifyPropertyChanged
    {
        private ImageRecognitionLibrary LibraryObject;
        readonly Dispatcher dispatcher = Dispatcher.CurrentDispatcher;

        private void EventHandler(object sender, ObjectInImageProbability StructureObject)
        {
            dispatcher.BeginInvoke(new Action(() =>
            {
                ImageCollection.Add(StructureObject);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ImageCollection"));
                AllClassLabels ClassLabel = AllClassLabelsCollection.First(picture => picture.ClassLabel == StructureObject.ClassLabel);
                ClassLabel.NumberOfTimes++;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AllClassLabelsCollection"));
            }));
        }

        private void ImageRecognitionInWPF()
        {
            RecognitionStatus = true;
            ThreadPool.QueueUserWorkItem(new WaitCallback(MagicParameter =>
            {
                LibraryObject.ProgramStart(ChosenDirectoryPath);
                dispatcher.BeginInvoke(new Action(() =>
                {
                    RecognitionStatus = false;
                }));
            }));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<ObjectInImageProbability> ImageCollection { get; set; }
        public ObservableCollection<ObjectInImageProbability> SingleClassLabelCollection { get; set; }
        public ObservableCollection<AllClassLabels> AllClassLabelsCollection { get; set; }
        public bool RecognitionStatus = false;
        public string ChosenDirectoryPath { get; set; }

        public RecognitionViewModel()
        {
            ImageCollection = new ObservableCollection<ObjectInImageProbability>();
            SingleClassLabelCollection = new ObservableCollection<ObjectInImageProbability>();
            AllClassLabelsCollection = new ObservableCollection<AllClassLabels>();
            for (int i = 0; i <= 999; i++)
            {
                AllClassLabelsCollection.Add(new AllClassLabels()
                {
                    ClassLabel = ImageRecognitionLibrary.classLabels[i],
                    NumberOfTimes = 0
                });
            }
        }

        public void NewOpeningAndRecognition()
        {
            LibraryObject = new ImageRecognitionLibrary();
            LibraryObject.ResultEvent += EventHandler;
            ImageCollection.Clear();
            SingleClassLabelCollection.Clear();
            for (int i = 0; i <= 999; i++)
            {
                AllClassLabelsCollection[i].NumberOfTimes = 0;
            }

            ImageRecognitionInWPF();
        }

        public void Stop()
        {
            LibraryObject.RecognitionStop();
            RecognitionStatus = false;
        }

        public void CollectionFilter(AllClassLabels classLabelElement)
        {
            foreach(ObjectInImageProbability element in ImageCollection.Where(image => image.ClassLabel == classLabelElement.ClassLabel))
            {
                SingleClassLabelCollection.Add(element);
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SingleClassLabelCollection"));
        }
    }

    public class AllClassLabels : INotifyPropertyChanged
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