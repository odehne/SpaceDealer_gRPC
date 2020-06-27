using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SpaceDealerCoreUi
{
	public partial class frmPlayerName : Form
	{
        public string OldName { get; set; }

        public frmPlayerName()
        {
            InitializeComponent();
        }

        public void SetPlayerName(string playerName)
        {
            txtNewName.Text = playerName;
        }


        public string GetPlayerName()
        {
            return txtNewName.Text;
        }

        private void OnOK(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OnCancel(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
