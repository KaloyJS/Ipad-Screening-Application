using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using LibUsbDotNet.DeviceNotify;
using Newtonsoft.Json;
using Button = System.Windows.Controls.Button;
using ComboBox = System.Windows.Controls.ComboBox;
using MessageBox = System.Windows.Forms.MessageBox;

namespace Ipad_Screening_Application
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Declarations

        private Application_Properties App;
        public PortConnectionViewModel portConnectionViewModel;
        //LibUsbDotNet Instance
        public static IDeviceNotifier UsbDeviceNotifier = DeviceNotifier.OpenDeviceNotifier();

        #endregion

        #region MainWindow
        public MainWindow()
        {
            InitializeComponent();
            //Create Instance of Application Properties
            App = new Application_Properties();
            // Instance of view Model
            portConnectionViewModel = new PortConnectionViewModel();
            // Connect view model to datacontext for binding
            DataContext = portConnectionViewModel;
            UsbDeviceNotifier.OnDeviceNotify += OnDeviceNotifyEvent;
        }

        #endregion

        #region On connect/disconnect event handler
        /// <summary>
        /// Event handler for any connect and disconnect of USB Devices
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDeviceNotifyEvent(object sender, DeviceNotifyEventArgs e)
        {
            if (e.EventType.ToString() == "DeviceArrival")
            {
                // Device connected
                // sets event type
                App.USBEventType = USBEvent.Connected;
                if (e.Device != null)
                {
                    // sets Vid and Pid of connected device                    
                    App.DeviceVID = e.Device.IdVendor.ToString("X4");
                    App.DevicePID = e.Device.IdProduct.ToString("X4");
                    // Get details on connected device, 
                    int? connectedPort = USBClassLibrary_Methods.DeviceConnected(App, portConnectionViewModel);
                    if (connectedPort != null && connectedPort <= 2)
                    {
                        //Power_0.SelectedValue = "Yes";
                        // extract nullable int value
                        int _connectedPort = connectedPort.Value;
                        
                        // Put thread on sleep for 2 seconds for system to process connected device
                        int milliseconds = 250;
                        Thread.Sleep(milliseconds);
                        Application_Methods.OnConnectPortProcess(_connectedPort, portConnectionViewModel, App);


                    }

                }
            }
            else
            {
                // sets event type
                App.USBEventType = USBEvent.Disconnected;
                if (e.Device != null)
                {
                    // sets Vid and Pid of connected device                    
                    App.DeviceVID = e.Device.IdVendor.ToString("X4");
                    App.DevicePID = e.Device.IdProduct.ToString("X4");
                    // Get details on disconnected device, 
                    int? disconnectedPort = USBClassLibrary_Methods.DeviceDisconnected(App, portConnectionViewModel);
                    if (disconnectedPort != null && disconnectedPort <= 2)
                    {
                        // extract nullable int value
                        int _disconnectedPort = disconnectedPort.Value;                        
                        Application_Methods.OnDisconnectPortProcess(_disconnectedPort, portConnectionViewModel, App);
                    }
                }
            }
        }



        #endregion

        #region power status change handler
        private void Power_Change_Handler(object sender, SelectionChangedEventArgs e)
        {
            // Gets current button
            ComboBox current = (ComboBox)sender;
            string currentName = current.Name;
            if (current.SelectedValue != null)
            {
                string power_status = current.SelectedValue.ToString();
                // Get port from name of power combobox
                int port = Application_Methods.GetPortFromElementName(currentName);
                // if power is no then set plist to editable
                if (power_status == "No")               
                {
                  
                    portConnectionViewModel.plist[port].Status = false;
                    portConnectionViewModel.button[port].Status = true;
                }
                else if (power_status == "Yes")
                {
                    //portConnectionViewModel.plist[port].Status = true;
                    //portConnectionViewModel.button[port].Status = false;
                }
            }
        }

        #endregion


        #region submit button handler
        /// <summary>
        /// Button Handler 
        /// Modes : content = "Save device info" = push device object to php endpoint to save info
        /// content = "Print Label" = prints device object label
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void submitButton_Click(object sender, RoutedEventArgs e)
        {
            // Gets current button
            Button current = (Button)sender;
            string currentName = current.Name;
            string type = current.Content.ToString();
            int port = Application_Methods.GetPortFromElementName(currentName);


            if (type == "Click to Save")
            {
                Application_Methods.Save_data(portConnectionViewModel, port);
            }
            else if (type == "Print Label")
            {
                Application_Methods.PrintLabel(port, portConnectionViewModel);
            }
            else if (type == "Reprint Label")
            {
                Application_Methods.ReprintLabel(portConnectionViewModel, port);
            }

        }

        #endregion

       
    }
}
