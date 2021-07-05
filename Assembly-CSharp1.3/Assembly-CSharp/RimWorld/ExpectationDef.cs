using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A61 RID: 2657
	public class ExpectationDef : Def
	{
		// Token: 0x17000B2A RID: 2858
		// (get) Token: 0x06003FD6 RID: 16342 RVA: 0x0015A450 File Offset: 0x00158650
		public bool WealthTriggered
		{
			get
			{
				return this.maxMapWealth >= 0f;
			}
		}

		// Token: 0x06003FD7 RID: 16343 RVA: 0x0015A462 File Offset: 0x00158662
		public override IEnumerable<string> ConfigErrors()
		{
			if (this.order < 0)
			{
				yield return "order not defined";
			}
			yield break;
		}

		// Token: 0x040023C7 RID: 9159
		public int order = -1;

		// Token: 0x040023C8 RID: 9160
		public int thoughtStage = -1;

		// Token: 0x040023C9 RID: 9161
		public float maxMapWealth = -1f;

		// Token: 0x040023CA RID: 9162
		public float joyToleranceDropPerDay;

		// Token: 0x040023CB RID: 9163
		public float ritualQualityOffset;

		// Token: 0x040023CC RID: 9164
		public int joyKindsNeeded;

		// Token: 0x040023CD RID: 9165
		public bool forRoles;
	}
}
