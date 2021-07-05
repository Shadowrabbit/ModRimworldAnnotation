using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001305 RID: 4869
	public class Dialog_ScenarioList_Load : Dialog_ScenarioList
	{
		// Token: 0x06007508 RID: 29960 RVA: 0x0027DEE8 File Offset: 0x0027C0E8
		public Dialog_ScenarioList_Load(Action<Scenario> scenarioReturner)
		{
			this.interactButLabel = "LoadGameButton".Translate();
			this.scenarioReturner = scenarioReturner;
		}

		// Token: 0x06007509 RID: 29961 RVA: 0x0027DF0C File Offset: 0x0027C10C
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

		// Token: 0x0400408D RID: 16525
		private Action<Scenario> scenarioReturner;
	}
}
