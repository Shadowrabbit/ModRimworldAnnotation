using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D6D RID: 3437
	public class CompAbilityEffect_StartTrial : CompAbilityEffect_StartRitualOnPawn
	{
		// Token: 0x17000DCD RID: 3533
		// (get) Token: 0x06004FC1 RID: 20417 RVA: 0x001AAFEE File Offset: 0x001A91EE
		public new CompProperties_AbilityStartTrial Props
		{
			get
			{
				return (CompProperties_AbilityStartTrial)this.props;
			}
		}

		// Token: 0x06004FC2 RID: 20418 RVA: 0x001AAFFC File Offset: 0x001A91FC
		protected override Precept_Ritual RitualForTarget(Pawn pawn)
		{
			PreceptDef preceptDef = pawn.InMentalState ? this.Props.ritualDefForMentalState : (pawn.IsPrisonerOfColony ? this.Props.ritualDefForPrisoner : null);
			if (preceptDef != null)
			{
				for (int i = 0; i < this.parent.pawn.Ideo.PreceptsListForReading.Count; i++)
				{
					if (this.parent.pawn.Ideo.PreceptsListForReading[i].def == preceptDef)
					{
						return (Precept_Ritual)this.parent.pawn.Ideo.PreceptsListForReading[i];
					}
				}
			}
			return base.RitualForTarget(pawn);
		}

		// Token: 0x06004FC3 RID: 20419 RVA: 0x001AB0A8 File Offset: 0x001A92A8
		public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
		{
			Pawn pawn = target.Pawn;
			return pawn != null && AbilityUtility.ValidateCanWalk(pawn, throwMessages) && AbilityUtility.ValidateNotGuilty(pawn, throwMessages) && base.Valid(target, throwMessages);
		}
	}
}
