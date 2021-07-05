using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009A7 RID: 2471
	public class ThoughtWorker_Sick : ThoughtWorker
	{
		// Token: 0x06003DDC RID: 15836 RVA: 0x00153970 File Offset: 0x00151B70
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			return p.health.hediffSet.AnyHediffMakesSickThought;
		}
	}
}
