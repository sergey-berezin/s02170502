using System;
using System.Windows;

namespace WpfApp
{

    public partial class MainWindow : Window
    {
        private void InterruptButton_Click(object sender, RoutedEventArgs e)
        {
            if (RecognitionViewModel != null && RecognitionViewModel.RecognitionStatus == true)
            {
                RecognitionViewModel.Stop();
            }
        }

        private void DatabaseCleaningButton_Click(object sender, RoutedEventArgs e)
        {
            RecognitionViewModel.DatabaseCleaning();
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
        }

    }
}