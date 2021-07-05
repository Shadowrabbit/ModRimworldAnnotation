using System;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x020001E4 RID: 484
	public sealed class MapInfo : IExposable
	{
		// Token: 0x170002A8 RID: 680
		// (get) Token: 0x06000DB9 RID: 3513 RVA: 0x0004DA1F File Offset: 0x0004BC1F
		public int Tile
		{
			get
			{
				return this.parent.Tile;
			}
		}

		// Token: 0x170002A9 RID: 681
		// (get) Token: 0x06000DBA RID: 3514 RVA: 0x0004DA2C File Offset: 0x0004BC2C
		public int NumCells
		{
			get
			{
				return this.Size.x * this.Size.y * this.Size.z;
			}
		}

		// Token: 0x170002AA RID: 682
		// (get) Token: 0x06000DBB RID: 3515 RVA: 0x0004DA51 File Offset: 0x0004BC51
		// (set) Token: 0x06000DBC RID: 3516 RVA: 0x0004DA59 File Offset: 0x0004BC59
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

		// Token: 0x06000DBD RID: 3517 RVA: 0x0004DA64 File Offset: 0x0004BC64
		public void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.sizeInt, "size", default(IntVec3), false);
			Scribe_References.Look<MapParent>(ref this.parent, "parent", false);
		}

		// Token: 0x04000B34 RID: 2868
		private IntVec3 sizeInt;

		// Token: 0x04000B35 RID: 2869
		public MapParent parent;
	}
}
