using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200164D RID: 5709
	public class QuestNode_GenerateNonBuildableMonumentRequiredResources : QuestNode
	{
		// Token: 0x0600854F RID: 34127 RVA: 0x000126F5 File Offset: 0x000108F5
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x06008550 RID: 34128 RVA: 0x002FDCEC File Offset: 0x002FBEEC
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			MonumentMarker value = this.monumentMarker.GetValue(slate);
			for (int i = 0; i < value.sketch.Things.Count; i++)
			{
				ThingDef def = value.sketch.Things[i].def;
				ThingDef stuff = value.sketch.Things[i].stuff;
				if (def.category == ThingCategory.Building && !def.BuildableByPlayer)
				{
					MinifiedThing obj = ThingMaker.MakeThing(def, stuff).MakeMinified();
					QuestGenUtility.AddToOrMakeList(QuestGen.slate, this.addToList.GetValue(slate), obj);
				}
			}
		}

		// Token: 0x04005314 RID: 21268
		[NoTranslate]
		public SlateRef<string> addToList;

		// Token: 0x04005315 RID: 21269
		public SlateRef<MonumentMarker> monumentMarker;
	}
}
