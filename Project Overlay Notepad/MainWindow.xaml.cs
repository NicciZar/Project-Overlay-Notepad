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

        public string LocationAndName;

        public bool pressed_ctrl;

        public MainWindow()
        {
            InitializeComponent();
            textBox.Text = DefaultTextBoxText;
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
            AssVersion = fvi.FileVersion;
            textBox.FontSize = 12;
            

            LocationAndName = System.Reflection.Assembly.GetEntryAssembly().Location;

            this.Title = "Project Overlay Notepad " + AssVersion;

            //defaults ctrl status to false
            pressed_ctrl = false;
        }

        private void textBox_KeyDown(object sender, KeyEventArgs e)
        {
            //checks if ctrl is pressed
            if (e.Key == Key.LeftCtrl)
            {
                pressed_ctrl = true;
            }
            else
            {
                pressed_ctrl = false;
            }

            if ((e.Key == Key.S) && (Keyboard.IsKeyDown(Key.LeftCtrl)))
            {
                textBox.IsEnabled = false;
                //ability to Save to a TextFile

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
                //ability to Load TextFiles into the Textbox

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
                //toggles DarkMode on/off

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

            if ((e.Key == Key.N) && (Keyboard.IsKeyDown(Key.LeftCtrl)))
            {
                textBox.IsEnabled = false;
                //Opens new Window with current executable
                System.Diagnostics.Process.Start(LocationAndName);
                textBox.IsEnabled = true;
            }

            if ((e.Key == Key.H) && (Keyboard.IsKeyDown(Key.LeftCtrl)))
            {
                textBox.IsEnabled = false;
                //Opens help/about menu
                MessageBox.Show("Hotkeys:\n\nCTRL + S = SAVE\nCTRL + L = LOAD\nCTRL + D = Darkmode (dark Editor)\nCTRL + N = New Window\n\nMade by:\nNicolas HORST\nGitHub: \nNicciZar\nVersion: " + AssVersion, "Helpmenu", MessageBoxButton.OK);
                textBox.IsEnabled = true;
            }

        }

        private void textBox_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            //Detects if Mousewheel + crtl is pressed = scrolls down or up. If font size gets too small (or too big?, tere seems to be a fixed max size for the font) if will reset to font size 1
            try
            {
                if (pressed_ctrl == true)
                {
                    if (e.Delta > 0)
                    { textBox.FontSize++; }
                    else
                    { textBox.FontSize--; }
                }
                else
                {

                }

            }
            catch (Exception)
            {
                textBox.FontSize = 1;
            }
        }

        private void textBox_KeyUp(object sender, KeyEventArgs e)
        {
            //Checks if ctrl is still pressed (if the key is not pressed anymore)
            if (e.Key == Key.LeftCtrl)
            {
                pressed_ctrl = false;
            }
            else
            {
                pressed_ctrl = true;
            }
        }
    }
}
