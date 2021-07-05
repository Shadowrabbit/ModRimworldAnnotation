using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020000A4 RID: 164
	public class HediffCompProperties
	{
		// Token: 0x06000549 RID: 1353 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void PostLoad()
		{
		}

		// Token: 0x0600054A RID: 1354 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void ResolveReferences(HediffDef parent)
		{
		}

		// Token: 0x0600054B RID: 1355 RVA: 0x0001B844 File Offset: 0x00019A44
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

		// Token: 0x040002C1 RID: 705
		[TranslationHandle]
		public Type compClass;
	}
}
