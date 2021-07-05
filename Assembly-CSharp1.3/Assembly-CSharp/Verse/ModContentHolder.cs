using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000238 RID: 568
	public class ModContentHolder<T> where T : class
	{
		// Token: 0x06001021 RID: 4129 RVA: 0x0005BE90 File Offset: 0x0005A090
		public ModContentHolder(ModContentPack mod)
		{
			this.mod = mod;
		}

		// Token: 0x06001022 RID: 4130 RVA: 0x0005BEB8 File Offset: 0x0005A0B8
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

		// Token: 0x06001023 RID: 4131 RVA: 0x0005BF80 File Offset: 0x0005A180
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
					}));
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

		// Token: 0x06001024 RID: 4132 RVA: 0x0005C0CC File Offset: 0x0005A2CC
		public T Get(string path)
		{
			T result;
			if (this.contentList.TryGetValue(path, out result))
			{
				return result;
			}
			return default(T);
		}

		// Token: 0x06001025 RID: 4133 RVA: 0x0005C0F4 File Offset: 0x0005A2F4
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

		// Token: 0x04000C88 RID: 3208
		private ModContentPack mod;

		// Token: 0x04000C89 RID: 3209
		public Dictionary<string, T> contentList = new Dictionary<string, T>();

		// Token: 0x04000C8A RID: 3210
		public List<IDisposable> extraDisposables = new List<IDisposable>();
	}
}
