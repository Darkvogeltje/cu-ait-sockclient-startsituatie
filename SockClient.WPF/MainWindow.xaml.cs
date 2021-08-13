using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using SockClient.Lib;

namespace SockClient.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            DoStartup();
            FillDictionary();
        }
        private void DoStartup()
        {
            var dataTable = Helper.ReadConfigFile();
            txtPort.Text = dataTable.Rows[0]["ServerPort"].ToString();
            txtIP.Text = dataTable.Rows[0]["ServerIp"].ToString();
            txtActiveFolder.Text = "";
            grpStop.Visibility = Visibility.Hidden;
            grpStart.IsEnabled = true;
            grpConfig.IsEnabled = true;
            grpInstruction.Visibility = Visibility.Hidden;
            grpResponse.Visibility = Visibility.Hidden;

        }
        private void BtnSaveConfig_Click(object sender, RoutedEventArgs e)
        {
            int.TryParse(txtPort.Text, out var port);
            if (port == 0) port = 49200;
            if (port < 49152) port = 49200;
            if (port > 65535) port = 49200;
            txtPort.Text = port.ToString();
            var ipString = txtIP.Text.Trim();
            try
            {
                var unused = IPAddress.Parse(ipString);
            }
            catch
            {
                ipString = "127.0.0.0";
                txtIP.Text = ipString;
            }
            Helper.UpdateConfigFile(ipString, port);
            MessageBox.Show("Configuration saved", "Configuration saved to config.xml",
                MessageBoxButton.OK, MessageBoxImage.Information);

        }
        private void FillDictionary()
        {
            tbkDictionary.Text = "";
            tbkDictionary.Text += "DIRALL" + Environment.NewLine;
            tbkDictionary.Text += "DIRFILES" + Environment.NewLine;
            tbkDictionary.Text += "DIRFOLDERS" + Environment.NewLine;
            tbkDictionary.Text += "CURRENTDIR" + Environment.NewLine;
            tbkDictionary.Text += "CHANGEDIR <existing foldername>" + Environment.NewLine;
            tbkDictionary.Text += "CHANGEDIR UP" + Environment.NewLine;
            tbkDictionary.Text += "CHANGEDIR ROOT" + Environment.NewLine;
            tbkDictionary.Text += "MAKEDIR <non existing foldername>" + Environment.NewLine;
            tbkDictionary.Text += "REMOVEDIR <remove existing empty folder>" + Environment.NewLine;
            tbkDictionary.Text += "REMOVEDIRALL <remove existing folder>" + Environment.NewLine;
            tbkDictionary.Text += "RENAMEDIR <existing folder>,<new name>" + Environment.NewLine;
            tbkDictionary.Text += "CONTENTFILE <existing file>" + Environment.NewLine;
            tbkDictionary.Text += "REMOVEFILE <remove existing file>" + Environment.NewLine;
            tbkDictionary.Text += "RENAMEFILE <rename existing file>,<new name>" + Environment.NewLine;

            cmbInstructions.Items.Clear();
            cmbInstructions.Items.Add("DIRALL");
            cmbInstructions.Items.Add("DIRFILES");
            cmbInstructions.Items.Add("DIRFOLDERS");
            cmbInstructions.Items.Add("CURRENTDIR");
            cmbInstructions.Items.Add("CHANGEDIR <existing foldername>");
            cmbInstructions.Items.Add("CHANGEDIR UP");
            cmbInstructions.Items.Add("CHANGEDIR ROOT");
            cmbInstructions.Items.Add("MAKEDIR <non existing foldername>");
            cmbInstructions.Items.Add("REMOVEDIR <remove existing empty folder>");
            cmbInstructions.Items.Add("REMOVEDIRALL <remove existing folder>");
            cmbInstructions.Items.Add("RENAMEDIR <existing folder>,<new name>");
            cmbInstructions.Items.Add("CONTENTFILE <existing file>");
            cmbInstructions.Items.Add("REMOVEFILE <remove existing file>");
            cmbInstructions.Items.Add("RENAMEFILE <rename existing file>,<new name>");

        }

        private void BtnTest_Click(object sender, RoutedEventArgs e)
        {
            var answer = SendCommand("HELLO");
            if (answer.IndexOf("<CF>", StringComparison.Ordinal) > -1)
            {
                txtActiveFolder.Text = answer.Replace("<CF>", "");
                grpConfig.IsEnabled = false;
                grpStart.IsEnabled = false;
                grpStop.Visibility = Visibility.Visible;
                grpInstruction.Visibility = Visibility.Visible;
                txtArgument1.Text = "";
                txtArgument2.Text = "";
                lblArgument1.Content = "";
                lblArgument2.Content = "";
                txtArgument1.Visibility = Visibility.Hidden;
                txtArgument2.Visibility = Visibility.Hidden;
                lblArgument1.Visibility = Visibility.Hidden;
                lblArgument2.Visibility = Visibility.Hidden;
                cmbInstructions.SelectedIndex = -1;
                grpResponse.Visibility = Visibility.Visible;
            }
            else
            {
                txtActiveFolder.Text = answer;
            }
        }
        private void BtnStop_Click(object sender, RoutedEventArgs e)
        {
            string answer = SendCommand("GOODBYE");
            txtActiveFolder.Text = answer;
            grpStop.Visibility = Visibility.Hidden;
            grpStart.IsEnabled = true;
            grpConfig.IsEnabled = true;
            grpInstruction.Visibility = Visibility.Hidden;
            grpResponse.Visibility = Visibility.Hidden;

        }
        private string SendCommand(string command)
        {
            var serverIp = IPAddress.Parse(txtIP.Text);
            var serverPort = int.Parse(txtPort.Text);
            var server = new IPEndPoint(serverIp, serverPort);
            var clientHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            var clientIp = clientHostInfo.AddressList[0];
            foreach (var ip in clientHostInfo.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    clientIp = ip;
                    break;
                }
            }
            var clientSocket = new Socket(clientIp.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                clientSocket.Connect(server);
                var clientRequest = Encoding.ASCII.GetBytes(command + "##EOM");
                clientSocket.Send(clientRequest);
                var serverResponse = new byte[8019];
                var messageLength = clientSocket.Receive(serverResponse);
                return Encoding.ASCII.GetString(serverResponse, 0, messageLength).ToUpper().Trim();
            }
            catch (Exception)
            {
                return "no response from the server";
            }
        }


        private void CmbInstructions_SelectionChanged(object sender, SelectionChangedEventArgs e, string[] parts)
        {
            lblArgument1.Visibility = Visibility.Hidden;
            lblArgument2.Visibility = Visibility.Hidden;
            txtArgument1.Visibility = Visibility.Hidden;
            txtArgument2.Visibility = Visibility.Hidden;
            if (cmbInstructions.SelectedIndex == -1) return;
            if (parts.Count() >= 2)
            {
                lblArgument1.Visibility = Visibility.Visible;
                lblArgument1.Content = parts[1].Replace(">", "") + " : ";
                txtArgument1.Visibility = Visibility.Visible;
                txtArgument1.Text = "";
                txtArgument1.Focus();
            }
            if (parts.Count() == 3)
            {
                lblArgument2.Visibility = Visibility.Visible;
                lblArgument2.Content = parts[2].Replace(">", "") + " : ";
                txtArgument2.Visibility = Visibility.Visible;
                txtArgument2.Text = "";
            }

        }

        private void BtnSend_Click(object sender, RoutedEventArgs e)
        {
            if (cmbInstructions.SelectedIndex == -1) return;
            string command = cmbInstructions.SelectedItem.ToString();
            if (txtArgument1.Visibility == Visibility.Visible)
            {
                if (txtArgument1.Text.Trim() == "")
                {
                    txtArgument1.Focus();
                    return;
                }
                command += "|" + txtArgument1.Text.Trim().ToUpper();
            }
            if (txtArgument2.Visibility == Visibility.Visible)
            {
                if (txtArgument2.Text.Trim() == "")
                {
                    txtArgument2.Focus();
                    return;
                }
                command += "|" + txtArgument2.Text.Trim().ToUpper();
            }
            string response = SendCommand(command);
            if (response.IndexOf("<CF>", StringComparison.Ordinal) > -1)
            {
                txtActiveFolder.Text = response.Replace("<CF>", "");
                tbkResponse.Text = SendCommand("DIRALL");
            }
            else
                tbkResponse.Text = response;

        }
    }
}
