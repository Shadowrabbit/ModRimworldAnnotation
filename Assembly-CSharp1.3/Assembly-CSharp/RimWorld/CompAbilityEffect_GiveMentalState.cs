using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D40 RID: 3392
	public class CompAbilityEffect_GiveMentalState : CompAbilityEffect
	{
		// Token: 0x17000DB5 RID: 3509
		// (get) Token: 0x06004F4A RID: 20298 RVA: 0x001A949A File Offset: 0x001A769A
		public new CompProperties_AbilityGiveMentalState Props
		{
			get
			{
				return (CompProperties_AbilityGiveMentalState)this.props;
			}
		}

		// Token: 0x06004F4B RID: 20299 RVA: 0x001A94A8 File Offset: 0x001A76A8
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			Pawn pawn = this.Props.applyToSelf ? this.parent.pawn : (target.Thing as Pawn);
			if (pawn != null && !pawn.InMentalState)
			{
				CompAbilityEffect_GiveMentalState.TryGiveMentalStateWithDuration(pawn.RaceProps.IsMechanoid ? (this.Props.stateDefForMechs ?? this.Props.stateDef) : this.Props.stateDef, pawn, this.parent.def, this.Props.durationMultiplier, this.parent.pawn);
				RestUtility.WakeUp(pawn);
			}
		}

		// Token: 0x06004F4C RID: 20300 RVA: 0x001A9550 File Offset: 0x001A7750
		public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
		{
			Pawn pawn = target.Pawn;
			return pawn == null || AbilityUtility.ValidateNoMentalState(pawn, throwMessages);
		}

		// Token: 0x06004F4D RID: 20301 RVA: 0x001A9574 File Offset: 0x001A7774
		public static void TryGiveMentalStateWithDuration(MentalStateDef def, Pawn p, AbilityDef ability, StatDef multiplierStat, Pawn caster)
		{
			if (p.mindState.mentalStateHandler.TryStartMentalState(def, null, true, false, null, false, false, ability.IsPsycast))
			{
				float num = ability.GetStatValueAbstract(StatDefOf.Ability_Duration, caster);
				if (multiplierStat != null)
				{
					num *= p.GetStatValue(multiplierStat, true);
				}
				p.mindState.mentalStateHandler.CurState.forceRecoverAfterTicks = num.SecondsToTicks();
			}
		}
	}
}
