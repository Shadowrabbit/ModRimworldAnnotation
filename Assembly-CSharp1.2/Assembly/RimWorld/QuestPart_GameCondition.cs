using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200114F RID: 4431
	public class QuestPart_GameCondition : QuestPart
	{
		// Token: 0x17000F3B RID: 3899
		// (get) Token: 0x0600613C RID: 24892 RVA: 0x00042FCB File Offset: 0x000411CB
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in base.QuestLookTargets)
				{
					yield return globalTargetInfo;
				}
				IEnumerator<GlobalTargetInfo> enumerator = null;
				if (this.mapParent != null)
				{
					yield return this.mapParent;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x0600613D RID: 24893 RVA: 0x001E7374 File Offset: 0x001E5574
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal && (this.targetWorld || this.mapParent != null) && this.gameCondition != null)
			{
				this.gameCondition.quest = this.quest;
				if (this.targetWorld)
				{
					Find.World.gameConditionManager.RegisterCondition(this.gameCondition);
				}
				else
				{
					this.mapParent.Map.gameConditionManager.RegisterCondition(this.gameCondition);
				}
				if (this.sendStandardLetter)
				{
					Find.LetterStack.ReceiveLetter(this.gameCondition.LabelCap, this.gameCondition.LetterText, this.gameCondition.def.letterDef, LookTargets.Invalid, null, this.quest, null, null);
				}
				this.gameCondition = null;
			}
		}

		// Token: 0x0600613E RID: 24894 RVA: 0x001E7460 File Offset: 0x001E5660
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Deep.Look<GameCondition>(ref this.gameCondition, "gameCondition", Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.targetWorld, "targetWorld", false, false);
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_Values.Look<bool>(ref this.sendStandardLetter, "sendStandardLetter", true, false);
		}

		// Token: 0x0600613F RID: 24895 RVA: 0x001E74D0 File Offset: 0x001E56D0
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int;
			if (Find.AnyPlayerHomeMap != null)
			{
				this.gameCondition = GameConditionMaker.MakeCondition(GameConditionDefOf.ColdSnap, Rand.RangeInclusive(2500, 7500));
				this.mapParent = Find.RandomPlayerHomeMap.Parent;
			}
		}

		// Token: 0x040040FA RID: 16634
		public string inSignal;

		// Token: 0x040040FB RID: 16635
		public GameCondition gameCondition;

		// Token: 0x040040FC RID: 16636
		public bool targetWorld;

		// Token: 0x040040FD RID: 16637
		public MapParent mapParent;

		// Token: 0x040040FE RID: 16638
		public bool sendStandardLetter = true;
	}
}
