using System;

namespace Verse
{
	// Token: 0x020000EE RID: 238
	public class MissingPart
	{
		// Token: 0x17000124 RID: 292
		// (get) Token: 0x0600066F RID: 1647 RVA: 0x0001F85D File Offset: 0x0001DA5D
		public BodyPartDef BodyPart
		{
			get
			{
				return this.bodyPart;
			}
		}

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x06000670 RID: 1648 RVA: 0x0001F865 File Offset: 0x0001DA65
		public HediffDef Injury
		{
			get
			{
				return this.injury;
			}
		}

		// Token: 0x040005A3 RID: 1443
		private BodyPartDef bodyPart;

		// Token: 0x040005A4 RID: 1444
		private HediffDef injury;
	}
}
