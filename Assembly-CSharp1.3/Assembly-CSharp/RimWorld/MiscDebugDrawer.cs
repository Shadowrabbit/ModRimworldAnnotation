using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200148C RID: 5260
	public static class MiscDebugDrawer
	{
		// Token: 0x06007DCE RID: 32206 RVA: 0x002C9388 File Offset: 0x002C7588
		public static void DebugDrawInteractionCells()
		{
			if (Find.CurrentMap == null)
			{
				return;
			}
			if (DebugViewSettings.drawInteractionCells)
			{
				foreach (object obj in Find.Selector.SelectedObjects)
				{
					Thing thing = obj as Thing;
					if (thing != null)
					{
						CellRenderer.RenderCell(thing.InteractionCell, 0.5f);
					}
				}
			}
		}
	}
}
