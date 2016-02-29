using AIBot.Control;
using AIBot.Display;
using AIBot.Object;
using System;
using System.Collections.Generic;
using System.IO.Ports;
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

namespace AIBot
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public class BoolToStringConverter : BoolToValueConverter<String> { }
    
    public partial class MainWindow : Window
    {
        public MainViewModel model;//{ get; set; }
        public Motions motion;// { get; set; }
        public UARTController control;// { get; set; }
        public JoystickController joy;// { get; set; }
        public SerialPort port;//{ get; set; }
        public bool IsConnected { get; set; }
        public MainWindow()
        {            
            InitializeComponent();
            model = MainViewModel.GetInstance();
            //------- plz remove it if real sutiation --------//
            control = new UARTController(new SerialPort(),true);
            motion = new Motions(control);
            //------------------------------------------------//
            joy = new JoystickController(JoyEventFireMode.Toggle);
            joy.OnKey += Joy_OnKey;
            joy.StartPulling();
            IsConnected = false;
            btConnect.Content = "Connect";     
        }
        private void btConnect_Click(object sender, RoutedEventArgs e)
        {
            if (IsConnected)
            {                
                ClosePort();
                IsConnected = false;
                btConnect.Content = "Connect";
            }
            else
            {
                if (OpenPort())
                {
                    IsConnected = true;
                    btConnect.Content = "Disconnect";
                }
                else
                {
                    ClosePort();
                }                
            }
        }
        private bool OpenPort()
        {
            if (comboPortName.SelectedIndex == -1 || string.IsNullOrEmpty(comboPortName.Items[comboPortName.SelectedIndex].ToString()))
            {
                comboPortName.Focus();
                comboPortName.IsDropDownOpen = true;
                return false;
            }
            try
            {
                port = new SerialPort(comboPortName.Items[comboPortName.SelectedIndex].ToString(), 115200);
                control = new UARTController(port);
                motion = new Motions(control);
                port.NewLine = "\r\n";
                port.Open();
                Application.Current.Dispatcher.BeginInvoke((Action)delegate ()
                {
                    motion.BaseBody();
                });
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        private void ClosePort()
        {
            try {
                port.Close();
            }catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        private void Joy_OnKey(object sender, JoyStickEvent key)
        {
            switch (key.key)
            {
                case JoyKey.RUP:
                    Application.Current.Dispatcher.BeginInvoke((Action)delegate ()
                    {
                        if(key.Value == 128)
                            motion.BodyUp();
                    });
                    break;
                case JoyKey.RDOWN:
                    Application.Current.Dispatcher.BeginInvoke((Action)delegate ()
                    {
                        if(key.Value == 128)
                            motion.BodyDown();
                    });
                    break;
                case JoyKey.RLEFT:
                    Application.Current.Dispatcher.BeginInvoke((Action)delegate ()
                    {

                    });
                    break;
                case JoyKey.RRIGHT:
                    Application.Current.Dispatcher.BeginInvoke((Action)delegate ()
                    {

                    });
                    break;
                case JoyKey.L1:
                    Application.Current.Dispatcher.BeginInvoke((Action)delegate ()
                    {
                        if (key.Value == 128)
                            motion.StartRotage(true);
                        else
                            motion.EndRotage();
                    });
                    break;
                case JoyKey.R1:
                    Application.Current.Dispatcher.BeginInvoke((Action)delegate ()
                    {
                        if (key.Value == 128)
                            motion.StartRotage(false);
                        else
                            motion.EndRotage();
                    });
                    break;
                case JoyKey.LUP:
                    Application.Current.Dispatcher.BeginInvoke((Action)delegate ()
                    {
                        if (key.Value == 128)
                        {
                            motion.StartWalk1(WalkDirection.Up);
                        }
                        else
                        {
                            motion.EndWalk1();
                        }
                    });
                    break;
                case JoyKey.LDOWN:
                    Application.Current.Dispatcher.BeginInvoke((Action)delegate ()
                    {
                        if (key.Value == 128)
                        {
                            motion.StartWalk1(WalkDirection.Down);
                        }
                        else
                        {
                            motion.EndWalk1();
                        }
                    });
                    break;
                case JoyKey.LLEFT:
                    Application.Current.Dispatcher.BeginInvoke((Action)delegate ()
                    {
                        if (key.Value == 128)
                        {
                            motion.StartWalk1(WalkDirection.Left);
                        }
                        else
                        {
                            motion.EndWalk1();
                        }
                    });
                    break;
                case JoyKey.LRIGHT:
                    Application.Current.Dispatcher.BeginInvoke((Action)delegate ()
                    {
                        if (key.Value == 128)
                        {
                            motion.StartWalk1(WalkDirection.Right);
                        }
                        else
                        {
                            motion.EndWalk1();
                        }
                    });
                    break;
                case JoyKey.Start:
                    Application.Current.Dispatcher.BeginInvoke((Action)delegate ()
                    {
                        if (key.Value == 128)
                        {                            
                            motion.TestLeg();                            
                        }
                        //motion.Cal90Up();
                    });
                    break;
            }
        }
        private void btBodyUp_Click(object sender, RoutedEventArgs e)
        {
            motion.BodyUp();
        }

        private void btBodyDown_Click(object sender, RoutedEventArgs e)
        {
            motion.BodyDown();
        }

        private void btForward_Click(object sender, RoutedEventArgs e)
        {
            motion.StartWalk1(WalkDirection.Up);
        }

        private void btBack_Click(object sender, RoutedEventArgs e)
        {
            motion.StartWalk1(WalkDirection.Down);
        }

        private void btLeft_Click(object sender, RoutedEventArgs e)
        {
            motion.StartWalk1(WalkDirection.Left);
        }
        private void btRight_Click(object sender, RoutedEventArgs e)
        {
            motion.StartWalk1(WalkDirection.Right);
        }
        private void btEndWalk_Click(object sender, RoutedEventArgs e)
        {
            motion.EndWalk1();
            motion.EndRotage();
        }
        private void button8_Click(object sender, RoutedEventArgs e)
        {
            motion.TestLeg();
        }
        
    }
}
