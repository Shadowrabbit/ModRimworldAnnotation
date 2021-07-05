using System;
using Steamworks;
using UnityEngine;
using Verse.Steam;

namespace Verse
{
	// Token: 0x0200089B RID: 2203
	public static class SteamUtility
	{
		// Token: 0x1700087A RID: 2170
		// (get) Token: 0x0600369B RID: 13979 RVA: 0x0002A5F0 File Offset: 0x000287F0
		public static string SteamPersonaName
		{
			get
			{
				if (SteamManager.Initialized && SteamUtility.cachedPersonaName == null)
				{
					SteamUtility.cachedPersonaName = SteamFriends.GetPersonaName();
				}
				if (SteamUtility.cachedPersonaName == null)
				{
					return "???";
				}
				return SteamUtility.cachedPersonaName;
			}
		}

		// Token: 0x0600369C RID: 13980 RVA: 0x0002A61C File Offset: 0x0002881C
		public static void OpenUrl(string url)
		{
			if (SteamManager.Initialized && SteamUtils.IsOverlayEnabled())
			{
				SteamFriends.ActivateGameOverlayToWebPage(url, EActivateGameOverlayToWebPageMode.k_EActivateGameOverlayToWebPageMode_Default);
				return;
			}
			Application.OpenURL(url);
		}

		// Token: 0x0600369D RID: 13981 RVA: 0x0002A63A File Offset: 0x0002883A
		public static void OpenWorkshopPage(PublishedFileId_t pfid)
		{
			SteamUtility.OpenUrl(SteamUtility.SteamWorkshopPageUrl(pfid));
		}

		// Token: 0x0600369E RID: 13982 RVA: 0x0002A647 File Offset: 0x00028847
		public static void OpenSteamWorkshopPage()
		{
			SteamUtility.OpenUrl("http://steamcommunity.com/workshop/browse/?appid=" + SteamUtils.GetAppID());
		}

		// Token: 0x0600369F RID: 13983 RVA: 0x0002A662 File Offset: 0x00028862
		public static string SteamWorkshopPageUrl(PublishedFileId_t pfid)
		{
			return "steam://url/CommunityFilePage/" + pfid;
		}

		// Token: 0x04002625 RID: 9765
		private static string cachedPersonaName;
	}
}
