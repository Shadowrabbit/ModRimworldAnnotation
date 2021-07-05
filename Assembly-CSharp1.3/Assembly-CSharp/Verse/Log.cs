using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200003A RID: 58
	public static class Log
	{
		// Token: 0x1700007B RID: 123
		// (get) Token: 0x06000334 RID: 820 RVA: 0x0001173F File Offset: 0x0000F93F
		public static IEnumerable<LogMessage> Messages
		{
			get
			{
				return Log.messageQueue.Messages;
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x06000335 RID: 821 RVA: 0x0001174B File Offset: 0x0000F94B
		private static bool ReachedMaxMessagesLimit
		{
			get
			{
				return Log.reachedMaxMessagesLimit;
			}
		}

		// Token: 0x06000336 RID: 822 RVA: 0x00011752 File Offset: 0x0000F952
		public static void ResetMessageCount()
		{
			Log.messageCount = 0;
			if (Log.reachedMaxMessagesLimit)
			{
				Debug.unityLogger.logEnabled = true;
				Log.reachedMaxMessagesLimit = false;
				Log.Message("Message logging is now once again on.");
			}
		}

		// Token: 0x06000337 RID: 823 RVA: 0x0001177C File Offset: 0x0000F97C
		[Obsolete]
		public static void Message(string text, bool ignoreStopLoggingLimit)
		{
			Log.Message(text);
		}

		// Token: 0x06000338 RID: 824 RVA: 0x00011784 File Offset: 0x0000F984
		public static void Message(string text)
		{
			if (Log.ReachedMaxMessagesLimit)
			{
				return;
			}
			Debug.Log(text);
			Log.messageQueue.Enqueue(new LogMessage(LogMessageType.Message, text, StackTraceUtility.ExtractStackTrace()));
			Log.PostMessage();
		}

		// Token: 0x06000339 RID: 825 RVA: 0x000117AF File Offset: 0x0000F9AF
		[Obsolete]
		public static void Warning(string text, bool ignoreStopLoggingLimit)
		{
			Log.Warning(text);
		}

		// Token: 0x0600033A RID: 826 RVA: 0x000117B7 File Offset: 0x0000F9B7
		public static void Warning(string text)
		{
			if (Log.ReachedMaxMessagesLimit)
			{
				return;
			}
			Debug.LogWarning(text);
			Log.messageQueue.Enqueue(new LogMessage(LogMessageType.Warning, text, StackTraceUtility.ExtractStackTrace()));
			Log.PostMessage();
		}

		// Token: 0x0600033B RID: 827 RVA: 0x000117E2 File Offset: 0x0000F9E2
		[Obsolete]
		public static void Error(string text, bool ignoreStopLoggingLimit)
		{
			Log.Error(text);
		}

		// Token: 0x0600033C RID: 828 RVA: 0x000117EC File Offset: 0x0000F9EC
		public static void Error(string text)
		{
			if (Log.ReachedMaxMessagesLimit)
			{
				return;
			}
			Debug.LogError(text);
			if (!Log.currentlyLoggingError)
			{
				Log.currentlyLoggingError = true;
				try
				{
					if (Prefs.PauseOnError && Current.ProgramState == ProgramState.Playing)
					{
						Find.TickManager.Pause();
					}
					Log.messageQueue.Enqueue(new LogMessage(LogMessageType.Error, text, StackTraceUtility.ExtractStackTrace()));
					Log.PostMessage();
					if (!PlayDataLoader.Loaded || Prefs.DevMode)
					{
						Log.TryOpenLogWindow();
					}
				}
				catch (Exception arg)
				{
					Debug.LogError("An error occurred while logging an error: " + arg);
				}
				finally
				{
					Log.currentlyLoggingError = false;
				}
			}
		}

		// Token: 0x0600033D RID: 829 RVA: 0x00011894 File Offset: 0x0000FA94
		[Obsolete]
		public static void ErrorOnce(string text, int key, bool ignoreStopLoggingLimit)
		{
			Log.ErrorOnce(text, key);
		}

		// Token: 0x0600033E RID: 830 RVA: 0x0001189D File Offset: 0x0000FA9D
		public static void ErrorOnce(string text, int key)
		{
			if (Log.ReachedMaxMessagesLimit)
			{
				return;
			}
			if (Log.usedKeys.Contains(key))
			{
				return;
			}
			Log.usedKeys.Add(key);
			Log.Error(text);
		}

		// Token: 0x0600033F RID: 831 RVA: 0x000118C7 File Offset: 0x0000FAC7
		public static void Clear()
		{
			EditWindow_Log.ClearSelectedMessage();
			Log.messageQueue.Clear();
			Log.ResetMessageCount();
		}

		// Token: 0x06000340 RID: 832 RVA: 0x000118DD File Offset: 0x0000FADD
		public static void TryOpenLogWindow()
		{
			if (StaticConstructorOnStartupUtility.coreStaticAssetsLoaded || UnityData.IsInMainThread)
			{
				EditWindow_Log.TryAutoOpen();
			}
		}

		// Token: 0x06000341 RID: 833 RVA: 0x000118F2 File Offset: 0x0000FAF2
		private static void PostMessage()
		{
			if (Log.openOnMessage)
			{
				Log.TryOpenLogWindow();
				EditWindow_Log.SelectLastMessage(true);
			}
		}

		// Token: 0x06000342 RID: 834 RVA: 0x00011906 File Offset: 0x0000FB06
		public static void Notify_MessageReceivedThreadedInternal(string msg, string stackTrace, LogType type)
		{
			if (++Log.messageCount == 1000)
			{
				Log.Warning("Reached max messages limit. Stopping logging to avoid spam.");
				Log.reachedMaxMessagesLimit = true;
				Debug.unityLogger.logEnabled = false;
			}
		}

		// Token: 0x040000B3 RID: 179
		private static LogMessageQueue messageQueue = new LogMessageQueue();

		// Token: 0x040000B4 RID: 180
		private static HashSet<int> usedKeys = new HashSet<int>();

		// Token: 0x040000B5 RID: 181
		public static bool openOnMessage = false;

		// Token: 0x040000B6 RID: 182
		private static bool currentlyLoggingError;

		// Token: 0x040000B7 RID: 183
		private static int messageCount;

		// Token: 0x040000B8 RID: 184
		private static bool reachedMaxMessagesLimit;

		// Token: 0x040000B9 RID: 185
		private const int StopLoggingAtMessageCount = 1000;
	}
}
