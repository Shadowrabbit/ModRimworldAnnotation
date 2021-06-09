using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200116C RID: 4460
	public class SignalManager
	{
		// Token: 0x0600621C RID: 25116 RVA: 0x001EA87C File Offset: 0x001E8A7C
		public void RegisterReceiver(ISignalReceiver receiver)
		{
			if (receiver == null)
			{
				Log.Error("Tried to register a null reciever.", false);
				return;
			}
			if (this.receivers.Contains(receiver))
			{
				Log.Error("Tried to register the same receiver twice: " + receiver.ToStringSafe<ISignalReceiver>(), false);
				return;
			}
			this.receivers.Add(receiver);
		}

		// Token: 0x0600621D RID: 25117 RVA: 0x00043808 File Offset: 0x00041A08
		public void DeregisterReceiver(ISignalReceiver receiver)
		{
			this.receivers.Remove(receiver);
		}

		// Token: 0x0600621E RID: 25118 RVA: 0x001EA8CC File Offset: 0x001E8ACC
		public void SendSignal(Signal signal)
		{
			if (this.signalsThisFrame >= 3000)
			{
				if (this.signalsThisFrame == 3000)
				{
					Log.Error("Reached max signals per frame (" + 3000 + "). Ignoring further signals.", false);
				}
				this.signalsThisFrame++;
				return;
			}
			this.signalsThisFrame++;
			if (DebugViewSettings.logSignals)
			{
				Log.Message("Signal: tag=" + signal.tag.ToStringSafe<string>() + " args=" + signal.args.Args.ToStringSafeEnumerable(), false);
			}
			for (int i = 0; i < this.receivers.Count; i++)
			{
				try
				{
					this.receivers[i].Notify_SignalReceived(signal);
				}
				catch (Exception ex)
				{
					Log.Error(string.Concat(new object[]
					{
						"Error while sending signal to ",
						this.receivers[i].ToStringSafe<ISignalReceiver>(),
						": ",
						ex
					}), false);
				}
			}
		}

		// Token: 0x0600621F RID: 25119 RVA: 0x00043817 File Offset: 0x00041A17
		public void SignalManagerUpdate()
		{
			this.signalsThisFrame = 0;
		}

		// Token: 0x040041AD RID: 16813
		private int signalsThisFrame;

		// Token: 0x040041AE RID: 16814
		private const int MaxSignalsPerFrame = 3000;

		// Token: 0x040041AF RID: 16815
		public List<ISignalReceiver> receivers = new List<ISignalReceiver>();
	}
}
