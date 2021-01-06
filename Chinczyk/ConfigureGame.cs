using System;
using System.Drawing;
using System.Windows.Forms;
using System.Resources;

namespace Chinczyk
{
    public partial class ConfigureGame : Form
    {
        public ConfigureGame()
        {
            InitializeComponent();
        }

        readonly ResourceManager _locRm = LanguageSelect.LocRm;

        private void GameLoad(object sender, EventArgs e)
        {
            checkBoxR.Text = _locRm.GetString("pCheckBoxR");
            checkBoxY.Text = _locRm.GetString("pCheckBoxY");
            checkBoxB.Text = _locRm.GetString("pCheckBoxB");
            checkBoxG.Text = _locRm.GetString("pCheckBoxG");
            buttonStart.Text = _locRm.GetString("pGameStart");
            this.Text = _locRm.GetString("pFormName");
        }

        public static int NumOfPlayers;
        public static readonly int[] Figure = { -1, -1, -1, -1 };
        public static string Red = "X", Yellow = "X", Blue = "X", Green = "X";

        private void buttonStart_Click(object sender, EventArgs e)
        {
            errorProvider.Clear();

            if(checkBoxR.Checked && textBoxR.Text == "")
                errorProvider.SetError(textBoxR, _locRm.GetString("pRedName"));
            else if(checkBoxY.Checked && textBoxY.Text == "")
                errorProvider.SetError(textBoxY, _locRm.GetString("pYellowName"));
            else if (checkBoxB.Checked && textBoxB.Text == "")
                errorProvider.SetError(textBoxB, _locRm.GetString("pBlueName"));
            else if (checkBoxG.Checked && textBoxG.Text == "")
                errorProvider.SetError(textBoxG, _locRm.GetString("pGreenName"));
            else if (NumOfPlayers < 1)
                errorProvider.SetError(buttonStart, _locRm.GetString("pMinPlayers"));
            else
            {
                if(textBoxR.Text!=string.Empty)
                    Red = textBoxR.Text;
                if(textBoxY.Text!=string.Empty)
                    Yellow = textBoxY.Text;
                if (textBoxB.Text != string.Empty)
                    Blue = textBoxB.Text;
                if (textBoxG.Text != string.Empty)
                    Green = textBoxG.Text;

                GameTable t = new GameTable();
                this.Hide();
                t.Show();
            }
        }

        private void PrepareGame(CheckBox cb1, CheckBox cb2, CheckBox cb3, CheckBox cb4, 
                                    TextBox tb1, TextBox tb2,TextBox tb3,TextBox tb4,
                                    
                                    Color activeColor, Color nonactiveColor, int numPlayer1, int numPlayer2, bool r, bool y, bool b, bool g)
        {
            errorProvider.Clear();

            if (cb1.Checked)
            {
                tb1.Enabled = true;
                tb1.BackColor = activeColor;
                
                if(y) cb2.Enabled = true;
                NumOfPlayers = numPlayer1;
            }
            else
            {
                if (r)
                {
                    tb1.BackColor = nonactiveColor;
                    tb1.Enabled = false;
                    tb1.Text = string.Empty;
                }
                if (y)
                {
                    cb2.Enabled = false;
                    tb2.Enabled = false;
                    tb2.Text = string.Empty;
                }
                if (b)
                {
                    cb3.Enabled = false;
                    tb3.Enabled = false;
                    tb3.Text = string.Empty;
                }
                if (g)
                {
                    cb4.Enabled = false;
                    tb4.Enabled = false;
                    tb4.Text = string.Empty;
                }

                if(y) cb2.Checked = false;
                if(b) cb3.Checked = false;
                if(g) cb4.Checked = false;

                
                NumOfPlayers = numPlayer2;
            }
        }

        private void checkBoxR_CheckedChanged(object sender, EventArgs e)
        {
            PrepareGame(checkBoxR, checkBoxY, checkBoxB, checkBoxG,
                                    textBoxR, textBoxY, textBoxB, textBoxG,
                                   
                                    Color.LightCoral, SystemColors.Control, 0, 0, true, true, true, true);
        }

        private void checkBoxY_CheckedChanged(object sender, EventArgs e)
        {
            PrepareGame(checkBoxY, checkBoxB, checkBoxG, null,
                             textBoxY, textBoxB, textBoxG, null,
                             
                             Color.Orange, SystemColors.Control, 1, 0, true, true, true, false);
        }

        private void checkBoxB_CheckedChanged(object sender, EventArgs e)
        {
            PrepareGame(checkBoxB, checkBoxG, null, null,
                            textBoxB, textBoxG, null, null,
                            
                            Color.SteelBlue, SystemColors.Control, 2, 1, true, true, false, false);
        }

        private void checkBoxG_CheckedChanged(object sender, EventArgs e)
        {
            PrepareGame(checkBoxG, null, null, null,
                           textBoxG, null, null, null,
                           
                           Color.DarkSeaGreen, SystemColors.Control, 3, 2, true, false, false, false);
        }
    }
}