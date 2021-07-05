using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x020016E9 RID: 5865
	public class QuestNode_SpawnMechCluster : QuestNode
	{
		// Token: 0x0600877B RID: 34683 RVA: 0x00307588 File Offset: 0x00305788
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_MechCluster questPart_MechCluster = new QuestPart_MechCluster();
			questPart_MechCluster.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_MechCluster.tag = QuestGenUtility.HardcodedTargetQuestTagWithQuestID(this.tag.GetValue(slate));
			questPart_MechCluster.mapParent = slate.Get<Map>("map", null, false).Parent;
			questPart_MechCluster.sketch = this.GenerateSketch(slate);
			questPart_MechCluster.dropSpot = (this.dropSpot.GetValue(slate) ?? IntVec3.Invalid);
			QuestGen.quest.AddPart(questPart_MechCluster);
			string text = "";
			if (questPart_MechCluster.sketch.pawns != null)
			{
				text += PawnUtility.PawnKindsToLineList(from m in questPart_MechCluster.sketch.pawns
				select m.kindDef, "  - ", ColoredText.ThreatColor);
			}
			string[] array = (from t in questPart_MechCluster.sketch.buildingsSketch.Things
			where GenHostility.IsDefMechClusterThreat(t.def)
			group t by t.def.label).Select(delegate(IGrouping<string, SketchThing> grp)
			{
				int num = grp.Count<SketchThing>();
				return num + " " + ((num > 1) ? Find.ActiveLanguageWorker.Pluralize(grp.Key, num) : grp.Key);
			}).ToArray<string>();
			if (array.Any<string>())
			{
				if (text != "")
				{
					text += "\n";
				}
				text += array.ToLineList(ColoredText.ThreatColor, "  - ");
			}
			if (text != "")
			{
				QuestGen.AddQuestDescriptionRules(new List<Rule>
				{
					new Rule_String("allThreats", text)
				});
			}
		}

		// Token: 0x0600877C RID: 34684 RVA: 0x0030777C File Offset: 0x0030597C
		private MechClusterSketch GenerateSketch(Slate slate)
		{
			return MechClusterGenerator.GenerateClusterSketch(this.points.GetValue(slate) ?? slate.Get<float>("points", 0f, false), slate.Get<Map>("map", null, false), true, false);
		}

		// Token: 0x0600877D RID: 34685 RVA: 0x003077CD File Offset: 0x003059CD
		protected override bool TestRunInt(Slate slate)
		{
			return Find.Storyteller.difficulty.allowViolentQuests && Faction.OfMechanoids != null && slate.Get<Map>("map", null, false) != null;
		}

		// Token: 0x04005592 RID: 21906
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x04005593 RID: 21907
		[NoTranslate]
		public SlateRef<string> tag;

		// Token: 0x04005594 RID: 21908
		public SlateRef<float?> points;

		// Token: 0x04005595 RID: 21909
		public SlateRef<IntVec3?> dropSpot;
	}
}
