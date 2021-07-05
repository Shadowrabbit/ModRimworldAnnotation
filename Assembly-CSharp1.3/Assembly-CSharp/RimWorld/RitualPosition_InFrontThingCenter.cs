using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F82 RID: 3970
	public class RitualPosition_InFrontThingCenter : RitualPosition_VerticalThingCenter
	{
		// Token: 0x06005E02 RID: 24066 RVA: 0x002045F0 File Offset: 0x002027F0
		protected override CellRect GetRect(CellRect thingRect)
		{
			IntVec3 intVec = IntVec3.South + this.offset;
			return new CellRect(thingRect.minX + intVec.x, thingRect.minZ + intVec.z, thingRect.Width, 1);
		}
	}
}
