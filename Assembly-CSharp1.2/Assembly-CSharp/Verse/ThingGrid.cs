using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200026F RID: 623
	public sealed class ThingGrid
	{
		// Token: 0x06001014 RID: 4116 RVA: 0x000B8678 File Offset: 0x000B6878
		public ThingGrid(Map map)
		{
			this.map = map;
			CellIndices cellIndices = map.cellIndices;
			this.thingGrid = new List<Thing>[cellIndices.NumGridCells];
			for (int i = 0; i < cellIndices.NumGridCells; i++)
			{
				this.thingGrid[i] = new List<Thing>(4);
			}
		}

		// Token: 0x06001015 RID: 4117 RVA: 0x000B86CC File Offset: 0x000B68CC
		public void Register(Thing t)
		{
			if (t.def.size.x == 1 && t.def.size.z == 1)
			{
				this.RegisterInCell(t, t.Position);
				return;
			}
			CellRect cellRect = t.OccupiedRect();
			for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
			{
				for (int j = cellRect.minX; j <= cellRect.maxX; j++)
				{
					this.RegisterInCell(t, new IntVec3(j, 0, i));
				}
			}
		}

		// Token: 0x06001016 RID: 4118 RVA: 0x000B8750 File Offset: 0x000B6950
		private void RegisterInCell(Thing t, IntVec3 c)
		{
			if (!c.InBounds(this.map))
			{
				Log.Warning(string.Concat(new object[]
				{
					t,
					" tried to register out of bounds at ",
					c,
					". Destroying."
				}), false);
				t.Destroy(DestroyMode.Vanish);
				return;
			}
			this.thingGrid[this.map.cellIndices.CellToIndex(c)].Add(t);
		}

		// Token: 0x06001017 RID: 4119 RVA: 0x000B87C0 File Offset: 0x000B69C0
		public void Deregister(Thing t, bool doEvenIfDespawned = false)
		{
			if (!t.Spawned && !doEvenIfDespawned)
			{
				return;
			}
			if (t.def.size.x == 1 && t.def.size.z == 1)
			{
				this.DeregisterInCell(t, t.Position);
				return;
			}
			CellRect cellRect = t.OccupiedRect();
			for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
			{
				for (int j = cellRect.minX; j <= cellRect.maxX; j++)
				{
					this.DeregisterInCell(t, new IntVec3(j, 0, i));
				}
			}
		}

		// Token: 0x06001018 RID: 4120 RVA: 0x000B8850 File Offset: 0x000B6A50
		private void DeregisterInCell(Thing t, IntVec3 c)
		{
			if (!c.InBounds(this.map))
			{
				Log.Error(t + " tried to de-register out of bounds at " + c, false);
				return;
			}
			int num = this.map.cellIndices.CellToIndex(c);
			if (this.thingGrid[num].Contains(t))
			{
				this.thingGrid[num].Remove(t);
			}
		}

		// Token: 0x06001019 RID: 4121 RVA: 0x00012070 File Offset: 0x00010270
		public IEnumerable<Thing> ThingsAt(IntVec3 c)
		{
			if (!c.InBounds(this.map))
			{
				yield break;
			}
			List<Thing> list = this.thingGrid[this.map.cellIndices.CellToIndex(c)];
			int num;
			for (int i = 0; i < list.Count; i = num + 1)
			{
				yield return list[i];
				num = i;
			}
			yield break;
		}

		// Token: 0x0600101A RID: 4122 RVA: 0x000B88B4 File Offset: 0x000B6AB4
		public List<Thing> ThingsListAt(IntVec3 c)
		{
			if (!c.InBounds(this.map))
			{
				Log.ErrorOnce("Got ThingsListAt out of bounds: " + c, 495287, false);
				return ThingGrid.EmptyThingList;
			}
			return this.thingGrid[this.map.cellIndices.CellToIndex(c)];
		}

		// Token: 0x0600101B RID: 4123 RVA: 0x00012087 File Offset: 0x00010287
		public List<Thing> ThingsListAtFast(IntVec3 c)
		{
			return this.thingGrid[this.map.cellIndices.CellToIndex(c)];
		}

		// Token: 0x0600101C RID: 4124 RVA: 0x000120A1 File Offset: 0x000102A1
		public List<Thing> ThingsListAtFast(int index)
		{
			return this.thingGrid[index];
		}

		// Token: 0x0600101D RID: 4125 RVA: 0x000120AB File Offset: 0x000102AB
		public bool CellContains(IntVec3 c, ThingCategory cat)
		{
			return this.ThingAt(c, cat) != null;
		}

		// Token: 0x0600101E RID: 4126 RVA: 0x000B8908 File Offset: 0x000B6B08
		public Thing ThingAt(IntVec3 c, ThingCategory cat)
		{
			if (!c.InBounds(this.map))
			{
				return null;
			}
			List<Thing> list = this.thingGrid[this.map.cellIndices.CellToIndex(c)];
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].def.category == cat)
				{
					return list[i];
				}
			}
			return null;
		}

		// Token: 0x0600101F RID: 4127 RVA: 0x000120B8 File Offset: 0x000102B8
		public bool CellContains(IntVec3 c, ThingDef def)
		{
			return this.ThingAt(c, def) != null;
		}

		// Token: 0x06001020 RID: 4128 RVA: 0x000B896C File Offset: 0x000B6B6C
		public Thing ThingAt(IntVec3 c, ThingDef def)
		{
			if (!c.InBounds(this.map))
			{
				return null;
			}
			List<Thing> list = this.thingGrid[this.map.cellIndices.CellToIndex(c)];
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].def == def)
				{
					return list[i];
				}
			}
			return null;
		}

		// Token: 0x06001021 RID: 4129 RVA: 0x000B89CC File Offset: 0x000B6BCC
		public T ThingAt<T>(IntVec3 c) where T : Thing
		{
			if (!c.InBounds(this.map))
			{
				return default(T);
			}
			List<Thing> list = this.thingGrid[this.map.cellIndices.CellToIndex(c)];
			for (int i = 0; i < list.Count; i++)
			{
				T t = list[i] as T;
				if (t != null)
				{
					return t;
				}
			}
			return default(T);
		}

		// Token: 0x04000CD4 RID: 3284
		private Map map;

		// Token: 0x04000CD5 RID: 3285
		private List<Thing>[] thingGrid;

		// Token: 0x04000CD6 RID: 3286
		private static readonly List<Thing> EmptyThingList = new List<Thing>();
	}
}
