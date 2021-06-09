using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000076 RID: 118
	public class LogMessageQueue
	{
		// Token: 0x170000CA RID: 202
		// (get) Token: 0x0600047C RID: 1148 RVA: 0x0000A0CE File Offset: 0x000082CE
		public IEnumerable<LogMessage> Messages
		{
			get
			{
				return this.messages;
			}
		}

		// Token: 0x0600047D RID: 1149 RVA: 0x00087BC8 File Offset: 0x00085DC8
		public void Enqueue(LogMessage msg)
		{
			if (this.lastMessage != null && msg.CanCombineWith(this.lastMessage))
			{
				this.lastMessage.repeats++;
				return;
			}
			this.lastMessage = msg;
			this.messages.Enqueue(msg);
			if (this.messages.Count > this.maxMessages)
			{
				EditWindow_Log.Notify_MessageDequeued(this.messages.Dequeue());
			}
		}

		// Token: 0x0600047E RID: 1150 RVA: 0x0000A0D6 File Offset: 0x000082D6
		internal void Clear()
		{
			this.messages.Clear();
			this.lastMessage = null;
		}

		// Token: 0x04000207 RID: 519
		public int maxMessages = 200;

		// Token: 0x04000208 RID: 520
		private Queue<LogMessage> messages = new Queue<LogMessage>();

		// Token: 0x04000209 RID: 521
		private LogMessage lastMessage;
	}
}
