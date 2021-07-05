using System;

namespace Verse.AI
{
	// Token: 0x02000A78 RID: 2680
	public class ThinkNode_ChancePerHour_InsectDigChance : ThinkNode_ChancePerHour
	{
		// Token: 0x06003FF9 RID: 16377 RVA: 0x00181C64 File Offset: 0x0017FE64
		protected override float MtbHours(Pawn pawn)
		{
			Room room = pawn.GetRoom(RegionType.Set_Passable);
			if (room == null)
			{
				return 18f;
			}
			int num = room.IsHuge ? 9999 : room.CellCount;
			float num2 = GenMath.LerpDoubleClamped(2f, 25f, 6f, 1f, (float)num);
			return 18f / num2;
		}

		// Token: 0x04002C22 RID: 11298
		private const float BaseMtbHours = 18f;
	}
}
