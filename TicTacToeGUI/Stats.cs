using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TicTacToeGUI
{
    public partial class Stats : Form
    {
        private TicTacToe mf;
        private dataCon db = new dataCon();
        public Stats(TicTacToe mf)
        {
            InitializeComponent();
            this.mf = mf;
        }

        private void Stats_Load(object sender, EventArgs e)
        {
            ArrayList list;
            db.ConnectDB();
            int id = mf.getPlayerID();
            string query = "SELECT Username FROM Player WHERE id=" + id + ";";
            list = db.runMyCommand(query);
            textBox1.Text = ((Object)list[0]).ToString();
            string query1 = "SELECT * FROM Hard WHERE id=" + id + ";";
            list = db.runMyCommand(query1);
            DH.Text = ((Object[])list[0])[3].ToString();
            LH.Text = ((Object[])list[0])[2].ToString();
            WH.Text = ((Object[])list[0])[1].ToString();
            string query2 = "SELECT * FROM Medium WHERE id=" + id + ";";
            list = db.runMyCommand(query2);
            DM.Text = ((Object[])list[0])[3].ToString();
            LM.Text = ((Object[])list[0])[2].ToString();
            WM.Text = ((Object[])list[0])[1].ToString();
            string query3 = "SELECT * FROM Easy WHERE id=" + id + ";";
            list = db.runMyCommand(query3);
            DE.Text = ((Object[])list[0])[3].ToString();
            LE.Text = ((Object[])list[0])[2].ToString();
            WE.Text = ((Object[])list[0])[1].ToString();
        }
    }
}
