using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001653 RID: 5715
	public class QuestNode_GenerateWorldObject : QuestNode
	{
		// Token: 0x06008561 RID: 34145 RVA: 0x002FE368 File Offset: 0x002FC568
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

		// Token: 0x06008562 RID: 34146 RVA: 0x000126F5 File Offset: 0x000108F5
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x04005341 RID: 21313
		public SlateRef<WorldObjectDef> def;

		// Token: 0x04005342 RID: 21314
		public SlateRef<int> tile;

		// Token: 0x04005343 RID: 21315
		public SlateRef<Faction> faction;

		// Token: 0x04005344 RID: 21316
		[NoTranslate]
		public SlateRef<string> storeAs;
	}
}
