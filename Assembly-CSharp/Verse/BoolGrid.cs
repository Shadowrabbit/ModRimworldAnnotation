using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000261 RID: 609
	public class BoolGrid : IExposable
	{
		// Token: 0x170002C7 RID: 711
		// (get) Token: 0x06000F5D RID: 3933 RVA: 0x00011871 File Offset: 0x0000FA71
		public int TrueCount
		{
			get
			{
				return this.trueCountInt;
			}
		}

		// Token: 0x170002C8 RID: 712
		// (get) Token: 0x06000F5E RID: 3934 RVA: 0x00011879 File Offset: 0x0000FA79
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

		// Token: 0x170002C9 RID: 713
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

		// Token: 0x170002CA RID: 714
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

		// Token: 0x170002CB RID: 715
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

		// Token: 0x06000F65 RID: 3941 RVA: 0x000118E8 File Offset: 0x0000FAE8
		public BoolGrid()
		{
		}

		// Token: 0x06000F66 RID: 3942 RVA: 0x000118F7 File Offset: 0x0000FAF7
		public BoolGrid(Map map)
		{
			this.ClearAndResizeTo(map);
		}

		// Token: 0x06000F67 RID: 3943 RVA: 0x0001190D File Offset: 0x0000FB0D
		public bool MapSizeMatches(Map map)
		{
			return this.mapSizeX == map.Size.x && this.mapSizeZ == map.Size.z;
		}

		// Token: 0x06000F68 RID: 3944 RVA: 0x000B68A8 File Offset: 0x000B4AA8
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

		// Token: 0x06000F69 RID: 3945 RVA: 0x000B691C File Offset: 0x000B4B1C
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

		// Token: 0x06000F6A RID: 3946 RVA: 0x00011937 File Offset: 0x0000FB37
		public void Clear()
		{
			Array.Clear(this.arr, 0, this.arr.Length);
			this.trueCountInt = 0;
			this.minPossibleTrueIndexCached = -1;
			this.minPossibleTrueIndexDirty = false;
		}

		// Token: 0x06000F6B RID: 3947 RVA: 0x00011962 File Offset: 0x0000FB62
		public virtual void Set(IntVec3 c, bool value)
		{
			this.Set(CellIndicesUtility.CellToIndex(c, this.mapSizeX), value);
		}

		// Token: 0x06000F6C RID: 3948 RVA: 0x000B698C File Offset: 0x000B4B8C
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

		// Token: 0x06000F6D RID: 3949 RVA: 0x000B69F8 File Offset: 0x000B4BF8
		public void Invert()
		{
			for (int i = 0; i < this.arr.Length; i++)
			{
				this.arr[i] = !this.arr[i];
			}
			this.trueCountInt = this.arr.Length - this.trueCountInt;
			this.minPossibleTrueIndexDirty = true;
		}

		// Token: 0x04000CA2 RID: 3234
		private bool[] arr;

		// Token: 0x04000CA3 RID: 3235
		private int trueCountInt;

		// Token: 0x04000CA4 RID: 3236
		private int mapSizeX;

		// Token: 0x04000CA5 RID: 3237
		private int mapSizeZ;

		// Token: 0x04000CA6 RID: 3238
		private int minPossibleTrueIndexCached = -1;

		// Token: 0x04000CA7 RID: 3239
		private bool minPossibleTrueIndexDirty;
	}
}
