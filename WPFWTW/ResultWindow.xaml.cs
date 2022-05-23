using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;

namespace WPFWTW
{
    /// <summary>
    /// Interaction logic for ResultWindow.xaml
    /// </summary>
    public partial class ResultWindow : Window
    {
        public ResultWindow(string outputFile)
        {
            InitializeComponent();
            MouseDown += Window_MouseDown;

            if (outputFile != null)
            {
                Paragraph paragraph = new Paragraph();
                paragraph.Inlines.Add(File.ReadAllText(outputFile));
                FlowDocument document = new FlowDocument(paragraph);
                outputFileReader.Document = document;
            }
            else
            {
                Paragraph paragraph = new Paragraph();
                paragraph.Inlines.Add("Nothing to see");
                FlowDocument document = new FlowDocument(paragraph);
                outputFileReader.Document = document;
            }

        }

        private void returnToMainWindow_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }
    }
}
