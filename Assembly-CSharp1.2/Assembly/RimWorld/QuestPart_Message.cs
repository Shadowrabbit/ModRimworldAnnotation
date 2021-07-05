using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020010EC RID: 4332
	public class QuestPart_Message : QuestPart
	{
		// Token: 0x17000EB3 RID: 3763
		// (get) Token: 0x06005E9C RID: 24220 RVA: 0x000417BC File Offset: 0x0003F9BC
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

		// Token: 0x06005E9D RID: 24221 RVA: 0x001DFF6C File Offset: 0x001DE16C
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
					Messages.Message(formattedText, lookTargets, this.messageType ?? MessageTypeDefOf.NeutralEvent, this.quest, this.historical);
				}
			}
		}

		// Token: 0x06005E9E RID: 24222 RVA: 0x001E0008 File Offset: 0x001DE208
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<string>(ref this.message, "message", null, false);
			Scribe_Defs.Look<MessageTypeDef>(ref this.messageType, "messageType");
			Scribe_Deep.Look<LookTargets>(ref this.lookTargets, "lookTargets", Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.historical, "historical", true, false);
			Scribe_Values.Look<bool>(ref this.getLookTargetsFromSignal, "getLookTargetsFromSignal", true, false);
		}

		// Token: 0x06005E9F RID: 24223 RVA: 0x000417CC File Offset: 0x0003F9CC
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int;
			this.message = "Dev: Test";
			this.messageType = MessageTypeDefOf.PositiveEvent;
		}

		// Token: 0x04003F3E RID: 16190
		public string inSignal;

		// Token: 0x04003F3F RID: 16191
		public string message;

		// Token: 0x04003F40 RID: 16192
		public MessageTypeDef messageType;

		// Token: 0x04003F41 RID: 16193
		public LookTargets lookTargets;

		// Token: 0x04003F42 RID: 16194
		public bool historical = true;

		// Token: 0x04003F43 RID: 16195
		public bool getLookTargetsFromSignal = true;
	}
}
