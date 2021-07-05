using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000237 RID: 567
	public class ModAssetBundlesHandler
	{
		// Token: 0x0600101C RID: 4124 RVA: 0x0005BD34 File Offset: 0x00059F34
		public ModAssetBundlesHandler(ModContentPack mod)
		{
			this.mod = mod;
		}

		// Token: 0x0600101D RID: 4125 RVA: 0x0005BD50 File Offset: 0x00059F50
		public void ReloadAll()
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(Path.Combine(this.mod.RootDir, "AssetBundles"));
			if (!directoryInfo.Exists)
			{
				return;
			}
			foreach (FileInfo fileInfo in directoryInfo.GetFiles("*", SearchOption.AllDirectories))
			{
				if (fileInfo.Extension.NullOrEmpty())
				{
					AssetBundle assetBundle = AssetBundle.LoadFromFile(fileInfo.FullName);
					if (assetBundle != null)
					{
						this.loadedAssetBundles.Add(assetBundle);
					}
					else
					{
						Log.Error("Could not load asset bundle at " + fileInfo.FullName);
					}
				}
			}
		}

		// Token: 0x0600101E RID: 4126 RVA: 0x0005BDE8 File Offset: 0x00059FE8
		public void ClearDestroy()
		{
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				for (int i = 0; i < this.loadedAssetBundles.Count; i++)
				{
					this.loadedAssetBundles[i].Unload(true);
				}
				this.loadedAssetBundles.Clear();
			});
		}

		// Token: 0x04000C84 RID: 3204
		private ModContentPack mod;

		// Token: 0x04000C85 RID: 3205
		public List<AssetBundle> loadedAssetBundles = new List<AssetBundle>();

		// Token: 0x04000C86 RID: 3206
		public static readonly string[] TextureExtensions = new string[]
		{
			".png",
			".jpg",
			".jpeg",
			".psd"
		};

		// Token: 0x04000C87 RID: 3207
		public static readonly string[] AudioClipExtensions = new string[]
		{
			".wav",
			".mp3"
		};
	}
}
