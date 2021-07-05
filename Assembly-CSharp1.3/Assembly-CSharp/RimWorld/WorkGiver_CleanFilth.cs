using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000835 RID: 2101
	internal class WorkGiver_CleanFilth : WorkGiver_Scanner
	{
		// Token: 0x170009E7 RID: 2535
		// (get) Token: 0x0600379F RID: 14239 RVA: 0x0009007E File Offset: 0x0008E27E
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x170009E8 RID: 2536
		// (get) Token: 0x060037A0 RID: 14240 RVA: 0x00139898 File Offset: 0x00137A98
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Filth);
			}
		}

		// Token: 0x170009E9 RID: 2537
		// (get) Token: 0x060037A1 RID: 14241 RVA: 0x001398A1 File Offset: 0x00137AA1
		public override int MaxRegionsToScanBeforeGlobalSearch
		{
			get
			{
				return 4;
			}
		}

		// Token: 0x060037A2 RID: 14242 RVA: 0x001398A4 File Offset: 0x00137AA4
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.listerFilthInHomeArea.FilthInHomeArea;
		}

		// Token: 0x060037A3 RID: 14243 RVA: 0x001398B6 File Offset: 0x00137AB6
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return pawn.Map.listerFilthInHomeArea.FilthInHomeArea.Count == 0;
		}

		// Token: 0x060037A4 RID: 14244 RVA: 0x001398D0 File Offset: 0x00137AD0
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Filth filth = t as Filth;
			return filth != null && filth.Map.areaManager.Home[filth.Position] && pawn.CanReserve(t, 1, -1, null, forced) && filth.TicksSinceThickened >= this.MinTicksSinceThickened;
		}

		// Token: 0x060037A5 RID: 14245 RVA: 0x00139930 File Offset: 0x00137B30
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			WorkGiver_CleanFilth.<>c__DisplayClass10_0 CS$<>8__locals1 = new WorkGiver_CleanFilth.<>c__DisplayClass10_0();
			CS$<>8__locals1.pawn = pawn;
			Job job = JobMaker.MakeJob(JobDefOf.Clean);
			job.AddQueuedTarget(TargetIndex.A, t);
			int num = 15;
			CS$<>8__locals1.map = t.Map;
			CS$<>8__locals1.room = t.GetRoom(RegionType.Set_All);
			for (int i = 0; i < 100; i++)
			{
				IntVec3 c = t.Position + GenRadial.RadialPattern[i];
				if (CS$<>8__locals1.<JobOnThing>g__ShouldClean|0(c))
				{
					List<Thing> thingList = c.GetThingList(CS$<>8__locals1.map);
					for (int j = 0; j < thingList.Count; j++)
					{
						Thing thing = thingList[j];
						if (this.HasJobOnThing(CS$<>8__locals1.pawn, thing, forced) && thing != t)
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
				job.targetQueueA.SortBy((LocalTargetInfo targ) => targ.Cell.DistanceToSquared(CS$<>8__locals1.pawn.Position));
			}
			return job;
		}

		// Token: 0x04001F17 RID: 7959
		private int MinTicksSinceThickened = 600;
	}
}
