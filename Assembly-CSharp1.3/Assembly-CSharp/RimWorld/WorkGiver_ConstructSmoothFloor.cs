using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000812 RID: 2066
	public class WorkGiver_ConstructSmoothFloor : WorkGiver_ConstructAffectFloor
	{
		// Token: 0x170009CE RID: 2510
		// (get) Token: 0x0600370B RID: 14091 RVA: 0x0011EB9F File Offset: 0x0011CD9F
		protected override DesignationDef DesDef
		{
			get
			{
				return DesignationDefOf.SmoothFloor;
			}
		}

		// Token: 0x0600370C RID: 14092 RVA: 0x00137777 File Offset: 0x00135977
		public override Job JobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			return JobMaker.MakeJob(JobDefOf.SmoothFloor, c);
		}
	}
}
