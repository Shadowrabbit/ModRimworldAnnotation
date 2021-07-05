using System;
using System.Collections.Generic;
using System.Text;
using RimWorld;
using Steamworks;

namespace Verse.Steam
{
	// Token: 0x02000505 RID: 1285
	public static class WorkshopItems
	{
		// Token: 0x170007A0 RID: 1952
		// (get) Token: 0x060026F5 RID: 9973 RVA: 0x000F0E7E File Offset: 0x000EF07E
		public static IEnumerable<WorkshopItem> AllSubscribedItems
		{
			get
			{
				return WorkshopItems.subbedItems;
			}
		}

		// Token: 0x170007A1 RID: 1953
		// (get) Token: 0x060026F6 RID: 9974 RVA: 0x000F0E88 File Offset: 0x000EF088
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

		// Token: 0x060026F7 RID: 9975 RVA: 0x000F0EC3 File Offset: 0x000EF0C3
		static WorkshopItems()
		{
			WorkshopItems.RebuildItemsList();
		}

		// Token: 0x060026F8 RID: 9976 RVA: 0x0000313F File Offset: 0x0000133F
		public static void EnsureInit()
		{
		}

		// Token: 0x060026F9 RID: 9977 RVA: 0x000F0ED4 File Offset: 0x000EF0D4
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

		// Token: 0x060026FA RID: 9978 RVA: 0x000F0F1B File Offset: 0x000EF11B
		public static bool HasItem(PublishedFileId_t pfid)
		{
			return WorkshopItems.GetItem(pfid) != null;
		}

		// Token: 0x060026FB RID: 9979 RVA: 0x000F0F28 File Offset: 0x000EF128
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

		// Token: 0x060026FC RID: 9980 RVA: 0x000F0F9C File Offset: 0x000EF19C
		internal static void Notify_Subscribed(PublishedFileId_t pfid)
		{
			WorkshopItems.RebuildItemsList();
		}

		// Token: 0x060026FD RID: 9981 RVA: 0x000F0F9C File Offset: 0x000EF19C
		internal static void Notify_Installed(PublishedFileId_t pfid)
		{
			WorkshopItems.RebuildItemsList();
		}

		// Token: 0x060026FE RID: 9982 RVA: 0x000F0F9C File Offset: 0x000EF19C
		internal static void Notify_Unsubscribed(PublishedFileId_t pfid)
		{
			WorkshopItems.RebuildItemsList();
		}

		// Token: 0x060026FF RID: 9983 RVA: 0x000F0FA4 File Offset: 0x000EF1A4
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

		// Token: 0x04001840 RID: 6208
		private static List<WorkshopItem> subbedItems = new List<WorkshopItem>();
	}
}
