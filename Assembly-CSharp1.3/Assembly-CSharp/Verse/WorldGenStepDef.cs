using System;

namespace Verse
{
	// Token: 0x02000122 RID: 290
	public class WorldGenStepDef : Def
	{
		// Token: 0x060007BB RID: 1979 RVA: 0x00023FCB File Offset: 0x000221CB
		public override void PostLoad()
		{
			base.PostLoad();
			this.worldGenStep.def = this;
		}

		// Token: 0x04000781 RID: 1921
		public float order;

		// Token: 0x04000782 RID: 1922
		public WorldGenStep worldGenStep;
	}
}
