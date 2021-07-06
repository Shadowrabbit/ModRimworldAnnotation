using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020003EC RID: 1004
	public class HediffCompProperties_RecoveryThought : HediffCompProperties
	{
		// Token: 0x06001889 RID: 6281 RVA: 0x00017463 File Offset: 0x00015663
		public HediffCompProperties_RecoveryThought()
		{
			this.compClass = typeof(HediffComp_RecoveryThought);
		}

		// Token: 0x04001290 RID: 4752
		public ThoughtDef thought;
	}
}
