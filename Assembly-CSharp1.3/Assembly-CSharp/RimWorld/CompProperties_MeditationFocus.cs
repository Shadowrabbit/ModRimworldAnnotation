using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200114F RID: 4431
	public class CompProperties_MeditationFocus : CompProperties_StatOffsetBase
	{
		// Token: 0x06006A7B RID: 27259 RVA: 0x0023CFD7 File Offset: 0x0023B1D7
		public CompProperties_MeditationFocus()
		{
			this.compClass = typeof(CompMeditationFocus);
		}

		// Token: 0x06006A7C RID: 27260 RVA: 0x0023CFFA File Offset: 0x0023B1FA
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

		// Token: 0x06006A7D RID: 27261 RVA: 0x0023D014 File Offset: 0x0023B214
		public override void ResolveReferences(ThingDef parent)
		{
			base.PostLoadSpecial(parent);
			for (int i = 0; i < this.offsets.Count; i++)
			{
				this.offsets[i].ResolveReferences();
			}
		}

		// Token: 0x04003B56 RID: 15190
		public List<MeditationFocusDef> focusTypes = new List<MeditationFocusDef>();
	}
}
