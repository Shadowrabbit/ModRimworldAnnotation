using System;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003DF RID: 991
	[StaticConstructorOnStartup]
	public class EditWindow_Log : EditWindow
	{
		// Token: 0x170005A7 RID: 1447
		// (get) Token: 0x06001DEA RID: 7658 RVA: 0x000BAFDB File Offset: 0x000B91DB
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2((float)UI.screenWidth / 2f, (float)UI.screenHeight / 2f);
			}
		}

		// Token: 0x170005A8 RID: 1448
		// (get) Token: 0x06001DEB RID: 7659 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool IsDebug
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170005A9 RID: 1449
		// (get) Token: 0x06001DEC RID: 7660 RVA: 0x000BAFFA File Offset: 0x000B91FA
		// (set) Token: 0x06001DED RID: 7661 RVA: 0x000BB001 File Offset: 0x000B9201
		private static LogMessage SelectedMessage
		{
			get
			{
				return EditWindow_Log.selectedMessage;
			}
			set
			{
				if (EditWindow_Log.selectedMessage == value)
				{
					return;
				}
				EditWindow_Log.selectedMessage = value;
				if (UnityData.IsInMainThread && GUI.GetNameOfFocusedControl() == EditWindow_Log.MessageDetailsControlName)
				{
					UI.UnfocusCurrentControl();
				}
			}
		}

		// Token: 0x06001DEE RID: 7662 RVA: 0x000BB02F File Offset: 0x000B922F
		public EditWindow_Log()
		{
			this.optionalTitle = "Debug log";
		}

		// Token: 0x06001DEF RID: 7663 RVA: 0x000BB042 File Offset: 0x000B9242
		public static void TryAutoOpen()
		{
			if (EditWindow_Log.canAutoOpen)
			{
				EditWindow_Log.wantsToOpen = true;
			}
		}

		// Token: 0x06001DF0 RID: 7664 RVA: 0x000BB051 File Offset: 0x000B9251
		public static void ClearSelectedMessage()
		{
			EditWindow_Log.SelectedMessage = null;
			EditWindow_Log.detailsScrollPosition = Vector2.zero;
		}

		// Token: 0x06001DF1 RID: 7665 RVA: 0x000BB063 File Offset: 0x000B9263
		public static void SelectLastMessage(bool expandDetailsPane = false)
		{
			EditWindow_Log.ClearSelectedMessage();
			EditWindow_Log.SelectedMessage = Log.Messages.LastOrDefault<LogMessage>();
			EditWindow_Log.messagesScrollPosition.y = (float)Log.Messages.Count<LogMessage>() * 30f;
			if (expandDetailsPane)
			{
				EditWindow_Log.detailsPaneHeight = 9999f;
			}
		}

		// Token: 0x06001DF2 RID: 7666 RVA: 0x000BB0A1 File Offset: 0x000B92A1
		public static void ClearAll()
		{
			EditWindow_Log.ClearSelectedMessage();
			EditWindow_Log.messagesScrollPosition = Vector2.zero;
		}

		// Token: 0x06001DF3 RID: 7667 RVA: 0x000BB0B2 File Offset: 0x000B92B2
		public override void PostClose()
		{
			base.PostClose();
			EditWindow_Log.wantsToOpen = false;
		}

		// Token: 0x06001DF4 RID: 7668 RVA: 0x000BB0C0 File Offset: 0x000B92C0
		public override void DoWindowContents(Rect inRect)
		{
			Text.Font = GameFont.Tiny;
			WidgetRow widgetRow = new WidgetRow(0f, 0f, UIDirection.RightThenUp, 99999f, 4f);
			if (widgetRow.ButtonText("Clear", "Clear all log messages.", true, true, true, null))
			{
				Log.Clear();
				EditWindow_Log.ClearAll();
			}
			if (widgetRow.ButtonText("Trace big", "Set the stack trace to be large on screen.", true, true, true, null))
			{
				EditWindow_Log.detailsPaneHeight = 700f;
			}
			if (widgetRow.ButtonText("Trace medium", "Set the stack trace to be medium-sized on screen.", true, true, true, null))
			{
				EditWindow_Log.detailsPaneHeight = 300f;
			}
			if (widgetRow.ButtonText("Trace small", "Set the stack trace to be small on screen.", true, true, true, null))
			{
				EditWindow_Log.detailsPaneHeight = 100f;
			}
			if (EditWindow_Log.canAutoOpen)
			{
				if (widgetRow.ButtonText("Auto-open is ON", "", true, true, true, null))
				{
					EditWindow_Log.canAutoOpen = false;
				}
			}
			else if (widgetRow.ButtonText("Auto-open is OFF", "", true, true, true, null))
			{
				EditWindow_Log.canAutoOpen = true;
			}
			if (widgetRow.ButtonText("Copy to clipboard", "Copy all messages to the clipboard.", true, true, true, null))
			{
				this.CopyAllMessagesToClipboard();
			}
			Text.Font = GameFont.Small;
			Rect rect = new Rect(inRect);
			rect.yMin += 26f;
			rect.yMax = inRect.height;
			if (EditWindow_Log.selectedMessage != null)
			{
				rect.yMax -= EditWindow_Log.detailsPaneHeight;
			}
			Rect detailsRect = new Rect(inRect);
			detailsRect.yMin = rect.yMax;
			this.DoMessagesListing(rect);
			this.DoMessageDetails(detailsRect, inRect);
			if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && Mouse.IsOver(rect))
			{
				EditWindow_Log.ClearSelectedMessage();
			}
			EditWindow_Log.detailsPaneHeight = Mathf.Max(EditWindow_Log.detailsPaneHeight, 10f);
			EditWindow_Log.detailsPaneHeight = Mathf.Min(EditWindow_Log.detailsPaneHeight, inRect.height - 80f);
		}

		// Token: 0x06001DF5 RID: 7669 RVA: 0x000BB2C6 File Offset: 0x000B94C6
		public static void Notify_MessageDequeued(LogMessage oldMessage)
		{
			if (EditWindow_Log.SelectedMessage == oldMessage)
			{
				EditWindow_Log.SelectedMessage = null;
			}
		}

		// Token: 0x06001DF6 RID: 7670 RVA: 0x000BB2D8 File Offset: 0x000B94D8
		private void DoMessagesListing(Rect listingRect)
		{
			Rect viewRect = new Rect(0f, 0f, listingRect.width - 16f, this.listingViewHeight + 100f);
			Widgets.BeginScrollView(listingRect, ref EditWindow_Log.messagesScrollPosition, viewRect, true);
			float width = viewRect.width - 28f;
			Text.Font = GameFont.Tiny;
			float num = 0f;
			bool flag = false;
			foreach (LogMessage logMessage in Log.Messages)
			{
				string text = logMessage.text;
				if (text.Length > 1000)
				{
					text = text.Substring(0, 1000);
				}
				float num2 = Math.Min(30f, Text.CalcHeight(text, width));
				GUI.color = new Color(1f, 1f, 1f, 0.7f);
				Widgets.Label(new Rect(4f, num, 28f, num2), logMessage.repeats.ToStringCached());
				Rect rect = new Rect(28f, num, width, num2);
				if (EditWindow_Log.selectedMessage == logMessage)
				{
					GUI.DrawTexture(rect, EditWindow_Log.SelectedMessageTex);
				}
				else if (flag)
				{
					GUI.DrawTexture(rect, EditWindow_Log.AltMessageTex);
				}
				if (Widgets.ButtonInvisible(rect, true))
				{
					EditWindow_Log.ClearSelectedMessage();
					EditWindow_Log.SelectedMessage = logMessage;
				}
				GUI.color = logMessage.Color;
				Widgets.Label(rect, text);
				num += num2;
				flag = !flag;
			}
			if (Event.current.type == EventType.Layout)
			{
				this.listingViewHeight = num;
			}
			Widgets.EndScrollView();
			GUI.color = Color.white;
		}

		// Token: 0x06001DF7 RID: 7671 RVA: 0x000BB494 File Offset: 0x000B9694
		private void DoMessageDetails(Rect detailsRect, Rect outRect)
		{
			if (EditWindow_Log.selectedMessage == null)
			{
				return;
			}
			Rect rect = detailsRect;
			rect.height = 7f;
			Rect rect2 = detailsRect;
			rect2.yMin = rect.yMax;
			GUI.DrawTexture(rect, EditWindow_Log.StackTraceBorderTex);
			if (Mouse.IsOver(rect))
			{
				Widgets.DrawHighlight(rect);
			}
			if (Event.current.type == EventType.MouseDown && Mouse.IsOver(rect))
			{
				this.borderDragging = true;
				Event.current.Use();
			}
			if (this.borderDragging)
			{
				EditWindow_Log.detailsPaneHeight = outRect.height + Mathf.Round(3.5f) - Event.current.mousePosition.y;
			}
			if (Event.current.rawType == EventType.MouseUp)
			{
				this.borderDragging = false;
			}
			GUI.DrawTexture(rect2, EditWindow_Log.StackTraceAreaTex);
			string text = EditWindow_Log.selectedMessage.text + "\n" + EditWindow_Log.selectedMessage.StackTrace;
			GUI.SetNextControlName(EditWindow_Log.MessageDetailsControlName);
			if (text.Length > 15000)
			{
				Widgets.LabelScrollable(rect2, text, ref EditWindow_Log.detailsScrollPosition, false, true, true);
				return;
			}
			Widgets.TextAreaScrollable(rect2, text, ref EditWindow_Log.detailsScrollPosition, true);
		}

		// Token: 0x06001DF8 RID: 7672 RVA: 0x000BB5A8 File Offset: 0x000B97A8
		private void CopyAllMessagesToClipboard()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (LogMessage logMessage in Log.Messages)
			{
				if (stringBuilder.Length != 0)
				{
					stringBuilder.AppendLine();
				}
				stringBuilder.AppendLine(logMessage.text);
				stringBuilder.Append(logMessage.StackTrace);
				if (stringBuilder[stringBuilder.Length - 1] != '\n')
				{
					stringBuilder.AppendLine();
				}
			}
			GUIUtility.systemCopyBuffer = stringBuilder.ToString();
		}

		// Token: 0x040011F6 RID: 4598
		private static LogMessage selectedMessage = null;

		// Token: 0x040011F7 RID: 4599
		private static Vector2 messagesScrollPosition;

		// Token: 0x040011F8 RID: 4600
		private static Vector2 detailsScrollPosition;

		// Token: 0x040011F9 RID: 4601
		private static float detailsPaneHeight = 100f;

		// Token: 0x040011FA RID: 4602
		private static bool canAutoOpen = true;

		// Token: 0x040011FB RID: 4603
		public static bool wantsToOpen = false;

		// Token: 0x040011FC RID: 4604
		private float listingViewHeight;

		// Token: 0x040011FD RID: 4605
		private bool borderDragging;

		// Token: 0x040011FE RID: 4606
		private const float CountWidth = 28f;

		// Token: 0x040011FF RID: 4607
		private const float Yinc = 25f;

		// Token: 0x04001200 RID: 4608
		private const float DetailsPaneBorderHeight = 7f;

		// Token: 0x04001201 RID: 4609
		private const float DetailsPaneMinHeight = 10f;

		// Token: 0x04001202 RID: 4610
		private const float ListingMinHeight = 80f;

		// Token: 0x04001203 RID: 4611
		private const float TopAreaHeight = 26f;

		// Token: 0x04001204 RID: 4612
		private const float MessageMaxHeight = 30f;

		// Token: 0x04001205 RID: 4613
		private static readonly Texture2D AltMessageTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.17f, 0.17f, 0.17f, 0.85f));

		// Token: 0x04001206 RID: 4614
		private static readonly Texture2D SelectedMessageTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.25f, 0.25f, 0.17f, 0.85f));

		// Token: 0x04001207 RID: 4615
		private static readonly Texture2D StackTraceAreaTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.1f, 0.1f, 0.1f, 0.5f));

		// Token: 0x04001208 RID: 4616
		private static readonly Texture2D StackTraceBorderTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.3f, 0.3f, 0.3f, 1f));

		// Token: 0x04001209 RID: 4617
		private static readonly string MessageDetailsControlName = "MessageDetailsTextArea";
	}
}
