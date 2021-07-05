using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000427 RID: 1063
	public static class TooltipHandler
	{
		// Token: 0x06001FF6 RID: 8182 RVA: 0x000C5D5C File Offset: 0x000C3F5C
		public static void ClearTooltipsFrom(Rect rect)
		{
			if (Event.current.type != EventType.Repaint)
			{
				return;
			}
			if (Mouse.IsOver(rect))
			{
				TooltipHandler.dyingTips.Clear();
				foreach (KeyValuePair<int, ActiveTip> keyValuePair in TooltipHandler.activeTips)
				{
					if (keyValuePair.Value.lastTriggerFrame == TooltipHandler.frame)
					{
						TooltipHandler.dyingTips.Add(keyValuePair.Key);
					}
				}
				for (int i = 0; i < TooltipHandler.dyingTips.Count; i++)
				{
					TooltipHandler.activeTips.Remove(TooltipHandler.dyingTips[i]);
				}
			}
		}

		// Token: 0x06001FF7 RID: 8183 RVA: 0x000C5E1C File Offset: 0x000C401C
		public static void TipRegion(Rect rect, Func<string> textGetter, int uniqueId)
		{
			TooltipHandler.TipRegion(rect, new TipSignal(textGetter, uniqueId));
		}

		// Token: 0x06001FF8 RID: 8184 RVA: 0x000C5E2B File Offset: 0x000C402B
		public static void TipRegionByKey(Rect rect, string key)
		{
			if (!Mouse.IsOver(rect) && !DebugViewSettings.drawTooltipEdges)
			{
				return;
			}
			TooltipHandler.TipRegion(rect, key.Translate());
		}

		// Token: 0x06001FF9 RID: 8185 RVA: 0x000C5E4E File Offset: 0x000C404E
		public static void TipRegionByKey(Rect rect, string key, NamedArgument arg1)
		{
			if (!Mouse.IsOver(rect) && !DebugViewSettings.drawTooltipEdges)
			{
				return;
			}
			TooltipHandler.TipRegion(rect, key.Translate(arg1));
		}

		// Token: 0x06001FFA RID: 8186 RVA: 0x000C5E72 File Offset: 0x000C4072
		public static void TipRegionByKey(Rect rect, string key, NamedArgument arg1, NamedArgument arg2)
		{
			if (!Mouse.IsOver(rect) && !DebugViewSettings.drawTooltipEdges)
			{
				return;
			}
			TooltipHandler.TipRegion(rect, key.Translate(arg1, arg2));
		}

		// Token: 0x06001FFB RID: 8187 RVA: 0x000C5E97 File Offset: 0x000C4097
		public static void TipRegionByKey(Rect rect, string key, NamedArgument arg1, NamedArgument arg2, NamedArgument arg3)
		{
			if (!Mouse.IsOver(rect) && !DebugViewSettings.drawTooltipEdges)
			{
				return;
			}
			TooltipHandler.TipRegion(rect, key.Translate(arg1, arg2, arg3));
		}

		// Token: 0x06001FFC RID: 8188 RVA: 0x000C5EC0 File Offset: 0x000C40C0
		public static void TipRegion(Rect rect, TipSignal tip)
		{
			if (Event.current.type != EventType.Repaint)
			{
				return;
			}
			if (tip.textGetter == null && tip.text.NullOrEmpty())
			{
				return;
			}
			if (!Mouse.IsOver(rect) && !DebugViewSettings.drawTooltipEdges)
			{
				return;
			}
			if (DebugViewSettings.drawTooltipEdges)
			{
				Widgets.DrawBox(rect, 1, null);
			}
			if (!TooltipHandler.activeTips.ContainsKey(tip.uniqueId))
			{
				ActiveTip value = new ActiveTip(tip);
				TooltipHandler.activeTips.Add(tip.uniqueId, value);
				TooltipHandler.activeTips[tip.uniqueId].firstTriggerTime = (double)Time.realtimeSinceStartup;
			}
			TooltipHandler.activeTips[tip.uniqueId].lastTriggerFrame = TooltipHandler.frame;
			TooltipHandler.activeTips[tip.uniqueId].signal.text = tip.text;
			TooltipHandler.activeTips[tip.uniqueId].signal.textGetter = tip.textGetter;
		}

		// Token: 0x06001FFD RID: 8189 RVA: 0x000C5FAF File Offset: 0x000C41AF
		public static void DoTooltipGUI()
		{
			TooltipHandler.DrawActiveTips();
			if (Event.current.type == EventType.Repaint)
			{
				TooltipHandler.CleanActiveTooltips();
				TooltipHandler.frame++;
			}
		}

		// Token: 0x06001FFE RID: 8190 RVA: 0x000C5FD4 File Offset: 0x000C41D4
		private static void DrawActiveTips()
		{
			if (TooltipHandler.activeTips.Count == 0)
			{
				return;
			}
			TooltipHandler.drawingTips.Clear();
			foreach (ActiveTip activeTip in TooltipHandler.activeTips.Values)
			{
				if ((double)Time.realtimeSinceStartup > activeTip.firstTriggerTime + (double)activeTip.signal.delay)
				{
					TooltipHandler.drawingTips.Add(activeTip);
				}
			}
			if (TooltipHandler.drawingTips.Any<ActiveTip>())
			{
				TooltipHandler.drawingTips.Sort(TooltipHandler.compareTooltipsByPriorityCached);
				Vector2 pos = TooltipHandler.CalculateInitialTipPosition(TooltipHandler.drawingTips);
				for (int i = 0; i < TooltipHandler.drawingTips.Count; i++)
				{
					pos.y += TooltipHandler.drawingTips[i].DrawTooltip(pos);
					pos.y += 2f;
				}
				TooltipHandler.drawingTips.Clear();
			}
		}

		// Token: 0x06001FFF RID: 8191 RVA: 0x000C60D4 File Offset: 0x000C42D4
		private static void CleanActiveTooltips()
		{
			TooltipHandler.dyingTips.Clear();
			foreach (KeyValuePair<int, ActiveTip> keyValuePair in TooltipHandler.activeTips)
			{
				if (keyValuePair.Value.lastTriggerFrame != TooltipHandler.frame)
				{
					TooltipHandler.dyingTips.Add(keyValuePair.Key);
				}
			}
			for (int i = 0; i < TooltipHandler.dyingTips.Count; i++)
			{
				TooltipHandler.activeTips.Remove(TooltipHandler.dyingTips[i]);
			}
		}

		// Token: 0x06002000 RID: 8192 RVA: 0x000C6178 File Offset: 0x000C4378
		private static Vector2 CalculateInitialTipPosition(List<ActiveTip> drawingTips)
		{
			float num = 0f;
			float num2 = 0f;
			for (int i = 0; i < drawingTips.Count; i++)
			{
				Rect tipRect = drawingTips[i].TipRect;
				num += tipRect.height;
				num2 = Mathf.Max(num2, tipRect.width);
				if (i != drawingTips.Count - 1)
				{
					num += 2f;
				}
			}
			return GenUI.GetMouseAttachedWindowPos(num2, num);
		}

		// Token: 0x06002001 RID: 8193 RVA: 0x000C61E1 File Offset: 0x000C43E1
		private static int CompareTooltipsByPriority(ActiveTip A, ActiveTip B)
		{
			if (A.signal.priority == B.signal.priority)
			{
				return 0;
			}
			if (A.signal.priority == TooltipPriority.Pawn)
			{
				return -1;
			}
			if (B.signal.priority == TooltipPriority.Pawn)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x04001362 RID: 4962
		private static Dictionary<int, ActiveTip> activeTips = new Dictionary<int, ActiveTip>();

		// Token: 0x04001363 RID: 4963
		private static int frame = 0;

		// Token: 0x04001364 RID: 4964
		private static List<int> dyingTips = new List<int>(32);

		// Token: 0x04001365 RID: 4965
		private const float SpaceBetweenTooltips = 2f;

		// Token: 0x04001366 RID: 4966
		private static List<ActiveTip> drawingTips = new List<ActiveTip>();

		// Token: 0x04001367 RID: 4967
		private static Comparison<ActiveTip> compareTooltipsByPriorityCached = new Comparison<ActiveTip>(TooltipHandler.CompareTooltipsByPriority);
	}
}
