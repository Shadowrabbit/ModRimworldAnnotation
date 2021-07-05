using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001244 RID: 4676
	public class Alert_ImmobileCaravan : Alert_Critical
	{
		// Token: 0x17001398 RID: 5016
		// (get) Token: 0x0600703C RID: 28732 RVA: 0x002561D0 File Offset: 0x002543D0
		private List<Caravan> ImmobileCaravans
		{
			get
			{
				this.immobileCaravansResult.Clear();
				List<Caravan> caravans = Find.WorldObjects.Caravans;
				for (int i = 0; i < caravans.Count; i++)
				{
					if (caravans[i].IsPlayerControlled && caravans[i].ImmobilizedByMass)
					{
						this.immobileCaravansResult.Add(caravans[i]);
					}
				}
				return this.immobileCaravansResult;
			}
		}

		// Token: 0x0600703D RID: 28733 RVA: 0x00256238 File Offset: 0x00254438
		public Alert_ImmobileCaravan()
		{
			this.defaultLabel = "ImmobileCaravan".Translate();
			this.defaultExplanation = "ImmobileCaravanDesc".Translate();
		}

		// Token: 0x0600703E RID: 28734 RVA: 0x00256275 File Offset: 0x00254475
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.ImmobileCaravans);
		}

		// Token: 0x04003DF7 RID: 15863
		private List<Caravan> immobileCaravansResult = new List<Caravan>();
	}
}
