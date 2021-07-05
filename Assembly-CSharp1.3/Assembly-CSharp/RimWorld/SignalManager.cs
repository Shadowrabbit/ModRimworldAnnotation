using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BD5 RID: 3029
	public class SignalManager
	{
		// Token: 0x0600471D RID: 18205 RVA: 0x0017859A File Offset: 0x0017679A
		public void RegisterReceiver(ISignalReceiver receiver)
		{
			if (receiver == null)
			{
				Log.Error("Tried to register a null reciever.");
				return;
			}
			if (this.receivers.Contains(receiver))
			{
				Log.Error("Tried to register the same receiver twice: " + receiver.ToStringSafe<ISignalReceiver>());
				return;
			}
			this.receivers.Add(receiver);
		}

		// Token: 0x0600471E RID: 18206 RVA: 0x001785DA File Offset: 0x001767DA
		public void DeregisterReceiver(ISignalReceiver receiver)
		{
			this.receivers.Remove(receiver);
		}

		// Token: 0x0600471F RID: 18207 RVA: 0x001785EC File Offset: 0x001767EC
		public void SendSignal(Signal signal)
		{
			if (this.signalsThisFrame >= 3000)
			{
				if (this.signalsThisFrame == 3000)
				{
					Log.Error("Reached max signals per frame (" + 3000 + "). Ignoring further signals.");
				}
				this.signalsThisFrame++;
				return;
			}
			this.signalsThisFrame++;
			if (DebugViewSettings.logSignals)
			{
				Log.Message("Signal: tag=" + signal.tag.ToStringSafe<string>() + " args=" + signal.args.Args.ToStringSafeEnumerable());
			}
			SignalManager.tmpReceivers.Clear();
			SignalManager.tmpReceivers.AddRange(this.receivers);
			for (int i = 0; i < SignalManager.tmpReceivers.Count; i++)
			{
				try
				{
					SignalManager.tmpReceivers[i].Notify_SignalReceived(signal);
				}
				catch (Exception ex)
				{
					Log.Error(string.Concat(new object[]
					{
						"Error while sending signal to ",
						SignalManager.tmpReceivers[i].ToStringSafe<ISignalReceiver>(),
						": ",
						ex
					}));
				}
			}
		}

		// Token: 0x06004720 RID: 18208 RVA: 0x00178714 File Offset: 0x00176914
		public void SignalManagerUpdate()
		{
			this.signalsThisFrame = 0;
		}

		// Token: 0x04002B98 RID: 11160
		private int signalsThisFrame;

		// Token: 0x04002B99 RID: 11161
		private const int MaxSignalsPerFrame = 3000;

		// Token: 0x04002B9A RID: 11162
		public List<ISignalReceiver> receivers = new List<ISignalReceiver>();

		// Token: 0x04002B9B RID: 11163
		private static List<ISignalReceiver> tmpReceivers = new List<ISignalReceiver>();
	}
}
