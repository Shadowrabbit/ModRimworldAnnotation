using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020003C1 RID: 961
	public class HediffCompProperties_ChangeImplantLevel : HediffCompProperties
	{
		// Token: 0x060017E7 RID: 6119 RVA: 0x00016C26 File Offset: 0x00014E26
		public HediffCompProperties_ChangeImplantLevel()
		{
			this.compClass = typeof(HediffComp_ChangeImplantLevel);
		}

		// Token: 0x060017E8 RID: 6120 RVA: 0x00016C3E File Offset: 0x00014E3E
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
			else if (!typeof(Hediff_ImplantWithLevel).IsAssignableFrom(this.implant.hediffClass))
			{
				yield return "implant is not Hediff_ImplantWithLevel";
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

		// Token: 0x0400122E RID: 4654
		public HediffDef implant;

		// Token: 0x0400122F RID: 4655
		public int levelOffset;

		// Token: 0x04001230 RID: 4656
		public List<ChangeImplantLevel_Probability> probabilityPerStage;
	}
}
