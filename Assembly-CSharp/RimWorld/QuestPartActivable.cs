using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001129 RID: 4393
	public abstract class QuestPartActivable : QuestPart
	{
		// Token: 0x17000F01 RID: 3841
		// (get) Token: 0x0600601E RID: 24606 RVA: 0x00042634 File Offset: 0x00040834
		public QuestPartState State
		{
			get
			{
				return this.state;
			}
		}

		// Token: 0x17000F02 RID: 3842
		// (get) Token: 0x0600601F RID: 24607 RVA: 0x0004263C File Offset: 0x0004083C
		public int EnableTick
		{
			get
			{
				if (this.State != QuestPartState.Enabled)
				{
					return -1;
				}
				return this.enableTick;
			}
		}

		// Token: 0x17000F03 RID: 3843
		// (get) Token: 0x06006020 RID: 24608 RVA: 0x001E37A0 File Offset: 0x001E19A0
		public string OutSignalEnabled
		{
			get
			{
				return string.Concat(new object[]
				{
					"Quest",
					this.quest.id,
					".Part",
					base.Index,
					".Enabled"
				});
			}
		}

		// Token: 0x17000F04 RID: 3844
		// (get) Token: 0x06006021 RID: 24609 RVA: 0x001E37F4 File Offset: 0x001E19F4
		public string OutSignalCompleted
		{
			get
			{
				return string.Concat(new object[]
				{
					"Quest",
					this.quest.id,
					".Part",
					base.Index,
					".Completed"
				});
			}
		}

		// Token: 0x17000F05 RID: 3845
		// (get) Token: 0x06006022 RID: 24610 RVA: 0x0004264F File Offset: 0x0004084F
		public virtual string ExpiryInfoPart { get; }

		// Token: 0x17000F06 RID: 3846
		// (get) Token: 0x06006023 RID: 24611 RVA: 0x00042657 File Offset: 0x00040857
		public virtual string ExpiryInfoPartTip { get; }

		// Token: 0x17000F07 RID: 3847
		// (get) Token: 0x06006024 RID: 24612 RVA: 0x0004265F File Offset: 0x0004085F
		public virtual AlertReport AlertReport
		{
			get
			{
				return AlertReport.Inactive;
			}
		}

		// Token: 0x17000F08 RID: 3848
		// (get) Token: 0x06006025 RID: 24613 RVA: 0x0000C32E File Offset: 0x0000A52E
		public virtual string AlertLabel
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000F09 RID: 3849
		// (get) Token: 0x06006026 RID: 24614 RVA: 0x0000C32E File Offset: 0x0000A52E
		public virtual string AlertExplanation
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000F0A RID: 3850
		// (get) Token: 0x06006027 RID: 24615 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public virtual bool AlertCritical
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000F0B RID: 3851
		// (get) Token: 0x06006028 RID: 24616 RVA: 0x00042666 File Offset: 0x00040866
		public bool AlertDirty
		{
			get
			{
				return this.cachedAlert != null && ((this.AlertCritical && !(this.cachedAlert is Alert_CustomCritical)) || (!this.AlertCritical && !(this.cachedAlert is Alert_Custom)));
			}
		}

		// Token: 0x17000F0C RID: 3852
		// (get) Token: 0x06006029 RID: 24617 RVA: 0x001E3848 File Offset: 0x001E1A48
		public Alert CachedAlert
		{
			get
			{
				AlertReport alertReport = this.AlertReport;
				if (this.cachedAlert == null)
				{
					if (!alertReport.active)
					{
						return null;
					}
					if (this.AlertCritical)
					{
						this.cachedAlert = new Alert_CustomCritical();
					}
					else
					{
						this.cachedAlert = new Alert_Custom();
					}
				}
				Alert_Custom alert_Custom = this.cachedAlert as Alert_Custom;
				if (alert_Custom != null)
				{
					if (alertReport.active)
					{
						alert_Custom.label = this.AlertLabel;
						alert_Custom.explanation = this.AlertExplanation;
					}
					alert_Custom.report = alertReport;
				}
				Alert_CustomCritical alert_CustomCritical = this.cachedAlert as Alert_CustomCritical;
				if (alert_CustomCritical != null)
				{
					if (alertReport.active)
					{
						alert_CustomCritical.label = this.AlertLabel;
						alert_CustomCritical.explanation = this.AlertExplanation;
					}
					alert_CustomCritical.report = alertReport;
				}
				return this.cachedAlert;
			}
		}

		// Token: 0x0600602A RID: 24618 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void QuestPartTick()
		{
		}

		// Token: 0x0600602B RID: 24619 RVA: 0x0000C32E File Offset: 0x0000A52E
		public virtual string ExtraInspectString(ISelectable target)
		{
			return null;
		}

		// Token: 0x0600602C RID: 24620 RVA: 0x000426A4 File Offset: 0x000408A4
		public void ClearCachedAlert()
		{
			this.cachedAlert = null;
		}

		// Token: 0x0600602D RID: 24621 RVA: 0x001E3900 File Offset: 0x001E1B00
		protected virtual void Enable(SignalArgs receivedArgs)
		{
			if (this.state == QuestPartState.Enabled)
			{
				Log.Error("Tried to enable QuestPart while already enabled. part=" + this, false);
				return;
			}
			this.state = QuestPartState.Enabled;
			this.enableTick = Find.TickManager.TicksGame;
			Find.SignalManager.SendSignal(new Signal(this.OutSignalEnabled));
		}

		// Token: 0x0600602E RID: 24622 RVA: 0x001E3954 File Offset: 0x001E1B54
		protected void Complete()
		{
			this.Complete(default(SignalArgs));
		}

		// Token: 0x0600602F RID: 24623 RVA: 0x000426AD File Offset: 0x000408AD
		protected void Complete(NamedArgument signalArg1)
		{
			this.Complete(new SignalArgs(signalArg1));
		}

		// Token: 0x06006030 RID: 24624 RVA: 0x000426BB File Offset: 0x000408BB
		protected void Complete(NamedArgument signalArg1, NamedArgument signalArg2)
		{
			this.Complete(new SignalArgs(signalArg1, signalArg2));
		}

		// Token: 0x06006031 RID: 24625 RVA: 0x000426CA File Offset: 0x000408CA
		protected void Complete(NamedArgument signalArg1, NamedArgument signalArg2, NamedArgument signalArg3)
		{
			this.Complete(new SignalArgs(signalArg1, signalArg2, signalArg3));
		}

		// Token: 0x06006032 RID: 24626 RVA: 0x000426DA File Offset: 0x000408DA
		protected void Complete(NamedArgument signalArg1, NamedArgument signalArg2, NamedArgument signalArg3, NamedArgument signalArg4)
		{
			this.Complete(new SignalArgs(signalArg1, signalArg2, signalArg3, signalArg4));
		}

		// Token: 0x06006033 RID: 24627 RVA: 0x000426EC File Offset: 0x000408EC
		protected void Complete(params NamedArgument[] signalArgs)
		{
			this.Complete(new SignalArgs(signalArgs));
		}

		// Token: 0x06006034 RID: 24628 RVA: 0x001E3970 File Offset: 0x001E1B70
		protected virtual void Complete(SignalArgs signalArgs)
		{
			if (this.state != QuestPartState.Enabled)
			{
				Log.Error("Tried to end QuestPart but its state is not Active. part=" + this, false);
				return;
			}
			this.state = QuestPartState.Disabled;
			if (this.outcomeCompletedSignalArg != QuestEndOutcome.Unknown)
			{
				signalArgs.Add(this.outcomeCompletedSignalArg.Named("OUTCOME"));
			}
			Find.SignalManager.SendSignal(new Signal(this.OutSignalCompleted, signalArgs));
			if (!this.outSignalsCompleted.NullOrEmpty<string>())
			{
				for (int i = 0; i < this.outSignalsCompleted.Count; i++)
				{
					if (!this.outSignalsCompleted[i].NullOrEmpty())
					{
						Find.SignalManager.SendSignal(new Signal(this.outSignalsCompleted[i], signalArgs));
					}
				}
			}
		}

		// Token: 0x06006035 RID: 24629 RVA: 0x000426FA File Offset: 0x000408FA
		protected virtual void Disable()
		{
			if (this.state != QuestPartState.Enabled)
			{
				Log.Error("Tried to disable QuestPart but its state is not enabled. part=" + this, false);
				return;
			}
			this.state = QuestPartState.Disabled;
		}

		// Token: 0x06006036 RID: 24630 RVA: 0x001E3A2C File Offset: 0x001E1C2C
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			if (signal.tag == this.inSignalEnable && (this.state == QuestPartState.NeverEnabled || (this.state == QuestPartState.Disabled && this.reactivatable)))
			{
				this.Enable(signal.args);
				return;
			}
			if (this.state == QuestPartState.Enabled)
			{
				if (signal.tag == this.inSignalDisable)
				{
					this.Disable();
					return;
				}
				this.ProcessQuestSignal(signal);
			}
		}

		// Token: 0x06006037 RID: 24631 RVA: 0x00006A05 File Offset: 0x00004C05
		protected virtual void ProcessQuestSignal(Signal signal)
		{
		}

		// Token: 0x06006038 RID: 24632 RVA: 0x0004271E File Offset: 0x0004091E
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignalEnable = this.quest.InitiateSignal;
		}

		// Token: 0x06006039 RID: 24633 RVA: 0x001E3A9C File Offset: 0x001E1C9C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignalEnable, "inSignalEnable", null, false);
			Scribe_Values.Look<string>(ref this.inSignalDisable, "inSignalDisable", null, false);
			Scribe_Values.Look<bool>(ref this.reactivatable, "reactivatable", false, false);
			if (Scribe.mode != LoadSaveMode.Saving || !this.outSignalsCompleted.NullOrEmpty<string>())
			{
				Scribe_Collections.Look<string>(ref this.outSignalsCompleted, "outSignalsCompleted", LookMode.Value, Array.Empty<object>());
			}
			Scribe_Values.Look<QuestEndOutcome>(ref this.outcomeCompletedSignalArg, "outcomeCompletedSignalArg", QuestEndOutcome.Unknown, false);
			Scribe_Values.Look<QuestPartState>(ref this.state, "state", QuestPartState.NeverEnabled, false);
			Scribe_Values.Look<int>(ref this.enableTick, "enableTick", -1, false);
		}

		// Token: 0x04004034 RID: 16436
		public string inSignalEnable;

		// Token: 0x04004035 RID: 16437
		public string inSignalDisable;

		// Token: 0x04004036 RID: 16438
		public bool reactivatable;

		// Token: 0x04004037 RID: 16439
		public List<string> outSignalsCompleted = new List<string>();

		// Token: 0x04004038 RID: 16440
		public QuestEndOutcome outcomeCompletedSignalArg;

		// Token: 0x04004039 RID: 16441
		private QuestPartState state;

		// Token: 0x0400403A RID: 16442
		protected int enableTick = -1;

		// Token: 0x0400403B RID: 16443
		private Alert cachedAlert;
	}
}
