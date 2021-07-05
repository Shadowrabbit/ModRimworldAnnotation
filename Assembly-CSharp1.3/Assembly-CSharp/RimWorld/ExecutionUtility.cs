using System;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200074D RID: 1869
	public static class ExecutionUtility
	{
		// Token: 0x060033B8 RID: 13240 RVA: 0x00125AE8 File Offset: 0x00123CE8
		public static void DoExecutionByCut(Pawn executioner, Pawn victim, int bloodPerWeight = 8, bool spawnBlood = true)
		{
			if (spawnBlood)
			{
				int num = Mathf.Max(GenMath.RoundRandom(victim.BodySize * (float)bloodPerWeight), 1);
				for (int i = 0; i < num; i++)
				{
					victim.health.DropBloodFilth();
				}
			}
			if (ModsConfig.IdeologyActive && victim.RaceProps.Animal)
			{
				Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.SlaughteredAnimal, executioner.Named(HistoryEventArgsNames.Doer)), true);
				if (victim.BodySize >= 0.7f)
				{
					FactionIdeosTracker ideos = executioner.Faction.ideos;
					if (ideos != null)
					{
						ideos.ResetLastAnimalSlaughteredTick();
					}
				}
			}
			BodyPartRecord bodyPartRecord = ExecutionUtility.ExecuteCutPart(victim);
			int num2 = Mathf.Clamp((int)victim.health.hediffSet.GetPartHealth(bodyPartRecord) - 1, 1, 20);
			DamageInfo damageInfo = new DamageInfo(DamageDefOf.ExecutionCut, (float)num2, 999f, -1f, executioner, bodyPartRecord, null, DamageInfo.SourceCategory.ThingOrUnknown, null, false, spawnBlood);
			victim.TakeDamage(damageInfo);
			if (!victim.Dead)
			{
				victim.Kill(new DamageInfo?(damageInfo), null);
			}
			SoundDefOf.Execute_Cut.PlayOneShot(victim);
		}

		// Token: 0x060033B9 RID: 13241 RVA: 0x00125BF0 File Offset: 0x00123DF0
		private static BodyPartRecord ExecuteCutPart(Pawn pawn)
		{
			BodyPartRecord bodyPartRecord = pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null).FirstOrDefault((BodyPartRecord x) => x.def == BodyPartDefOf.Neck);
			if (bodyPartRecord != null)
			{
				return bodyPartRecord;
			}
			bodyPartRecord = pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null).FirstOrDefault((BodyPartRecord x) => x.def == BodyPartDefOf.Head);
			if (bodyPartRecord != null)
			{
				return bodyPartRecord;
			}
			bodyPartRecord = pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null).FirstOrDefault((BodyPartRecord x) => x.def == BodyPartDefOf.InsectHead);
			if (bodyPartRecord != null)
			{
				return bodyPartRecord;
			}
			bodyPartRecord = pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null).FirstOrDefault((BodyPartRecord x) => x.def == BodyPartDefOf.Body);
			if (bodyPartRecord != null)
			{
				return bodyPartRecord;
			}
			bodyPartRecord = pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null).FirstOrDefault((BodyPartRecord x) => x.def == BodyPartDefOf.SnakeBody);
			if (bodyPartRecord != null)
			{
				return bodyPartRecord;
			}
			bodyPartRecord = pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null).FirstOrDefault((BodyPartRecord x) => x.def == BodyPartDefOf.Torso);
			if (bodyPartRecord != null)
			{
				return bodyPartRecord;
			}
			Log.Error("No good slaughter cut part found for " + pawn);
			return pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null).RandomElementByWeight((BodyPartRecord x) => x.coverageAbsWithChildren);
		}

		// Token: 0x04001E15 RID: 7701
		public const float MinBodySizeForLargeAnimalSlaughtered = 0.7f;
	}
}
