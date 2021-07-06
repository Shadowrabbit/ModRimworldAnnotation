using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D9E RID: 3486
	public class WorkGiver_Researcher : WorkGiver_Scanner
	{
		// Token: 0x17000C2C RID: 3116
		// (get) Token: 0x06004F77 RID: 20343 RVA: 0x00037DFA File Offset: 0x00035FFA
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				if (Find.ResearchManager.currentProj == null)
				{
					return ThingRequest.ForGroup(ThingRequestGroup.Nothing);
				}
				return ThingRequest.ForGroup(ThingRequestGroup.ResearchBench);
			}
		}

		// Token: 0x17000C2D RID: 3117
		// (get) Token: 0x06004F78 RID: 20344 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override bool Prioritized
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06004F79 RID: 20345 RVA: 0x00037E16 File Offset: 0x00036016
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return Find.ResearchManager.currentProj == null;
		}

		// Token: 0x06004F7A RID: 20346 RVA: 0x001B4D00 File Offset: 0x001B2F00
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			ResearchProjectDef currentProj = Find.ResearchManager.currentProj;
			if (currentProj == null)
			{
				return false;
			}
			Building_ResearchBench building_ResearchBench = t as Building_ResearchBench;
			return building_ResearchBench != null && currentProj.CanBeResearchedAt(building_ResearchBench, false) && pawn.CanReserve(t, 1, -1, null, forced);
		}

		// Token: 0x06004F7B RID: 20347 RVA: 0x00037E27 File Offset: 0x00036027
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return JobMaker.MakeJob(JobDefOf.Research, t);
		}

		// Token: 0x06004F7C RID: 20348 RVA: 0x00037E39 File Offset: 0x00036039
		public override float GetPriority(Pawn pawn, TargetInfo t)
		{
			return t.Thing.GetStatValue(StatDefOf.ResearchSpeedFactor, true);
		}
	}
}
