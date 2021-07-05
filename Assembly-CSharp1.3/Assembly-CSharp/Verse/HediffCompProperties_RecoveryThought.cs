using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020002AF RID: 687
	public class HediffCompProperties_RecoveryThought : HediffCompProperties
	{
		// Token: 0x060012BF RID: 4799 RVA: 0x0006B8DD File Offset: 0x00069ADD
		public HediffCompProperties_RecoveryThought()
		{
			this.compClass = typeof(HediffComp_RecoveryThought);
		}

		// Token: 0x04000E28 RID: 3624
		public ThoughtDef thought;
	}
}
