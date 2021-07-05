using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020015CE RID: 5582
	public static class ScenarioMaker
	{
		// Token: 0x170012C0 RID: 4800
		// (get) Token: 0x0600795A RID: 31066 RVA: 0x00051AB6 File Offset: 0x0004FCB6
		public static Scenario GeneratingScenario
		{
			get
			{
				return ScenarioMaker.scen;
			}
		}

		// Token: 0x0600795B RID: 31067 RVA: 0x0024CA60 File Offset: 0x0024AC60
		public static Scenario GenerateNewRandomScenario(string seed)
		{
			Rand.PushState();
			Rand.Seed = seed.GetHashCode();
			int @int = Rand.Int;
			int[] array = new int[10];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = Rand.Int;
			}
			int int2 = Rand.Int;
			ScenarioMaker.scen = new Scenario();
			ScenarioMaker.scen.Category = ScenarioCategory.CustomLocal;
			ScenarioMaker.scen.name = NameGenerator.GenerateName(RulePackDefOf.NamerScenario, null, false, null, null);
			ScenarioMaker.scen.description = null;
			ScenarioMaker.scen.summary = null;
			Rand.Seed = @int;
			ScenarioMaker.scen.playerFaction = (ScenPart_PlayerFaction)ScenarioMaker.MakeScenPart(ScenPartDefOf.PlayerFaction);
			ScenarioMaker.scen.parts.Add(ScenarioMaker.MakeScenPart(ScenPartDefOf.ConfigPage_ConfigureStartingPawns));
			ScenarioMaker.scen.parts.Add(ScenarioMaker.MakeScenPart(ScenPartDefOf.PlayerPawnsArriveMethod));
			Rand.Seed = array[0];
			ScenarioMaker.AddCategoryScenParts(ScenarioMaker.scen, ScenPartCategory.PlayerPawnFilter, Rand.RangeInclusive(-1, 2));
			Rand.Seed = array[1];
			ScenarioMaker.AddCategoryScenParts(ScenarioMaker.scen, ScenPartCategory.StartingImportant, Rand.RangeInclusive(0, 2));
			Rand.Seed = array[2];
			ScenarioMaker.AddCategoryScenParts(ScenarioMaker.scen, ScenPartCategory.PlayerPawnModifier, Rand.RangeInclusive(-1, 2));
			Rand.Seed = array[3];
			ScenarioMaker.AddCategoryScenParts(ScenarioMaker.scen, ScenPartCategory.Rule, Rand.RangeInclusive(-2, 3));
			Rand.Seed = array[4];
			ScenarioMaker.AddCategoryScenParts(ScenarioMaker.scen, ScenPartCategory.StartingItem, Rand.RangeInclusive(0, 6));
			Rand.Seed = array[5];
			ScenarioMaker.AddCategoryScenParts(ScenarioMaker.scen, ScenPartCategory.WorldThing, Rand.RangeInclusive(-3, 6));
			Rand.Seed = array[6];
			ScenarioMaker.AddCategoryScenParts(ScenarioMaker.scen, ScenPartCategory.GameCondition, Rand.RangeInclusive(-1, 2));
			Rand.Seed = int2;
			foreach (ScenPart scenPart in ScenarioMaker.scen.AllParts)
			{
				scenPart.Randomize();
			}
			for (int j = 0; j < ScenarioMaker.scen.parts.Count; j++)
			{
				for (int k = 0; k < ScenarioMaker.scen.parts.Count; k++)
				{
					if (j != k && ScenarioMaker.scen.parts[j].TryMerge(ScenarioMaker.scen.parts[k]))
					{
						ScenarioMaker.scen.parts.RemoveAt(k);
						k--;
						if (j > k)
						{
							j--;
						}
					}
				}
			}
			for (int l = 0; l < ScenarioMaker.scen.parts.Count; l++)
			{
				for (int m = 0; m < ScenarioMaker.scen.parts.Count; m++)
				{
					if (l != m && !ScenarioMaker.scen.parts[l].CanCoexistWith(ScenarioMaker.scen.parts[m]))
					{
						ScenarioMaker.scen.parts.RemoveAt(m);
						m--;
						if (l > m)
						{
							l--;
						}
					}
				}
			}
			foreach (string text in ScenarioMaker.scen.ConfigErrors())
			{
				Log.Error(text, false);
			}
			Rand.PopState();
			Scenario result = ScenarioMaker.scen;
			ScenarioMaker.scen = null;
			return result;
		}

		// Token: 0x0600795C RID: 31068 RVA: 0x00051ABD File Offset: 0x0004FCBD
		private static void AddCategoryScenParts(Scenario scen, ScenPartCategory cat, int count)
		{
			scen.parts.AddRange(ScenarioMaker.RandomScenPartsOfCategory(scen, cat, count));
		}

		// Token: 0x0600795D RID: 31069 RVA: 0x00051AD2 File Offset: 0x0004FCD2
		private static IEnumerable<ScenPart> RandomScenPartsOfCategory(Scenario scen, ScenPartCategory cat, int count)
		{
			if (count <= 0)
			{
				yield break;
			}
			IEnumerable<ScenPartDef> allowedParts = from d in ScenarioMaker.AddableParts(scen)
			where d.category == cat
			select d;
			int numYielded = 0;
			int numTries = 0;
			while (numYielded < count)
			{
				if (!allowedParts.Any<ScenPartDef>())
				{
					yield break;
				}
				ScenPart scenPart = ScenarioMaker.MakeScenPart(allowedParts.RandomElementByWeight((ScenPartDef d) => d.selectionWeight));
				int num;
				if (ScenarioMaker.CanAddPart(scen, scenPart))
				{
					yield return scenPart;
					num = numYielded;
					numYielded = num + 1;
				}
				num = numTries;
				numTries = num + 1;
				if (numTries > 100)
				{
					Log.Error(string.Concat(new object[]
					{
						"Could not add ScenPart of category ",
						cat,
						" to scenario ",
						scen,
						" after 50 tries."
					}), false);
					yield break;
				}
			}
			yield break;
		}

		// Token: 0x0600795E RID: 31070 RVA: 0x0024CDB0 File Offset: 0x0024AFB0
		public static IEnumerable<ScenPartDef> AddableParts(Scenario scen)
		{
			return from d in DefDatabase<ScenPartDef>.AllDefs
			where scen.AllParts.Count((ScenPart p) => p.def == d) < d.maxUses
			select d;
		}

		// Token: 0x0600795F RID: 31071 RVA: 0x0024CDE0 File Offset: 0x0024AFE0
		private static bool CanAddPart(Scenario scen, ScenPart newPart)
		{
			for (int i = 0; i < scen.parts.Count; i++)
			{
				if (!newPart.CanCoexistWith(scen.parts[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06007960 RID: 31072 RVA: 0x00051AF0 File Offset: 0x0004FCF0
		public static ScenPart MakeScenPart(ScenPartDef def)
		{
			ScenPart scenPart = (ScenPart)Activator.CreateInstance(def.scenPartClass);
			scenPart.def = def;
			return scenPart;
		}

		// Token: 0x04004FCB RID: 20427
		private static Scenario scen;
	}
}
