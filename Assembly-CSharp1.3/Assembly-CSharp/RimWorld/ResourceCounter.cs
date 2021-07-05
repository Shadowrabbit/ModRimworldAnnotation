using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CDB RID: 3291
	public sealed class ResourceCounter
	{
		// Token: 0x17000D3E RID: 3390
		// (get) Token: 0x06004CD3 RID: 19667 RVA: 0x0019A1DC File Offset: 0x001983DC
		public int Silver
		{
			get
			{
				return this.GetCount(ThingDefOf.Silver);
			}
		}

		// Token: 0x17000D3F RID: 3391
		// (get) Token: 0x06004CD4 RID: 19668 RVA: 0x0019A1EC File Offset: 0x001983EC
		public float TotalHumanEdibleNutrition
		{
			get
			{
				float num = 0f;
				foreach (KeyValuePair<ThingDef, int> keyValuePair in this.countedAmounts)
				{
					if (keyValuePair.Key.IsNutritionGivingIngestible && keyValuePair.Key.ingestible.HumanEdible)
					{
						num += keyValuePair.Key.GetStatValueAbstract(StatDefOf.Nutrition, null) * (float)keyValuePair.Value;
					}
				}
				return num;
			}
		}

		// Token: 0x17000D40 RID: 3392
		// (get) Token: 0x06004CD5 RID: 19669 RVA: 0x0019A280 File Offset: 0x00198480
		public Dictionary<ThingDef, int> AllCountedAmounts
		{
			get
			{
				return this.countedAmounts;
			}
		}

		// Token: 0x06004CD6 RID: 19670 RVA: 0x0019A288 File Offset: 0x00198488
		public static void ResetDefs()
		{
			ResourceCounter.resources.Clear();
			ResourceCounter.resources.AddRange(from def in DefDatabase<ThingDef>.AllDefs
			where def.CountAsResource
			orderby def.resourceReadoutPriority descending
			select def);
		}

		// Token: 0x06004CD7 RID: 19671 RVA: 0x0019A2F6 File Offset: 0x001984F6
		public ResourceCounter(Map map)
		{
			this.map = map;
			this.ResetResourceCounts();
		}

		// Token: 0x06004CD8 RID: 19672 RVA: 0x0019A318 File Offset: 0x00198518
		public void ResetResourceCounts()
		{
			this.countedAmounts.Clear();
			for (int i = 0; i < ResourceCounter.resources.Count; i++)
			{
				this.countedAmounts.Add(ResourceCounter.resources[i], 0);
			}
		}

		// Token: 0x06004CD9 RID: 19673 RVA: 0x0019A35C File Offset: 0x0019855C
		public int GetCount(ThingDef rDef)
		{
			if (rDef.resourceReadoutPriority == ResourceCountPriority.Uncounted)
			{
				return 0;
			}
			int result;
			if (this.countedAmounts.TryGetValue(rDef, out result))
			{
				return result;
			}
			Log.Error("Looked for nonexistent key " + rDef + " in counted resources.");
			this.countedAmounts.Add(rDef, 0);
			return 0;
		}

		// Token: 0x06004CDA RID: 19674 RVA: 0x0019A3A8 File Offset: 0x001985A8
		public int GetCountIn(ThingRequestGroup group)
		{
			int num = 0;
			foreach (KeyValuePair<ThingDef, int> keyValuePair in this.countedAmounts)
			{
				if (group.Includes(keyValuePair.Key))
				{
					num += keyValuePair.Value;
				}
			}
			return num;
		}

		// Token: 0x06004CDB RID: 19675 RVA: 0x0019A410 File Offset: 0x00198610
		public int GetCountIn(ThingCategoryDef cat)
		{
			int num = 0;
			for (int i = 0; i < cat.childThingDefs.Count; i++)
			{
				num += this.GetCount(cat.childThingDefs[i]);
			}
			for (int j = 0; j < cat.childCategories.Count; j++)
			{
				if (!cat.childCategories[j].resourceReadoutRoot)
				{
					num += this.GetCountIn(cat.childCategories[j]);
				}
			}
			return num;
		}

		// Token: 0x06004CDC RID: 19676 RVA: 0x0019A489 File Offset: 0x00198689
		public void ResourceCounterTick()
		{
			if (Find.TickManager.TicksGame % 204 == 0)
			{
				this.UpdateResourceCounts();
			}
		}

		// Token: 0x06004CDD RID: 19677 RVA: 0x0019A4A4 File Offset: 0x001986A4
		public void UpdateResourceCounts()
		{
			this.ResetResourceCounts();
			List<SlotGroup> allGroupsListForReading = this.map.haulDestinationManager.AllGroupsListForReading;
			for (int i = 0; i < allGroupsListForReading.Count; i++)
			{
				foreach (Thing outerThing in allGroupsListForReading[i].HeldThings)
				{
					Thing innerIfMinified = outerThing.GetInnerIfMinified();
					if (innerIfMinified.def.CountAsResource && this.ShouldCount(innerIfMinified))
					{
						Dictionary<ThingDef, int> dictionary = this.countedAmounts;
						ThingDef def = innerIfMinified.def;
						dictionary[def] += innerIfMinified.stackCount;
					}
				}
			}
		}

		// Token: 0x06004CDE RID: 19678 RVA: 0x0019A55C File Offset: 0x0019875C
		private bool ShouldCount(Thing t)
		{
			return !t.IsNotFresh();
		}

		// Token: 0x04002E7F RID: 11903
		private Map map;

		// Token: 0x04002E80 RID: 11904
		private Dictionary<ThingDef, int> countedAmounts = new Dictionary<ThingDef, int>();

		// Token: 0x04002E81 RID: 11905
		private static List<ThingDef> resources = new List<ThingDef>();
	}
}
