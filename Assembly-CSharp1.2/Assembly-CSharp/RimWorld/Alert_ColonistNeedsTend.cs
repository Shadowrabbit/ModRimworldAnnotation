using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x0200194F RID: 6479
	public class Alert_ColonistNeedsTend : Alert
	{
		// Token: 0x06008F83 RID: 36739 RVA: 0x00060244 File Offset: 0x0005E444
		public Alert_ColonistNeedsTend()
		{
			this.defaultLabel = "ColonistNeedsTreatment".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x170016A9 RID: 5801
		// (get) Token: 0x06008F84 RID: 36740 RVA: 0x0029515C File Offset: 0x0029335C
		private List<Pawn> NeedingColonists
		{
			get
			{
				this.needingColonistsResult.Clear();
				foreach (Pawn pawn in PawnsFinder.AllMaps_FreeColonists)
				{
					if ((pawn.Spawned || pawn.BrieflyDespawned()) && pawn.health.HasHediffsNeedingTendByPlayer(true))
					{
						Building_Bed building_Bed = pawn.CurrentBed();
						if ((building_Bed == null || !building_Bed.Medical) && !Alert_ColonistNeedsRescuing.NeedsRescue(pawn))
						{
							this.needingColonistsResult.Add(pawn);
						}
					}
				}
				return this.needingColonistsResult;
			}
		}

		// Token: 0x06008F85 RID: 36741 RVA: 0x002951FC File Offset: 0x002933FC
		public override TaggedString GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn pawn in this.NeedingColonists)
			{
				stringBuilder.AppendLine("  - " + pawn.NameShortColored.Resolve());
			}
			return "ColonistNeedsTreatmentDesc".Translate(stringBuilder.ToString());
		}

		// Token: 0x06008F86 RID: 36742 RVA: 0x00060273 File Offset: 0x0005E473
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.NeedingColonists);
		}

		// Token: 0x04005B6D RID: 23405
		private List<Pawn> needingColonistsResult = new List<Pawn>();
	}
}
