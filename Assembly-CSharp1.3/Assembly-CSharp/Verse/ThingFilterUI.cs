using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x020004F0 RID: 1264
	public static class ThingFilterUI
	{
		// Token: 0x06002633 RID: 9779 RVA: 0x000ED4FC File Offset: 0x000EB6FC
		public static void DoThingFilterConfigWindow(Rect rect, ThingFilterUI.UIState state, ThingFilter filter, ThingFilter parentFilter = null, int openMask = 1, IEnumerable<ThingDef> forceHiddenDefs = null, IEnumerable<SpecialThingFilterDef> forceHiddenFilters = null, bool forceHideHitPointsConfig = false, List<ThingDef> suppressSmallVolumeTags = null, Map map = null)
		{
			Widgets.DrawMenuSection(rect);
			Text.Font = GameFont.Tiny;
			float num = rect.width - 2f;
			Rect rect2 = new Rect(rect.x + 1f, rect.y + 1f, num / 2f, 24f);
			if (Widgets.ButtonText(rect2, "ClearAll".Translate(), true, true, true))
			{
				filter.SetDisallowAll(forceHiddenDefs, forceHiddenFilters);
				SoundDefOf.Checkbox_TurnedOff.PlayOneShotOnCamera(null);
			}
			if (Widgets.ButtonText(new Rect(rect2.xMax + 1f, rect2.y, rect.xMax - 1f - (rect2.xMax + 1f), 24f), "AllowAll".Translate(), true, true, true))
			{
				filter.SetAllowAll(parentFilter, false);
				SoundDefOf.Checkbox_TurnedOn.PlayOneShotOnCamera(null);
			}
			Text.Font = GameFont.Small;
			rect.yMin = rect2.yMax;
			int num2 = 1;
			Rect rect3 = new Rect(rect.x + 1f, rect.y + 1f + (float)num2, rect.width - 2f, 24f);
			state.quickSearch.OnGUI(rect3, null);
			rect.yMin = rect3.yMax;
			TreeNode_ThingCategory node = ThingCategoryNodeDatabase.RootNode;
			bool flag = true;
			bool flag2 = true;
			if (parentFilter != null)
			{
				node = parentFilter.DisplayRootCategory;
				flag = parentFilter.allowedHitPointsConfigurable;
				flag2 = parentFilter.allowedQualitiesConfigurable;
			}
			Rect viewRect = new Rect(0f, 0f, rect.width - 16f, ThingFilterUI.viewHeight);
			Rect visibleRect = new Rect(0f, 0f, rect.width, rect.height);
			visibleRect.position += state.scrollPosition;
			Widgets.BeginScrollView(rect, ref state.scrollPosition, viewRect, true);
			float num3 = 2f;
			if (flag && !forceHideHitPointsConfig)
			{
				ThingFilterUI.DrawHitPointsFilterConfig(ref num3, viewRect.width, filter);
			}
			if (flag2)
			{
				ThingFilterUI.DrawQualityFilterConfig(ref num3, viewRect.width, filter);
			}
			float num4 = num3;
			Rect rect4 = new Rect(0f, num3, viewRect.width, 9999f);
			visibleRect.position -= rect4.position;
			Listing_TreeThingFilter listing_TreeThingFilter = new Listing_TreeThingFilter(filter, parentFilter, forceHiddenDefs, forceHiddenFilters, suppressSmallVolumeTags, state.quickSearch.filter);
			listing_TreeThingFilter.Begin(rect4);
			listing_TreeThingFilter.ListCategoryChildren(node, openMask, map, visibleRect);
			listing_TreeThingFilter.End();
			state.quickSearch.noResultsMatched = (listing_TreeThingFilter.matchCount == 0);
			if (Event.current.type == EventType.Layout)
			{
				ThingFilterUI.viewHeight = num4 + listing_TreeThingFilter.CurHeight + 90f;
			}
			Widgets.EndScrollView();
		}

		// Token: 0x06002634 RID: 9780 RVA: 0x000ED7B8 File Offset: 0x000EB9B8
		private static void DrawHitPointsFilterConfig(ref float y, float width, ThingFilter filter)
		{
			Rect rect = new Rect(20f, y, width - 20f, 28f);
			FloatRange allowedHitPointsPercents = filter.AllowedHitPointsPercents;
			Widgets.FloatRange(rect, 1, ref allowedHitPointsPercents, 0f, 1f, "HitPoints", ToStringStyle.PercentZero);
			filter.AllowedHitPointsPercents = allowedHitPointsPercents;
			y += 28f;
			y += 5f;
			Text.Font = GameFont.Small;
		}

		// Token: 0x06002635 RID: 9781 RVA: 0x000ED820 File Offset: 0x000EBA20
		private static void DrawQualityFilterConfig(ref float y, float width, ThingFilter filter)
		{
			Rect rect = new Rect(20f, y, width - 20f, 28f);
			QualityRange allowedQualityLevels = filter.AllowedQualityLevels;
			Widgets.QualityRange(rect, 876813230, ref allowedQualityLevels);
			filter.AllowedQualityLevels = allowedQualityLevels;
			y += 28f;
			y += 5f;
			Text.Font = GameFont.Small;
		}

		// Token: 0x04001806 RID: 6150
		private static float viewHeight;

		// Token: 0x04001807 RID: 6151
		private const float ExtraViewHeight = 90f;

		// Token: 0x04001808 RID: 6152
		private const float RangeLabelTab = 10f;

		// Token: 0x04001809 RID: 6153
		private const float RangeLabelHeight = 19f;

		// Token: 0x0400180A RID: 6154
		private const float SliderHeight = 28f;

		// Token: 0x0400180B RID: 6155
		private const float SliderTab = 20f;

		// Token: 0x02001CDE RID: 7390
		public class UIState
		{
			// Token: 0x04006F5E RID: 28510
			public Vector2 scrollPosition;

			// Token: 0x04006F5F RID: 28511
			public QuickSearchWidget quickSearch = new QuickSearchWidget();
		}
	}
}
