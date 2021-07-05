using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001F08 RID: 7944
	public class QuestNode_GenerateMonumentMarker : QuestNode
	{
		// Token: 0x0600AA19 RID: 43545 RVA: 0x0000A2A7 File Offset: 0x000084A7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600AA1A RID: 43546 RVA: 0x0031A9B0 File Offset: 0x00318BB0
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			MonumentMarker monumentMarker = (MonumentMarker)ThingMaker.MakeThing(ThingDefOf.MonumentMarker, null);
			monumentMarker.sketch = this.sketch.GetValue(slate);
			slate.Set<MonumentMarker>(this.storeAs.GetValue(slate), monumentMarker, false);
		}

		// Token: 0x04007364 RID: 29540
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x04007365 RID: 29541
		public SlateRef<Sketch> sketch;
	}
}
