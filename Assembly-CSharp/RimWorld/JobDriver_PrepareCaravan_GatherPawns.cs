using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B48 RID: 2888
	[Obsolete]
	public class JobDriver_PrepareCaravan_GatherPawns : JobDriver
	{
		// Token: 0x17000A93 RID: 2707
		// (get) Token: 0x060043EB RID: 17387 RVA: 0x0016B214 File Offset: 0x00169414
		private Pawn AnimalOrSlave
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x060043EC RID: 17388 RVA: 0x0003248B File Offset: 0x0003068B
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.AnimalOrSlave, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x060043ED RID: 17389 RVA: 0x000324AD File Offset: 0x000306AD
		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield break;
		}

		// Token: 0x060043EE RID: 17390 RVA: 0x0000C32E File Offset: 0x0000A52E
		[Obsolete]
		private Toil SetFollowerToil()
		{
			return null;
		}

		// Token: 0x04002E3F RID: 11839
		private const TargetIndex AnimalOrSlaveInd = TargetIndex.A;
	}
}
