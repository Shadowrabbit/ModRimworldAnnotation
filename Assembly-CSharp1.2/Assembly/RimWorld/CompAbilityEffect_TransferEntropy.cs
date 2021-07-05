using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001399 RID: 5017
	public class CompAbilityEffect_TransferEntropy : CompAbilityEffect
	{
		// Token: 0x170010D1 RID: 4305
		// (get) Token: 0x06006CD4 RID: 27860 RVA: 0x0004A01F File Offset: 0x0004821F
		public new CompProperties_AbilityTransferEntropy Props
		{
			get
			{
				return (CompProperties_AbilityTransferEntropy)this.props;
			}
		}

		// Token: 0x06006CD5 RID: 27861 RVA: 0x002167E0 File Offset: 0x002149E0
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			Pawn pawn = target.Pawn;
			if (pawn != null)
			{
				Pawn pawn2 = this.parent.pawn;
				if (this.Props.targetReceivesEntropy)
				{
					pawn.psychicEntropy.TryAddEntropy(pawn2.psychicEntropy.EntropyValue, pawn2, false, true);
				}
				pawn2.psychicEntropy.RemoveAllEntropy();
				MoteMaker.MakeInteractionOverlay(ThingDefOf.Mote_PsychicLinkPulse, this.parent.pawn, pawn);
				return;
			}
			Log.Error("CompAbilityEffect_TransferEntropy is only applicable to pawns.", false);
		}

		// Token: 0x06006CD6 RID: 27862 RVA: 0x0004A02C File Offset: 0x0004822C
		public override bool GizmoDisabled(out string reason)
		{
			if (this.parent.pawn.psychicEntropy.EntropyValue <= 0f)
			{
				reason = "AbilityNoEntropyToDump".Translate();
				return true;
			}
			return base.GizmoDisabled(out reason);
		}

		// Token: 0x06006CD7 RID: 27863 RVA: 0x00215328 File Offset: 0x00213528
		public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
		{
			Pawn pawn = target.Pawn;
			return pawn == null || AbilityUtility.ValidateNoMentalState(pawn, throwMessages);
		}
	}
}
