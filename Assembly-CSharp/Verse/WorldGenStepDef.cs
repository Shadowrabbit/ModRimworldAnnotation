using System;

namespace Verse
{
	// Token: 0x020001B8 RID: 440
	public class WorldGenStepDef : Def
	{
		// Token: 0x06000B20 RID: 2848 RVA: 0x0000EA57 File Offset: 0x0000CC57
		public override void PostLoad()
		{
			base.PostLoad();
			this.worldGenStep.def = this;
		}

		// Token: 0x04000A02 RID: 2562
		public float order;

		// Token: 0x04000A03 RID: 2563
		public WorldGenStep worldGenStep;
	}
}
