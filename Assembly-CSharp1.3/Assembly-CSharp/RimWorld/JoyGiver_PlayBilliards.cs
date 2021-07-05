using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007D9 RID: 2009
	public class JoyGiver_PlayBilliards : JoyGiver_InteractBuilding
	{
		// Token: 0x170009BA RID: 2490
		// (get) Token: 0x060035F6 RID: 13814 RVA: 0x000126F5 File Offset: 0x000108F5
		protected override bool CanDoDuringGathering
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060035F7 RID: 13815 RVA: 0x00131829 File Offset: 0x0012FA29
		protected override Job TryGivePlayJob(Pawn pawn, Thing t)
		{
			if (!JoyGiver_PlayBilliards.ThingHasStandableSpaceOnAllSides(t))
			{
				return null;
			}
			return JobMaker.MakeJob(this.def.jobDef, t);
		}

		// Token: 0x060035F8 RID: 13816 RVA: 0x0013184C File Offset: 0x0012FA4C
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
