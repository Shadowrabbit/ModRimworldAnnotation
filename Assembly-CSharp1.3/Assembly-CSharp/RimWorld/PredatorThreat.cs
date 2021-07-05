using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000ECC RID: 3788
	public class PredatorThreat : IExposable
	{
		// Token: 0x17000F9F RID: 3999
		// (get) Token: 0x06005959 RID: 22873 RVA: 0x001E776E File Offset: 0x001E596E
		public bool Expired
		{
			get
			{
				return !this.predator.Spawned || Find.TickManager.TicksGame >= this.lastAttackTicks + 600;
			}
		}

		// Token: 0x0600595A RID: 22874 RVA: 0x001E779A File Offset: 0x001E599A
		public void ExposeData()
		{
			Scribe_References.Look<Pawn>(ref this.predator, "predator", false);
			Scribe_Values.Look<int>(ref this.lastAttackTicks, "lastAttackTicks", 0, false);
		}

		// Token: 0x04003469 RID: 13417
		public Pawn predator;

		// Token: 0x0400346A RID: 13418
		public int lastAttackTicks;

		// Token: 0x0400346B RID: 13419
		private const int ExpireAfterTicks = 600;
	}
}
