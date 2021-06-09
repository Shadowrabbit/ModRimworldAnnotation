using System;
using System.Reflection;
using UnityEngine;
using Verse;
using Verse.Steam;

namespace RimWorld
{
	// Token: 0x02000B18 RID: 2840
	public static class VersionControl
	{
		// Token: 0x17000A44 RID: 2628
		// (get) Token: 0x0600424D RID: 16973 RVA: 0x00031640 File Offset: 0x0002F840
		public static Version CurrentVersion
		{
			get
			{
				return VersionControl.version;
			}
		}

		// Token: 0x17000A45 RID: 2629
		// (get) Token: 0x0600424E RID: 16974 RVA: 0x00031647 File Offset: 0x0002F847
		public static string CurrentVersionString
		{
			get
			{
				return VersionControl.versionString;
			}
		}

		// Token: 0x17000A46 RID: 2630
		// (get) Token: 0x0600424F RID: 16975 RVA: 0x0003164E File Offset: 0x0002F84E
		public static string CurrentVersionStringWithoutBuild
		{
			get
			{
				return VersionControl.versionStringWithoutBuild;
			}
		}

		// Token: 0x17000A47 RID: 2631
		// (get) Token: 0x06004250 RID: 16976 RVA: 0x00031655 File Offset: 0x0002F855
		public static string CurrentVersionStringWithRev
		{
			get
			{
				return VersionControl.versionStringWithRev;
			}
		}

		// Token: 0x17000A48 RID: 2632
		// (get) Token: 0x06004251 RID: 16977 RVA: 0x0003165C File Offset: 0x0002F85C
		public static int CurrentMajor
		{
			get
			{
				return VersionControl.version.Major;
			}
		}

		// Token: 0x17000A49 RID: 2633
		// (get) Token: 0x06004252 RID: 16978 RVA: 0x00031668 File Offset: 0x0002F868
		public static int CurrentMinor
		{
			get
			{
				return VersionControl.version.Minor;
			}
		}

		// Token: 0x17000A4A RID: 2634
		// (get) Token: 0x06004253 RID: 16979 RVA: 0x00031674 File Offset: 0x0002F874
		public static int CurrentBuild
		{
			get
			{
				return VersionControl.version.Build;
			}
		}

		// Token: 0x17000A4B RID: 2635
		// (get) Token: 0x06004254 RID: 16980 RVA: 0x00031680 File Offset: 0x0002F880
		public static int CurrentRevision
		{
			get
			{
				return VersionControl.version.Revision;
			}
		}

		// Token: 0x17000A4C RID: 2636
		// (get) Token: 0x06004255 RID: 16981 RVA: 0x0003168C File Offset: 0x0002F88C
		public static DateTime CurrentBuildDate
		{
			get
			{
				return VersionControl.buildDate;
			}
		}

		// Token: 0x06004256 RID: 16982 RVA: 0x00189834 File Offset: 0x00187A34
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

		// Token: 0x06004257 RID: 16983 RVA: 0x00189998 File Offset: 0x00187B98
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

		// Token: 0x06004258 RID: 16984 RVA: 0x00189AD4 File Offset: 0x00187CD4
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

		// Token: 0x06004259 RID: 16985 RVA: 0x00031693 File Offset: 0x0002F893
		public static void LogVersionNumber()
		{
			Log.Message("RimWorld " + VersionControl.versionStringWithRev, false);
		}

		// Token: 0x0600425A RID: 16986 RVA: 0x000316AA File Offset: 0x0002F8AA
		public static bool IsCompatible(Version v)
		{
			return v.Major == VersionControl.CurrentMajor && v.Minor == VersionControl.CurrentMinor;
		}

		// Token: 0x0600425B RID: 16987 RVA: 0x00189B10 File Offset: 0x00187D10
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

		// Token: 0x0600425C RID: 16988 RVA: 0x00189B78 File Offset: 0x00187D78
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
				Log.Warning("Could not get build from version string " + str, false);
			}
			return result;
		}

		// Token: 0x0600425D RID: 16989 RVA: 0x00189BC8 File Offset: 0x00187DC8
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
				Log.Warning("Could not get minor version from version string " + str, false);
			}
			return result;
		}

		// Token: 0x0600425E RID: 16990 RVA: 0x00189C18 File Offset: 0x00187E18
		public static int MajorFromVersionString(string str)
		{
			str = VersionControl.VersionStringWithoutRev(str);
			int result = 0;
			if (!int.TryParse(str.Split(new char[]
			{
				'.'
			})[0], out result))
			{
				Log.Warning("Could not get major version from version string " + str, false);
			}
			return result;
		}

		// Token: 0x0600425F RID: 16991 RVA: 0x000316C8 File Offset: 0x0002F8C8
		public static string VersionStringWithoutRev(string str)
		{
			return str.Split(new char[]
			{
				' '
			})[0];
		}

		// Token: 0x06004260 RID: 16992 RVA: 0x00189C60 File Offset: 0x00187E60
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

		// Token: 0x06004261 RID: 16993 RVA: 0x00189D0C File Offset: 0x00187F0C
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

		// Token: 0x04002D88 RID: 11656
		private static Version version;

		// Token: 0x04002D89 RID: 11657
		private static string versionStringWithoutBuild;

		// Token: 0x04002D8A RID: 11658
		private static string versionString;

		// Token: 0x04002D8B RID: 11659
		private static string versionStringWithRev;

		// Token: 0x04002D8C RID: 11660
		private static DateTime buildDate;
	}
}
