using System;
using System.Windows.Forms;
using System.Resources;

namespace Chinczyk
{
    public partial class LanguageSelect : Form
    {
        public LanguageSelect()
        {
            InitializeComponent();
        }

        public static ResourceManager LocRm;

        private void buttonPl_Click(object sender, EventArgs e)
        {
            LocRm = new ResourceManager("Chinczyk.Language-pl-PL", typeof(LanguageSelect).Assembly);

            ConfigureGame pr = new ConfigureGame();
            //this.Hide();
            pr.Show();
        }

        private void buttonEng_Click(object sender, EventArgs e)
        {
            LocRm = new ResourceManager("Chinczyk.Language-en-EN", typeof(LanguageSelect).Assembly);

            ConfigureGame pr = new ConfigureGame();
            //this.Hide();
            pr.Show();
        }

        private void buttonGer_Click(object sender, EventArgs e)
        {
            LocRm = new ResourceManager("Chinczyk.Language-de-DE", typeof(LanguageSelect).Assembly);

            ConfigureGame pr = new ConfigureGame();
            //this.Hide();
            pr.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            About ab = new About(LocRm);
            ab.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
