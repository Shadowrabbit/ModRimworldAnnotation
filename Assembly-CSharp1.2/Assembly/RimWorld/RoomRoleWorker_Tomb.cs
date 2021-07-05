using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200130E RID: 4878
	public class RoomRoleWorker_Tomb : RoomRoleWorker
	{
		// Token: 0x060069B2 RID: 27058 RVA: 0x00208A60 File Offset: 0x00206C60
		public override float GetScore(Room room)
		{
			int num = 0;
			List<Thing> containedAndAdjacentThings = room.ContainedAndAdjacentThings;
			for (int i = 0; i < containedAndAdjacentThings.Count; i++)
			{
				if (containedAndAdjacentThings[i] is Building_Sarcophagus)
				{
					num++;
				}
			}
			return 50f * (float)num;
		}
	}
}
