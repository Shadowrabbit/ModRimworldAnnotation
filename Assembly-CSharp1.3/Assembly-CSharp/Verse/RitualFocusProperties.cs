using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020000B5 RID: 181
	public class RitualFocusProperties
	{
		// Token: 0x04000364 RID: 868
		public IntRange spectateDistance = new IntRange(2, 2);

		// Token: 0x04000365 RID: 869
		public SpectateRectSide allowedSpectateSides = SpectateRectSide.Horizontal;

		// Token: 0x04000366 RID: 870
		public bool consumable;
	}
}
