using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B62 RID: 7010
	public class PawnColumnWorker_FollowDrafted : PawnColumnWorker_Checkbox
	{
		// Token: 0x06009A86 RID: 39558 RVA: 0x00066DE6 File Offset: 0x00064FE6
		protected override bool HasCheckbox(Pawn pawn)
		{
			return pawn.RaceProps.Animal && pawn.Faction == Faction.OfPlayer && pawn.training.HasLearned(TrainableDefOf.Obedience);
		}

		// Token: 0x06009A87 RID: 39559 RVA: 0x00066E14 File Offset: 0x00065014
		protected override bool GetValue(Pawn pawn)
		{
			return pawn.playerSettings.followDrafted;
		}

		// Token: 0x06009A88 RID: 39560 RVA: 0x00066E21 File Offset: 0x00065021
		protected override void SetValue(Pawn pawn, bool value)
		{
			pawn.playerSettings.followDrafted = value;
		}
	}
}
