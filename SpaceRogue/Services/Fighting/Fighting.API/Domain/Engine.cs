using Cope.SpaceRogue.Fighting.API.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Cope.SpaceRogue.Fighting.API
{
	public static class Engine
	{
		public static GalaxyModel Galaxy { get; set; }
		public static async Task Init()
		{
			Galaxy = new GalaxyModel(Factory.Mediator);
			await Galaxy.Load();
		}

		public static void Play()
		{
			do
			{
				Update();
				Thread.Sleep(1000);
			} while (true);
		}

		private static void Update()
		{
			//TODO: Add shopping updates here
		}
	}
}
