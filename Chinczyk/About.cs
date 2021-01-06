using System;
using System.Windows.Forms;
using System.Resources;

namespace Chinczyk
{
    partial class About : Form
    {
        ResourceManager _locRm;

        public About(ResourceManager locRm)
        {
            _locRm = locRm;
            InitializeComponent();
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tableLayoutPanel_Paint(object sender, PaintEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void labelProductName_Click(object sender, EventArgs e)
        {
            throw new System.NotImplementedException();
        }
    }
}
