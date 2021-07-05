using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EBE RID: 3774
	public class ThoughtWorker_IsUndergroundForUndergrounder : ThoughtWorker
	{
		// Token: 0x060053CE RID: 21454 RVA: 0x001C1C58 File Offset: 0x001BFE58
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			bool flag;
			return ThoughtWorker_IsIndoorsForUndergrounder.IsAwakeAndIndoors(p, out flag) && flag;
		}
	}
}
