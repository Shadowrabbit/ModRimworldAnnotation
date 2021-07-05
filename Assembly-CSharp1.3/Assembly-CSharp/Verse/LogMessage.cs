using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200003C RID: 60
	public class LogMessage
	{
		// Token: 0x1700007D RID: 125
		// (get) Token: 0x06000344 RID: 836 RVA: 0x00011954 File Offset: 0x0000FB54
		public Color Color
		{
			get
			{
				switch (this.type)
				{
				case LogMessageType.Message:
					return Color.white;
				case LogMessageType.Warning:
					return Color.yellow;
				case LogMessageType.Error:
					return ColorLibrary.LogError;
				default:
					return Color.white;
				}
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x06000345 RID: 837 RVA: 0x00011993 File Offset: 0x0000FB93
		public string StackTrace
		{
			get
			{
				if (this.stackTrace != null)
				{
					return this.stackTrace;
				}
				return "No stack trace.";
			}
		}

		// Token: 0x06000346 RID: 838 RVA: 0x000119A9 File Offset: 0x0000FBA9
		public LogMessage(string text)
		{
			this.text = text;
			this.type = LogMessageType.Message;
			this.stackTrace = null;
		}

		// Token: 0x06000347 RID: 839 RVA: 0x000119CD File Offset: 0x0000FBCD
		public LogMessage(LogMessageType type, string text, string stackTrace)
		{
			this.text = text;
			this.type = type;
			this.stackTrace = stackTrace;
		}

		// Token: 0x06000348 RID: 840 RVA: 0x000119F1 File Offset: 0x0000FBF1
		public override string ToString()
		{
			if (this.repeats > 1)
			{
				return "(" + this.repeats.ToString() + ") " + this.text;
			}
			return this.text;
		}

		// Token: 0x06000349 RID: 841 RVA: 0x00011A23 File Offset: 0x0000FC23
		public bool CanCombineWith(LogMessage other)
		{
			return this.text == other.text && this.type == other.type;
		}

		// Token: 0x040000BE RID: 190
		public string text;

		// Token: 0x040000BF RID: 191
		public LogMessageType type;

		// Token: 0x040000C0 RID: 192
		public int repeats = 1;

		// Token: 0x040000C1 RID: 193
		private string stackTrace;
	}
}
