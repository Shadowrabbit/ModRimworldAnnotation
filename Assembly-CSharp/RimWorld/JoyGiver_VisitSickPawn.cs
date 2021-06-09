using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D0F RID: 3343
	public class JoyGiver_VisitSickPawn : JoyGiver
	{
		// Token: 0x06004CA3 RID: 19619 RVA: 0x001AAE58 File Offset: 0x001A9058
		public override Job TryGiveJob(Pawn pawn)
		{
			if (!InteractionUtility.CanInitiateInteraction(pawn, null))
			{
				return null;
			}
			Pawn pawn2 = SickPawnVisitUtility.FindRandomSickPawn(pawn, JoyCategory.Low);
			if (pawn2 == null)
			{
				return null;
			}
			return JobMaker.MakeJob(this.def.jobDef, pawn2, SickPawnVisitUtility.FindChair(pawn, pawn2));
		}
	}
}
