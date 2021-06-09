using System;

namespace RimWorld.Planet
{
	// Token: 0x0200215C RID: 8540
	public class SitePartDefWithParams
	{
		// Token: 0x0600B60D RID: 46605 RVA: 0x0007629E File Offset: 0x0007449E
		public SitePartDefWithParams(SitePartDef def, SitePartParams parms)
		{
			this.def = def;
			this.parms = parms;
		}

		// Token: 0x04007CB6 RID: 31926
		public SitePartDef def;

		// Token: 0x04007CB7 RID: 31927
		public SitePartParams parms;
	}
}
