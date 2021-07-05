using System;

namespace Verse
{
	// Token: 0x020000FB RID: 251
	public class RoofDef : Def
	{
		// Token: 0x17000141 RID: 321
		// (get) Token: 0x060006CB RID: 1739 RVA: 0x00021054 File Offset: 0x0001F254
		public bool VanishOnCollapse
		{
			get
			{
				return !this.isThickRoof;
			}
		}

		// Token: 0x04000609 RID: 1545
		public bool isNatural;

		// Token: 0x0400060A RID: 1546
		public bool isThickRoof;

		// Token: 0x0400060B RID: 1547
		public ThingDef collapseLeavingThingDef;

		// Token: 0x0400060C RID: 1548
		public ThingDef filthLeaving;

		// Token: 0x0400060D RID: 1549
		public SoundDef soundPunchThrough;
	}
}
