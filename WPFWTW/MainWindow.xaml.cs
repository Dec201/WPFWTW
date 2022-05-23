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
using WPFWTW.Data;
using WPFWTW.Logger;
using WPFWTW.Persistence;

namespace WPFWTW
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {



        private string inputFile;
        private string outputFile;
        private string loggingFile;


        public MainWindow()
        {
            InitializeComponent();
            MouseDown += Window_MouseDown;
        }

        private void inputFileButton_Click(object sender, RoutedEventArgs e)
        {

            var inputFileTextBox = (TextBox)FindName("inputFileTextBox");

            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();

            openFileDialog.DefaultExt = ".csv";
            openFileDialog.Filter = "txt Files (*.txt)|*.txt|CSV Files (*.csv)|*.csv";

            Nullable<bool> result = openFileDialog.ShowDialog();

            if(result == true)
            {
                inputFile = openFileDialog.FileName;
                inputFileTextBox.Text = inputFile;           
            }
        }

        private void outputFileButton_Click(object sender, RoutedEventArgs e)
        {
            var outputFileTextBox = (TextBox)FindName("outputFileTextBox");

            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();

            openFileDialog.DefaultExt = ".csv";
            openFileDialog.Filter = "txt Files (*.txt)|*.txt|CSV Files (*.csv)|*.csv";

            Nullable<bool> result = openFileDialog.ShowDialog();

            if (result == true)
            {
                outputFile = openFileDialog.FileName;
                outputFileTextBox.Text = outputFile;
            }
        }


        private void loggingFileButton_Click(object sender, RoutedEventArgs e)
        {

            var loggingFileTextBox = (TextBox)FindName("loggingFileTextBox");

            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();

            openFileDialog.DefaultExt = ".csv";
            openFileDialog.Filter = "txt Files (*.txt)|*.txt|CSV Files (*.csv)|*.csv";

            Nullable<bool> result = openFileDialog.ShowDialog();

            if (result == true)
            {
                loggingFile = openFileDialog.FileName;
                loggingFileTextBox.Text = loggingFile;
            }





            /*            var logggingFileTextBox = (System.Windows.Controls.TextBox)FindName("loggingFileTextBox");

                        FolderBrowserDialog folderDialog = new FolderBrowserDialog();

                        folderDialog.ShowDialog();

                        DialogResult result = folderDialog.ShowDialog();

                        if (result == System.Windows.Forms.DialogResult.OK)
                        {
                            loggingFile = folderDialog.SelectedPath;
                            logggingFileTextBox.Text = loggingFile;
                        }*/
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

        private void runConversionButton_Click(object sender, RoutedEventArgs e)
        {
            StartMethod();
        }

        void StartMethod()
        {

            // Application Start
            // String paths will need adjusting
            // Potential while loop to keep processing data


            if (loggingFile != null && outputFile != null && inputFile != null)
            {
                CSVDataMapper CSVDataMapper = new CSVDataMapper(new FileLogger(loggingFile), new FilePersistence(outputFile), inputFile);
                CSVDataMapper.MapCSVData();

                MessageBox.Show("Complete", "File Converted", MessageBoxButton.OK, MessageBoxImage.Information);            
                
                ResultWindow resultWindow = new ResultWindow(outputFile);
                resultWindow.Show();
            }
            else
            {
                MessageBox.Show("Make sure you have all three files selected", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }




        }

    }
}
