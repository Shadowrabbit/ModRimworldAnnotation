using System;

namespace Verse
{
	// Token: 0x0200017B RID: 379
	public class RoofDef : Def
	{
		// Token: 0x170001CA RID: 458
		// (get) Token: 0x06000994 RID: 2452 RVA: 0x0000D7EE File Offset: 0x0000B9EE
		public bool VanishOnCollapse
		{
			get
			{
				return !this.isThickRoof;
			}
		}

		// Token: 0x04000830 RID: 2096
		public bool isNatural;

		// Token: 0x04000831 RID: 2097
		public bool isThickRoof;

		// Token: 0x04000832 RID: 2098
		public ThingDef collapseLeavingThingDef;

		// Token: 0x04000833 RID: 2099
		public ThingDef filthLeaving;

		// Token: 0x04000834 RID: 2100
		public SoundDef soundPunchThrough;
	}
}
