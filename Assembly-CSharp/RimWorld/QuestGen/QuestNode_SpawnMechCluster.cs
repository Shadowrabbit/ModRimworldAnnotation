using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x02001FC3 RID: 8131
	public class QuestNode_SpawnMechCluster : QuestNode
	{
		// Token: 0x0600AC92 RID: 44178 RVA: 0x003228D4 File Offset: 0x00320AD4
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_MechCluster questPart_MechCluster = new QuestPart_MechCluster();
			questPart_MechCluster.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_MechCluster.tag = QuestGenUtility.HardcodedTargetQuestTagWithQuestID(this.tag.GetValue(slate));
			questPart_MechCluster.mapParent = slate.Get<Map>("map", null, false).Parent;
			questPart_MechCluster.sketch = this.GenerateSketch(slate);
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

		// Token: 0x0600AC93 RID: 44179 RVA: 0x00322A9C File Offset: 0x00320C9C
		private MechClusterSketch GenerateSketch(Slate slate)
		{
			return MechClusterGenerator.GenerateClusterSketch_NewTemp(this.points.GetValue(slate) ?? slate.Get<float>("points", 0f, false), slate.Get<Map>("map", null, false), true, false);
		}

		// Token: 0x0600AC94 RID: 44180 RVA: 0x00070A43 File Offset: 0x0006EC43
		protected override bool TestRunInt(Slate slate)
		{
			return Find.Storyteller.difficultyValues.allowViolentQuests && slate.Get<Map>("map", null, false) != null;
		}

		// Token: 0x0400760B RID: 30219
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x0400760C RID: 30220
		[NoTranslate]
		public SlateRef<string> tag;

		// Token: 0x0400760D RID: 30221
		public SlateRef<float?> points;
	}
}
