using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001950 RID: 6480
	public class Alert_StarvationColonists : Alert
	{
		// Token: 0x06008F87 RID: 36743 RVA: 0x00060280 File Offset: 0x0005E480
		public Alert_StarvationColonists()
		{
			this.defaultLabel = "Starvation".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x170016AA RID: 5802
		// (get) Token: 0x06008F88 RID: 36744 RVA: 0x00295284 File Offset: 0x00293484
		private List<Pawn> StarvingColonists
		{
			get
			{
				this.starvingColonistsResult.Clear();
				foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonists_NoCryptosleep)
				{
					if (pawn.needs.food != null && pawn.needs.food.Starving)
					{
						this.starvingColonistsResult.Add(pawn);
					}
				}
				return this.starvingColonistsResult;
			}
		}

		// Token: 0x06008F89 RID: 36745 RVA: 0x0029530C File Offset: 0x0029350C
		public override TaggedString GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn pawn in this.StarvingColonists)
			{
				stringBuilder.AppendLine("  - " + pawn.NameShortColored.Resolve());
			}
			return "StarvationDesc".Translate(stringBuilder.ToString());
		}

		// Token: 0x06008F8A RID: 36746 RVA: 0x000602AF File Offset: 0x0005E4AF
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.StarvingColonists);
		}

		// Token: 0x04005B6E RID: 23406
		private List<Pawn> starvingColonistsResult = new List<Pawn>();
	}
}
