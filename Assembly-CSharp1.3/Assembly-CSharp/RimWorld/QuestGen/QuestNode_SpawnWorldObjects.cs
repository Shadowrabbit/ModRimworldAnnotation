using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020016EB RID: 5867
	public class QuestNode_SpawnWorldObjects : QuestNode
	{
		// Token: 0x06008782 RID: 34690 RVA: 0x000126F5 File Offset: 0x000108F5
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x06008783 RID: 34691 RVA: 0x0030790C File Offset: 0x00305B0C
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.worldObjects.GetValue(slate) == null)
			{
				return;
			}
			string text = QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false);
			foreach (WorldObject worldObject in this.worldObjects.GetValue(slate))
			{
				QuestPart_SpawnWorldObject questPart_SpawnWorldObject = new QuestPart_SpawnWorldObject();
				questPart_SpawnWorldObject.worldObject = worldObject;
				questPart_SpawnWorldObject.inSignal = text;
				questPart_SpawnWorldObject.defsToExcludeFromHyperlinks = this.defsToExcludeFromHyperlinks.GetValue(slate);
				if (this.tile.GetValue(slate) != null)
				{
					worldObject.Tile = this.tile.GetValue(slate).Value;
				}
				QuestGen.quest.AddPart(questPart_SpawnWorldObject);
			}
		}

		// Token: 0x0400559D RID: 21917
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x0400559E RID: 21918
		public SlateRef<IEnumerable<WorldObject>> worldObjects;

		// Token: 0x0400559F RID: 21919
		public SlateRef<int?> tile;

		// Token: 0x040055A0 RID: 21920
		public SlateRef<List<ThingDef>> defsToExcludeFromHyperlinks;
	}
}
