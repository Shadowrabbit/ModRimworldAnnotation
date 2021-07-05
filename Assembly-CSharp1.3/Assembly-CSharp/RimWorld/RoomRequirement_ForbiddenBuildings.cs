using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001530 RID: 5424
	public class RoomRequirement_ForbiddenBuildings : RoomRequirement
	{
		// Token: 0x0600810D RID: 33037 RVA: 0x002DA61C File Offset: 0x002D881C
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

		// Token: 0x0600810E RID: 33038 RVA: 0x002DA6BC File Offset: 0x002D88BC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<string>(ref this.buildingTags, "buildingTags", LookMode.Undefined, Array.Empty<object>());
		}

		// Token: 0x0400505A RID: 20570
		public List<string> buildingTags = new List<string>();
	}
}
