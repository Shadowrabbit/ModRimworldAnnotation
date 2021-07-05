using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020008C5 RID: 2245
	public class LordToilData_Venerate : LordToilData
	{
		// Token: 0x06003B2E RID: 15150 RVA: 0x0014ABF1 File Offset: 0x00148DF1
		public override void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.currentNearVeneratorTicks, "currentNearVeneratorTicks", 0, false);
			Scribe_Values.Look<int>(ref this.lastNearVeneratorIndex, "lastNearVeneratorIndex", 0, false);
		}

		// Token: 0x0400203B RID: 8251
		public int currentNearVeneratorTicks = -1;

		// Token: 0x0400203C RID: 8252
		public int lastNearVeneratorIndex = -1;
	}
}
