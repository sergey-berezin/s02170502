using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Linq;

namespace WpfApp
{
    public partial class MainWindow : Window
    {
        private void InterruptButton_Click(object sender, RoutedEventArgs e)
        {
            if (RecognitionViewModel != null && RecognitionViewModel.RecognitionStatus == true && RecognitionViewModel.DatabaseCleaningStatus == false)
            {
                RecognitionViewModel.Stop();
            }
        }

        private void DatabaseCleaningButton_Click(object sender, RoutedEventArgs e)
        {
            if (RecognitionViewModel != null && RecognitionViewModel.RecognitionStatus == false && RecognitionViewModel.DatabaseCleaningStatus == false)
            {
                RecognitionViewModel.DatabaseCleaning();
            }
        }

        private void StatUpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if (RecognitionViewModel != null && RecognitionViewModel.RecognitionStatus == false && RecognitionViewModel.DatabaseCleaningStatus == false)
            {
                RecognitionViewModel.StatisticsGetting();
            }
        }

        private void FolderOpeningButton_Click(object sender, RoutedEventArgs e)
        {
            if (RecognitionViewModel != null && RecognitionViewModel.RecognitionStatus == false && RecognitionViewModel.DatabaseCleaningStatus == false)
            {
                var dialog = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog();
                if (dialog.ShowDialog(this).GetValueOrDefault())
                {
                    DirectoryName.Text = dialog.SelectedPath;
                    RecognitionViewModel.ChosenDirectoryPath = DirectoryName.Text;
                }

                RecognitionViewModel.RecognitionStatus = false;
                RecognitionViewModel.NewOpeningAndRecognition();
            }
        }

        private void AllClassLabelsListboxChanged(object sender, EventArgs e)
        {
            RecognitionViewModel.SingleClassLabelCollection.Clear();
            if (AllClassLabelsListBox.SelectedItem is AllClassLabels ClassLabelElement)
            {
                RecognitionViewModel.CollectionFilter(ClassLabelElement);
            }
        }

        public RecognitionViewModel RecognitionViewModel;

        public MainWindow()
        {
            InitializeComponent();
            RecognitionViewModel = new RecognitionViewModel();
            this.DataContext = RecognitionViewModel;
            RecognitionViewModel.RecognitionStatus = false;
            RecognitionViewModel.DatabaseCleaningStatus = false;
        }

    }
}