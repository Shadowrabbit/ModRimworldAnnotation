using System;

namespace Verse
{
	// Token: 0x020001A8 RID: 424
	public static class PsychGlowUtility
	{
		// Token: 0x06000BD8 RID: 3032 RVA: 0x00040758 File Offset: 0x0003E958
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
