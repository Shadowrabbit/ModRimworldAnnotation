using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001139 RID: 4409
	public static class ForbidUtility
	{
		// Token: 0x060069ED RID: 27117 RVA: 0x0023AB7C File Offset: 0x00238D7C
		public static void SetForbidden(this Thing t, bool value, bool warnOnFail = true)
		{
			if (t == null)
			{
				if (warnOnFail)
				{
					Log.Error("Tried to SetForbidden on null Thing.");
				}
				return;
			}
			ThingWithComps thingWithComps = t as ThingWithComps;
			if (thingWithComps == null)
			{
				if (warnOnFail)
				{
					Log.Error("Tried to SetForbidden on non-ThingWithComps Thing " + t);
				}
				return;
			}
			CompForbiddable comp = thingWithComps.GetComp<CompForbiddable>();
			if (comp == null)
			{
				if (warnOnFail)
				{
					Log.Error("Tried to SetForbidden on non-Forbiddable Thing " + t);
				}
				return;
			}
			comp.Forbidden = value;
		}

		// Token: 0x060069EE RID: 27118 RVA: 0x0023ABE0 File Offset: 0x00238DE0
		public static void SetForbiddenIfOutsideHomeArea(this Thing t)
		{
			if (!t.Spawned)
			{
				Log.Error("SetForbiddenIfOutsideHomeArea unspawned thing " + t);
			}
			if (t.Position.InBounds(t.Map) && !t.Map.areaManager.Home[t.Position])
			{
				t.SetForbidden(true, false);
			}
		}

		// Token: 0x060069EF RID: 27119 RVA: 0x0023AC40 File Offset: 0x00238E40
		public static bool CaresAboutForbidden(Pawn pawn, bool cellTarget)
		{
			return (pawn.HostFaction == null || (pawn.HostFaction == Faction.OfPlayer && pawn.Spawned && !pawn.Map.IsPlayerHome && (pawn.GetRoom(RegionType.Set_All) == null || !pawn.GetRoom(RegionType.Set_All).IsPrisonCell) && (!pawn.IsPrisoner || pawn.guest.PrisonerIsSecure))) && !pawn.InMentalState && !SlaveRebellionUtility.IsRebelling(pawn) && (!cellTarget || !ThinkNode_ConditionalShouldFollowMaster.ShouldFollowMaster(pawn));
		}

		// Token: 0x060069F0 RID: 27120 RVA: 0x0023ACD0 File Offset: 0x00238ED0
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

		// Token: 0x060069F1 RID: 27121 RVA: 0x0023AD0C File Offset: 0x00238F0C
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
			if (lord != null && lord.extraForbiddenThings.Contains(t))
			{
				return true;
			}
			foreach (Lord lord2 in pawn.MapHeld.lordManager.lords)
			{
				LordToil_Ritual lordToil_Ritual;
				if ((lordToil_Ritual = (lord2.CurLordToil as LordToil_Ritual)) != null && lordToil_Ritual.ReservedThings.Contains(t) && lord2 != lord)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060069F2 RID: 27122 RVA: 0x0023ADEC File Offset: 0x00238FEC
		public static bool IsForbiddenToPass(this Building_Door t, Pawn pawn)
		{
			return ForbidUtility.CaresAboutForbidden(pawn, false) && t.IsForbidden(pawn.Faction);
		}

		// Token: 0x060069F3 RID: 27123 RVA: 0x0023AE0C File Offset: 0x0023900C
		public static bool IsForbidden(this IntVec3 c, Pawn pawn)
		{
			return ForbidUtility.CaresAboutForbidden(pawn, true) && (!c.InAllowedArea(pawn) || (pawn.mindState.maxDistToSquadFlag > 0f && !c.InHorDistOf(pawn.DutyLocation(), pawn.mindState.maxDistToSquadFlag)));
		}

		// Token: 0x060069F4 RID: 27124 RVA: 0x0023AE60 File Offset: 0x00239060
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

		// Token: 0x060069F5 RID: 27125 RVA: 0x0023AEB4 File Offset: 0x002390B4
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
