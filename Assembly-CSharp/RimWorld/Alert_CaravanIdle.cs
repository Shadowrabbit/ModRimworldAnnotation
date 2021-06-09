using System;
using System.Collections.Generic;
using System.Text;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001956 RID: 6486
	public class Alert_CaravanIdle : Alert
	{
		// Token: 0x170016B0 RID: 5808
		// (get) Token: 0x06008FA0 RID: 36768 RVA: 0x00295A0C File Offset: 0x00293C0C
		private List<Caravan> IdleCaravans
		{
			get
			{
				this.idleCaravansResult.Clear();
				foreach (Caravan caravan in Find.WorldObjects.Caravans)
				{
					if (caravan.Spawned && caravan.IsPlayerControlled && !caravan.pather.MovingNow && !caravan.CantMove)
					{
						this.idleCaravansResult.Add(caravan);
					}
				}
				return this.idleCaravansResult;
			}
		}

		// Token: 0x06008FA1 RID: 36769 RVA: 0x000603CB File Offset: 0x0005E5CB
		public override string GetLabel()
		{
			return "CaravanIdle".Translate();
		}

		// Token: 0x06008FA2 RID: 36770 RVA: 0x00295AA0 File Offset: 0x00293CA0
		public override TaggedString GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Caravan caravan in this.IdleCaravans)
			{
				stringBuilder.AppendLine("  - " + caravan.Label);
			}
			return "CaravanIdleDesc".Translate(stringBuilder.ToString());
		}

		// Token: 0x06008FA3 RID: 36771 RVA: 0x000603DC File Offset: 0x0005E5DC
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.IdleCaravans);
		}

		// Token: 0x04005B75 RID: 23413
		private List<Caravan> idleCaravansResult = new List<Caravan>();
	}
}
