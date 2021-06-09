using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000097 RID: 151
	public class AbilityCompProperties
	{
		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x06000523 RID: 1315 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public virtual bool OverridesPsyfocusCost
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x06000524 RID: 1316 RVA: 0x0000A70C File Offset: 0x0000890C
		public virtual FloatRange PsyfocusCostRange
		{
			get
			{
				return FloatRange.ZeroToOne;
			}
		}

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x06000525 RID: 1317 RVA: 0x0000A713 File Offset: 0x00008913
		public virtual string PsyfocusCostExplanation
		{
			get
			{
				return "";
			}
		}

		// Token: 0x06000526 RID: 1318 RVA: 0x0000A71A File Offset: 0x0000891A
		public virtual IEnumerable<string> ConfigErrors(AbilityDef parentDef)
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

		// Token: 0x04000286 RID: 646
		[TranslationHandle]
		public Type compClass;
	}
}
