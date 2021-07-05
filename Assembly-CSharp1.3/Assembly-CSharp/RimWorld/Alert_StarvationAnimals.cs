using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001279 RID: 4729
	public class Alert_StarvationAnimals : Alert
	{
		// Token: 0x06007117 RID: 28951 RVA: 0x0025AE2E File Offset: 0x0025902E
		public Alert_StarvationAnimals()
		{
			this.defaultLabel = "StarvationAnimals".Translate();
		}

		// Token: 0x170013B9 RID: 5049
		// (get) Token: 0x06007118 RID: 28952 RVA: 0x0025AE58 File Offset: 0x00259058
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

		// Token: 0x06007119 RID: 28953 RVA: 0x0025AF2C File Offset: 0x0025912C
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

		// Token: 0x0600711A RID: 28954 RVA: 0x0025B00C File Offset: 0x0025920C
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.StarvingAnimals);
		}

		// Token: 0x04003E37 RID: 15927
		private List<Pawn> starvingAnimalsResult = new List<Pawn>();
	}
}
