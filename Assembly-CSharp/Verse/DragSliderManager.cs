using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000765 RID: 1893
	public static class DragSliderManager
	{
		// Token: 0x06002FAF RID: 12207 RVA: 0x000257A8 File Offset: 0x000239A8
		public static void ForceStop()
		{
			DragSliderManager.dragging = false;
		}

		// Token: 0x06002FB0 RID: 12208 RVA: 0x000257B0 File Offset: 0x000239B0
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

		// Token: 0x06002FB1 RID: 12209 RVA: 0x000257EF File Offset: 0x000239EF
		private static void StartDragSliding(DragSliderCallback newDraggingUpdateMethod, DragSliderCallback newCompletedMethod)
		{
			DragSliderManager.dragging = true;
			DragSliderManager.draggingUpdateMethod = newDraggingUpdateMethod;
			DragSliderManager.completedMethod = newCompletedMethod;
			DragSliderManager.rootX = UI.MousePositionOnUI.x;
		}

		// Token: 0x06002FB2 RID: 12210 RVA: 0x00025812 File Offset: 0x00023A12
		private static float CurMouseOffset()
		{
			return UI.MousePositionOnUI.x - DragSliderManager.rootX;
		}

		// Token: 0x06002FB3 RID: 12211 RVA: 0x0013C0C4 File Offset: 0x0013A2C4
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

		// Token: 0x06002FB4 RID: 12212 RVA: 0x00025824 File Offset: 0x00023A24
		public static void DragSlidersUpdate()
		{
			if (DragSliderManager.dragging && DragSliderManager.draggingUpdateMethod != null)
			{
				DragSliderManager.draggingUpdateMethod(DragSliderManager.CurMouseOffset(), DragSliderManager.lastRateFactor);
			}
		}

		// Token: 0x04002050 RID: 8272
		private static bool dragging = false;

		// Token: 0x04002051 RID: 8273
		private static float rootX;

		// Token: 0x04002052 RID: 8274
		private static float lastRateFactor = 1f;

		// Token: 0x04002053 RID: 8275
		private static DragSliderCallback draggingUpdateMethod;

		// Token: 0x04002054 RID: 8276
		private static DragSliderCallback completedMethod;
	}
}
