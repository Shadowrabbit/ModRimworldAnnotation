using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200137B RID: 4987
	public class CompAbilityEffect_GiveHediff : CompAbilityEffect_WithDuration
	{
		// Token: 0x170010BE RID: 4286
		// (get) Token: 0x06006C68 RID: 27752 RVA: 0x00049BCE File Offset: 0x00047DCE
		public new CompProperties_AbilityGiveHediff Props
		{
			get
			{
				return (CompProperties_AbilityGiveHediff)this.props;
			}
		}

		// Token: 0x06006C69 RID: 27753 RVA: 0x002150B0 File Offset: 0x002132B0
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			if (!this.Props.onlyApplyToSelf)
			{
				this.ApplyInner(target.Pawn, this.parent.pawn);
			}
			if (this.Props.applyToSelf || this.Props.onlyApplyToSelf)
			{
				this.ApplyInner(this.parent.pawn, target.Pawn);
			}
		}

		// Token: 0x06006C6A RID: 27754 RVA: 0x0021511C File Offset: 0x0021331C
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
