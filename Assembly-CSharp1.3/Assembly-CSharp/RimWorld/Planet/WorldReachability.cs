using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001730 RID: 5936
	public class WorldReachability
	{
		// Token: 0x060088EC RID: 35052 RVA: 0x003136D4 File Offset: 0x003118D4
		public WorldReachability()
		{
			this.fields = new int[Find.WorldGrid.TilesCount];
			this.nextFieldID = 1;
			this.InvalidateAllFields();
		}

		// Token: 0x060088ED RID: 35053 RVA: 0x003136FE File Offset: 0x003118FE
		public void ClearCache()
		{
			this.InvalidateAllFields();
		}

		// Token: 0x060088EE RID: 35054 RVA: 0x00313706 File Offset: 0x00311906
		public bool CanReach(Caravan c, int tile)
		{
			return this.CanReach(c.Tile, tile);
		}

		// Token: 0x060088EF RID: 35055 RVA: 0x00313718 File Offset: 0x00311918
		public bool CanReach(int startTile, int destTile)
		{
			if (startTile < 0 || startTile >= this.fields.Length || destTile < 0 || destTile >= this.fields.Length)
			{
				return false;
			}
			if (this.fields[startTile] == this.impassableFieldID || this.fields[destTile] == this.impassableFieldID)
			{
				return false;
			}
			if (this.IsValidField(this.fields[startTile]) || this.IsValidField(this.fields[destTile]))
			{
				return this.fields[startTile] == this.fields[destTile];
			}
			this.FloodFillAt(startTile);
			return this.fields[startTile] != this.impassableFieldID && this.fields[startTile] == this.fields[destTile];
		}

		// Token: 0x060088F0 RID: 35056 RVA: 0x003137C5 File Offset: 0x003119C5
		private void InvalidateAllFields()
		{
			if (this.nextFieldID == 2147483646)
			{
				this.nextFieldID = 1;
			}
			this.minValidFieldID = this.nextFieldID;
			this.impassableFieldID = this.nextFieldID;
			this.nextFieldID++;
		}

		// Token: 0x060088F1 RID: 35057 RVA: 0x00313801 File Offset: 0x00311A01
		private bool IsValidField(int fieldID)
		{
			return fieldID >= this.minValidFieldID;
		}

		// Token: 0x060088F2 RID: 35058 RVA: 0x00313810 File Offset: 0x00311A10
		private void FloodFillAt(int tile)
		{
			World world = Find.World;
			if (world.Impassable(tile))
			{
				this.fields[tile] = this.impassableFieldID;
				return;
			}
			Find.WorldFloodFiller.FloodFill(tile, (int x) => !world.Impassable(x), delegate(int x)
			{
				this.fields[x] = this.nextFieldID;
			}, int.MaxValue, null);
			this.nextFieldID++;
		}

		// Token: 0x040056F6 RID: 22262
		private int[] fields;

		// Token: 0x040056F7 RID: 22263
		private int nextFieldID;

		// Token: 0x040056F8 RID: 22264
		private int impassableFieldID;

		// Token: 0x040056F9 RID: 22265
		private int minValidFieldID;
	}
}
