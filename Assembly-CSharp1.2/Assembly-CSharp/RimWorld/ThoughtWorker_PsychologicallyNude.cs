using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EB6 RID: 3766
	public class ThoughtWorker_PsychologicallyNude : ThoughtWorker
	{
		// Token: 0x060053BD RID: 21437 RVA: 0x0003A465 File Offset: 0x00038665
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			return p.apparel.PsychologicallyNude;
		}
	}
}
