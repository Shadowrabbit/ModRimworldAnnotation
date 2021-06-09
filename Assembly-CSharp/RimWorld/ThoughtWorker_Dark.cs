using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EAC RID: 3756
	public class ThoughtWorker_Dark : ThoughtWorker
	{
		// Token: 0x060053A7 RID: 21415 RVA: 0x0003A3E5 File Offset: 0x000385E5
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			return p.Awake() && p.needs.mood.recentMemory.TicksSinceLastLight > 240;
		}
	}
}
