using System;

namespace Verse
{
	// Token: 0x0200089E RID: 2206
	public abstract class SpecialThingFilterWorker
	{
		// Token: 0x060036AF RID: 13999
		public abstract bool Matches(Thing t);

		// Token: 0x060036B0 RID: 14000 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public virtual bool AlwaysMatches(ThingDef def)
		{
			return false;
		}

		// Token: 0x060036B1 RID: 14001 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public virtual bool CanEverMatch(ThingDef def)
		{
			return true;
		}
	}
}
