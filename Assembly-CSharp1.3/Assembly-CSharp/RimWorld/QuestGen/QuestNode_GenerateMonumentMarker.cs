using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200164C RID: 5708
	public class QuestNode_GenerateMonumentMarker : QuestNode
	{
		// Token: 0x0600854C RID: 34124 RVA: 0x000126F5 File Offset: 0x000108F5
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600854D RID: 34125 RVA: 0x002FDCA0 File Offset: 0x002FBEA0
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			MonumentMarker monumentMarker = (MonumentMarker)ThingMaker.MakeThing(ThingDefOf.MonumentMarker, null);
			monumentMarker.sketch = this.sketch.GetValue(slate);
			slate.Set<MonumentMarker>(this.storeAs.GetValue(slate), monumentMarker, false);
		}

		// Token: 0x04005312 RID: 21266
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x04005313 RID: 21267
		public SlateRef<Sketch> sketch;
	}
}
