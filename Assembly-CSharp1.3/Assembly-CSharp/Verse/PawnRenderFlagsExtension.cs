using System;

namespace Verse
{
	// Token: 0x02000266 RID: 614
	public static class PawnRenderFlagsExtension
	{
		// Token: 0x0600115A RID: 4442 RVA: 0x00062E68 File Offset: 0x00061068
		public static bool FlagSet(this PawnRenderFlags flags, PawnRenderFlags flag)
		{
			return (flags & flag) > PawnRenderFlags.None;
		}
	}
}
