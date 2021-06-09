using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D4C RID: 3404
	public class WorkGiver_ConstructSmoothFloor : WorkGiver_ConstructAffectFloor
	{
		// Token: 0x17000BE1 RID: 3041
		// (get) Token: 0x06004DD4 RID: 19924 RVA: 0x00032620 File Offset: 0x00030820
		protected override DesignationDef DesDef
		{
			get
			{
				return DesignationDefOf.SmoothFloor;
			}
		}

		// Token: 0x06004DD5 RID: 19925 RVA: 0x00036FC1 File Offset: 0x000351C1
		public override Job JobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			return JobMaker.MakeJob(JobDefOf.SmoothFloor, c);
		}
	}
}
