using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001262 RID: 4706
	public class Alert_Exhaustion : Alert
	{
		// Token: 0x060070BC RID: 28860 RVA: 0x00259031 File Offset: 0x00257231
		public Alert_Exhaustion()
		{
			this.defaultLabel = "Exhaustion".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x170013A7 RID: 5031
		// (get) Token: 0x060070BD RID: 28861 RVA: 0x00259060 File Offset: 0x00257260
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

		// Token: 0x060070BE RID: 28862 RVA: 0x002590F0 File Offset: 0x002572F0
		public override TaggedString GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn pawn in this.ExhaustedColonists)
			{
				stringBuilder.AppendLine("  - " + pawn.NameShortColored.Resolve());
			}
			return "ExhaustionDesc".Translate(stringBuilder.ToString());
		}

		// Token: 0x060070BF RID: 28863 RVA: 0x00259178 File Offset: 0x00257378
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.ExhaustedColonists);
		}

		// Token: 0x04003E22 RID: 15906
		private List<Pawn> exhaustedColonistsResult = new List<Pawn>();
	}
}
