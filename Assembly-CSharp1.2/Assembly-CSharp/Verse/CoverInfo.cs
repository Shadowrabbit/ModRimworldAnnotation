using System;

namespace Verse
{
	// Token: 0x02000892 RID: 2194
	public struct CoverInfo
	{
		// Token: 0x1700086C RID: 2156
		// (get) Token: 0x0600366D RID: 13933 RVA: 0x0002A427 File Offset: 0x00028627
		public Thing Thing
		{
			get
			{
				return this.thingInt;
			}
		}

		// Token: 0x1700086D RID: 2157
		// (get) Token: 0x0600366E RID: 13934 RVA: 0x0002A42F File Offset: 0x0002862F
		public float BlockChance
		{
			get
			{
				return this.blockChanceInt;
			}
		}

		// Token: 0x1700086E RID: 2158
		// (get) Token: 0x0600366F RID: 13935 RVA: 0x0002A437 File Offset: 0x00028637
		public static CoverInfo Invalid
		{
			get
			{
				return new CoverInfo(null, -999f);
			}
		}

		// Token: 0x06003670 RID: 13936 RVA: 0x0002A444 File Offset: 0x00028644
		public CoverInfo(Thing thing, float blockChance)
		{
			this.thingInt = thing;
			this.blockChanceInt = blockChance;
		}

		// Token: 0x040025F1 RID: 9713
		private Thing thingInt;

		// Token: 0x040025F2 RID: 9714
		private float blockChanceInt;
	}
}
