using System;
using Steamworks;
using UnityEngine;
using Verse.Steam;

namespace Verse
{
	// Token: 0x020004EC RID: 1260
	public static class SteamUtility
	{
		// Token: 0x1700075A RID: 1882
		// (get) Token: 0x06002602 RID: 9730 RVA: 0x000EBEEC File Offset: 0x000EA0EC
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

		// Token: 0x06002603 RID: 9731 RVA: 0x000EBF18 File Offset: 0x000EA118
		public static void OpenUrl(string url)
		{
			if (SteamManager.Initialized && SteamUtils.IsOverlayEnabled())
			{
				SteamFriends.ActivateGameOverlayToWebPage(url, EActivateGameOverlayToWebPageMode.k_EActivateGameOverlayToWebPageMode_Default);
				return;
			}
			Application.OpenURL(url);
		}

		// Token: 0x06002604 RID: 9732 RVA: 0x000EBF36 File Offset: 0x000EA136
		public static void OpenWorkshopPage(PublishedFileId_t pfid)
		{
			SteamUtility.OpenUrl(SteamUtility.SteamWorkshopPageUrl(pfid));
		}

		// Token: 0x06002605 RID: 9733 RVA: 0x000EBF43 File Offset: 0x000EA143
		public static void OpenSteamWorkshopPage()
		{
			SteamUtility.OpenUrl("http://steamcommunity.com/workshop/browse/?appid=" + SteamUtils.GetAppID());
		}

		// Token: 0x06002606 RID: 9734 RVA: 0x000EBF5E File Offset: 0x000EA15E
		public static string SteamWorkshopPageUrl(PublishedFileId_t pfid)
		{
			return "steam://url/CommunityFilePage/" + pfid;
		}

		// Token: 0x040017E4 RID: 6116
		private static string cachedPersonaName;
	}
}
