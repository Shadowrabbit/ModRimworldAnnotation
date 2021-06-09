using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018C8 RID: 6344
	public class CompTargetEffect_Manhunter : CompTargetEffect
	{
		// Token: 0x06008CB5 RID: 36021 RVA: 0x0028D8D8 File Offset: 0x0028BAD8
		public override void DoEffectOn(Pawn user, Thing target)
		{
			Pawn pawn = (Pawn)target;
			if (pawn.Dead)
			{
				return;
			}
			pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Manhunter, null, false, false, null, false);
		}
	}
}
