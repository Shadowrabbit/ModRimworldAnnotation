using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200109A RID: 4250
	public class SignalAction_Delay : SignalAction
	{
		// Token: 0x17001157 RID: 4439
		// (get) Token: 0x06006554 RID: 25940 RVA: 0x00223CE5 File Offset: 0x00221EE5
		public bool Activated
		{
			get
			{
				return this.activated;
			}
		}

		// Token: 0x17001158 RID: 4440
		// (get) Token: 0x06006555 RID: 25941 RVA: 0x00002688 File Offset: 0x00000888
		public virtual Alert_ActionDelay Alert
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001159 RID: 4441
		// (get) Token: 0x06006556 RID: 25942 RVA: 0x0001276E File Offset: 0x0001096E
		public virtual bool ShouldRemoveNow
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06006557 RID: 25943 RVA: 0x00223CED File Offset: 0x00221EED
		public override void Notify_SignalReceived(Signal signal)
		{
			if (signal.tag == this.signalTag)
			{
				this.DoAction(signal.args);
			}
		}

		// Token: 0x06006558 RID: 25944 RVA: 0x00223D0E File Offset: 0x00221F0E
		protected override void DoAction(SignalArgs args)
		{
			if (this.delayTicks <= 0)
			{
				this.CompleteInt();
				return;
			}
			this.activated = true;
		}

		// Token: 0x06006559 RID: 25945 RVA: 0x00223D27 File Offset: 0x00221F27
		protected virtual void Complete()
		{
			if (!this.completedSignalTag.NullOrEmpty())
			{
				Find.SignalManager.SendSignal(new Signal(this.completedSignalTag));
			}
		}

		// Token: 0x0600655A RID: 25946 RVA: 0x00223D4B File Offset: 0x00221F4B
		private void CompleteInt()
		{
			this.Complete();
			if (!base.Destroyed)
			{
				this.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x0600655B RID: 25947 RVA: 0x00223D62 File Offset: 0x00221F62
		public override void Tick()
		{
			if (!this.activated)
			{
				return;
			}
			if (this.delayTicks <= 0)
			{
				this.CompleteInt();
				return;
			}
			if (this.ShouldRemoveNow)
			{
				this.Destroy(DestroyMode.Vanish);
				return;
			}
			this.delayTicks--;
		}

		// Token: 0x0600655C RID: 25948 RVA: 0x00223D9B File Offset: 0x00221F9B
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.activated, "activated", false, false);
			Scribe_Values.Look<int>(ref this.delayTicks, "delayTicks", 0, false);
			Scribe_Values.Look<string>(ref this.completedSignalTag, "completedSignalTag", null, false);
		}

		// Token: 0x04003912 RID: 14610
		public int delayTicks;

		// Token: 0x04003913 RID: 14611
		public string completedSignalTag;

		// Token: 0x04003914 RID: 14612
		private bool activated;
	}
}
