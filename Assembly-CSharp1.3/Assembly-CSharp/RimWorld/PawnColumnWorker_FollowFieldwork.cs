using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001379 RID: 4985
	public class PawnColumnWorker_FollowFieldwork : PawnColumnWorker_Checkbox
	{
		// Token: 0x0600794D RID: 31053 RVA: 0x002AF73B File Offset: 0x002AD93B
		protected override bool HasCheckbox(Pawn pawn)
		{
			return pawn.RaceProps.Animal && pawn.Faction == Faction.OfPlayer && pawn.training.HasLearned(TrainableDefOf.Obedience);
		}

		// Token: 0x0600794E RID: 31054 RVA: 0x002AF784 File Offset: 0x002AD984
		protected override bool GetValue(Pawn pawn)
		{
			return pawn.playerSettings.followFieldwork;
		}

		// Token: 0x0600794F RID: 31055 RVA: 0x002AF791 File Offset: 0x002AD991
		protected override void SetValue(Pawn pawn, bool value, PawnTable table)
		{
			pawn.playerSettings.followFieldwork = value;
		}
	}
}
