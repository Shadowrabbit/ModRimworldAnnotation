using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200125E RID: 4702
	public class Alert_ColonistLeftUnburied : Alert
	{
		// Token: 0x060070AD RID: 28845 RVA: 0x00258B10 File Offset: 0x00256D10
		public static bool IsCorpseOfColonist(Corpse corpse)
		{
			return corpse.InnerPawn.Faction == Faction.OfPlayer && corpse.InnerPawn.def.race.Humanlike && !corpse.InnerPawn.IsQuestLodger() && !corpse.InnerPawn.IsSlave && !corpse.IsInAnyStorage();
		}

		// Token: 0x170013A3 RID: 5027
		// (get) Token: 0x060070AE RID: 28846 RVA: 0x00258B6C File Offset: 0x00256D6C
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

		// Token: 0x060070AF RID: 28847 RVA: 0x00258C1C File Offset: 0x00256E1C
		public Alert_ColonistLeftUnburied()
		{
			this.defaultLabel = "AlertColonistLeftUnburied".Translate();
			this.defaultExplanation = "AlertColonistLeftUnburiedDesc".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x060070B0 RID: 28848 RVA: 0x00258C6B File Offset: 0x00256E6B
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.UnburiedColonistCorpses);
		}

		// Token: 0x04003E1E RID: 15902
		private List<Thing> unburiedColonistCorpsesResult = new List<Thing>();
	}
}
