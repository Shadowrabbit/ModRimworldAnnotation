using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C2E RID: 3118
	public class StatsRecord : IExposable
	{
		// Token: 0x06004938 RID: 18744 RVA: 0x00183D80 File Offset: 0x00181F80
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

		// Token: 0x06004939 RID: 18745 RVA: 0x00183DF5 File Offset: 0x00181FF5
		public void Notify_ColonistKilled()
		{
			this.colonistsKilled++;
		}

		// Token: 0x0600493A RID: 18746 RVA: 0x00183E08 File Offset: 0x00182008
		public void UpdateGreatestPopulation()
		{
			int a = PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_Colonists.Count<Pawn>();
			this.greatestPopulation = Mathf.Max(a, this.greatestPopulation);
		}

		// Token: 0x04002C86 RID: 11398
		public int numRaidsEnemy;

		// Token: 0x04002C87 RID: 11399
		public int numThreatBigs;

		// Token: 0x04002C88 RID: 11400
		public int colonistsKilled;

		// Token: 0x04002C89 RID: 11401
		public int colonistsLaunched;

		// Token: 0x04002C8A RID: 11402
		public int greatestPopulation;
	}
}
