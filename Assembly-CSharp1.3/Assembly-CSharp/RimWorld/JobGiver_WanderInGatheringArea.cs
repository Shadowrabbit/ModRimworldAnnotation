using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007A8 RID: 1960
	public class JobGiver_WanderInGatheringArea : JobGiver_Wander
	{
		// Token: 0x06003558 RID: 13656 RVA: 0x0012DBFC File Offset: 0x0012BDFC
		protected override IntVec3 GetExactWanderDest(Pawn pawn)
		{
			Predicate<IntVec3> cellValidator = (IntVec3 x) => this.allowUnroofed || !x.Roofed(pawn.Map);
			IntVec3 result;
			if (this.desiredRadius > 0f && GatheringsUtility.TryFindRandomCellInGatheringAreaWithRadius(pawn, this.desiredRadius, cellValidator, out result))
			{
				return result;
			}
			if (GatheringsUtility.TryFindRandomCellInGatheringArea(pawn, cellValidator, out result))
			{
				return result;
			}
			return IntVec3.Invalid;
		}

		// Token: 0x06003559 RID: 13657 RVA: 0x0002974C File Offset: 0x0002794C
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600355A RID: 13658 RVA: 0x0012DC66 File Offset: 0x0012BE66
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_WanderInGatheringArea jobGiver_WanderInGatheringArea = (JobGiver_WanderInGatheringArea)base.DeepCopy(resolve);
			jobGiver_WanderInGatheringArea.allowUnroofed = this.allowUnroofed;
			jobGiver_WanderInGatheringArea.desiredRadius = this.desiredRadius;
			return jobGiver_WanderInGatheringArea;
		}

		// Token: 0x04001E8C RID: 7820
		public bool allowUnroofed = true;

		// Token: 0x04001E8D RID: 7821
		public float desiredRadius = -1f;
	}
}
