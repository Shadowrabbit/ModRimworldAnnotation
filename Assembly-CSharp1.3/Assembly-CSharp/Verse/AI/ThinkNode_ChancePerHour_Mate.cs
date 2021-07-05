using System;

namespace Verse.AI
{
	// Token: 0x02000617 RID: 1559
	public class ThinkNode_ChancePerHour_Mate : ThinkNode_ChancePerHour
	{
		// Token: 0x06002D14 RID: 11540 RVA: 0x0010F0C7 File Offset: 0x0010D2C7
		protected override float MtbHours(Pawn pawn)
		{
			return pawn.RaceProps.mateMtbHours;
		}
	}
}
