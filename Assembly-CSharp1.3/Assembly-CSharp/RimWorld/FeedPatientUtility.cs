using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200083C RID: 2108
	public static class FeedPatientUtility
	{
		// Token: 0x060037DD RID: 14301 RVA: 0x0013AFE4 File Offset: 0x001391E4
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
			if (p.Spawned && p.ShouldBeSlaughtered())
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

		// Token: 0x060037DE RID: 14302 RVA: 0x0013B09C File Offset: 0x0013929C
		public static bool IsHungry(Pawn p)
		{
			return p.needs != null && p.needs.food != null && p.needs.food.CurLevelPercentage <= p.needs.food.PercentageThreshHungry + 0.02f;
		}
	}
}
