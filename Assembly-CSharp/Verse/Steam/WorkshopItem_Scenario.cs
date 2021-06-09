using System;
using System.IO;
using System.Linq;
using RimWorld;
using Steamworks;

namespace Verse.Steam
{
	// Token: 0x020008BC RID: 2236
	public class WorkshopItem_Scenario : WorkshopItem
	{
		// Token: 0x170008BD RID: 2237
		// (get) Token: 0x060037A6 RID: 14246 RVA: 0x0002B0B5 File Offset: 0x000292B5
		// (set) Token: 0x060037A7 RID: 14247 RVA: 0x0002B0BD File Offset: 0x000292BD
		public override PublishedFileId_t PublishedFileId
		{
			get
			{
				return base.PublishedFileId;
			}
			set
			{
				base.PublishedFileId = value;
				if (this.cachedScenario != null)
				{
					this.cachedScenario.SetPublishedFileId(value);
				}
			}
		}

		// Token: 0x060037A8 RID: 14248 RVA: 0x0002B0DA File Offset: 0x000292DA
		public Scenario GetScenario()
		{
			if (this.cachedScenario == null)
			{
				this.LoadScenario();
			}
			return this.cachedScenario;
		}

		// Token: 0x060037A9 RID: 14249 RVA: 0x00161894 File Offset: 0x0015FA94
		private void LoadScenario()
		{
			if (GameDataSaveLoader.TryLoadScenario((from fi in base.Directory.GetFiles("*.rsc")
			where fi.Extension == ".rsc"
			select fi).First<FileInfo>().FullName, ScenarioCategory.SteamWorkshop, out this.cachedScenario))
			{
				this.cachedScenario.SetPublishedFileId(this.PublishedFileId);
			}
		}

		// Token: 0x040026AB RID: 9899
		private Scenario cachedScenario;
	}
}
