using System;
using RimWorld;
using Verse;

// Token: 0x02000006 RID: 6
public static class HistoryEventUtility
{
	// Token: 0x06000010 RID: 16 RVA: 0x00002354 File Offset: 0x00000554
	public static bool IsKillingInnocentAnimal(Pawn executioner, Pawn victim)
	{
		if (!ModsConfig.IdeologyActive)
		{
			return false;
		}
		if (!victim.RaceProps.Animal)
		{
			return false;
		}
		if (victim.Faction != null && executioner.Faction != null && victim.Faction.HostileTo(executioner.Faction))
		{
			return false;
		}
		if (victim.health.hediffSet.HasHediff(HediffDefOf.Scaria, false))
		{
			return false;
		}
		if (executioner.CurJob != null && executioner.CurJob.def == JobDefOf.PredatorHunt)
		{
			return false;
		}
		if (victim.CurJob != null && victim.CurJob.def == JobDefOf.PredatorHunt)
		{
			Pawn prey = ((JobDriver_PredatorHunt)victim.jobs.curDriver).Prey;
			if (prey != null)
			{
				if (prey.RaceProps.Humanlike)
				{
					return false;
				}
				if (prey.RaceProps.Animal && prey.Faction != null && prey.Faction.def.humanlikeFaction)
				{
					return false;
				}
			}
		}
		return !victim.InMentalState || victim.MentalState.causedByDamage || victim.MentalState.causedByPsycast;
	}
}
