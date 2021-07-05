using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020012C1 RID: 4801
	public static class TameUtility
	{
		// Token: 0x060072B5 RID: 29365 RVA: 0x00264634 File Offset: 0x00262834
		public static void ShowDesignationWarnings(Pawn pawn, bool showManhunterOnTameFailWarning = true)
		{
			if (showManhunterOnTameFailWarning)
			{
				float manhunterOnTameFailChance = pawn.RaceProps.manhunterOnTameFailChance;
				if (manhunterOnTameFailChance >= 0.015f)
				{
					Messages.Message("MessageAnimalManhuntsOnTameFailed".Translate(pawn.kindDef.GetLabelPlural(-1).CapitalizeFirst(), manhunterOnTameFailChance.ToStringPercent(), pawn.Named("ANIMAL")), pawn, MessageTypeDefOf.CautionInput, false);
				}
			}
			IEnumerable<Pawn> source = from c in pawn.Map.mapPawns.FreeColonistsSpawned
			where c.workSettings.WorkIsActive(WorkTypeDefOf.Handling)
			select c;
			if (!source.Any<Pawn>())
			{
				source = pawn.Map.mapPawns.FreeColonistsSpawned;
			}
			if (source.Any<Pawn>())
			{
				Pawn pawn2 = source.MaxBy((Pawn c) => c.skills.GetSkill(SkillDefOf.Animals).Level);
				int level = pawn2.skills.GetSkill(SkillDefOf.Animals).Level;
				int num = TrainableUtility.MinimumHandlingSkill(pawn);
				if (num > level)
				{
					Messages.Message("MessageNoHandlerSkilledEnough".Translate(pawn.kindDef.label, num.ToStringCached(), SkillDefOf.Animals.LabelCap, pawn2.LabelShort, level, pawn.Named("ANIMAL"), pawn2.Named("HANDLER")), pawn, MessageTypeDefOf.CautionInput, false);
				}
			}
		}

		// Token: 0x060072B6 RID: 29366 RVA: 0x002647B8 File Offset: 0x002629B8
		public static bool CanTame(Pawn pawn)
		{
			return pawn.AnimalOrWildMan() && (pawn.Faction == null || !pawn.Faction.def.humanlikeFaction) && pawn.RaceProps.wildness < 1f && pawn.RaceProps.animalType != AnimalType.Dryad;
		}

		// Token: 0x060072B7 RID: 29367 RVA: 0x0026480C File Offset: 0x00262A0C
		public static bool TriedToTameTooRecently(Pawn animal)
		{
			return Find.TickManager.TicksGame < animal.mindState.lastAssignedInteractTime + 30000;
		}

		// Token: 0x04003EC6 RID: 16070
		public const int MinTameInterval = 30000;
	}
}
