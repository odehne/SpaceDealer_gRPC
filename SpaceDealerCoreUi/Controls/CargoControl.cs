using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SpaceDealerService;

namespace SpaceDealerCoreUi.Controls
{
	public partial class CargoControl : UserControl
	{
		public CargoControl()
		{
			InitializeComponent();
		}

		public void Init(ProductInStock product)
		{
			lblProductName.Text = product.ProductName;
			lblWeight.Text = product.TotalWeight.ToDecimalString();
		}

		private void CargoControl_Load(object sender, EventArgs e)
		{

		}
	}
}
