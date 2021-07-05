using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200136F RID: 4975
	public class CompAbilityEffect_EffecterOnTarget : CompAbilityEffect
	{
		// Token: 0x170010B7 RID: 4279
		// (get) Token: 0x06006C35 RID: 27701 RVA: 0x00049A00 File Offset: 0x00047C00
		public new CompProperties_AbilityEffecterOnTarget Props
		{
			get
			{
				return (CompProperties_AbilityEffecterOnTarget)this.props;
			}
		}

		// Token: 0x06006C36 RID: 27702 RVA: 0x0021419C File Offset: 0x0021239C
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			Effecter effecter;
			if (target.HasThing)
			{
				effecter = this.Props.effecterDef.Spawn(target.Thing, this.parent.pawn.Map, this.Props.scale);
			}
			else
			{
				effecter = this.Props.effecterDef.Spawn(target.Cell, this.parent.pawn.Map, this.Props.scale);
			}
			if (this.Props.maintainForTicks > 0)
			{
				this.parent.AddEffecterToMaintain(effecter, target.Cell, this.Props.maintainForTicks);
				return;
			}
			effecter.Cleanup();
		}
	}
}
