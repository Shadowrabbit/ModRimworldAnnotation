using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020017DF RID: 6111
	public class CompProperties_MeditationFocus : CompProperties_StatOffsetBase
	{
		// Token: 0x0600873D RID: 34621 RVA: 0x0005AD62 File Offset: 0x00058F62
		public CompProperties_MeditationFocus()
		{
			this.compClass = typeof(CompMeditationFocus);
		}

		// Token: 0x0600873E RID: 34622 RVA: 0x0005AD85 File Offset: 0x00058F85
		public override IEnumerable<string> GetExplanationAbstract(ThingDef def)
		{
			int num;
			for (int i = 0; i < this.offsets.Count; i = num + 1)
			{
				string explanationAbstract = this.offsets[i].GetExplanationAbstract(def);
				if (!explanationAbstract.NullOrEmpty())
				{
					yield return explanationAbstract;
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x0600873F RID: 34623 RVA: 0x0027B5E0 File Offset: 0x002797E0
		public override void ResolveReferences(ThingDef parent)
		{
			base.PostLoadSpecial(parent);
			for (int i = 0; i < this.offsets.Count; i++)
			{
				this.offsets[i].ResolveReferences();
			}
		}

		// Token: 0x040056EA RID: 22250
		public List<MeditationFocusDef> focusTypes = new List<MeditationFocusDef>();
	}
}
