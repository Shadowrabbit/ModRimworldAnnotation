using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200194D RID: 6477
	public class Alert_ColonistLeftUnburied : Alert
	{
		// Token: 0x06008F7C RID: 36732 RVA: 0x00294F20 File Offset: 0x00293120
		public static bool IsCorpseOfColonist(Corpse corpse)
		{
			return corpse.InnerPawn.Faction == Faction.OfPlayer && corpse.InnerPawn.def.race.Humanlike && !corpse.InnerPawn.IsQuestLodger() && !corpse.IsInAnyStorage();
		}

		// Token: 0x170016A7 RID: 5799
		// (get) Token: 0x06008F7D RID: 36733 RVA: 0x00294F70 File Offset: 0x00293170
		private List<Thing> UnburiedColonistCorpses
		{
			get
			{
				this.unburiedColonistCorpsesResult.Clear();
				foreach (Map map in Find.Maps)
				{
					if (map.mapPawns.AnyFreeColonistSpawned)
					{
						List<Thing> list = map.listerThings.ThingsMatching(ThingRequest.ForGroup(ThingRequestGroup.Corpse));
						for (int i = 0; i < list.Count; i++)
						{
							Corpse corpse = (Corpse)list[i];
							if (Alert_ColonistLeftUnburied.IsCorpseOfColonist(corpse))
							{
								this.unburiedColonistCorpsesResult.Add(corpse);
							}
						}
					}
				}
				return this.unburiedColonistCorpsesResult;
			}
		}

		// Token: 0x06008F7E RID: 36734 RVA: 0x00295020 File Offset: 0x00293220
		public Alert_ColonistLeftUnburied()
		{
			this.defaultLabel = "AlertColonistLeftUnburied".Translate();
			this.defaultExplanation = "AlertColonistLeftUnburiedDesc".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x06008F7F RID: 36735 RVA: 0x0006022A File Offset: 0x0005E42A
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.UnburiedColonistCorpses);
		}

		// Token: 0x04005B6B RID: 23403
		private List<Thing> unburiedColonistCorpsesResult = new List<Thing>();
	}
}
