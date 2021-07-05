using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D23 RID: 7459
	public abstract class StatPart
	{
		// Token: 0x0600A225 RID: 41509
		public abstract void TransformValue(StatRequest req, ref float val);

		// Token: 0x0600A226 RID: 41510
		public abstract string ExplanationPart(StatRequest req);

		// Token: 0x0600A227 RID: 41511 RVA: 0x0006BC72 File Offset: 0x00069E72
		public virtual IEnumerable<string> ConfigErrors()
		{
			yield break;
		}

		// Token: 0x0600A228 RID: 41512 RVA: 0x0006BC7B File Offset: 0x00069E7B
		public virtual IEnumerable<Dialog_InfoCard.Hyperlink> GetInfoCardHyperlinks(StatRequest req)
		{
			yield break;
		}

		// Token: 0x04006E4C RID: 28236
		public float priority;

		// Token: 0x04006E4D RID: 28237
		[Unsaved(false)]
		public StatDef parentStat;
	}
}
