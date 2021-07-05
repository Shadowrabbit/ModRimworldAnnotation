using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x0200042C RID: 1068
	public static class DragAndDropWidget
	{
		// Token: 0x0600200F RID: 8207 RVA: 0x000C6970 File Offset: 0x000C4B70
		public static void DragAndDropWidgetOnGUI_BeforeWindowStack()
		{
			if (DragAndDropWidget.dragBegun && DragAndDropWidget.draggingDraggable >= 0 && DragAndDropWidget.draggingDraggable < DragAndDropWidget.draggables.Count)
			{
				int groupID = DragAndDropWidget.draggables[DragAndDropWidget.draggingDraggable].groupID;
				if (groupID >= 0 && groupID < DragAndDropWidget.groups.Count && DragAndDropWidget.groups[groupID].extraDraggedItemOnGUI != null)
				{
					DragAndDropWidget.groups[groupID].extraDraggedItemOnGUI(DragAndDropWidget.draggables[DragAndDropWidget.draggingDraggable].context, DragAndDropWidget.dragStartPos);
				}
			}
		}

		// Token: 0x06002010 RID: 8208 RVA: 0x000C6A04 File Offset: 0x000C4C04
		public static void DragAndDropWidgetOnGUI_AfterWindowStack()
		{
			if (Event.current.rawType == EventType.MouseUp)
			{
				DragAndDropWidget.released = true;
			}
			if (Event.current.type == EventType.Repaint)
			{
				if (DragAndDropWidget.clicked)
				{
					DragAndDropWidget.StopDragging();
					for (int i = 0; i < DragAndDropWidget.draggables.Count; i++)
					{
						if (DragAndDropWidget.draggables[i].rect == DragAndDropWidget.clickedInRect)
						{
							DragAndDropWidget.draggingDraggable = i;
							Action onStartDragging = DragAndDropWidget.draggables[i].onStartDragging;
							if (onStartDragging != null)
							{
								onStartDragging();
							}
							DragAndDropWidget.dragStartPos = Event.current.mousePosition;
							break;
						}
					}
					DragAndDropWidget.mouseIsDown = true;
					DragAndDropWidget.clicked = false;
				}
				if (DragAndDropWidget.draggingDraggable >= DragAndDropWidget.draggables.Count)
				{
					DragAndDropWidget.StopDragging();
				}
				if (DragAndDropWidget.draggables.Count != DragAndDropWidget.lastFrameDraggableCount)
				{
					DragAndDropWidget.StopDragging();
				}
				if (DragAndDropWidget.released)
				{
					DragAndDropWidget.released = false;
					if (!DragAndDropWidget.dragBegun && DragAndDropWidget.mouseIsDown)
					{
						foreach (DragAndDropWidget.DraggableInstance draggableInstance in DragAndDropWidget.draggables)
						{
							Rect absRect = draggableInstance.absRect;
							if (absRect.Contains(Event.current.mousePosition) && draggableInstance.clickHandler != null)
							{
								draggableInstance.clickHandler();
							}
						}
					}
					DragAndDropWidget.mouseIsDown = false;
					if (DragAndDropWidget.dragBegun && DragAndDropWidget.draggingDraggable >= 0)
					{
						DragAndDropWidget.DraggableInstance draggableInstance2 = DragAndDropWidget.draggables[DragAndDropWidget.draggingDraggable];
						DragAndDropWidget.DropAreaInstance? dropAreaInstance = null;
						for (int j = DragAndDropWidget.dropAreas.Count - 1; j >= 0; j--)
						{
							DragAndDropWidget.DropAreaInstance dropAreaInstance2 = DragAndDropWidget.dropAreas[j];
							if (draggableInstance2.groupID == dropAreaInstance2.groupID && dropAreaInstance2.absRect.Contains(Event.current.mousePosition))
							{
								dropAreaInstance = new DragAndDropWidget.DropAreaInstance?(dropAreaInstance2);
							}
						}
						if (dropAreaInstance != null)
						{
							Action<object> onDrop = dropAreaInstance.Value.onDrop;
							if (onDrop != null)
							{
								onDrop(draggableInstance2.context);
							}
						}
						else
						{
							SoundDefOf.ClickReject.PlayOneShotOnCamera(null);
						}
					}
					DragAndDropWidget.StopDragging();
				}
				DragAndDropWidget.lastFrameDraggableCount = DragAndDropWidget.draggables.Count;
				DragAndDropWidget.groups.Clear();
				DragAndDropWidget.draggables.Clear();
				DragAndDropWidget.dropAreas.Clear();
			}
		}

		// Token: 0x06002011 RID: 8209 RVA: 0x000C6C58 File Offset: 0x000C4E58
		public static int NewGroup(Action<object, Vector2> extraDraggedItemOnGUI = null)
		{
			if (Event.current.type != EventType.Repaint)
			{
				return -1;
			}
			DragAndDropWidget.DraggableGroup item = default(DragAndDropWidget.DraggableGroup);
			item.extraDraggedItemOnGUI = extraDraggedItemOnGUI;
			DragAndDropWidget.groups.Add(item);
			return DragAndDropWidget.groups.Count - 1;
		}

		// Token: 0x06002012 RID: 8210 RVA: 0x000C6C9C File Offset: 0x000C4E9C
		public static bool Draggable(int groupID, Rect rect, object context, Action clickHandler = null, Action onStartDragging = null)
		{
			if (Event.current.type == EventType.Repaint)
			{
				DragAndDropWidget.DraggableInstance item = default(DragAndDropWidget.DraggableInstance);
				item.groupID = groupID;
				item.rect = rect;
				item.context = context;
				item.clickHandler = clickHandler;
				item.onStartDragging = onStartDragging;
				item.absRect = new Rect(UI.GUIToScreenPoint(rect.position), rect.size);
				DragAndDropWidget.draggables.Add(item);
				int num = DragAndDropWidget.draggables.Count - 1;
				if (DragAndDropWidget.draggingDraggable != -1 && (DragAndDropWidget.dragBegun || Vector2.Distance(DragAndDropWidget.clickedAt, Event.current.mousePosition) > 5f))
				{
					if (!DragAndDropWidget.dragBegun)
					{
						SoundDefOf.DragElement.PlayOneShotOnCamera(null);
						DragAndDropWidget.dragBegun = true;
					}
					if (DragAndDropWidget.draggingDraggable == num)
					{
						GUI.color = DragAndDropWidget.HighlightColor;
						Widgets.DrawHighlight(rect);
						GUI.color = Color.white;
					}
				}
				return DragAndDropWidget.draggingDraggable == num && DragAndDropWidget.dragBegun;
			}
			if (Event.current.rawType == EventType.MouseUp)
			{
				DragAndDropWidget.released = true;
			}
			if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && Mouse.IsOver(rect))
			{
				DragAndDropWidget.clicked = true;
				DragAndDropWidget.clickedAt = Event.current.mousePosition;
				DragAndDropWidget.clickedInRect = rect;
			}
			return false;
		}

		// Token: 0x06002013 RID: 8211 RVA: 0x000C6DE4 File Offset: 0x000C4FE4
		public static void DropArea(int groupID, Rect rect, Action<object> onDrop, object context)
		{
			if (Event.current.type == EventType.Repaint)
			{
				DragAndDropWidget.DropAreaInstance item = default(DragAndDropWidget.DropAreaInstance);
				item.groupID = groupID;
				item.rect = rect;
				item.onDrop = onDrop;
				item.absRect = new Rect(UI.GUIToScreenPoint(rect.position), rect.size);
				item.context = context;
				DragAndDropWidget.dropAreas.Add(item);
			}
		}

		// Token: 0x06002014 RID: 8212 RVA: 0x000C6E50 File Offset: 0x000C5050
		public static object CurrentlyDraggedDraggable()
		{
			if (!DragAndDropWidget.dragBegun || DragAndDropWidget.draggingDraggable < 0)
			{
				return null;
			}
			return DragAndDropWidget.draggables[DragAndDropWidget.draggingDraggable].context;
		}

		// Token: 0x06002015 RID: 8213 RVA: 0x000C6E78 File Offset: 0x000C5078
		public static object HoveringDropArea(int groupID)
		{
			DragAndDropWidget.DropAreaInstance? dropAreaInstance = null;
			for (int i = DragAndDropWidget.dropAreas.Count - 1; i >= 0; i--)
			{
				DragAndDropWidget.DropAreaInstance dropAreaInstance2 = DragAndDropWidget.dropAreas[i];
				if (groupID == dropAreaInstance2.groupID && dropAreaInstance2.rect.Contains(Event.current.mousePosition))
				{
					dropAreaInstance = new DragAndDropWidget.DropAreaInstance?(dropAreaInstance2);
				}
			}
			if (dropAreaInstance == null)
			{
				return null;
			}
			return dropAreaInstance.Value.context;
		}

		// Token: 0x06002016 RID: 8214 RVA: 0x000C6EF0 File Offset: 0x000C50F0
		public static Rect? HoveringDropAreaRect(int groupID, Vector3? mousePos = null)
		{
			Vector3 point = mousePos ?? Event.current.mousePosition;
			DragAndDropWidget.DropAreaInstance? dropAreaInstance = null;
			for (int i = DragAndDropWidget.dropAreas.Count - 1; i >= 0; i--)
			{
				DragAndDropWidget.DropAreaInstance dropAreaInstance2 = DragAndDropWidget.dropAreas[i];
				if (groupID == dropAreaInstance2.groupID && dropAreaInstance2.rect.Contains(point))
				{
					dropAreaInstance = new DragAndDropWidget.DropAreaInstance?(dropAreaInstance2);
				}
			}
			if (dropAreaInstance == null)
			{
				return null;
			}
			return new Rect?(dropAreaInstance.GetValueOrDefault().rect);
		}

		// Token: 0x06002017 RID: 8215 RVA: 0x000C6F94 File Offset: 0x000C5194
		public static object DraggableAt(int groupID, Vector3 mousePos)
		{
			DragAndDropWidget.DraggableInstance? draggableInstance = null;
			for (int i = DragAndDropWidget.draggables.Count - 1; i >= 0; i--)
			{
				DragAndDropWidget.DraggableInstance draggableInstance2 = DragAndDropWidget.draggables[i];
				if (groupID == draggableInstance2.groupID && draggableInstance2.rect.Contains(mousePos))
				{
					draggableInstance = new DragAndDropWidget.DraggableInstance?(draggableInstance2);
				}
			}
			if (draggableInstance == null)
			{
				return null;
			}
			return draggableInstance.Value.context;
		}

		// Token: 0x06002018 RID: 8216 RVA: 0x000C7004 File Offset: 0x000C5204
		private static object GetDraggable(int groupID, Vector3 mousePosAbs, int direction)
		{
			float num = float.PositiveInfinity;
			DragAndDropWidget.DraggableInstance? draggableInstance = null;
			for (int i = DragAndDropWidget.draggables.Count - 1; i >= 0; i--)
			{
				DragAndDropWidget.DraggableInstance draggableInstance2 = DragAndDropWidget.draggables[i];
				if (groupID == draggableInstance2.groupID)
				{
					Rect absRect = draggableInstance2.absRect;
					if (mousePosAbs.y >= absRect.yMin && mousePosAbs.y <= absRect.yMax)
					{
						float num2 = (mousePosAbs.x - absRect.xMax) * (float)direction;
						if (num2 >= 0f && num2 < num)
						{
							num = num2;
							draggableInstance = new DragAndDropWidget.DraggableInstance?(draggableInstance2);
						}
					}
				}
			}
			if (draggableInstance == null)
			{
				return null;
			}
			return draggableInstance.Value.context;
		}

		// Token: 0x06002019 RID: 8217 RVA: 0x000C70B4 File Offset: 0x000C52B4
		public static object GetDraggableBefore(int groupID, Vector3 mousePosAbs)
		{
			return DragAndDropWidget.GetDraggable(groupID, mousePosAbs, 1);
		}

		// Token: 0x0600201A RID: 8218 RVA: 0x000C70BE File Offset: 0x000C52BE
		public static object GetDraggableAfter(int groupID, Vector3 mousePosAbs)
		{
			return DragAndDropWidget.GetDraggable(groupID, mousePosAbs, -1);
		}

		// Token: 0x0600201B RID: 8219 RVA: 0x000C70C8 File Offset: 0x000C52C8
		private static void StopDragging()
		{
			DragAndDropWidget.draggingDraggable = -1;
			DragAndDropWidget.dragStartPos = default(Vector2);
			DragAndDropWidget.dragBegun = false;
		}

		// Token: 0x04001371 RID: 4977
		private static List<DragAndDropWidget.DraggableGroup> groups = new List<DragAndDropWidget.DraggableGroup>();

		// Token: 0x04001372 RID: 4978
		private static List<DragAndDropWidget.DropAreaInstance> dropAreas = new List<DragAndDropWidget.DropAreaInstance>();

		// Token: 0x04001373 RID: 4979
		private static List<DragAndDropWidget.DraggableInstance> draggables = new List<DragAndDropWidget.DraggableInstance>();

		// Token: 0x04001374 RID: 4980
		private static int draggingDraggable = -1;

		// Token: 0x04001375 RID: 4981
		private static Vector2 dragStartPos;

		// Token: 0x04001376 RID: 4982
		private static bool mouseIsDown;

		// Token: 0x04001377 RID: 4983
		private static bool clicked;

		// Token: 0x04001378 RID: 4984
		private static bool released;

		// Token: 0x04001379 RID: 4985
		private static bool dragBegun;

		// Token: 0x0400137A RID: 4986
		private static Vector2 clickedAt;

		// Token: 0x0400137B RID: 4987
		private static Rect clickedInRect;

		// Token: 0x0400137C RID: 4988
		private static int lastFrameDraggableCount = -1;

		// Token: 0x0400137D RID: 4989
		private const float MinMouseMoveToHighlightDraggable = 5f;

		// Token: 0x0400137E RID: 4990
		private static readonly Color LineColor = new Color(1f, 1f, 1f, 0.6f);

		// Token: 0x0400137F RID: 4991
		private static readonly Color HighlightColor = new Color(1f, 1f, 1f, 0.3f);

		// Token: 0x04001380 RID: 4992
		private const float LineWidth = 2f;

		// Token: 0x02001C5B RID: 7259
		private struct DraggableGroup
		{
			// Token: 0x04006D9A RID: 28058
			public Action<object, Vector2> extraDraggedItemOnGUI;
		}

		// Token: 0x02001C5C RID: 7260
		private struct DropAreaInstance
		{
			// Token: 0x04006D9B RID: 28059
			public int groupID;

			// Token: 0x04006D9C RID: 28060
			public Rect rect;

			// Token: 0x04006D9D RID: 28061
			public Rect absRect;

			// Token: 0x04006D9E RID: 28062
			public Action<object> onDrop;

			// Token: 0x04006D9F RID: 28063
			public object context;
		}

		// Token: 0x02001C5D RID: 7261
		private struct DraggableInstance
		{
			// Token: 0x04006DA0 RID: 28064
			public int groupID;

			// Token: 0x04006DA1 RID: 28065
			public Rect rect;

			// Token: 0x04006DA2 RID: 28066
			public Rect absRect;

			// Token: 0x04006DA3 RID: 28067
			public object context;

			// Token: 0x04006DA4 RID: 28068
			public Action clickHandler;

			// Token: 0x04006DA5 RID: 28069
			public Action onStartDragging;
		}
	}
}
