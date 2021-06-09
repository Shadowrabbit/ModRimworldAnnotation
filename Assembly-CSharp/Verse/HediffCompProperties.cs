using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000109 RID: 265
	public class HediffCompProperties
	{
		// Token: 0x0600075B RID: 1883 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void PostLoad()
		{
		}

		// Token: 0x0600075C RID: 1884 RVA: 0x0000BF34 File Offset: 0x0000A134
		public virtual IEnumerable<string> ConfigErrors(HediffDef parentDef)
		{
			if (this.compClass == null)
			{
				yield return "compClass is null";
			}
			int num;
			for (int i = 0; i < parentDef.comps.Count; i = num + 1)
			{
				if (parentDef.comps[i] != this && parentDef.comps[i].compClass == this.compClass)
				{
					yield return "two comps with same compClass: " + this.compClass;
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x0400049D RID: 1181
		[TranslationHandle]
		public Type compClass;
	}
}
