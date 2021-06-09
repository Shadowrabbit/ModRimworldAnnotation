using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000BCA RID: 3018
	public class JobDriver_HaulToTransporter : JobDriver_HaulToContainer
	{
		// Token: 0x17000B24 RID: 2852
		// (get) Token: 0x060046F8 RID: 18168 RVA: 0x00033C1B File Offset: 0x00031E1B
		public CompTransporter Transporter
		{
			get
			{
				if (base.Container == null)
				{
					return null;
				}
				return base.Container.TryGetComp<CompTransporter>();
			}
		}

		// Token: 0x060046F9 RID: 18169 RVA: 0x00033C32 File Offset: 0x00031E32
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.initialCount, "initialCount", 0, false);
		}

		// Token: 0x060046FA RID: 18170 RVA: 0x00196984 File Offset: 0x00194B84
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			this.pawn.ReserveAsManyAsPossible(this.job.GetTargetQueue(TargetIndex.A), this.job, 1, -1, null);
			this.pawn.ReserveAsManyAsPossible(this.job.GetTargetQueue(TargetIndex.B), this.job, 1, -1, null);
			return true;
		}

		// Token: 0x060046FB RID: 18171 RVA: 0x001969D4 File Offset: 0x00194BD4
		public override void Notify_Starting()
		{
			base.Notify_Starting();
			ThingCount thingCount;
			if (this.job.targetA.IsValid)
			{
				thingCount = new ThingCount(this.job.targetA.Thing, this.job.targetA.Thing.stackCount);
			}
			else
			{
				thingCount = LoadTransportersJobUtility.FindThingToLoad(this.pawn, base.Container.TryGetComp<CompTransporter>());
			}
			this.job.targetA = thingCount.Thing;
			this.job.count = thingCount.Count;
			this.initialCount = thingCount.Count;
			this.pawn.Reserve(thingCount.Thing, this.job, 1, -1, null, true);
		}

		// Token: 0x04002F9B RID: 12187
		public int initialCount;
	}
}
