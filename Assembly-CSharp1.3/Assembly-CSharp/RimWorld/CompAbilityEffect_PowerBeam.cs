using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D4F RID: 3407
	public class CompAbilityEffect_PowerBeam : CompAbilityEffect_WithDest
	{
		// Token: 0x17000DBC RID: 3516
		// (get) Token: 0x06004F77 RID: 20343 RVA: 0x001AA2A8 File Offset: 0x001A84A8
		public new CompProperties_PowerBeam Props
		{
			get
			{
				return (CompProperties_PowerBeam)this.props;
			}
		}

		// Token: 0x06004F78 RID: 20344 RVA: 0x001AA2B8 File Offset: 0x001A84B8
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			PowerBeam powerBeam = (PowerBeam)GenSpawn.Spawn(ThingDefOf.PowerBeam, target.Cell, this.parent.pawn.Map, WipeMode.Vanish);
			powerBeam.duration = this.Props.durationTicks;
			powerBeam.instigator = this.parent.pawn;
			powerBeam.StartStrike();
		}
	}
}
