using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using RimWorld;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Verse
{
	// Token: 0x0200003E RID: 62
	public static class LongEventHandler
	{
		// Token: 0x17000080 RID: 128
		// (get) Token: 0x0600034E RID: 846 RVA: 0x00011AEF File Offset: 0x0000FCEF
		public static bool ShouldWaitForEvent
		{
			get
			{
				return LongEventHandler.AnyEventNowOrWaiting && ((LongEventHandler.currentEvent != null && !LongEventHandler.currentEvent.UseStandardWindow) || (Find.UIRoot == null || Find.WindowStack == null));
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x0600034F RID: 847 RVA: 0x00011B20 File Offset: 0x0000FD20
		public static bool AnyEventNowOrWaiting
		{
			get
			{
				return LongEventHandler.currentEvent != null || LongEventHandler.eventQueue.Count > 0;
			}
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x06000350 RID: 848 RVA: 0x00011B38 File Offset: 0x0000FD38
		public static bool AnyEventWhichDoesntUseStandardWindowNowOrWaiting
		{
			get
			{
				LongEventHandler.QueuedLongEvent queuedLongEvent = LongEventHandler.currentEvent;
				if (queuedLongEvent != null && !queuedLongEvent.UseStandardWindow)
				{
					return true;
				}
				return LongEventHandler.eventQueue.Any((LongEventHandler.QueuedLongEvent x) => !x.UseStandardWindow);
			}
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x06000351 RID: 849 RVA: 0x00011B81 File Offset: 0x0000FD81
		public static bool ForcePause
		{
			get
			{
				return LongEventHandler.AnyEventNowOrWaiting;
			}
		}

		// Token: 0x06000352 RID: 850 RVA: 0x00011B88 File Offset: 0x0000FD88
		public static void QueueLongEvent(Action action, string textKey, bool doAsynchronously, Action<Exception> exceptionHandler, bool showExtraUIInfo = true)
		{
			LongEventHandler.QueuedLongEvent queuedLongEvent = new LongEventHandler.QueuedLongEvent();
			queuedLongEvent.eventAction = action;
			queuedLongEvent.eventTextKey = textKey;
			queuedLongEvent.doAsynchronously = doAsynchronously;
			queuedLongEvent.exceptionHandler = exceptionHandler;
			queuedLongEvent.canEverUseStandardWindow = !LongEventHandler.AnyEventWhichDoesntUseStandardWindowNowOrWaiting;
			queuedLongEvent.showExtraUIInfo = showExtraUIInfo;
			LongEventHandler.eventQueue.Enqueue(queuedLongEvent);
		}

		// Token: 0x06000353 RID: 851 RVA: 0x00011BD8 File Offset: 0x0000FDD8
		public static void QueueLongEvent(IEnumerable action, string textKey, Action<Exception> exceptionHandler = null, bool showExtraUIInfo = true)
		{
			LongEventHandler.QueuedLongEvent queuedLongEvent = new LongEventHandler.QueuedLongEvent();
			queuedLongEvent.eventActionEnumerator = action.GetEnumerator();
			queuedLongEvent.eventTextKey = textKey;
			queuedLongEvent.doAsynchronously = false;
			queuedLongEvent.exceptionHandler = exceptionHandler;
			queuedLongEvent.canEverUseStandardWindow = !LongEventHandler.AnyEventWhichDoesntUseStandardWindowNowOrWaiting;
			queuedLongEvent.showExtraUIInfo = showExtraUIInfo;
			LongEventHandler.eventQueue.Enqueue(queuedLongEvent);
		}

		// Token: 0x06000354 RID: 852 RVA: 0x00011C2C File Offset: 0x0000FE2C
		public static void QueueLongEvent(Action preLoadLevelAction, string levelToLoad, string textKey, bool doAsynchronously, Action<Exception> exceptionHandler, bool showExtraUIInfo = true)
		{
			LongEventHandler.QueuedLongEvent queuedLongEvent = new LongEventHandler.QueuedLongEvent();
			queuedLongEvent.eventAction = preLoadLevelAction;
			queuedLongEvent.levelToLoad = levelToLoad;
			queuedLongEvent.eventTextKey = textKey;
			queuedLongEvent.doAsynchronously = doAsynchronously;
			queuedLongEvent.exceptionHandler = exceptionHandler;
			queuedLongEvent.canEverUseStandardWindow = !LongEventHandler.AnyEventWhichDoesntUseStandardWindowNowOrWaiting;
			queuedLongEvent.showExtraUIInfo = showExtraUIInfo;
			LongEventHandler.eventQueue.Enqueue(queuedLongEvent);
		}

		// Token: 0x06000355 RID: 853 RVA: 0x00011C84 File Offset: 0x0000FE84
		public static void ClearQueuedEvents()
		{
			LongEventHandler.eventQueue.Clear();
		}

		// Token: 0x06000356 RID: 854 RVA: 0x00011C90 File Offset: 0x0000FE90
		public static void LongEventsOnGUI()
		{
			if (LongEventHandler.currentEvent == null)
			{
				GameplayTipWindow.ResetTipTimer();
				return;
			}
			float num = LongEventHandler.StatusRectSize.x;
			object currentEventTextLock = LongEventHandler.CurrentEventTextLock;
			lock (currentEventTextLock)
			{
				Text.Font = GameFont.Small;
				num = Mathf.Max(num, Text.CalcSize(LongEventHandler.currentEvent.eventText + "...").x + 40f);
			}
			bool flag2 = Find.UIRoot != null && !LongEventHandler.currentEvent.UseStandardWindow && LongEventHandler.currentEvent.showExtraUIInfo;
			bool flag3 = Find.UIRoot != null && Current.Game != null && !LongEventHandler.currentEvent.UseStandardWindow && LongEventHandler.currentEvent.showExtraUIInfo;
			Vector2 vector = flag3 ? ModSummaryWindow.GetEffectiveSize() : Vector2.zero;
			float num2 = LongEventHandler.StatusRectSize.y;
			if (flag3)
			{
				num2 += 17f + vector.y;
			}
			if (flag2)
			{
				num2 += 17f + GameplayTipWindow.WindowSize.y;
			}
			float num3 = ((float)UI.screenHeight - num2) / 2f;
			Vector2 vector2 = new Vector2(((float)UI.screenWidth - GameplayTipWindow.WindowSize.x) / 2f, num3 + LongEventHandler.StatusRectSize.y + 17f);
			Vector2 offset = new Vector2(((float)UI.screenWidth - vector.x) / 2f, vector2.y + GameplayTipWindow.WindowSize.y + 17f);
			Rect rect = new Rect(((float)UI.screenWidth - num) / 2f, num3, num, LongEventHandler.StatusRectSize.y);
			rect = rect.Rounded();
			if (!LongEventHandler.currentEvent.UseStandardWindow || Find.UIRoot == null || Find.WindowStack == null)
			{
				if (UIMenuBackgroundManager.background == null)
				{
					UIMenuBackgroundManager.background = new UI_BackgroundMain();
				}
				UIMenuBackgroundManager.background.BackgroundOnGUI();
				Widgets.DrawShadowAround(rect);
				Widgets.DrawWindowBackground(rect);
				LongEventHandler.DrawLongEventWindowContents(rect);
				if (flag2)
				{
					GameplayTipWindow.DrawWindow(vector2, false);
				}
				if (flag3)
				{
					ModSummaryWindow.DrawWindow(offset, false);
					TooltipHandler.DoTooltipGUI();
					return;
				}
			}
			else
			{
				LongEventHandler.DrawLongEventWindow(rect);
				if (flag2)
				{
					GameplayTipWindow.DrawWindow(vector2, true);
				}
			}
		}

		// Token: 0x06000357 RID: 855 RVA: 0x00011EC0 File Offset: 0x000100C0
		private static void DrawLongEventWindow(Rect statusRect)
		{
			Find.WindowStack.ImmediateWindow(62893994, statusRect, WindowLayer.Super, delegate
			{
				LongEventHandler.DrawLongEventWindowContents(statusRect.AtZero());
			}, true, false, 1f, null);
		}

		// Token: 0x06000358 RID: 856 RVA: 0x00011F04 File Offset: 0x00010104
		public static void LongEventsUpdate(out bool sceneChanged)
		{
			sceneChanged = false;
			if (LongEventHandler.currentEvent != null)
			{
				if (LongEventHandler.currentEvent.eventActionEnumerator != null)
				{
					LongEventHandler.UpdateCurrentEnumeratorEvent();
				}
				else if (LongEventHandler.currentEvent.doAsynchronously)
				{
					LongEventHandler.UpdateCurrentAsynchronousEvent();
				}
				else
				{
					LongEventHandler.UpdateCurrentSynchronousEvent(out sceneChanged);
				}
			}
			if (LongEventHandler.currentEvent == null && LongEventHandler.eventQueue.Count > 0)
			{
				LongEventHandler.currentEvent = LongEventHandler.eventQueue.Dequeue();
				if (LongEventHandler.currentEvent.eventTextKey == null)
				{
					LongEventHandler.currentEvent.eventText = "";
					return;
				}
				LongEventHandler.currentEvent.eventText = LongEventHandler.currentEvent.eventTextKey.Translate();
			}
		}

		// Token: 0x06000359 RID: 857 RVA: 0x00011FA4 File Offset: 0x000101A4
		public static void ExecuteWhenFinished(Action action)
		{
			LongEventHandler.toExecuteWhenFinished.Add(action);
			if ((LongEventHandler.currentEvent == null || LongEventHandler.currentEvent.ShouldWaitUntilDisplayed) && !LongEventHandler.executingToExecuteWhenFinished)
			{
				LongEventHandler.ExecuteToExecuteWhenFinished();
			}
		}

		// Token: 0x0600035A RID: 858 RVA: 0x00011FD0 File Offset: 0x000101D0
		public static void SetCurrentEventText(string newText)
		{
			object currentEventTextLock = LongEventHandler.CurrentEventTextLock;
			lock (currentEventTextLock)
			{
				if (LongEventHandler.currentEvent != null)
				{
					LongEventHandler.currentEvent.eventText = newText;
				}
			}
		}

		// Token: 0x0600035B RID: 859 RVA: 0x00012020 File Offset: 0x00010220
		private static void UpdateCurrentEnumeratorEvent()
		{
			try
			{
				float num = Time.realtimeSinceStartup + 0.1f;
				while (LongEventHandler.currentEvent.eventActionEnumerator.MoveNext())
				{
					if (num <= Time.realtimeSinceStartup)
					{
						return;
					}
				}
				IDisposable disposable = LongEventHandler.currentEvent.eventActionEnumerator as IDisposable;
				if (disposable != null)
				{
					disposable.Dispose();
				}
				LongEventHandler.currentEvent = null;
				LongEventHandler.eventThread = null;
				LongEventHandler.levelLoadOp = null;
				LongEventHandler.ExecuteToExecuteWhenFinished();
			}
			catch (Exception ex)
			{
				Log.Error("Exception from long event: " + ex);
				if (LongEventHandler.currentEvent != null)
				{
					IDisposable disposable2 = LongEventHandler.currentEvent.eventActionEnumerator as IDisposable;
					if (disposable2 != null)
					{
						disposable2.Dispose();
					}
					if (LongEventHandler.currentEvent.exceptionHandler != null)
					{
						LongEventHandler.currentEvent.exceptionHandler(ex);
					}
				}
				LongEventHandler.currentEvent = null;
				LongEventHandler.eventThread = null;
				LongEventHandler.levelLoadOp = null;
			}
		}

		// Token: 0x0600035C RID: 860 RVA: 0x000120F8 File Offset: 0x000102F8
		private static void UpdateCurrentAsynchronousEvent()
		{
			if (LongEventHandler.eventThread == null)
			{
				LongEventHandler.eventThread = new Thread(delegate()
				{
					LongEventHandler.RunEventFromAnotherThread(LongEventHandler.currentEvent.eventAction);
				});
				LongEventHandler.eventThread.Start();
				return;
			}
			if (!LongEventHandler.eventThread.IsAlive)
			{
				bool flag = false;
				if (!LongEventHandler.currentEvent.levelToLoad.NullOrEmpty())
				{
					if (LongEventHandler.levelLoadOp == null)
					{
						LongEventHandler.levelLoadOp = SceneManager.LoadSceneAsync(LongEventHandler.currentEvent.levelToLoad);
					}
					else if (LongEventHandler.levelLoadOp.isDone)
					{
						flag = true;
					}
				}
				else
				{
					flag = true;
				}
				if (flag)
				{
					LongEventHandler.currentEvent = null;
					LongEventHandler.eventThread = null;
					LongEventHandler.levelLoadOp = null;
					LongEventHandler.ExecuteToExecuteWhenFinished();
				}
			}
		}

		// Token: 0x0600035D RID: 861 RVA: 0x000121A8 File Offset: 0x000103A8
		private static void UpdateCurrentSynchronousEvent(out bool sceneChanged)
		{
			sceneChanged = false;
			if (LongEventHandler.currentEvent.ShouldWaitUntilDisplayed)
			{
				return;
			}
			try
			{
				if (LongEventHandler.currentEvent.eventAction != null)
				{
					LongEventHandler.currentEvent.eventAction();
				}
				if (!LongEventHandler.currentEvent.levelToLoad.NullOrEmpty())
				{
					SceneManager.LoadScene(LongEventHandler.currentEvent.levelToLoad);
					sceneChanged = true;
				}
				LongEventHandler.currentEvent = null;
				LongEventHandler.eventThread = null;
				LongEventHandler.levelLoadOp = null;
				LongEventHandler.ExecuteToExecuteWhenFinished();
			}
			catch (Exception ex)
			{
				Log.Error("Exception from long event: " + ex);
				if (LongEventHandler.currentEvent != null && LongEventHandler.currentEvent.exceptionHandler != null)
				{
					LongEventHandler.currentEvent.exceptionHandler(ex);
				}
				LongEventHandler.currentEvent = null;
				LongEventHandler.eventThread = null;
				LongEventHandler.levelLoadOp = null;
			}
		}

		// Token: 0x0600035E RID: 862 RVA: 0x00012278 File Offset: 0x00010478
		private static void RunEventFromAnotherThread(Action action)
		{
			CultureInfoUtility.EnsureEnglish();
			try
			{
				if (action != null)
				{
					action();
				}
			}
			catch (Exception ex)
			{
				Log.Error("Exception from asynchronous event: " + ex);
				try
				{
					if (LongEventHandler.currentEvent != null && LongEventHandler.currentEvent.exceptionHandler != null)
					{
						LongEventHandler.currentEvent.exceptionHandler(ex);
					}
				}
				catch (Exception arg)
				{
					Log.Error("Exception was thrown while trying to handle exception. Exception: " + arg);
				}
			}
		}

		// Token: 0x0600035F RID: 863 RVA: 0x000122FC File Offset: 0x000104FC
		private static void ExecuteToExecuteWhenFinished()
		{
			if (LongEventHandler.executingToExecuteWhenFinished)
			{
				Log.Warning("Already executing.");
				return;
			}
			LongEventHandler.executingToExecuteWhenFinished = true;
			if (LongEventHandler.toExecuteWhenFinished.Count > 0)
			{
				DeepProfiler.Start("ExecuteToExecuteWhenFinished()");
			}
			for (int i = 0; i < LongEventHandler.toExecuteWhenFinished.Count; i++)
			{
				DeepProfiler.Start(LongEventHandler.toExecuteWhenFinished[i].Method.DeclaringType.ToString() + " -> " + LongEventHandler.toExecuteWhenFinished[i].Method.ToString());
				try
				{
					LongEventHandler.toExecuteWhenFinished[i]();
				}
				catch (Exception arg)
				{
					Log.Error("Could not execute post-long-event action. Exception: " + arg);
				}
				finally
				{
					DeepProfiler.End();
				}
			}
			if (LongEventHandler.toExecuteWhenFinished.Count > 0)
			{
				DeepProfiler.End();
			}
			LongEventHandler.toExecuteWhenFinished.Clear();
			LongEventHandler.executingToExecuteWhenFinished = false;
		}

		// Token: 0x06000360 RID: 864 RVA: 0x000123F4 File Offset: 0x000105F4
		private static void DrawLongEventWindowContents(Rect rect)
		{
			if (LongEventHandler.currentEvent == null)
			{
				return;
			}
			if (Event.current.type == EventType.Repaint)
			{
				LongEventHandler.currentEvent.alreadyDisplayed = true;
			}
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.MiddleCenter;
			float num = 0f;
			if (LongEventHandler.levelLoadOp != null)
			{
				float f = 1f;
				if (!LongEventHandler.levelLoadOp.isDone)
				{
					f = LongEventHandler.levelLoadOp.progress;
				}
				TaggedString taggedString = "LoadingAssets".Translate() + " " + f.ToStringPercent();
				num = Text.CalcSize(taggedString).x;
				Widgets.Label(rect, taggedString);
			}
			else
			{
				object currentEventTextLock = LongEventHandler.CurrentEventTextLock;
				lock (currentEventTextLock)
				{
					num = Text.CalcSize(LongEventHandler.currentEvent.eventText).x;
					Widgets.Label(rect, LongEventHandler.currentEvent.eventText);
				}
			}
			Text.Anchor = TextAnchor.MiddleLeft;
			rect.xMin = rect.center.x + num / 2f;
			Widgets.Label(rect, (!LongEventHandler.currentEvent.UseAnimatedDots) ? "..." : GenText.MarchingEllipsis(0f));
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x040000C5 RID: 197
		private static Queue<LongEventHandler.QueuedLongEvent> eventQueue = new Queue<LongEventHandler.QueuedLongEvent>();

		// Token: 0x040000C6 RID: 198
		private static LongEventHandler.QueuedLongEvent currentEvent = null;

		// Token: 0x040000C7 RID: 199
		private static Thread eventThread = null;

		// Token: 0x040000C8 RID: 200
		private static AsyncOperation levelLoadOp = null;

		// Token: 0x040000C9 RID: 201
		private static List<Action> toExecuteWhenFinished = new List<Action>();

		// Token: 0x040000CA RID: 202
		private static bool executingToExecuteWhenFinished = false;

		// Token: 0x040000CB RID: 203
		private static readonly object CurrentEventTextLock = new object();

		// Token: 0x040000CC RID: 204
		private static readonly Vector2 StatusRectSize = new Vector2(240f, 75f);

		// Token: 0x02001896 RID: 6294
		private class QueuedLongEvent
		{
			// Token: 0x17001879 RID: 6265
			// (get) Token: 0x060093FB RID: 37883 RVA: 0x0034E147 File Offset: 0x0034C347
			public bool UseAnimatedDots
			{
				get
				{
					return this.doAsynchronously || this.eventActionEnumerator != null;
				}
			}

			// Token: 0x1700187A RID: 6266
			// (get) Token: 0x060093FC RID: 37884 RVA: 0x0034E15C File Offset: 0x0034C35C
			public bool ShouldWaitUntilDisplayed
			{
				get
				{
					return !this.alreadyDisplayed && this.UseStandardWindow && !this.eventText.NullOrEmpty();
				}
			}

			// Token: 0x1700187B RID: 6267
			// (get) Token: 0x060093FD RID: 37885 RVA: 0x0034E17E File Offset: 0x0034C37E
			public bool UseStandardWindow
			{
				get
				{
					return this.canEverUseStandardWindow && !this.doAsynchronously && this.eventActionEnumerator == null;
				}
			}

			// Token: 0x04005E13 RID: 24083
			public Action eventAction;

			// Token: 0x04005E14 RID: 24084
			public IEnumerator eventActionEnumerator;

			// Token: 0x04005E15 RID: 24085
			public string levelToLoad;

			// Token: 0x04005E16 RID: 24086
			public string eventTextKey = "";

			// Token: 0x04005E17 RID: 24087
			public string eventText = "";

			// Token: 0x04005E18 RID: 24088
			public bool doAsynchronously;

			// Token: 0x04005E19 RID: 24089
			public Action<Exception> exceptionHandler;

			// Token: 0x04005E1A RID: 24090
			public bool alreadyDisplayed;

			// Token: 0x04005E1B RID: 24091
			public bool canEverUseStandardWindow = true;

			// Token: 0x04005E1C RID: 24092
			public bool showExtraUIInfo = true;
		}
	}
}
