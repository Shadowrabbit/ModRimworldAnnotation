using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x0200043C RID: 1084
	public static class TabDrawer
	{
		// Token: 0x06002082 RID: 8322 RVA: 0x000C9AAC File Offset: 0x000C7CAC
		public static TabRecord DrawTabs(Rect baseRect, List<TabRecord> tabs, int rows)
		{
			if (rows <= 1)
			{
				return TabDrawer.DrawTabs<TabRecord>(baseRect, tabs, 200f);
			}
			int num = Mathf.FloorToInt((float)(tabs.Count / rows));
			int num2 = 0;
			TabRecord result = null;
			Rect rect = baseRect;
			baseRect.yMin -= (float)(rows - 1) * 31f;
			Rect rect2 = baseRect;
			rect2.yMax = rect.y;
			Widgets.DrawMenuSection(rect2);
			for (int i = 0; i < rows; i++)
			{
				int num3 = (i == 0) ? (tabs.Count - (rows - 1) * num) : num;
				TabDrawer.tmpTabs.Clear();
				for (int j = num2; j < num2 + num3; j++)
				{
					TabDrawer.tmpTabs.Add(tabs[j]);
				}
				TabRecord tabRecord = TabDrawer.DrawTabs<TabRecord>(baseRect, TabDrawer.tmpTabs, baseRect.width);
				if (tabRecord != null)
				{
					result = tabRecord;
				}
				baseRect.yMin += 31f;
				num2 += num3;
			}
			TabDrawer.tmpTabs.Clear();
			return result;
		}

		// Token: 0x06002083 RID: 8323 RVA: 0x000C9BA4 File Offset: 0x000C7DA4
		public static TTabRecord DrawTabs<TTabRecord>(Rect baseRect, List<TTabRecord> tabs, float maxTabWidth = 200f) where TTabRecord : TabRecord
		{
			TTabRecord ttabRecord = default(TTabRecord);
			TTabRecord ttabRecord2 = tabs.Find((TTabRecord t) => t.Selected);
			float num = baseRect.width + (float)(tabs.Count - 1) * 10f;
			float tabWidth = num / (float)tabs.Count;
			if (tabWidth > maxTabWidth)
			{
				tabWidth = maxTabWidth;
			}
			Rect position = new Rect(baseRect);
			position.y -= 32f;
			position.height = 9999f;
			GUI.BeginGroup(position);
			Text.Anchor = TextAnchor.MiddleCenter;
			Text.Font = GameFont.Small;
			Func<TTabRecord, Rect> func = (TTabRecord tab) => new Rect((float)tabs.IndexOf(tab) * (tabWidth - 10f), 1f, tabWidth, 32f);
			List<TTabRecord> list = tabs.ListFullCopy<TTabRecord>();
			if (ttabRecord2 != null)
			{
				list.Remove(ttabRecord2);
				list.Add(ttabRecord2);
			}
			TabRecord tabRecord = null;
			List<TTabRecord> list2 = list.ListFullCopy<TTabRecord>();
			list2.Reverse();
			for (int i = 0; i < list2.Count; i++)
			{
				TTabRecord ttabRecord3 = list2[i];
				Rect rect = func(ttabRecord3);
				if (tabRecord == null && Mouse.IsOver(rect))
				{
					tabRecord = ttabRecord3;
				}
				MouseoverSounds.DoRegion(rect, SoundDefOf.Mouseover_Tab);
				if (Widgets.ButtonInvisible(rect, true))
				{
					ttabRecord = ttabRecord3;
				}
			}
			foreach (TTabRecord ttabRecord4 in list)
			{
				Rect rect2 = func(ttabRecord4);
				ttabRecord4.Draw(rect2);
			}
			Text.Anchor = TextAnchor.UpperLeft;
			GUI.EndGroup();
			if (ttabRecord != null && ttabRecord != ttabRecord2)
			{
				SoundDefOf.RowTabSelect.PlayOneShotOnCamera(null);
				if (ttabRecord.clickedAction != null)
				{
					ttabRecord.clickedAction();
				}
			}
			return ttabRecord;
		}

		// Token: 0x040013C9 RID: 5065
		private const float MaxTabWidth = 200f;

		// Token: 0x040013CA RID: 5066
		public const float TabHeight = 32f;

		// Token: 0x040013CB RID: 5067
		public const float TabHoriztonalOverlap = 10f;

		// Token: 0x040013CC RID: 5068
		private static List<TabRecord> tmpTabs = new List<TabRecord>();
	}
}
