using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FA9 RID: 4009
	public abstract class RitualTargetFilter : IExposable
	{
		// Token: 0x06005EAD RID: 24237 RVA: 0x000033AC File Offset: 0x000015AC
		public RitualTargetFilter()
		{
		}

		// Token: 0x06005EAE RID: 24238 RVA: 0x00206E53 File Offset: 0x00205053
		public RitualTargetFilter(RitualTargetFilterDef def)
		{
			this.def = def;
		}

		// Token: 0x06005EAF RID: 24239
		public abstract bool CanStart(TargetInfo initiator, TargetInfo selectedTarget, out string rejectionReason);

		// Token: 0x06005EB0 RID: 24240
		public abstract TargetInfo BestTarget(TargetInfo initiator, TargetInfo selectedTarget);

		// Token: 0x06005EB1 RID: 24241
		public abstract IEnumerable<string> GetTargetInfos(TargetInfo initiator);

		// Token: 0x06005EB2 RID: 24242 RVA: 0x00206E62 File Offset: 0x00205062
		public virtual void ExposeData()
		{
			Scribe_Defs.Look<RitualTargetFilterDef>(ref this.def, "def");
		}

		// Token: 0x04003697 RID: 13975
		public RitualTargetFilterDef def;
	}
}
