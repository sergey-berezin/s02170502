using System;
using System.Linq;
using ObjectsImageRecognitionLibrary;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Threading;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace WpfApp
{
    public class RecognitionViewModel: INotifyPropertyChanged
    {
        private ImageRecognitionLibrary LibraryObject;
        private ModelContext model;
        private readonly object LockObject;
        readonly Dispatcher dispatcher = Dispatcher.CurrentDispatcher;
        private void EventHandler(object sender, ObjectInImageProbability StructureObject)
        {
            dispatcher.BeginInvoke(new Action(() =>
            {
                ImageCollection.Add(new ObjectInImageProbability(StructureObject.Path, StructureObject.ClassLabel, StructureObject.Probability));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ImageCollection"));
                AllClassLabels label = AllClassLabelsCollection.First(item => item.ClassLabel == StructureObject.ClassLabel);
                label.NumberOfTimes++;
                
                Task.Run(() =>
                {
                    lock (LockObject)
                    {
                        model.DatabaseAdding(StructureObject);
                    }
                });
            }));
        }

        private void ImageRecognitionInWPF()
        {
            RecognitionStatus = true;
            
            ThreadPool.QueueUserWorkItem(new WaitCallback(MagicParameter =>
            {
                foreach (var path in Directory.GetFiles(ChosenDirectoryPath, "*.jpg"))
                {
                    ImageObject ObjectCheck = model.DatabaseCheck(path);

                    dispatcher.BeginInvoke(new Action(() =>
                    {
                        if (ObjectCheck == null)
                        {
                            Task.Run(() =>
                            {
                                LibraryObject = new ImageRecognitionLibrary();
                                LibraryObject.ResultEvent += EventHandler;
                                LibraryObject.ProgramStart(path);
                            });
                        }
                        else
                        {
                            ImageCollection.Add(new ObjectInImageProbability(path, ObjectCheck.ClassLabel, ObjectCheck.Probability));
                            AllClassLabels label = AllClassLabelsCollection.First(item => item.ClassLabel == ObjectCheck.ClassLabel);
                            label.NumberOfTimes++;
                            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ImageCollection"));
                        }
                    }));
                }
                RecognitionStatus = false;
            }));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<ObjectInImageProbability> ImageCollection { get; set; }
        public ObservableCollection<ObjectInImageProbability> SingleClassLabelCollection { get; set; }
        public ObservableCollection<AllClassLabels> AllClassLabelsCollection { get; set; }
        public bool RecognitionStatus = false;
        public bool DatabaseCleaningStatus = false;
        public string ChosenDirectoryPath { get; set; }

        public RecognitionViewModel()
        {
            ImageCollection = new ObservableCollection<ObjectInImageProbability>();
            SingleClassLabelCollection = new ObservableCollection<ObjectInImageProbability>();
            AllClassLabelsCollection = new ObservableCollection<AllClassLabels>();
            model = new ModelContext();
            LockObject = new object();
            for (int i = 0; i <= 999; i++)
            {
                AllClassLabelsCollection.Add(new AllClassLabels()
                {
                    ClassLabel = ImageRecognitionLibrary.classLabels[i],
                    NumberOfTimes = 0,
                    DatabaseNumberOfTimes = 0
                });
            }
        }

        public void NewOpeningAndRecognition()
        {
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

        public void DatabaseCleaning()
        {
            if (RecognitionStatus == false)
            {
                lock (LockObject)
                {
                    DatabaseCleaningStatus = true;
                    model.DatabaseCleanup();
                    model = new ModelContext();
                    DatabaseCleaningStatus = false;
                }
            }
        }
    }

    public class AllClassLabels : INotifyPropertyChanged
    {
        private int times;
        private int DatabaseTimes;
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

        public int DatabaseNumberOfTimes
        {
            get
            {
                return DatabaseTimes;
            }

            set
            {
                DatabaseTimes = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DatabaseNumberOfTimes"));
            }
        }
    }
}