using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D78 RID: 3448
	public class CompAbilityEffect_TransferEntropy : CompAbilityEffect
	{
		// Token: 0x17000DD6 RID: 3542
		// (get) Token: 0x06004FE8 RID: 20456 RVA: 0x001ABB05 File Offset: 0x001A9D05
		public new CompProperties_AbilityTransferEntropy Props
		{
			get
			{
				return (CompProperties_AbilityTransferEntropy)this.props;
			}
		}

		// Token: 0x06004FE9 RID: 20457 RVA: 0x001ABB14 File Offset: 0x001A9D14
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
			Log.Error("CompAbilityEffect_TransferEntropy is only applicable to pawns.");
		}

		// Token: 0x06004FEA RID: 20458 RVA: 0x001ABB9E File Offset: 0x001A9D9E
		public override bool GizmoDisabled(out string reason)
		{
			if (this.parent.pawn.psychicEntropy.EntropyValue <= 0f)
			{
				reason = "AbilityNoEntropyToDump".Translate();
				return true;
			}
			return base.GizmoDisabled(out reason);
		}

		// Token: 0x06004FEB RID: 20459 RVA: 0x001ABBD8 File Offset: 0x001A9DD8
		public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
		{
			Pawn pawn = target.Pawn;
			return pawn == null || AbilityUtility.ValidateNoMentalState(pawn, throwMessages);
		}
	}
}
