using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020015CB RID: 5579
	public static class ScenarioLister
	{
		// Token: 0x0600793C RID: 31036 RVA: 0x000519C3 File Offset: 0x0004FBC3
		public static IEnumerable<Scenario> AllScenarios()
		{
			ScenarioLister.RecacheIfDirty();
			foreach (ScenarioDef scenarioDef in DefDatabase<ScenarioDef>.AllDefs)
			{
				yield return scenarioDef.scenario;
			}
			IEnumerator<ScenarioDef> enumerator = null;
			foreach (Scenario scenario in ScenarioFiles.AllScenariosLocal)
			{
				yield return scenario;
			}
			IEnumerator<Scenario> enumerator2 = null;
			foreach (Scenario scenario2 in ScenarioFiles.AllScenariosWorkshop)
			{
				yield return scenario2;
			}
			enumerator2 = null;
			yield break;
			yield break;
		}

		// Token: 0x0600793D RID: 31037 RVA: 0x000519CC File Offset: 0x0004FBCC
		public static IEnumerable<Scenario> ScenariosInCategory(ScenarioCategory cat)
		{
			ScenarioLister.RecacheIfDirty();
			if (cat == ScenarioCategory.FromDef)
			{
				foreach (ScenarioDef scenarioDef in DefDatabase<ScenarioDef>.AllDefs)
				{
					yield return scenarioDef.scenario;
				}
				IEnumerator<ScenarioDef> enumerator = null;
			}
			else if (cat == ScenarioCategory.CustomLocal)
			{
				foreach (Scenario scenario in ScenarioFiles.AllScenariosLocal)
				{
					yield return scenario;
				}
				IEnumerator<Scenario> enumerator2 = null;
			}
			else if (cat == ScenarioCategory.SteamWorkshop)
			{
				foreach (Scenario scenario2 in ScenarioFiles.AllScenariosWorkshop)
				{
					yield return scenario2;
				}
				IEnumerator<Scenario> enumerator2 = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x0600793E RID: 31038 RVA: 0x0024C474 File Offset: 0x0024A674
		public static bool ScenarioIsListedAnywhere(Scenario scen)
		{
			ScenarioLister.RecacheIfDirty();
			using (IEnumerator<ScenarioDef> enumerator = DefDatabase<ScenarioDef>.AllDefs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.scenario == scen)
					{
						return true;
					}
				}
			}
			foreach (Scenario scenario in ScenarioFiles.AllScenariosLocal)
			{
				if (scen == scenario)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600793F RID: 31039 RVA: 0x000519DC File Offset: 0x0004FBDC
		public static void MarkDirty()
		{
			ScenarioLister.dirty = true;
		}

		// Token: 0x06007940 RID: 31040 RVA: 0x000519E4 File Offset: 0x0004FBE4
		private static void RecacheIfDirty()
		{
			if (ScenarioLister.dirty)
			{
				ScenarioLister.RecacheData();
			}
		}

		// Token: 0x06007941 RID: 31041 RVA: 0x0024C508 File Offset: 0x0024A708
		private static void RecacheData()
		{
			ScenarioLister.dirty = false;
			int num = ScenarioLister.ScenarioListHash();
			ScenarioFiles.RecacheData();
			if (ScenarioLister.ScenarioListHash() != num && !LongEventHandler.ShouldWaitForEvent)
			{
				Page_SelectScenario page_SelectScenario = Find.WindowStack.WindowOfType<Page_SelectScenario>();
				if (page_SelectScenario != null)
				{
					page_SelectScenario.Notify_ScenarioListChanged();
				}
			}
		}

		// Token: 0x06007942 RID: 31042 RVA: 0x0024C54C File Offset: 0x0024A74C
		public static int ScenarioListHash()
		{
			int num = 9826121;
			foreach (Scenario scenario in ScenarioLister.AllScenarios())
			{
				num ^= 791 * scenario.GetHashCode() * 6121;
			}
			return num;
		}

		// Token: 0x04004FBE RID: 20414
		private static bool dirty = true;
	}
}
