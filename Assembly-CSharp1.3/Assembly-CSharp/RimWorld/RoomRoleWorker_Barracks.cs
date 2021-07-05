using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CDE RID: 3294
	public class RoomRoleWorker_Barracks : RoomRoleWorker
	{
		// Token: 0x06004CE8 RID: 19688 RVA: 0x0019A848 File Offset: 0x00198A48
		public override float GetScore(Room room)
		{
			int num = 0;
			int num2 = 0;
			List<Thing> containedAndAdjacentThings = room.ContainedAndAdjacentThings;
			List<Pawn> list = null;
			for (int i = 0; i < containedAndAdjacentThings.Count; i++)
			{
				Building_Bed building_Bed;
				if ((building_Bed = (containedAndAdjacentThings[i] as Building_Bed)) != null && building_Bed.def.building.bed_humanlike)
				{
					if (building_Bed.ForPrisoners)
					{
						return 0f;
					}
					num++;
					if (!building_Bed.Medical)
					{
						List<Pawn> assignedPawnsForReading = building_Bed.CompAssignableToPawn.AssignedPawnsForReading;
						if (!assignedPawnsForReading.NullOrEmpty<Pawn>())
						{
							if (list == null)
							{
								list = assignedPawnsForReading[0].GetLoveCluster();
							}
							using (List<Pawn>.Enumerator enumerator = assignedPawnsForReading.GetEnumerator())
							{
								while (enumerator.MoveNext())
								{
									Pawn item = enumerator.Current;
									if (!list.Contains(item))
									{
										num2++;
										break;
									}
								}
								goto IL_C5;
							}
						}
						num2++;
					}
				}
				IL_C5:;
			}
			if (num <= 1)
			{
				return 0f;
			}
			return (float)num2 * 100100f;
		}
	}
}
