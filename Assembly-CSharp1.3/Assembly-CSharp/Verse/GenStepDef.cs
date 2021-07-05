using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020000C7 RID: 199
	public class GenStepDef : Def
	{
		// Token: 0x060005EF RID: 1519 RVA: 0x0001E458 File Offset: 0x0001C658
		public override void PostLoad()
		{
			base.PostLoad();
			this.genStep.def = this;
		}

		// Token: 0x04000425 RID: 1061
		public SitePartDef linkWithSite;

		// Token: 0x04000426 RID: 1062
		public float order;

		// Token: 0x04000427 RID: 1063
		public GenStep genStep;
	}
}
