using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200193C RID: 6460
	public class Alert_ImmobileCaravan : Alert_Critical
	{
		// Token: 0x170016A0 RID: 5792
		// (get) Token: 0x06008F32 RID: 36658 RVA: 0x00293C04 File Offset: 0x00291E04
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

		// Token: 0x06008F33 RID: 36659 RVA: 0x0005FDAF File Offset: 0x0005DFAF
		public Alert_ImmobileCaravan()
		{
			this.defaultLabel = "ImmobileCaravan".Translate();
			this.defaultExplanation = "ImmobileCaravanDesc".Translate();
		}

		// Token: 0x06008F34 RID: 36660 RVA: 0x0005FDEC File Offset: 0x0005DFEC
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.ImmobileCaravans);
		}

		// Token: 0x04005B54 RID: 23380
		private List<Caravan> immobileCaravansResult = new List<Caravan>();
	}
}
