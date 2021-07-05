using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020004F2 RID: 1266
	public class LinkGrid
	{
		// Token: 0x06001F86 RID: 8070 RVA: 0x0001BBEB File Offset: 0x00019DEB
		public LinkGrid(Map map)
		{
			this.map = map;
			this.linkGrid = new LinkFlags[map.cellIndices.NumGridCells];
		}

		// Token: 0x06001F87 RID: 8071 RVA: 0x0001BC10 File Offset: 0x00019E10
		public LinkFlags LinkFlagsAt(IntVec3 c)
		{
			return this.linkGrid[this.map.cellIndices.CellToIndex(c)];
		}

		// Token: 0x06001F88 RID: 8072 RVA: 0x00100494 File Offset: 0x000FE694
		public void Notify_LinkerCreatedOrDestroyed(Thing linker)
		{
			CellIndices cellIndices = this.map.cellIndices;
			foreach (IntVec3 c in linker.OccupiedRect())
			{
				LinkFlags linkFlags = LinkFlags.None;
				List<Thing> list = this.map.thingGrid.ThingsListAt(c);
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].def.graphicData != null)
					{
						linkFlags |= list[i].def.graphicData.linkFlags;
					}
				}
				this.linkGrid[cellIndices.CellToIndex(c)] = linkFlags;
			}
		}

		// Token: 0x04001624 RID: 5668
		private Map map;

		// Token: 0x04001625 RID: 5669
		private LinkFlags[] linkGrid;
	}
}
