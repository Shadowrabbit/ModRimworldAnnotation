using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001260 RID: 4704
	public class Alert_ColonistNeedsTend : Alert
	{
		// Token: 0x060070B4 RID: 28852 RVA: 0x00258D80 File Offset: 0x00256F80
		public Alert_ColonistNeedsTend()
		{
			this.defaultLabel = "ColonistNeedsTreatment".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x170013A5 RID: 5029
		// (get) Token: 0x060070B5 RID: 28853 RVA: 0x00258DB0 File Offset: 0x00256FB0
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

		// Token: 0x060070B6 RID: 28854 RVA: 0x00258E50 File Offset: 0x00257050
		public override TaggedString GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn pawn in this.NeedingColonists)
			{
				stringBuilder.AppendLine("  - " + pawn.NameShortColored.Resolve());
			}
			return "ColonistNeedsTreatmentDesc".Translate(stringBuilder.ToString());
		}

		// Token: 0x060070B7 RID: 28855 RVA: 0x00258ED8 File Offset: 0x002570D8
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.NeedingColonists);
		}

		// Token: 0x04003E20 RID: 15904
		private List<Pawn> needingColonistsResult = new List<Pawn>();
	}
}
