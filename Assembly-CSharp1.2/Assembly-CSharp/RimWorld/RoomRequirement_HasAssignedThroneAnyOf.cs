using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DBE RID: 7614
	public class RoomRequirement_HasAssignedThroneAnyOf : RoomRequirement_ThingAnyOf
	{
		// Token: 0x0600A58E RID: 42382 RVA: 0x00300D24 File Offset: 0x002FEF24
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
