using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001314 RID: 4884
	public class RoomStatWorker_Space : RoomStatWorker
	{
		// Token: 0x060069C0 RID: 27072 RVA: 0x00208E38 File Offset: 0x00207038
		public override float GetScore(Room room)
		{
			if (room.PsychologicallyOutdoors)
			{
				return 350f;
			}
			float num = 0f;
			foreach (IntVec3 c in room.Cells)
			{
				if (c.Standable(room.Map))
				{
					num += 1.4f;
				}
				else if (c.Walkable(room.Map))
				{
					num += 0.5f;
				}
			}
			return Mathf.Min(num, 350f);
		}
	}
}
