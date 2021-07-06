using System;
using System.Collections.Generic;
using System.Text;
using RimWorld;
using Steamworks;

namespace Verse.Steam
{
	// Token: 0x020008C1 RID: 2241
	public static class WorkshopItems
	{
		// Token: 0x170008C7 RID: 2247
		// (get) Token: 0x060037C7 RID: 14279 RVA: 0x0002B1F6 File Offset: 0x000293F6
		public static IEnumerable<WorkshopItem> AllSubscribedItems
		{
			get
			{
				return WorkshopItems.subbedItems;
			}
		}

		// Token: 0x170008C8 RID: 2248
		// (get) Token: 0x060037C8 RID: 14280 RVA: 0x00161944 File Offset: 0x0015FB44
		public static int DownloadingItemsCount
		{
			get
			{
				int num = 0;
				for (int i = 0; i < WorkshopItems.subbedItems.Count; i++)
				{
					if (WorkshopItems.subbedItems[i] is WorkshopItem_NotInstalled)
					{
						num++;
					}
				}
				return num;
			}
		}

		// Token: 0x060037C9 RID: 14281 RVA: 0x0002B1FD File Offset: 0x000293FD
		static WorkshopItems()
		{
			WorkshopItems.RebuildItemsList();
		}

		// Token: 0x060037CA RID: 14282 RVA: 0x00006A05 File Offset: 0x00004C05
		public static void EnsureInit()
		{
		}

		// Token: 0x060037CB RID: 14283 RVA: 0x00161980 File Offset: 0x0015FB80
		public static WorkshopItem GetItem(PublishedFileId_t pfid)
		{
			for (int i = 0; i < WorkshopItems.subbedItems.Count; i++)
			{
				if (WorkshopItems.subbedItems[i].PublishedFileId == pfid)
				{
					return WorkshopItems.subbedItems[i];
				}
			}
			return null;
		}

		// Token: 0x060037CC RID: 14284 RVA: 0x0002B20E File Offset: 0x0002940E
		public static bool HasItem(PublishedFileId_t pfid)
		{
			return WorkshopItems.GetItem(pfid) != null;
		}

		// Token: 0x060037CD RID: 14285 RVA: 0x001619C8 File Offset: 0x0015FBC8
		private static void RebuildItemsList()
		{
			if (!SteamManager.Initialized)
			{
				return;
			}
			WorkshopItems.subbedItems.Clear();
			foreach (PublishedFileId_t pfid in Workshop.AllSubscribedItems())
			{
				WorkshopItems.subbedItems.Add(WorkshopItem.MakeFrom(pfid));
			}
			ModLister.RebuildModList();
			ScenarioLister.MarkDirty();
		}

		// Token: 0x060037CE RID: 14286 RVA: 0x0002B219 File Offset: 0x00029419
		internal static void Notify_Subscribed(PublishedFileId_t pfid)
		{
			WorkshopItems.RebuildItemsList();
		}

		// Token: 0x060037CF RID: 14287 RVA: 0x0002B219 File Offset: 0x00029419
		internal static void Notify_Installed(PublishedFileId_t pfid)
		{
			WorkshopItems.RebuildItemsList();
		}

		// Token: 0x060037D0 RID: 14288 RVA: 0x0002B219 File Offset: 0x00029419
		internal static void Notify_Unsubscribed(PublishedFileId_t pfid)
		{
			WorkshopItems.RebuildItemsList();
		}

		// Token: 0x060037D1 RID: 14289 RVA: 0x00161A3C File Offset: 0x0015FC3C
		public static string DebugOutput()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Subscribed items:");
			foreach (WorkshopItem workshopItem in WorkshopItems.subbedItems)
			{
				stringBuilder.AppendLine("  " + workshopItem.ToString());
			}
			return stringBuilder.ToString();
		}

		// Token: 0x040026B1 RID: 9905
		private static List<WorkshopItem> subbedItems = new List<WorkshopItem>();
	}
}
