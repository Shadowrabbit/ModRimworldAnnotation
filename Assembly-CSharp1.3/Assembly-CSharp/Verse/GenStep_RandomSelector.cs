using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020001E6 RID: 486
	public class GenStep_RandomSelector : GenStep
	{
		// Token: 0x170002AB RID: 683
		// (get) Token: 0x06000DC0 RID: 3520 RVA: 0x0004DAAF File Offset: 0x0004BCAF
		public override int SeedPart
		{
			get
			{
				return 174742427;
			}
		}

		// Token: 0x06000DC1 RID: 3521 RVA: 0x0004DAB8 File Offset: 0x0004BCB8
		public override void Generate(Map map, GenStepParams parms)
		{
			RandomGenStepSelectorOption randomGenStepSelectorOption = this.options.RandomElementByWeight((RandomGenStepSelectorOption opt) => opt.weight);
			if (randomGenStepSelectorOption.genStep != null)
			{
				randomGenStepSelectorOption.genStep.Generate(map, parms);
			}
			if (randomGenStepSelectorOption.def != null)
			{
				randomGenStepSelectorOption.def.genStep.Generate(map, parms);
			}
		}

		// Token: 0x04000B39 RID: 2873
		public List<RandomGenStepSelectorOption> options;
	}
}
