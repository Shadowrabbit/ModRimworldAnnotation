﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D88 RID: 3464
	public static class LoadTransportersJobUtility
	{
		// Token: 0x06004F02 RID: 20226 RVA: 0x001B396C File Offset: 0x001B1B6C
		public static bool HasJobOnTransporter(Pawn pawn, CompTransporter transporter)
		{
			return !transporter.parent.IsForbidden(pawn) && transporter.AnythingLeftToLoad && pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation) && pawn.CanReach(transporter.parent, PathEndMode.Touch, pawn.NormalMaxDanger(), false, TraverseMode.ByPawn) && LoadTransportersJobUtility.FindThingToLoad(pawn, transporter).Thing != null;
		}

		// Token: 0x06004F03 RID: 20227 RVA: 0x000379D9 File Offset: 0x00035BD9
		public static Job JobOnTransporter(Pawn p, CompTransporter transporter)
		{
			Job job = JobMaker.MakeJob(JobDefOf.HaulToTransporter, LocalTargetInfo.Invalid, transporter.parent);
			job.ignoreForbidden = true;
			return job;
		}

		// Token: 0x06004F04 RID: 20228 RVA: 0x001B39E0 File Offset: 0x001B1BE0
		public static ThingCount FindThingToLoad(Pawn p, CompTransporter transporter)
		{
			LoadTransportersJobUtility.neededThings.Clear();
			List<TransferableOneWay> leftToLoad = transporter.leftToLoad;
			LoadTransportersJobUtility.tmpAlreadyLoading.Clear();
			if (leftToLoad != null)
			{
				List<Pawn> allPawnsSpawned = transporter.Map.mapPawns.AllPawnsSpawned;
				for (int i = 0; i < allPawnsSpawned.Count; i++)
				{
					if (allPawnsSpawned[i] != p && allPawnsSpawned[i].CurJobDef == JobDefOf.HaulToTransporter)
					{
						JobDriver_HaulToTransporter jobDriver_HaulToTransporter = (JobDriver_HaulToTransporter)allPawnsSpawned[i].jobs.curDriver;
						if (jobDriver_HaulToTransporter.Container == transporter.parent)
						{
							TransferableOneWay transferableOneWay = TransferableUtility.TransferableMatchingDesperate(jobDriver_HaulToTransporter.ThingToCarry, leftToLoad, TransferAsOneMode.PodsOrCaravanPacking);
							if (transferableOneWay != null)
							{
								int num = 0;
								if (LoadTransportersJobUtility.tmpAlreadyLoading.TryGetValue(transferableOneWay, out num))
								{
									LoadTransportersJobUtility.tmpAlreadyLoading[transferableOneWay] = num + jobDriver_HaulToTransporter.initialCount;
								}
								else
								{
									LoadTransportersJobUtility.tmpAlreadyLoading.Add(transferableOneWay, jobDriver_HaulToTransporter.initialCount);
								}
							}
						}
					}
				}
				for (int j = 0; j < leftToLoad.Count; j++)
				{
					TransferableOneWay transferableOneWay2 = leftToLoad[j];
					int num2;
					if (!LoadTransportersJobUtility.tmpAlreadyLoading.TryGetValue(leftToLoad[j], out num2))
					{
						num2 = 0;
					}
					if (transferableOneWay2.CountToTransfer - num2 > 0)
					{
						for (int k = 0; k < transferableOneWay2.things.Count; k++)
						{
							LoadTransportersJobUtility.neededThings.Add(transferableOneWay2.things[k]);
						}
					}
				}
			}
			if (!LoadTransportersJobUtility.neededThings.Any<Thing>())
			{
				LoadTransportersJobUtility.tmpAlreadyLoading.Clear();
				return default(ThingCount);
			}
			Thing thing = GenClosest.ClosestThingReachable(p.Position, p.Map, ThingRequest.ForGroup(ThingRequestGroup.HaulableEver), PathEndMode.Touch, TraverseParms.For(p, Danger.Deadly, TraverseMode.ByPawn, false), 9999f, (Thing x) => LoadTransportersJobUtility.neededThings.Contains(x) && p.CanReserve(x, 1, -1, null, false), null, 0, -1, false, RegionType.Set_Passable, false);
			if (thing == null)
			{
				foreach (Thing thing2 in LoadTransportersJobUtility.neededThings)
				{
					Pawn pawn = thing2 as Pawn;
					if (pawn != null && (!pawn.IsColonist || pawn.Downed) && !pawn.inventory.UnloadEverything && p.CanReserveAndReach(pawn, PathEndMode.Touch, Danger.Deadly, 1, -1, null, false))
					{
						LoadTransportersJobUtility.neededThings.Clear();
						LoadTransportersJobUtility.tmpAlreadyLoading.Clear();
						return new ThingCount(pawn, 1);
					}
				}
			}
			LoadTransportersJobUtility.neededThings.Clear();
			if (thing != null)
			{
				TransferableOneWay transferableOneWay3 = null;
				for (int l = 0; l < leftToLoad.Count; l++)
				{
					if (leftToLoad[l].things.Contains(thing))
					{
						transferableOneWay3 = leftToLoad[l];
						break;
					}
				}
				int num3;
				if (!LoadTransportersJobUtility.tmpAlreadyLoading.TryGetValue(transferableOneWay3, out num3))
				{
					num3 = 0;
				}
				LoadTransportersJobUtility.tmpAlreadyLoading.Clear();
				return new ThingCount(thing, Mathf.Min(transferableOneWay3.CountToTransfer - num3, thing.stackCount));
			}
			LoadTransportersJobUtility.tmpAlreadyLoading.Clear();
			return default(ThingCount);
		}

		// Token: 0x04003362 RID: 13154
		private static HashSet<Thing> neededThings = new HashSet<Thing>();

		// Token: 0x04003363 RID: 13155
		private static Dictionary<TransferableOneWay, int> tmpAlreadyLoading = new Dictionary<TransferableOneWay, int>();
	}
}
