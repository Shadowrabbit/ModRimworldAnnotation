using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F30 RID: 3888
	public class FilthProperties
	{
		// Token: 0x17000D0D RID: 3341
		// (get) Token: 0x060055A2 RID: 21922 RVA: 0x0003B7C8 File Offset: 0x000399C8
		public bool TerrainSourced
		{
			get
			{
				return (this.placementMask & FilthSourceFlags.Terrain) > FilthSourceFlags.None;
			}
		}

		// Token: 0x040036D4 RID: 14036
		public float cleaningWorkToReduceThickness = 35f;

		// Token: 0x040036D5 RID: 14037
		public bool canFilthAttach;

		// Token: 0x040036D6 RID: 14038
		public bool rainWashes;

		// Token: 0x040036D7 RID: 14039
		public bool allowsFire = true;

		// Token: 0x040036D8 RID: 14040
		public int maxThickness = 100;

		// Token: 0x040036D9 RID: 14041
		public FloatRange disappearsInDays = FloatRange.Zero;

		// Token: 0x040036DA RID: 14042
		public FilthSourceFlags placementMask = FilthSourceFlags.Unnatural;

		// Token: 0x040036DB RID: 14043
		public SoundDef cleaningSound;
	}
}
