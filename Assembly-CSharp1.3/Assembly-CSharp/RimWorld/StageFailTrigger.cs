using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F13 RID: 3859
	public abstract class StageFailTrigger : IExposable
	{
		// Token: 0x06005BED RID: 23533 RVA: 0x001FC16B File Offset: 0x001FA36B
		public string Reason(LordJob_Ritual ritual, TargetInfo spot)
		{
			return this.desc;
		}

		// Token: 0x06005BEE RID: 23534
		public abstract bool Failed(LordJob_Ritual ritual, TargetInfo spot, TargetInfo focus);

		// Token: 0x06005BEF RID: 23535 RVA: 0x001FC173 File Offset: 0x001FA373
		public virtual void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.desc, "desc", null, false);
			Scribe_Values.Look<int>(ref this.allowanceTicks, "allowanceTicks", 0, false);
		}

		// Token: 0x04003588 RID: 13704
		[MustTranslate]
		public string desc;

		// Token: 0x04003589 RID: 13705
		public int allowanceTicks;
	}
}
