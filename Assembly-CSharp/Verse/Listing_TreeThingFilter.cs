using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000747 RID: 1863
	public class Listing_TreeThingFilter : Listing_Tree
	{
		// Token: 0x06002EF0 RID: 12016 RVA: 0x00024D72 File Offset: 0x00022F72
		public Listing_TreeThingFilter(ThingFilter filter, ThingFilter parentFilter, IEnumerable<ThingDef> forceHiddenDefs, IEnumerable<SpecialThingFilterDef> forceHiddenFilters, List<ThingDef> suppressSmallVolumeTags)
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
		}

		// Token: 0x06002EF1 RID: 12017 RVA: 0x00139718 File Offset: 0x00137918
		public void DoCategoryChildren(TreeNode_ThingCategory node, int indentLevel, int openMask, Map map, bool isRoot = false)
		{
			if (isRoot)
			{
				foreach (SpecialThingFilterDef sfDef in node.catDef.ParentsSpecialThingFilterDefs)
				{
					if (this.Visible_NewTemp(sfDef, node))
					{
						this.DoSpecialFilter(sfDef, indentLevel);
					}
				}
			}
			List<SpecialThingFilterDef> childSpecialFilters = node.catDef.childSpecialFilters;
			for (int i = 0; i < childSpecialFilters.Count; i++)
			{
				if (this.Visible_NewTemp(childSpecialFilters[i], node))
				{
					this.DoSpecialFilter(childSpecialFilters[i], indentLevel);
				}
			}
			foreach (TreeNode_ThingCategory node2 in node.ChildCategoryNodes)
			{
				if (this.Visible(node2))
				{
					this.DoCategory(node2, indentLevel, openMask, map);
				}
			}
			foreach (ThingDef thingDef in from n in node.catDef.childThingDefs
			orderby n.label
			select n)
			{
				if (this.Visible(thingDef))
				{
					this.DoThingDef(thingDef, indentLevel, map);
				}
			}
		}

		// Token: 0x06002EF2 RID: 12018 RVA: 0x00139880 File Offset: 0x00137A80
		private void DoSpecialFilter(SpecialThingFilterDef sfDef, int nestLevel)
		{
			if (!sfDef.configurable)
			{
				return;
			}
			base.LabelLeft("*" + sfDef.LabelCap, sfDef.description, nestLevel, 0f);
			bool flag = this.filter.Allows(sfDef);
			bool flag2 = flag;
			Widgets.Checkbox(new Vector2(this.LabelWidth, this.curY), ref flag, this.lineHeight, false, true, null, null);
			if (flag != flag2)
			{
				this.filter.SetAllow(sfDef, flag);
			}
			base.EndLine();
		}

		// Token: 0x06002EF3 RID: 12019 RVA: 0x00139908 File Offset: 0x00137B08
		public void DoCategory(TreeNode_ThingCategory node, int indentLevel, int openMask, Map map)
		{
			base.OpenCloseWidget(node, indentLevel, openMask);
			base.LabelLeft(node.LabelCap, node.catDef.description, indentLevel, 0f);
			MultiCheckboxState multiCheckboxState = this.AllowanceStateOf(node);
			MultiCheckboxState multiCheckboxState2 = Widgets.CheckboxMulti(new Rect(this.LabelWidth, this.curY, this.lineHeight, this.lineHeight), multiCheckboxState, true);
			if (multiCheckboxState != multiCheckboxState2)
			{
				this.filter.SetAllow(node.catDef, multiCheckboxState2 == MultiCheckboxState.On, this.forceHiddenDefs, this.hiddenSpecialFilters);
			}
			base.EndLine();
			if (node.IsOpen(openMask))
			{
				this.DoCategoryChildren(node, indentLevel + 1, openMask, map, false);
			}
		}

		// Token: 0x06002EF4 RID: 12020 RVA: 0x001399AC File Offset: 0x00137BAC
		private void DoThingDef(ThingDef tDef, int nestLevel, Map map)
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
			base.LabelLeft(tDef.LabelCap, text, nestLevel, num);
			bool flag = this.filter.Allows(tDef);
			bool flag2 = flag;
			Widgets.Checkbox(new Vector2(this.LabelWidth, this.curY), ref flag, this.lineHeight, false, true, null, null);
			if (flag != flag2)
			{
				this.filter.SetAllow(tDef, flag);
			}
			base.EndLine();
		}

		// Token: 0x06002EF5 RID: 12021 RVA: 0x00139B74 File Offset: 0x00137D74
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
				if (this.Visible_NewTemp(sf, cat) && !this.filter.Allows(sf))
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

		// Token: 0x06002EF6 RID: 12022 RVA: 0x00139C54 File Offset: 0x00137E54
		private bool Visible(ThingDef td)
		{
			if (td.menuHidden)
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

		// Token: 0x06002EF7 RID: 12023 RVA: 0x00024DB0 File Offset: 0x00022FB0
		private bool Visible(TreeNode_ThingCategory node)
		{
			return node.catDef.DescendantThingDefs.Any(new Func<ThingDef, bool>(this.Visible));
		}

		// Token: 0x06002EF8 RID: 12024 RVA: 0x00024DCE File Offset: 0x00022FCE
		[Obsolete("Obsolete, only used to avoid errors when patching")]
		private bool Visible(SpecialThingFilterDef filter)
		{
			return this.Visible_NewTemp(filter, new TreeNode_ThingCategory(ThingCategoryDefOf.Root));
		}

		// Token: 0x06002EF9 RID: 12025 RVA: 0x00139CAC File Offset: 0x00137EAC
		private bool Visible_NewTemp(SpecialThingFilterDef filter, TreeNode_ThingCategory node)
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

		// Token: 0x06002EFA RID: 12026 RVA: 0x00139D08 File Offset: 0x00137F08
		private void CalculateHiddenSpecialFilters(TreeNode_ThingCategory node)
		{
			this.hiddenSpecialFilters = new List<SpecialThingFilterDef>();
			if (this.tempForceHiddenSpecialFilters != null)
			{
				this.hiddenSpecialFilters.AddRange(this.tempForceHiddenSpecialFilters);
			}
			IEnumerable<SpecialThingFilterDef> enumerable = node.catDef.ParentsSpecialThingFilterDefs.Concat(node.catDef.DescendantSpecialThingFilterDefs);
			IEnumerable<ThingDef> enumerable2 = node.catDef.DescendantThingDefs;
			if (this.parentFilter != null)
			{
				enumerable2 = from x in enumerable2
				where this.parentFilter.Allows(x)
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
					this.hiddenSpecialFilters.Add(specialThingFilterDef);
				}
			}
		}

		// Token: 0x04001FDC RID: 8156
		private ThingFilter filter;

		// Token: 0x04001FDD RID: 8157
		private ThingFilter parentFilter;

		// Token: 0x04001FDE RID: 8158
		private List<SpecialThingFilterDef> hiddenSpecialFilters;

		// Token: 0x04001FDF RID: 8159
		private List<ThingDef> forceHiddenDefs;

		// Token: 0x04001FE0 RID: 8160
		private List<SpecialThingFilterDef> tempForceHiddenSpecialFilters;

		// Token: 0x04001FE1 RID: 8161
		private List<ThingDef> suppressSmallVolumeTags;
	}
}
