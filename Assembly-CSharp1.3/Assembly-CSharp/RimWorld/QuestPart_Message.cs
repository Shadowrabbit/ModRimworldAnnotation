using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B87 RID: 2951
	public class QuestPart_Message : QuestPart
	{
		// Token: 0x17000C17 RID: 3095
		// (get) Token: 0x06004503 RID: 17667 RVA: 0x0016DD9B File Offset: 0x0016BF9B
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in base.QuestLookTargets)
				{
					yield return globalTargetInfo;
				}
				IEnumerator<GlobalTargetInfo> enumerator = null;
				if (this.addQuestLookTargets)
				{
					GlobalTargetInfo globalTargetInfo2 = this.lookTargets.TryGetPrimaryTarget();
					if (globalTargetInfo2.IsValid)
					{
						yield return globalTargetInfo2;
					}
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x06004504 RID: 17668 RVA: 0x0016DDAC File Offset: 0x0016BFAC
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				LookTargets lookTargets = this.lookTargets;
				if (this.getLookTargetsFromSignal && !lookTargets.IsValid())
				{
					SignalArgsUtility.TryGetLookTargets(signal.args, "SUBJECT", out lookTargets);
				}
				TaggedString formattedText = signal.args.GetFormattedText(this.message);
				if (!formattedText.NullOrEmpty())
				{
					Messages.Message(formattedText, lookTargets, this.messageType ?? MessageTypeDefOf.NeutralEvent, this.quest.hidden ? null : this.quest, this.historical);
				}
			}
		}

		// Token: 0x06004505 RID: 17669 RVA: 0x0016DE58 File Offset: 0x0016C058
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<string>(ref this.message, "message", null, false);
			Scribe_Defs.Look<MessageTypeDef>(ref this.messageType, "messageType");
			Scribe_Deep.Look<LookTargets>(ref this.lookTargets, "lookTargets", Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.historical, "historical", true, false);
			Scribe_Values.Look<bool>(ref this.getLookTargetsFromSignal, "getLookTargetsFromSignal", true, false);
			Scribe_Values.Look<bool>(ref this.addQuestLookTargets, "addQuestLookTargets", true, false);
		}

		// Token: 0x06004506 RID: 17670 RVA: 0x0016DEEA File Offset: 0x0016C0EA
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int;
			this.message = "Dev: Test";
			this.messageType = MessageTypeDefOf.PositiveEvent;
		}

		// Token: 0x040029E7 RID: 10727
		public string inSignal;

		// Token: 0x040029E8 RID: 10728
		public string message;

		// Token: 0x040029E9 RID: 10729
		public MessageTypeDef messageType;

		// Token: 0x040029EA RID: 10730
		public LookTargets lookTargets;

		// Token: 0x040029EB RID: 10731
		public bool historical = true;

		// Token: 0x040029EC RID: 10732
		public bool getLookTargetsFromSignal = true;

		// Token: 0x040029ED RID: 10733
		public bool addQuestLookTargets = true;
	}
}
