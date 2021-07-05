using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000285 RID: 645
	public class HediffCompProperties_DamageBrain : HediffCompProperties
	{
		// Token: 0x06001238 RID: 4664 RVA: 0x00069836 File Offset: 0x00067A36
		public HediffCompProperties_DamageBrain()
		{
			this.compClass = typeof(HediffComp_DamageBrain);
		}

		// Token: 0x06001239 RID: 4665 RVA: 0x00069859 File Offset: 0x00067A59
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

		// Token: 0x04000DD8 RID: 3544
		public IntRange damageAmount = IntRange.zero;

		// Token: 0x04000DD9 RID: 3545
		public List<float> mtbDaysPerStage;
	}
}
