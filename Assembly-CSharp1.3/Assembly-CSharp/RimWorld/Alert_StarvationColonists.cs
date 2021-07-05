using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001261 RID: 4705
	public class Alert_StarvationColonists : Alert
	{
		// Token: 0x060070B8 RID: 28856 RVA: 0x00258EE5 File Offset: 0x002570E5
		public Alert_StarvationColonists()
		{
			this.defaultLabel = "Starvation".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x170013A6 RID: 5030
		// (get) Token: 0x060070B9 RID: 28857 RVA: 0x00258F14 File Offset: 0x00257114
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

		// Token: 0x060070BA RID: 28858 RVA: 0x00258F9C File Offset: 0x0025719C
		public override TaggedString GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn pawn in this.StarvingColonists)
			{
				stringBuilder.AppendLine("  - " + pawn.NameShortColored.Resolve());
			}
			return "StarvationDesc".Translate(stringBuilder.ToString());
		}

		// Token: 0x060070BB RID: 28859 RVA: 0x00259024 File Offset: 0x00257224
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.StarvingColonists);
		}

		// Token: 0x04003E21 RID: 15905
		private List<Pawn> starvingColonistsResult = new List<Pawn>();
	}
}
