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
    public partial class TicTacToe : Form
    {
        private int click_c = 1,onePlayer=0,plaX=0;
        private bool boolClient = false, boolServer=false;
        private AlgoAI al = new AlgoAI();
        private PictureBox[,] pBox = new PictureBox[3, 3];
        private int playerID=-1;
        private string dataTab = "Hard";
        private dataCon db = new dataCon();
        
        public TicTacToe()
        {
            InitializeComponent();
            
            db.ConnectDB();
            if (playerID == -1) {
                statsToolStripMenuItem.Enabled = false;
            }
            pBox[0,0]=pictureBox1;pBox[0,1]=pictureBox2;pBox[0,2]=pictureBox3;
            pBox[1,0]=pictureBox4;pBox[1,1]=pictureBox5;pBox[1,2]=pictureBox6;
            pBox[2,0]=pictureBox7;pBox[2,1]=pictureBox8;pBox[2,2]=pictureBox9;
            al.init();
            initNet();
            al.setDifficulty(Difficulty.HARD);
        }

        #region Set Network Game Parameters
        public clsServer clsServ;
        public clsClient clsCli;
        public void initNet() {
            clsServ = new clsServer(this);
            clsCli = new clsClient(this);
        }
        public void setPlay(int Play) {
            plaX = Play;
        }
        public void setPlayerID(int id) {
            playerID = id;
        }
        public int getPlayerID()
        {
            return playerID ;
        }
        public void netWorkPlay(int[] loc,int play){
            if (boolServer)
            {
                if (play == 1)
                {
                    clsServ.SendMove(loc[0], loc[1], 1);
                }
                else {
                    clsServ.SendMove(loc[0], loc[1], 0);
                }
                
            }
            else if(boolClient) {
                if (play == 1)
                {
                    clsCli.SendMove(loc[0], loc[1], 1);
                }
                else {
                    clsCli.SendMove(loc[0], loc[1], 0);
                }
            }
            if (pBox[loc[0], loc[1]] != null)
            {
                pBox[loc[0], loc[1]].Enabled = false;
                int win = al.getWin();
                if (win == 1)
                {
                    MessageBox.Show("Player 1 win"); endGame(); pBox[loc[0], loc[1]].Image = null;
                }
                else if (win == -1)
                {
                    MessageBox.Show("Player 2 win"); endGame(); pBox[loc[0], loc[1]].Image = null;
                }

            }
        }
        public void netPlay(int[] loc,int type)
        {
            if (type == 1)
            {
                pBox[loc[0],loc[1]].ImageLocation = "tic-tac-toe-X.png";
                click_c += 1;
                al.getInput(-1, loc[0], loc[1]);

            }
            else
            {
                pBox[loc[0], loc[1]].ImageLocation = "tic-tac-toe-O.png";
                click_c += 1;
                al.getInput(1, loc[0], loc[1]);
            }
            if (pBox[loc[0], loc[1]] != null)
            {
                pBox[loc[0], loc[1]].Enabled = false;
                int win = al.getWin();
                if (win == 1)
                {
                    MessageBox.Show("Player 1 win"); endGame(); pBox[loc[0], loc[1]].Image = null;
                }
                else if (win == -1)
                {
                    MessageBox.Show("Player 2 win"); endGame(); pBox[loc[0], loc[1]].Image = null;
                }

            }
        }
        #endregion

        #region Main game events
        public void clicAct(PictureBox pB,int[] loc) {
             if (onePlayer == 1)
            {
                if (plaX == 1)
                {
                    if (click_c == 1)
                    {
                        al.mark(1, 0, 0);
                        pBox[0, 0].ImageLocation = "tic-tac-toe-X.png";
                        pBox[0, 0].Enabled = false;
                        click_c += 1;
                    }
                    else {

                        pB.ImageLocation = "tic-tac-toe-O.png";
                            click_c += 1;
                            al.getInput(-1, loc[0], loc[1]);
                            String s = al.playXCont();
                            pB.Enabled = false;
                            pBox[al.getlastMarked()[0], al.getlastMarked()[1]].ImageLocation = "tic-tac-toe-X.png";
                                click_c += 1;
                                pBox[al.getlastMarked()[0], al.getlastMarked()[1]].Enabled = false;
                                string field;
                            if (!s.Equals(""))
                            {
                                
                                if (s.Equals("You win")) {
                                    MessageBox.Show(this, "Game over! You win.", "TicTac", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    field = "Won";
                                }
                                else if (s.Equals("You lose"))
                                {
                                    MessageBox.Show(this, "Game over! You Lose", "TicTac", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    field = "Lost";
                                }
                                else {
                                    MessageBox.Show(this, "Game over! It's a draw", "TicTac", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    field = "Drawn";
                                }
                                if (playerID != -1)
                                {
                                    string queryRead = "select " + field + " from " + dataTab + " where Id=" + playerID;
                                    ArrayList ar = db.runMyCommand(queryRead);
                                    int gameVal = Int32.Parse(((Object)(ar[0])).ToString());
                                    gameVal = gameVal + 1;
                                    string queryWrite = "Update " + dataTab + " set " + field + "= " + gameVal + " where Id=" + playerID;
                                    db.writeCommand(queryWrite);
                                }
                                endGame();
                            }
                    }
                }
                else if (plaX == 0) {
                    
                        pB.ImageLocation = "tic-tac-toe-X.png";
                        click_c += 1;
                        al.getInput(1, loc[0], loc[1]);
                        pB.Enabled = false;
                        String s = al.playOCont(al.getlastMarked()[0], al.getlastMarked()[1], click_c-1);

                        if (s.Equals("")||click_c<9)
                        {
                            pBox[al.getlastMarked()[0], al.getlastMarked()[1]].ImageLocation = "tic-tac-toe-O.png";
                            click_c += 1;
                            pBox[al.getlastMarked()[0], al.getlastMarked()[1]].Enabled = false;
                        }
                            string field;
                        if (!s.Equals(""))
                        {
                            if (s.Equals("You win"))
                            {
                                MessageBox.Show(this, "Game over! You win.", "TicTac", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                field = "Won";
                            }
                            else if (s.Equals("You lose"))
                            {
                                MessageBox.Show(this, "Game over! You Lose", "TicTac", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                field = "Lost";
                            }
                            else
                            {
                                MessageBox.Show(this, "Game over! It's a draw", "TicTac", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                field = "Drawn";
                            }
                            if (playerID != -1)
                            {
                                string queryRead = "select " + field + " from " + dataTab + " where Id=" + playerID;
                                ArrayList ar = db.runMyCommand(queryRead);
                                int gameVal = Int32.Parse(((Object[])ar[0]).ToString());
                                gameVal = gameVal + 1;
                                string queryWrite = "Update " + dataTab + " set " + field + "= " + gameVal + " where Id=" + playerID;
                                db.writeCommand(queryWrite);
                            }
                            endGame();
                        }
                    
                    
                }
            }else if(boolServer||boolClient){
                if (click_c % 2 == plaX)
                {
                    if (plaX == 1)
                    {
                        pB.ImageLocation = "tic-tac-toe-X.png";
                        click_c += 1;
                        al.getInput(-1, loc[0], loc[1]);

                    }
                    else if (plaX == 0)
                    {
                        pB.ImageLocation = "tic-tac-toe-O.png";
                        click_c += 1;
                        al.getInput(1, loc[0], loc[1]);

                    }
                    netWorkPlay(loc, plaX);
                    if (pB != null)
                    {
                        pB.Enabled = false;
                        int win = al.getWin();
                        if (win == 1)
                        {
                            MessageBox.Show("Player 1 win"); endGame(); pB.Image = null;
                        }
                        else if (win == -1)
                        {
                            MessageBox.Show("Player 2 win"); endGame(); pB.Image = null;
                        }

                    }
                }
                
            }
            else
            {
                if (click_c % 2 == 1)
                {
                    pB.ImageLocation = "tic-tac-toe-X.png";
                    click_c += 1;
                    al.getInput(-1, loc[0], loc[1]);
                    
                }
                else
                {
                    pB.ImageLocation = "tic-tac-toe-O.png";
                    click_c += 1;
                    al.getInput(1, loc[0], loc[1]);
                    
                }
                
                if (pB != null)
                {
                    pB.Enabled = false;
                    int win = al.getWin();
                    if (win == 1)
                    {
                        MessageBox.Show("Player 1 win"); endGame(); pB.Image = null;
                    }
                    else if(win==-1){
                        MessageBox.Show("Player 2 win"); endGame(); pB.Image = null; 
                    }
                    
                }
            }
             



        }
        public void endGame() {
            
            PictureBox[] pbArr = {pictureBox1, pictureBox2, pictureBox3, pictureBox4, pictureBox5, pictureBox6, pictureBox7, pictureBox8, pictureBox9 };
            click_c = 1;
            foreach (PictureBox pb in pbArr) {
                pb.Image = null;
                pb.Enabled = true;
            }
            al.setBack();
        }
        #endregion

        #region Graphics events
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            int[] loc = { 0, 0 };
            clicAct(pictureBox1,loc);
           
        }

       
       
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            int[] loc = { 0, 1 };
            clicAct(pictureBox2, loc);
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            int[] loc = { 0, 2 };
            clicAct(pictureBox3, loc);
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            int[] loc = { 1, 0 };
            clicAct(pictureBox4, loc);
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            int[] loc = { 1, 1 };
            clicAct(pictureBox5, loc);
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            int[] loc = { 1, 2 };
            clicAct(pictureBox6, loc);
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            int[] loc = { 2, 0 };
            clicAct(pictureBox7, loc);
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            int[] loc = { 2, 1 };
            clicAct(pictureBox8, loc);
        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {
            int[] loc = { 2, 2 };
            clicAct(pictureBox9, loc);
        }
        #endregion

        #region Game option
        

        private void playXToolStripMenuItem_Click(object sender, EventArgs e)
        {
            endGame();
            onePlayer = 1;
            plaX = 1;
            clicAct(null, null);
        }

        private void playOToolStripMenuItem_Click(object sender, EventArgs e)
        {
            endGame();
            onePlayer = 1;
            plaX = 0;
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void TicTacToe_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show( "Do you want to exit?", "Confirm Exit", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                e.Cancel=true;
            }
            else {
                Disconnect();
                this.Dispose();
            }
            

        }

        private void playXToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            boolServer = true;
            clsServ.StartServer();
            plaX = 1;
            clsServ.setPlay(plaX);
        }
        
        private void connectToAServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            boolClient = true;
            clsCli.ConnectServer();
        }

        


        private void hardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            hardToolStripMenuItem.Checked = true;
            mediumToolStripMenuItem.Checked = false;
            easyToolStripMenuItem.Checked = false;
            al.setDifficulty(Difficulty.HARD);
            dataTab = "Hard";
        }

        private void mediumToolStripMenuItem_Click(object sender, EventArgs e)
        {
            hardToolStripMenuItem.Checked = false;
            mediumToolStripMenuItem.Checked = true;
            easyToolStripMenuItem.Checked = false;
            al.setDifficulty(Difficulty.MEDIUM);
            dataTab = "Medium";
        }

        private void easyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            hardToolStripMenuItem.Checked = false;
            mediumToolStripMenuItem.Checked = false;
            easyToolStripMenuItem.Checked = true;
            al.setDifficulty(Difficulty.EASY);
            dataTab = "Easy";
        }

        private void selectAliasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainForm mf = new MainForm();
            mf.setGame(this);
            mf.Activated += new EventHandler(mf_Activated);
            mf.FormClosed += new FormClosedEventHandler(mf_FormClosed);
            mf.Show();
            
        }

        private void mf_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Enabled = true;
            if (playerID == -1)
            {
                statsToolStripMenuItem.Enabled = false;
            }
            else {
                statsToolStripMenuItem.Enabled = true;
            }
        }

        private void mf_Activated(object sender, EventArgs e)
        {
            this.Enabled = false;
        }

        private void statsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Stats mf = new Stats(this);
            mf.Activated += new EventHandler(mf_Activated);
            mf.FormClosed += new FormClosedEventHandler(mf_FormClosed);
            mf.Show();
        }

        private void playOToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            boolServer = true;
            clsServ.StartServer();
            plaX = 0;
            clsServ.setPlay(plaX);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About abFrm = new About();
            abFrm.Activated += new EventHandler(mf_Activated);
            abFrm.FormClosed += new FormClosedEventHandler(mf_FormClosed);
            abFrm.Show();
        }



        private void disconnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (boolClient) {
                clsCli.Disconnect();
            }
            else if (boolServer) {
                clsServ.Disconnect();
            }
        }

        #endregion

        private void userGuideToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HelpForm mf = new HelpForm();
            mf.Activated += new EventHandler(mf_Activated);
            mf.FormClosed += new FormClosedEventHandler(mf_FormClosed);
            mf.Show();
        }

        private void TicTacToe_Load(object sender, EventArgs e)
        {

        }
        public void Disconnect() {
            if (boolClient)
            {
                clsCli.Disconnect();
                
            }
            else if (boolServer)
            {
                clsServ.Disconnect();
            }
            onePlayer = 0;
        }
        private void twoPlayerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Disconnect();
            boolClient = false;
            boolServer = false;

        }

    }

}
