using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001FC5 RID: 8133
	public class QuestNode_SpawnSkyfaller : QuestNode
	{
		// Token: 0x0600AC9C RID: 44188 RVA: 0x0000A2A7 File Offset: 0x000084A7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600AC9D RID: 44189 RVA: 0x00322B34 File Offset: 0x00320D34
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			Map map = QuestGen.slate.Get<Map>("map", null, false);
			Skyfaller thing = SkyfallerMaker.MakeSkyfaller(this.skyfallerDef.GetValue(slate), this.innerThings.GetValue(slate));
			QuestPart_SpawnThing questPart_SpawnThing = new QuestPart_SpawnThing();
			questPart_SpawnThing.thing = thing;
			questPart_SpawnThing.mapParent = map.Parent;
			if (this.factionOfForSafeSpot.GetValue(slate) != null)
			{
				questPart_SpawnThing.factionForFindingSpot = this.factionOfForSafeSpot.GetValue(slate).Faction;
			}
			if (this.cell.GetValue(slate) != null)
			{
				questPart_SpawnThing.cell = this.cell.GetValue(slate).Value;
			}
			questPart_SpawnThing.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_SpawnThing.lookForSafeSpot = this.lookForSafeSpot.GetValue(slate);
			questPart_SpawnThing.tryLandInShipLandingZone = this.tryLandInShipLandingZone.GetValue(slate);
			QuestGen.quest.AddPart(questPart_SpawnThing);
		}

		// Token: 0x04007613 RID: 30227
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x04007614 RID: 30228
		public SlateRef<ThingDef> skyfallerDef;

		// Token: 0x04007615 RID: 30229
		public SlateRef<IEnumerable<Thing>> innerThings;

		// Token: 0x04007616 RID: 30230
		public SlateRef<IntVec3?> cell;

		// Token: 0x04007617 RID: 30231
		public SlateRef<Pawn> factionOfForSafeSpot;

		// Token: 0x04007618 RID: 30232
		public SlateRef<bool> lookForSafeSpot;

		// Token: 0x04007619 RID: 30233
		public SlateRef<bool> tryLandInShipLandingZone;
	}
}
