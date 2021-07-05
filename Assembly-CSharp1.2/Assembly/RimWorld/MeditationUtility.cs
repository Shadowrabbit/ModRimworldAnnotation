using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D16 RID: 3350
	public class MeditationUtility
	{
		// Token: 0x06004CBE RID: 19646 RVA: 0x001ABA60 File Offset: 0x001A9C60
		public static Job GetMeditationJob(Pawn pawn, bool forJoy = false)
		{
			MeditationSpotAndFocus meditationSpotAndFocus = MeditationUtility.FindMeditationSpot(pawn);
			if (meditationSpotAndFocus.IsValid)
			{
				Building_Throne t;
				Job job;
				if ((t = (meditationSpotAndFocus.focus.Thing as Building_Throne)) != null)
				{
					job = JobMaker.MakeJob(JobDefOf.Reign, t, null, t);
				}
				else
				{
					job = JobMaker.MakeJob(JobDefOf.Meditate, meditationSpotAndFocus.spot, null, meditationSpotAndFocus.focus);
				}
				job.ignoreJoyTimeAssignment = !forJoy;
				return job;
			}
			return null;
		}

		// Token: 0x06004CBF RID: 19647 RVA: 0x001ABADC File Offset: 0x001A9CDC
		public static MeditationSpotAndFocus FindMeditationSpot(Pawn pawn)
		{
			float num = float.MinValue;
			LocalTargetInfo spot = LocalTargetInfo.Invalid;
			LocalTargetInfo focus = LocalTargetInfo.Invalid;
			if (!ModLister.RoyaltyInstalled)
			{
				Log.ErrorOnce("Psyfocus meditation is a Royalty-specific game system. If you want to use this code please check ModLister.RoyaltyInstalled before calling it.", 657324, false);
				return new MeditationSpotAndFocus(spot, focus);
			}
			Room ownedRoom = pawn.ownership.OwnedRoom;
			foreach (LocalTargetInfo localTargetInfo in MeditationUtility.AllMeditationSpotCandidates(pawn, true))
			{
				if (MeditationUtility.SafeEnvironmentalConditions(pawn, localTargetInfo.Cell, pawn.Map))
				{
					LocalTargetInfo localTargetInfo2 = (localTargetInfo.Thing is Building_Throne) ? localTargetInfo.Thing : MeditationUtility.BestFocusAt(localTargetInfo, pawn);
					float num2 = 1f / Mathf.Max((float)localTargetInfo.Cell.DistanceToSquared(pawn.Position), 0.1f);
					if (pawn.HasPsylink && localTargetInfo2.IsValid)
					{
						num2 += localTargetInfo2.Thing.GetStatValueForPawn(StatDefOf.MeditationFocusStrength, pawn, true) * 100f;
					}
					Room room = localTargetInfo.Cell.GetRoom(pawn.Map, RegionType.Set_Passable);
					if (room != null && ownedRoom == room)
					{
						num2 += 1f;
					}
					Building building;
					if (localTargetInfo.Thing != null && (building = (localTargetInfo.Thing as Building)) != null && building.GetAssignedPawn() == pawn)
					{
						num2 += (float)((building.def == ThingDefOf.MeditationSpot) ? 200 : 100);
					}
					if (!localTargetInfo.Cell.Standable(pawn.Map))
					{
						num2 = float.NegativeInfinity;
					}
					if (num2 > num)
					{
						spot = localTargetInfo;
						focus = localTargetInfo2;
						num = num2;
					}
				}
			}
			return new MeditationSpotAndFocus(spot, focus);
		}

		// Token: 0x06004CC0 RID: 19648 RVA: 0x00036684 File Offset: 0x00034884
		public static IEnumerable<LocalTargetInfo> AllMeditationSpotCandidates(Pawn pawn, bool allowFallbackSpots = true)
		{
			bool flag = false;
			if (pawn.royalty != null && pawn.royalty.AllTitlesInEffectForReading.Count > 0 && !pawn.IsPrisonerOfColony)
			{
				Building_Throne building_Throne = RoyalTitleUtility.FindBestUsableThrone(pawn);
				if (building_Throne != null)
				{
					yield return building_Throne;
					flag = true;
				}
			}
			if (!pawn.IsPrisonerOfColony)
			{
				IEnumerable<Building> source = pawn.Map.listerBuildings.AllBuildingsColonistOfDef(ThingDefOf.MeditationSpot);
				Func<Building, bool> <>9__2;
				Func<Building, bool> predicate;
				if ((predicate = <>9__2) == null)
				{
					predicate = (<>9__2 = delegate(Building s)
					{
						if (s.IsForbidden(pawn) || !s.Position.Standable(s.Map))
						{
							return false;
						}
						if (s.GetAssignedPawn() != null && s.GetAssignedPawn() != pawn)
						{
							return false;
						}
						Room room3 = s.GetRoom(RegionType.Set_Passable);
						return (room3 == null || MeditationUtility.CanUseRoomToMeditate(room3, pawn)) && pawn.CanReserveAndReach(s, PathEndMode.OnCell, pawn.NormalMaxDanger(), 1, -1, null, false);
					});
				}
				foreach (Building t in source.Where(predicate))
				{
					yield return t;
					flag = true;
				}
				IEnumerator<Building> enumerator = null;
			}
			if (flag || !allowFallbackSpots)
			{
				yield break;
			}
			List<Thing> list = pawn.Map.listerThings.ThingsInGroup(ThingRequestGroup.MeditationFocus);
			foreach (Thing thing in list)
			{
				if (thing.def != ThingDefOf.Wall)
				{
					Room room = thing.GetRoom(RegionType.Set_Passable);
					if ((room == null || MeditationUtility.CanUseRoomToMeditate(room, pawn)) && thing.GetStatValueForPawn(StatDefOf.MeditationFocusStrength, pawn, true) > 0f)
					{
						LocalTargetInfo localTargetInfo = MeditationUtility.MeditationSpotForFocus(thing, pawn, null);
						if (localTargetInfo.IsValid)
						{
							yield return localTargetInfo;
						}
					}
				}
			}
			List<Thing>.Enumerator enumerator2 = default(List<Thing>.Enumerator);
			Building_Bed bed = pawn.ownership.OwnedBed;
			Building_Bed building_Bed = bed;
			Room room2 = (building_Bed != null) ? building_Bed.GetRoom(RegionType.Set_Passable) : null;
			IntVec3 c2;
			if (room2 != null && !room2.PsychologicallyOutdoors && pawn.CanReserveAndReach(bed, PathEndMode.OnCell, pawn.NormalMaxDanger(), 1, -1, null, false))
			{
				foreach (LocalTargetInfo localTargetInfo2 in MeditationUtility.FocusSpotsInTheRoom(pawn, room2))
				{
					yield return localTargetInfo2;
				}
				IEnumerator<LocalTargetInfo> enumerator3 = null;
				c2 = RCellFinder.RandomWanderDestFor(pawn, bed.Position, MeditationUtility.WanderRadius, (Pawn p, IntVec3 c, IntVec3 r) => c.Standable(p.Map) && c.GetDoor(p.Map) == null && WanderRoomUtility.IsValidWanderDest(p, c, r), pawn.NormalMaxDanger());
				if (c2.IsValid)
				{
					yield return c2;
				}
			}
			if (pawn.IsPrisonerOfColony)
			{
				yield break;
			}
			IntVec3 colonyWanderRoot = WanderUtility.GetColonyWanderRoot(pawn);
			c2 = RCellFinder.RandomWanderDestFor(pawn, colonyWanderRoot, MeditationUtility.WanderRadius, delegate(Pawn p, IntVec3 c, IntVec3 r)
			{
				if (!c.Standable(p.Map) || c.GetDoor(p.Map) != null || !p.CanReserveAndReach(c, PathEndMode.OnCell, p.NormalMaxDanger(), 1, -1, null, false))
				{
					return false;
				}
				Room room3 = c.GetRoom(p.Map, RegionType.Set_Passable);
				return room3 == null || MeditationUtility.CanUseRoomToMeditate(room3, pawn);
			}, pawn.NormalMaxDanger());
			if (c2.IsValid)
			{
				yield return c2;
			}
			yield break;
			yield break;
		}

		// Token: 0x06004CC1 RID: 19649 RVA: 0x0003669B File Offset: 0x0003489B
		public static bool SafeEnvironmentalConditions(Pawn pawn, IntVec3 cell, Map map)
		{
			return (!map.gameConditionManager.ConditionIsActive(GameConditionDefOf.ToxicFallout) || cell.Roofed(map)) && cell.GetDangerFor(pawn, map) == Danger.None;
		}

		// Token: 0x06004CC2 RID: 19650 RVA: 0x001ABCA4 File Offset: 0x001A9EA4
		public static bool CanMeditateNow(Pawn pawn)
		{
			if (pawn.needs.rest != null && pawn.needs.rest.CurCategory >= RestCategory.VeryTired)
			{
				return false;
			}
			if (pawn.needs.food.Starving)
			{
				return false;
			}
			if (!pawn.Awake())
			{
				return false;
			}
			if (pawn.health.hediffSet.BleedRateTotal <= 0f)
			{
				if (HealthAIUtility.ShouldSeekMedicalRest(pawn))
				{
					Pawn_TimetableTracker timetable = pawn.timetable;
					if (((timetable != null) ? timetable.CurrentAssignment : null) != TimeAssignmentDefOf.Meditate)
					{
						return false;
					}
				}
				if (!HealthAIUtility.ShouldSeekMedicalRestUrgent(pawn))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004CC3 RID: 19651 RVA: 0x000366C8 File Offset: 0x000348C8
		public static bool CanOnlyMeditateInBed(Pawn pawn)
		{
			return pawn.Downed;
		}

		// Token: 0x06004CC4 RID: 19652 RVA: 0x000366D0 File Offset: 0x000348D0
		public static bool ShouldMeditateInBed(Pawn pawn)
		{
			return pawn.Downed || pawn.health.hediffSet.AnyHediffMakesSickThought;
		}

		// Token: 0x06004CC5 RID: 19653 RVA: 0x001ABD34 File Offset: 0x001A9F34
		public static LocalTargetInfo BestFocusAt(LocalTargetInfo spot, Pawn pawn)
		{
			float num = 0f;
			LocalTargetInfo result = LocalTargetInfo.Invalid;
			foreach (Thing thing in GenRadial.RadialDistinctThingsAround(spot.Cell, pawn.MapHeld, MeditationUtility.FocusObjectSearchRadius, false))
			{
				if (GenSight.LineOfSightToThing(spot.Cell, thing, pawn.Map, false, null) && !(thing is Building_Throne))
				{
					CompMeditationFocus compMeditationFocus = thing.TryGetComp<CompMeditationFocus>();
					if (compMeditationFocus != null && compMeditationFocus.CanPawnUse(pawn))
					{
						float statValueForPawn = thing.GetStatValueForPawn(StatDefOf.MeditationFocusStrength, pawn, true);
						if (statValueForPawn > num)
						{
							result = thing;
							num = statValueForPawn;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06004CC6 RID: 19654 RVA: 0x000366F1 File Offset: 0x000348F1
		public static IEnumerable<LocalTargetInfo> FocusSpotsInTheRoom(Pawn pawn, Room r)
		{
			foreach (Thing thing in r.ContainedAndAdjacentThings)
			{
				CompMeditationFocus compMeditationFocus = thing.TryGetComp<CompMeditationFocus>();
				if (compMeditationFocus != null && compMeditationFocus.CanPawnUse(pawn) && !(thing is Building_Throne) && thing.GetStatValueForPawn(StatDefOf.MeditationFocusStrength, pawn, true) > 0f)
				{
					LocalTargetInfo localTargetInfo = MeditationUtility.MeditationSpotForFocus(thing, pawn, new Func<IntVec3, bool>(r.ContainsCell));
					if (localTargetInfo.IsValid)
					{
						yield return localTargetInfo;
					}
				}
			}
			List<Thing>.Enumerator enumerator = default(List<Thing>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06004CC7 RID: 19655 RVA: 0x001ABDEC File Offset: 0x001A9FEC
		public static LocalTargetInfo MeditationSpotForFocus(Thing t, Pawn p, Func<IntVec3, bool> validator = null)
		{
			return (from cell in t.OccupiedRect().ExpandedBy(2).AdjacentCellsCardinal
			where (validator == null || validator(cell)) && !cell.IsForbidden(p) && p.CanReserveAndReach(cell, PathEndMode.OnCell, p.NormalMaxDanger(), 1, -1, null, false) && cell.Standable(p.Map)
			select cell).RandomElementWithFallback(IntVec3.Invalid);
		}

		// Token: 0x06004CC8 RID: 19656 RVA: 0x00036708 File Offset: 0x00034908
		public static IEnumerable<MeditationFocusDef> FocusTypesAvailableForPawn(Pawn pawn)
		{
			int num;
			for (int i = 0; i < DefDatabase<MeditationFocusDef>.AllDefsListForReading.Count; i = num + 1)
			{
				MeditationFocusDef meditationFocusDef = DefDatabase<MeditationFocusDef>.AllDefsListForReading[i];
				if (meditationFocusDef.CanPawnUse(pawn))
				{
					yield return meditationFocusDef;
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x06004CC9 RID: 19657 RVA: 0x00036718 File Offset: 0x00034918
		public static string FocusTypesAvailableForPawnString(Pawn pawn)
		{
			return (from f in MeditationUtility.FocusTypesAvailableForPawn(pawn)
			select f.label).ToCommaList(false);
		}

		// Token: 0x06004CCA RID: 19658 RVA: 0x0003674A File Offset: 0x0003494A
		public static IEnumerable<Dialog_InfoCard.Hyperlink> FocusObjectsForPawnHyperlinks(Pawn pawn)
		{
			int num;
			for (int i = 0; i < DefDatabase<MeditationFocusDef>.AllDefsListForReading.Count; i = num + 1)
			{
				MeditationFocusDef meditationFocusDef = DefDatabase<MeditationFocusDef>.AllDefsListForReading[i];
				if (meditationFocusDef.CanPawnUse(pawn))
				{
					if (!MeditationUtility.focusObjectHyperlinksPerTypeCache.ContainsKey(meditationFocusDef))
					{
						List<Dialog_InfoCard.Hyperlink> list2 = new List<Dialog_InfoCard.Hyperlink>();
						foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefsListForReading)
						{
							CompProperties_MeditationFocus compProperties = thingDef.GetCompProperties<CompProperties_MeditationFocus>();
							if (compProperties != null && compProperties.focusTypes.Contains(meditationFocusDef))
							{
								list2.Add(new Dialog_InfoCard.Hyperlink(thingDef, -1));
							}
						}
						MeditationUtility.focusObjectHyperlinksPerTypeCache[meditationFocusDef] = list2;
					}
					List<Dialog_InfoCard.Hyperlink> list = MeditationUtility.focusObjectHyperlinksPerTypeCache[meditationFocusDef];
					for (int j = 0; j < list.Count; j = num + 1)
					{
						yield return list[j];
						num = j;
					}
					list = null;
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x06004CCB RID: 19659 RVA: 0x001ABE44 File Offset: 0x001AA044
		public static string FocusTypeAvailableExplanation(Pawn pawn)
		{
			string text = "";
			for (int i = 0; i < DefDatabase<MeditationFocusDef>.AllDefsListForReading.Count; i++)
			{
				MeditationFocusDef meditationFocusDef = DefDatabase<MeditationFocusDef>.AllDefsListForReading[i];
				if (meditationFocusDef.CanPawnUse(pawn))
				{
					text = string.Concat(new string[]
					{
						text,
						"MeditationFocusCanUse".Translate(meditationFocusDef.label).RawText,
						":\n",
						meditationFocusDef.EnablingThingsExplanation(pawn),
						"\n\n"
					});
					if (!MeditationUtility.focusObjectsPerTypeCache.ContainsKey(meditationFocusDef))
					{
						List<string> list = new List<string>();
						foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefsListForReading)
						{
							CompProperties_MeditationFocus compProperties = thingDef.GetCompProperties<CompProperties_MeditationFocus>();
							if (compProperties != null && compProperties.focusTypes.Contains(meditationFocusDef))
							{
								list.AddDistinct(thingDef.label);
							}
						}
						MeditationUtility.focusObjectsPerTypeCache[meditationFocusDef] = list.ToLineList("  - ", true);
					}
					text += "MeditationFocusObjects".Translate(meditationFocusDef.label).CapitalizeFirst() + ":\n" + MeditationUtility.focusObjectsPerTypeCache[meditationFocusDef] + "\n\n";
				}
			}
			return text;
		}

		// Token: 0x06004CCC RID: 19660 RVA: 0x001ABFB8 File Offset: 0x001AA1B8
		public static void DrawMeditationSpotOverlay(IntVec3 center, Map map)
		{
			GenDraw.DrawRadiusRing(center, MeditationUtility.FocusObjectSearchRadius);
			foreach (Thing thing in GenRadial.RadialDistinctThingsAround(center, map, MeditationUtility.FocusObjectSearchRadius, false))
			{
				if (!(thing is Building_Throne) && thing.def != ThingDefOf.Wall && thing.TryGetComp<CompMeditationFocus>() != null && GenSight.LineOfSight(center, thing.Position, map, false, null, 0, 0))
				{
					GenDraw.DrawLineBetween(center.ToVector3() + new Vector3(0.5f, 0f, 0.5f), thing.TrueCenter(), SimpleColor.White);
				}
			}
		}

		// Token: 0x06004CCD RID: 19661 RVA: 0x0003675A File Offset: 0x0003495A
		public static bool CanUseRoomToMeditate(Room r, Pawn p)
		{
			return (r.Owners.EnumerableNullOrEmpty<Pawn>() || r.Owners.Contains(p)) && (!r.isPrisonCell || p.IsPrisoner);
		}

		// Token: 0x06004CCE RID: 19662 RVA: 0x0003678C File Offset: 0x0003498C
		public static IEnumerable<Thing> GetMeditationFociAffectedByBuilding(Map map, ThingDef def, Faction faction, IntVec3 pos, Rot4 rotation)
		{
			if (!MeditationUtility.CountsAsArtificialBuilding(def, faction))
			{
				yield break;
			}
			foreach (Thing thing in map.listerThings.ThingsMatching(ThingRequest.ForGroup(ThingRequestGroup.MeditationFocus)))
			{
				CompMeditationFocus compMeditationFocus = thing.TryGetComp<CompMeditationFocus>();
				if (compMeditationFocus != null && compMeditationFocus.WillBeAffectedBy(def, faction, pos, rotation))
				{
					yield return thing;
				}
			}
			List<Thing>.Enumerator enumerator = default(List<Thing>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06004CCF RID: 19663 RVA: 0x001AC06C File Offset: 0x001AA26C
		public static void DrawMeditationFociAffectedByBuildingOverlay(Map map, ThingDef def, Faction faction, IntVec3 pos, Rot4 rotation)
		{
			int num = 0;
			foreach (Thing thing in MeditationUtility.GetMeditationFociAffectedByBuilding(map, def, faction, pos, rotation))
			{
				if (num++ > 10)
				{
					break;
				}
				CompToggleDrawAffectedMeditationFoci compToggleDrawAffectedMeditationFoci = thing.TryGetComp<CompToggleDrawAffectedMeditationFoci>();
				if (compToggleDrawAffectedMeditationFoci == null || compToggleDrawAffectedMeditationFoci.Enabled)
				{
					GenAdj.OccupiedRect(pos, rotation, def.size);
					GenDraw.DrawLineBetween(GenThing.TrueCenter(pos, rotation, def.size, def.Altitude), thing.TrueCenter(), SimpleColor.Red);
				}
			}
		}

		// Token: 0x06004CD0 RID: 19664 RVA: 0x000367B9 File Offset: 0x000349B9
		public static bool CountsAsArtificialBuilding(ThingDef def, Faction faction)
		{
			return def.category == ThingCategory.Building && faction != null && def.building.artificialForMeditationPurposes;
		}

		// Token: 0x06004CD1 RID: 19665 RVA: 0x000367D4 File Offset: 0x000349D4
		public static bool CountsAsArtificialBuilding(Thing t)
		{
			return MeditationUtility.CountsAsArtificialBuilding(t.def, t.Faction);
		}

		// Token: 0x06004CD2 RID: 19666 RVA: 0x001AC108 File Offset: 0x001AA308
		public static void DrawArtificialBuildingOverlay(IntVec3 pos, ThingDef def, Map map, float radius)
		{
			GenDraw.DrawRadiusRing(pos, radius, MeditationUtility.ArtificialBuildingRingColor, null);
			int num = 0;
			foreach (Thing t in map.listerArtificialBuildingsForMeditation.GetForCell(pos, radius))
			{
				if (num++ > 10)
				{
					break;
				}
				GenDraw.DrawLineBetween(GenThing.TrueCenter(pos, Rot4.North, def.size, def.Altitude), t.TrueCenter(), SimpleColor.Red);
			}
		}

		// Token: 0x06004CD3 RID: 19667 RVA: 0x001AC198 File Offset: 0x001AA398
		public static float PsyfocusGainPerTick(Pawn pawn, Thing focus = null)
		{
			float num = pawn.GetStatValue(StatDefOf.MeditationFocusGain, true);
			if (focus != null && !focus.Destroyed)
			{
				num += focus.GetStatValueForPawn(StatDefOf.MeditationFocusStrength, pawn, true);
			}
			return num / 60000f;
		}

		// Token: 0x06004CD4 RID: 19668 RVA: 0x001AC1D4 File Offset: 0x001AA3D4
		public static void CheckMeditationScheduleTeachOpportunity(Pawn pawn)
		{
			if (pawn.Dead || !pawn.Spawned || !pawn.HasPsylink)
			{
				return;
			}
			if (pawn.Faction != Faction.OfPlayer || pawn.IsQuestLodger())
			{
				return;
			}
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.MeditationSchedule, pawn, OpportunityType.GoodToKnow);
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.MeditationDesiredPsyfocus, pawn, OpportunityType.GoodToKnow);
		}

		// Token: 0x0400327B RID: 12923
		public static float FocusObjectSearchRadius = 3.9f;

		// Token: 0x0400327C RID: 12924
		private static float WanderRadius = 10f;

		// Token: 0x0400327D RID: 12925
		public static readonly Color ArtificialBuildingRingColor = new Color(0.8f, 0.49f, 0.43f);

		// Token: 0x0400327E RID: 12926
		private static Dictionary<MeditationFocusDef, List<Dialog_InfoCard.Hyperlink>> focusObjectHyperlinksPerTypeCache = new Dictionary<MeditationFocusDef, List<Dialog_InfoCard.Hyperlink>>();

		// Token: 0x0400327F RID: 12927
		private static Dictionary<MeditationFocusDef, string> focusObjectsPerTypeCache = new Dictionary<MeditationFocusDef, string>();
	}
}
