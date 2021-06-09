using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002155 RID: 8533
	public class SiteCoreBackCompat : IExposable
	{
		// Token: 0x0600B5BB RID: 46523 RVA: 0x00076023 File Offset: 0x00074223
		public void ExposeData()
		{
			Scribe_Defs.Look<SitePartDef>(ref this.def, "def");
			Scribe_Deep.Look<SitePartParams>(ref this.parms, "parms", Array.Empty<object>());
		}

		// Token: 0x04007C88 RID: 31880
		public SitePartDef def;

		// Token: 0x04007C89 RID: 31881
		public SitePartParams parms;
	}
}
