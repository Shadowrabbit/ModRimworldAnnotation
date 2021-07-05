using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B70 RID: 2928
	public class QuestPart_Dialog : QuestPart
	{
		// Token: 0x17000C03 RID: 3075
		// (get) Token: 0x0600447E RID: 17534 RVA: 0x0016B60C File Offset: 0x0016980C
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

		// Token: 0x17000C04 RID: 3076
		// (get) Token: 0x0600447F RID: 17535 RVA: 0x0016B61C File Offset: 0x0016981C
		public override IEnumerable<Faction> InvolvedFactions
		{
			get
			{
				foreach (Faction faction in base.InvolvedFactions)
				{
					yield return faction;
				}
				IEnumerator<Faction> enumerator = null;
				if (this.relatedFaction != null)
				{
					yield return this.relatedFaction;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x06004480 RID: 17536 RVA: 0x0016B62C File Offset: 0x0016982C
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				DiaNode diaNode = new DiaNode(signal.args.GetFormattedText(this.text));
				LookTargets resolvedLookTargets = this.lookTargets;
				if (this.getLookTargetsFromSignal && !resolvedLookTargets.IsValid())
				{
					SignalArgsUtility.TryGetLookTargets(signal.args, "SUBJECT", out resolvedLookTargets);
				}
				if (resolvedLookTargets.IsValid())
				{
					DiaOption diaOption = new DiaOption("JumpToLocation".Translate());
					diaOption.action = delegate()
					{
						CameraJumper.TryJumpAndSelect(resolvedLookTargets.TryGetPrimaryTarget());
					};
					diaOption.resolveTree = true;
					diaNode.options.Add(diaOption);
				}
				if (this.options.Any<QuestPart_Dialog.Option>())
				{
					for (int i = 0; i < this.options.Count; i++)
					{
						int localIndex = i;
						DiaOption diaOption2 = new DiaOption(signal.args.GetFormattedText(this.options[i].text));
						diaOption2.action = delegate()
						{
							Find.SignalManager.SendSignal(new Signal(this.options[localIndex].outSignal));
						};
						diaOption2.resolveTree = true;
						diaNode.options.Add(diaOption2);
					}
				}
				else
				{
					DiaOption diaOption3 = new DiaOption("OK".Translate());
					diaOption3.resolveTree = true;
					diaNode.options.Add(diaOption3);
				}
				TaggedString formattedText = signal.args.GetFormattedText(this.title);
				Find.WindowStack.Add(new Dialog_NodeTreeWithFactionInfo(diaNode, this.relatedFaction, true, this.radioMode, formattedText));
				if (this.addToArchive)
				{
					Find.Archive.Add(new ArchivedDialog(diaNode.text, formattedText, this.relatedFaction));
				}
			}
		}

		// Token: 0x06004481 RID: 17537 RVA: 0x0016B834 File Offset: 0x00169A34
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<string>(ref this.text, "text", null, false);
			Scribe_Values.Look<string>(ref this.title, "title", null, false);
			Scribe_Collections.Look<QuestPart_Dialog.Option>(ref this.options, "options", LookMode.Deep, Array.Empty<object>());
			Scribe_References.Look<Faction>(ref this.relatedFaction, "relatedFaction", false);
			Scribe_Values.Look<bool>(ref this.addToArchive, "addToArchive", true, false);
			Scribe_Values.Look<bool>(ref this.radioMode, "radioMode", false, false);
			Scribe_Values.Look<bool>(ref this.getLookTargetsFromSignal, "getLookTargetsFromSignal", false, false);
			Scribe_Deep.Look<LookTargets>(ref this.lookTargets, "lookTargets", Array.Empty<object>());
		}

		// Token: 0x06004482 RID: 17538 RVA: 0x0016B8F0 File Offset: 0x00169AF0
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int;
			this.title = "Title";
			this.text = "Dev: Test";
			this.relatedFaction = Faction.OfMechanoids;
			this.addToArchive = false;
			QuestPart_Dialog.Option option = new QuestPart_Dialog.Option();
			option.text = "Option 1";
			option.outSignal = "DebugSignal" + Rand.Int;
			this.options.Add(option);
			QuestPart_Dialog.Option option2 = new QuestPart_Dialog.Option();
			option2.text = "Option 2";
			option2.outSignal = "DebugSignal" + Rand.Int;
			this.options.Add(option2);
		}

		// Token: 0x0400298D RID: 10637
		public string inSignal;

		// Token: 0x0400298E RID: 10638
		public string text;

		// Token: 0x0400298F RID: 10639
		public string title;

		// Token: 0x04002990 RID: 10640
		public List<QuestPart_Dialog.Option> options = new List<QuestPart_Dialog.Option>();

		// Token: 0x04002991 RID: 10641
		public Faction relatedFaction;

		// Token: 0x04002992 RID: 10642
		public bool addToArchive = true;

		// Token: 0x04002993 RID: 10643
		public bool radioMode;

		// Token: 0x04002994 RID: 10644
		public bool getLookTargetsFromSignal;

		// Token: 0x04002995 RID: 10645
		public LookTargets lookTargets;

		// Token: 0x0200208D RID: 8333
		public class Option : IExposable
		{
			// Token: 0x0600BA62 RID: 47714 RVA: 0x003C2593 File Offset: 0x003C0793
			public void ExposeData()
			{
				Scribe_Values.Look<string>(ref this.text, "text", null, false);
				Scribe_Values.Look<string>(ref this.outSignal, "outSignal", null, false);
			}

			// Token: 0x04007CBB RID: 31931
			public string text;

			// Token: 0x04007CBC RID: 31932
			public string outSignal;
		}
	}
}
