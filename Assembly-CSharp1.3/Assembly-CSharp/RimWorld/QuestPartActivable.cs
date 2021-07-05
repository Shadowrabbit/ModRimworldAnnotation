using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BBA RID: 3002
	public abstract class QuestPartActivable : QuestPart
	{
		// Token: 0x17000C42 RID: 3138
		// (get) Token: 0x06004615 RID: 17941 RVA: 0x00172798 File Offset: 0x00170998
		public QuestPartState State
		{
			get
			{
				return this.state;
			}
		}

		// Token: 0x17000C43 RID: 3139
		// (get) Token: 0x06004616 RID: 17942 RVA: 0x001727A0 File Offset: 0x001709A0
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

		// Token: 0x17000C44 RID: 3140
		// (get) Token: 0x06004617 RID: 17943 RVA: 0x001727B4 File Offset: 0x001709B4
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

		// Token: 0x17000C45 RID: 3141
		// (get) Token: 0x06004618 RID: 17944 RVA: 0x00172808 File Offset: 0x00170A08
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

		// Token: 0x17000C46 RID: 3142
		// (get) Token: 0x06004619 RID: 17945 RVA: 0x00172859 File Offset: 0x00170A59
		public virtual string ExpiryInfoPart { get; }

		// Token: 0x17000C47 RID: 3143
		// (get) Token: 0x0600461A RID: 17946 RVA: 0x00172861 File Offset: 0x00170A61
		public virtual string ExpiryInfoPartTip { get; }

		// Token: 0x17000C48 RID: 3144
		// (get) Token: 0x0600461B RID: 17947 RVA: 0x00172869 File Offset: 0x00170A69
		public virtual AlertReport AlertReport
		{
			get
			{
				return AlertReport.Inactive;
			}
		}

		// Token: 0x17000C49 RID: 3145
		// (get) Token: 0x0600461C RID: 17948 RVA: 0x00002688 File Offset: 0x00000888
		public virtual string AlertLabel
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000C4A RID: 3146
		// (get) Token: 0x0600461D RID: 17949 RVA: 0x00002688 File Offset: 0x00000888
		public virtual string AlertExplanation
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000C4B RID: 3147
		// (get) Token: 0x0600461E RID: 17950 RVA: 0x0001276E File Offset: 0x0001096E
		public virtual bool AlertCritical
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000C4C RID: 3148
		// (get) Token: 0x0600461F RID: 17951 RVA: 0x00172870 File Offset: 0x00170A70
		public bool AlertDirty
		{
			get
			{
				return this.cachedAlert != null && ((this.AlertCritical && !(this.cachedAlert is Alert_CustomCritical)) || (!this.AlertCritical && !(this.cachedAlert is Alert_Custom)));
			}
		}

		// Token: 0x17000C4D RID: 3149
		// (get) Token: 0x06004620 RID: 17952 RVA: 0x001728B0 File Offset: 0x00170AB0
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

		// Token: 0x06004621 RID: 17953 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void QuestPartTick()
		{
		}

		// Token: 0x06004622 RID: 17954 RVA: 0x00002688 File Offset: 0x00000888
		public virtual string ExtraInspectString(ISelectable target)
		{
			return null;
		}

		// Token: 0x06004623 RID: 17955 RVA: 0x00002688 File Offset: 0x00000888
		public virtual IEnumerable<Gizmo> ExtraGizmos(ISelectable target)
		{
			return null;
		}

		// Token: 0x06004624 RID: 17956 RVA: 0x00172968 File Offset: 0x00170B68
		public void ClearCachedAlert()
		{
			this.cachedAlert = null;
		}

		// Token: 0x06004625 RID: 17957 RVA: 0x00172974 File Offset: 0x00170B74
		protected virtual void Enable(SignalArgs receivedArgs)
		{
			if (this.state == QuestPartState.Enabled)
			{
				Log.Error("Tried to enable QuestPart while already enabled. part=" + this);
				return;
			}
			this.state = QuestPartState.Enabled;
			this.enableTick = Find.TickManager.TicksGame;
			Find.SignalManager.SendSignal(new Signal(this.OutSignalEnabled));
		}

		// Token: 0x06004626 RID: 17958 RVA: 0x001729C8 File Offset: 0x00170BC8
		protected void Complete()
		{
			this.Complete(default(SignalArgs));
		}

		// Token: 0x06004627 RID: 17959 RVA: 0x001729E4 File Offset: 0x00170BE4
		protected void Complete(NamedArgument signalArg1)
		{
			this.Complete(new SignalArgs(signalArg1));
		}

		// Token: 0x06004628 RID: 17960 RVA: 0x001729F2 File Offset: 0x00170BF2
		protected void Complete(NamedArgument signalArg1, NamedArgument signalArg2)
		{
			this.Complete(new SignalArgs(signalArg1, signalArg2));
		}

		// Token: 0x06004629 RID: 17961 RVA: 0x00172A01 File Offset: 0x00170C01
		protected void Complete(NamedArgument signalArg1, NamedArgument signalArg2, NamedArgument signalArg3)
		{
			this.Complete(new SignalArgs(signalArg1, signalArg2, signalArg3));
		}

		// Token: 0x0600462A RID: 17962 RVA: 0x00172A11 File Offset: 0x00170C11
		protected void Complete(NamedArgument signalArg1, NamedArgument signalArg2, NamedArgument signalArg3, NamedArgument signalArg4)
		{
			this.Complete(new SignalArgs(signalArg1, signalArg2, signalArg3, signalArg4));
		}

		// Token: 0x0600462B RID: 17963 RVA: 0x00172A23 File Offset: 0x00170C23
		protected void Complete(params NamedArgument[] signalArgs)
		{
			this.Complete(new SignalArgs(signalArgs));
		}

		// Token: 0x0600462C RID: 17964 RVA: 0x00172A34 File Offset: 0x00170C34
		protected virtual void Complete(SignalArgs signalArgs)
		{
			if (this.state != QuestPartState.Enabled)
			{
				Log.Error("Tried to end QuestPart but its state is not Active. part=" + this);
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

		// Token: 0x0600462D RID: 17965 RVA: 0x00172AEE File Offset: 0x00170CEE
		protected virtual void Disable()
		{
			if (this.state != QuestPartState.Enabled)
			{
				Log.Error("Tried to disable QuestPart but its state is not enabled. part=" + this);
				return;
			}
			this.state = QuestPartState.Disabled;
		}

		// Token: 0x0600462E RID: 17966 RVA: 0x00172B14 File Offset: 0x00170D14
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

		// Token: 0x0600462F RID: 17967 RVA: 0x0000313F File Offset: 0x0000133F
		protected virtual void ProcessQuestSignal(Signal signal)
		{
		}

		// Token: 0x06004630 RID: 17968 RVA: 0x00172B84 File Offset: 0x00170D84
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignalEnable = this.quest.InitiateSignal;
		}

		// Token: 0x06004631 RID: 17969 RVA: 0x00164FC8 File Offset: 0x001631C8
		public void DebugForceComplete()
		{
			this.Complete();
		}

		// Token: 0x06004632 RID: 17970 RVA: 0x00172BA0 File Offset: 0x00170DA0
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

		// Token: 0x04002AAB RID: 10923
		public string inSignalEnable;

		// Token: 0x04002AAC RID: 10924
		public string inSignalDisable;

		// Token: 0x04002AAD RID: 10925
		public bool reactivatable;

		// Token: 0x04002AAE RID: 10926
		public List<string> outSignalsCompleted = new List<string>();

		// Token: 0x04002AAF RID: 10927
		public QuestEndOutcome outcomeCompletedSignalArg;

		// Token: 0x04002AB0 RID: 10928
		private QuestPartState state;

		// Token: 0x04002AB1 RID: 10929
		protected int enableTick = -1;

		// Token: 0x04002AB2 RID: 10930
		private Alert cachedAlert;
	}
}
