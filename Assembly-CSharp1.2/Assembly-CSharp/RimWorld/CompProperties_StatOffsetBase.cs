using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001868 RID: 6248
	public class CompProperties_StatOffsetBase : CompProperties
	{
		// Token: 0x06008AA2 RID: 35490 RVA: 0x0005CF78 File Offset: 0x0005B178
		public virtual IEnumerable<string> GetExplanationAbstract(ThingDef def)
		{
			yield break;
		}

		// Token: 0x06008AA3 RID: 35491 RVA: 0x002877FC File Offset: 0x002859FC
		public virtual float GetMaxOffset(Thing parent = null)
		{
			float num = 0f;
			for (int i = 0; i < this.offsets.Count; i++)
			{
				num += this.offsets[i].MaxOffset(parent);
			}
			return num;
		}

		// Token: 0x040058F1 RID: 22769
		public StatDef statDef;

		// Token: 0x040058F2 RID: 22770
		public List<FocusStrengthOffset> offsets = new List<FocusStrengthOffset>();
	}
}
