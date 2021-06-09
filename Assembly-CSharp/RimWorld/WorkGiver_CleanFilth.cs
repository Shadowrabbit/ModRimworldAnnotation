using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D66 RID: 3430
	internal class WorkGiver_CleanFilth : WorkGiver_Scanner
	{
		// Token: 0x17000BF8 RID: 3064
		// (get) Token: 0x06004E4D RID: 20045 RVA: 0x0001B6B4 File Offset: 0x000198B4
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x17000BF9 RID: 3065
		// (get) Token: 0x06004E4E RID: 20046 RVA: 0x00037417 File Offset: 0x00035617
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Filth);
			}
		}

		// Token: 0x17000BFA RID: 3066
		// (get) Token: 0x06004E4F RID: 20047 RVA: 0x00037420 File Offset: 0x00035620
		public override int MaxRegionsToScanBeforeGlobalSearch
		{
			get
			{
				return 4;
			}
		}

		// Token: 0x06004E50 RID: 20048 RVA: 0x00037423 File Offset: 0x00035623
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.listerFilthInHomeArea.FilthInHomeArea;
		}

		// Token: 0x06004E51 RID: 20049 RVA: 0x00037435 File Offset: 0x00035635
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return pawn.Map.listerFilthInHomeArea.FilthInHomeArea.Count == 0;
		}

		// Token: 0x06004E52 RID: 20050 RVA: 0x001B0EA4 File Offset: 0x001AF0A4
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Filth filth = t as Filth;
			return filth != null && filth.Map.areaManager.Home[filth.Position] && pawn.CanReserve(t, 1, -1, null, forced) && filth.TicksSinceThickened >= this.MinTicksSinceThickened;
		}

		// Token: 0x06004E53 RID: 20051 RVA: 0x001B0F04 File Offset: 0x001AF104
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Job job = JobMaker.MakeJob(JobDefOf.Clean);
			job.AddQueuedTarget(TargetIndex.A, t);
			int num = 15;
			Map map = t.Map;
			Room room = t.GetRoom(RegionType.Set_Passable);
			for (int i = 0; i < 100; i++)
			{
				IntVec3 intVec = t.Position + GenRadial.RadialPattern[i];
				if (intVec.InBounds(map) && intVec.GetRoom(map, RegionType.Set_Passable) == room)
				{
					List<Thing> thingList = intVec.GetThingList(map);
					for (int j = 0; j < thingList.Count; j++)
					{
						Thing thing = thingList[j];
						if (this.HasJobOnThing(pawn, thing, forced) && thing != t)
						{
							job.AddQueuedTarget(TargetIndex.A, thing);
						}
					}
					if (job.GetTargetQueue(TargetIndex.A).Count >= num)
					{
						break;
					}
				}
			}
			if (job.targetQueueA != null && job.targetQueueA.Count >= 5)
			{
				job.targetQueueA.SortBy((LocalTargetInfo targ) => targ.Cell.DistanceToSquared(pawn.Position));
			}
			return job;
		}

		// Token: 0x0400331B RID: 13083
		private int MinTicksSinceThickened = 600;
	}
}
