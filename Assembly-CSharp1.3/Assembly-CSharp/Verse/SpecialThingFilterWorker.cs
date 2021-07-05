using System;

namespace Verse
{
	// Token: 0x020004EE RID: 1262
	public abstract class SpecialThingFilterWorker
	{
		// Token: 0x0600260D RID: 9741
		public abstract bool Matches(Thing t);

		// Token: 0x0600260E RID: 9742 RVA: 0x0001276E File Offset: 0x0001096E
		public virtual bool AlwaysMatches(ThingDef def)
		{
			return false;
		}

		// Token: 0x0600260F RID: 9743 RVA: 0x000126F5 File Offset: 0x000108F5
		public virtual bool CanEverMatch(ThingDef def)
		{
			return true;
		}
	}
}
