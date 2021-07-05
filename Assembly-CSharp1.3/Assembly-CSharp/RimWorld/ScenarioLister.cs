using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200101E RID: 4126
	public static class ScenarioLister
	{
		// Token: 0x06006161 RID: 24929 RVA: 0x002112A6 File Offset: 0x0020F4A6
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

		// Token: 0x06006162 RID: 24930 RVA: 0x002112AF File Offset: 0x0020F4AF
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

		// Token: 0x06006163 RID: 24931 RVA: 0x002112C0 File Offset: 0x0020F4C0
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

		// Token: 0x06006164 RID: 24932 RVA: 0x00211354 File Offset: 0x0020F554
		public static void MarkDirty()
		{
			ScenarioLister.dirty = true;
		}

		// Token: 0x06006165 RID: 24933 RVA: 0x0021135C File Offset: 0x0020F55C
		private static void RecacheIfDirty()
		{
			if (ScenarioLister.dirty)
			{
				ScenarioLister.RecacheData();
			}
		}

		// Token: 0x06006166 RID: 24934 RVA: 0x0021136C File Offset: 0x0020F56C
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

		// Token: 0x06006167 RID: 24935 RVA: 0x002113B0 File Offset: 0x0020F5B0
		public static int ScenarioListHash()
		{
			int num = 9826121;
			foreach (Scenario scenario in ScenarioLister.AllScenarios())
			{
				num ^= 791 * scenario.GetHashCode() * 6121;
			}
			return num;
		}

		// Token: 0x0400377F RID: 14207
		private static bool dirty = true;
	}
}
