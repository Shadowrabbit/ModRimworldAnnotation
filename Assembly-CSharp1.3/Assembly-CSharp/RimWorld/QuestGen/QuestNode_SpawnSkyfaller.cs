using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020016EA RID: 5866
	public class QuestNode_SpawnSkyfaller : QuestNode
	{
		// Token: 0x0600877F RID: 34687 RVA: 0x000126F5 File Offset: 0x000108F5
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x06008780 RID: 34688 RVA: 0x003077FC File Offset: 0x003059FC
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

		// Token: 0x04005596 RID: 21910
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x04005597 RID: 21911
		public SlateRef<ThingDef> skyfallerDef;

		// Token: 0x04005598 RID: 21912
		public SlateRef<IEnumerable<Thing>> innerThings;

		// Token: 0x04005599 RID: 21913
		public SlateRef<IntVec3?> cell;

		// Token: 0x0400559A RID: 21914
		public SlateRef<Pawn> factionOfForSafeSpot;

		// Token: 0x0400559B RID: 21915
		public SlateRef<bool> lookForSafeSpot;

		// Token: 0x0400559C RID: 21916
		public SlateRef<bool> tryLandInShipLandingZone;
	}
}
