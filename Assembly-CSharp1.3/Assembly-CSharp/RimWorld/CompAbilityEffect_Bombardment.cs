using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D21 RID: 3361
	public class CompAbilityEffect_Bombardment : CompAbilityEffect_WithDest
	{
		// Token: 0x17000DA6 RID: 3494
		// (get) Token: 0x06004EE5 RID: 20197 RVA: 0x001A6CC3 File Offset: 0x001A4EC3
		public new CompProperties_Bombardment Props
		{
			get
			{
				return (CompProperties_Bombardment)this.props;
			}
		}

		// Token: 0x06004EE6 RID: 20198 RVA: 0x001A6CD0 File Offset: 0x001A4ED0
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			Bombardment bombardment = (Bombardment)GenSpawn.Spawn(ThingDefOf.Bombardment, target.Cell, this.parent.pawn.Map, WipeMode.Vanish);
			bombardment.duration = this.Props.durationTicks;
			bombardment.instigator = this.parent.pawn;
			bombardment.StartStrike();
		}
	}
}
