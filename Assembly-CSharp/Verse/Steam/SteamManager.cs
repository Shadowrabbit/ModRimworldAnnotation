using System;
using System.Text;
using Steamworks;
using UnityEngine;

namespace Verse.Steam
{
	// Token: 0x020008B7 RID: 2231
	public static class SteamManager
	{
		// Token: 0x170008B6 RID: 2230
		// (get) Token: 0x0600377D RID: 14205 RVA: 0x0002AF26 File Offset: 0x00029126
		public static bool Initialized
		{
			get
			{
				return SteamManager.initializedInt;
			}
		}

		// Token: 0x170008B7 RID: 2231
		// (get) Token: 0x0600377E RID: 14206 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public static bool Active
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600377F RID: 14207 RVA: 0x00160CCC File Offset: 0x0015EECC
		public static void InitIfNeeded()
		{
			if (SteamManager.initializedInt)
			{
				return;
			}
			if (!Packsize.Test())
			{
				Log.Error("[Steamworks.NET] Packsize Test returned false, the wrong version of Steamworks.NET is being run in this platform.", false);
			}
			if (!DllCheck.Test())
			{
				Log.Error("[Steamworks.NET] DllCheck Test returned false, One or more of the Steamworks binaries seems to be the wrong version.", false);
			}
			try
			{
				if (SteamAPI.RestartAppIfNecessary(AppId_t.Invalid))
				{
					Application.Quit();
					return;
				}
			}
			catch (DllNotFoundException arg)
			{
				Log.Error("[Steamworks.NET] Could not load [lib]steam_api.dll/so/dylib. It's likely not in the correct location. Refer to the README for more details.\n" + arg, false);
				Application.Quit();
				return;
			}
			SteamManager.initializedInt = SteamAPI.Init();
			if (!SteamManager.initializedInt)
			{
				Log.Warning("[Steamworks.NET] SteamAPI.Init() failed. Possible causes: Steam client not running, launched from outside Steam without steam_appid.txt in place, running with different privileges than Steam client (e.g. \"as administrator\")", false);
				return;
			}
			if (SteamManager.steamAPIWarningMessageHook == null)
			{
				SteamManager.steamAPIWarningMessageHook = new SteamAPIWarningMessageHook_t(SteamManager.SteamAPIDebugTextHook);
				SteamClient.SetWarningMessageHook(SteamManager.steamAPIWarningMessageHook);
			}
			Workshop.Init();
		}

		// Token: 0x06003780 RID: 14208 RVA: 0x0002AF2D File Offset: 0x0002912D
		public static void Update()
		{
			if (!SteamManager.initializedInt)
			{
				return;
			}
			SteamAPI.RunCallbacks();
		}

		// Token: 0x06003781 RID: 14209 RVA: 0x0002AF3C File Offset: 0x0002913C
		public static void ShutdownSteam()
		{
			if (!SteamManager.initializedInt)
			{
				return;
			}
			SteamAPI.Shutdown();
		}

		// Token: 0x06003782 RID: 14210 RVA: 0x0002AF4B File Offset: 0x0002914B
		private static void SteamAPIDebugTextHook(int nSeverity, StringBuilder pchDebugText)
		{
			Log.Error(pchDebugText.ToString(), false);
		}

		// Token: 0x04002695 RID: 9877
		private static SteamAPIWarningMessageHook_t steamAPIWarningMessageHook;

		// Token: 0x04002696 RID: 9878
		private static bool initializedInt;
	}
}
