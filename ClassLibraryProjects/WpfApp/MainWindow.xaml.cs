using System;
using System.Windows;

namespace WpfApp
{
    public partial class MainWindow : Window
    {
        private void InterruptButton_Click(object sender, RoutedEventArgs e)
        {
            if (RecognitionViewModel != null && RecognitionViewModel.RecognitionStatus == true &&
                RecognitionViewModel.DatabaseCleaningStatus == false && RecognitionViewModel.StatisticsGettingStatus == false)
            {
                RecognitionViewModel.Stop();
            }
            else
            {
                MessageBox.Show("Program interruption is not available at the moment...");
            }
        }

        private void DatabaseCleaningButton_Click(object sender, RoutedEventArgs e)
        {
            if (RecognitionViewModel != null && RecognitionViewModel.RecognitionStatus == false &&
                RecognitionViewModel.DatabaseCleaningStatus == false && RecognitionViewModel.StatisticsGettingStatus == false)
            {
                RecognitionViewModel.DatabaseCleaning();
            }
            else
            {
                MessageBox.Show("Database cleaning is not available / is executed at the moment...");
            }
        }

        private void StatUpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if (RecognitionViewModel != null && RecognitionViewModel.RecognitionStatus == false &&
                RecognitionViewModel.DatabaseCleaningStatus == false && RecognitionViewModel.StatisticsGettingStatus == false)
            {
                RecognitionViewModel.StatisticsGetting();
            }
            else
            {
                MessageBox.Show("Statistics getting is not available / is executed at the moment...");
            }
        }

        private void FolderOpeningButton_Click(object sender, RoutedEventArgs e)
        {
            if (RecognitionViewModel != null && RecognitionViewModel.RecognitionStatus == false &&
                RecognitionViewModel.DatabaseCleaningStatus == false && RecognitionViewModel.StatisticsGettingStatus == false)
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
            else
            {
                MessageBox.Show("Folder opening is not available / is executed at the moment...");
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