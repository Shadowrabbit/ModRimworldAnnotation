using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F9F RID: 3999
	public abstract class RitualStageAction : IExposable
	{
		// Token: 0x06005E8F RID: 24207 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Apply(LordJob_Ritual ritual)
		{
		}

		// Token: 0x06005E90 RID: 24208 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void ApplyToPawn(LordJob_Ritual ritual, Pawn pawn)
		{
		}

		// Token: 0x06005E91 RID: 24209
		public abstract void ExposeData();
	}
}
