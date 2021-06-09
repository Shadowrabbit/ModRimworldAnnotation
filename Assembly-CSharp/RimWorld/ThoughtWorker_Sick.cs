using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EB1 RID: 3761
	public class ThoughtWorker_Sick : ThoughtWorker
	{
		// Token: 0x060053B3 RID: 21427 RVA: 0x0003A446 File Offset: 0x00038646
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			return p.health.hediffSet.AnyHediffMakesSickThought;
		}
	}
}
