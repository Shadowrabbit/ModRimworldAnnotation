using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001531 RID: 5425
	public class RoomRequirement_HasAssignedThroneAnyOf : RoomRequirement_ThingAnyOf
	{
		// Token: 0x06008110 RID: 33040 RVA: 0x002DA6F0 File Offset: 0x002D88F0
		public override bool Met(Room r, Pawn p = null)
		{
			if (p == null)
			{
				return false;
			}
			foreach (Thing thing in r.ContainedAndAdjacentThings)
			{
				if (this.things.Contains(thing.def) && p.ownership.AssignedThrone == thing)
				{
					return true;
				}
			}
			return false;
		}
	}
}
