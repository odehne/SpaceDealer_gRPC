using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Domain
{
	public class SimpleDiceRoller
	{
		private static int reRolls = 1; //This is number of dice you are rolling

		public static double GetRandomCredits(int min, int max)
		{
			Random random = new Random();
			var total = random.Next(min, max);
			return total;
		}

		public static int Roll(DiceType diceType = DiceType.d20, int modifier = 0, bool runModifierForEachDiceRoll = false)
		{
			var dice = (int)diceType;
			int total = 0;
			Random random = new Random();

			for (int i = 0; i < reRolls; i++)
			{
				total += random.Next(1, dice + 1);
			}
			total += runModifierForEachDiceRoll ? (modifier * reRolls) : modifier;

			string output = "Result: {0}    ({1}{2})";
			if (modifier > 0)
			{

				output = runModifierForEachDiceRoll ?
					"Result: {0}    ({1} x 1{2}+{3})" : "Result: {0}    ({1}{2} + {3})";
			}

			return (total / reRolls);
		}



	}

	public enum DiceType
	{
		d4 = 4,
		d6 = 6,
		d8 = 8,
		d10 = 10,
		d12 = 12,
		d20 = 20,
		d100 = 100
	}

}
