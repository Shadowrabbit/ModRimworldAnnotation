using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200033A RID: 826
	public class ModAssetBundlesHandler
	{
		// Token: 0x060014FE RID: 5374 RVA: 0x00014FF3 File Offset: 0x000131F3
		public ModAssetBundlesHandler(ModContentPack mod)
		{
			this.mod = mod;
		}

		// Token: 0x060014FF RID: 5375 RVA: 0x000D1184 File Offset: 0x000CF384
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
						Log.Error("Could not load asset bundle at " + fileInfo.FullName, false);
					}
				}
			}
		}

		// Token: 0x06001500 RID: 5376 RVA: 0x0001500D File Offset: 0x0001320D
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

		// Token: 0x0400104C RID: 4172
		private ModContentPack mod;

		// Token: 0x0400104D RID: 4173
		public List<AssetBundle> loadedAssetBundles = new List<AssetBundle>();

		// Token: 0x0400104E RID: 4174
		public static readonly string[] TextureExtensions = new string[]
		{
			".png",
			".jpg",
			".jpeg",
			".psd"
		};

		// Token: 0x0400104F RID: 4175
		public static readonly string[] AudioClipExtensions = new string[]
		{
			".wav",
			".mp3"
		};
	}
}
