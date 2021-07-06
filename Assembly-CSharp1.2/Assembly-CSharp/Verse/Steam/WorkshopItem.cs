using System;
using System.IO;
using Steamworks;

namespace Verse.Steam
{
	// Token: 0x020008BA RID: 2234
	public class WorkshopItem
	{
		// Token: 0x170008BB RID: 2235
		// (get) Token: 0x0600379F RID: 14239 RVA: 0x0002B072 File Offset: 0x00029272
		public DirectoryInfo Directory
		{
			get
			{
				return this.directoryInt;
			}
		}

		// Token: 0x170008BC RID: 2236
		// (get) Token: 0x060037A0 RID: 14240 RVA: 0x0002B07A File Offset: 0x0002927A
		// (set) Token: 0x060037A1 RID: 14241 RVA: 0x0002B082 File Offset: 0x00029282
		public virtual PublishedFileId_t PublishedFileId
		{
			get
			{
				return this.pfidInt;
			}
			set
			{
				this.pfidInt = value;
			}
		}

		// Token: 0x060037A2 RID: 14242 RVA: 0x001617E4 File Offset: 0x0015F9E4
		public static WorkshopItem MakeFrom(PublishedFileId_t pfid)
		{
			ulong num;
			string path;
			uint num2;
			bool itemInstallInfo = SteamUGC.GetItemInstallInfo(pfid, out num, out path, 257U, out num2);
			WorkshopItem workshopItem = null;
			if (!itemInstallInfo)
			{
				workshopItem = new WorkshopItem_NotInstalled();
			}
			else
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(path);
				if (!directoryInfo.Exists)
				{
					Log.Error("Created WorkshopItem for " + pfid + " but there is no folder for it.", false);
					return new WorkshopItem_NotInstalled();
				}
				FileInfo[] files = directoryInfo.GetFiles();
				for (int i = 0; i < files.Length; i++)
				{
					if (files[i].Extension == ".rsc")
					{
						workshopItem = new WorkshopItem_Scenario();
						break;
					}
				}
				if (workshopItem == null)
				{
					workshopItem = new WorkshopItem_Mod();
				}
				workshopItem.directoryInt = directoryInfo;
			}
			workshopItem.PublishedFileId = pfid;
			return workshopItem;
		}

		// Token: 0x060037A3 RID: 14243 RVA: 0x0002B08B File Offset: 0x0002928B
		public override string ToString()
		{
			return base.GetType().ToString() + "-" + this.PublishedFileId;
		}

		// Token: 0x040026A9 RID: 9897
		protected DirectoryInfo directoryInt;

		// Token: 0x040026AA RID: 9898
		private PublishedFileId_t pfidInt;
	}
}
