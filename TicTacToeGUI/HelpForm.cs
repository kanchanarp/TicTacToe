using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TicTacToeGUI
{
    public partial class HelpForm : Form
    {
        string file_name;
        public HelpForm()
        {
            InitializeComponent();
        }

        private void HelpForm_Load(object sender, EventArgs e)
        {
           string curDir = Directory.GetCurrentDirectory();
            this.webHelp.Url = new Uri(String.Format("file:///{0}/Help/about.html", curDir));

            webHelp.Refresh();
        }

        private void webHelp_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }


        private void listBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (listBox1.SelectedIndex == 0)
            {
                file_name = "about.html";
            }
            else if (listBox1.SelectedIndex == 1)
            {
                file_name = "one_player.html";
            }
            else if (listBox1.SelectedIndex == 2)
            {
                file_name = "difficulty.html";
            }
            else if (listBox1.SelectedIndex == 3)
            {
                file_name = "alias.html";
            }
            else if (listBox1.SelectedIndex == 4)
            {
                file_name = "stats.html";
            }
            else if (listBox1.SelectedIndex == 5)
            {
                file_name = "two_player.html";
            }
            else if (listBox1.SelectedIndex == 6)
            {
                file_name = "net_player.html";
            }
            else {
                file_name = "about.html";
            }

            string curDir = Directory.GetCurrentDirectory();
            this.webHelp.Url = new Uri(String.Format("file:///{0}/Help/" + file_name, curDir));
            webHelp.AllowNavigation = true;
            webHelp.Update();
        }
    }
}
