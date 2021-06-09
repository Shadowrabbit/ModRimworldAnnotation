using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200195C RID: 6492
	public class Alert_TitleRequiresBedroom : Alert
	{
		// Token: 0x06008FB6 RID: 36790 RVA: 0x000604F2 File Offset: 0x0005E6F2
		public Alert_TitleRequiresBedroom()
		{
			this.defaultLabel = "NeedBedroomAssigned".Translate();
			this.defaultExplanation = "NeedBedroomAssignedDesc".Translate();
		}

		// Token: 0x170016B4 RID: 5812
		// (get) Token: 0x06008FB7 RID: 36791 RVA: 0x002960F4 File Offset: 0x002942F4
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

		// Token: 0x06008FB8 RID: 36792 RVA: 0x0006052F File Offset: 0x0005E72F
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.Targets);
		}

		// Token: 0x04005B7D RID: 23421
		private List<Pawn> targetsResult = new List<Pawn>();
	}
}
