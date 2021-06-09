using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018D9 RID: 6361
	public class CompUseEffect_GainAbility : CompUseEffect
	{
		// Token: 0x17001625 RID: 5669
		// (get) Token: 0x06008CF0 RID: 36080 RVA: 0x0005E7D3 File Offset: 0x0005C9D3
		private AbilityDef Ability
		{
			get
			{
				return this.parent.GetComp<CompNeurotrainer>().ability;
			}
		}

		// Token: 0x06008CF1 RID: 36081 RVA: 0x0028E330 File Offset: 0x0028C530
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

		// Token: 0x06008CF2 RID: 36082 RVA: 0x0028E398 File Offset: 0x0028C598
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

		// Token: 0x06008CF3 RID: 36083 RVA: 0x0028E438 File Offset: 0x0028C638
		public override TaggedString ConfirmMessage(Pawn p)
		{
			Hediff firstHediffOfDef = p.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.PsychicAmplifier, false);
			if (firstHediffOfDef == null)
			{
				return null;
			}
			if (this.Ability.level > ((Hediff_ImplantWithLevel)firstHediffOfDef).level)
			{
				return "PsylinkTooLowForGainAbility".Translate(p.Named("PAWN"), this.Ability.label.Named("ABILITY"));
			}
			return null;
		}
	}
}
