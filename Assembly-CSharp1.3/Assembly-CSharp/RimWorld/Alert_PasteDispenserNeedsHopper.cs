using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001264 RID: 4708
	public class Alert_PasteDispenserNeedsHopper : Alert
	{
		// Token: 0x170013A9 RID: 5033
		// (get) Token: 0x060070C4 RID: 28868 RVA: 0x002592D0 File Offset: 0x002574D0
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

		// Token: 0x060070C5 RID: 28869 RVA: 0x002593D4 File Offset: 0x002575D4
		public Alert_PasteDispenserNeedsHopper()
		{
			this.defaultLabel = "NeedFoodHopper".Translate();
			this.defaultExplanation = "NeedFoodHopperDesc".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x060070C6 RID: 28870 RVA: 0x00259423 File Offset: 0x00257623
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.BadDispensers);
		}

		// Token: 0x04003E24 RID: 15908
		private List<Thing> badDispensersResult = new List<Thing>();
	}
}
