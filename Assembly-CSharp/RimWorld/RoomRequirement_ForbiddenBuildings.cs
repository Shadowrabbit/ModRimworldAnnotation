using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DBD RID: 7613
	public class RoomRequirement_ForbiddenBuildings : RoomRequirement
	{
		// Token: 0x0600A58C RID: 42380 RVA: 0x00300C84 File Offset: 0x002FEE84
		public override bool Met(Room r, Pawn p = null)
		{
			foreach (Thing thing in r.ContainedAndAdjacentThings)
			{
				if (thing.def.building != null)
				{
					for (int i = 0; i < this.buildingTags.Count; i++)
					{
						string item = this.buildingTags[i];
						if (thing.def.building.buildingTags.Contains(item))
						{
							return false;
						}
					}
				}
			}
			return true;
		}

		// Token: 0x04007036 RID: 28726
		public List<string> buildingTags = new List<string>();
	}
}
