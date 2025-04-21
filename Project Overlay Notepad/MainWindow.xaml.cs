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
using System.Runtime;

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

        public string LocationAndName;

        public bool pressed_ctrl = false;

        public MainWindow()
        {
            InitializeComponent();
            textBox.Text = DefaultTextBoxText;

            setWindowTitle();
            
            textBox.FontSize = 12;

            LocationAndName = System.Reflection.Assembly.GetEntryAssembly().Location;

        }

        private void setWindowTitle()
        {
            // Sets the title of the window to the current text in the textbox
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
            AssVersion = fvi.FileVersion;
            this.Title = "Project Overlay Notepad " + AssVersion;
        }

        private void textBox_KeyDown(object sender, KeyEventArgs e)
        {

            switch (e.Key)
            {
                case Key.LeftCtrl:
                    pressed_ctrl = true;
                    break;

                case Key.S when Keyboard.IsKeyDown(Key.LeftCtrl):
                    textBox.IsEnabled = false;
                    // ability to Save to a TextFile
                    SaveFileDialog saveFileDialog = new SaveFileDialog
                    {
                        Filter = "Text file (*.txt)|*.txt",
                        OverwritePrompt = true,
                        ValidateNames = true
                    };
                    if (saveFileDialog.ShowDialog() == true)
                    {
                        File.WriteAllText(saveFileDialog.FileName, textBox.Text);
                    }
                    textBox.IsEnabled = true;
                    break;

                case Key.L when Keyboard.IsKeyDown(Key.LeftCtrl):
                    textBox.IsEnabled = false;
                    // ability to Load TextFiles into the Textbox
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
                        }
                        else
                        {
                            MessageBoxResult result = MessageBox.Show("There is content in your Editor! \n\nDo you want to replace it?", "Warning", MessageBoxButton.YesNo);
                            if (result == MessageBoxResult.Yes)
                            {
                                textBox.Text = File.ReadAllText(openFileDialog.FileName);
                            }
                        }
                    }
                    textBox.IsEnabled = true;
                    break;

                case Key.D when Keyboard.IsKeyDown(Key.LeftCtrl):
                    textBox.IsEnabled = false;
                    // toggles DarkMode on/off
                    if (!DarkMode)
                    {
                        DarkMode = true;
                        textBox.Background = Brushes.Black;
                        textBox.Foreground = Brushes.White;
                    }
                    else
                    {
                        DarkMode = false;
                        textBox.Background = Brushes.White;
                        textBox.Foreground = Brushes.Black;
                    }
                    textBox.IsEnabled = true;
                    break;

                case Key.N when Keyboard.IsKeyDown(Key.LeftCtrl):
                    textBox.IsEnabled = false;
                    // Opens new Window with current executable
                    System.Diagnostics.Process.Start(LocationAndName);
                    textBox.IsEnabled = true;
                    break;

                case Key.H when Keyboard.IsKeyDown(Key.LeftCtrl):
                    textBox.IsEnabled = false;
                    // Opens help/about menu
                    MessageBox.Show("Hotkeys:\n\nCTRL + S = SAVE\nCTRL + L = LOAD\nCTRL + D = Darkmode (dark Editor)\nCTRL + N = New Window\n\nMade by:\nNicolas HORST\nGitHub: \nNicciZar\nVersion: " + AssVersion, "Helpmenu", MessageBoxButton.OK);
                    textBox.IsEnabled = true;
                    break;

                default:
                    pressed_ctrl = false;
                    break;
            }


        }

        private void textBox_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            //Detects if Mousewheel + crtl is pressed = scrolls down or up. If font size gets too small (or too big?, tere seems to be a fixed max size for the font) if will reset to font size 1
            try
            {
                if (pressed_ctrl)
                {
                    if (e.Delta > 0)
                    {
                        textBox.FontSize++;
                    }
                    else
                    {
                        textBox.FontSize--;
                    }
                }
            }
            catch (Exception ex)
            {
                //Ignores the fontsize 0 error (helps with not crashing the program)
                textBox.FontSize = 1;
                Console.WriteLine(ex.Message + " - has been ignored");
            }
        }

        private void textBox_KeyUp(object sender, KeyEventArgs e)
        {
            // Resets pressed_ctrl only when the Ctrl key is released
            if (e.Key == Key.LeftCtrl)
            {
                pressed_ctrl = false;
            }
        }
    }
}
