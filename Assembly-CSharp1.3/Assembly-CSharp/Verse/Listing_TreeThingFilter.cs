using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000415 RID: 1045
	public class Listing_TreeThingFilter : Listing_Tree
	{
		// Token: 0x06001F5F RID: 8031 RVA: 0x000C3580 File Offset: 0x000C1780
		public Listing_TreeThingFilter(ThingFilter filter, ThingFilter parentFilter, IEnumerable<ThingDef> forceHiddenDefs, IEnumerable<SpecialThingFilterDef> forceHiddenFilters, List<ThingDef> suppressSmallVolumeTags, QuickSearchFilter searchFilter)
		{
			this.filter = filter;
			this.parentFilter = parentFilter;
			if (forceHiddenDefs != null)
			{
				this.forceHiddenDefs = forceHiddenDefs.ToList<ThingDef>();
			}
			if (forceHiddenFilters != null)
			{
				this.tempForceHiddenSpecialFilters = forceHiddenFilters.ToList<SpecialThingFilterDef>();
			}
			this.suppressSmallVolumeTags = suppressSmallVolumeTags;
			this.searchFilter = searchFilter;
		}

		// Token: 0x06001F60 RID: 8032 RVA: 0x000C35D4 File Offset: 0x000C17D4
		public void ListCategoryChildren(TreeNode_ThingCategory node, int openMask, Map map, Rect visibleRect)
		{
			this.visibleRect = visibleRect;
			int num = 0;
			foreach (SpecialThingFilterDef sfDef in node.catDef.ParentsSpecialThingFilterDefs)
			{
				if (this.Visible(sfDef, node))
				{
					this.DoSpecialFilter(sfDef, num);
				}
			}
			this.DoCategoryChildren(node, num, openMask, map, false);
		}

		// Token: 0x06001F61 RID: 8033 RVA: 0x000C3648 File Offset: 0x000C1848
		private void DoCategoryChildren(TreeNode_ThingCategory node, int indentLevel, int openMask, Map map, bool subtreeMatchedSearch)
		{
			Listing_TreeThingFilter.<>c__DisplayClass13_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.subtreeMatchedSearch = subtreeMatchedSearch;
			List<SpecialThingFilterDef> childSpecialFilters = node.catDef.childSpecialFilters;
			for (int i = 0; i < childSpecialFilters.Count; i++)
			{
				if (this.Visible(childSpecialFilters[i], node))
				{
					this.DoSpecialFilter(childSpecialFilters[i], indentLevel);
				}
			}
			foreach (TreeNode_ThingCategory treeNode_ThingCategory in node.ChildCategoryNodes)
			{
				if (this.Visible(treeNode_ThingCategory) && !this.<DoCategoryChildren>g__HideCategoryDueToSearch|13_0(treeNode_ThingCategory, ref CS$<>8__locals1))
				{
					this.DoCategory(treeNode_ThingCategory, indentLevel, openMask, map, CS$<>8__locals1.subtreeMatchedSearch);
				}
			}
			foreach (ThingDef thingDef in node.catDef.SortedChildThingDefs)
			{
				if (this.Visible(thingDef) && !this.<DoCategoryChildren>g__HideThingDueToSearch|13_1(thingDef, ref CS$<>8__locals1))
				{
					this.DoThingDef(thingDef, indentLevel, map);
				}
			}
		}

		// Token: 0x06001F62 RID: 8034 RVA: 0x000C3768 File Offset: 0x000C1968
		private void DoSpecialFilter(SpecialThingFilterDef sfDef, int nestLevel)
		{
			if (!sfDef.configurable)
			{
				return;
			}
			Color? textColor = null;
			if (this.searchFilter.Matches(sfDef))
			{
				this.matchCount++;
			}
			else
			{
				textColor = new Color?(Listing_TreeThingFilter.NoMatchColor);
			}
			if (this.CurrentRowVisibleOnScreen())
			{
				base.LabelLeft("*" + sfDef.LabelCap, sfDef.description, nestLevel, 0f, textColor);
				bool flag = this.filter.Allows(sfDef);
				bool flag2 = flag;
				Widgets.Checkbox(new Vector2(this.LabelWidth, this.curY), ref flag, this.lineHeight, false, true, null, null);
				if (flag != flag2)
				{
					this.filter.SetAllow(sfDef, flag);
				}
			}
			base.EndLine();
		}

		// Token: 0x06001F63 RID: 8035 RVA: 0x000C3828 File Offset: 0x000C1A28
		private void DoCategory(TreeNode_ThingCategory node, int indentLevel, int openMask, Map map, bool subtreeMatchedSearch)
		{
			Color? textColor = null;
			if (this.searchFilter.Active)
			{
				if (this.CategoryMatches(node))
				{
					subtreeMatchedSearch = true;
					this.matchCount++;
				}
				else
				{
					textColor = new Color?(Listing_TreeThingFilter.NoMatchColor);
				}
			}
			if (this.CurrentRowVisibleOnScreen())
			{
				base.OpenCloseWidget(node, indentLevel, openMask);
				base.LabelLeft(node.LabelCap, node.catDef.description, indentLevel, 0f, textColor);
				MultiCheckboxState multiCheckboxState = this.AllowanceStateOf(node);
				MultiCheckboxState multiCheckboxState2 = Widgets.CheckboxMulti(new Rect(this.LabelWidth, this.curY, this.lineHeight, this.lineHeight), multiCheckboxState, true);
				if (multiCheckboxState != multiCheckboxState2)
				{
					this.filter.SetAllow(node.catDef, multiCheckboxState2 == MultiCheckboxState.On, this.forceHiddenDefs, this.hiddenSpecialFilters);
				}
			}
			base.EndLine();
			if (this.IsOpen(node, openMask))
			{
				this.DoCategoryChildren(node, indentLevel + 1, openMask, map, subtreeMatchedSearch);
			}
		}

		// Token: 0x06001F64 RID: 8036 RVA: 0x000C3914 File Offset: 0x000C1B14
		private void DoThingDef(ThingDef tDef, int nestLevel, Map map)
		{
			Color? textColor = null;
			if (this.searchFilter.Matches(tDef))
			{
				this.matchCount++;
			}
			else
			{
				textColor = new Color?(Listing_TreeThingFilter.NoMatchColor);
			}
			if (this.CurrentRowVisibleOnScreen())
			{
				object obj = (this.suppressSmallVolumeTags == null || !this.suppressSmallVolumeTags.Contains(tDef)) && tDef.IsStuff && tDef.smallVolume;
				string text = tDef.DescriptionDetailed;
				object obj2 = obj;
				if (obj2 != null)
				{
					text += "\n\n" + "ThisIsSmallVolume".Translate(10.ToStringCached());
				}
				float num = -4f;
				if (obj2 != null)
				{
					Rect rect = new Rect(this.LabelWidth - 19f, this.curY, 19f, 20f);
					Text.Font = GameFont.Tiny;
					Text.Anchor = TextAnchor.UpperRight;
					GUI.color = Color.gray;
					Widgets.Label(rect, "/" + 10.ToStringCached());
					Text.Font = GameFont.Small;
					GenUI.ResetLabelAlign();
					GUI.color = Color.white;
				}
				num -= 19f;
				if (map != null)
				{
					int count = map.resourceCounter.GetCount(tDef);
					if (count > 0)
					{
						string text2 = count.ToStringCached();
						Rect rect2 = new Rect(0f, this.curY, this.LabelWidth + num, 40f);
						Text.Font = GameFont.Tiny;
						Text.Anchor = TextAnchor.UpperRight;
						GUI.color = new Color(0.5f, 0.5f, 0.1f);
						Widgets.Label(rect2, text2);
						num -= Text.CalcSize(text2).x;
						GenUI.ResetLabelAlign();
						Text.Font = GameFont.Small;
						GUI.color = Color.white;
					}
				}
				base.LabelLeft(tDef.LabelCap, text, nestLevel, num, textColor);
				bool flag = this.filter.Allows(tDef);
				bool flag2 = flag;
				Widgets.Checkbox(new Vector2(this.LabelWidth, this.curY), ref flag, this.lineHeight, false, true, null, null);
				if (flag != flag2)
				{
					this.filter.SetAllow(tDef, flag);
				}
			}
			base.EndLine();
		}

		// Token: 0x06001F65 RID: 8037 RVA: 0x000C3B1C File Offset: 0x000C1D1C
		public MultiCheckboxState AllowanceStateOf(TreeNode_ThingCategory cat)
		{
			int num = 0;
			int num2 = 0;
			foreach (ThingDef thingDef in cat.catDef.DescendantThingDefs)
			{
				if (this.Visible(thingDef))
				{
					num++;
					if (this.filter.Allows(thingDef))
					{
						num2++;
					}
				}
			}
			bool flag = false;
			foreach (SpecialThingFilterDef sf in cat.catDef.DescendantSpecialThingFilterDefs)
			{
				if (this.Visible(sf, cat) && !this.filter.Allows(sf))
				{
					flag = true;
					break;
				}
			}
			if (num2 == 0)
			{
				return MultiCheckboxState.Off;
			}
			if (num == num2 && !flag)
			{
				return MultiCheckboxState.On;
			}
			return MultiCheckboxState.Partial;
		}

		// Token: 0x06001F66 RID: 8038 RVA: 0x000C3BFC File Offset: 0x000C1DFC
		private bool Visible(ThingDef td)
		{
			if (!td.PlayerAcquirable)
			{
				return false;
			}
			if (this.forceHiddenDefs != null && this.forceHiddenDefs.Contains(td))
			{
				return false;
			}
			if (this.parentFilter != null)
			{
				if (!this.parentFilter.Allows(td))
				{
					return false;
				}
				if (this.parentFilter.IsAlwaysDisallowedDueToSpecialFilters(td))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06001F67 RID: 8039 RVA: 0x000C3C54 File Offset: 0x000C1E54
		public override bool IsOpen(TreeNode node, int openMask)
		{
			TreeNode_ThingCategory node2;
			return base.IsOpen(node, openMask) || ((node2 = (node as TreeNode_ThingCategory)) != null && this.searchFilter.Active && this.ThisOrDescendantsVisibleAndMatchesSearch(node2));
		}

		// Token: 0x06001F68 RID: 8040 RVA: 0x000C3C90 File Offset: 0x000C1E90
		private bool ThisOrDescendantsVisibleAndMatchesSearch(TreeNode_ThingCategory node)
		{
			if (this.Visible(node) && this.CategoryMatches(node))
			{
				return true;
			}
			foreach (ThingDef td in node.catDef.childThingDefs)
			{
				if (this.<ThisOrDescendantsVisibleAndMatchesSearch>g__ThingDefVisibleAndMatches|20_0(td))
				{
					return true;
				}
			}
			foreach (SpecialThingFilterDef sf in node.catDef.childSpecialFilters)
			{
				if (this.<ThisOrDescendantsVisibleAndMatchesSearch>g__SpecialFilterVisibleAndMatches|20_1(sf, node))
				{
					return true;
				}
			}
			foreach (ThingCategoryDef thingCategoryDef in node.catDef.childCategories)
			{
				if (this.ThisOrDescendantsVisibleAndMatchesSearch(thingCategoryDef.treeNode))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001F69 RID: 8041 RVA: 0x000C3DB0 File Offset: 0x000C1FB0
		private bool CategoryMatches(TreeNode_ThingCategory node)
		{
			return this.searchFilter.Matches(node.catDef.label);
		}

		// Token: 0x06001F6A RID: 8042 RVA: 0x000C3DC8 File Offset: 0x000C1FC8
		private bool Visible(TreeNode_ThingCategory node)
		{
			return node.catDef.DescendantThingDefs.Any(new Func<ThingDef, bool>(this.Visible));
		}

		// Token: 0x06001F6B RID: 8043 RVA: 0x000C3DE8 File Offset: 0x000C1FE8
		private bool Visible(SpecialThingFilterDef filter, TreeNode_ThingCategory node)
		{
			if (this.parentFilter != null && !this.parentFilter.Allows(filter))
			{
				return false;
			}
			if (this.hiddenSpecialFilters == null)
			{
				this.CalculateHiddenSpecialFilters(node);
			}
			for (int i = 0; i < this.hiddenSpecialFilters.Count; i++)
			{
				if (this.hiddenSpecialFilters[i] == filter)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06001F6C RID: 8044 RVA: 0x000C3E44 File Offset: 0x000C2044
		private bool CurrentRowVisibleOnScreen()
		{
			Rect other = new Rect(0f, this.curY, base.ColumnWidth, this.lineHeight);
			return this.visibleRect.Overlaps(other);
		}

		// Token: 0x06001F6D RID: 8045 RVA: 0x000C3E7B File Offset: 0x000C207B
		private void CalculateHiddenSpecialFilters(TreeNode_ThingCategory node)
		{
			this.hiddenSpecialFilters = Listing_TreeThingFilter.GetCachedHiddenSpecialFilters(node, this.parentFilter);
			if (this.tempForceHiddenSpecialFilters != null)
			{
				this.hiddenSpecialFilters = new List<SpecialThingFilterDef>(this.hiddenSpecialFilters);
				this.hiddenSpecialFilters.AddRange(this.tempForceHiddenSpecialFilters);
			}
		}

		// Token: 0x06001F6E RID: 8046 RVA: 0x000C3EBC File Offset: 0x000C20BC
		private static List<SpecialThingFilterDef> GetCachedHiddenSpecialFilters(TreeNode_ThingCategory node, ThingFilter parentFilter)
		{
			ValueTuple<TreeNode_ThingCategory, ThingFilter> key = new ValueTuple<TreeNode_ThingCategory, ThingFilter>(node, parentFilter);
			List<SpecialThingFilterDef> list;
			if (!Listing_TreeThingFilter.cachedHiddenSpecialFilters.TryGetValue(key, out list))
			{
				list = Listing_TreeThingFilter.CalculateHiddenSpecialFilters(node, parentFilter);
				Listing_TreeThingFilter.cachedHiddenSpecialFilters.Add(key, list);
			}
			return list;
		}

		// Token: 0x06001F6F RID: 8047 RVA: 0x000C3EF8 File Offset: 0x000C20F8
		private static List<SpecialThingFilterDef> CalculateHiddenSpecialFilters(TreeNode_ThingCategory node, ThingFilter parentFilter)
		{
			List<SpecialThingFilterDef> list = new List<SpecialThingFilterDef>();
			IEnumerable<SpecialThingFilterDef> enumerable = node.catDef.ParentsSpecialThingFilterDefs.Concat(node.catDef.DescendantSpecialThingFilterDefs);
			IEnumerable<ThingDef> enumerable2 = node.catDef.DescendantThingDefs;
			if (parentFilter != null)
			{
				enumerable2 = from x in enumerable2
				where parentFilter.Allows(x)
				select x;
			}
			foreach (SpecialThingFilterDef specialThingFilterDef in enumerable)
			{
				bool flag = false;
				foreach (ThingDef def in enumerable2)
				{
					if (specialThingFilterDef.Worker.CanEverMatch(def))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					list.Add(specialThingFilterDef);
				}
			}
			return list;
		}

		// Token: 0x06001F70 RID: 8048 RVA: 0x000C3FEC File Offset: 0x000C21EC
		public static void ResetStaticData()
		{
			Listing_TreeThingFilter.cachedHiddenSpecialFilters.Clear();
		}

		// Token: 0x06001F72 RID: 8050 RVA: 0x000C4013 File Offset: 0x000C2213
		[CompilerGenerated]
		private bool <DoCategoryChildren>g__HideCategoryDueToSearch|13_0(TreeNode_ThingCategory subCat, ref Listing_TreeThingFilter.<>c__DisplayClass13_0 A_2)
		{
			return !(!this.searchFilter.Active | A_2.subtreeMatchedSearch) && !this.CategoryMatches(subCat) && !this.ThisOrDescendantsVisibleAndMatchesSearch(subCat);
		}

		// Token: 0x06001F73 RID: 8051 RVA: 0x000C4045 File Offset: 0x000C2245
		[CompilerGenerated]
		private bool <DoCategoryChildren>g__HideThingDueToSearch|13_1(ThingDef tDef, ref Listing_TreeThingFilter.<>c__DisplayClass13_0 A_2)
		{
			return !(!this.searchFilter.Active | A_2.subtreeMatchedSearch) && !this.searchFilter.Matches(tDef);
		}

		// Token: 0x06001F74 RID: 8052 RVA: 0x000C406F File Offset: 0x000C226F
		[CompilerGenerated]
		private bool <ThisOrDescendantsVisibleAndMatchesSearch>g__ThingDefVisibleAndMatches|20_0(ThingDef td)
		{
			return this.Visible(td) && this.searchFilter.Matches(td);
		}

		// Token: 0x06001F75 RID: 8053 RVA: 0x000C4088 File Offset: 0x000C2288
		[CompilerGenerated]
		private bool <ThisOrDescendantsVisibleAndMatchesSearch>g__SpecialFilterVisibleAndMatches|20_1(SpecialThingFilterDef sf, TreeNode_ThingCategory subCat)
		{
			return this.Visible(sf, subCat) && this.searchFilter.Matches(sf);
		}

		// Token: 0x0400130B RID: 4875
		private static readonly Color NoMatchColor = Color.grey;

		// Token: 0x0400130C RID: 4876
		private static readonly LRUCache<ValueTuple<TreeNode_ThingCategory, ThingFilter>, List<SpecialThingFilterDef>> cachedHiddenSpecialFilters = new LRUCache<ValueTuple<TreeNode_ThingCategory, ThingFilter>, List<SpecialThingFilterDef>>(500);

		// Token: 0x0400130D RID: 4877
		private ThingFilter filter;

		// Token: 0x0400130E RID: 4878
		private ThingFilter parentFilter;

		// Token: 0x0400130F RID: 4879
		private List<SpecialThingFilterDef> hiddenSpecialFilters;

		// Token: 0x04001310 RID: 4880
		private List<ThingDef> forceHiddenDefs;

		// Token: 0x04001311 RID: 4881
		private List<SpecialThingFilterDef> tempForceHiddenSpecialFilters;

		// Token: 0x04001312 RID: 4882
		private List<ThingDef> suppressSmallVolumeTags;

		// Token: 0x04001313 RID: 4883
		protected QuickSearchFilter searchFilter;

		// Token: 0x04001314 RID: 4884
		public int matchCount;

		// Token: 0x04001315 RID: 4885
		private Rect visibleRect;
	}
}
