using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpaceDealerForms.Controls
{
	public partial class DistressCallControl : UserControl
	{
		public void SetMessage(string imagePath, string headline, string subHeadline)
		{
			this.pictureBox1.Load(imagePath);
			lblHeadline.Text = headline;
			lblSubHeadline.Text = subHeadline;
		}

		public DistressCallControl()
		{
			InitializeComponent();
		}

		private void lblHeadline_Click(object sender, EventArgs e)
		{

		}
	}
}
