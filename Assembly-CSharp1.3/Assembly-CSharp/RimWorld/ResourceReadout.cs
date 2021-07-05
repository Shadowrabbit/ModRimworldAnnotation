using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020013A9 RID: 5033
	public class ResourceReadout
	{
		// Token: 0x06007A84 RID: 31364 RVA: 0x002B38CB File Offset: 0x002B1ACB
		public ResourceReadout()
		{
			this.RootThingCategories = (from cat in DefDatabase<ThingCategoryDef>.AllDefs
			where cat.resourceReadoutRoot
			select cat).ToList<ThingCategoryDef>();
		}

		// Token: 0x06007A85 RID: 31365 RVA: 0x002B3908 File Offset: 0x002B1B08
		public void ResourceReadoutOnGUI()
		{
			if (Event.current.type == EventType.Layout)
			{
				return;
			}
			if (Current.ProgramState != ProgramState.Playing)
			{
				return;
			}
			if (Find.MainTabsRoot.OpenTab == MainButtonDefOf.Menu)
			{
				return;
			}
			GenUI.DrawTextWinterShadow(new Rect(256f, 512f, -256f, -512f));
			Text.Font = GameFont.Small;
			Rect rect = Prefs.ResourceReadoutCategorized ? new Rect(2f, 7f, 124f, (float)(UI.screenHeight - 7) - 200f) : new Rect(7f, 7f, 110f, (float)(UI.screenHeight - 7) - 200f);
			Rect rect2 = new Rect(0f, 0f, rect.width, this.lastDrawnHeight);
			bool flag = rect2.height > rect.height;
			if (flag)
			{
				Widgets.BeginScrollView(rect, ref this.scrollPosition, rect2, false);
			}
			else
			{
				this.scrollPosition = Vector2.zero;
				GUI.BeginGroup(rect);
			}
			if (!Prefs.ResourceReadoutCategorized)
			{
				this.DoReadoutSimple(rect2, rect.height);
			}
			else
			{
				this.DoReadoutCategorized(rect2);
			}
			if (flag)
			{
				Widgets.EndScrollView();
				return;
			}
			GUI.EndGroup();
		}

		// Token: 0x06007A86 RID: 31366 RVA: 0x002B3A30 File Offset: 0x002B1C30
		private void DoReadoutCategorized(Rect rect)
		{
			Listing_ResourceReadout listing_ResourceReadout = new Listing_ResourceReadout(Find.CurrentMap);
			listing_ResourceReadout.Begin(rect);
			listing_ResourceReadout.nestIndentWidth = 7f;
			listing_ResourceReadout.lineHeight = 24f;
			listing_ResourceReadout.verticalSpacing = 0f;
			for (int i = 0; i < this.RootThingCategories.Count; i++)
			{
				listing_ResourceReadout.DoCategory(this.RootThingCategories[i].treeNode, 0, 32);
			}
			listing_ResourceReadout.End();
			this.lastDrawnHeight = listing_ResourceReadout.CurHeight;
		}

		// Token: 0x06007A87 RID: 31367 RVA: 0x002B3AB4 File Offset: 0x002B1CB4
		private void DoReadoutSimple(Rect rect, float outRectHeight)
		{
			GUI.BeginGroup(rect);
			Text.Anchor = TextAnchor.MiddleLeft;
			float num = 0f;
			foreach (KeyValuePair<ThingDef, int> keyValuePair in Find.CurrentMap.resourceCounter.AllCountedAmounts)
			{
				if (keyValuePair.Value > 0 || keyValuePair.Key.resourceReadoutAlwaysShow)
				{
					Rect rect2 = new Rect(0f, num, 999f, 24f);
					if (rect2.yMax >= this.scrollPosition.y && rect2.y <= this.scrollPosition.y + outRectHeight)
					{
						this.DrawResourceSimple(rect2, keyValuePair.Key);
					}
					num += 24f;
				}
			}
			Text.Anchor = TextAnchor.UpperLeft;
			this.lastDrawnHeight = num;
			GUI.EndGroup();
		}

		// Token: 0x06007A88 RID: 31368 RVA: 0x002B3B9C File Offset: 0x002B1D9C
		public void DrawResourceSimple(Rect rect, ThingDef thingDef)
		{
			this.DrawIcon(rect.x, rect.y, thingDef);
			rect.y += 2f;
			int count = Find.CurrentMap.resourceCounter.GetCount(thingDef);
			Widgets.Label(new Rect(34f, rect.y, rect.width - 34f, rect.height), count.ToStringCached());
		}

		// Token: 0x06007A89 RID: 31369 RVA: 0x002B3C14 File Offset: 0x002B1E14
		private void DrawIcon(float x, float y, ThingDef thingDef)
		{
			Rect rect = new Rect(x, y, 27f, 27f);
			Color color = GUI.color;
			Widgets.ThingIcon(rect, thingDef, null, null, 1f, null);
			GUI.color = color;
			if (Mouse.IsOver(rect))
			{
				TaggedString str = thingDef.LabelCap + ": " + thingDef.description.CapitalizeFirst();
				TooltipHandler.TipRegion(rect, str);
			}
		}

		// Token: 0x040043C0 RID: 17344
		private Vector2 scrollPosition;

		// Token: 0x040043C1 RID: 17345
		private float lastDrawnHeight;

		// Token: 0x040043C2 RID: 17346
		private readonly List<ThingCategoryDef> RootThingCategories;

		// Token: 0x040043C3 RID: 17347
		private const float LineHeightSimple = 24f;

		// Token: 0x040043C4 RID: 17348
		private const float LineHeightCategorized = 24f;

		// Token: 0x040043C5 RID: 17349
		private const float DistFromScreenBottom = 200f;
	}
}
