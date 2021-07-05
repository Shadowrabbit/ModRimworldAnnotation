using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D2C RID: 3372
	public class CompAbilityEffect_DropMechCluster : CompAbilityEffect_WithDest
	{
		// Token: 0x17000DAD RID: 3501
		// (get) Token: 0x06004F0E RID: 20238 RVA: 0x001A8148 File Offset: 0x001A6348
		public new CompProperties_DropMechCluster Props
		{
			get
			{
				return (CompProperties_DropMechCluster)this.props;
			}
		}

		// Token: 0x06004F0F RID: 20239 RVA: 0x001A8158 File Offset: 0x001A6358
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			MechClusterSketch sketch = MechClusterGenerator.GenerateClusterSketch(this.Props.points, this.parent.pawn.Map, this.Props.startDormant, false);
			MechClusterUtility.SpawnCluster(target.Cell, this.parent.pawn.Map, sketch, true, false, null);
		}

		// Token: 0x06004F10 RID: 20240 RVA: 0x001A81BB File Offset: 0x001A63BB
		public override bool CanApplyOn(LocalTargetInfo target, LocalTargetInfo dest)
		{
			return ModsConfig.RoyaltyActive && Faction.OfMechanoids != null && base.CanApplyOn(target, dest);
		}
	}
}
