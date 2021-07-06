using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001957 RID: 6487
	public class Alert_RoyalNoThroneAssigned : Alert
	{
		// Token: 0x06008FA5 RID: 36773 RVA: 0x000603FC File Offset: 0x0005E5FC
		public Alert_RoyalNoThroneAssigned()
		{
			this.defaultLabel = "NeedThroneAssigned".Translate();
			this.defaultExplanation = "NeedThroneAssignedDesc".Translate();
		}

		// Token: 0x170016B1 RID: 5809
		// (get) Token: 0x06008FA6 RID: 36774 RVA: 0x00295B20 File Offset: 0x00293D20
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

		// Token: 0x06008FA7 RID: 36775 RVA: 0x00060439 File Offset: 0x0005E639
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.Targets);
		}

		// Token: 0x04005B76 RID: 23414
		private List<Pawn> targetsResult = new List<Pawn>();
	}
}
