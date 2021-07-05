using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200042E RID: 1070
	public static class DragSliderManager
	{
		// Token: 0x06002021 RID: 8225 RVA: 0x000C7157 File Offset: 0x000C5357
		public static void ForceStop()
		{
			DragSliderManager.dragging = false;
		}

		// Token: 0x06002022 RID: 8226 RVA: 0x000C715F File Offset: 0x000C535F
		public static bool DragSlider(Rect rect, float rateFactor, DragSliderCallback newStartMethod, DragSliderCallback newDraggingUpdateMethod, DragSliderCallback newCompletedMethod)
		{
			if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && Mouse.IsOver(rect))
			{
				DragSliderManager.lastRateFactor = rateFactor;
				newStartMethod(0f, rateFactor);
				DragSliderManager.StartDragSliding(newDraggingUpdateMethod, newCompletedMethod);
				return true;
			}
			return false;
		}

		// Token: 0x06002023 RID: 8227 RVA: 0x000C719E File Offset: 0x000C539E
		private static void StartDragSliding(DragSliderCallback newDraggingUpdateMethod, DragSliderCallback newCompletedMethod)
		{
			DragSliderManager.dragging = true;
			DragSliderManager.draggingUpdateMethod = newDraggingUpdateMethod;
			DragSliderManager.completedMethod = newCompletedMethod;
			DragSliderManager.rootX = UI.MousePositionOnUI.x;
		}

		// Token: 0x06002024 RID: 8228 RVA: 0x000C71C1 File Offset: 0x000C53C1
		private static float CurMouseOffset()
		{
			return UI.MousePositionOnUI.x - DragSliderManager.rootX;
		}

		// Token: 0x06002025 RID: 8229 RVA: 0x000C71D4 File Offset: 0x000C53D4
		public static void DragSlidersOnGUI()
		{
			if (DragSliderManager.dragging && Event.current.type == EventType.MouseUp && Event.current.button == 0)
			{
				DragSliderManager.dragging = false;
				if (DragSliderManager.completedMethod != null)
				{
					DragSliderManager.completedMethod(DragSliderManager.CurMouseOffset(), DragSliderManager.lastRateFactor);
				}
			}
		}

		// Token: 0x06002026 RID: 8230 RVA: 0x000C7222 File Offset: 0x000C5422
		public static void DragSlidersUpdate()
		{
			if (DragSliderManager.dragging && DragSliderManager.draggingUpdateMethod != null)
			{
				DragSliderManager.draggingUpdateMethod(DragSliderManager.CurMouseOffset(), DragSliderManager.lastRateFactor);
			}
		}

		// Token: 0x04001381 RID: 4993
		private static bool dragging = false;

		// Token: 0x04001382 RID: 4994
		private static float rootX;

		// Token: 0x04001383 RID: 4995
		private static float lastRateFactor = 1f;

		// Token: 0x04001384 RID: 4996
		private static DragSliderCallback draggingUpdateMethod;

		// Token: 0x04001385 RID: 4997
		private static DragSliderCallback completedMethod;
	}
}
