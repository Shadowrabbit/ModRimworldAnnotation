using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001676 RID: 5750
	public class QuestNode_GetMapOf : QuestNode
	{
		// Token: 0x060085E0 RID: 34272 RVA: 0x002FFFE1 File Offset: 0x002FE1E1
		protected override bool TestRunInt(Slate slate)
		{
			this.DoWork(slate);
			return true;
		}

		// Token: 0x060085E1 RID: 34273 RVA: 0x002FFFEB File Offset: 0x002FE1EB
		protected override void RunInt()
		{
			this.DoWork(QuestGen.slate);
		}

		// Token: 0x060085E2 RID: 34274 RVA: 0x002FFFF8 File Offset: 0x002FE1F8
		private void DoWork(Slate slate)
		{
			if (this.mapOf.GetValue(slate) != null)
			{
				slate.Set<Map>(this.storeAs.GetValue(slate), this.mapOf.GetValue(slate).MapHeld, false);
			}
		}

		// Token: 0x040053AE RID: 21422
		[NoTranslate]
		public SlateRef<string> storeAs = "map";

		// Token: 0x040053AF RID: 21423
		public SlateRef<Thing> mapOf;
	}
}
