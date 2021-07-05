using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020012FE RID: 4862
	public sealed class ResourceCounter
	{
		// Token: 0x17001041 RID: 4161
		// (get) Token: 0x06006980 RID: 27008 RVA: 0x00047F92 File Offset: 0x00046192
		public int Silver
		{
			get
			{
				return this.GetCount(ThingDefOf.Silver);
			}
		}

		// Token: 0x17001042 RID: 4162
		// (get) Token: 0x06006981 RID: 27009 RVA: 0x00207FE0 File Offset: 0x002061E0
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

		// Token: 0x17001043 RID: 4163
		// (get) Token: 0x06006982 RID: 27010 RVA: 0x00047F9F File Offset: 0x0004619F
		public Dictionary<ThingDef, int> AllCountedAmounts
		{
			get
			{
				return this.countedAmounts;
			}
		}

		// Token: 0x06006983 RID: 27011 RVA: 0x00208074 File Offset: 0x00206274
		public static void ResetDefs()
		{
			ResourceCounter.resources.Clear();
			ResourceCounter.resources.AddRange(from def in DefDatabase<ThingDef>.AllDefs
			where def.CountAsResource
			orderby def.resourceReadoutPriority descending
			select def);
		}

		// Token: 0x06006984 RID: 27012 RVA: 0x00047FA7 File Offset: 0x000461A7
		public ResourceCounter(Map map)
		{
			this.map = map;
			this.ResetResourceCounts();
		}

		// Token: 0x06006985 RID: 27013 RVA: 0x002080E4 File Offset: 0x002062E4
		public void ResetResourceCounts()
		{
			this.countedAmounts.Clear();
			for (int i = 0; i < ResourceCounter.resources.Count; i++)
			{
				this.countedAmounts.Add(ResourceCounter.resources[i], 0);
			}
		}

		// Token: 0x06006986 RID: 27014 RVA: 0x00208128 File Offset: 0x00206328
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
			Log.Error("Looked for nonexistent key " + rDef + " in counted resources.", false);
			this.countedAmounts.Add(rDef, 0);
			return 0;
		}

		// Token: 0x06006987 RID: 27015 RVA: 0x00208178 File Offset: 0x00206378
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

		// Token: 0x06006988 RID: 27016 RVA: 0x002081E0 File Offset: 0x002063E0
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

		// Token: 0x06006989 RID: 27017 RVA: 0x00047FC7 File Offset: 0x000461C7
		public void ResourceCounterTick()
		{
			if (Find.TickManager.TicksGame % 204 == 0)
			{
				this.UpdateResourceCounts();
			}
		}

		// Token: 0x0600698A RID: 27018 RVA: 0x0020825C File Offset: 0x0020645C
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

		// Token: 0x0600698B RID: 27019 RVA: 0x00047FE1 File Offset: 0x000461E1
		private bool ShouldCount(Thing t)
		{
			return !t.IsNotFresh();
		}

		// Token: 0x04004648 RID: 17992
		private Map map;

		// Token: 0x04004649 RID: 17993
		private Dictionary<ThingDef, int> countedAmounts = new Dictionary<ThingDef, int>();

		// Token: 0x0400464A RID: 17994
		private static List<ThingDef> resources = new List<ThingDef>();
	}
}
