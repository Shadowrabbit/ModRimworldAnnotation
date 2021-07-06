using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001F0E RID: 7950
	public class QuestNode_GenerateWorldObject : QuestNode
	{
		// Token: 0x0600AA2B RID: 43563 RVA: 0x0031AFF0 File Offset: 0x003191F0
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			WorldObject worldObject = WorldObjectMaker.MakeWorldObject(this.def.GetValue(slate));
			worldObject.Tile = this.tile.GetValue(slate);
			if (this.faction.GetValue(slate) != null)
			{
				worldObject.SetFaction(this.faction.GetValue(slate));
			}
			if (this.storeAs.GetValue(slate) != null)
			{
				QuestGen.slate.Set<WorldObject>(this.storeAs.GetValue(slate), worldObject, false);
			}
		}

		// Token: 0x0600AA2C RID: 43564 RVA: 0x0000A2A7 File Offset: 0x000084A7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x04007391 RID: 29585
		public SlateRef<WorldObjectDef> def;

		// Token: 0x04007392 RID: 29586
		public SlateRef<int> tile;

		// Token: 0x04007393 RID: 29587
		public SlateRef<Faction> faction;

		// Token: 0x04007394 RID: 29588
		[NoTranslate]
		public SlateRef<string> storeAs;
	}
}
