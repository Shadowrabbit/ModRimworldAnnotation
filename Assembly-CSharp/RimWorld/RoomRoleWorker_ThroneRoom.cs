using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200130D RID: 4877
	public class RoomRoleWorker_ThroneRoom : RoomRoleWorker
	{
		// Token: 0x060069AF RID: 27055 RVA: 0x00048071 File Offset: 0x00046271
		public static string Validate(Room room)
		{
			if (room == null || room.OutdoorsForWork)
			{
				return "ThroneMustBePlacedInside".Translate();
			}
			return null;
		}

		// Token: 0x060069B0 RID: 27056 RVA: 0x00208A10 File Offset: 0x00206C10
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
