using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200134F RID: 4943
	public static class PlantToGrowSettableUtility
	{
		// Token: 0x06006B53 RID: 27475 RVA: 0x000490B3 File Offset: 0x000472B3
		public static Command_SetPlantToGrow SetPlantToGrowCommand(IPlantToGrowSettable settable)
		{
			return new Command_SetPlantToGrow
			{
				defaultDesc = "CommandSelectPlantToGrowDesc".Translate(),
				hotKey = KeyBindingDefOf.Misc12,
				settable = settable
			};
		}
	}
}
