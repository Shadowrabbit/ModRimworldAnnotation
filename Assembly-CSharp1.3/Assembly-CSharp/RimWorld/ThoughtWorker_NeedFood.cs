using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009DC RID: 2524
	public class ThoughtWorker_NeedFood : ThoughtWorker
	{
		// Token: 0x06003E66 RID: 15974 RVA: 0x00155208 File Offset: 0x00153408
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (p.needs.food == null)
			{
				return ThoughtState.Inactive;
			}
			switch (p.needs.food.CurCategory)
			{
			case HungerCategory.Fed:
				return ThoughtState.Inactive;
			case HungerCategory.Hungry:
				return ThoughtState.ActiveAtStage(0);
			case HungerCategory.UrgentlyHungry:
				return ThoughtState.ActiveAtStage(1);
			case HungerCategory.Starving:
			{
				Hediff firstHediffOfDef = p.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Malnutrition, false);
				int num = (firstHediffOfDef == null) ? 0 : firstHediffOfDef.CurStageIndex;
				return ThoughtState.ActiveAtStage(2 + num);
			}
			default:
				throw new NotImplementedException();
			}
		}
	}
}
