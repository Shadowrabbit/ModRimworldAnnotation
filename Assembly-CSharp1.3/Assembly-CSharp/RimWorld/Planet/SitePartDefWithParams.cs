using System;

namespace RimWorld.Planet
{
	// Token: 0x020017D5 RID: 6101
	public class SitePartDefWithParams
	{
		// Token: 0x06008E19 RID: 36377 RVA: 0x00330B32 File Offset: 0x0032ED32
		public SitePartDefWithParams(SitePartDef def, SitePartParams parms)
		{
			this.def = def;
			this.parms = parms;
		}

		// Token: 0x040059B8 RID: 22968
		public SitePartDef def;

		// Token: 0x040059B9 RID: 22969
		public SitePartParams parms;
	}
}
