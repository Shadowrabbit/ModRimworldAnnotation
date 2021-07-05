using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200126B RID: 4715
	public class Alert_RoyalNoThroneAssigned : Alert
	{
		// Token: 0x060070E4 RID: 28900 RVA: 0x00259D2C File Offset: 0x00257F2C
		public Alert_RoyalNoThroneAssigned()
		{
			this.defaultLabel = "NeedThroneAssigned".Translate();
			this.defaultExplanation = "NeedThroneAssignedDesc".Translate();
		}

		// Token: 0x170013B0 RID: 5040
		// (get) Token: 0x060070E5 RID: 28901 RVA: 0x00259D6C File Offset: 0x00257F6C
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
						if (pawn.royalty != null && !pawn.Suspended && pawn.royalty.CanRequireThroneroom())
						{
							bool flag = false;
							List<RoyalTitle> allTitlesForReading = pawn.royalty.AllTitlesForReading;
							for (int j = 0; j < allTitlesForReading.Count; j++)
							{
								if (!allTitlesForReading[j].def.throneRoomRequirements.NullOrEmpty<RoomRequirement>())
								{
									flag = true;
									break;
								}
							}
							if (flag && pawn.ownership.AssignedThrone == null)
							{
								this.targetsResult.Add(pawn);
							}
						}
					}
				}
				return this.targetsResult;
			}
		}

		// Token: 0x060070E6 RID: 28902 RVA: 0x00259E78 File Offset: 0x00258078
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.Targets);
		}

		// Token: 0x04003E2C RID: 15916
		private List<Pawn> targetsResult = new List<Pawn>();
	}
}
