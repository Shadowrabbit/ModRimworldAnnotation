using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200152F RID: 5423
	public class RoomRequirement_ForbidAltars : RoomRequirement
	{
		// Token: 0x0600810B RID: 33035 RVA: 0x002DA5BC File Offset: 0x002D87BC
		public override bool Met(Room r, Pawn p = null)
		{
			using (List<Thing>.Enumerator enumerator = r.ContainedAndAdjacentThings.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.def.isAltar)
					{
						return false;
					}
				}
			}
			return true;
		}
	}
}
