using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001FBA RID: 8122
	public class QuestNode_SetItemStashContents : QuestNode
	{
		// Token: 0x0600AC70 RID: 44144 RVA: 0x0000A2A7 File Offset: 0x000084A7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600AC71 RID: 44145 RVA: 0x00322218 File Offset: 0x00320418
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			slate.Set<IEnumerable<ThingDef>>("itemStashThings", this.GetContents(slate), false);
		}

		// Token: 0x0600AC72 RID: 44146 RVA: 0x0007096D File Offset: 0x0006EB6D
		private IEnumerable<ThingDef> GetContents(Slate slate)
		{
			IEnumerable<ThingDef> value = this.items.GetValue(slate);
			if (value != null)
			{
				foreach (ThingDef thingDef in value)
				{
					yield return thingDef;
				}
				IEnumerator<ThingDef> enumerator = null;
			}
			List<QuestNode_SetItemStashContents.ThingCategoryCount> value2 = this.categories.GetValue(slate);
			if (value2 != null)
			{
				using (List<QuestNode_SetItemStashContents.ThingCategoryCount>.Enumerator enumerator2 = value2.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						QuestNode_SetItemStashContents.<>c__DisplayClass6_0 CS$<>8__locals1 = new QuestNode_SetItemStashContents.<>c__DisplayClass6_0();
						CS$<>8__locals1.c = enumerator2.Current;
						try
						{
							int amt = Mathf.Max(CS$<>8__locals1.c.amount.RandomInRange, 1);
							int num;
							for (int i = 0; i < amt; i = num + 1)
							{
								IEnumerable<ThingDef> allDefs = DefDatabase<ThingDef>.AllDefs;
								Func<ThingDef, bool> predicate;
								if ((predicate = CS$<>8__locals1.<>9__0) == null)
								{
									predicate = (CS$<>8__locals1.<>9__0 = ((ThingDef x) => x.thingCategories != null && x.thingCategories.Contains(CS$<>8__locals1.c.category) && (CS$<>8__locals1.c.allowDuplicates || !QuestNode_SetItemStashContents.tmpItems.Contains(x))));
								}
								ThingDef thingDef2;
								if (allDefs.Where(predicate).TryRandomElement(out thingDef2))
								{
									QuestNode_SetItemStashContents.tmpItems.Add(thingDef2);
									yield return thingDef2;
								}
								num = i;
							}
						}
						finally
						{
							QuestNode_SetItemStashContents.tmpItems.Clear();
						}
						CS$<>8__locals1 = null;
					}
				}
				List<QuestNode_SetItemStashContents.ThingCategoryCount>.Enumerator enumerator2 = default(List<QuestNode_SetItemStashContents.ThingCategoryCount>.Enumerator);
			}
			yield break;
			yield break;
		}

		// Token: 0x040075E4 RID: 30180
		public SlateRef<IEnumerable<ThingDef>> items;

		// Token: 0x040075E5 RID: 30181
		public SlateRef<List<QuestNode_SetItemStashContents.ThingCategoryCount>> categories;

		// Token: 0x040075E6 RID: 30182
		private static List<ThingDef> tmpItems = new List<ThingDef>();

		// Token: 0x02001FBB RID: 8123
		public class ThingCategoryCount
		{
			// Token: 0x040075E7 RID: 30183
			public ThingCategoryDef category;

			// Token: 0x040075E8 RID: 30184
			public IntRange amount;

			// Token: 0x040075E9 RID: 30185
			public bool allowDuplicates = true;
		}
	}
}
