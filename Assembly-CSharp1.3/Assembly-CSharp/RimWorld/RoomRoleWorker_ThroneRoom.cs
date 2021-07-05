using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CE9 RID: 3305
	public class RoomRoleWorker_ThroneRoom : RoomRoleWorker
	{
		// Token: 0x06004CFE RID: 19710 RVA: 0x0019ADDE File Offset: 0x00198FDE
		public static string Validate(Room room)
		{
			if (room == null || room.OutdoorsForWork)
			{
				return "ThroneMustBePlacedInside".Translate();
			}
			return null;
		}

		// Token: 0x06004CFF RID: 19711 RVA: 0x0019ADFC File Offset: 0x00198FFC
		public override float GetScore(Room room)
		{
			List<Thing> containedAndAdjacentThings = room.ContainedAndAdjacentThings;
			bool flag = false;
			for (int i = 0; i < containedAndAdjacentThings.Count; i++)
			{
				if (containedAndAdjacentThings[i] is Building_Throne)
				{
					flag = true;
					break;
				}
			}
			return (float)((flag && RoomRoleWorker_ThroneRoom.Validate(room) == null) ? 10000 : 0);
		}
	}
}
