using System;

namespace Verse.AI
{
	// Token: 0x02000619 RID: 1561
	public class ThinkNode_ChancePerHour_InsectDigChance : ThinkNode_ChancePerHour
	{
		// Token: 0x06002D18 RID: 11544 RVA: 0x0010F108 File Offset: 0x0010D308
		protected override float MtbHours(Pawn pawn)
		{
			Room room = pawn.GetRoom(RegionType.Set_All);
			if (room == null)
			{
				return 18f;
			}
			int num = room.IsHuge ? 9999 : room.CellCount;
			float num2 = GenMath.LerpDoubleClamped(2f, 25f, 6f, 1f, (float)num);
			return 18f / num2;
		}

		// Token: 0x04001BAB RID: 7083
		private const float BaseMtbHours = 18f;
	}
}
