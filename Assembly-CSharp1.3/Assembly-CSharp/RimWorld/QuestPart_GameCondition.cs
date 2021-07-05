using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BC8 RID: 3016
	public class QuestPart_GameCondition : QuestPart
	{
		// Token: 0x17000C64 RID: 3172
		// (get) Token: 0x0600469C RID: 18076 RVA: 0x0017580C File Offset: 0x00173A0C
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

		// Token: 0x0600469D RID: 18077 RVA: 0x0017581C File Offset: 0x00173A1C
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

		// Token: 0x0600469E RID: 18078 RVA: 0x00175908 File Offset: 0x00173B08
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Deep.Look<GameCondition>(ref this.gameCondition, "gameCondition", Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.targetWorld, "targetWorld", false, false);
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_Values.Look<bool>(ref this.sendStandardLetter, "sendStandardLetter", true, false);
		}

		// Token: 0x0600469F RID: 18079 RVA: 0x00175978 File Offset: 0x00173B78
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

		// Token: 0x04002B13 RID: 11027
		public string inSignal;

		// Token: 0x04002B14 RID: 11028
		public GameCondition gameCondition;

		// Token: 0x04002B15 RID: 11029
		public bool targetWorld;

		// Token: 0x04002B16 RID: 11030
		public MapParent mapParent;

		// Token: 0x04002B17 RID: 11031
		public bool sendStandardLetter = true;
	}
}
