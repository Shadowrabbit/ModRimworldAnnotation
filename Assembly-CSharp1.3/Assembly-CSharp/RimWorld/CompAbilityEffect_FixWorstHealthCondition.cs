using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D34 RID: 3380
	public class CompAbilityEffect_FixWorstHealthCondition : CompAbilityEffect
	{
		// Token: 0x17000DB1 RID: 3505
		// (get) Token: 0x06004F2A RID: 20266 RVA: 0x001A8BB8 File Offset: 0x001A6DB8
		public new CompProperties_AbilityFixWorstHealthCondition Props
		{
			get
			{
				return (CompProperties_AbilityFixWorstHealthCondition)this.props;
			}
		}

		// Token: 0x06004F2B RID: 20267 RVA: 0x001A8BC8 File Offset: 0x001A6DC8
		public override bool CanApplyOn(LocalTargetInfo target, LocalTargetInfo dest)
		{
			if (target.Pawn == null)
			{
				return false;
			}
			return target.Pawn.health.hediffSet.hediffs.Any((Hediff x) => x.def.isBad && x.def.everCurableByItem && x.Visible) && base.CanApplyOn(target, dest);
		}

		// Token: 0x06004F2C RID: 20268 RVA: 0x001A8C28 File Offset: 0x001A6E28
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			TaggedString value = HealthUtility.FixWorstHealthCondition(target.Pawn);
			if (base.SendLetter)
			{
				Find.LetterStack.ReceiveLetter(this.Props.customLetterLabel, this.Props.customLetterText.Formatted(this.parent.pawn, target.Pawn, value), LetterDefOf.PositiveEvent, new LookTargets(target.Pawn), null, null, null, null);
			}
		}
	}
}
