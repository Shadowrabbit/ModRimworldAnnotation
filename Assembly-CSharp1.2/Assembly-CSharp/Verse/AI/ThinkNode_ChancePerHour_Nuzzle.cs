using System;

namespace Verse.AI
{
	// Token: 0x02000A75 RID: 2677
	public class ThinkNode_ChancePerHour_Nuzzle : ThinkNode_ChancePerHour
	{
		// Token: 0x06003FF3 RID: 16371 RVA: 0x0002FE9B File Offset: 0x0002E09B
		protected override float MtbHours(Pawn pawn)
		{
			return pawn.RaceProps.nuzzleMtbHours;
		}
	}
}
