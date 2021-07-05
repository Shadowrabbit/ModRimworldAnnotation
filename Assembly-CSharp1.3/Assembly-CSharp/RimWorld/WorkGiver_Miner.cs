using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200084D RID: 2125
	public class WorkGiver_Miner : WorkGiver_Scanner
	{
		// Token: 0x17000A03 RID: 2563
		// (get) Token: 0x06003836 RID: 14390 RVA: 0x0009007E File Offset: 0x0008E27E
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x06003837 RID: 14391 RVA: 0x00034716 File Offset: 0x00032916
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x06003838 RID: 14392 RVA: 0x0013C808 File Offset: 0x0013AA08
		public static void ResetStaticData()
		{
			WorkGiver_Miner.NoPathTrans = "NoPath".Translate();
		}

		// Token: 0x06003839 RID: 14393 RVA: 0x0013C81E File Offset: 0x0013AA1E
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			foreach (Designation designation in pawn.Map.designationManager.SpawnedDesignationsOfDef(DesignationDefOf.Mine))
			{
				bool flag = false;
				for (int i = 0; i < 8; i++)
				{
					IntVec3 c = designation.target.Cell + GenAdj.AdjacentCells[i];
					if (c.InBounds(pawn.Map) && c.Walkable(pawn.Map))
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					Mineable firstMineable = designation.target.Cell.GetFirstMineable(pawn.Map);
					if (firstMineable != null)
					{
						yield return firstMineable;
					}
				}
			}
			IEnumerator<Designation> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600383A RID: 14394 RVA: 0x0013C82E File Offset: 0x0013AA2E
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return !pawn.Map.designationManager.AnySpawnedDesignationOfDef(DesignationDefOf.Mine);
		}

		// Token: 0x0600383B RID: 14395 RVA: 0x0013C848 File Offset: 0x0013AA48
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!t.def.mineable)
			{
				return null;
			}
			if (pawn.Map.designationManager.DesignationAt(t.Position, DesignationDefOf.Mine) == null)
			{
				return null;
			}
			if (!pawn.CanReserve(t, 1, -1, null, forced))
			{
				return null;
			}
			if (!new HistoryEvent(HistoryEventDefOf.Mined, pawn.Named(HistoryEventArgsNames.Doer)).Notify_PawnAboutToDo_Job())
			{
				return null;
			}
			bool flag = false;
			for (int i = 0; i < 8; i++)
			{
				IntVec3 intVec = t.Position + GenAdj.AdjacentCells[i];
				if (intVec.InBounds(pawn.Map) && intVec.Standable(pawn.Map) && ReachabilityImmediate.CanReachImmediate(intVec, t, pawn.Map, PathEndMode.Touch, pawn))
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				for (int j = 0; j < 8; j++)
				{
					IntVec3 intVec2 = t.Position + GenAdj.AdjacentCells[j];
					if (intVec2.InBounds(t.Map) && ReachabilityImmediate.CanReachImmediate(intVec2, t, pawn.Map, PathEndMode.Touch, pawn) && intVec2.WalkableBy(t.Map, pawn) && !intVec2.Standable(t.Map))
					{
						Thing thing = null;
						List<Thing> thingList = intVec2.GetThingList(t.Map);
						for (int k = 0; k < thingList.Count; k++)
						{
							if (thingList[k].def.designateHaulable && thingList[k].def.passability == Traversability.PassThroughOnly)
							{
								thing = thingList[k];
								break;
							}
						}
						if (thing != null)
						{
							Job job = HaulAIUtility.HaulAsideJobFor(pawn, thing);
							if (job != null)
							{
								return job;
							}
							JobFailReason.Is(WorkGiver_Miner.NoPathTrans, null);
							return null;
						}
					}
				}
				JobFailReason.Is(WorkGiver_Miner.NoPathTrans, null);
				return null;
			}
			return JobMaker.MakeJob(JobDefOf.Mine, t, 20000, true);
		}

		// Token: 0x04001F2F RID: 7983
		private static string NoPathTrans;

		// Token: 0x04001F30 RID: 7984
		private const int MiningJobTicks = 20000;
	}
}
