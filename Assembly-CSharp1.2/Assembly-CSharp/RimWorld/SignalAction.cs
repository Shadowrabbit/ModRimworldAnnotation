using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020016F6 RID: 5878
	public abstract class SignalAction : Thing
	{
		// Token: 0x06008133 RID: 33075 RVA: 0x00056C3F File Offset: 0x00054E3F
		public override void Notify_SignalReceived(Signal signal)
		{
			base.Notify_SignalReceived(signal);
			if (signal.tag == this.signalTag)
			{
				this.DoAction(signal.args);
				if (!base.Destroyed)
				{
					this.Destroy(DestroyMode.Vanish);
				}
			}
		}

		// Token: 0x06008134 RID: 33076
		protected abstract void DoAction(SignalArgs args);

		// Token: 0x06008135 RID: 33077 RVA: 0x00056C76 File Offset: 0x00054E76
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.signalTag, "signalTag", null, false);
		}

		// Token: 0x040053B5 RID: 21429
		public string signalTag;
	}
}
