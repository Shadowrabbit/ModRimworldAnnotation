using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000073 RID: 115
	public static class Log
	{
		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x0600046A RID: 1130 RVA: 0x00009ECD File Offset: 0x000080CD
		public static IEnumerable<LogMessage> Messages
		{
			get
			{
				return Log.messageQueue.Messages;
			}
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x0600046B RID: 1131 RVA: 0x00009ED9 File Offset: 0x000080D9
		private static bool ReachedMaxMessagesLimit
		{
			get
			{
				return Log.reachedMaxMessagesLimit;
			}
		}

		// Token: 0x0600046C RID: 1132 RVA: 0x00009EE0 File Offset: 0x000080E0
		public static void ResetMessageCount()
		{
			Log.messageCount = 0;
			if (Log.reachedMaxMessagesLimit)
			{
				Debug.unityLogger.logEnabled = true;
				Log.reachedMaxMessagesLimit = false;
				Log.Message("Message logging is now once again on.", false);
			}
		}

		// Token: 0x0600046D RID: 1133 RVA: 0x00009F0B File Offset: 0x0000810B
		public static void Message(string text, bool ignoreStopLoggingLimit = false)
		{
			if (Log.ReachedMaxMessagesLimit)
			{
				return;
			}
			Debug.Log(text);
			Log.messageQueue.Enqueue(new LogMessage(LogMessageType.Message, text, StackTraceUtility.ExtractStackTrace()));
			Log.PostMessage();
		}

		// Token: 0x0600046E RID: 1134 RVA: 0x00009F36 File Offset: 0x00008136
		public static void Warning(string text, bool ignoreStopLoggingLimit = false)
		{
			if (Log.ReachedMaxMessagesLimit)
			{
				return;
			}
			Debug.LogWarning(text);
			Log.messageQueue.Enqueue(new LogMessage(LogMessageType.Warning, text, StackTraceUtility.ExtractStackTrace()));
			Log.PostMessage();
		}

		// Token: 0x0600046F RID: 1135 RVA: 0x00087AE0 File Offset: 0x00085CE0
		public static void Error(string text, bool ignoreStopLoggingLimit = false)
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

		// Token: 0x06000470 RID: 1136 RVA: 0x00009F61 File Offset: 0x00008161
		public static void ErrorOnce(string text, int key, bool ignoreStopLoggingLimit = false)
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
			Log.Error(text, ignoreStopLoggingLimit);
		}

		// Token: 0x06000471 RID: 1137 RVA: 0x00009F8C File Offset: 0x0000818C
		public static void Clear()
		{
			EditWindow_Log.ClearSelectedMessage();
			Log.messageQueue.Clear();
			Log.ResetMessageCount();
		}

		// Token: 0x06000472 RID: 1138 RVA: 0x00009FA2 File Offset: 0x000081A2
		public static void TryOpenLogWindow()
		{
			if (StaticConstructorOnStartupUtility.coreStaticAssetsLoaded || UnityData.IsInMainThread)
			{
				EditWindow_Log.TryAutoOpen();
			}
		}

		// Token: 0x06000473 RID: 1139 RVA: 0x00009FB7 File Offset: 0x000081B7
		private static void PostMessage()
		{
			if (Log.openOnMessage)
			{
				Log.TryOpenLogWindow();
				EditWindow_Log.SelectLastMessage(true);
			}
		}

		// Token: 0x06000474 RID: 1140 RVA: 0x00009FCB File Offset: 0x000081CB
		public static void Notify_MessageReceivedThreadedInternal(string msg, string stackTrace, LogType type)
		{
			if (++Log.messageCount == 1000)
			{
				Log.Warning("Reached max messages limit. Stopping logging to avoid spam.", false);
				Log.reachedMaxMessagesLimit = true;
				Debug.unityLogger.logEnabled = false;
			}
		}

		// Token: 0x040001F8 RID: 504
		private static LogMessageQueue messageQueue = new LogMessageQueue();

		// Token: 0x040001F9 RID: 505
		private static HashSet<int> usedKeys = new HashSet<int>();

		// Token: 0x040001FA RID: 506
		public static bool openOnMessage = false;

		// Token: 0x040001FB RID: 507
		private static bool currentlyLoggingError;

		// Token: 0x040001FC RID: 508
		private static int messageCount;

		// Token: 0x040001FD RID: 509
		private static bool reachedMaxMessagesLimit;

		// Token: 0x040001FE RID: 510
		private const int StopLoggingAtMessageCount = 1000;
	}
}
