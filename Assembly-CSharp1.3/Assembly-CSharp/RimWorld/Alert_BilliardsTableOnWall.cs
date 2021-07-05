using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200127A RID: 4730
	public class Alert_BilliardsTableOnWall : Alert
	{
		// Token: 0x170013BA RID: 5050
		// (get) Token: 0x0600711B RID: 28955 RVA: 0x0025B01C File Offset: 0x0025921C
		private List<Thing> BadTables
		{
			get
			{
				this.badTablesResult.Clear();
				List<Map> maps = Find.Maps;
				Faction ofPlayer = Faction.OfPlayer;
				for (int i = 0; i < maps.Count; i++)
				{
					List<Thing> list = maps[i].listerThings.ThingsOfDef(ThingDefOf.BilliardsTable);
					for (int j = 0; j < list.Count; j++)
					{
						if (list[j].Faction == ofPlayer && !JoyGiver_PlayBilliards.ThingHasStandableSpaceOnAllSides(list[j]))
						{
							this.badTablesResult.Add(list[j]);
						}
					}
				}
				return this.badTablesResult;
			}
		}

		// Token: 0x0600711C RID: 28956 RVA: 0x0025B0B5 File Offset: 0x002592B5
		public Alert_BilliardsTableOnWall()
		{
			this.defaultLabel = "BilliardsNeedsSpace".Translate();
			this.defaultExplanation = "BilliardsNeedsSpaceDesc".Translate();
		}

		// Token: 0x0600711D RID: 28957 RVA: 0x0025B0F2 File Offset: 0x002592F2
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.BadTables);
		}

		// Token: 0x04003E38 RID: 15928
		private List<Thing> badTablesResult = new List<Thing>();
	}
}
