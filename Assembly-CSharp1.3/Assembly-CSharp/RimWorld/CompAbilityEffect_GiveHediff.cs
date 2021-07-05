using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D3C RID: 3388
	public class CompAbilityEffect_GiveHediff : CompAbilityEffect_WithDuration
	{
		// Token: 0x17000DB4 RID: 3508
		// (get) Token: 0x06004F40 RID: 20288 RVA: 0x001A925A File Offset: 0x001A745A
		public new CompProperties_AbilityGiveHediff Props
		{
			get
			{
				return (CompProperties_AbilityGiveHediff)this.props;
			}
		}

		// Token: 0x06004F41 RID: 20289 RVA: 0x001A9268 File Offset: 0x001A7468
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			if (!this.Props.onlyApplyToSelf && this.Props.applyToTarget)
			{
				this.ApplyInner(target.Pawn, this.parent.pawn);
			}
			if (this.Props.applyToSelf || this.Props.onlyApplyToSelf)
			{
				this.ApplyInner(this.parent.pawn, target.Pawn);
			}
		}

		// Token: 0x06004F42 RID: 20290 RVA: 0x001A92E4 File Offset: 0x001A74E4
		protected void ApplyInner(Pawn target, Pawn other)
		{
			if (target != null)
			{
				if (this.Props.replaceExisting)
				{
					Hediff firstHediffOfDef = target.health.hediffSet.GetFirstHediffOfDef(this.Props.hediffDef, false);
					if (firstHediffOfDef != null)
					{
						target.health.RemoveHediff(firstHediffOfDef);
					}
				}
				Hediff hediff = HediffMaker.MakeHediff(this.Props.hediffDef, target, this.Props.onlyBrain ? target.health.hediffSet.GetBrain() : null);
				HediffComp_Disappears hediffComp_Disappears = hediff.TryGetComp<HediffComp_Disappears>();
				if (hediffComp_Disappears != null)
				{
					hediffComp_Disappears.ticksToDisappear = base.GetDurationSeconds(target).SecondsToTicks();
				}
				HediffComp_Link hediffComp_Link = hediff.TryGetComp<HediffComp_Link>();
				if (hediffComp_Link != null)
				{
					hediffComp_Link.other = other;
					hediffComp_Link.drawConnection = (target == this.parent.pawn);
				}
				target.health.AddHediff(hediff, null, null, null);
			}
		}
	}
}
