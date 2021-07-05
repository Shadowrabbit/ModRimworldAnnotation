using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200151F RID: 5407
	public struct SkullspikeSighting : IExposable
	{
		// Token: 0x170015E3 RID: 5603
		// (get) Token: 0x060080A0 RID: 32928 RVA: 0x002D919A File Offset: 0x002D739A
		public int TicksSinceSighting
		{
			get
			{
				return Find.TickManager.TicksGame - this.tickSighted;
			}
		}

		// Token: 0x060080A1 RID: 32929 RVA: 0x002D91AD File Offset: 0x002D73AD
		public SkullspikeSighting(Thing skullspike, int tickSighted)
		{
			this.skullspike = skullspike;
			this.tickSighted = tickSighted;
		}

		// Token: 0x060080A2 RID: 32930 RVA: 0x002D91BD File Offset: 0x002D73BD
		public void ExposeData()
		{
			Scribe_References.Look<Thing>(ref this.skullspike, "skullspike", false);
			Scribe_Values.Look<int>(ref this.tickSighted, "tickSighted", 0, false);
		}

		// Token: 0x04005018 RID: 20504
		public Thing skullspike;

		// Token: 0x04005019 RID: 20505
		public int tickSighted;

		// Token: 0x0400501A RID: 20506
		public const int MaxSightingAge = 1800;

		// Token: 0x0400501B RID: 20507
		public const int CheckRadius = 10;

		// Token: 0x0400501C RID: 20508
		public const int CheckIntervalTicks = 60;
	}
}
