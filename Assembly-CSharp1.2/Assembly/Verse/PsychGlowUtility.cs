using System;

namespace Verse
{
	// Token: 0x0200025E RID: 606
	public static class PsychGlowUtility
	{
		// Token: 0x06000F4E RID: 3918 RVA: 0x000B64F0 File Offset: 0x000B46F0
		public static string GetLabel(this PsychGlow gl)
		{
			switch (gl)
			{
			case PsychGlow.Dark:
				return "Dark".Translate();
			case PsychGlow.Lit:
				return "Lit".Translate();
			case PsychGlow.Overlit:
				return "LitBrightly".Translate();
			default:
				throw new ArgumentException();
			}
		}
	}
}
