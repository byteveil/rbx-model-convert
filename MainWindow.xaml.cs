using System.IO;
using System.Windows;
using Microsoft.Win32;
using RobloxFiles;

namespace RbxModelConvert
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ConvertButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Roblox Files|*.rbxm;*.rbxmx|All Files|*.*",
                Title = "Select a Roblox file"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                FilePathTextBox.Text = filePath;

                ResultTextBlock.Text = "Processing...";
                RobloxFile robloxFile = RobloxFile.Open(filePath);

                switch (robloxFile)
                {
                    case BinaryRobloxFile:
                        {
                            ResultTextBlock.Text = "Converting from BinaryRobloxFile";
                            RobloxFile xmlResult = new XmlRobloxFile();
                            foreach (Instance robloxInstance in robloxFile.GetChildren())
                            {
                                robloxInstance.Parent = xmlResult;
                            }
                            string savePath = filePath.Replace(".rbxm", ".rbxmx");
                            xmlResult.Save(savePath);
                            ResultTextBlock.Text = $"Conversion complete. File saved at: {savePath}";
                            break;
                        }

                    case XmlRobloxFile:
                        {
                            ResultTextBlock.Text = "Converting from Xml";
                            RobloxFile binaryResult = new BinaryRobloxFile();
                            foreach (Instance robloxInstance in robloxFile.GetChildren())
                            {
                                robloxInstance.Parent = binaryResult;
                            }
                            string savePath = filePath.Replace(".rbxmx", ".rbxm");
                            binaryResult.Save(savePath);
                            ResultTextBlock.Text = $"Conversion complete. File saved at: {savePath}";
                            break;
                        }
                }
            }
            else
            {
                ResultTextBlock.Text = "File selection canceled.";
            }
        }
    }
}
