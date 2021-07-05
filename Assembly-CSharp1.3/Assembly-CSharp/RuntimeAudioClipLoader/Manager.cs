using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using UnityEngine;
using Verse;

namespace RuntimeAudioClipLoader
{
	// Token: 0x02000010 RID: 16
	[StaticConstructorOnStartup]
	public class Manager : MonoBehaviour
	{
		// Token: 0x06000052 RID: 82 RVA: 0x00003F88 File Offset: 0x00002188
		static Manager()
		{
			Manager.supportedFormats = Enum.GetNames(typeof(AudioFormat));
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00003FE8 File Offset: 0x000021E8
		public static AudioClip Load(string filePath, bool doStream = false, bool loadInBackground = true, bool useCache = true)
		{
			if (!Manager.IsSupportedFormat(filePath))
			{
				Debug.LogError("Could not load AudioClip at path '" + filePath + "' it's extensions marks unsupported format, supported formats are: " + string.Join(", ", Enum.GetNames(typeof(AudioFormat))));
				return null;
			}
			AudioClip audioClip = null;
			if (useCache && Manager.cache.TryGetValue(filePath, out audioClip) && audioClip)
			{
				return audioClip;
			}
			audioClip = Manager.Load(new StreamReader(filePath).BaseStream, Manager.GetAudioFormat(filePath), filePath, doStream, loadInBackground, true);
			if (useCache)
			{
				Manager.cache[filePath] = audioClip;
			}
			return audioClip;
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00004078 File Offset: 0x00002278
		public static AudioClip Load(Stream dataStream, AudioFormat audioFormat, string unityAudioClipName, bool doStream = false, bool loadInBackground = true, bool diposeDataStreamIfNotNeeded = true)
		{
			AudioClip audioClip = null;
			CustomAudioFileReader reader = null;
			try
			{
				reader = new CustomAudioFileReader(dataStream, audioFormat);
				Manager.AudioInstance audioInstance = new Manager.AudioInstance
				{
					reader = reader,
					samplesCount = (int)(reader.Length / (long)(reader.WaveFormat.BitsPerSample / 8))
				};
				if (doStream)
				{
					audioClip = AudioClip.Create(unityAudioClipName, audioInstance.samplesCount / audioInstance.channels, audioInstance.channels, audioInstance.sampleRate, doStream, delegate(float[] target)
					{
						reader.Read(target, 0, target.Length);
					}, delegate(int target)
					{
						reader.Seek((long)target, SeekOrigin.Begin);
					});
					audioInstance.audioClip = audioClip;
					Manager.SetAudioClipLoadType(audioInstance, AudioClipLoadType.Streaming);
					Manager.SetAudioClipLoadState(audioInstance, AudioDataLoadState.Loaded);
				}
				else
				{
					audioClip = AudioClip.Create(unityAudioClipName, audioInstance.samplesCount / audioInstance.channels, audioInstance.channels, audioInstance.sampleRate, doStream);
					audioInstance.audioClip = audioClip;
					if (diposeDataStreamIfNotNeeded)
					{
						audioInstance.streamToDisposeOnceDone = dataStream;
					}
					Manager.SetAudioClipLoadType(audioInstance, AudioClipLoadType.DecompressOnLoad);
					Manager.SetAudioClipLoadState(audioInstance, AudioDataLoadState.Loading);
					if (loadInBackground)
					{
						Queue<Manager.AudioInstance> obj = Manager.deferredLoadQueue;
						lock (obj)
						{
							Manager.deferredLoadQueue.Enqueue(audioInstance);
						}
						Manager.RunDeferredLoaderThread();
						Manager.EnsureInstanceExists();
					}
					else
					{
						audioInstance.dataToSet = new float[audioInstance.samplesCount];
						audioInstance.reader.Read(audioInstance.dataToSet, 0, audioInstance.dataToSet.Length);
						audioInstance.audioClip.SetData(audioInstance.dataToSet, 0);
						Manager.SetAudioClipLoadState(audioInstance, AudioDataLoadState.Loaded);
					}
				}
			}
			catch (Exception ex)
			{
				Manager.SetAudioClipLoadState(audioClip, AudioDataLoadState.Failed);
				Debug.LogError(string.Concat(new object[]
				{
					"Could not load AudioClip named '",
					unityAudioClipName,
					"', exception:",
					ex
				}));
			}
			return audioClip;
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00004274 File Offset: 0x00002474
		private static void RunDeferredLoaderThread()
		{
			if (Manager.deferredLoaderThread == null || !Manager.deferredLoaderThread.IsAlive)
			{
				Manager.deferredLoaderThread = new Thread(new ThreadStart(Manager.DeferredLoaderMain));
				Manager.deferredLoaderThread.IsBackground = true;
				Manager.deferredLoaderThread.Start();
			}
		}

		// Token: 0x06000056 RID: 86 RVA: 0x000042B4 File Offset: 0x000024B4
		private static void DeferredLoaderMain()
		{
			Manager.AudioInstance audioInstance = null;
			bool flag = true;
			long num = 100000L;
			while (flag || num > 0L)
			{
				num -= 1L;
				Queue<Manager.AudioInstance> obj = Manager.deferredLoadQueue;
				lock (obj)
				{
					flag = (Manager.deferredLoadQueue.Count > 0);
					if (!flag)
					{
						continue;
					}
					audioInstance = Manager.deferredLoadQueue.Dequeue();
				}
				num = 100000L;
				try
				{
					audioInstance.dataToSet = new float[audioInstance.samplesCount];
					audioInstance.reader.Read(audioInstance.dataToSet, 0, audioInstance.dataToSet.Length);
					audioInstance.reader.Close();
					audioInstance.reader.Dispose();
					if (audioInstance.streamToDisposeOnceDone != null)
					{
						audioInstance.streamToDisposeOnceDone.Close();
						audioInstance.streamToDisposeOnceDone.Dispose();
						audioInstance.streamToDisposeOnceDone = null;
					}
					obj = Manager.deferredSetDataQueue;
					lock (obj)
					{
						Manager.deferredSetDataQueue.Enqueue(audioInstance);
					}
				}
				catch (Exception exception)
				{
					Debug.LogException(exception);
					obj = Manager.deferredSetFail;
					lock (obj)
					{
						Manager.deferredSetFail.Enqueue(audioInstance);
					}
				}
			}
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00004420 File Offset: 0x00002620
		private void Update()
		{
			Manager.AudioInstance audioInstance = null;
			bool flag = true;
			Queue<Manager.AudioInstance> obj;
			while (flag)
			{
				obj = Manager.deferredSetDataQueue;
				lock (obj)
				{
					flag = (Manager.deferredSetDataQueue.Count > 0);
					if (!flag)
					{
						break;
					}
					audioInstance = Manager.deferredSetDataQueue.Dequeue();
				}
				audioInstance.audioClip.SetData(audioInstance.dataToSet, 0);
				Manager.SetAudioClipLoadState(audioInstance, AudioDataLoadState.Loaded);
				audioInstance.audioClip = null;
				audioInstance.dataToSet = null;
			}
			obj = Manager.deferredSetFail;
			lock (obj)
			{
				while (Manager.deferredSetFail.Count > 0)
				{
					audioInstance = Manager.deferredSetFail.Dequeue();
					Manager.SetAudioClipLoadState(audioInstance, AudioDataLoadState.Failed);
				}
			}
		}

		// Token: 0x06000058 RID: 88 RVA: 0x000044FC File Offset: 0x000026FC
		private static void EnsureInstanceExists()
		{
			if (!Manager.managerInstance)
			{
				Manager.managerInstance = new GameObject("Runtime AudioClip Loader Manger singleton instance");
				Manager.managerInstance.hideFlags = HideFlags.HideAndDontSave;
				Manager.managerInstance.AddComponent<Manager>();
			}
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00004530 File Offset: 0x00002730
		public static void SetAudioClipLoadState(AudioClip audioClip, AudioDataLoadState newLoadState)
		{
			Manager.audioLoadState[audioClip] = newLoadState;
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00004540 File Offset: 0x00002740
		public static AudioDataLoadState GetAudioClipLoadState(AudioClip audioClip)
		{
			AudioDataLoadState result = AudioDataLoadState.Failed;
			if (audioClip != null)
			{
				result = audioClip.loadState;
				Manager.audioLoadState.TryGetValue(audioClip, out result);
			}
			return result;
		}

		// Token: 0x0600005B RID: 91 RVA: 0x0000456E File Offset: 0x0000276E
		public static void SetAudioClipLoadType(AudioClip audioClip, AudioClipLoadType newLoadType)
		{
			Manager.audioClipLoadType[audioClip] = newLoadType;
		}

		// Token: 0x0600005C RID: 92 RVA: 0x0000457C File Offset: 0x0000277C
		public static AudioClipLoadType GetAudioClipLoadType(AudioClip audioClip)
		{
			AudioClipLoadType result = (AudioClipLoadType)(-1);
			if (audioClip != null)
			{
				result = audioClip.loadType;
				Manager.audioClipLoadType.TryGetValue(audioClip, out result);
			}
			return result;
		}

		// Token: 0x0600005D RID: 93 RVA: 0x000045AA File Offset: 0x000027AA
		private static string GetExtension(string filePath)
		{
			return Path.GetExtension(filePath).Substring(1).ToLower();
		}

		// Token: 0x0600005E RID: 94 RVA: 0x000045BD File Offset: 0x000027BD
		public static bool IsSupportedFormat(string filePath)
		{
			return Manager.supportedFormats.Contains(Manager.GetExtension(filePath));
		}

		// Token: 0x0600005F RID: 95 RVA: 0x000045D0 File Offset: 0x000027D0
		public static AudioFormat GetAudioFormat(string filePath)
		{
			AudioFormat result = AudioFormat.unknown;
			try
			{
				result = (AudioFormat)Enum.Parse(typeof(AudioFormat), Manager.GetExtension(filePath), true);
			}
			catch
			{
			}
			return result;
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00004614 File Offset: 0x00002814
		public static void ClearCache()
		{
			Manager.cache.Clear();
		}

		// Token: 0x0400002A RID: 42
		private static readonly string[] supportedFormats;

		// Token: 0x0400002B RID: 43
		private static Dictionary<string, AudioClip> cache = new Dictionary<string, AudioClip>();

		// Token: 0x0400002C RID: 44
		private static Queue<Manager.AudioInstance> deferredLoadQueue = new Queue<Manager.AudioInstance>();

		// Token: 0x0400002D RID: 45
		private static Queue<Manager.AudioInstance> deferredSetDataQueue = new Queue<Manager.AudioInstance>();

		// Token: 0x0400002E RID: 46
		private static Queue<Manager.AudioInstance> deferredSetFail = new Queue<Manager.AudioInstance>();

		// Token: 0x0400002F RID: 47
		private static Thread deferredLoaderThread;

		// Token: 0x04000030 RID: 48
		private static GameObject managerInstance;

		// Token: 0x04000031 RID: 49
		private static Dictionary<AudioClip, AudioClipLoadType> audioClipLoadType = new Dictionary<AudioClip, AudioClipLoadType>();

		// Token: 0x04000032 RID: 50
		private static Dictionary<AudioClip, AudioDataLoadState> audioLoadState = new Dictionary<AudioClip, AudioDataLoadState>();

		// Token: 0x0200184F RID: 6223
		private class AudioInstance
		{
			// Token: 0x1700182E RID: 6190
			// (get) Token: 0x0600927A RID: 37498 RVA: 0x00349DC7 File Offset: 0x00347FC7
			public int channels
			{
				get
				{
					return this.reader.WaveFormat.Channels;
				}
			}

			// Token: 0x1700182F RID: 6191
			// (get) Token: 0x0600927B RID: 37499 RVA: 0x00349DD9 File Offset: 0x00347FD9
			public int sampleRate
			{
				get
				{
					return this.reader.WaveFormat.SampleRate;
				}
			}

			// Token: 0x0600927C RID: 37500 RVA: 0x00349DEB File Offset: 0x00347FEB
			public static implicit operator AudioClip(Manager.AudioInstance ai)
			{
				return ai.audioClip;
			}

			// Token: 0x04005CAB RID: 23723
			public AudioClip audioClip;

			// Token: 0x04005CAC RID: 23724
			public CustomAudioFileReader reader;

			// Token: 0x04005CAD RID: 23725
			public float[] dataToSet;

			// Token: 0x04005CAE RID: 23726
			public int samplesCount;

			// Token: 0x04005CAF RID: 23727
			public Stream streamToDisposeOnceDone;
		}
	}
}
