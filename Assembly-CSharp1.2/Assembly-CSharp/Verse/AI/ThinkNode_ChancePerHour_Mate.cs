using System;

namespace Verse.AI
{
	// Token: 0x02000A76 RID: 2678
	public class ThinkNode_ChancePerHour_Mate : ThinkNode_ChancePerHour
	{
		// Token: 0x06003FF5 RID: 16373 RVA: 0x0002FEB0 File Offset: 0x0002E0B0
		protected override float MtbHours(Pawn pawn)
		{
			return pawn.RaceProps.mateMtbHours;
		}
	}
}
