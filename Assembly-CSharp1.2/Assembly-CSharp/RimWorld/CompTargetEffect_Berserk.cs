using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018C3 RID: 6339
	public class CompTargetEffect_Berserk : CompTargetEffect
	{
		// Token: 0x06008CAB RID: 36011 RVA: 0x0028D758 File Offset: 0x0028B958
		public override void DoEffectOn(Pawn user, Thing target)
		{
			Pawn pawn = (Pawn)target;
			if (pawn.Dead)
			{
				return;
			}
			pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Berserk, null, true, false, null, false);
			Find.BattleLog.Add(new BattleLogEntry_ItemUsed(user, target, this.parent.def, RulePackDefOf.Event_ItemUsed));
		}
	}
}
