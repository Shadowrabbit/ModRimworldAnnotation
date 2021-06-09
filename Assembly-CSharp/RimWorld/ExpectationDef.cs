using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F86 RID: 3974
	public class ExpectationDef : Def
	{
		// Token: 0x17000D6B RID: 3435
		// (get) Token: 0x06005730 RID: 22320 RVA: 0x0003C77F File Offset: 0x0003A97F
		public bool WealthTriggered
		{
			get
			{
				return this.maxMapWealth >= 0f;
			}
		}

		// Token: 0x06005731 RID: 22321 RVA: 0x0003C791 File Offset: 0x0003A991
		public override IEnumerable<string> ConfigErrors()
		{
			if (this.order < 0)
			{
				yield return "order not defined";
			}
			yield break;
		}

		// Token: 0x040038AA RID: 14506
		public int order = -1;

		// Token: 0x040038AB RID: 14507
		public int thoughtStage = -1;

		// Token: 0x040038AC RID: 14508
		public float maxMapWealth = -1f;

		// Token: 0x040038AD RID: 14509
		public float joyToleranceDropPerDay;

		// Token: 0x040038AE RID: 14510
		public int joyKindsNeeded;
	}
}
