using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D5B RID: 3419
	public class CompAbilityEffect_RemoveHediff : CompAbilityEffect
	{
		// Token: 0x17000DC3 RID: 3523
		// (get) Token: 0x06004F94 RID: 20372 RVA: 0x001AA777 File Offset: 0x001A8977
		public new CompProperties_AbilityRemoveHediff Props
		{
			get
			{
				return (CompProperties_AbilityRemoveHediff)this.props;
			}
		}

		// Token: 0x06004F95 RID: 20373 RVA: 0x001AA784 File Offset: 0x001A8984
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			if (this.Props.applyToSelf)
			{
				this.RemoveHediff(this.parent.pawn);
			}
			if (target.Pawn != null && this.Props.applyToTarget && target.Pawn != this.parent.pawn)
			{
				this.RemoveHediff(target.Pawn);
			}
		}

		// Token: 0x06004F96 RID: 20374 RVA: 0x001AA7F0 File Offset: 0x001A89F0
		private void RemoveHediff(Pawn pawn)
		{
			Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(this.Props.hediffDef, false);
			if (firstHediffOfDef != null)
			{
				pawn.health.RemoveHediff(firstHediffOfDef);
			}
		}
	}
}
