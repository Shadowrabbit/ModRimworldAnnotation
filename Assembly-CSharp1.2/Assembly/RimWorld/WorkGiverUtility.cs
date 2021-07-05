using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000DAE RID: 3502
	public static class WorkGiverUtility
	{
		// Token: 0x06004FD6 RID: 20438 RVA: 0x001B5470 File Offset: 0x001B3670
		public static Job HaulStuffOffBillGiverJob(Pawn pawn, IBillGiver giver, Thing thingToIgnore)
		{
			foreach (IntVec3 c in giver.IngredientStackCells)
			{
				Thing thing = pawn.Map.thingGrid.ThingAt(c, ThingCategory.Item);
				if (thing != null && thing != thingToIgnore)
				{
					return HaulAIUtility.HaulAsideJobFor(pawn, thing);
				}
			}
			return null;
		}
	}
}
