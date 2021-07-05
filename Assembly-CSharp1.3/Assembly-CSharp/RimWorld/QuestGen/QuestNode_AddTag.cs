using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001634 RID: 5684
	public class QuestNode_AddTag : QuestNode
	{
		// Token: 0x06008500 RID: 34048 RVA: 0x000126F5 File Offset: 0x000108F5
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x06008501 RID: 34049 RVA: 0x002FC3A4 File Offset: 0x002FA5A4
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.targets.GetValue(slate) == null)
			{
				return;
			}
			string questTagToAdd = QuestGenUtility.HardcodedTargetQuestTagWithQuestID(this.tag.GetValue(slate));
			foreach (object obj in this.targets.GetValue(slate))
			{
				Thing thing;
				WorldObject worldObject;
				TransportShip transportShip;
				if ((thing = (obj as Thing)) != null)
				{
					QuestUtility.AddQuestTag(ref thing.questTags, questTagToAdd);
				}
				else if ((worldObject = (obj as WorldObject)) != null)
				{
					QuestUtility.AddQuestTag(ref worldObject.questTags, questTagToAdd);
				}
				else if ((transportShip = (obj as TransportShip)) != null)
				{
					QuestUtility.AddQuestTag(ref transportShip.questTags, questTagToAdd);
				}
			}
		}

		// Token: 0x040052B8 RID: 21176
		[NoTranslate]
		public SlateRef<IEnumerable<object>> targets;

		// Token: 0x040052B9 RID: 21177
		[NoTranslate]
		public SlateRef<string> tag;
	}
}
