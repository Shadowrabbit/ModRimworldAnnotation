using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D8B RID: 3467
	public class WorkGiver_Miner : WorkGiver_Scanner
	{
		// Token: 0x17000C16 RID: 3094
		// (get) Token: 0x06004F0D RID: 20237 RVA: 0x0001B6B4 File Offset: 0x000198B4
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x06004F0E RID: 20238 RVA: 0x0002EB44 File Offset: 0x0002CD44
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x06004F0F RID: 20239 RVA: 0x00037A64 File Offset: 0x00035C64
		public static void ResetStaticData()
		{
			WorkGiver_Miner.NoPathTrans = "NoPath".Translate();
		}

		// Token: 0x06004F10 RID: 20240 RVA: 0x00037A7A File Offset: 0x00035C7A
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

		// Token: 0x06004F11 RID: 20241 RVA: 0x00037A8A File Offset: 0x00035C8A
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return !pawn.Map.designationManager.AnySpawnedDesignationOfDef(DesignationDefOf.Mine);
		}

		// Token: 0x06004F12 RID: 20242 RVA: 0x001B3E78 File Offset: 0x001B2078
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
					if (intVec2.InBounds(t.Map) && ReachabilityImmediate.CanReachImmediate(intVec2, t, pawn.Map, PathEndMode.Touch, pawn) && intVec2.Walkable(t.Map) && !intVec2.Standable(t.Map))
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

		// Token: 0x04003365 RID: 13157
		private static string NoPathTrans;

		// Token: 0x04003366 RID: 13158
		private const int MiningJobTicks = 20000;
	}
}
