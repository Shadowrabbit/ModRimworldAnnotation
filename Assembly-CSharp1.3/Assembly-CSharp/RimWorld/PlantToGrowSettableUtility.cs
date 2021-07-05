using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D11 RID: 3345
	public static class PlantToGrowSettableUtility
	{
		// Token: 0x06004E4B RID: 20043 RVA: 0x001A4395 File Offset: 0x001A2595
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
