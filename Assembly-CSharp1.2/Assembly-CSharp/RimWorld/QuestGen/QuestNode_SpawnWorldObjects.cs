using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001FC6 RID: 8134
	public class QuestNode_SpawnWorldObjects : QuestNode
	{
		// Token: 0x0600AC9F RID: 44191 RVA: 0x0000A2A7 File Offset: 0x000084A7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600ACA0 RID: 44192 RVA: 0x00322C44 File Offset: 0x00320E44
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

		// Token: 0x0400761A RID: 30234
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x0400761B RID: 30235
		public SlateRef<IEnumerable<WorldObject>> worldObjects;

		// Token: 0x0400761C RID: 30236
		public SlateRef<int?> tile;

		// Token: 0x0400761D RID: 30237
		public SlateRef<List<ThingDef>> defsToExcludeFromHyperlinks;
	}
}
