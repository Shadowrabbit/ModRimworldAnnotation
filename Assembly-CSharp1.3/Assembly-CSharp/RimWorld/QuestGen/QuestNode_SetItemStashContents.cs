using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020016E3 RID: 5859
	public class QuestNode_SetItemStashContents : QuestNode
	{
		// Token: 0x06008767 RID: 34663 RVA: 0x000126F5 File Offset: 0x000108F5
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x06008768 RID: 34664 RVA: 0x003071BC File Offset: 0x003053BC
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			slate.Set<IEnumerable<ThingDef>>("itemStashThings", this.GetContents(slate), false);
		}

		// Token: 0x06008769 RID: 34665 RVA: 0x003071E2 File Offset: 0x003053E2
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

		// Token: 0x0400557B RID: 21883
		public SlateRef<IEnumerable<ThingDef>> items;

		// Token: 0x0400557C RID: 21884
		public SlateRef<List<QuestNode_SetItemStashContents.ThingCategoryCount>> categories;

		// Token: 0x0400557D RID: 21885
		private static List<ThingDef> tmpItems = new List<ThingDef>();

		// Token: 0x0200294E RID: 10574
		public class ThingCategoryCount
		{
			// Token: 0x04009B53 RID: 39763
			public ThingCategoryDef category;

			// Token: 0x04009B54 RID: 39764
			public IntRange amount;

			// Token: 0x04009B55 RID: 39765
			public bool allowDuplicates = true;
		}
	}
}
