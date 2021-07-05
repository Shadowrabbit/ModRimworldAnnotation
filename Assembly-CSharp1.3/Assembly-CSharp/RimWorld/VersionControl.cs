using System;
using System.Reflection;
using UnityEngine;
using Verse;
using Verse.Steam;

namespace RimWorld
{
	// Token: 0x020006B4 RID: 1716
	public static class VersionControl
	{
		// Token: 0x170008D9 RID: 2265
		// (get) Token: 0x06002F75 RID: 12149 RVA: 0x001191C0 File Offset: 0x001173C0
		public static Version CurrentVersion
		{
			get
			{
				return VersionControl.version;
			}
		}

		// Token: 0x170008DA RID: 2266
		// (get) Token: 0x06002F76 RID: 12150 RVA: 0x001191C7 File Offset: 0x001173C7
		public static string CurrentVersionString
		{
			get
			{
				return VersionControl.versionString;
			}
		}

		// Token: 0x170008DB RID: 2267
		// (get) Token: 0x06002F77 RID: 12151 RVA: 0x001191CE File Offset: 0x001173CE
		public static string CurrentVersionStringWithoutBuild
		{
			get
			{
				return VersionControl.versionStringWithoutBuild;
			}
		}

		// Token: 0x170008DC RID: 2268
		// (get) Token: 0x06002F78 RID: 12152 RVA: 0x001191D5 File Offset: 0x001173D5
		public static string CurrentVersionStringWithRev
		{
			get
			{
				return VersionControl.versionStringWithRev;
			}
		}

		// Token: 0x170008DD RID: 2269
		// (get) Token: 0x06002F79 RID: 12153 RVA: 0x001191DC File Offset: 0x001173DC
		public static int CurrentMajor
		{
			get
			{
				return VersionControl.version.Major;
			}
		}

		// Token: 0x170008DE RID: 2270
		// (get) Token: 0x06002F7A RID: 12154 RVA: 0x001191E8 File Offset: 0x001173E8
		public static int CurrentMinor
		{
			get
			{
				return VersionControl.version.Minor;
			}
		}

		// Token: 0x170008DF RID: 2271
		// (get) Token: 0x06002F7B RID: 12155 RVA: 0x001191F4 File Offset: 0x001173F4
		public static int CurrentBuild
		{
			get
			{
				return VersionControl.version.Build;
			}
		}

		// Token: 0x170008E0 RID: 2272
		// (get) Token: 0x06002F7C RID: 12156 RVA: 0x00119200 File Offset: 0x00117400
		public static int CurrentRevision
		{
			get
			{
				return VersionControl.version.Revision;
			}
		}

		// Token: 0x170008E1 RID: 2273
		// (get) Token: 0x06002F7D RID: 12157 RVA: 0x0011920C File Offset: 0x0011740C
		public static DateTime CurrentBuildDate
		{
			get
			{
				return VersionControl.buildDate;
			}
		}

		// Token: 0x06002F7E RID: 12158 RVA: 0x00119214 File Offset: 0x00117414
		static VersionControl()
		{
			Version version = Assembly.GetExecutingAssembly().GetName().Version;
			VersionControl.buildDate = new DateTime(2000, 1, 1).AddDays((double)version.Build);
			int build = version.Build - 4805;
			int revision = version.Revision * 2 / 60;
			VersionControl.version = new Version(version.Major, version.Minor, build, revision);
			VersionControl.versionStringWithRev = string.Concat(new object[]
			{
				VersionControl.version.Major,
				".",
				VersionControl.version.Minor,
				".",
				VersionControl.version.Build,
				" rev",
				VersionControl.version.Revision
			});
			VersionControl.versionString = string.Concat(new object[]
			{
				VersionControl.version.Major,
				".",
				VersionControl.version.Minor,
				".",
				VersionControl.version.Build
			});
			VersionControl.versionStringWithoutBuild = VersionControl.version.Major + "." + VersionControl.version.Minor;
		}

		// Token: 0x06002F7F RID: 12159 RVA: 0x00119378 File Offset: 0x00117578
		public static void DrawInfoInCorner()
		{
			Text.Font = GameFont.Small;
			GUI.color = new Color(1f, 1f, 1f, 0.5f);
			string text = "VersionIndicator".Translate(VersionControl.versionString);
			string versionExtraInfo = VersionControl.GetVersionExtraInfo();
			if (!versionExtraInfo.NullOrEmpty())
			{
				text = text + " (" + versionExtraInfo + ")";
			}
			text += "\n" + "CompiledOn".Translate(VersionControl.buildDate.ToString("MMM d yyyy"));
			if (SteamManager.Initialized)
			{
				text += "\n" + "LoggedIntoSteamAs".Translate(SteamUtility.SteamPersonaName);
			}
			Rect rect = new Rect(10f, 10f, 330f, Text.CalcHeight(text, 330f));
			Widgets.Label(rect, text);
			GUI.color = Color.white;
			LatestVersionGetter component = Current.Root.gameObject.GetComponent<LatestVersionGetter>();
			Rect rect2 = new Rect(10f, rect.yMax - 5f, 330f, 999f);
			component.DrawAt(rect2);
		}

		// Token: 0x06002F80 RID: 12160 RVA: 0x001194B4 File Offset: 0x001176B4
		private static string GetVersionExtraInfo()
		{
			string text = "";
			if (UnityData.Is32BitBuild)
			{
				text += "32-bit";
			}
			else if (UnityData.Is64BitBuild)
			{
				text += "64-bit";
			}
			return text;
		}

		// Token: 0x06002F81 RID: 12161 RVA: 0x001194F0 File Offset: 0x001176F0
		public static void LogVersionNumber()
		{
			Log.Message("RimWorld " + VersionControl.versionStringWithRev);
		}

		// Token: 0x06002F82 RID: 12162 RVA: 0x00119506 File Offset: 0x00117706
		public static bool IsCompatible(Version v)
		{
			return v.Major == VersionControl.CurrentMajor && v.Minor == VersionControl.CurrentMinor;
		}

		// Token: 0x06002F83 RID: 12163 RVA: 0x00119524 File Offset: 0x00117724
		public static bool TryParseVersionString(string str, out Version version)
		{
			version = null;
			if (str == null)
			{
				return false;
			}
			string[] array = str.Split(new char[]
			{
				'.'
			});
			if (array.Length < 2)
			{
				return false;
			}
			for (int i = 0; i < 2; i++)
			{
				int num;
				if (!int.TryParse(array[i], out num))
				{
					return false;
				}
				if (num < 0)
				{
					return false;
				}
			}
			version = new Version(int.Parse(array[0]), int.Parse(array[1]));
			return true;
		}

		// Token: 0x06002F84 RID: 12164 RVA: 0x0011958C File Offset: 0x0011778C
		public static int BuildFromVersionString(string str)
		{
			str = VersionControl.VersionStringWithoutRev(str);
			int result = 0;
			string[] array = str.Split(new char[]
			{
				'.'
			});
			if (array.Length < 3 || !int.TryParse(array[2], out result))
			{
				Log.Warning("Could not get build from version string " + str);
			}
			return result;
		}

		// Token: 0x06002F85 RID: 12165 RVA: 0x001195D8 File Offset: 0x001177D8
		public static int MinorFromVersionString(string str)
		{
			str = VersionControl.VersionStringWithoutRev(str);
			int result = 0;
			string[] array = str.Split(new char[]
			{
				'.'
			});
			if (array.Length < 2 || !int.TryParse(array[1], out result))
			{
				Log.Warning("Could not get minor version from version string " + str);
			}
			return result;
		}

		// Token: 0x06002F86 RID: 12166 RVA: 0x00119624 File Offset: 0x00117824
		public static int MajorFromVersionString(string str)
		{
			str = VersionControl.VersionStringWithoutRev(str);
			int result = 0;
			if (!int.TryParse(str.Split(new char[]
			{
				'.'
			})[0], out result))
			{
				Log.Warning("Could not get major version from version string " + str);
			}
			return result;
		}

		// Token: 0x06002F87 RID: 12167 RVA: 0x00119668 File Offset: 0x00117868
		public static string VersionStringWithoutRev(string str)
		{
			return str.Split(new char[]
			{
				' '
			})[0];
		}

		// Token: 0x06002F88 RID: 12168 RVA: 0x00119680 File Offset: 0x00117880
		public static Version VersionFromString(string str)
		{
			if (str.NullOrEmpty())
			{
				throw new ArgumentException("str");
			}
			string[] array = str.Split(new char[]
			{
				'.'
			});
			if (array.Length > 3)
			{
				throw new ArgumentException("str");
			}
			int major = 0;
			int minor = 0;
			int build = 0;
			for (int i = 0; i < 3; i++)
			{
				int num;
				if (!int.TryParse(array[i], out num))
				{
					throw new ArgumentException("str");
				}
				if (num < 0)
				{
					throw new ArgumentException("str");
				}
				switch (i)
				{
				case 0:
					major = num;
					break;
				case 1:
					minor = num;
					break;
				case 2:
					build = num;
					break;
				}
			}
			return new Version(major, minor, build);
		}

		// Token: 0x06002F89 RID: 12169 RVA: 0x0011972C File Offset: 0x0011792C
		public static bool IsWellFormattedVersionString(string str)
		{
			string[] array = str.Split(new char[]
			{
				'.'
			});
			if (array.Length != 2)
			{
				return false;
			}
			for (int i = 0; i < 2; i++)
			{
				int num;
				if (!int.TryParse(array[i], out num))
				{
					return false;
				}
				if (num < 0)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x04001CFC RID: 7420
		private static Version version;

		// Token: 0x04001CFD RID: 7421
		private static string versionStringWithoutBuild;

		// Token: 0x04001CFE RID: 7422
		private static string versionString;

		// Token: 0x04001CFF RID: 7423
		private static string versionStringWithRev;

		// Token: 0x04001D00 RID: 7424
		private static DateTime buildDate;
	}
}
