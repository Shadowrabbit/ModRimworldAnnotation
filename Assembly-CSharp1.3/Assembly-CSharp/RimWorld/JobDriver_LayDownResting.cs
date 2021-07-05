using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000722 RID: 1826
	public class JobDriver_LayDownResting : JobDriver_LayDownAwake
	{
		// Token: 0x17000975 RID: 2421
		// (get) Token: 0x060032B8 RID: 12984 RVA: 0x000FE241 File Offset: 0x000FC441
		public override Rot4 ForcedLayingRotation
		{
			get
			{
				return Rot4.Invalid;
			}
		}

		// Token: 0x060032B9 RID: 12985 RVA: 0x0012346F File Offset: 0x0012166F
		public override Toil LayDownToil(bool hasBed)
		{
			return Toils_LayDown.LayDown(TargetIndex.A, hasBed, this.LookForOtherJobs, this.CanSleep, this.CanRest, PawnPosture.LayingOnGroundFaceUp);
		}
	}
}
