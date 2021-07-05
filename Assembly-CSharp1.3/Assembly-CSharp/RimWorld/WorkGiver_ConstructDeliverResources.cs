﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000815 RID: 2069
	public abstract class WorkGiver_ConstructDeliverResources : WorkGiver_Scanner
	{
		// Token: 0x06003716 RID: 14102 RVA: 0x00034716 File Offset: 0x00032916
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x06003717 RID: 14103 RVA: 0x0013789F File Offset: 0x00135A9F
		public static void ResetStaticData()
		{
			WorkGiver_ConstructDeliverResources.MissingMaterialsTranslated = "MissingMaterials".Translate();
			WorkGiver_ConstructDeliverResources.ForbiddenLowerTranslated = "ForbiddenLower".Translate();
			WorkGiver_ConstructDeliverResources.NoPathTranslated = "NoPath".Translate();
		}

		// Token: 0x06003718 RID: 14104 RVA: 0x001378DD File Offset: 0x00135ADD
		private static bool ResourceValidator(Pawn pawn, ThingDefCountClass need, Thing th)
		{
			return th.def == need.thingDef && !th.IsForbidden(pawn) && pawn.CanReserve(th, 1, -1, null, false);
		}

		// Token: 0x06003719 RID: 14105 RVA: 0x00137910 File Offset: 0x00135B10
		protected Job ResourceDeliverJobFor(Pawn pawn, IConstructible c, bool canRemoveExistingFloorUnderNearbyNeeders = true)
		{
			Blueprint_Install blueprint_Install = c as Blueprint_Install;
			if (blueprint_Install != null)
			{
				return this.InstallJob(pawn, blueprint_Install);
			}
			bool flag = false;
			ThingDefCountClass thingDefCountClass = null;
			List<ThingDefCountClass> list = c.MaterialsNeeded();
			int count = list.Count;
			int i = 0;
			while (i < count)
			{
				ThingDefCountClass need = list[i];
				if (!pawn.Map.itemAvailability.ThingsAvailableAnywhere(need, pawn))
				{
					flag = true;
					thingDefCountClass = need;
					break;
				}
				Thing foundRes = GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForDef(need.thingDef), PathEndMode.ClosestTouch, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false), 9999f, (Thing r) => WorkGiver_ConstructDeliverResources.ResourceValidator(pawn, need, r), null, 0, -1, false, RegionType.Set_Passable, false);
				if (foundRes != null)
				{
					int resTotalAvailable;
					this.FindAvailableNearbyResources(foundRes, pawn, out resTotalAvailable);
					int num;
					Job job;
					HashSet<Thing> hashSet = this.FindNearbyNeeders(pawn, need, c, resTotalAvailable, canRemoveExistingFloorUnderNearbyNeeders, out num, out job);
					if (job != null)
					{
						return job;
					}
					hashSet.Add((Thing)c);
					Thing thing = hashSet.MinBy((Thing nee) => IntVec3Utility.ManhattanDistanceFlat(foundRes.Position, nee.Position));
					hashSet.Remove(thing);
					int num2 = 0;
					int j = 0;
					do
					{
						num2 += WorkGiver_ConstructDeliverResources.resourcesAvailable[j].stackCount;
						j++;
					}
					while (num2 < num && j < WorkGiver_ConstructDeliverResources.resourcesAvailable.Count);
					WorkGiver_ConstructDeliverResources.resourcesAvailable.RemoveRange(j, WorkGiver_ConstructDeliverResources.resourcesAvailable.Count - j);
					WorkGiver_ConstructDeliverResources.resourcesAvailable.Remove(foundRes);
					Job job2 = JobMaker.MakeJob(JobDefOf.HaulToContainer);
					job2.targetA = foundRes;
					job2.targetQueueA = new List<LocalTargetInfo>();
					for (j = 0; j < WorkGiver_ConstructDeliverResources.resourcesAvailable.Count; j++)
					{
						job2.targetQueueA.Add(WorkGiver_ConstructDeliverResources.resourcesAvailable[j]);
					}
					job2.targetB = thing;
					if (hashSet.Count > 0)
					{
						job2.targetQueueB = new List<LocalTargetInfo>();
						foreach (Thing t in hashSet)
						{
							job2.targetQueueB.Add(t);
						}
					}
					job2.targetC = (Thing)c;
					job2.count = num;
					job2.haulMode = HaulMode.ToContainer;
					return job2;
				}
				else
				{
					flag = true;
					thingDefCountClass = need;
					i++;
				}
			}
			if (flag)
			{
				JobFailReason.Is(string.Format("{0}: {1}", WorkGiver_ConstructDeliverResources.MissingMaterialsTranslated, thingDefCountClass.thingDef.label), null);
			}
			return null;
		}

		// Token: 0x0600371A RID: 14106 RVA: 0x00137C44 File Offset: 0x00135E44
		private void FindAvailableNearbyResources(Thing firstFoundResource, Pawn pawn, out int resTotalAvailable)
		{
			int num = Mathf.Min(firstFoundResource.def.stackLimit, pawn.carryTracker.MaxStackSpaceEver(firstFoundResource.def));
			resTotalAvailable = 0;
			WorkGiver_ConstructDeliverResources.resourcesAvailable.Clear();
			WorkGiver_ConstructDeliverResources.resourcesAvailable.Add(firstFoundResource);
			resTotalAvailable += firstFoundResource.stackCount;
			if (resTotalAvailable < num)
			{
				foreach (Thing thing in GenRadial.RadialDistinctThingsAround(firstFoundResource.Position, firstFoundResource.Map, 5f, false))
				{
					if (resTotalAvailable >= num)
					{
						break;
					}
					if (thing.def == firstFoundResource.def && GenAI.CanUseItemForWork(pawn, thing))
					{
						WorkGiver_ConstructDeliverResources.resourcesAvailable.Add(thing);
						resTotalAvailable += thing.stackCount;
					}
				}
			}
		}

		// Token: 0x0600371B RID: 14107 RVA: 0x00137D1C File Offset: 0x00135F1C
		private HashSet<Thing> FindNearbyNeeders(Pawn pawn, ThingDefCountClass need, IConstructible c, int resTotalAvailable, bool canRemoveExistingFloorUnderNearbyNeeders, out int neededTotal, out Job jobToMakeNeederAvailable)
		{
			neededTotal = need.count;
			HashSet<Thing> hashSet = new HashSet<Thing>();
			Thing thing = (Thing)c;
			foreach (Thing thing2 in GenRadial.RadialDistinctThingsAround(thing.Position, thing.Map, 8f, true))
			{
				if (neededTotal >= resTotalAvailable)
				{
					break;
				}
				if (this.IsNewValidNearbyNeeder(thing2, hashSet, c, pawn))
				{
					Blueprint blueprint = thing2 as Blueprint;
					if (blueprint == null || !WorkGiver_ConstructDeliverResources.ShouldRemoveExistingFloorFirst(pawn, blueprint))
					{
						int num = GenConstruct.AmountNeededByOf((IConstructible)thing2, need.thingDef);
						if (num > 0)
						{
							hashSet.Add(thing2);
							neededTotal += num;
						}
					}
				}
			}
			Blueprint blueprint2 = c as Blueprint;
			if (blueprint2 != null && blueprint2.def.entityDefToBuild is TerrainDef && canRemoveExistingFloorUnderNearbyNeeders && neededTotal < resTotalAvailable)
			{
				foreach (Thing thing3 in GenRadial.RadialDistinctThingsAround(thing.Position, thing.Map, 3f, false))
				{
					if (this.IsNewValidNearbyNeeder(thing3, hashSet, c, pawn))
					{
						Blueprint blueprint3 = thing3 as Blueprint;
						if (blueprint3 != null)
						{
							Job job = this.RemoveExistingFloorJob(pawn, blueprint3);
							if (job != null)
							{
								jobToMakeNeederAvailable = job;
								return hashSet;
							}
						}
					}
				}
			}
			jobToMakeNeederAvailable = null;
			return hashSet;
		}

		// Token: 0x0600371C RID: 14108 RVA: 0x00137E90 File Offset: 0x00136090
		private bool IsNewValidNearbyNeeder(Thing t, HashSet<Thing> nearbyNeeders, IConstructible constructible, Pawn pawn)
		{
			return t is IConstructible && t != constructible && !(t is Blueprint_Install) && t.Faction == pawn.Faction && !t.IsForbidden(pawn) && !nearbyNeeders.Contains(t) && GenConstruct.CanConstruct(t, pawn, false, false);
		}

		// Token: 0x0600371D RID: 14109 RVA: 0x00137EE2 File Offset: 0x001360E2
		protected static bool ShouldRemoveExistingFloorFirst(Pawn pawn, Blueprint blue)
		{
			return blue.def.entityDefToBuild is TerrainDef && pawn.Map.terrainGrid.CanRemoveTopLayerAt(blue.Position);
		}

		// Token: 0x0600371E RID: 14110 RVA: 0x00137F14 File Offset: 0x00136114
		protected Job RemoveExistingFloorJob(Pawn pawn, Blueprint blue)
		{
			if (!WorkGiver_ConstructDeliverResources.ShouldRemoveExistingFloorFirst(pawn, blue))
			{
				return null;
			}
			if (!pawn.CanReserve(blue.Position, 1, -1, ReservationLayerDefOf.Floor, false))
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.RemoveFloor, blue.Position);
			job.ignoreDesignations = true;
			return job;
		}

		// Token: 0x0600371F RID: 14111 RVA: 0x00137F68 File Offset: 0x00136168
		private Job InstallJob(Pawn pawn, Blueprint_Install install)
		{
			Thing miniToInstallOrBuildingToReinstall = install.MiniToInstallOrBuildingToReinstall;
			IThingHolder parentHolder = miniToInstallOrBuildingToReinstall.ParentHolder;
			Pawn_CarryTracker pawn_CarryTracker;
			if (parentHolder != null && (pawn_CarryTracker = (parentHolder as Pawn_CarryTracker)) != null)
			{
				JobFailReason.Is("BeingCarriedBy".Translate(pawn_CarryTracker.pawn), null);
				return null;
			}
			if (miniToInstallOrBuildingToReinstall.IsForbidden(pawn))
			{
				JobFailReason.Is(WorkGiver_ConstructDeliverResources.ForbiddenLowerTranslated, null);
				return null;
			}
			if (!pawn.CanReach(miniToInstallOrBuildingToReinstall, PathEndMode.ClosestTouch, pawn.NormalMaxDanger(), false, false, TraverseMode.ByPawn))
			{
				JobFailReason.Is(WorkGiver_ConstructDeliverResources.NoPathTranslated, null);
				return null;
			}
			if (!pawn.CanReserve(miniToInstallOrBuildingToReinstall, 1, -1, null, false))
			{
				Pawn pawn2 = pawn.Map.reservationManager.FirstRespectedReserver(miniToInstallOrBuildingToReinstall, pawn);
				if (pawn2 != null)
				{
					JobFailReason.Is("ReservedBy".Translate(pawn2.LabelShort, pawn2), null);
				}
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.HaulToContainer);
			job.targetA = miniToInstallOrBuildingToReinstall;
			job.targetB = install;
			job.count = 1;
			job.haulMode = HaulMode.ToContainer;
			return job;
		}

		// Token: 0x04001F06 RID: 7942
		private static List<Thing> resourcesAvailable = new List<Thing>();

		// Token: 0x04001F07 RID: 7943
		private const float MultiPickupRadius = 5f;

		// Token: 0x04001F08 RID: 7944
		private const float NearbyConstructScanRadius = 8f;

		// Token: 0x04001F09 RID: 7945
		private static string MissingMaterialsTranslated;

		// Token: 0x04001F0A RID: 7946
		private static string ForbiddenLowerTranslated;

		// Token: 0x04001F0B RID: 7947
		private static string NoPathTranslated;
	}
}
