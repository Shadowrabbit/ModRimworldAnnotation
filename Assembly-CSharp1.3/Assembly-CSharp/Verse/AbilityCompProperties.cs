using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000054 RID: 84
	public class AbilityCompProperties
	{
		// Token: 0x1700009B RID: 155
		// (get) Token: 0x060003D9 RID: 985 RVA: 0x0001276E File Offset: 0x0001096E
		public virtual bool OverridesPsyfocusCost
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x060003DA RID: 986 RVA: 0x00014F6E File Offset: 0x0001316E
		public virtual FloatRange PsyfocusCostRange
		{
			get
			{
				return FloatRange.ZeroToOne;
			}
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x060003DB RID: 987 RVA: 0x00014F75 File Offset: 0x00013175
		public virtual string PsyfocusCostExplanation
		{
			get
			{
				return "";
			}
		}

		// Token: 0x060003DC RID: 988 RVA: 0x00014F7C File Offset: 0x0001317C
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

		// Token: 0x04000125 RID: 293
		[TranslationHandle]
		public Type compClass;
	}
}
