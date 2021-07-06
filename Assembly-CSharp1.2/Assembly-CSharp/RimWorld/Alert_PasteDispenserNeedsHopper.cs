using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001953 RID: 6483
	public class Alert_PasteDispenserNeedsHopper : Alert
	{
		// Token: 0x170016AD RID: 5805
		// (get) Token: 0x06008F93 RID: 36755 RVA: 0x002955B8 File Offset: 0x002937B8
		private List<Thing> BadDispensers
		{
			get
			{
				this.badDispensersResult.Clear();
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					foreach (Thing thing in maps[i].listerThings.ThingsInGroup(ThingRequestGroup.FoodDispenser))
					{
						bool flag = false;
						ThingDef hopper = ThingDefOf.Hopper;
						foreach (IntVec3 c in ((Building_NutrientPasteDispenser)thing).AdjCellsCardinalInBounds)
						{
							Thing edifice = c.GetEdifice(thing.Map);
							if (edifice != null && edifice.def == hopper)
							{
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							this.badDispensersResult.Add(thing);
						}
					}
				}
				return this.badDispensersResult;
			}
		}

		// Token: 0x06008F94 RID: 36756 RVA: 0x002956BC File Offset: 0x002938BC
		public Alert_PasteDispenserNeedsHopper()
		{
			this.defaultLabel = "NeedFoodHopper".Translate();
			this.defaultExplanation = "NeedFoodHopperDesc".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x06008F95 RID: 36757 RVA: 0x00060334 File Offset: 0x0005E534
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.BadDispensers);
		}

		// Token: 0x04005B71 RID: 23409
		private List<Thing> badDispensersResult = new List<Thing>();
	}
}
