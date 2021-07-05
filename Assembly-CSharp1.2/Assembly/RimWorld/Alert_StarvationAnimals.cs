using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001969 RID: 6505
	public class Alert_StarvationAnimals : Alert
	{
		// Token: 0x06008FE5 RID: 36837 RVA: 0x000607E2 File Offset: 0x0005E9E2
		public Alert_StarvationAnimals()
		{
			this.defaultLabel = "StarvationAnimals".Translate();
		}

		// Token: 0x170016BA RID: 5818
		// (get) Token: 0x06008FE6 RID: 36838 RVA: 0x00296B84 File Offset: 0x00294D84
		private List<Pawn> StarvingAnimals
		{
			get
			{
				this.starvingAnimalsResult.Clear();
				foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_OfPlayerFaction_NoCryptosleep)
				{
					if (pawn.HostFaction == null && !pawn.RaceProps.Humanlike && pawn.needs.food != null && (pawn.needs.food.TicksStarving > 30000 || (pawn.health.hediffSet.HasHediff(HediffDefOf.Pregnant, true) && pawn.needs.food.TicksStarving > 5000)))
					{
						this.starvingAnimalsResult.Add(pawn);
					}
				}
				return this.starvingAnimalsResult;
			}
		}

		// Token: 0x06008FE7 RID: 36839 RVA: 0x00296C58 File Offset: 0x00294E58
		public override TaggedString GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn pawn in from a in this.StarvingAnimals
			orderby a.def.label
			select a)
			{
				stringBuilder.Append("    " + pawn.LabelShortCap);
				if (pawn.Name.IsValid && !pawn.Name.Numerical)
				{
					stringBuilder.Append(" (" + pawn.def.label + ")");
				}
				stringBuilder.AppendLine();
			}
			return "StarvationAnimalsDesc".Translate(stringBuilder.ToString());
		}

		// Token: 0x06008FE8 RID: 36840 RVA: 0x0006080A File Offset: 0x0005EA0A
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.StarvingAnimals);
		}

		// Token: 0x04005B8A RID: 23434
		private List<Pawn> starvingAnimalsResult = new List<Pawn>();
	}
}
