using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200003D RID: 61
	public class LogMessageQueue
	{
		// Token: 0x1700007F RID: 127
		// (get) Token: 0x0600034A RID: 842 RVA: 0x00011A48 File Offset: 0x0000FC48
		public IEnumerable<LogMessage> Messages
		{
			get
			{
				return this.messages;
			}
		}

		// Token: 0x0600034B RID: 843 RVA: 0x00011A50 File Offset: 0x0000FC50
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

		// Token: 0x0600034C RID: 844 RVA: 0x00011ABD File Offset: 0x0000FCBD
		internal void Clear()
		{
			this.messages.Clear();
			this.lastMessage = null;
		}

		// Token: 0x040000C2 RID: 194
		public int maxMessages = 200;

		// Token: 0x040000C3 RID: 195
		private Queue<LogMessage> messages = new Queue<LogMessage>();

		// Token: 0x040000C4 RID: 196
		private LogMessage lastMessage;
	}
}
