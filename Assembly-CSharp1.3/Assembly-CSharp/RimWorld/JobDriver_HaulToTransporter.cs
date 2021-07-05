using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200071A RID: 1818
	public class JobDriver_HaulToTransporter : JobDriver_HaulToContainer
	{
		// Token: 0x17000967 RID: 2407
		// (get) Token: 0x06003281 RID: 12929 RVA: 0x00122D15 File Offset: 0x00120F15
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

		// Token: 0x06003282 RID: 12930 RVA: 0x00122D2C File Offset: 0x00120F2C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.initialCount, "initialCount", 0, false);
		}

		// Token: 0x06003283 RID: 12931 RVA: 0x00122D48 File Offset: 0x00120F48
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			this.pawn.ReserveAsManyAsPossible(this.job.GetTargetQueue(TargetIndex.A), this.job, 1, -1, null);
			this.pawn.ReserveAsManyAsPossible(this.job.GetTargetQueue(TargetIndex.B), this.job, 1, -1, null);
			return true;
		}

		// Token: 0x06003284 RID: 12932 RVA: 0x00122D98 File Offset: 0x00120F98
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

		// Token: 0x04001DC7 RID: 7623
		public int initialCount;
	}
}
