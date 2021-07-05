using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001263 RID: 4707
	public class Alert_Heatstroke : Alert
	{
		// Token: 0x060070C0 RID: 28864 RVA: 0x00259185 File Offset: 0x00257385
		public Alert_Heatstroke()
		{
			this.defaultLabel = "AlertHeatstroke".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x170013A8 RID: 5032
		// (get) Token: 0x060070C1 RID: 28865 RVA: 0x002591B4 File Offset: 0x002573B4
		private List<Pawn> HeatstrokePawns
		{
			get
			{
				this.heatstrokePawnsResult.Clear();
				List<Pawn> list = PawnsFinder.AllMaps_SpawnedPawnsInFaction(Faction.OfPlayer);
				for (int i = 0; i < list.Count; i++)
				{
					Pawn pawn = list[i];
					if (pawn.health != null && !pawn.RaceProps.Animal && pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Heatstroke, true) != null)
					{
						this.heatstrokePawnsResult.Add(pawn);
					}
				}
				return this.heatstrokePawnsResult;
			}
		}

		// Token: 0x060070C2 RID: 28866 RVA: 0x00259230 File Offset: 0x00257430
		public override TaggedString GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn pawn in this.HeatstrokePawns)
			{
				stringBuilder.AppendLine("  - " + pawn.NameShortColored.Resolve());
			}
			return string.Format("AlertHeatstrokeDesc".Translate(), stringBuilder.ToString());
		}

		// Token: 0x060070C3 RID: 28867 RVA: 0x002592C0 File Offset: 0x002574C0
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.HeatstrokePawns);
		}

		// Token: 0x04003E23 RID: 15907
		private List<Pawn> heatstrokePawnsResult = new List<Pawn>();
	}
}
