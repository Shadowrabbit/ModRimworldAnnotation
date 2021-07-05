using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020014C0 RID: 5312
	public abstract class StatPart
	{
		// Token: 0x06007ED2 RID: 32466
		public abstract void TransformValue(StatRequest req, ref float val);

		// Token: 0x06007ED3 RID: 32467
		public abstract string ExplanationPart(StatRequest req);

		// Token: 0x06007ED4 RID: 32468 RVA: 0x002CE92F File Offset: 0x002CCB2F
		public virtual IEnumerable<string> ConfigErrors()
		{
			yield break;
		}

		// Token: 0x06007ED5 RID: 32469 RVA: 0x002CE938 File Offset: 0x002CCB38
		public virtual IEnumerable<Dialog_InfoCard.Hyperlink> GetInfoCardHyperlinks(StatRequest req)
		{
			yield break;
		}

		// Token: 0x04004F5C RID: 20316
		public float priority;

		// Token: 0x04004F5D RID: 20317
		[Unsaved(false)]
		public StatDef parentStat;
	}
}
