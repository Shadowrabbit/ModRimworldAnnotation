using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200033B RID: 827
	public class ModContentHolder<T> where T : class
	{
		// Token: 0x06001503 RID: 5379 RVA: 0x00015020 File Offset: 0x00013220
		public ModContentHolder(ModContentPack mod)
		{
			this.mod = mod;
		}

		// Token: 0x06001504 RID: 5380 RVA: 0x000D12B4 File Offset: 0x000CF4B4
		public void ClearDestroy()
		{
			if (typeof(UnityEngine.Object).IsAssignableFrom(typeof(T)))
			{
				foreach (T localObj2 in this.contentList.Values)
				{
					T localObj = localObj2;
					LongEventHandler.ExecuteWhenFinished(delegate
					{
						UnityEngine.Object.Destroy((UnityEngine.Object)((object)localObj));
					});
				}
			}
			for (int i = 0; i < this.extraDisposables.Count; i++)
			{
				this.extraDisposables[i].Dispose();
			}
			this.extraDisposables.Clear();
			this.contentList.Clear();
		}

		// Token: 0x06001505 RID: 5381 RVA: 0x000D137C File Offset: 0x000CF57C
		public void ReloadAll()
		{
			foreach (Pair<string, LoadedContentItem<T>> pair in ModContentLoader<T>.LoadAllForMod(this.mod))
			{
				string text = pair.First;
				text = text.Replace('\\', '/');
				string text2 = GenFilePaths.ContentPath<T>();
				if (text.StartsWith(text2))
				{
					text = text.Substring(text2.Length);
				}
				if (text.EndsWith(Path.GetExtension(text)))
				{
					text = text.Substring(0, text.Length - Path.GetExtension(text).Length);
				}
				if (this.contentList.ContainsKey(text))
				{
					Log.Warning(string.Concat(new object[]
					{
						"Tried to load duplicate ",
						typeof(T),
						" with path: ",
						pair.Second.internalFile,
						" and internal path: ",
						text
					}), false);
				}
				else
				{
					this.contentList.Add(text, pair.Second.contentItem);
					if (pair.Second.extraDisposable != null)
					{
						this.extraDisposables.Add(pair.Second.extraDisposable);
					}
				}
			}
		}

		// Token: 0x06001506 RID: 5382 RVA: 0x000D14C8 File Offset: 0x000CF6C8
		public T Get(string path)
		{
			T result;
			if (this.contentList.TryGetValue(path, out result))
			{
				return result;
			}
			return default(T);
		}

		// Token: 0x06001507 RID: 5383 RVA: 0x00015045 File Offset: 0x00013245
		public IEnumerable<T> GetAllUnderPath(string pathRoot)
		{
			foreach (KeyValuePair<string, T> keyValuePair in this.contentList)
			{
				if (keyValuePair.Key.StartsWith(pathRoot))
				{
					yield return keyValuePair.Value;
				}
			}
			Dictionary<string, T>.Enumerator enumerator = default(Dictionary<string, T>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x04001050 RID: 4176
		private ModContentPack mod;

		// Token: 0x04001051 RID: 4177
		public Dictionary<string, T> contentList = new Dictionary<string, T>();

		// Token: 0x04001052 RID: 4178
		public List<IDisposable> extraDisposables = new List<IDisposable>();
	}
}
