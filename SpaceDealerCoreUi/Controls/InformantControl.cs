using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpaceDealerCoreUi.Controls
{
	public partial class InformantControl : UserControl
	{
		public void SetMessage(string imagePath, string headline, string subHeadline)
		{
			this.pictureBox1.Load(imagePath);
			lblHeadline.Text = headline;
			lblSubHeadline.Text = subHeadline;
		}

		public InformantControl()
		{
			InitializeComponent();
		}
	}
}
