using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FF3 RID: 4083
	public class ThingSetMakerDef : Def
	{
		// Token: 0x06005901 RID: 22785 RVA: 0x0003DD27 File Offset: 0x0003BF27
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			this.root.ResolveReferences();
		}

		// Token: 0x04003B81 RID: 15233
		public ThingSetMaker root;

		// Token: 0x04003B82 RID: 15234
		public ThingSetMakerParams debugParams;
	}
}
