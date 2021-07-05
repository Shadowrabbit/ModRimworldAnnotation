using System;

namespace Verse
{
	// Token: 0x02000123 RID: 291
	public abstract class WorldGenStep
	{
		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x060007BD RID: 1981
		public abstract int SeedPart { get; }

		// Token: 0x060007BE RID: 1982
		public abstract void GenerateFresh(string seed);

		// Token: 0x060007BF RID: 1983 RVA: 0x00023FDF File Offset: 0x000221DF
		public virtual void GenerateWithoutWorldData(string seed)
		{
			this.GenerateFresh(seed);
		}

		// Token: 0x060007C0 RID: 1984 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void GenerateFromScribe(string seed)
		{
		}

		// Token: 0x04000783 RID: 1923
		public WorldGenStepDef def;
	}
}
