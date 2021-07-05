using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AD6 RID: 2774
	public class ThingSetMakerDef : Def
	{
		// Token: 0x0600415C RID: 16732 RVA: 0x0015F47A File Offset: 0x0015D67A
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			this.root.ResolveReferences();
		}

		// Token: 0x04002741 RID: 10049
		public ThingSetMaker root;

		// Token: 0x04002742 RID: 10050
		public ThingSetMakerParams debugParams;
	}
}
