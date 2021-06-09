using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020013F0 RID: 5104
	public static class AgeInjuryUtility
	{
		// Token: 0x06006E63 RID: 28259 RVA: 0x0004AC96 File Offset: 0x00048E96
		public static IEnumerable<HediffGiver_Birthday> RandomHediffsToGainOnBirthday(Pawn pawn, int age)
		{
			return AgeInjuryUtility.RandomHediffsToGainOnBirthday(pawn.def, age);
		}

		// Token: 0x06006E64 RID: 28260 RVA: 0x0004ACA4 File Offset: 0x00048EA4
		private static IEnumerable<HediffGiver_Birthday> RandomHediffsToGainOnBirthday(ThingDef raceDef, int age)
		{
			List<HediffGiverSetDef> sets = raceDef.race.hediffGiverSets;
			if (sets == null)
			{
				yield break;
			}
			int num;
			for (int i = 0; i < sets.Count; i = num + 1)
			{
				List<HediffGiver> givers = sets[i].hediffGivers;
				for (int j = 0; j < givers.Count; j = num + 1)
				{
					HediffGiver_Birthday hediffGiver_Birthday = givers[j] as HediffGiver_Birthday;
					if (hediffGiver_Birthday != null)
					{
						float x = (float)age / raceDef.race.lifeExpectancy;
						if (Rand.Value < hediffGiver_Birthday.ageFractionChanceCurve.Evaluate(x))
						{
							yield return hediffGiver_Birthday;
						}
					}
					num = j;
				}
				givers = null;
				num = i;
			}
			yield break;
		}

		// Token: 0x06006E65 RID: 28261 RVA: 0x0021D374 File Offset: 0x0021B574
		public static void GenerateRandomOldAgeInjuries(Pawn pawn, bool tryNotToKillPawn)
		{
			float num = pawn.RaceProps.IsMechanoid ? 2500f : pawn.RaceProps.lifeExpectancy;
			float num2 = num / 8f;
			float b = num * 1.5f;
			float chance = pawn.RaceProps.Humanlike ? 0.15f : 0.03f;
			int num3 = 0;
			for (float num4 = num2; num4 < Mathf.Min((float)pawn.ageTracker.AgeBiologicalYears, b); num4 += num2)
			{
				if (Rand.Chance(chance))
				{
					num3++;
				}
			}
			Func<BodyPartRecord, bool> <>9__0;
			for (int i = 0; i < num3; i++)
			{
				IEnumerable<BodyPartRecord> notMissingParts = pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null);
				Func<BodyPartRecord, bool> predicate;
				if ((predicate = <>9__0) == null)
				{
					predicate = (<>9__0 = ((BodyPartRecord x) => x.depth == BodyPartDepth.Outside && (x.def.permanentInjuryChanceFactor != 0f || x.def.pawnGeneratorCanAmputate) && !pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(x)));
				}
				IEnumerable<BodyPartRecord> source = notMissingParts.Where(predicate);
				if (source.Any<BodyPartRecord>())
				{
					BodyPartRecord bodyPartRecord = source.RandomElementByWeight((BodyPartRecord x) => x.coverageAbs);
					HediffDef hediffDefFromDamage = HealthUtility.GetHediffDefFromDamage(AgeInjuryUtility.RandomPermanentInjuryDamageType(bodyPartRecord.def.frostbiteVulnerability > 0f && pawn.RaceProps.ToolUser), pawn, bodyPartRecord);
					if (bodyPartRecord.def.pawnGeneratorCanAmputate && Rand.Chance(0.3f))
					{
						Hediff_MissingPart hediff_MissingPart = (Hediff_MissingPart)HediffMaker.MakeHediff(HediffDefOf.MissingBodyPart, pawn, null);
						hediff_MissingPart.lastInjury = hediffDefFromDamage;
						hediff_MissingPart.Part = bodyPartRecord;
						hediff_MissingPart.IsFresh = false;
						if (!tryNotToKillPawn || !pawn.health.WouldDieAfterAddingHediff(hediff_MissingPart))
						{
							pawn.health.AddHediff(hediff_MissingPart, bodyPartRecord, null, null);
							if (pawn.RaceProps.Humanlike && bodyPartRecord.def == BodyPartDefOf.Leg && Rand.Chance(0.5f))
							{
								RecipeDefOf.InstallPegLeg.Worker.ApplyOnPawn(pawn, bodyPartRecord, null, AgeInjuryUtility.emptyIngredientsList, null);
							}
						}
					}
					else if (bodyPartRecord.def.permanentInjuryChanceFactor > 0f && hediffDefFromDamage.HasComp(typeof(HediffComp_GetsPermanent)))
					{
						Hediff_Injury hediff_Injury = (Hediff_Injury)HediffMaker.MakeHediff(hediffDefFromDamage, pawn, null);
						hediff_Injury.Severity = (float)Rand.RangeInclusive(2, 6);
						hediff_Injury.TryGetComp<HediffComp_GetsPermanent>().IsPermanent = true;
						hediff_Injury.Part = bodyPartRecord;
						if (!tryNotToKillPawn || !pawn.health.WouldDieAfterAddingHediff(hediff_Injury))
						{
							pawn.health.AddHediff(hediff_Injury, bodyPartRecord, null, null);
						}
					}
				}
			}
			for (int j = 1; j < pawn.ageTracker.AgeBiologicalYears; j++)
			{
				foreach (HediffGiver_Birthday hediffGiver_Birthday in AgeInjuryUtility.RandomHediffsToGainOnBirthday(pawn, j))
				{
					hediffGiver_Birthday.TryApplyAndSimulateSeverityChange(pawn, (float)j, tryNotToKillPawn);
					if (pawn.Dead)
					{
						break;
					}
				}
				if (pawn.Dead)
				{
					break;
				}
			}
		}

		// Token: 0x06006E66 RID: 28262 RVA: 0x0021D6EC File Offset: 0x0021B8EC
		private static DamageDef RandomPermanentInjuryDamageType(bool allowFrostbite)
		{
			switch (Rand.RangeInclusive(0, 3 + (allowFrostbite ? 1 : 0)))
			{
			case 0:
				return DamageDefOf.Bullet;
			case 1:
				return DamageDefOf.Scratch;
			case 2:
				return DamageDefOf.Bite;
			case 3:
				return DamageDefOf.Stab;
			case 4:
				return DamageDefOf.Frostbite;
			default:
				throw new Exception();
			}
		}

		// Token: 0x06006E67 RID: 28263 RVA: 0x0021D748 File Offset: 0x0021B948
		[DebugOutput]
		public static void PermanentInjuryCalculations()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("=======Theoretical injuries=========");
			for (int i = 0; i < 10; i++)
			{
				stringBuilder.AppendLine("#" + i + ":");
				List<HediffDef> list = new List<HediffDef>();
				for (int j = 0; j < 100; j++)
				{
					foreach (HediffGiver_Birthday hediffGiver_Birthday in AgeInjuryUtility.RandomHediffsToGainOnBirthday(ThingDefOf.Human, j))
					{
						if (!list.Contains(hediffGiver_Birthday.hediff))
						{
							list.Add(hediffGiver_Birthday.hediff);
							stringBuilder.AppendLine(string.Concat(new object[]
							{
								"  age ",
								j,
								" - ",
								hediffGiver_Birthday.hediff
							}));
						}
					}
				}
			}
			Log.Message(stringBuilder.ToString(), false);
			stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("=======Actual injuries=========");
			for (int k = 0; k < 200; k++)
			{
				Pawn pawn = PawnGenerator.GeneratePawn(Faction.OfPlayer.def.basicMemberKind, Faction.OfPlayer);
				if (pawn.ageTracker.AgeBiologicalYears >= 40)
				{
					stringBuilder.AppendLine(pawn.Name + " age " + pawn.ageTracker.AgeBiologicalYears);
					foreach (Hediff arg in pawn.health.hediffSet.hediffs)
					{
						stringBuilder.AppendLine(" - " + arg);
					}
				}
				Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Discard);
			}
			Log.Message(stringBuilder.ToString(), false);
		}

		// Token: 0x040048C7 RID: 18631
		private const int MaxPermanentInjuryAge = 100;

		// Token: 0x040048C8 RID: 18632
		private static List<Thing> emptyIngredientsList = new List<Thing>();
	}
}
