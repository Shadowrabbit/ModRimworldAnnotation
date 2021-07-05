using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000439 RID: 1081
	public static class ReorderableWidget
	{
		// Token: 0x06002072 RID: 8306 RVA: 0x000C900C File Offset: 0x000C720C
		public static void ReorderableWidgetOnGUI_BeforeWindowStack()
		{
			if (ReorderableWidget.dragBegun && ReorderableWidget.draggingReorderable >= 0 && ReorderableWidget.draggingReorderable < ReorderableWidget.reorderables.Count)
			{
				int groupID = ReorderableWidget.reorderables[ReorderableWidget.draggingReorderable].groupID;
				if (groupID >= 0 && groupID < ReorderableWidget.groups.Count && ReorderableWidget.groups[groupID].extraDraggedItemOnGUI != null)
				{
					ReorderableWidget.groups[groupID].extraDraggedItemOnGUI(ReorderableWidget.GetIndexWithinGroup(ReorderableWidget.draggingReorderable), ReorderableWidget.dragStartPos);
				}
			}
		}

		// Token: 0x06002073 RID: 8307 RVA: 0x000C9098 File Offset: 0x000C7298
		public static void ReorderableWidgetOnGUI_AfterWindowStack()
		{
			if (Event.current.rawType == EventType.MouseUp)
			{
				ReorderableWidget.released = true;
			}
			if (Event.current.type == EventType.Repaint)
			{
				if (ReorderableWidget.clicked)
				{
					ReorderableWidget.StopDragging();
					for (int i = 0; i < ReorderableWidget.reorderables.Count; i++)
					{
						if (ReorderableWidget.reorderables[i].rect == ReorderableWidget.clickedInRect)
						{
							ReorderableWidget.draggingReorderable = i;
							ReorderableWidget.dragStartPos = Event.current.mousePosition;
							break;
						}
					}
					ReorderableWidget.clicked = false;
				}
				if (ReorderableWidget.draggingReorderable >= ReorderableWidget.reorderables.Count)
				{
					ReorderableWidget.StopDragging();
				}
				if (ReorderableWidget.reorderables.Count != ReorderableWidget.lastFrameReorderableCount)
				{
					ReorderableWidget.StopDragging();
				}
				ReorderableWidget.lastInsertNear = ReorderableWidget.CurrentInsertNear(out ReorderableWidget.lastInsertNearLeft);
				if (ReorderableWidget.released)
				{
					ReorderableWidget.released = false;
					if (ReorderableWidget.dragBegun && ReorderableWidget.draggingReorderable >= 0)
					{
						int indexWithinGroup = ReorderableWidget.GetIndexWithinGroup(ReorderableWidget.draggingReorderable);
						int num;
						if (ReorderableWidget.lastInsertNear == ReorderableWidget.draggingReorderable)
						{
							num = indexWithinGroup;
						}
						else if (ReorderableWidget.lastInsertNearLeft)
						{
							num = ReorderableWidget.GetIndexWithinGroup(ReorderableWidget.lastInsertNear);
						}
						else
						{
							num = ReorderableWidget.GetIndexWithinGroup(ReorderableWidget.lastInsertNear) + 1;
						}
						if (num >= 0 && num != indexWithinGroup && num != indexWithinGroup + 1)
						{
							SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
							try
							{
								ReorderableWidget.groups[ReorderableWidget.reorderables[ReorderableWidget.draggingReorderable].groupID].reorderedAction(indexWithinGroup, num);
							}
							catch (Exception ex)
							{
								Log.Error(string.Concat(new object[]
								{
									"Could not reorder elements (from ",
									indexWithinGroup,
									" to ",
									num,
									"): ",
									ex
								}));
							}
						}
					}
					ReorderableWidget.StopDragging();
				}
				ReorderableWidget.lastFrameReorderableCount = ReorderableWidget.reorderables.Count;
				ReorderableWidget.groups.Clear();
				ReorderableWidget.reorderables.Clear();
			}
		}

		// Token: 0x06002074 RID: 8308 RVA: 0x000C9280 File Offset: 0x000C7480
		public static int NewGroup(Action<int, int> reorderedAction, ReorderableDirection direction, float drawLineExactlyBetween_space = -1f, Action<int, Vector2> extraDraggedItemOnGUI = null)
		{
			if (Event.current.type != EventType.Repaint)
			{
				return -1;
			}
			ReorderableWidget.ReorderableGroup item = default(ReorderableWidget.ReorderableGroup);
			item.reorderedAction = reorderedAction;
			item.direction = direction;
			item.drawLineExactlyBetween_space = drawLineExactlyBetween_space;
			item.extraDraggedItemOnGUI = extraDraggedItemOnGUI;
			ReorderableWidget.groups.Add(item);
			return ReorderableWidget.groups.Count - 1;
		}

		// Token: 0x06002075 RID: 8309 RVA: 0x000C92DC File Offset: 0x000C74DC
		public static bool Reorderable(int groupID, Rect rect, bool useRightButton = false)
		{
			if (Event.current.type == EventType.Repaint)
			{
				ReorderableWidget.ReorderableInstance item = default(ReorderableWidget.ReorderableInstance);
				item.groupID = groupID;
				item.rect = rect;
				item.absRect = new Rect(UI.GUIToScreenPoint(rect.position), rect.size);
				ReorderableWidget.reorderables.Add(item);
				int num = ReorderableWidget.reorderables.Count - 1;
				if (ReorderableWidget.draggingReorderable != -1 && (ReorderableWidget.dragBegun || Vector2.Distance(ReorderableWidget.clickedAt, Event.current.mousePosition) > 5f))
				{
					if (!ReorderableWidget.dragBegun)
					{
						SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
						ReorderableWidget.dragBegun = true;
					}
					if (ReorderableWidget.draggingReorderable == num)
					{
						GUI.color = ReorderableWidget.HighlightColor;
						Widgets.DrawHighlight(rect);
						GUI.color = Color.white;
					}
					if (ReorderableWidget.lastInsertNear == num && groupID >= 0 && groupID < ReorderableWidget.groups.Count)
					{
						Rect rect2 = ReorderableWidget.reorderables[ReorderableWidget.lastInsertNear].rect;
						ReorderableWidget.ReorderableGroup reorderableGroup = ReorderableWidget.groups[groupID];
						if (reorderableGroup.DrawLineExactlyBetween)
						{
							if (reorderableGroup.direction == ReorderableDirection.Horizontal)
							{
								rect2.xMin -= reorderableGroup.drawLineExactlyBetween_space / 2f;
								rect2.xMax += reorderableGroup.drawLineExactlyBetween_space / 2f;
							}
							else
							{
								rect2.yMin -= reorderableGroup.drawLineExactlyBetween_space / 2f;
								rect2.yMax += reorderableGroup.drawLineExactlyBetween_space / 2f;
							}
						}
						GUI.color = ReorderableWidget.LineColor;
						if (reorderableGroup.direction == ReorderableDirection.Horizontal)
						{
							if (ReorderableWidget.lastInsertNearLeft)
							{
								Widgets.DrawLine(rect2.position, new Vector2(rect2.x, rect2.yMax), ReorderableWidget.LineColor, 2f);
							}
							else
							{
								Widgets.DrawLine(new Vector2(rect2.xMax, rect2.y), new Vector2(rect2.xMax, rect2.yMax), ReorderableWidget.LineColor, 2f);
							}
						}
						else if (ReorderableWidget.lastInsertNearLeft)
						{
							Widgets.DrawLine(rect2.position, new Vector2(rect2.xMax, rect2.y), ReorderableWidget.LineColor, 2f);
						}
						else
						{
							Widgets.DrawLine(new Vector2(rect2.x, rect2.yMax), new Vector2(rect2.xMax, rect2.yMax), ReorderableWidget.LineColor, 2f);
						}
						GUI.color = Color.white;
					}
				}
				return ReorderableWidget.draggingReorderable == num && ReorderableWidget.dragBegun;
			}
			if (Event.current.rawType == EventType.MouseUp)
			{
				ReorderableWidget.released = true;
			}
			if (Event.current.type == EventType.MouseDown && ((useRightButton && Event.current.button == 1) || (!useRightButton && Event.current.button == 0)) && Mouse.IsOver(rect))
			{
				ReorderableWidget.clicked = true;
				ReorderableWidget.clickedAt = Event.current.mousePosition;
				ReorderableWidget.clickedInRect = rect;
			}
			return false;
		}

		// Token: 0x06002076 RID: 8310 RVA: 0x000C95D4 File Offset: 0x000C77D4
		private static int CurrentInsertNear(out bool toTheLeft)
		{
			toTheLeft = false;
			if (ReorderableWidget.draggingReorderable < 0)
			{
				return -1;
			}
			int groupID = ReorderableWidget.reorderables[ReorderableWidget.draggingReorderable].groupID;
			if (groupID < 0 || groupID >= ReorderableWidget.groups.Count)
			{
				Log.ErrorOnce("Reorderable used invalid group.", 1968375560);
				return -1;
			}
			int num = -1;
			for (int i = 0; i < ReorderableWidget.reorderables.Count; i++)
			{
				ReorderableWidget.ReorderableInstance reorderableInstance = ReorderableWidget.reorderables[i];
				if (reorderableInstance.groupID == groupID && (num == -1 || Event.current.mousePosition.DistanceToRect(reorderableInstance.absRect) < Event.current.mousePosition.DistanceToRect(ReorderableWidget.reorderables[num].absRect)))
				{
					num = i;
				}
			}
			if (num >= 0)
			{
				ReorderableWidget.ReorderableInstance reorderableInstance2 = ReorderableWidget.reorderables[num];
				if (ReorderableWidget.groups[reorderableInstance2.groupID].direction == ReorderableDirection.Horizontal)
				{
					toTheLeft = (Event.current.mousePosition.x < reorderableInstance2.absRect.center.x);
				}
				else
				{
					toTheLeft = (Event.current.mousePosition.y < reorderableInstance2.absRect.center.y);
				}
			}
			return num;
		}

		// Token: 0x06002077 RID: 8311 RVA: 0x000C9704 File Offset: 0x000C7904
		private static int GetIndexWithinGroup(int index)
		{
			if (index < 0 || index >= ReorderableWidget.reorderables.Count)
			{
				return -1;
			}
			int num = -1;
			for (int i = 0; i <= index; i++)
			{
				if (ReorderableWidget.reorderables[i].groupID == ReorderableWidget.reorderables[index].groupID)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06002078 RID: 8312 RVA: 0x000C9759 File Offset: 0x000C7959
		private static void StopDragging()
		{
			ReorderableWidget.draggingReorderable = -1;
			ReorderableWidget.dragStartPos = default(Vector2);
			ReorderableWidget.lastInsertNear = -1;
			ReorderableWidget.dragBegun = false;
		}

		// Token: 0x040013B1 RID: 5041
		private static List<ReorderableWidget.ReorderableGroup> groups = new List<ReorderableWidget.ReorderableGroup>();

		// Token: 0x040013B2 RID: 5042
		private static List<ReorderableWidget.ReorderableInstance> reorderables = new List<ReorderableWidget.ReorderableInstance>();

		// Token: 0x040013B3 RID: 5043
		private static int draggingReorderable = -1;

		// Token: 0x040013B4 RID: 5044
		private static Vector2 dragStartPos;

		// Token: 0x040013B5 RID: 5045
		private static bool clicked;

		// Token: 0x040013B6 RID: 5046
		private static bool released;

		// Token: 0x040013B7 RID: 5047
		private static bool dragBegun;

		// Token: 0x040013B8 RID: 5048
		private static Vector2 clickedAt;

		// Token: 0x040013B9 RID: 5049
		private static Rect clickedInRect;

		// Token: 0x040013BA RID: 5050
		private static int lastInsertNear = -1;

		// Token: 0x040013BB RID: 5051
		private static bool lastInsertNearLeft;

		// Token: 0x040013BC RID: 5052
		private static int lastFrameReorderableCount = -1;

		// Token: 0x040013BD RID: 5053
		private const float MinMouseMoveToHighlightReorderable = 5f;

		// Token: 0x040013BE RID: 5054
		private static readonly Color LineColor = new Color(1f, 1f, 1f, 0.6f);

		// Token: 0x040013BF RID: 5055
		private static readonly Color HighlightColor = new Color(1f, 1f, 1f, 0.3f);

		// Token: 0x040013C0 RID: 5056
		private const float LineWidth = 2f;

		// Token: 0x02001C69 RID: 7273
		private struct ReorderableGroup
		{
			// Token: 0x170019FC RID: 6652
			// (get) Token: 0x0600A728 RID: 42792 RVA: 0x003831CA File Offset: 0x003813CA
			public bool DrawLineExactlyBetween
			{
				get
				{
					return this.drawLineExactlyBetween_space > 0f;
				}
			}

			// Token: 0x04006DC6 RID: 28102
			public Action<int, int> reorderedAction;

			// Token: 0x04006DC7 RID: 28103
			public ReorderableDirection direction;

			// Token: 0x04006DC8 RID: 28104
			public float drawLineExactlyBetween_space;

			// Token: 0x04006DC9 RID: 28105
			public Action<int, Vector2> extraDraggedItemOnGUI;
		}

		// Token: 0x02001C6A RID: 7274
		private struct ReorderableInstance
		{
			// Token: 0x04006DCA RID: 28106
			public int groupID;

			// Token: 0x04006DCB RID: 28107
			public Rect rect;

			// Token: 0x04006DCC RID: 28108
			public Rect absRect;
		}
	}
}
