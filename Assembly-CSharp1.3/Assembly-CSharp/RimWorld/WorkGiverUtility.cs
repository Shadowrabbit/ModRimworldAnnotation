using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000804 RID: 2052
	public static class WorkGiverUtility
	{
		// Token: 0x060036CD RID: 14029 RVA: 0x00136BCC File Offset: 0x00134DCC
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
