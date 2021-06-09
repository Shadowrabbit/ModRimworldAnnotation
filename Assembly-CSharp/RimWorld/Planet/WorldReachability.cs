using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002010 RID: 8208
	public class WorldReachability
	{
		// Token: 0x0600ADDB RID: 44507 RVA: 0x000712CB File Offset: 0x0006F4CB
		public WorldReachability()
		{
			this.fields = new int[Find.WorldGrid.TilesCount];
			this.nextFieldID = 1;
			this.InvalidateAllFields();
		}

		// Token: 0x0600ADDC RID: 44508 RVA: 0x000712F5 File Offset: 0x0006F4F5
		public void ClearCache()
		{
			this.InvalidateAllFields();
		}

		// Token: 0x0600ADDD RID: 44509 RVA: 0x000712FD File Offset: 0x0006F4FD
		public bool CanReach(Caravan c, int tile)
		{
			return this.CanReach(c.Tile, tile);
		}

		// Token: 0x0600ADDE RID: 44510 RVA: 0x00329C24 File Offset: 0x00327E24
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

		// Token: 0x0600ADDF RID: 44511 RVA: 0x0007130C File Offset: 0x0006F50C
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

		// Token: 0x0600ADE0 RID: 44512 RVA: 0x00071348 File Offset: 0x0006F548
		private bool IsValidField(int fieldID)
		{
			return fieldID >= this.minValidFieldID;
		}

		// Token: 0x0600ADE1 RID: 44513 RVA: 0x00329CD4 File Offset: 0x00327ED4
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

		// Token: 0x04007768 RID: 30568
		private int[] fields;

		// Token: 0x04007769 RID: 30569
		private int nextFieldID;

		// Token: 0x0400776A RID: 30570
		private int impassableFieldID;

		// Token: 0x0400776B RID: 30571
		private int minValidFieldID;
	}
}
