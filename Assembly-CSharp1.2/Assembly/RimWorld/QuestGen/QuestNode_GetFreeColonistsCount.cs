using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001F18 RID: 7960
	public class QuestNode_GetFreeColonistsCount : QuestNode
	{
		// Token: 0x0600AA4E RID: 43598 RVA: 0x0006F9D5 File Offset: 0x0006DBD5
		protected override bool TestRunInt(Slate slate)
		{
			this.SetVars(slate);
			return true;
		}

		// Token: 0x0600AA4F RID: 43599 RVA: 0x0006F9DF File Offset: 0x0006DBDF
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		// Token: 0x0600AA50 RID: 43600 RVA: 0x0031B43C File Offset: 0x0031963C
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

		// Token: 0x040073AC RID: 29612
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x040073AD RID: 29613
		public SlateRef<Map> onlyThisMap;
	}
}
