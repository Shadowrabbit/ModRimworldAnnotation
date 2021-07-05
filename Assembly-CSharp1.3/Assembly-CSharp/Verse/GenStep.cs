using System;

namespace Verse
{
	// Token: 0x020000C8 RID: 200
	public abstract class GenStep
	{
		// Token: 0x17000100 RID: 256
		// (get) Token: 0x060005F1 RID: 1521
		public abstract int SeedPart { get; }

		// Token: 0x060005F2 RID: 1522
		public abstract void Generate(Map map, GenStepParams parms);

		// Token: 0x04000428 RID: 1064
		public GenStepDef def;
	}
}
