using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CF1 RID: 3313
	public class RoomStatWorker_Space : RoomStatWorker
	{
		// Token: 0x06004D12 RID: 19730 RVA: 0x0019B360 File Offset: 0x00199560
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
				else if (c.WalkableByNormal(room.Map))
				{
					num += 0.5f;
				}
			}
			return Mathf.Min(num, 350f);
		}
	}
}
