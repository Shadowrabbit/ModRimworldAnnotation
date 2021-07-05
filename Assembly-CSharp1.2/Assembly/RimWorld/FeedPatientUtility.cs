using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D76 RID: 3446
	public static class FeedPatientUtility
	{
		// Token: 0x06004EA4 RID: 20132 RVA: 0x001B2754 File Offset: 0x001B0954
		public static bool ShouldBeFed(Pawn p)
		{
			if (p.GetPosture() == PawnPosture.Standing)
			{
				return false;
			}
			if (p.NonHumanlikeOrWildMan())
			{
				Building_Bed building_Bed = p.CurrentBed();
				if (building_Bed == null || building_Bed.Faction != Faction.OfPlayer)
				{
					return false;
				}
			}
			else
			{
				if (p.Faction != Faction.OfPlayer && p.HostFaction != Faction.OfPlayer)
				{
					return false;
				}
				if (!p.InBed())
				{
					return false;
				}
			}
			if (!p.RaceProps.EatsFood)
			{
				return false;
			}
			if (p.Spawned && p.Map.designationManager.DesignationOn(p, DesignationDefOf.Slaughter) != null)
			{
				return false;
			}
			if (!HealthAIUtility.ShouldSeekMedicalRest(p))
			{
				return false;
			}
			if (p.HostFaction != null)
			{
				if (p.HostFaction != Faction.OfPlayer)
				{
					return false;
				}
				if (p.guest != null && !p.guest.CanBeBroughtFood)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06004EA5 RID: 20133 RVA: 0x001B281C File Offset: 0x001B0A1C
		public static bool IsHungry(Pawn p)
		{
			return p.needs != null && p.needs.food != null && p.needs.food.CurLevelPercentage <= p.needs.food.PercentageThreshHungry + 0.02f;
		}
	}
}
