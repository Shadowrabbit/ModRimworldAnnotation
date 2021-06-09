using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020010BA RID: 4282
	public class QuestPart_Dialog : QuestPart
	{
		// Token: 0x17000E81 RID: 3713
		// (get) Token: 0x06005D65 RID: 23909 RVA: 0x00040C4D File Offset: 0x0003EE4D
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

		// Token: 0x17000E82 RID: 3714
		// (get) Token: 0x06005D66 RID: 23910 RVA: 0x00040C5D File Offset: 0x0003EE5D
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

		// Token: 0x06005D67 RID: 23911 RVA: 0x001DC978 File Offset: 0x001DAB78
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

		// Token: 0x06005D68 RID: 23912 RVA: 0x001DCB80 File Offset: 0x001DAD80
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

		// Token: 0x06005D69 RID: 23913 RVA: 0x001DCC3C File Offset: 0x001DAE3C
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

		// Token: 0x04003E78 RID: 15992
		public string inSignal;

		// Token: 0x04003E79 RID: 15993
		public string text;

		// Token: 0x04003E7A RID: 15994
		public string title;

		// Token: 0x04003E7B RID: 15995
		public List<QuestPart_Dialog.Option> options = new List<QuestPart_Dialog.Option>();

		// Token: 0x04003E7C RID: 15996
		public Faction relatedFaction;

		// Token: 0x04003E7D RID: 15997
		public bool addToArchive = true;

		// Token: 0x04003E7E RID: 15998
		public bool radioMode;

		// Token: 0x04003E7F RID: 15999
		public bool getLookTargetsFromSignal;

		// Token: 0x04003E80 RID: 16000
		public LookTargets lookTargets;

		// Token: 0x020010BB RID: 4283
		public class Option : IExposable
		{
			// Token: 0x06005D6D RID: 23917 RVA: 0x00040C87 File Offset: 0x0003EE87
			public void ExposeData()
			{
				Scribe_Values.Look<string>(ref this.text, "text", null, false);
				Scribe_Values.Look<string>(ref this.outSignal, "outSignal", null, false);
			}

			// Token: 0x04003E81 RID: 16001
			public string text;

			// Token: 0x04003E82 RID: 16002
			public string outSignal;
		}
	}
}
