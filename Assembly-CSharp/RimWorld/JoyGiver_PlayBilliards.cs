using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000CF2 RID: 3314
	public class JoyGiver_PlayBilliards : JoyGiver_InteractBuilding
	{
		// Token: 0x17000BBA RID: 3002
		// (get) Token: 0x06004C30 RID: 19504 RVA: 0x0000A2A7 File Offset: 0x000084A7
		protected override bool CanDoDuringGathering
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06004C31 RID: 19505 RVA: 0x000362A7 File Offset: 0x000344A7
		protected override Job TryGivePlayJob(Pawn pawn, Thing t)
		{
			if (!JoyGiver_PlayBilliards.ThingHasStandableSpaceOnAllSides(t))
			{
				return null;
			}
			return JobMaker.MakeJob(this.def.jobDef, t);
		}

		// Token: 0x06004C32 RID: 19506 RVA: 0x001A8EFC File Offset: 0x001A70FC
		public static bool ThingHasStandableSpaceOnAllSides(Thing t)
		{
			CellRect cellRect = t.OccupiedRect();
			foreach (IntVec3 c in cellRect.ExpandedBy(1))
			{
				if (!cellRect.Contains(c) && !c.Standable(t.Map))
				{
					return false;
				}
			}
			return true;
		}
	}
}
