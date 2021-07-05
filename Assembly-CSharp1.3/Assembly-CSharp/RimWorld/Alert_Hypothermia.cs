using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001240 RID: 4672
	public class Alert_Hypothermia : Alert_Critical
	{
		// Token: 0x0600702A RID: 28714 RVA: 0x00255BC0 File Offset: 0x00253DC0
		public Alert_Hypothermia()
		{
			this.defaultLabel = "AlertHypothermia".Translate();
		}

		// Token: 0x17001394 RID: 5012
		// (get) Token: 0x0600702B RID: 28715 RVA: 0x00255BE8 File Offset: 0x00253DE8
		private List<Pawn> HypothermiaDangerColonists
		{
			get
			{
				this.hypothermiaDangerColonistsResult.Clear();
				foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonists_NoCryptosleep)
				{
					if (!pawn.SafeTemperatureRange().Includes(pawn.AmbientTemperature))
					{
						Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Hypothermia, false);
						if (firstHediffOfDef != null && firstHediffOfDef.CurStageIndex >= 3)
						{
							this.hypothermiaDangerColonistsResult.Add(pawn);
						}
					}
				}
				return this.hypothermiaDangerColonistsResult;
			}
		}

		// Token: 0x0600702C RID: 28716 RVA: 0x00255C8C File Offset: 0x00253E8C
		public override TaggedString GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn pawn in this.HypothermiaDangerColonists)
			{
				stringBuilder.AppendLine("  - " + pawn.NameShortColored.Resolve());
			}
			return "AlertHypothermiaDesc".Translate(stringBuilder.ToString());
		}

		// Token: 0x0600702D RID: 28717 RVA: 0x00255D14 File Offset: 0x00253F14
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.HypothermiaDangerColonists);
		}

		// Token: 0x04003DF4 RID: 15860
		private List<Pawn> hypothermiaDangerColonistsResult = new List<Pawn>();
	}
}
