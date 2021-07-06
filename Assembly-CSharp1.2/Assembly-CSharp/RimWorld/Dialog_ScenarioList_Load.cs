using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A1F RID: 6687
	public class Dialog_ScenarioList_Load : Dialog_ScenarioList
	{
		// Token: 0x060093B0 RID: 37808 RVA: 0x00062ED3 File Offset: 0x000610D3
		public Dialog_ScenarioList_Load(Action<Scenario> scenarioReturner)
		{
			this.interactButLabel = "LoadGameButton".Translate();
			this.scenarioReturner = scenarioReturner;
		}

		// Token: 0x060093B1 RID: 37809 RVA: 0x002A9044 File Offset: 0x002A7244
		protected override void DoFileInteraction(string fileName)
		{
			string filePath = GenFilePaths.AbsPathForScenario(fileName);
			PreLoadUtility.CheckVersionAndLoad(filePath, ScribeMetaHeaderUtility.ScribeHeaderMode.Scenario, delegate
			{
				Scenario obj = null;
				if (GameDataSaveLoader.TryLoadScenario(filePath, ScenarioCategory.CustomLocal, out obj))
				{
					this.scenarioReturner(obj);
				}
				this.Close(true);
			});
		}

		// Token: 0x04005D94 RID: 23956
		private Action<Scenario> scenarioReturner;
	}
}
