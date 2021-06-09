using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000265 RID: 613
	public sealed class CoverGrid
	{
		// Token: 0x170002D6 RID: 726
		public Thing this[int index]
		{
			get
			{
				return this.innerArray[index];
			}
		}

		// Token: 0x170002D7 RID: 727
		public Thing this[IntVec3 c]
		{
			get
			{
				return this.innerArray[this.map.cellIndices.CellToIndex(c)];
			}
		}

		// Token: 0x06000F92 RID: 3986 RVA: 0x00011B10 File Offset: 0x0000FD10
		public CoverGrid(Map map)
		{
			this.map = map;
			this.innerArray = new Thing[map.cellIndices.NumGridCells];
		}

		// Token: 0x06000F93 RID: 3987 RVA: 0x000B6E14 File Offset: 0x000B5014
		public void Register(Thing t)
		{
			if (t.def.Fillage == FillCategory.None)
			{
				return;
			}
			CellRect cellRect = t.OccupiedRect();
			for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
			{
				for (int j = cellRect.minX; j <= cellRect.maxX; j++)
				{
					IntVec3 c = new IntVec3(j, 0, i);
					this.RecalculateCell(c, null);
				}
			}
		}

		// Token: 0x06000F94 RID: 3988 RVA: 0x000B6E74 File Offset: 0x000B5074
		public void DeRegister(Thing t)
		{
			if (t.def.Fillage == FillCategory.None)
			{
				return;
			}
			CellRect cellRect = t.OccupiedRect();
			for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
			{
				for (int j = cellRect.minX; j <= cellRect.maxX; j++)
				{
					IntVec3 c = new IntVec3(j, 0, i);
					this.RecalculateCell(c, t);
				}
			}
		}

		// Token: 0x06000F95 RID: 3989 RVA: 0x000B6ED4 File Offset: 0x000B50D4
		private void RecalculateCell(IntVec3 c, Thing ignoreThing = null)
		{
			Thing thing = null;
			float num = 0.001f;
			List<Thing> list = this.map.thingGrid.ThingsListAtFast(c);
			for (int i = 0; i < list.Count; i++)
			{
				Thing thing2 = list[i];
				if (thing2 != ignoreThing && !thing2.Destroyed && thing2.Spawned && thing2.def.fillPercent > num)
				{
					thing = thing2;
					num = thing2.def.fillPercent;
				}
			}
			this.innerArray[this.map.cellIndices.CellToIndex(c)] = thing;
		}

		// Token: 0x04000CB5 RID: 3253
		private Map map;

		// Token: 0x04000CB6 RID: 3254
		private Thing[] innerArray;
	}
}
