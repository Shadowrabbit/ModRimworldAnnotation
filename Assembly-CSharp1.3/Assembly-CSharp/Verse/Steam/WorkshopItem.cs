using System;
using System.IO;
using Steamworks;

namespace Verse.Steam
{
	// Token: 0x020004FF RID: 1279
	public class WorkshopItem
	{
		// Token: 0x17000794 RID: 1940
		// (get) Token: 0x060026D0 RID: 9936 RVA: 0x000F0BA9 File Offset: 0x000EEDA9
		public DirectoryInfo Directory
		{
			get
			{
				return this.directoryInt;
			}
		}

		// Token: 0x17000795 RID: 1941
		// (get) Token: 0x060026D1 RID: 9937 RVA: 0x000F0BB1 File Offset: 0x000EEDB1
		// (set) Token: 0x060026D2 RID: 9938 RVA: 0x000F0BB9 File Offset: 0x000EEDB9
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

		// Token: 0x060026D3 RID: 9939 RVA: 0x000F0BC4 File Offset: 0x000EEDC4
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
					Log.Error("Created WorkshopItem for " + pfid + " but there is no folder for it.");
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

		// Token: 0x060026D4 RID: 9940 RVA: 0x000F0C72 File Offset: 0x000EEE72
		public override string ToString()
		{
			return base.GetType().ToString() + "-" + this.PublishedFileId;
		}

		// Token: 0x0400183A RID: 6202
		protected DirectoryInfo directoryInt;

		// Token: 0x0400183B RID: 6203
		private PublishedFileId_t pfidInt;
	}
}
