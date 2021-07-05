using System;
using System.Text;
using Steamworks;
using UnityEngine;

namespace Verse.Steam
{
	// Token: 0x020004FD RID: 1277
	public static class SteamManager
	{
		// Token: 0x17000791 RID: 1937
		// (get) Token: 0x060026B6 RID: 9910 RVA: 0x000F0062 File Offset: 0x000EE262
		public static bool Initialized
		{
			get
			{
				return SteamManager.initializedInt;
			}
		}

		// Token: 0x17000792 RID: 1938
		// (get) Token: 0x060026B7 RID: 9911 RVA: 0x000126F5 File Offset: 0x000108F5
		public static bool Active
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060026B8 RID: 9912 RVA: 0x000F006C File Offset: 0x000EE26C
		public static void InitIfNeeded()
		{
			if (SteamManager.initializedInt)
			{
				return;
			}
			if (!Packsize.Test())
			{
				Log.Error("[Steamworks.NET] Packsize Test returned false, the wrong version of Steamworks.NET is being run in this platform.");
			}
			if (!DllCheck.Test())
			{
				Log.Error("[Steamworks.NET] DllCheck Test returned false, One or more of the Steamworks binaries seems to be the wrong version.");
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
				Log.Error("[Steamworks.NET] Could not load [lib]steam_api.dll/so/dylib. It's likely not in the correct location. Refer to the README for more details.\n" + arg);
				Application.Quit();
				return;
			}
			SteamManager.initializedInt = SteamAPI.Init();
			if (!SteamManager.initializedInt)
			{
				Log.Warning("[Steamworks.NET] SteamAPI.Init() failed. Possible causes: Steam client not running, launched from outside Steam without steam_appid.txt in place, running with different privileges than Steam client (e.g. \"as administrator\")");
				return;
			}
			if (SteamManager.steamAPIWarningMessageHook == null)
			{
				SteamManager.steamAPIWarningMessageHook = new SteamAPIWarningMessageHook_t(SteamManager.SteamAPIDebugTextHook);
				SteamClient.SetWarningMessageHook(SteamManager.steamAPIWarningMessageHook);
			}
			Workshop.Init();
		}

		// Token: 0x060026B9 RID: 9913 RVA: 0x000F0124 File Offset: 0x000EE324
		public static void Update()
		{
			if (!SteamManager.initializedInt)
			{
				return;
			}
			SteamAPI.RunCallbacks();
		}

		// Token: 0x060026BA RID: 9914 RVA: 0x000F0133 File Offset: 0x000EE333
		public static void ShutdownSteam()
		{
			if (!SteamManager.initializedInt)
			{
				return;
			}
			SteamAPI.Shutdown();
		}

		// Token: 0x060026BB RID: 9915 RVA: 0x000F0142 File Offset: 0x000EE342
		private static void SteamAPIDebugTextHook(int nSeverity, StringBuilder pchDebugText)
		{
			Log.Error(pchDebugText.ToString());
		}

		// Token: 0x0400182C RID: 6188
		private static SteamAPIWarningMessageHook_t steamAPIWarningMessageHook;

		// Token: 0x0400182D RID: 6189
		private static bool initializedInt;
	}
}
