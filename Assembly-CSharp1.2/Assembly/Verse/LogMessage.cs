using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000075 RID: 117
	public class LogMessage
	{
		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x06000476 RID: 1142 RVA: 0x00087B88 File Offset: 0x00085D88
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
					return Color.red;
				default:
					return Color.white;
				}
			}
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x06000477 RID: 1143 RVA: 0x0000A019 File Offset: 0x00008219
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

		// Token: 0x06000478 RID: 1144 RVA: 0x0000A02F File Offset: 0x0000822F
		public LogMessage(string text)
		{
			this.text = text;
			this.type = LogMessageType.Message;
			this.stackTrace = null;
		}

		// Token: 0x06000479 RID: 1145 RVA: 0x0000A053 File Offset: 0x00008253
		public LogMessage(LogMessageType type, string text, string stackTrace)
		{
			this.text = text;
			this.type = type;
			this.stackTrace = stackTrace;
		}

		// Token: 0x0600047A RID: 1146 RVA: 0x0000A077 File Offset: 0x00008277
		public override string ToString()
		{
			if (this.repeats > 1)
			{
				return "(" + this.repeats.ToString() + ") " + this.text;
			}
			return this.text;
		}

		// Token: 0x0600047B RID: 1147 RVA: 0x0000A0A9 File Offset: 0x000082A9
		public bool CanCombineWith(LogMessage other)
		{
			return this.text == other.text && this.type == other.type;
		}

		// Token: 0x04000203 RID: 515
		public string text;

		// Token: 0x04000204 RID: 516
		public LogMessageType type;

		// Token: 0x04000205 RID: 517
		public int repeats = 1;

		// Token: 0x04000206 RID: 518
		private string stackTrace;
	}
}
