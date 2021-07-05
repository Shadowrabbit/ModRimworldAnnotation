using System;

namespace Verse.AI
{
	// Token: 0x02000615 RID: 1557
	public class ThinkNode_ChancePerHour_Nuzzle : ThinkNode_ChancePerHour
	{
		// Token: 0x06002D10 RID: 11536 RVA: 0x0010F095 File Offset: 0x0010D295
		protected override float MtbHours(Pawn pawn)
		{
			return pawn.RaceProps.nuzzleMtbHours;
		}
	}
}
