using System.Windows.Forms;

namespace SpaceDealerCoreUi.Controls
{
	public partial class FoundNewPlanetControl : UserControl
	{
		public void SetMessage(string imagePath, string headline, string subHeadline)
		{
			this.pictureBox1.Load(imagePath);
			lblHeadline.Text = headline;
			lblSubHeadline.Text = subHeadline;
		}

		public FoundNewPlanetControl()
		{
			InitializeComponent();
		}
	}
}
