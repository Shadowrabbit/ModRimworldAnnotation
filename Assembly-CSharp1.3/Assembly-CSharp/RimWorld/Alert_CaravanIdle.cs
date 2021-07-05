using System;
using System.Collections.Generic;
using System.Text;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200126A RID: 4714
	public class Alert_CaravanIdle : Alert
	{
		// Token: 0x170013AF RID: 5039
		// (get) Token: 0x060070DF RID: 28895 RVA: 0x00259BE4 File Offset: 0x00257DE4
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

		// Token: 0x060070E0 RID: 28896 RVA: 0x00259C78 File Offset: 0x00257E78
		public override string GetLabel()
		{
			return "CaravanIdle".Translate();
		}

		// Token: 0x060070E1 RID: 28897 RVA: 0x00259C8C File Offset: 0x00257E8C
		public override TaggedString GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Caravan caravan in this.IdleCaravans)
			{
				stringBuilder.AppendLine("  - " + caravan.Label);
			}
			return "CaravanIdleDesc".Translate(stringBuilder.ToString());
		}

		// Token: 0x060070E2 RID: 28898 RVA: 0x00259D0C File Offset: 0x00257F0C
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.IdleCaravans);
		}

		// Token: 0x04003E2B RID: 15915
		private List<Caravan> idleCaravansResult = new List<Caravan>();
	}
}
