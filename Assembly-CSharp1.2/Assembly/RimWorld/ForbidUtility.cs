using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020017C4 RID: 6084
	public static class ForbidUtility
	{
		// Token: 0x06008690 RID: 34448 RVA: 0x00279358 File Offset: 0x00277558
		public static void SetForbidden(this Thing t, bool value, bool warnOnFail = true)
		{
			if (t == null)
			{
				if (warnOnFail)
				{
					Log.Error("Tried to SetForbidden on null Thing.", false);
				}
				return;
			}
			ThingWithComps thingWithComps = t as ThingWithComps;
			if (thingWithComps == null)
			{
				if (warnOnFail)
				{
					Log.Error("Tried to SetForbidden on non-ThingWithComps Thing " + t, false);
				}
				return;
			}
			CompForbiddable comp = thingWithComps.GetComp<CompForbiddable>();
			if (comp == null)
			{
				if (warnOnFail)
				{
					Log.Error("Tried to SetForbidden on non-Forbiddable Thing " + t, false);
				}
				return;
			}
			comp.Forbidden = value;
		}

		// Token: 0x06008691 RID: 34449 RVA: 0x002793BC File Offset: 0x002775BC
		public static void SetForbiddenIfOutsideHomeArea(this Thing t)
		{
			if (!t.Spawned)
			{
				Log.Error("SetForbiddenIfOutsideHomeArea unspawned thing " + t, false);
			}
			if (t.Position.InBounds(t.Map) && !t.Map.areaManager.Home[t.Position])
			{
				t.SetForbidden(true, false);
			}
		}

		// Token: 0x06008692 RID: 34450 RVA: 0x0027941C File Offset: 0x0027761C
		public static bool CaresAboutForbidden(Pawn pawn, bool cellTarget)
		{
			return (pawn.HostFaction == null || (pawn.HostFaction == Faction.OfPlayer && pawn.Spawned && !pawn.Map.IsPlayerHome && (pawn.GetRoom(RegionType.Set_Passable) == null || !pawn.GetRoom(RegionType.Set_Passable).isPrisonCell) && (!pawn.IsPrisoner || pawn.guest.PrisonerIsSecure))) && !pawn.InMentalState && (!cellTarget || !ThinkNode_ConditionalShouldFollowMaster.ShouldFollowMaster(pawn));
		}

		// Token: 0x06008693 RID: 34451 RVA: 0x002794A0 File Offset: 0x002776A0
		public static bool InAllowedArea(this IntVec3 c, Pawn forPawn)
		{
			if (forPawn.playerSettings != null)
			{
				Area effectiveAreaRestrictionInPawnCurrentMap = forPawn.playerSettings.EffectiveAreaRestrictionInPawnCurrentMap;
				if (effectiveAreaRestrictionInPawnCurrentMap != null && effectiveAreaRestrictionInPawnCurrentMap.TrueCount > 0 && !effectiveAreaRestrictionInPawnCurrentMap[c])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06008694 RID: 34452 RVA: 0x002794DC File Offset: 0x002776DC
		public static bool IsForbidden(this Thing t, Pawn pawn)
		{
			if (!ForbidUtility.CaresAboutForbidden(pawn, false))
			{
				return false;
			}
			if (t.Spawned && t.Position.IsForbidden(pawn))
			{
				return true;
			}
			if (t.IsForbidden(pawn.Faction) || t.IsForbidden(pawn.HostFaction))
			{
				return true;
			}
			Lord lord = pawn.GetLord();
			return lord != null && lord.extraForbiddenThings.Contains(t);
		}

		// Token: 0x06008695 RID: 34453 RVA: 0x0005A472 File Offset: 0x00058672
		public static bool IsForbiddenToPass(this Building_Door t, Pawn pawn)
		{
			return ForbidUtility.CaresAboutForbidden(pawn, false) && t.IsForbidden(pawn.Faction);
		}

		// Token: 0x06008696 RID: 34454 RVA: 0x00279548 File Offset: 0x00277748
		public static bool IsForbidden(this IntVec3 c, Pawn pawn)
		{
			return ForbidUtility.CaresAboutForbidden(pawn, true) && (!c.InAllowedArea(pawn) || (pawn.mindState.maxDistToSquadFlag > 0f && !c.InHorDistOf(pawn.DutyLocation(), pawn.mindState.maxDistToSquadFlag)));
		}

		// Token: 0x06008697 RID: 34455 RVA: 0x0027959C File Offset: 0x0027779C
		public static bool IsForbiddenEntirely(this Region r, Pawn pawn)
		{
			if (!ForbidUtility.CaresAboutForbidden(pawn, true))
			{
				return false;
			}
			if (pawn.playerSettings != null)
			{
				Area effectiveAreaRestriction = pawn.playerSettings.EffectiveAreaRestriction;
				if (effectiveAreaRestriction != null && effectiveAreaRestriction.TrueCount > 0 && effectiveAreaRestriction.Map == r.Map && r.OverlapWith(effectiveAreaRestriction) == AreaOverlap.None)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06008698 RID: 34456 RVA: 0x002795F0 File Offset: 0x002777F0
		public static bool IsForbidden(this Thing t, Faction faction)
		{
			if (faction == null)
			{
				return false;
			}
			if (faction != Faction.OfPlayer)
			{
				return false;
			}
			ThingWithComps thingWithComps = t as ThingWithComps;
			if (thingWithComps == null)
			{
				return false;
			}
			CompForbiddable comp = thingWithComps.GetComp<CompForbiddable>();
			return comp != null && comp.Forbidden;
		}
	}
}
