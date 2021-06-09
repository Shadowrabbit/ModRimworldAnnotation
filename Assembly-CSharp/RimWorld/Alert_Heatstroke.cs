using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001952 RID: 6482
	public class Alert_Heatstroke : Alert
	{
		// Token: 0x06008F8F RID: 36751 RVA: 0x000602F8 File Offset: 0x0005E4F8
		public Alert_Heatstroke()
		{
			this.defaultLabel = "AlertHeatstroke".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x170016AC RID: 5804
		// (get) Token: 0x06008F90 RID: 36752 RVA: 0x002954AC File Offset: 0x002936AC
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

		// Token: 0x06008F91 RID: 36753 RVA: 0x00295528 File Offset: 0x00293728
		public override TaggedString GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn pawn in this.HeatstrokePawns)
			{
				stringBuilder.AppendLine("  - " + pawn.NameShortColored.Resolve());
			}
			return string.Format("AlertHeatstrokeDesc".Translate(), stringBuilder.ToString());
		}

		// Token: 0x06008F92 RID: 36754 RVA: 0x00060327 File Offset: 0x0005E527
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.HeatstrokePawns);
		}

		// Token: 0x04005B70 RID: 23408
		private List<Pawn> heatstrokePawnsResult = new List<Pawn>();
	}
}
