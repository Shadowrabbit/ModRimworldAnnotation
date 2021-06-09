using System;

namespace Verse
{
	// Token: 0x02000135 RID: 309
	public abstract class GenStep
	{
		// Token: 0x1700017A RID: 378
		// (get) Token: 0x06000839 RID: 2105
		public abstract int SeedPart { get; }

		// Token: 0x0600083A RID: 2106
		public abstract void Generate(Map map, GenStepParams parms);

		// Token: 0x04000616 RID: 1558
		public GenStepDef def;
	}
}
