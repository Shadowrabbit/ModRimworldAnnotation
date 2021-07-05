using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020011A9 RID: 4521
	public class CompProperties_StatOffsetBase : CompProperties
	{
		// Token: 0x06006CE9 RID: 27881 RVA: 0x002490FA File Offset: 0x002472FA
		public virtual IEnumerable<string> GetExplanationAbstract(ThingDef def)
		{
			yield break;
		}

		// Token: 0x06006CEA RID: 27882 RVA: 0x00249104 File Offset: 0x00247304
		public virtual float GetMaxOffset(Thing parent = null)
		{
			float num = 0f;
			for (int i = 0; i < this.offsets.Count; i++)
			{
				num += this.offsets[i].MaxOffset(parent);
			}
			return num;
		}

		// Token: 0x04003C9A RID: 15514
		public StatDef statDef;

		// Token: 0x04003C9B RID: 15515
		public List<FocusStrengthOffset> offsets = new List<FocusStrengthOffset>();
	}
}
