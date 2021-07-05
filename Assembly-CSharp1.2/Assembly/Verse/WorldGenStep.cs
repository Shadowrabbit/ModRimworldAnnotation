using System;

namespace Verse
{
	// Token: 0x020001B9 RID: 441
	public abstract class WorldGenStep
	{
		// Token: 0x17000244 RID: 580
		// (get) Token: 0x06000B22 RID: 2850
		public abstract int SeedPart { get; }

		// Token: 0x06000B23 RID: 2851
		public abstract void GenerateFresh(string seed);

		// Token: 0x06000B24 RID: 2852 RVA: 0x0000EA6B File Offset: 0x0000CC6B
		public virtual void GenerateWithoutWorldData(string seed)
		{
			this.GenerateFresh(seed);
		}

		// Token: 0x06000B25 RID: 2853 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void GenerateFromScribe(string seed)
		{
		}

		// Token: 0x04000A04 RID: 2564
		public WorldGenStepDef def;
	}
}
