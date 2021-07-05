using System;

namespace Verse
{
	// Token: 0x02000444 RID: 1092
	internal static class DraggableResultUtility
	{
		// Token: 0x0600212D RID: 8493 RVA: 0x000CF979 File Offset: 0x000CDB79
		public static bool AnyPressed(this Widgets.DraggableResult result)
		{
			return result == Widgets.DraggableResult.Pressed || result == Widgets.DraggableResult.DraggedThenPressed;
		}
	}
}
