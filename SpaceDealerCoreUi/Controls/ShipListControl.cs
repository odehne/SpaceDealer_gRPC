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
using SpaceDealerCoreUi;

namespace SpaceDealerCoreUi.Controls
{
	public partial class ShipListControl : UserControl
	{
		public ShipListControl()
		{
			InitializeComponent();
		}

		public void Init(Ship ship)
		{
			lblShipNameAndState.Text = $"{ship.ShipName} (Status: {ship.State.ToString()})";
			pictureBox1.Load(ship.PicturePath);

			var loaded = 0.0;

			if (ship.CargoLoad != null)
			{
				foreach (var item in ship.CargoLoad.LoadedProducts)
				{
					loaded += item.TotalWeight;
				}
			}

			var currentPos = ""; 
			if (ship.Cruise != null)
			{
				currentPos = Tools.ToPosition(ship.Cruise.CurrentSector);
			}
			else
			{
				currentPos = Tools.ToPosition(ship.CurrentPlanet.Sector);
			}

			lblPositionAndFreight.Text = $"Position: {currentPos} Ladung: {ship.CargoLoad}t / {loaded.ToDecimalString()}t";

		}
	}
}
