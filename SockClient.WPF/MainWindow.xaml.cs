using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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
using SockClient.Lib;

namespace SockClient.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
        }
        private void DoStartup()
        {
        }
        private void BtnSaveConfig_Click(object sender, RoutedEventArgs e)
        {
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
        }
        private void BtnStop_Click(object sender, RoutedEventArgs e)
        {
        }
        //private string SendCommand(string command)
        //{
        //}

        private void CmbInstructions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void BtnSend_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}
