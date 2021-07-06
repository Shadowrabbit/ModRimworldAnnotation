using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200196B RID: 6507
	public class Alert_BilliardsTableOnWall : Alert
	{
		// Token: 0x170016BB RID: 5819
		// (get) Token: 0x06008FEC RID: 36844 RVA: 0x00296D38 File Offset: 0x00294F38
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

		// Token: 0x06008FED RID: 36845 RVA: 0x00060830 File Offset: 0x0005EA30
		public Alert_BilliardsTableOnWall()
		{
			this.defaultLabel = "BilliardsNeedsSpace".Translate();
			this.defaultExplanation = "BilliardsNeedsSpaceDesc".Translate();
		}

		// Token: 0x06008FEE RID: 36846 RVA: 0x0006086D File Offset: 0x0005EA6D
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.BadTables);
		}

		// Token: 0x04005B8D RID: 23437
		private List<Thing> badTablesResult = new List<Thing>();
	}
}
