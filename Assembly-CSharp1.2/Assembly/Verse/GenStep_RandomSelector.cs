using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020002AC RID: 684
	public class GenStep_RandomSelector : GenStep
	{
		// Token: 0x1700033D RID: 829
		// (get) Token: 0x0600117B RID: 4475 RVA: 0x00012BCD File Offset: 0x00010DCD
		public override int SeedPart
		{
			get
			{
				return 174742427;
			}
		}

		// Token: 0x0600117C RID: 4476 RVA: 0x000C28C0 File Offset: 0x000C0AC0
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

		// Token: 0x04000E22 RID: 3618
		public List<RandomGenStepSelectorOption> options;
	}
}
