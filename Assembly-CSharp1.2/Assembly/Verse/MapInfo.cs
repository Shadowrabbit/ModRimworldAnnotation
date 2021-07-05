using System;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x020002AA RID: 682
	public sealed class MapInfo : IExposable
	{
		// Token: 0x1700033A RID: 826
		// (get) Token: 0x06001174 RID: 4468 RVA: 0x00012B77 File Offset: 0x00010D77
		public int Tile
		{
			get
			{
				return this.parent.Tile;
			}
		}

		// Token: 0x1700033B RID: 827
		// (get) Token: 0x06001175 RID: 4469 RVA: 0x00012B84 File Offset: 0x00010D84
		public int NumCells
		{
			get
			{
				return this.Size.x * this.Size.y * this.Size.z;
			}
		}

		// Token: 0x1700033C RID: 828
		// (get) Token: 0x06001176 RID: 4470 RVA: 0x00012BA9 File Offset: 0x00010DA9
		// (set) Token: 0x06001177 RID: 4471 RVA: 0x00012BB1 File Offset: 0x00010DB1
		public IntVec3 Size
		{
			get
			{
				return this.sizeInt;
			}
			set
			{
				this.sizeInt = value;
			}
		}

		// Token: 0x06001178 RID: 4472 RVA: 0x000C2888 File Offset: 0x000C0A88
		public void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.sizeInt, "size", default(IntVec3), false);
			Scribe_References.Look<MapParent>(ref this.parent, "parent", false);
		}

		// Token: 0x04000E1D RID: 3613
		private IntVec3 sizeInt;

		// Token: 0x04000E1E RID: 3614
		public MapParent parent;
	}
}
