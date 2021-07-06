using System;

namespace Verse
{
	// Token: 0x020003E6 RID: 998
	public class HediffCompProperties_KillAfterDays : HediffCompProperties
	{
		// Token: 0x06001875 RID: 6261 RVA: 0x0001734E File Offset: 0x0001554E
		public HediffCompProperties_KillAfterDays()
		{
			this.compClass = typeof(HediffComp_KillAfterDays);
		}

		// Token: 0x04001285 RID: 4741
		public int days;
	}
}
