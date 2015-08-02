using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace TicTacToeGUI
{
    public partial class MainForm : Form
    {
        dataCon db = new dataCon();
        ArrayList lisPlayer=null;
        TicTacToe game;

        #region Form initialization
        public MainForm()
        {
            InitializeComponent();
        }
        public void setGame(TicTacToe game) {
            this.game = game;
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            resetListBox();
        }
        private void resetListBox()
        {
            lbPlayers.Items.Clear();
            db.ConnectDB();
            lisPlayer = db.runMyCommand("Select * from player");
            Console.WriteLine(lisPlayer.Count);
            if (lisPlayer != null)
            {
                foreach (Object[] ob in lisPlayer)
                {
                    Console.WriteLine(ob[1].ToString());
                    lbPlayers.Items.Add(ob[1].ToString());
                }
            }
        }
        #endregion

        #region Button actions
        private void btnNew_Click(object sender, EventArgs e)
        {
            txtNew.Enabled = true;
            btnSave.Enabled = true;
            txtNew.Select();
        }

        
        private void btnSave_Click(object sender, EventArgs e)
        {
            Regex regex = new Regex(@"([A-Za-z0-9\-]+)$");
            if (regex.IsMatch(txtNew.Text))
            {
                string query = "Insert into Player(Username) Values('" + txtNew.Text + "')";
                bool success = db.writeCommand(query);
                if (success)
                {
                    txtNew.Enabled = false;
                    btnSave.Enabled = false;
                    resetListBox();
                    int id = Int32.Parse(((Object[])lisPlayer[lisPlayer.Count - 1])[0].ToString());
                    string query2 = "Insert into Hard(id) Values(" + id + ")";
                    string query3 = "Insert into Medium(id) Values(" + id + ")";
                    string query4 = "Insert into Easy(id) Values(" + id + ")";
                    db.writeCommand(query2);
                    db.writeCommand(query3);
                    db.writeCommand(query4);
                    game.setPlayerID(id);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Unexpected error occured in saving the player. Please try again later.");
                }

            }
            else {
                MessageBox.Show("Invalid player name. Name can only have alphanumerical characters.");
                txtNew.Select();
            }
            
        }

        private void btnAno_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            int i = lbPlayers.SelectedIndex;
            if (i >= 0)
            {
                int id = Int32.Parse(((Object[])lisPlayer[i])[0].ToString());
                game.setPlayerID(id);
                this.Close();
            }
            else {
                MessageBox.Show("Please select an alias or play anonymous.");
            }

        }
        #endregion

    }
}
