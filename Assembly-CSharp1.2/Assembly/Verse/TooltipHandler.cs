using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200075E RID: 1886
	public static class TooltipHandler
	{
		// Token: 0x06002F8F RID: 12175 RVA: 0x0013B6DC File Offset: 0x001398DC
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

		// Token: 0x06002F90 RID: 12176 RVA: 0x0002556C File Offset: 0x0002376C
		public static void TipRegion(Rect rect, Func<string> textGetter, int uniqueId)
		{
			TooltipHandler.TipRegion(rect, new TipSignal(textGetter, uniqueId));
		}

		// Token: 0x06002F91 RID: 12177 RVA: 0x0002557B File Offset: 0x0002377B
		public static void TipRegionByKey(Rect rect, string key)
		{
			if (!Mouse.IsOver(rect) && !DebugViewSettings.drawTooltipEdges)
			{
				return;
			}
			TooltipHandler.TipRegion(rect, key.Translate());
		}

		// Token: 0x06002F92 RID: 12178 RVA: 0x0002559E File Offset: 0x0002379E
		public static void TipRegionByKey(Rect rect, string key, NamedArgument arg1)
		{
			if (!Mouse.IsOver(rect) && !DebugViewSettings.drawTooltipEdges)
			{
				return;
			}
			TooltipHandler.TipRegion(rect, key.Translate(arg1));
		}

		// Token: 0x06002F93 RID: 12179 RVA: 0x000255C2 File Offset: 0x000237C2
		public static void TipRegionByKey(Rect rect, string key, NamedArgument arg1, NamedArgument arg2)
		{
			if (!Mouse.IsOver(rect) && !DebugViewSettings.drawTooltipEdges)
			{
				return;
			}
			TooltipHandler.TipRegion(rect, key.Translate(arg1, arg2));
		}

		// Token: 0x06002F94 RID: 12180 RVA: 0x000255E7 File Offset: 0x000237E7
		public static void TipRegionByKey(Rect rect, string key, NamedArgument arg1, NamedArgument arg2, NamedArgument arg3)
		{
			if (!Mouse.IsOver(rect) && !DebugViewSettings.drawTooltipEdges)
			{
				return;
			}
			TooltipHandler.TipRegion(rect, key.Translate(arg1, arg2, arg3));
		}

		// Token: 0x06002F95 RID: 12181 RVA: 0x0013B79C File Offset: 0x0013999C
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
				Widgets.DrawBox(rect, 1);
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

		// Token: 0x06002F96 RID: 12182 RVA: 0x0002560E File Offset: 0x0002380E
		public static void DoTooltipGUI()
		{
			TooltipHandler.DrawActiveTips();
			if (Event.current.type == EventType.Repaint)
			{
				TooltipHandler.CleanActiveTooltips();
				TooltipHandler.frame++;
			}
		}

		// Token: 0x06002F97 RID: 12183 RVA: 0x0013B88C File Offset: 0x00139A8C
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

		// Token: 0x06002F98 RID: 12184 RVA: 0x0013B98C File Offset: 0x00139B8C
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

		// Token: 0x06002F99 RID: 12185 RVA: 0x0013BA30 File Offset: 0x00139C30
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

		// Token: 0x06002F9A RID: 12186 RVA: 0x00025633 File Offset: 0x00023833
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

		// Token: 0x0400203F RID: 8255
		private static Dictionary<int, ActiveTip> activeTips = new Dictionary<int, ActiveTip>();

		// Token: 0x04002040 RID: 8256
		private static int frame = 0;

		// Token: 0x04002041 RID: 8257
		private static List<int> dyingTips = new List<int>(32);

		// Token: 0x04002042 RID: 8258
		private const float SpaceBetweenTooltips = 2f;

		// Token: 0x04002043 RID: 8259
		private static List<ActiveTip> drawingTips = new List<ActiveTip>();

		// Token: 0x04002044 RID: 8260
		private static Comparison<ActiveTip> compareTooltipsByPriorityCached = new Comparison<ActiveTip>(TooltipHandler.CompareTooltipsByPriority);
	}
}
