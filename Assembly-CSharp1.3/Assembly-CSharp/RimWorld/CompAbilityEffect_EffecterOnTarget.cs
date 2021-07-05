using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D30 RID: 3376
	public class CompAbilityEffect_EffecterOnTarget : CompAbilityEffect
	{
		// Token: 0x17000DAF RID: 3503
		// (get) Token: 0x06004F17 RID: 20247 RVA: 0x001A82E0 File Offset: 0x001A64E0
		public new CompProperties_AbilityEffecterOnTarget Props
		{
			get
			{
				return (CompProperties_AbilityEffecterOnTarget)this.props;
			}
		}

		// Token: 0x06004F18 RID: 20248 RVA: 0x001A82F0 File Offset: 0x001A64F0
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
				this.parent.AddEffecterToMaintain(effecter, target.Cell, this.Props.maintainForTicks, null);
				return;
			}
			effecter.Cleanup();
		}
	}
}
