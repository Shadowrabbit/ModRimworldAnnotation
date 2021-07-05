using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F8A RID: 3978
	public class RitualPosition_BesideTree : RitualPosition_ThingDef
	{
		// Token: 0x1700103E RID: 4158
		// (get) Token: 0x06005E21 RID: 24097 RVA: 0x00204D26 File Offset: 0x00202F26
		protected override ThingDef ThingDef
		{
			get
			{
				return ThingDefOf.Plant_TreeGauranlen;
			}
		}

		// Token: 0x06005E22 RID: 24098 RVA: 0x00204D2D File Offset: 0x00202F2D
		public override IntVec3 PositionForThing(Thing t)
		{
			return t.Position - new IntVec3(1, 0, 0);
		}

		// Token: 0x06005E23 RID: 24099 RVA: 0x00204D42 File Offset: 0x00202F42
		protected override Rot4 FacingDir(Thing t)
		{
			return Rot4.East;
		}
	}
}
