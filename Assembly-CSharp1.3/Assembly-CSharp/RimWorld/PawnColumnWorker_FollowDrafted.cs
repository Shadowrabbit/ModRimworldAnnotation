using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001378 RID: 4984
	public class PawnColumnWorker_FollowDrafted : PawnColumnWorker_Checkbox
	{
		// Token: 0x06007949 RID: 31049 RVA: 0x002AF73B File Offset: 0x002AD93B
		protected override bool HasCheckbox(Pawn pawn)
		{
			return pawn.RaceProps.Animal && pawn.Faction == Faction.OfPlayer && pawn.training.HasLearned(TrainableDefOf.Obedience);
		}

		// Token: 0x0600794A RID: 31050 RVA: 0x002AF769 File Offset: 0x002AD969
		protected override bool GetValue(Pawn pawn)
		{
			return pawn.playerSettings.followDrafted;
		}

		// Token: 0x0600794B RID: 31051 RVA: 0x002AF776 File Offset: 0x002AD976
		protected override void SetValue(Pawn pawn, bool value, PawnTable table)
		{
			pawn.playerSettings.followDrafted = value;
		}
	}
}
