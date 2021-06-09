using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200130B RID: 4875
	public class RoomRoleWorker_RecRoom : RoomRoleWorker
	{
		// Token: 0x060069AB RID: 27051 RVA: 0x0020897C File Offset: 0x00206B7C
		public override float GetScore(Room room)
		{
			int num = 0;
			List<Thing> containedAndAdjacentThings = room.ContainedAndAdjacentThings;
			for (int i = 0; i < containedAndAdjacentThings.Count; i++)
			{
				Thing thing = containedAndAdjacentThings[i];
				if (thing.def.category == ThingCategory.Building)
				{
					List<JoyGiverDef> allDefsListForReading = DefDatabase<JoyGiverDef>.AllDefsListForReading;
					for (int j = 0; j < allDefsListForReading.Count; j++)
					{
						if (allDefsListForReading[j].thingDefs != null && allDefsListForReading[j].thingDefs.Contains(thing.def))
						{
							num++;
							break;
						}
					}
				}
			}
			return (float)num * 5f;
		}
	}
}
