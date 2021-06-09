using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D5B RID: 3419
	public class WorkGiver_Uninstall : WorkGiver_RemoveBuilding
	{
		// Token: 0x17000BF3 RID: 3059
		// (get) Token: 0x06004E27 RID: 20007 RVA: 0x00032C3B File Offset: 0x00030E3B
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.Uninstall;
			}
		}

		// Token: 0x17000BF4 RID: 3060
		// (get) Token: 0x06004E28 RID: 20008 RVA: 0x0003732D File Offset: 0x0003552D
		protected override JobDef RemoveBuildingJob
		{
			get
			{
				return JobDefOf.Uninstall;
			}
		}

		// Token: 0x06004E29 RID: 20009 RVA: 0x00037334 File Offset: 0x00035534
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
