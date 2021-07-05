using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020010A1 RID: 4257
	public class QuestPart_Alert : QuestPartActivable
	{
		// Token: 0x17000E66 RID: 3686
		// (get) Token: 0x06005CD2 RID: 23762 RVA: 0x0004066A File Offset: 0x0003E86A
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in base.QuestLookTargets)
				{
					yield return globalTargetInfo;
				}
				IEnumerator<GlobalTargetInfo> enumerator = null;
				GlobalTargetInfo globalTargetInfo2 = this.lookTargets.TryGetPrimaryTarget();
				if (globalTargetInfo2.IsValid)
				{
					yield return globalTargetInfo2;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x17000E67 RID: 3687
		// (get) Token: 0x06005CD3 RID: 23763 RVA: 0x0004067A File Offset: 0x0003E87A
		public override string AlertLabel
		{
			get
			{
				return this.resolvedLabel;
			}
		}

		// Token: 0x17000E68 RID: 3688
		// (get) Token: 0x06005CD4 RID: 23764 RVA: 0x00040682 File Offset: 0x0003E882
		public override string AlertExplanation
		{
			get
			{
				return this.resolvedExplanation;
			}
		}

		// Token: 0x17000E69 RID: 3689
		// (get) Token: 0x06005CD5 RID: 23765 RVA: 0x0004068A File Offset: 0x0003E88A
		public override bool AlertCritical
		{
			get
			{
				return this.critical;
			}
		}

		// Token: 0x17000E6A RID: 3690
		// (get) Token: 0x06005CD6 RID: 23766 RVA: 0x00040692 File Offset: 0x0003E892
		public override AlertReport AlertReport
		{
			get
			{
				if (this.resolvedLookTargets.IsValid())
				{
					return AlertReport.CulpritsAre(this.resolvedLookTargets.targets);
				}
				return AlertReport.Active;
			}
		}

		// Token: 0x06005CD7 RID: 23767 RVA: 0x001DB608 File Offset: 0x001D9808
		protected override void Enable(SignalArgs receivedArgs)
		{
			base.Enable(receivedArgs);
			this.resolvedLabel = receivedArgs.GetFormattedText(this.label);
			this.resolvedExplanation = receivedArgs.GetFormattedText(this.explanation);
			this.resolvedLookTargets = this.lookTargets;
			if (this.getLookTargetsFromSignal && !this.resolvedLookTargets.IsValid())
			{
				SignalArgsUtility.TryGetLookTargets(receivedArgs, "SUBJECT", out this.resolvedLookTargets);
			}
		}

		// Token: 0x06005CD8 RID: 23768 RVA: 0x001DB68C File Offset: 0x001D988C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.label, "label", null, false);
			Scribe_Values.Look<string>(ref this.explanation, "explanation", null, false);
			Scribe_Deep.Look<LookTargets>(ref this.lookTargets, "lookTargets", Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.critical, "critical", false, false);
			Scribe_Values.Look<bool>(ref this.getLookTargetsFromSignal, "getLookTargetsFromSignal", false, false);
			Scribe_Values.Look<string>(ref this.resolvedLabel, "resolvedLabel", null, false);
			Scribe_Values.Look<string>(ref this.resolvedExplanation, "resolvedExplanation", null, false);
			Scribe_Deep.Look<LookTargets>(ref this.resolvedLookTargets, "resolvedLookTargets", Array.Empty<object>());
		}

		// Token: 0x06005CD9 RID: 23769 RVA: 0x000406B7 File Offset: 0x0003E8B7
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.label = "Dev: Test";
			this.explanation = "Test text";
		}

		// Token: 0x04003E26 RID: 15910
		public string label;

		// Token: 0x04003E27 RID: 15911
		public string explanation;

		// Token: 0x04003E28 RID: 15912
		public LookTargets lookTargets;

		// Token: 0x04003E29 RID: 15913
		public bool critical;

		// Token: 0x04003E2A RID: 15914
		public bool getLookTargetsFromSignal;

		// Token: 0x04003E2B RID: 15915
		private string resolvedLabel;

		// Token: 0x04003E2C RID: 15916
		private string resolvedExplanation;

		// Token: 0x04003E2D RID: 15917
		private LookTargets resolvedLookTargets;
	}
}
