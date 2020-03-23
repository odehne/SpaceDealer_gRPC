using System.Windows.Forms;

namespace SpaceDealerForms.Controls
{
	public partial class TravellingControl : UserControl
	{
		public void SetMessage(string imagePath, string headline, string subHeadline)
		{
			this.pictureBox1.Load(imagePath);
			lblHeadline.Text = headline;
			lblSubHeadline.Text = subHeadline;
		}

		public TravellingControl()
		{
			InitializeComponent();
		}
	}
}
