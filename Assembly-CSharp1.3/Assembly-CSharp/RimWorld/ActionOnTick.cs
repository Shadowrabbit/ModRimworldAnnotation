using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FA4 RID: 4004
	public abstract class ActionOnTick : IExposable
	{
		// Token: 0x06005EA1 RID: 24225
		public abstract void Apply(LordJob_Ritual ritual);

		// Token: 0x06005EA2 RID: 24226 RVA: 0x00206CE7 File Offset: 0x00204EE7
		public virtual void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.tick, "tick", 0, false);
		}

		// Token: 0x0400368F RID: 13967
		public int tick;
	}
}
