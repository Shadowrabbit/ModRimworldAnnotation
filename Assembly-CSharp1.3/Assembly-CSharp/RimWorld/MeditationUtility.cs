using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007EE RID: 2030
	public class MeditationUtility
	{
		// Token: 0x06003658 RID: 13912 RVA: 0x001340D8 File Offset: 0x001322D8
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
					JobDef def = JobDefOf.Meditate;
					IdeoFoundation_Deity ideoFoundation_Deity;
					if (forJoy && ModsConfig.IdeologyActive && pawn.Ideo != null && (ideoFoundation_Deity = (pawn.Ideo.foundation as IdeoFoundation_Deity)) != null && ideoFoundation_Deity.DeitiesListForReading.Any<IdeoFoundation_Deity.Deity>())
					{
						def = JobDefOf.MeditatePray;
					}
					job = JobMaker.MakeJob(def, meditationSpotAndFocus.spot, null, meditationSpotAndFocus.focus);
				}
				job.ignoreJoyTimeAssignment = !forJoy;
				return job;
			}
			return null;
		}

		// Token: 0x06003659 RID: 13913 RVA: 0x00134194 File Offset: 0x00132394
		public static MeditationSpotAndFocus FindMeditationSpot(Pawn pawn)
		{
			float num = float.MinValue;
			LocalTargetInfo spot = LocalTargetInfo.Invalid;
			LocalTargetInfo focus = LocalTargetInfo.Invalid;
			if (!ModLister.CheckRoyalty("Psyfocus"))
			{
				return new MeditationSpotAndFocus(spot, focus);
			}
			Room ownedRoom = pawn.ownership.OwnedRoom;
			foreach (LocalTargetInfo localTargetInfo in MeditationUtility.AllMeditationSpotCandidates(pawn, true))
			{
				if (MeditationUtility.SafeEnvironmentalConditions(pawn, localTargetInfo.Cell, pawn.Map) && localTargetInfo.Cell.Standable(pawn.Map))
				{
					float num2 = 1f / Mathf.Max((float)localTargetInfo.Cell.DistanceToSquared(pawn.Position), 0.1f);
					LocalTargetInfo localTargetInfo2 = (localTargetInfo.Thing is Building_Throne) ? localTargetInfo.Thing : MeditationUtility.BestFocusAt(localTargetInfo, pawn);
					if (pawn.HasPsylink && localTargetInfo2.IsValid)
					{
						num2 += localTargetInfo2.Thing.GetStatValueForPawn(StatDefOf.MeditationFocusStrength, pawn, true) * 100f;
					}
					Room room = localTargetInfo.Cell.GetRoom(pawn.Map);
					if (room != null && ownedRoom == room)
					{
						num2 += 1f;
					}
					Building building;
					if (localTargetInfo.Thing != null && (building = (localTargetInfo.Thing as Building)) != null && building.GetAssignedPawn() == pawn)
					{
						num2 += (float)((building.def == ThingDefOf.MeditationSpot) ? 200 : 100);
					}
					if (room != null && ModsConfig.IdeologyActive && room.Role == RoomRoleDefOf.WorshipRoom)
					{
						num2 += 50f;
						foreach (Thing thing in room.ContainedAndAdjacentThings)
						{
							num2 += thing.GetStatValue(StatDefOf.StyleDominance, true);
						}
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

		// Token: 0x0600365A RID: 13914 RVA: 0x001343CC File Offset: 0x001325CC
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
						Room room3 = s.GetRoom(RegionType.Set_All);
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
					Room room = thing.GetRoom(RegionType.Set_All);
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
			Room room2 = (building_Bed != null) ? building_Bed.GetRoom(RegionType.Set_All) : null;
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
			using (IEnumerator<Room> enumerator4 = MeditationUtility.UsableWorshipRooms(pawn).GetEnumerator())
			{
				while (enumerator4.MoveNext())
				{
					MeditationUtility.<>c__DisplayClass5_1 CS$<>8__locals2 = new MeditationUtility.<>c__DisplayClass5_1();
					CS$<>8__locals2.room = enumerator4.Current;
					foreach (LocalTargetInfo localTargetInfo3 in MeditationUtility.FocusSpotsInTheRoom(pawn, CS$<>8__locals2.room))
					{
						if (pawn.CanReach(localTargetInfo3, PathEndMode.Touch, pawn.NormalMaxDanger(), false, false, TraverseMode.ByPawn))
						{
							yield return localTargetInfo3;
						}
					}
					IEnumerator<LocalTargetInfo> enumerator3 = null;
					IntVec3 randomCell = CS$<>8__locals2.room.Districts.RandomElement<District>().Regions.RandomElement<Region>().RandomCell;
					c2 = RCellFinder.RandomWanderDestFor(pawn, randomCell, MeditationUtility.WanderRadius, (Pawn p, IntVec3 c, IntVec3 r) => c.GetRoom(p.Map) == CS$<>8__locals2.room && c.Standable(p.Map) && c.GetDoor(p.Map) == null && WanderRoomUtility.IsValidWanderDest(p, c, r), pawn.NormalMaxDanger());
					if (c2.IsValid)
					{
						yield return c2;
					}
					CS$<>8__locals2 = null;
				}
			}
			IEnumerator<Room> enumerator4 = null;
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
				Room room3 = c.GetRoom(p.Map);
				return room3 == null || MeditationUtility.CanUseRoomToMeditate(room3, pawn);
			}, pawn.NormalMaxDanger());
			if (c2.IsValid)
			{
				yield return c2;
			}
			yield break;
			yield break;
		}

		// Token: 0x0600365B RID: 13915 RVA: 0x001343E3 File Offset: 0x001325E3
		public static IEnumerable<Room> UsableWorshipRooms(Pawn pawn)
		{
			foreach (Room room in pawn.Map.regionGrid.allRooms)
			{
				if (room.Role == RoomRoleDefOf.WorshipRoom && MeditationUtility.CanUseRoomToMeditate(room, pawn))
				{
					yield return room;
				}
			}
			List<Room>.Enumerator enumerator = default(List<Room>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x0600365C RID: 13916 RVA: 0x001343F3 File Offset: 0x001325F3
		public static bool SafeEnvironmentalConditions(Pawn pawn, IntVec3 cell, Map map)
		{
			return (!map.gameConditionManager.ConditionIsActive(GameConditionDefOf.ToxicFallout) || cell.Roofed(map)) && cell.GetDangerFor(pawn, map) == Danger.None;
		}

		// Token: 0x0600365D RID: 13917 RVA: 0x00134420 File Offset: 0x00132620
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

		// Token: 0x0600365E RID: 13918 RVA: 0x001344B0 File Offset: 0x001326B0
		public static bool CanOnlyMeditateInBed(Pawn pawn)
		{
			return pawn.Downed;
		}

		// Token: 0x0600365F RID: 13919 RVA: 0x001344B8 File Offset: 0x001326B8
		public static bool ShouldMeditateInBed(Pawn pawn)
		{
			return pawn.Downed || pawn.health.hediffSet.AnyHediffMakesSickThought;
		}

		// Token: 0x06003660 RID: 13920 RVA: 0x001344DC File Offset: 0x001326DC
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

		// Token: 0x06003661 RID: 13921 RVA: 0x00134594 File Offset: 0x00132794
		public static IEnumerable<LocalTargetInfo> FocusSpotsInTheRoom(Pawn pawn, Room r)
		{
			List<Thing> things = r.ContainedAndAdjacentThings;
			int num;
			for (int i = 0; i < things.Count; i = num + 1)
			{
				CompMeditationFocus compMeditationFocus = things[i].TryGetComp<CompMeditationFocus>();
				if (compMeditationFocus != null && compMeditationFocus.CanPawnUse(pawn) && !(things[i] is Building_Throne) && things[i].GetStatValueForPawn(StatDefOf.MeditationFocusStrength, pawn, true) > 0f)
				{
					LocalTargetInfo localTargetInfo = MeditationUtility.MeditationSpotForFocus(things[i], pawn, new Func<IntVec3, bool>(r.ContainsCell));
					if (localTargetInfo.IsValid)
					{
						yield return localTargetInfo;
					}
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x06003662 RID: 13922 RVA: 0x001345AC File Offset: 0x001327AC
		public static LocalTargetInfo MeditationSpotForFocus(Thing t, Pawn p, Func<IntVec3, bool> validator = null)
		{
			return (from cell in t.OccupiedRect().ExpandedBy(2).AdjacentCellsCardinal
			where (validator == null || validator(cell)) && !cell.IsForbidden(p) && p.CanReserveAndReach(cell, PathEndMode.OnCell, p.NormalMaxDanger(), 1, -1, null, false) && cell.Standable(p.Map)
			select cell).RandomElementWithFallback(IntVec3.Invalid);
		}

		// Token: 0x06003663 RID: 13923 RVA: 0x00134604 File Offset: 0x00132804
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

		// Token: 0x06003664 RID: 13924 RVA: 0x00134614 File Offset: 0x00132814
		public static string FocusTypesAvailableForPawnString(Pawn pawn)
		{
			return (from f in MeditationUtility.FocusTypesAvailableForPawn(pawn)
			select f.label).ToCommaList(false, false);
		}

		// Token: 0x06003665 RID: 13925 RVA: 0x00134647 File Offset: 0x00132847
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

		// Token: 0x06003666 RID: 13926 RVA: 0x00134658 File Offset: 0x00132858
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
						"MeditationFocusCanUse".Translate(meditationFocusDef.label).Resolve(),
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

		// Token: 0x06003667 RID: 13927 RVA: 0x001347CC File Offset: 0x001329CC
		public static void DrawMeditationSpotOverlay(IntVec3 center, Map map)
		{
			GenDraw.DrawRadiusRing(center, MeditationUtility.FocusObjectSearchRadius);
			foreach (Thing thing in GenRadial.RadialDistinctThingsAround(center, map, MeditationUtility.FocusObjectSearchRadius, false))
			{
				if (!(thing is Building_Throne) && thing.def != ThingDefOf.Wall && thing.TryGetComp<CompMeditationFocus>() != null && GenSight.LineOfSight(center, thing.Position, map, false, null, 0, 0))
				{
					GenDraw.DrawLineBetween(center.ToVector3() + new Vector3(0.5f, 0f, 0.5f), thing.TrueCenter(), SimpleColor.White, 0.2f);
				}
			}
		}

		// Token: 0x06003668 RID: 13928 RVA: 0x00134888 File Offset: 0x00132A88
		public static bool CanUseRoomToMeditate(Room r, Pawn p)
		{
			return (r.Owners.EnumerableNullOrEmpty<Pawn>() || r.Owners.Contains(p)) && (!r.IsPrisonCell || p.IsPrisoner);
		}

		// Token: 0x06003669 RID: 13929 RVA: 0x001348BA File Offset: 0x00132ABA
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

		// Token: 0x0600366A RID: 13930 RVA: 0x001348E8 File Offset: 0x00132AE8
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
					GenDraw.DrawLineBetween(GenThing.TrueCenter(pos, rotation, def.size, def.Altitude), thing.TrueCenter(), SimpleColor.Red, 0.2f);
				}
			}
		}

		// Token: 0x0600366B RID: 13931 RVA: 0x00134988 File Offset: 0x00132B88
		public static bool CountsAsArtificialBuilding(ThingDef def, Faction faction)
		{
			return def.category == ThingCategory.Building && faction != null && def.building.artificialForMeditationPurposes;
		}

		// Token: 0x0600366C RID: 13932 RVA: 0x001349A3 File Offset: 0x00132BA3
		public static bool CountsAsArtificialBuilding(Thing t)
		{
			return MeditationUtility.CountsAsArtificialBuilding(t.def, t.Faction);
		}

		// Token: 0x0600366D RID: 13933 RVA: 0x001349B8 File Offset: 0x00132BB8
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
				GenDraw.DrawLineBetween(GenThing.TrueCenter(pos, Rot4.North, def.size, def.Altitude), t.TrueCenter(), SimpleColor.Red, 0.2f);
			}
		}

		// Token: 0x0600366E RID: 13934 RVA: 0x00134A50 File Offset: 0x00132C50
		public static float PsyfocusGainPerTick(Pawn pawn, Thing focus = null)
		{
			float num = pawn.GetStatValue(StatDefOf.MeditationFocusGain, true);
			if (focus != null && !focus.Destroyed)
			{
				num += focus.GetStatValueForPawn(StatDefOf.MeditationFocusStrength, pawn, true);
			}
			return num / 60000f;
		}

		// Token: 0x0600366F RID: 13935 RVA: 0x00134A8C File Offset: 0x00132C8C
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

		// Token: 0x04001EE8 RID: 7912
		public static float FocusObjectSearchRadius = 3.9f;

		// Token: 0x04001EE9 RID: 7913
		private static float WanderRadius = 10f;

		// Token: 0x04001EEA RID: 7914
		public static readonly Color ArtificialBuildingRingColor = new Color(0.8f, 0.49f, 0.43f);

		// Token: 0x04001EEB RID: 7915
		private static Dictionary<MeditationFocusDef, List<Dialog_InfoCard.Hyperlink>> focusObjectHyperlinksPerTypeCache = new Dictionary<MeditationFocusDef, List<Dialog_InfoCard.Hyperlink>>();

		// Token: 0x04001EEC RID: 7916
		private static Dictionary<MeditationFocusDef, string> focusObjectsPerTypeCache = new Dictionary<MeditationFocusDef, string>();
	}
}
