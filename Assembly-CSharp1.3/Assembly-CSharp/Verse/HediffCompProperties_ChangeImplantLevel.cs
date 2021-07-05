using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000281 RID: 641
	public class HediffCompProperties_ChangeImplantLevel : HediffCompProperties
	{
		// Token: 0x0600122C RID: 4652 RVA: 0x000695F0 File Offset: 0x000677F0
		public HediffCompProperties_ChangeImplantLevel()
		{
			this.compClass = typeof(HediffComp_ChangeImplantLevel);
		}

		// Token: 0x0600122D RID: 4653 RVA: 0x00069608 File Offset: 0x00067808
		public override IEnumerable<string> ConfigErrors(HediffDef parentDef)
		{
			foreach (string text in base.ConfigErrors(parentDef))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.implant == null)
			{
				yield return "implant is null";
			}
			else if (!typeof(Hediff_Level).IsAssignableFrom(this.implant.hediffClass))
			{
				yield return "implant is not Hediff_Level";
			}
			if (this.levelOffset == 0)
			{
				yield return "levelOffset is 0";
			}
			if (this.probabilityPerStage == null)
			{
				yield return "probabilityPerStage is not defined";
			}
			else if (this.probabilityPerStage.Count != parentDef.stages.Count)
			{
				yield return "probabilityPerStage count doesn't match Hediffs number of stages";
			}
			yield break;
			yield break;
		}

		// Token: 0x04000DD1 RID: 3537
		public HediffDef implant;

		// Token: 0x04000DD2 RID: 3538
		public int levelOffset;

		// Token: 0x04000DD3 RID: 3539
		public List<ChangeImplantLevel_Probability> probabilityPerStage;
	}
}
