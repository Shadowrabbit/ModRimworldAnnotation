using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001251 RID: 4689
	public class StatsRecord : IExposable
	{
		// Token: 0x06006652 RID: 26194 RVA: 0x001F9348 File Offset: 0x001F7548
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.numRaidsEnemy, "numRaidsEnemy", 0, false);
			Scribe_Values.Look<int>(ref this.numThreatBigs, "numThreatsQueued", 0, false);
			Scribe_Values.Look<int>(ref this.colonistsKilled, "colonistsKilled", 0, false);
			Scribe_Values.Look<int>(ref this.colonistsLaunched, "colonistsLaunched", 0, false);
			Scribe_Values.Look<int>(ref this.greatestPopulation, "greatestPopulation", 3, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.UpdateGreatestPopulation();
			}
		}

		// Token: 0x06006653 RID: 26195 RVA: 0x00045E71 File Offset: 0x00044071
		public void Notify_ColonistKilled()
		{
			this.colonistsKilled++;
		}

		// Token: 0x06006654 RID: 26196 RVA: 0x001F93C0 File Offset: 0x001F75C0
		public void UpdateGreatestPopulation()
		{
			int a = PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_Colonists.Count<Pawn>();
			this.greatestPopulation = Mathf.Max(a, this.greatestPopulation);
		}

		// Token: 0x04004425 RID: 17445
		public int numRaidsEnemy;

		// Token: 0x04004426 RID: 17446
		public int numThreatBigs;

		// Token: 0x04004427 RID: 17447
		public int colonistsKilled;

		// Token: 0x04004428 RID: 17448
		public int colonistsLaunched;

		// Token: 0x04004429 RID: 17449
		public int greatestPopulation;
	}
}
