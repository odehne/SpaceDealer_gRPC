using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpaceDealerService
{
	public class SimpleDiceRoller
	{
		private static int NumberOfDice = 12; //This is number of dice you are rolling
		private static int Dice = (int)DiceType.d20; //This is the type of dice you are rolling
		private static int modifier = 0; //Use this if you need to add something per dice roll
		private static bool runModifierForEachDiceRoll = false; //true if the modifier is added on to every dice roll, false if it's added on once at the end


		public static int Roll()
		{
			int total = 0;
			Random random = new Random();

			for (int i = 0; i < Dice; i++)
			{
				total += random.Next(1, 12 + 1);
			}
			total += runModifierForEachDiceRoll ? (modifier * NumberOfDice) : modifier;

			string output = "Result: {0}    ({1}{2})";
			if (modifier > 0)
			{

				output = runModifierForEachDiceRoll ?
					"Result: {0}    ({1} x 1{2}+{3})" : "Result: {0}    ({1}{2} + {3})";
			}

			Console.WriteLine(String.Format(output, total, NumberOfDice, DiceType.GetName(typeof(DiceType), Dice), modifier));
			return total;
		}
		private enum DiceType
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
	
}
