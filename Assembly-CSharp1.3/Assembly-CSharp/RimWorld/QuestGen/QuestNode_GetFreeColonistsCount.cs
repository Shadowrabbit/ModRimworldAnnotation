using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200166B RID: 5739
	public class QuestNode_GetFreeColonistsCount : QuestNode
	{
		// Token: 0x060085B0 RID: 34224 RVA: 0x002FF3F4 File Offset: 0x002FD5F4
		protected override bool TestRunInt(Slate slate)
		{
			this.SetVars(slate);
			return true;
		}

		// Token: 0x060085B1 RID: 34225 RVA: 0x002FF3FE File Offset: 0x002FD5FE
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		// Token: 0x060085B2 RID: 34226 RVA: 0x002FF40C File Offset: 0x002FD60C
		private void SetVars(Slate slate)
		{
			int var;
			if (this.onlyThisMap.GetValue(slate) != null)
			{
				var = this.onlyThisMap.GetValue(slate).mapPawns.FreeColonistsCount;
			}
			else
			{
				var = PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonists.Count;
			}
			slate.Set<int>(this.storeAs.GetValue(slate), var, false);
		}

		// Token: 0x04005382 RID: 21378
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x04005383 RID: 21379
		public SlateRef<Map> onlyThisMap;
	}
}
