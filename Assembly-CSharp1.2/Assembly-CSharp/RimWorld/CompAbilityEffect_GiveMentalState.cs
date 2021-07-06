using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200137F RID: 4991
	public class CompAbilityEffect_GiveMentalState : CompAbilityEffect
	{
		// Token: 0x170010BF RID: 4287
		// (get) Token: 0x06006C72 RID: 27762 RVA: 0x00049C0D File Offset: 0x00047E0D
		public new CompProperties_AbilityGiveMentalState Props
		{
			get
			{
				return (CompProperties_AbilityGiveMentalState)this.props;
			}
		}

		// Token: 0x06006C73 RID: 27763 RVA: 0x002152A4 File Offset: 0x002134A4
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			Pawn pawn = target.Thing as Pawn;
			if (pawn != null && !pawn.InMentalState)
			{
				CompAbilityEffect_GiveMentalState.TryGiveMentalStateWithDuration(pawn.RaceProps.IsMechanoid ? (this.Props.stateDefForMechs ?? this.Props.stateDef) : this.Props.stateDef, pawn, this.parent.def, this.Props.durationMultiplier);
				RestUtility.WakeUp(pawn);
			}
		}

		// Token: 0x06006C74 RID: 27764 RVA: 0x00215328 File Offset: 0x00213528
		public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
		{
			Pawn pawn = target.Pawn;
			return pawn == null || AbilityUtility.ValidateNoMentalState(pawn, throwMessages);
		}

		// Token: 0x06006C75 RID: 27765 RVA: 0x0021534C File Offset: 0x0021354C
		public static void TryGiveMentalStateWithDuration(MentalStateDef def, Pawn p, AbilityDef ability, StatDef multiplierStat)
		{
			if (p.mindState.mentalStateHandler.TryStartMentalState(def, null, true, false, null, false))
			{
				float num = ability.statBases.GetStatValueFromList(StatDefOf.Ability_Duration, 10f);
				if (multiplierStat != null)
				{
					num *= p.GetStatValue(multiplierStat, true);
				}
				p.mindState.mentalStateHandler.CurState.forceRecoverAfterTicks = num.SecondsToTicks();
			}
		}
	}
}
