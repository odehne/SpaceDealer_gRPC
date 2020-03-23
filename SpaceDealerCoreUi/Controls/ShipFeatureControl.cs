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
	public partial class ShipFeatureControl : UserControl
	{
		public ShipFeatureControl()
		{
			InitializeComponent();
		}

		public void Init(string featureName, string description)
		{
			lblHeadline.Text = featureName;
			lblSubHeadline.Text = description;
		}
	}
}
