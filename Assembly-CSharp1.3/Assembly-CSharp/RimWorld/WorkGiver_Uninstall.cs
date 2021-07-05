using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200081D RID: 2077
	public class WorkGiver_Uninstall : WorkGiver_RemoveBuilding
	{
		// Token: 0x170009DC RID: 2524
		// (get) Token: 0x06003748 RID: 14152 RVA: 0x0011F2C2 File Offset: 0x0011D4C2
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.Uninstall;
			}
		}

		// Token: 0x170009DD RID: 2525
		// (get) Token: 0x06003749 RID: 14153 RVA: 0x0013857A File Offset: 0x0013677A
		protected override JobDef RemoveBuildingJob
		{
			get
			{
				return JobDefOf.Uninstall;
			}
		}

		// Token: 0x0600374A RID: 14154 RVA: 0x00138581 File Offset: 0x00136781
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (t.def.Claimable)
			{
				if (t.Faction != pawn.Faction)
				{
					return false;
				}
			}
			else if (pawn.Faction != Faction.OfPlayer)
			{
				return false;
			}
			return base.HasJobOnThing(pawn, t, forced);
		}
	}
}
