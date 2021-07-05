using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020001AB RID: 427
	public class BoolGrid : IExposable
	{
		// Token: 0x1700023E RID: 574
		// (get) Token: 0x06000BE7 RID: 3047 RVA: 0x00040C18 File Offset: 0x0003EE18
		public int TrueCount
		{
			get
			{
				return this.trueCountInt;
			}
		}

		// Token: 0x1700023F RID: 575
		// (get) Token: 0x06000BE8 RID: 3048 RVA: 0x00040C20 File Offset: 0x0003EE20
		public IEnumerable<IntVec3> ActiveCells
		{
			get
			{
				if (this.trueCountInt == 0)
				{
					yield break;
				}
				int yieldedCount = 0;
				bool canSetMinPossibleTrueIndex = this.minPossibleTrueIndexDirty;
				int num = this.minPossibleTrueIndexDirty ? 0 : this.minPossibleTrueIndexCached;
				int num2;
				for (int i = num; i < this.arr.Length; i = num2 + 1)
				{
					if (this.arr[i])
					{
						if (canSetMinPossibleTrueIndex && this.minPossibleTrueIndexDirty)
						{
							canSetMinPossibleTrueIndex = false;
							this.minPossibleTrueIndexDirty = false;
							this.minPossibleTrueIndexCached = i;
						}
						yield return CellIndicesUtility.IndexToCell(i, this.mapSizeX);
						num2 = yieldedCount;
						yieldedCount = num2 + 1;
						if (yieldedCount >= this.trueCountInt)
						{
							yield break;
						}
					}
					num2 = i;
				}
				yield break;
			}
		}

		// Token: 0x17000240 RID: 576
		public bool this[int index]
		{
			get
			{
				return this.arr[index];
			}
			set
			{
				this.Set(index, value);
			}
		}

		// Token: 0x17000241 RID: 577
		public bool this[IntVec3 c]
		{
			get
			{
				return this.arr[CellIndicesUtility.CellToIndex(c, this.mapSizeX)];
			}
			set
			{
				this.Set(c, value);
			}
		}

		// Token: 0x17000242 RID: 578
		public bool this[int x, int z]
		{
			get
			{
				return this.arr[CellIndicesUtility.CellToIndex(x, z, this.mapSizeX)];
			}
			set
			{
				this.Set(CellIndicesUtility.CellToIndex(x, z, this.mapSizeX), value);
			}
		}

		// Token: 0x06000BEF RID: 3055 RVA: 0x00040C8F File Offset: 0x0003EE8F
		public BoolGrid()
		{
		}

		// Token: 0x06000BF0 RID: 3056 RVA: 0x00040C9E File Offset: 0x0003EE9E
		public BoolGrid(Map map)
		{
			this.ClearAndResizeTo(map);
		}

		// Token: 0x06000BF1 RID: 3057 RVA: 0x00040CB4 File Offset: 0x0003EEB4
		public bool MapSizeMatches(Map map)
		{
			return this.mapSizeX == map.Size.x && this.mapSizeZ == map.Size.z;
		}

		// Token: 0x06000BF2 RID: 3058 RVA: 0x00040CE0 File Offset: 0x0003EEE0
		public void ClearAndResizeTo(Map map)
		{
			if (this.MapSizeMatches(map) && this.arr != null)
			{
				this.Clear();
				return;
			}
			this.mapSizeX = map.Size.x;
			this.mapSizeZ = map.Size.z;
			this.arr = new bool[this.mapSizeX * this.mapSizeZ];
			this.trueCountInt = 0;
			this.minPossibleTrueIndexCached = -1;
			this.minPossibleTrueIndexDirty = false;
		}

		// Token: 0x06000BF3 RID: 3059 RVA: 0x00040D54 File Offset: 0x0003EF54
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.trueCountInt, "trueCount", 0, false);
			Scribe_Values.Look<int>(ref this.mapSizeX, "mapSizeX", 0, false);
			Scribe_Values.Look<int>(ref this.mapSizeZ, "mapSizeZ", 0, false);
			DataExposeUtility.BoolArray(ref this.arr, this.mapSizeX * this.mapSizeZ, "arr");
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.minPossibleTrueIndexDirty = true;
			}
		}

		// Token: 0x06000BF4 RID: 3060 RVA: 0x00040DC3 File Offset: 0x0003EFC3
		public void Clear()
		{
			Array.Clear(this.arr, 0, this.arr.Length);
			this.trueCountInt = 0;
			this.minPossibleTrueIndexCached = -1;
			this.minPossibleTrueIndexDirty = false;
		}

		// Token: 0x06000BF5 RID: 3061 RVA: 0x00040DEE File Offset: 0x0003EFEE
		public virtual void Set(IntVec3 c, bool value)
		{
			this.Set(CellIndicesUtility.CellToIndex(c, this.mapSizeX), value);
		}

		// Token: 0x06000BF6 RID: 3062 RVA: 0x00040E04 File Offset: 0x0003F004
		public virtual void Set(int index, bool value)
		{
			if (this.arr[index] == value)
			{
				return;
			}
			this.arr[index] = value;
			if (value)
			{
				this.trueCountInt++;
				if (this.trueCountInt == 1 || index < this.minPossibleTrueIndexCached)
				{
					this.minPossibleTrueIndexCached = index;
					return;
				}
			}
			else
			{
				this.trueCountInt--;
				if (index == this.minPossibleTrueIndexCached)
				{
					this.minPossibleTrueIndexDirty = true;
				}
			}
		}

		// Token: 0x06000BF7 RID: 3063 RVA: 0x00040E70 File Offset: 0x0003F070
		public void Invert()
		{
			for (int i = 0; i < this.arr.Length; i++)
			{
				this.arr[i] = !this.arr[i];
			}
			this.trueCountInt = this.arr.Length - this.trueCountInt;
			this.minPossibleTrueIndexDirty = true;
		}

		// Token: 0x040009E6 RID: 2534
		private bool[] arr;

		// Token: 0x040009E7 RID: 2535
		private int trueCountInt;

		// Token: 0x040009E8 RID: 2536
		private int mapSizeX;

		// Token: 0x040009E9 RID: 2537
		private int mapSizeZ;

		// Token: 0x040009EA RID: 2538
		private int minPossibleTrueIndexCached = -1;

		// Token: 0x040009EB RID: 2539
		private bool minPossibleTrueIndexDirty;
	}
}
