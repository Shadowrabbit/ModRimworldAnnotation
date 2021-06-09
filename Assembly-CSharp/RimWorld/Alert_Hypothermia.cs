using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001938 RID: 6456
	public class Alert_Hypothermia : Alert_Critical
	{
		// Token: 0x06008F20 RID: 36640 RVA: 0x0005FCBB File Offset: 0x0005DEBB
		public Alert_Hypothermia()
		{
			this.defaultLabel = "AlertHypothermia".Translate();
		}

		// Token: 0x1700169C RID: 5788
		// (get) Token: 0x06008F21 RID: 36641 RVA: 0x002936EC File Offset: 0x002918EC
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

		// Token: 0x06008F22 RID: 36642 RVA: 0x00293790 File Offset: 0x00291990
		public override TaggedString GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn pawn in this.HypothermiaDangerColonists)
			{
				stringBuilder.AppendLine("  - " + pawn.NameShortColored.Resolve());
			}
			return "AlertHypothermiaDesc".Translate(stringBuilder.ToString());
		}

		// Token: 0x06008F23 RID: 36643 RVA: 0x0005FCE3 File Offset: 0x0005DEE3
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.HypothermiaDangerColonists);
		}

		// Token: 0x04005B51 RID: 23377
		private List<Pawn> hypothermiaDangerColonistsResult = new List<Pawn>();
	}
}
