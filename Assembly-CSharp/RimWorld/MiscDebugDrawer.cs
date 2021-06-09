using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CCB RID: 7371
	public static class MiscDebugDrawer
	{
		// Token: 0x0600A04A RID: 41034 RVA: 0x002EE850 File Offset: 0x002ECA50
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
