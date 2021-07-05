using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007E9 RID: 2025
	public class JoyGiver_VisitSickPawn : JoyGiver
	{
		// Token: 0x06003642 RID: 13890 RVA: 0x0013350C File Offset: 0x0013170C
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
