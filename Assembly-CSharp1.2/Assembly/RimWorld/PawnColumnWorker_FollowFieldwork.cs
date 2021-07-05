using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B63 RID: 7011
	public class PawnColumnWorker_FollowFieldwork : PawnColumnWorker_Checkbox
	{
		// Token: 0x06009A8A RID: 39562 RVA: 0x00066DE6 File Offset: 0x00064FE6
		protected override bool HasCheckbox(Pawn pawn)
		{
			return pawn.RaceProps.Animal && pawn.Faction == Faction.OfPlayer && pawn.training.HasLearned(TrainableDefOf.Obedience);
		}

		// Token: 0x06009A8B RID: 39563 RVA: 0x00066E2F File Offset: 0x0006502F
		protected override bool GetValue(Pawn pawn)
		{
			return pawn.playerSettings.followFieldwork;
		}

		// Token: 0x06009A8C RID: 39564 RVA: 0x00066E3C File Offset: 0x0006503C
		protected override void SetValue(Pawn pawn, bool value)
		{
			pawn.playerSettings.followFieldwork = value;
		}
	}
}
