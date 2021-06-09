using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020015AD RID: 5549
	public class PredatorThreat : IExposable
	{
		// Token: 0x170012AA RID: 4778
		// (get) Token: 0x0600787A RID: 30842 RVA: 0x00051241 File Offset: 0x0004F441
		public bool Expired
		{
			get
			{
				return !this.predator.Spawned || Find.TickManager.TicksGame >= this.lastAttackTicks + 600;
			}
		}

		// Token: 0x0600787B RID: 30843 RVA: 0x0005126D File Offset: 0x0004F46D
		public void ExposeData()
		{
			Scribe_References.Look<Pawn>(ref this.predator, "predator", false);
			Scribe_Values.Look<int>(ref this.lastAttackTicks, "lastAttackTicks", 0, false);
		}

		// Token: 0x04004F62 RID: 20322
		public Pawn predator;

		// Token: 0x04004F63 RID: 20323
		public int lastAttackTicks;

		// Token: 0x04004F64 RID: 20324
		private const int ExpireAfterTicks = 600;
	}
}
