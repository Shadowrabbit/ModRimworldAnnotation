using System;

namespace Verse
{
	// Token: 0x0200078D RID: 1933
	internal static class DraggableResultUtility
	{
		// Token: 0x060030D2 RID: 12498 RVA: 0x00026727 File Offset: 0x00024927
		public static bool AnyPressed(this Widgets.DraggableResult result)
		{
			return result == Widgets.DraggableResult.Pressed || result == Widgets.DraggableResult.DraggedThenPressed;
		}
	}
}
