using System;
using System.Collections.Generic;
using System.IO;
using Verse;
using Verse.Steam;

namespace RimWorld
{
	// Token: 0x020015CA RID: 5578
	public static class ScenarioFiles
	{
		// Token: 0x170012BA RID: 4794
		// (get) Token: 0x06007938 RID: 31032 RVA: 0x0005199F File Offset: 0x0004FB9F
		public static IEnumerable<Scenario> AllScenariosLocal
		{
			get
			{
				return ScenarioFiles.scenariosLocal;
			}
		}

		// Token: 0x170012BB RID: 4795
		// (get) Token: 0x06007939 RID: 31033 RVA: 0x000519A6 File Offset: 0x0004FBA6
		public static IEnumerable<Scenario> AllScenariosWorkshop
		{
			get
			{
				return ScenarioFiles.scenariosWorkshop;
			}
		}

		// Token: 0x0600793A RID: 31034 RVA: 0x0024C3B4 File Offset: 0x0024A5B4
		public static void RecacheData()
		{
			ScenarioFiles.scenariosLocal.Clear();
			using (IEnumerator<FileInfo> enumerator = GenFilePaths.AllCustomScenarioFiles.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Scenario item;
					if (GameDataSaveLoader.TryLoadScenario(enumerator.Current.FullName, ScenarioCategory.CustomLocal, out item))
					{
						ScenarioFiles.scenariosLocal.Add(item);
					}
				}
			}
			ScenarioFiles.scenariosWorkshop.Clear();
			foreach (WorkshopItem workshopItem in WorkshopItems.AllSubscribedItems)
			{
				WorkshopItem_Scenario workshopItem_Scenario = workshopItem as WorkshopItem_Scenario;
				if (workshopItem_Scenario != null)
				{
					ScenarioFiles.scenariosWorkshop.Add(workshopItem_Scenario.GetScenario());
				}
			}
		}

		// Token: 0x04004FBC RID: 20412
		private static List<Scenario> scenariosLocal = new List<Scenario>();

		// Token: 0x04004FBD RID: 20413
		private static List<Scenario> scenariosWorkshop = new List<Scenario>();
	}
}
