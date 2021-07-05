using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000D59 RID: 3417
	public class CompAbilityEffect_Reassure : CompAbilityEffect
	{
		// Token: 0x17000DC1 RID: 3521
		// (get) Token: 0x06004F8D RID: 20365 RVA: 0x001AA5A9 File Offset: 0x001A87A9
		public new CompProperties_AbilityReassure Props
		{
			get
			{
				return (CompProperties_AbilityReassure)this.props;
			}
		}

		// Token: 0x17000DC2 RID: 3522
		// (get) Token: 0x06004F8E RID: 20366 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool HideTargetPawnTooltip
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06004F8F RID: 20367 RVA: 0x001AA5B8 File Offset: 0x001A87B8
		public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
		{
			Pawn pawn = target.Pawn;
			return pawn != null && AbilityUtility.ValidateMustBeHuman(pawn, throwMessages) && AbilityUtility.ValidateNoMentalState(pawn, throwMessages) && AbilityUtility.ValidateIsAwake(pawn, throwMessages) && AbilityUtility.ValidateSameIdeo(this.parent.pawn, pawn, throwMessages);
		}

		// Token: 0x06004F90 RID: 20368 RVA: 0x001AA60A File Offset: 0x001A880A
		private float CertaintyGain(Pawn initiator, Pawn recipient)
		{
			return this.Props.baseCertaintyGain * initiator.GetStatValue(StatDefOf.NegotiationAbility, true);
		}

		// Token: 0x06004F91 RID: 20369 RVA: 0x001AA624 File Offset: 0x001A8824
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			if (!ModLister.CheckIdeology("Ideoligion certainty"))
			{
				return;
			}
			Pawn pawn = this.parent.pawn;
			Pawn pawn2 = target.Pawn;
			float certaintyGain = this.CertaintyGain(pawn, pawn2);
			float certainty = pawn2.ideo.Certainty;
			pawn2.ideo.Reassure(certaintyGain);
			Messages.Message(this.Props.successMessage.Formatted(pawn.Named("INITIATOR"), pawn2.Named("RECIPIENT"), certainty.ToStringPercent().Named("BEFORECERTAINTY"), pawn2.ideo.Certainty.ToStringPercent().Named("AFTERCERTAINTY"), pawn.Ideo.name.Named("IDEO")), new LookTargets(new Pawn[]
			{
				pawn,
				pawn2
			}), MessageTypeDefOf.PositiveEvent, true);
			PlayLogEntry_Interaction entry = new PlayLogEntry_Interaction(InteractionDefOf.Reassure, this.parent.pawn, pawn2, null);
			Find.PlayLog.Add(entry);
			if (this.Props.sound != null)
			{
				this.Props.sound.PlayOneShot(new TargetInfo(target.Cell, this.parent.pawn.Map, false));
			}
		}
	}
}
