using System;

namespace Verse
{
	// Token: 0x0200088B RID: 2187
	public static class AutomaticPauseModeExtension
	{
		// Token: 0x0600365C RID: 13916 RVA: 0x0015BA1C File Offset: 0x00159C1C
		public static string ToStringHuman(this AutomaticPauseMode mode)
		{
			switch (mode)
			{
			case AutomaticPauseMode.Never:
				return "AutomaticPauseMode_Never".Translate();
			case AutomaticPauseMode.MajorThreat:
				return "AutomaticPauseMode_MajorThreat".Translate();
			case AutomaticPauseMode.AnyThreat:
				return "AutomaticPauseMode_AnyThreat".Translate();
			case AutomaticPauseMode.AnyLetter:
				return "AutomaticPauseMode_AnyLetter".Translate();
			default:
				throw new NotImplementedException();
			}
		}
	}
}
