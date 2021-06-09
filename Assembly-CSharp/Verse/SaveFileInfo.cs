using System;
using System.IO;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004A2 RID: 1186
	public struct SaveFileInfo
	{
		// Token: 0x1700058E RID: 1422
		// (get) Token: 0x06001DA1 RID: 7585 RVA: 0x0001A842 File Offset: 0x00018A42
		public bool Valid
		{
			get
			{
				return this.gameVersion != null;
			}
		}

		// Token: 0x1700058F RID: 1423
		// (get) Token: 0x06001DA2 RID: 7586 RVA: 0x0001A84D File Offset: 0x00018A4D
		public FileInfo FileInfo
		{
			get
			{
				return this.fileInfo;
			}
		}

		// Token: 0x17000590 RID: 1424
		// (get) Token: 0x06001DA3 RID: 7587 RVA: 0x0001A855 File Offset: 0x00018A55
		public string GameVersion
		{
			get
			{
				if (!this.Valid)
				{
					return "???";
				}
				return this.gameVersion;
			}
		}

		// Token: 0x17000591 RID: 1425
		// (get) Token: 0x06001DA4 RID: 7588 RVA: 0x000F6CE4 File Offset: 0x000F4EE4
		public Color VersionColor
		{
			get
			{
				if (!this.Valid)
				{
					return ColoredText.RedReadable;
				}
				if (VersionControl.MajorFromVersionString(this.gameVersion) == VersionControl.CurrentMajor && VersionControl.MinorFromVersionString(this.gameVersion) == VersionControl.CurrentMinor)
				{
					return SaveFileInfo.UnimportantTextColor;
				}
				if (BackCompatibility.IsSaveCompatibleWith(this.gameVersion))
				{
					return Color.yellow;
				}
				return ColoredText.RedReadable;
			}
		}

		// Token: 0x17000592 RID: 1426
		// (get) Token: 0x06001DA5 RID: 7589 RVA: 0x000F6D44 File Offset: 0x000F4F44
		public TipSignal CompatibilityTip
		{
			get
			{
				if (!this.Valid)
				{
					return "SaveIsUnknownFormat".Translate();
				}
				if ((VersionControl.MajorFromVersionString(this.gameVersion) != VersionControl.CurrentMajor || VersionControl.MinorFromVersionString(this.gameVersion) != VersionControl.CurrentMinor) && !BackCompatibility.IsSaveCompatibleWith(this.gameVersion))
				{
					return "SaveIsFromDifferentGameVersion".Translate(VersionControl.CurrentVersionString, this.gameVersion);
				}
				if (VersionControl.BuildFromVersionString(this.gameVersion) != VersionControl.CurrentBuild)
				{
					return "SaveIsFromDifferentGameBuild".Translate(VersionControl.CurrentVersionString, this.gameVersion);
				}
				return "SaveIsFromThisGameBuild".Translate();
			}
		}

		// Token: 0x06001DA6 RID: 7590 RVA: 0x0001A86B File Offset: 0x00018A6B
		public SaveFileInfo(FileInfo fileInfo)
		{
			this.fileInfo = fileInfo;
			this.gameVersion = ScribeMetaHeaderUtility.GameVersionOf(fileInfo);
		}

		// Token: 0x0400152C RID: 5420
		private FileInfo fileInfo;

		// Token: 0x0400152D RID: 5421
		private string gameVersion;

		// Token: 0x0400152E RID: 5422
		public static readonly Color UnimportantTextColor = new Color(1f, 1f, 1f, 0.5f);
	}
}
