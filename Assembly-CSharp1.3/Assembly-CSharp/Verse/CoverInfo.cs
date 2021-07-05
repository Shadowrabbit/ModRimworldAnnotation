using System;

namespace Verse
{
	// Token: 0x020004E6 RID: 1254
	public struct CoverInfo
	{
		// Token: 0x1700074C RID: 1868
		// (get) Token: 0x060025DD RID: 9693 RVA: 0x000EAA96 File Offset: 0x000E8C96
		public Thing Thing
		{
			get
			{
				return this.thingInt;
			}
		}

		// Token: 0x1700074D RID: 1869
		// (get) Token: 0x060025DE RID: 9694 RVA: 0x000EAA9E File Offset: 0x000E8C9E
		public float BlockChance
		{
			get
			{
				return this.blockChanceInt;
			}
		}

		// Token: 0x1700074E RID: 1870
		// (get) Token: 0x060025DF RID: 9695 RVA: 0x000EAAA6 File Offset: 0x000E8CA6
		public static CoverInfo Invalid
		{
			get
			{
				return new CoverInfo(null, -999f);
			}
		}

		// Token: 0x060025E0 RID: 9696 RVA: 0x000EAAB3 File Offset: 0x000E8CB3
		public CoverInfo(Thing thing, float blockChance)
		{
			this.thingInt = thing;
			this.blockChanceInt = blockChance;
		}

		// Token: 0x040017B5 RID: 6069
		private Thing thingInt;

		// Token: 0x040017B6 RID: 6070
		private float blockChanceInt;
	}
}
