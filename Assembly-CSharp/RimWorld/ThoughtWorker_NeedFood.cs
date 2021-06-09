using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E7D RID: 3709
	public class ThoughtWorker_NeedFood : ThoughtWorker
	{
		// Token: 0x06005343 RID: 21315 RVA: 0x001C0230 File Offset: 0x001BE430
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
