using System;
using System.Collections.Generic;
using System.IO;
using Verse;
using Verse.Steam;

namespace RimWorld
{
	// Token: 0x0200101D RID: 4125
	public static class ScenarioFiles
	{
		// Token: 0x17001091 RID: 4241
		// (get) Token: 0x0600615D RID: 24925 RVA: 0x002111C2 File Offset: 0x0020F3C2
		public static IEnumerable<Scenario> AllScenariosLocal
		{
			get
			{
				return ScenarioFiles.scenariosLocal;
			}
		}

		// Token: 0x17001092 RID: 4242
		// (get) Token: 0x0600615E RID: 24926 RVA: 0x002111C9 File Offset: 0x0020F3C9
		public static IEnumerable<Scenario> AllScenariosWorkshop
		{
			get
			{
				return ScenarioFiles.scenariosWorkshop;
			}
		}

		// Token: 0x0600615F RID: 24927 RVA: 0x002111D0 File Offset: 0x0020F3D0
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

		// Token: 0x0400377D RID: 14205
		private static List<Scenario> scenariosLocal = new List<Scenario>();

		// Token: 0x0400377E RID: 14206
		private static List<Scenario> scenariosWorkshop = new List<Scenario>();
	}
}
