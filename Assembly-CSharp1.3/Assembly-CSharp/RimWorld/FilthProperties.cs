using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A1F RID: 2591
	public class FilthProperties
	{
		// Token: 0x17000AF9 RID: 2809
		// (get) Token: 0x06003F19 RID: 16153 RVA: 0x0015813F File Offset: 0x0015633F
		public bool TerrainSourced
		{
			get
			{
				return (this.placementMask & FilthSourceFlags.Terrain) > FilthSourceFlags.None;
			}
		}

		// Token: 0x04002231 RID: 8753
		public float cleaningWorkToReduceThickness = 35f;

		// Token: 0x04002232 RID: 8754
		public bool canFilthAttach;

		// Token: 0x04002233 RID: 8755
		public bool rainWashes;

		// Token: 0x04002234 RID: 8756
		public bool allowsFire = true;

		// Token: 0x04002235 RID: 8757
		public int maxThickness = 100;

		// Token: 0x04002236 RID: 8758
		public FloatRange disappearsInDays = FloatRange.Zero;

		// Token: 0x04002237 RID: 8759
		public FilthSourceFlags placementMask = FilthSourceFlags.Unnatural;

		// Token: 0x04002238 RID: 8760
		public SoundDef cleaningSound;
	}
}
