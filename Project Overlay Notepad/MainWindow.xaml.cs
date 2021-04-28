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
using Microsoft.Win32;
using System.IO;

namespace Project_Overlay_Notepad
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {

        public bool DarkMode = false;

        public string DefaultTextBoxText = "Enter Text here...\nPress CTRL + H for Help";

        public string AssVersion;

        public MainWindow()
        {
            InitializeComponent();
            textBox.Text = DefaultTextBoxText;
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
            AssVersion = fvi.FileVersion;
            this.Title = "Project Overlay Notepad " + AssVersion;
        }

        private void textBox_KeyDown(object sender, KeyEventArgs e)
        {

            if ((e.Key == Key.S) && (Keyboard.IsKeyDown(Key.LeftCtrl)))
            {
                textBox.IsEnabled = false;

                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "Text file (*.txt)|*.txt",
                    OverwritePrompt = true,
                    ValidateNames = true
                };
                if (saveFileDialog.ShowDialog() == true)
                {
                    File.WriteAllText(saveFileDialog.FileName, textBox.Text);
                    textBox.IsEnabled = true;
                }
                textBox.IsEnabled = true;
            }

            if ((e.Key == Key.L) && (Keyboard.IsKeyDown(Key.LeftCtrl)))
            {
                textBox.IsEnabled = false;

                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Filter = "Text file (*.txt)|*.txt",
                    ValidateNames = true
                };
                if (openFileDialog.ShowDialog() == true)
                {


                    if (textBox.Text == DefaultTextBoxText)
                    {
                        textBox.Text = File.ReadAllText(openFileDialog.FileName);
                        textBox.IsEnabled = true;
                    }
                    else
                    {
                        MessageBoxResult result = MessageBox.Show("There is content in your Editor! \n\nDo you want to replace it?", "Warning", MessageBoxButton.YesNo);
                        switch (result)
                        {
                            case MessageBoxResult.Yes:
                                textBox.Text = File.ReadAllText(openFileDialog.FileName);
                                textBox.IsEnabled = true;
                                break;

                            case MessageBoxResult.No:
                                textBox.IsEnabled = true;
                                break;
                        }
                    }
                }
                textBox.IsEnabled = true;
            }

            if ((e.Key == Key.D) && (Keyboard.IsKeyDown(Key.LeftCtrl)))
            {
                textBox.IsEnabled = false;

                if (DarkMode == false)
                {
                    DarkMode = true;
                    textBox.Background = Brushes.Black;
                    textBox.Foreground = Brushes.White;
                    textBox.IsEnabled = true;
                }
                else
                {
                    DarkMode = false;
                    textBox.Background = Brushes.White;
                    textBox.Foreground = Brushes.Black;
                    textBox.IsEnabled = true;
                }
                textBox.IsEnabled = true;
            }

            if ((e.Key == Key.H) && (Keyboard.IsKeyDown(Key.LeftCtrl)))
            {
                textBox.IsEnabled = false;
                MessageBox.Show("Hotkeys:\n\nCTRL + S = SAVE\nCTRL + L = LOAD\nCTRL + D = Darkmode (dark Editor)\n\nMade by:\nNicolas HORST\nVersion: " + AssVersion, "Helpmenu", MessageBoxButton.OK);
                textBox.IsEnabled = true;
            }

        }

    }
}
