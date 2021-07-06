using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001EF0 RID: 7920
	public class QuestNode_AddTag : QuestNode
	{
		// Token: 0x0600A9C9 RID: 43465 RVA: 0x0000A2A7 File Offset: 0x000084A7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600A9CA RID: 43466 RVA: 0x003194E4 File Offset: 0x003176E4
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
				Thing thing = obj as Thing;
				if (thing != null)
				{
					QuestUtility.AddQuestTag(ref thing.questTags, questTagToAdd);
				}
				else
				{
					WorldObject worldObject = obj as WorldObject;
					if (worldObject != null)
					{
						QuestUtility.AddQuestTag(ref worldObject.questTags, questTagToAdd);
					}
				}
			}
		}

		// Token: 0x0400730F RID: 29455
		[NoTranslate]
		public SlateRef<IEnumerable<object>> targets;

		// Token: 0x04007310 RID: 29456
		[NoTranslate]
		public SlateRef<string> tag;
	}
}
