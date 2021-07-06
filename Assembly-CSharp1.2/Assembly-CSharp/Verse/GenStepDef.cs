using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000134 RID: 308
	public class GenStepDef : Def
	{
		// Token: 0x06000837 RID: 2103 RVA: 0x0000C91C File Offset: 0x0000AB1C
		public override void PostLoad()
		{
			base.PostLoad();
			this.genStep.def = this;
		}

		// Token: 0x04000613 RID: 1555
		public SitePartDef linkWithSite;

		// Token: 0x04000614 RID: 1556
		public float order;

		// Token: 0x04000615 RID: 1557
		public GenStep genStep;
	}
}
