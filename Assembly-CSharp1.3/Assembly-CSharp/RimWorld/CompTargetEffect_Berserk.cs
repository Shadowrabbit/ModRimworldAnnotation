using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020011E1 RID: 4577
	public class CompTargetEffect_Berserk : CompTargetEffect
	{
		// Token: 0x06006E6D RID: 28269 RVA: 0x00250268 File Offset: 0x0024E468
		public override void DoEffectOn(Pawn user, Thing target)
		{
			Pawn pawn = (Pawn)target;
			if (pawn.Dead)
			{
				return;
			}
			pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Berserk, null, true, false, null, false, false, false);
			Find.BattleLog.Add(new BattleLogEntry_ItemUsed(user, target, this.parent.def, RulePackDefOf.Event_ItemUsed));
		}
	}
}
