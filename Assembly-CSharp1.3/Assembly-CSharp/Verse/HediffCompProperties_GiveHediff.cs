using System;

namespace Verse
{
	// Token: 0x0200029A RID: 666
	public class HediffCompProperties_GiveHediff : HediffCompProperties
	{
		// Token: 0x0600127C RID: 4732 RVA: 0x0006A6E2 File Offset: 0x000688E2
		public HediffCompProperties_GiveHediff()
		{
			this.compClass = typeof(HediffComp_GiveHediff);
		}

		// Token: 0x04000DFA RID: 3578
		public HediffDef hediffDef;

		// Token: 0x04000DFB RID: 3579
		public bool skipIfAlreadyExists;
	}
}
