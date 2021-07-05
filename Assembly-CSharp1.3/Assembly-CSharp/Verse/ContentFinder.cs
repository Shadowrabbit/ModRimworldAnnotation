using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000232 RID: 562
	public static class ContentFinder<T> where T : class
	{
		// Token: 0x06000FF9 RID: 4089 RVA: 0x0005A880 File Offset: 0x00058A80
		public static T Get(string itemPath, bool reportFailure = true)
		{
			if (!UnityData.IsInMainThread)
			{
				Log.Error("Tried to get a resource \"" + itemPath + "\" from a different thread. All resources must be loaded in the main thread.");
				return default(T);
			}
			T t = default(T);
			List<ModContentPack> runningModsListForReading = LoadedModManager.RunningModsListForReading;
			for (int i = runningModsListForReading.Count - 1; i >= 0; i--)
			{
				t = runningModsListForReading[i].GetContentHolder<T>().Get(itemPath);
				if (t != null)
				{
					return t;
				}
			}
			if (typeof(T) == typeof(Texture2D))
			{
				t = (T)((object)Resources.Load<Texture2D>(GenFilePaths.ContentPath<Texture2D>() + itemPath));
			}
			if (typeof(T) == typeof(AudioClip))
			{
				t = (T)((object)Resources.Load<AudioClip>(GenFilePaths.ContentPath<AudioClip>() + itemPath));
			}
			if (t != null)
			{
				return t;
			}
			for (int j = runningModsListForReading.Count - 1; j >= 0; j--)
			{
				for (int k = 0; k < runningModsListForReading[j].assetBundles.loadedAssetBundles.Count; k++)
				{
					AssetBundle assetBundle = runningModsListForReading[j].assetBundles.loadedAssetBundles[k];
					string path = Path.Combine("Assets", "Data");
					path = Path.Combine(path, runningModsListForReading[j].FolderName);
					if (typeof(T) == typeof(Texture2D))
					{
						string str = Path.Combine(Path.Combine(path, GenFilePaths.ContentPath<Texture2D>()), itemPath);
						for (int l = 0; l < ModAssetBundlesHandler.TextureExtensions.Length; l++)
						{
							t = (T)((object)assetBundle.LoadAsset<Texture2D>(str + ModAssetBundlesHandler.TextureExtensions[l]));
							if (t != null)
							{
								return t;
							}
						}
					}
					if (typeof(T) == typeof(AudioClip))
					{
						string str2 = Path.Combine(Path.Combine(path, GenFilePaths.ContentPath<AudioClip>()), itemPath);
						for (int m = 0; m < ModAssetBundlesHandler.AudioClipExtensions.Length; m++)
						{
							t = (T)((object)assetBundle.LoadAsset<AudioClip>(str2 + ModAssetBundlesHandler.AudioClipExtensions[m]));
							if (t != null)
							{
								return t;
							}
						}
					}
				}
			}
			if (reportFailure)
			{
				Log.Error(string.Concat(new object[]
				{
					"Could not load ",
					typeof(T),
					" at ",
					itemPath,
					" in any active mod or in base resources."
				}));
			}
			return default(T);
		}

		// Token: 0x06000FFA RID: 4090 RVA: 0x0005AB03 File Offset: 0x00058D03
		public static IEnumerable<T> GetAllInFolder(string folderPath)
		{
			if (!UnityData.IsInMainThread)
			{
				Log.Error("Tried to get all resources in a folder \"" + folderPath + "\" from a different thread. All resources must be loaded in the main thread.");
				yield break;
			}
			foreach (ModContentPack modContentPack in LoadedModManager.RunningMods)
			{
				foreach (T t in modContentPack.GetContentHolder<T>().GetAllUnderPath(folderPath))
				{
					yield return t;
				}
				IEnumerator<T> enumerator2 = null;
			}
			IEnumerator<ModContentPack> enumerator = null;
			T[] array = null;
			if (typeof(T) == typeof(Texture2D))
			{
				array = (T[])Resources.LoadAll<Texture2D>(GenFilePaths.ContentPath<Texture2D>() + folderPath);
			}
			if (typeof(T) == typeof(AudioClip))
			{
				array = (T[])Resources.LoadAll<AudioClip>(GenFilePaths.ContentPath<AudioClip>() + folderPath);
			}
			if (array != null)
			{
				foreach (T t2 in array)
				{
					yield return t2;
				}
				T[] array2 = null;
			}
			List<ModContentPack> mods = LoadedModManager.RunningModsListForReading;
			int num;
			for (int j = mods.Count - 1; j >= 0; j = num - 1)
			{
				for (int i = 0; i < mods[j].assetBundles.loadedAssetBundles.Count; i = num + 1)
				{
					AssetBundle assetBundle = mods[j].assetBundles.loadedAssetBundles[i];
					string dirForBundle = Path.Combine("Assets", "Data");
					dirForBundle = Path.Combine(dirForBundle, mods[j].FolderName);
					if (typeof(T) == typeof(Texture2D))
					{
						string fullPath = Path.Combine(Path.Combine(dirForBundle, GenFilePaths.ContentPath<Texture2D>()).Replace('\\', '/'), folderPath).ToLower();
						IEnumerable<string> enumerable = from p in mods[j].AllAssetNamesInBundle(i)
						where p.StartsWith(fullPath)
						select p;
						foreach (string text in enumerable)
						{
							if (ModAssetBundlesHandler.TextureExtensions.Contains(Path.GetExtension(text)))
							{
								yield return (T)((object)assetBundle.LoadAsset<Texture2D>(text));
							}
						}
						IEnumerator<string> enumerator3 = null;
					}
					if (typeof(T) == typeof(AudioClip))
					{
						string fullPath = Path.Combine(Path.Combine(dirForBundle, GenFilePaths.ContentPath<AudioClip>()).Replace('\\', '/'), folderPath).ToLower();
						IEnumerable<string> enumerable2 = from p in mods[j].AllAssetNamesInBundle(i)
						where p.StartsWith(fullPath)
						select p;
						foreach (string text2 in enumerable2)
						{
							if (ModAssetBundlesHandler.AudioClipExtensions.Contains(Path.GetExtension(text2)))
							{
								yield return (T)((object)assetBundle.LoadAsset<AudioClip>(text2));
							}
						}
						IEnumerator<string> enumerator3 = null;
					}
					assetBundle = null;
					dirForBundle = null;
					num = i;
				}
				num = j;
			}
			yield break;
			yield break;
		}
	}
}
