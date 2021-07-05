using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020011EA RID: 4586
	public class CompTargetEffect_Manhunter : CompTargetEffect
	{
		// Token: 0x06006E7F RID: 28287 RVA: 0x002504C4 File Offset: 0x0024E6C4
		public override void DoEffectOn(Pawn user, Thing target)
		{
			Pawn pawn = (Pawn)target;
			if (pawn.Dead)
			{
				return;
			}
			if (!pawn.Awake())
			{
				RestUtility.WakeUp(pawn);
			}
			pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Manhunter, null, false, false, null, false, false, false);
		}
	}
}
