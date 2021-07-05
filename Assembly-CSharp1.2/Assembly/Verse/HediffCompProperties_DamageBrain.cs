using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020003C6 RID: 966
	public class HediffCompProperties_DamageBrain : HediffCompProperties
	{
		// Token: 0x060017FC RID: 6140 RVA: 0x00016D5D File Offset: 0x00014F5D
		public HediffCompProperties_DamageBrain()
		{
			this.compClass = typeof(HediffComp_DamageBrain);
		}

		// Token: 0x060017FD RID: 6141 RVA: 0x00016D80 File Offset: 0x00014F80
		public override IEnumerable<string> ConfigErrors(HediffDef parentDef)
		{
			foreach (string text in base.ConfigErrors(parentDef))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.damageAmount == IntRange.zero)
			{
				yield return "damageAmount is not defined";
			}
			if (this.mtbDaysPerStage == null)
			{
				yield return "mtbDaysPerStage is not defined";
			}
			else if (this.mtbDaysPerStage.Count != parentDef.stages.Count)
			{
				yield return "mtbDaysPerStage count doesn't match Hediffs number of stages";
			}
			yield break;
			yield break;
		}

		// Token: 0x0400123C RID: 4668
		public IntRange damageAmount = IntRange.zero;

		// Token: 0x0400123D RID: 4669
		public List<float> mtbDaysPerStage;
	}
}
