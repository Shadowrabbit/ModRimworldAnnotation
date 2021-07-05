using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000979 RID: 2425
	public class ThoughtWorker_Precept_TreeDensity : ThoughtWorker_Precept
	{
		// Token: 0x06003D73 RID: 15731 RVA: 0x001522DA File Offset: 0x001504DA
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			return ModsConfig.IdeologyActive;
		}

		// Token: 0x06003D74 RID: 15732 RVA: 0x001522E8 File Offset: 0x001504E8
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!base.CurrentStateInternal(p).Active)
			{
				return ThoughtState.Inactive;
			}
			return ThoughtState.ActiveAtStage(this.ThoughtStageIndex(p));
		}

		// Token: 0x06003D75 RID: 15733 RVA: 0x00152318 File Offset: 0x00150518
		private int ThoughtStageIndex(Pawn p)
		{
			float densityDestroyed = p.Map.treeDestructionTracker.DensityDestroyed;
			if (densityDestroyed <= 0f)
			{
				return 0;
			}
			if (densityDestroyed <= 0.32f)
			{
				return 1;
			}
			if (densityDestroyed <= 1.6f)
			{
				return 2;
			}
			if (densityDestroyed <= 3.2f)
			{
				return 3;
			}
			return 0;
		}

		// Token: 0x040020DC RID: 8412
		private const float Density_Off = 0f;

		// Token: 0x040020DD RID: 8413
		private const float Density_Minor = 0.32f;

		// Token: 0x040020DE RID: 8414
		private const float Density_Major = 1.6f;

		// Token: 0x040020DF RID: 8415
		private const float Density_Extreme = 3.2f;
	}
}
