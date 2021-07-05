using System;
using System.IO;
using System.Threading;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200031F RID: 799
	public class SaveFileInfo
	{
		// Token: 0x170004B3 RID: 1203
		// (get) Token: 0x060016DB RID: 5851 RVA: 0x00086A74 File Offset: 0x00084C74
		private bool Valid
		{
			get
			{
				object obj = this.lockObject;
				bool result;
				lock (obj)
				{
					result = (this.gameVersion != null);
				}
				return result;
			}
		}

		// Token: 0x170004B4 RID: 1204
		// (get) Token: 0x060016DC RID: 5852 RVA: 0x00086ABC File Offset: 0x00084CBC
		public FileInfo FileInfo
		{
			get
			{
				return this.fileInfo;
			}
		}

		// Token: 0x170004B5 RID: 1205
		// (get) Token: 0x060016DD RID: 5853 RVA: 0x00086AC4 File Offset: 0x00084CC4
		public string FileName
		{
			get
			{
				return this.fileName;
			}
		}

		// Token: 0x170004B6 RID: 1206
		// (get) Token: 0x060016DE RID: 5854 RVA: 0x00086ACC File Offset: 0x00084CCC
		public DateTime LastWriteTime
		{
			get
			{
				return this.lastWriteTime;
			}
		}

		// Token: 0x170004B7 RID: 1207
		// (get) Token: 0x060016DF RID: 5855 RVA: 0x00086AD4 File Offset: 0x00084CD4
		public string GameVersion
		{
			get
			{
				bool flag = false;
				try
				{
					if (flag = this.TryLock(0))
					{
						if (!this.loaded)
						{
							return "LoadingVersionInfo".Translate();
						}
						if (!this.Valid)
						{
							return "???";
						}
						return this.gameVersion;
					}
				}
				finally
				{
					if (flag)
					{
						Monitor.Exit(this.lockObject);
					}
				}
				return "LoadingVersionInfo".Translate();
			}
		}

		// Token: 0x170004B8 RID: 1208
		// (get) Token: 0x060016E0 RID: 5856 RVA: 0x00086B54 File Offset: 0x00084D54
		public Color VersionColor
		{
			get
			{
				bool flag = false;
				try
				{
					if (flag = this.TryLock(0))
					{
						if (!this.loaded)
						{
							return Color.gray;
						}
						if (!this.Valid)
						{
							return ColorLibrary.RedReadable;
						}
						if (VersionControl.MajorFromVersionString(this.gameVersion) == VersionControl.CurrentMajor && VersionControl.MinorFromVersionString(this.gameVersion) == VersionControl.CurrentMinor)
						{
							return SaveFileInfo.UnimportantTextColor;
						}
						if (BackCompatibility.IsSaveCompatibleWith(this.gameVersion))
						{
							return Color.yellow;
						}
						return ColorLibrary.RedReadable;
					}
				}
				finally
				{
					if (flag)
					{
						Monitor.Exit(this.lockObject);
					}
				}
				return Color.gray;
			}
		}

		// Token: 0x170004B9 RID: 1209
		// (get) Token: 0x060016E1 RID: 5857 RVA: 0x00086C00 File Offset: 0x00084E00
		public TipSignal CompatibilityTip
		{
			get
			{
				bool flag = false;
				try
				{
					if (flag = this.TryLock(0))
					{
						if (!this.loaded)
						{
							return "LoadingVersionInfo".Translate();
						}
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
				finally
				{
					if (flag)
					{
						Monitor.Exit(this.lockObject);
					}
				}
				return "LoadingVersionInfo".Translate();
			}
		}

		// Token: 0x060016E2 RID: 5858 RVA: 0x00086D2C File Offset: 0x00084F2C
		private bool TryLock(int timeoutMilliseconds)
		{
			return Monitor.TryEnter(this.lockObject, timeoutMilliseconds);
		}

		// Token: 0x060016E3 RID: 5859 RVA: 0x00086D3A File Offset: 0x00084F3A
		public SaveFileInfo(FileInfo fileInfo)
		{
			this.fileInfo = fileInfo;
			this.fileName = fileInfo.Name;
			this.lastWriteTime = fileInfo.LastWriteTime;
		}

		// Token: 0x060016E4 RID: 5860 RVA: 0x00086D6C File Offset: 0x00084F6C
		public void LoadData()
		{
			object obj = this.lockObject;
			lock (obj)
			{
				this.gameVersion = ScribeMetaHeaderUtility.GameVersionOf(this.fileInfo);
				this.loaded = true;
			}
		}

		// Token: 0x04000FE2 RID: 4066
		private FileInfo fileInfo;

		// Token: 0x04000FE3 RID: 4067
		private string gameVersion;

		// Token: 0x04000FE4 RID: 4068
		private DateTime lastWriteTime;

		// Token: 0x04000FE5 RID: 4069
		private string fileName;

		// Token: 0x04000FE6 RID: 4070
		private bool loaded;

		// Token: 0x04000FE7 RID: 4071
		private object lockObject = new object();

		// Token: 0x04000FE8 RID: 4072
		public static readonly Color UnimportantTextColor = new Color(1f, 1f, 1f, 0.5f);
	}
}
