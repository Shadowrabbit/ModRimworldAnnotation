using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C0F RID: 3087
	public static class ManhunterPackIncidentUtility
	{
		// Token: 0x0600488B RID: 18571 RVA: 0x0017FC8C File Offset: 0x0017DE8C
		public static float ManhunterAnimalWeight(PawnKindDef animal, float points)
		{
			points = Mathf.Max(points, 70f);
			if (animal.combatPower * 2f > points)
			{
				return 0f;
			}
			int num = Mathf.Min(Mathf.RoundToInt(points / animal.combatPower), 100);
			return Mathf.Clamp01(Mathf.InverseLerp(100f, 10f, (float)num));
		}

		// Token: 0x0600488C RID: 18572 RVA: 0x0017FCE8 File Offset: 0x0017DEE8
		public static bool TryFindManhunterAnimalKind(float points, int tile, out PawnKindDef animalKind)
		{
			IEnumerable<PawnKindDef> source = from k in DefDatabase<PawnKindDef>.AllDefs
			where ManhunterPackIncidentUtility.CanArriveManhunter(k) && (tile == -1 || Find.World.tileTemperatures.SeasonAndOutdoorTemperatureAcceptableFor(tile, k.race))
			select k;
			if (source.Any<PawnKindDef>())
			{
				if (source.TryRandomElementByWeight((PawnKindDef a) => ManhunterPackIncidentUtility.ManhunterAnimalWeight(a, points), out animalKind))
				{
					return true;
				}
				if (points > source.Min((PawnKindDef a) => a.combatPower) * 2f)
				{
					animalKind = source.MaxBy((PawnKindDef a) => a.combatPower);
					return true;
				}
			}
			animalKind = null;
			return false;
		}

		// Token: 0x0600488D RID: 18573 RVA: 0x0017FD9F File Offset: 0x0017DF9F
		public static int GetAnimalsCount(PawnKindDef animalKind, float points)
		{
			return Mathf.Clamp(Mathf.RoundToInt(points / animalKind.combatPower), 2, 100);
		}

		// Token: 0x0600488E RID: 18574 RVA: 0x0017FDB8 File Offset: 0x0017DFB8
		public static List<Pawn> GenerateAnimals(PawnKindDef animalKind, int tile, float points, int animalCount = 0)
		{
			List<Pawn> list = new List<Pawn>();
			int num = (animalCount > 0) ? animalCount : ManhunterPackIncidentUtility.GetAnimalsCount(animalKind, points);
			for (int i = 0; i < num; i++)
			{
				Pawn item = PawnGenerator.GeneratePawn(new PawnGenerationRequest(animalKind, null, PawnGenerationContext.NonPlayer, tile, false, false, false, false, true, false, 1f, false, true, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, null, false, false));
				list.Add(item);
			}
			return list;
		}

		// Token: 0x0600488F RID: 18575 RVA: 0x0017FE5C File Offset: 0x0017E05C
		[DebugOutput]
		public static void ManhunterResults()
		{
			List<PawnKindDef> candidates = (from k in DefDatabase<PawnKindDef>.AllDefs.Where(new Func<PawnKindDef, bool>(ManhunterPackIncidentUtility.CanArriveManhunter))
			orderby -k.combatPower
			select k).ToList<PawnKindDef>();
			List<float> list = new List<float>();
			for (int i = 0; i < 30; i++)
			{
				list.Add(20f * Mathf.Pow(1.25f, (float)i));
			}
			DebugTables.MakeTablesDialog<float, PawnKindDef>(list, (float points) => points.ToString("F0") + " pts", candidates, (PawnKindDef candidate) => candidate.defName + " (" + candidate.combatPower.ToString("F0") + ")", delegate(float points, PawnKindDef candidate)
			{
				float num = candidates.Sum((PawnKindDef k) => ManhunterPackIncidentUtility.ManhunterAnimalWeight(k, points));
				float num2 = ManhunterPackIncidentUtility.ManhunterAnimalWeight(candidate, points);
				if (num2 == 0f)
				{
					return "0%";
				}
				return string.Format("{0}%, {1}", (num2 * 100f / num).ToString("F0"), Mathf.Max(Mathf.RoundToInt(points / candidate.combatPower), 1));
			}, "");
		}

		// Token: 0x06004890 RID: 18576 RVA: 0x0017FF3A File Offset: 0x0017E13A
		private static bool CanArriveManhunter(PawnKindDef kind)
		{
			return kind.RaceProps.Animal && kind.canArriveManhunter && kind.RaceProps.CanPassFences;
		}

		// Token: 0x04002C68 RID: 11368
		public const int MinAnimalCount = 2;

		// Token: 0x04002C69 RID: 11369
		public const int MaxAnimalCount = 100;

		// Token: 0x04002C6A RID: 11370
		public const float MinPoints = 70f;
	}
}
