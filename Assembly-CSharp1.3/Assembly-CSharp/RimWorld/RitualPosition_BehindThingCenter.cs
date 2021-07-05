using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F81 RID: 3969
	public class RitualPosition_BehindThingCenter : RitualPosition_VerticalThingCenter
	{
		// Token: 0x06005E00 RID: 24064 RVA: 0x002045A0 File Offset: 0x002027A0
		protected override CellRect GetRect(CellRect thingRect)
		{
			IntVec3 intVec = IntVec3.North + this.offset;
			return new CellRect(thingRect.minX + intVec.x, thingRect.maxZ + intVec.z, thingRect.Width, 1);
		}
	}
}
