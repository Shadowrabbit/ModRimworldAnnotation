using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020017D3 RID: 6099
	public class SiteCoreBackCompat : IExposable
	{
		// Token: 0x06008DF0 RID: 36336 RVA: 0x0032FEE0 File Offset: 0x0032E0E0
		public void ExposeData()
		{
			Scribe_Defs.Look<SitePartDef>(ref this.def, "def");
			Scribe_Deep.Look<SitePartParams>(ref this.parms, "parms", Array.Empty<object>());
		}

		// Token: 0x040059AA RID: 22954
		public SitePartDef def;

		// Token: 0x040059AB RID: 22955
		public SitePartParams parms;
	}
}
