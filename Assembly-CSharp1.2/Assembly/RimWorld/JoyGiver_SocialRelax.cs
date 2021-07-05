﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D04 RID: 3332
	public class JoyGiver_SocialRelax : JoyGiver
	{
		// Token: 0x06004C7F RID: 19583 RVA: 0x0003650F File Offset: 0x0003470F
		public override Job TryGiveJob(Pawn pawn)
		{
			return this.TryGiveJobInt(pawn, null);
		}

		// Token: 0x06004C80 RID: 19584 RVA: 0x001AA39C File Offset: 0x001A859C
		public override Job TryGiveJobInGatheringArea(Pawn pawn, IntVec3 gatheringSpot)
		{
			return this.TryGiveJobInt(pawn, (CompGatherSpot x) => GatheringsUtility.InGatheringArea(x.parent.Position, gatheringSpot, pawn.Map));
		}

		// Token: 0x06004C81 RID: 19585 RVA: 0x001AA3D8 File Offset: 0x001A85D8
		private Job TryGiveJobInt(Pawn pawn, Predicate<CompGatherSpot> gatherSpotValidator)
		{
			if (pawn.Map.gatherSpotLister.activeSpots.Count == 0)
			{
				return null;
			}
			JoyGiver_SocialRelax.workingSpots.Clear();
			for (int i = 0; i < pawn.Map.gatherSpotLister.activeSpots.Count; i++)
			{
				JoyGiver_SocialRelax.workingSpots.Add(pawn.Map.gatherSpotLister.activeSpots[i]);
			}
			CompGatherSpot compGatherSpot;
			while (JoyGiver_SocialRelax.workingSpots.TryRandomElement(out compGatherSpot))
			{
				JoyGiver_SocialRelax.workingSpots.Remove(compGatherSpot);
				if (!compGatherSpot.parent.IsForbidden(pawn) && pawn.CanReach(compGatherSpot.parent, PathEndMode.Touch, Danger.None, false, TraverseMode.ByPawn) && compGatherSpot.parent.IsSociallyProper(pawn) && compGatherSpot.parent.IsPoliticallyProper(pawn) && (gatherSpotValidator == null || gatherSpotValidator(compGatherSpot)))
				{
					Job job;
					Thing t2;
					if (compGatherSpot.parent.def.surfaceType == SurfaceType.Eat)
					{
						Thing t;
						if (!JoyGiver_SocialRelax.TryFindChairBesideTable(compGatherSpot.parent, pawn, out t))
						{
							return null;
						}
						job = JobMaker.MakeJob(this.def.jobDef, compGatherSpot.parent, t);
					}
					else if (JoyGiver_SocialRelax.TryFindChairNear(compGatherSpot.parent.Position, pawn, out t2))
					{
						job = JobMaker.MakeJob(this.def.jobDef, compGatherSpot.parent, t2);
					}
					else
					{
						IntVec3 c;
						if (!JoyGiver_SocialRelax.TryFindSitSpotOnGroundNear(compGatherSpot.parent.Position, pawn, out c))
						{
							return null;
						}
						job = JobMaker.MakeJob(this.def.jobDef, compGatherSpot.parent, c);
					}
					Thing thing;
					if (pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation) && JoyGiver_SocialRelax.TryFindIngestibleToNurse(compGatherSpot.parent.Position, pawn, out thing))
					{
						job.targetC = thing;
						job.count = Mathf.Min(thing.stackCount, thing.def.ingestible.maxNumToIngestAtOnce);
					}
					return job;
				}
			}
			return null;
		}

		// Token: 0x06004C82 RID: 19586 RVA: 0x001AA5D0 File Offset: 0x001A87D0
		private static bool TryFindIngestibleToNurse(IntVec3 center, Pawn ingester, out Thing ingestible)
		{
			if (ingester.IsTeetotaler())
			{
				ingestible = null;
				return false;
			}
			if (ingester.drugs == null)
			{
				ingestible = null;
				return false;
			}
			JoyGiver_SocialRelax.nurseableDrugs.Clear();
			DrugPolicy currentPolicy = ingester.drugs.CurrentPolicy;
			for (int i = 0; i < currentPolicy.Count; i++)
			{
				if (currentPolicy[i].allowedForJoy && currentPolicy[i].drug.ingestible.nurseable)
				{
					JoyGiver_SocialRelax.nurseableDrugs.Add(currentPolicy[i].drug);
				}
			}
			JoyGiver_SocialRelax.nurseableDrugs.Shuffle<ThingDef>();
			Predicate<Thing> <>9__0;
			for (int j = 0; j < JoyGiver_SocialRelax.nurseableDrugs.Count; j++)
			{
				List<Thing> list = ingester.Map.listerThings.ThingsOfDef(JoyGiver_SocialRelax.nurseableDrugs[j]);
				if (list.Count > 0)
				{
					Predicate<Thing> predicate;
					if ((predicate = <>9__0) == null)
					{
						predicate = (<>9__0 = ((Thing t) => ingester.CanReserve(t, 1, -1, null, false) && !t.IsForbidden(ingester)));
					}
					Predicate<Thing> validator = predicate;
					ingestible = GenClosest.ClosestThing_Global_Reachable(center, ingester.Map, list, PathEndMode.OnCell, TraverseParms.For(ingester, Danger.Deadly, TraverseMode.ByPawn, false), 40f, validator, null);
					if (ingestible != null)
					{
						return true;
					}
				}
			}
			ingestible = null;
			return false;
		}

		// Token: 0x06004C83 RID: 19587 RVA: 0x001AA720 File Offset: 0x001A8920
		private static bool TryFindChairBesideTable(Thing table, Pawn sitter, out Thing chair)
		{
			for (int i = 0; i < 30; i++)
			{
				Building edifice = table.RandomAdjacentCellCardinal().GetEdifice(table.Map);
				if (edifice != null && edifice.def.building.isSittable && sitter.CanReserve(edifice, 1, -1, null, false))
				{
					chair = edifice;
					return true;
				}
			}
			chair = null;
			return false;
		}

		// Token: 0x06004C84 RID: 19588 RVA: 0x001AA77C File Offset: 0x001A897C
		private static bool TryFindChairNear(IntVec3 center, Pawn sitter, out Thing chair)
		{
			for (int i = 0; i < JoyGiver_SocialRelax.RadialPatternMiddleOutward.Count; i++)
			{
				Building edifice = (center + JoyGiver_SocialRelax.RadialPatternMiddleOutward[i]).GetEdifice(sitter.Map);
				if (edifice != null && edifice.def.building.isSittable && sitter.CanReserve(edifice, 1, -1, null, false) && !edifice.IsForbidden(sitter) && GenSight.LineOfSight(center, edifice.Position, sitter.Map, true, null, 0, 0))
				{
					chair = edifice;
					return true;
				}
			}
			chair = null;
			return false;
		}

		// Token: 0x06004C85 RID: 19589 RVA: 0x001AA80C File Offset: 0x001A8A0C
		private static bool TryFindSitSpotOnGroundNear(IntVec3 center, Pawn sitter, out IntVec3 result)
		{
			for (int i = 0; i < 30; i++)
			{
				IntVec3 intVec = center + GenRadial.RadialPattern[Rand.Range(1, JoyGiver_SocialRelax.NumRadiusCells)];
				if (sitter.CanReserveAndReach(intVec, PathEndMode.OnCell, Danger.None, 1, -1, null, false) && intVec.GetEdifice(sitter.Map) == null && GenSight.LineOfSight(center, intVec, sitter.Map, true, null, 0, 0))
				{
					result = intVec;
					return true;
				}
			}
			result = IntVec3.Invalid;
			return false;
		}

		// Token: 0x0400325A RID: 12890
		private static List<CompGatherSpot> workingSpots = new List<CompGatherSpot>();

		// Token: 0x0400325B RID: 12891
		private const float GatherRadius = 3.9f;

		// Token: 0x0400325C RID: 12892
		private static readonly int NumRadiusCells = GenRadial.NumCellsInRadius(3.9f);

		// Token: 0x0400325D RID: 12893
		private static readonly List<IntVec3> RadialPatternMiddleOutward = (from c in GenRadial.RadialPattern.Take(JoyGiver_SocialRelax.NumRadiusCells)
		orderby Mathf.Abs((c - IntVec3.Zero).LengthHorizontal - 1.95f)
		select c).ToList<IntVec3>();

		// Token: 0x0400325E RID: 12894
		private static List<ThingDef> nurseableDrugs = new List<ThingDef>();
	}
}
