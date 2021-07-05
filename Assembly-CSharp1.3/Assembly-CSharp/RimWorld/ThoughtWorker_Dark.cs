using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009A2 RID: 2466
	public class ThoughtWorker_Dark : ThoughtWorker
	{
		// Token: 0x06003DCF RID: 15823 RVA: 0x00153698 File Offset: 0x00151898
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			return p.Awake() && p.needs.mood.recentMemory.TicksSinceLastLight > 240 && (p.Ideo == null || !p.Ideo.IdeoPrefersDarkness());
		}
	}
}
