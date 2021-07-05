using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200120A RID: 4618
	public class CompUseEffect_GainAbility : CompUseEffect
	{
		// Token: 0x17001346 RID: 4934
		// (get) Token: 0x06006EEF RID: 28399 RVA: 0x00251609 File Offset: 0x0024F809
		private AbilityDef Ability
		{
			get
			{
				return this.parent.GetComp<CompNeurotrainer>().ability;
			}
		}

		// Token: 0x06006EF0 RID: 28400 RVA: 0x0025161C File Offset: 0x0024F81C
		public override void DoEffect(Pawn user)
		{
			base.DoEffect(user);
			AbilityDef ability = this.Ability;
			user.abilities.GainAbility(ability);
			if (PawnUtility.ShouldSendNotificationAbout(user))
			{
				Messages.Message("AbilityNeurotrainerUsed".Translate(user.Named("USER"), ability.LabelCap), user, MessageTypeDefOf.PositiveEvent, true);
			}
		}

		// Token: 0x06006EF1 RID: 28401 RVA: 0x00251684 File Offset: 0x0024F884
		public override bool CanBeUsedBy(Pawn p, out string failReason)
		{
			if (!p.health.hediffSet.HasHediff(HediffDefOf.PsychicAmplifier, false))
			{
				failReason = "PsycastNeurotrainerNoPsylink".TranslateWithBackup("PsycastNeurotrainerNoPsychicAmplifier");
				return false;
			}
			if (p.abilities != null && p.abilities.abilities.Any((Ability a) => a.def == this.Ability))
			{
				failReason = "PsycastNeurotrainerAbilityAlreadyLearned".Translate(p.Named("USER"), this.Ability.LabelCap);
				return false;
			}
			return base.CanBeUsedBy(p, out failReason);
		}

		// Token: 0x06006EF2 RID: 28402 RVA: 0x00251724 File Offset: 0x0024F924
		public override TaggedString ConfirmMessage(Pawn p)
		{
			Hediff firstHediffOfDef = p.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.PsychicAmplifier, false);
			if (firstHediffOfDef == null)
			{
				return null;
			}
			if (this.Ability.level > ((Hediff_Level)firstHediffOfDef).level)
			{
				return "PsylinkTooLowForGainAbility".Translate(p.Named("PAWN"), this.Ability.label.Named("ABILITY"));
			}
			return null;
		}
	}
}
