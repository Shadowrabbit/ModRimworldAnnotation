using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007EA RID: 2026
	public static class SickPawnVisitUtility
	{
		// Token: 0x06003644 RID: 13892 RVA: 0x00133554 File Offset: 0x00131754
		public static Pawn FindRandomSickPawn(Pawn pawn, JoyCategory maxPatientJoy)
		{
			Pawn result;
			if (!(from x in pawn.Map.mapPawns.FreeColonistsSpawned
			where SickPawnVisitUtility.CanVisit(pawn, x, maxPatientJoy)
			select x).TryRandomElementByWeight((Pawn x) => SickPawnVisitUtility.VisitChanceScore(pawn, x), out result))
			{
				return null;
			}
			return result;
		}

		// Token: 0x06003645 RID: 13893 RVA: 0x001335B4 File Offset: 0x001317B4
		public static bool CanVisit(Pawn pawn, Pawn sick, JoyCategory maxPatientJoy)
		{
			return sick.IsColonist && pawn.RaceProps.Humanlike && !sick.Dead && pawn != sick && sick.InBed() && sick.Awake() && !sick.IsForbidden(pawn) && sick.needs.joy != null && sick.needs.joy.CurCategory <= maxPatientJoy && InteractionUtility.CanReceiveInteraction(sick, null) && !sick.needs.food.Starving && sick.needs.rest.CurLevel > 0.33f && pawn.CanReserveAndReach(sick, PathEndMode.InteractionCell, Danger.None, 1, -1, null, false) && !SickPawnVisitUtility.AboutToRecover(sick);
		}

		// Token: 0x06003646 RID: 13894 RVA: 0x0013367C File Offset: 0x0013187C
		public static Thing FindChair(Pawn forPawn, Pawn nearPawn)
		{
			Predicate<Thing> validator = (Thing x) => x.def.building.isSittable && !x.IsForbidden(forPawn) && GenSight.LineOfSight(x.Position, nearPawn.Position, nearPawn.Map, false, null, 0, 0) && forPawn.CanReserve(x, 1, -1, null, false) && (!x.def.rotatable || GenGeo.AngleDifferenceBetween(x.Rotation.AsAngle, (nearPawn.Position - x.Position).AngleFlat) <= 95f);
			return GenClosest.ClosestThingReachable(nearPawn.Position, nearPawn.Map, ThingRequest.ForGroup(ThingRequestGroup.BuildingArtificial), PathEndMode.OnCell, TraverseParms.For(forPawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false), 2.2f, validator, null, 0, 5, false, RegionType.Set_Passable, false);
		}

		// Token: 0x06003647 RID: 13895 RVA: 0x001336EC File Offset: 0x001318EC
		private static bool AboutToRecover(Pawn pawn)
		{
			if (pawn.Downed)
			{
				return false;
			}
			if (!HealthAIUtility.ShouldSeekMedicalRestUrgent(pawn) && !HealthAIUtility.ShouldSeekMedicalRest(pawn))
			{
				return true;
			}
			if (pawn.health.hediffSet.HasImmunizableNotImmuneHediff())
			{
				return false;
			}
			float num = 0f;
			List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				Hediff_Injury hediff_Injury = hediffs[i] as Hediff_Injury;
				if (hediff_Injury != null && (hediff_Injury.CanHealFromTending() || hediff_Injury.CanHealNaturally() || hediff_Injury.Bleeding))
				{
					num += hediff_Injury.Severity;
				}
			}
			return num < 8f * pawn.RaceProps.baseHealthScale;
		}

		// Token: 0x06003648 RID: 13896 RVA: 0x00133798 File Offset: 0x00131998
		private static float VisitChanceScore(Pawn pawn, Pawn sick)
		{
			float num = GenMath.LerpDouble(-100f, 100f, 0.05f, 2f, (float)pawn.relations.OpinionOf(sick));
			float lengthHorizontal = (pawn.Position - sick.Position).LengthHorizontal;
			float num2 = Mathf.Clamp(GenMath.LerpDouble(0f, 150f, 1f, 0.2f, lengthHorizontal), 0.2f, 1f);
			return num * num2;
		}
	}
}
