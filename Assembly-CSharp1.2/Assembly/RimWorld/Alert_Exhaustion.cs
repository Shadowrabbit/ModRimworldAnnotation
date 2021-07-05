using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001951 RID: 6481
	public class Alert_Exhaustion : Alert
	{
		// Token: 0x06008F8B RID: 36747 RVA: 0x000602BC File Offset: 0x0005E4BC
		public Alert_Exhaustion()
		{
			this.defaultLabel = "Exhaustion".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x170016AB RID: 5803
		// (get) Token: 0x06008F8C RID: 36748 RVA: 0x00295394 File Offset: 0x00293594
		private List<Pawn> ExhaustedColonists
		{
			get
			{
				this.exhaustedColonistsResult.Clear();
				List<Pawn> allMaps_FreeColonists = PawnsFinder.AllMaps_FreeColonists;
				for (int i = 0; i < allMaps_FreeColonists.Count; i++)
				{
					if ((allMaps_FreeColonists[i].Spawned || allMaps_FreeColonists[i].BrieflyDespawned()) && allMaps_FreeColonists[i].needs.rest != null && allMaps_FreeColonists[i].needs.rest.CurCategory == RestCategory.Exhausted)
					{
						this.exhaustedColonistsResult.Add(allMaps_FreeColonists[i]);
					}
				}
				return this.exhaustedColonistsResult;
			}
		}

		// Token: 0x06008F8D RID: 36749 RVA: 0x00295424 File Offset: 0x00293624
		public override TaggedString GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn pawn in this.ExhaustedColonists)
			{
				stringBuilder.AppendLine("  - " + pawn.NameShortColored.Resolve());
			}
			return "ExhaustionDesc".Translate(stringBuilder.ToString());
		}

		// Token: 0x06008F8E RID: 36750 RVA: 0x000602EB File Offset: 0x0005E4EB
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.ExhaustedColonists);
		}

		// Token: 0x04005B6F RID: 23407
		private List<Pawn> exhaustedColonistsResult = new List<Pawn>();
	}
}
