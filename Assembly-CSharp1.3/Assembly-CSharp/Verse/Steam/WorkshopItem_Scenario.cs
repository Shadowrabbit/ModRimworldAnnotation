using System;
using System.IO;
using System.Linq;
using RimWorld;
using Steamworks;

namespace Verse.Steam
{
	// Token: 0x02000501 RID: 1281
	public class WorkshopItem_Scenario : WorkshopItem
	{
		// Token: 0x17000796 RID: 1942
		// (get) Token: 0x060026D7 RID: 9943 RVA: 0x000F0C9C File Offset: 0x000EEE9C
		// (set) Token: 0x060026D8 RID: 9944 RVA: 0x000F0CA4 File Offset: 0x000EEEA4
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

		// Token: 0x060026D9 RID: 9945 RVA: 0x000F0CC1 File Offset: 0x000EEEC1
		public Scenario GetScenario()
		{
			if (this.cachedScenario == null)
			{
				this.LoadScenario();
			}
			return this.cachedScenario;
		}

		// Token: 0x060026DA RID: 9946 RVA: 0x000F0CD8 File Offset: 0x000EEED8
		private void LoadScenario()
		{
			if (GameDataSaveLoader.TryLoadScenario((from fi in base.Directory.GetFiles("*.rsc")
			where fi.Extension == ".rsc"
			select fi).First<FileInfo>().FullName, ScenarioCategory.SteamWorkshop, out this.cachedScenario))
			{
				this.cachedScenario.SetPublishedFileId(this.PublishedFileId);
			}
		}

		// Token: 0x0400183C RID: 6204
		private Scenario cachedScenario;
	}
}
