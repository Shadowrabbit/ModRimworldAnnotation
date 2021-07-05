using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B63 RID: 2915
	public class QuestPart_Alert : QuestPartActivable
	{
		// Token: 0x17000BF4 RID: 3060
		// (get) Token: 0x06004430 RID: 17456 RVA: 0x0016A31C File Offset: 0x0016851C
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

		// Token: 0x17000BF5 RID: 3061
		// (get) Token: 0x06004431 RID: 17457 RVA: 0x0016A32C File Offset: 0x0016852C
		public override string AlertLabel
		{
			get
			{
				return this.resolvedLabel;
			}
		}

		// Token: 0x17000BF6 RID: 3062
		// (get) Token: 0x06004432 RID: 17458 RVA: 0x0016A334 File Offset: 0x00168534
		public override string AlertExplanation
		{
			get
			{
				return this.resolvedExplanation;
			}
		}

		// Token: 0x17000BF7 RID: 3063
		// (get) Token: 0x06004433 RID: 17459 RVA: 0x0016A33C File Offset: 0x0016853C
		public override bool AlertCritical
		{
			get
			{
				return this.critical;
			}
		}

		// Token: 0x17000BF8 RID: 3064
		// (get) Token: 0x06004434 RID: 17460 RVA: 0x0016A344 File Offset: 0x00168544
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

		// Token: 0x06004435 RID: 17461 RVA: 0x0016A36C File Offset: 0x0016856C
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

		// Token: 0x06004436 RID: 17462 RVA: 0x0016A3F0 File Offset: 0x001685F0
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

		// Token: 0x06004437 RID: 17463 RVA: 0x0016A499 File Offset: 0x00168699
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.label = "Dev: Test";
			this.explanation = "Test text";
		}

		// Token: 0x0400295E RID: 10590
		public string label;

		// Token: 0x0400295F RID: 10591
		public string explanation;

		// Token: 0x04002960 RID: 10592
		public LookTargets lookTargets;

		// Token: 0x04002961 RID: 10593
		public bool critical;

		// Token: 0x04002962 RID: 10594
		public bool getLookTargetsFromSignal;

		// Token: 0x04002963 RID: 10595
		private string resolvedLabel;

		// Token: 0x04002964 RID: 10596
		private string resolvedExplanation;

		// Token: 0x04002965 RID: 10597
		private LookTargets resolvedLookTargets;
	}
}
