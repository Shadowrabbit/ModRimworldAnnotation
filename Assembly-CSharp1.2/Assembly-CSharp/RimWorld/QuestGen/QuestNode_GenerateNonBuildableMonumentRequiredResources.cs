using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001F09 RID: 7945
	public class QuestNode_GenerateNonBuildableMonumentRequiredResources : QuestNode
	{
		// Token: 0x0600AA1C RID: 43548 RVA: 0x0000A2A7 File Offset: 0x000084A7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600AA1D RID: 43549 RVA: 0x0031A9FC File Offset: 0x00318BFC
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

		// Token: 0x04007366 RID: 29542
		[NoTranslate]
		public SlateRef<string> addToList;

		// Token: 0x04007367 RID: 29543
		public SlateRef<MonumentMarker> monumentMarker;
	}
}
