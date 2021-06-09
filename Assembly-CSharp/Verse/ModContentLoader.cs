using System;
using System.Collections.Generic;
using System.IO;
using RimWorld.IO;
using RuntimeAudioClipLoader;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200033F RID: 831
	public static class ModContentLoader<T> where T : class
	{
		// Token: 0x06001514 RID: 5396 RVA: 0x000D1640 File Offset: 0x000CF840
		public static bool IsAcceptableExtension(string extension)
		{
			string[] array;
			if (typeof(T) == typeof(AudioClip))
			{
				array = ModContentLoader<T>.AcceptableExtensionsAudio;
			}
			else if (typeof(T) == typeof(Texture2D))
			{
				array = ModContentLoader<T>.AcceptableExtensionsTexture;
			}
			else
			{
				if (!(typeof(T) == typeof(string)))
				{
					Log.Error("Unknown content type " + typeof(T), false);
					return false;
				}
				array = ModContentLoader<T>.AcceptableExtensionsString;
			}
			foreach (string b in array)
			{
				if (extension.ToLower() == b)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001515 RID: 5397 RVA: 0x000150E1 File Offset: 0x000132E1
		public static IEnumerable<Pair<string, LoadedContentItem<T>>> LoadAllForMod(ModContentPack mod)
		{
			DeepProfiler.Start(string.Concat(new object[]
			{
				"Loading assets of type ",
				typeof(T),
				" for mod ",
				mod
			}));
			Dictionary<string, FileInfo> allFilesForMod = ModContentPack.GetAllFilesForMod(mod, GenFilePaths.ContentPath<T>(), new Func<string, bool>(ModContentLoader<T>.IsAcceptableExtension), null);
			foreach (KeyValuePair<string, FileInfo> keyValuePair in allFilesForMod)
			{
				LoadedContentItem<T> loadedContentItem = ModContentLoader<T>.LoadItem(keyValuePair.Value);
				if (loadedContentItem != null)
				{
					yield return new Pair<string, LoadedContentItem<T>>(keyValuePair.Key, loadedContentItem);
				}
			}
			Dictionary<string, FileInfo>.Enumerator enumerator = default(Dictionary<string, FileInfo>.Enumerator);
			DeepProfiler.End();
			yield break;
			yield break;
		}

		// Token: 0x06001516 RID: 5398 RVA: 0x000D16F8 File Offset: 0x000CF8F8
		public static LoadedContentItem<T> LoadItem(VirtualFile file)
		{
			try
			{
				if (typeof(T) == typeof(string))
				{
					return new LoadedContentItem<T>(file, (T)((object)file.ReadAllText()), null);
				}
				if (typeof(T) == typeof(Texture2D))
				{
					return new LoadedContentItem<T>(file, (T)((object)ModContentLoader<T>.LoadTexture(file)), null);
				}
				if (typeof(T) == typeof(AudioClip))
				{
					if (Prefs.LogVerbose)
					{
						DeepProfiler.Start("Loading file " + file);
					}
					IDisposable extraDisposable = null;
					T t;
					try
					{
						bool doStream = ModContentLoader<T>.ShouldStreamAudioClipFromFile(file);
						Stream stream = file.CreateReadStream();
						try
						{
							t = (T)((object)Manager.Load(stream, ModContentLoader<T>.GetFormat(file.Name), file.Name, doStream, true, true));
						}
						catch (Exception)
						{
							stream.Dispose();
							throw;
						}
						extraDisposable = stream;
					}
					finally
					{
						if (Prefs.LogVerbose)
						{
							DeepProfiler.End();
						}
					}
					UnityEngine.Object @object = t as UnityEngine.Object;
					if (@object != null)
					{
						@object.name = Path.GetFileNameWithoutExtension(file.Name);
					}
					return new LoadedContentItem<T>(file, t, extraDisposable);
				}
			}
			catch (Exception ex)
			{
				Log.Error(string.Concat(new object[]
				{
					"Exception loading ",
					typeof(T),
					" from file.\nabsFilePath: ",
					file.FullPath,
					"\nException: ",
					ex.ToString()
				}), false);
			}
			if (typeof(T) == typeof(Texture2D))
			{
				return (LoadedContentItem<T>)new LoadedContentItem<Texture2D>(file, BaseContent.BadTex, null);
			}
			return null;
		}

		// Token: 0x06001517 RID: 5399 RVA: 0x000D18F4 File Offset: 0x000CFAF4
		private static AudioFormat GetFormat(string filename)
		{
			string extension = Path.GetExtension(filename);
			if (extension == ".ogg")
			{
				return AudioFormat.ogg;
			}
			if (extension == ".mp3")
			{
				return AudioFormat.mp3;
			}
			if (extension == ".aiff" || extension == ".aif" || extension == ".aifc")
			{
				return AudioFormat.aiff;
			}
			if (!(extension == ".wav"))
			{
				return AudioFormat.unknown;
			}
			return AudioFormat.wav;
		}

		// Token: 0x06001518 RID: 5400 RVA: 0x000150F1 File Offset: 0x000132F1
		private static AudioType GetAudioTypeFromURI(string uri)
		{
			if (uri.EndsWith(".ogg"))
			{
				return AudioType.OGGVORBIS;
			}
			return AudioType.WAV;
		}

		// Token: 0x06001519 RID: 5401 RVA: 0x00015105 File Offset: 0x00013305
		private static bool ShouldStreamAudioClipFromFile(VirtualFile file)
		{
			return file is FilesystemFile && file.Exists && file.Length > 307200L;
		}

		// Token: 0x0600151A RID: 5402 RVA: 0x000D1964 File Offset: 0x000CFB64
		private static Texture2D LoadTexture(VirtualFile file)
		{
			Texture2D texture2D = null;
			if (file.Exists)
			{
				byte[] data = file.ReadAllBytes();
				texture2D = new Texture2D(2, 2, TextureFormat.Alpha8, true);
				texture2D.LoadImage(data);
				texture2D.Compress(true);
				texture2D.name = Path.GetFileNameWithoutExtension(file.Name);
				texture2D.filterMode = FilterMode.Trilinear;
				texture2D.anisoLevel = 2;
				texture2D.Apply(true, true);
			}
			return texture2D;
		}

		// Token: 0x0400105E RID: 4190
		private static string[] AcceptableExtensionsAudio = new string[]
		{
			".wav",
			".mp3",
			".ogg",
			".xm",
			".it",
			".mod",
			".s3m"
		};

		// Token: 0x0400105F RID: 4191
		private static string[] AcceptableExtensionsTexture = new string[]
		{
			".png",
			".jpg",
			".jpeg",
			".psd"
		};

		// Token: 0x04001060 RID: 4192
		private static string[] AcceptableExtensionsString = new string[]
		{
			".txt"
		};
	}
}
