using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200126E RID: 4718
	public class Alert_TitleRequiresBedroom : Alert
	{
		// Token: 0x060070EF RID: 28911 RVA: 0x0025A12F File Offset: 0x0025832F
		public Alert_TitleRequiresBedroom()
		{
			this.defaultLabel = "NeedBedroomAssigned".Translate();
			this.defaultExplanation = "NeedBedroomAssignedDesc".Translate();
		}

		// Token: 0x170013B3 RID: 5043
		// (get) Token: 0x060070F0 RID: 28912 RVA: 0x0025A16C File Offset: 0x0025836C
		public List<Pawn> Targets
		{
			get
			{
				this.targetsResult.Clear();
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					foreach (Pawn pawn in maps[i].mapPawns.FreeColonists)
					{
						if (pawn.royalty != null && pawn.royalty.CanRequireBedroom() && pawn.royalty.HighestTitleWithBedroomRequirements() != null && !pawn.Suspended && !pawn.royalty.HasPersonalBedroom())
						{
							this.targetsResult.Add(pawn);
						}
					}
				}
				return this.targetsResult;
			}
		}

		// Token: 0x060070F1 RID: 28913 RVA: 0x0025A234 File Offset: 0x00258434
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.Targets);
		}

		// Token: 0x04003E2F RID: 15919
		private List<Pawn> targetsResult = new List<Pawn>();
	}
}
