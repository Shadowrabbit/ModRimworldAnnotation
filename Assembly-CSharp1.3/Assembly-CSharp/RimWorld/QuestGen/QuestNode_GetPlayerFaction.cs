using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001683 RID: 5763
	public class QuestNode_GetPlayerFaction : QuestNode
	{
		// Token: 0x0600861B RID: 34331 RVA: 0x00301DDC File Offset: 0x002FFFDC
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			slate.Set<Faction>(this.storeAs.GetValue(slate), Faction.OfPlayer, false);
		}

		// Token: 0x0600861C RID: 34332 RVA: 0x00301E07 File Offset: 0x00300007
		protected override bool TestRunInt(Slate slate)
		{
			slate.Set<Faction>(this.storeAs.GetValue(slate), Faction.OfPlayer, false);
			return true;
		}

		// Token: 0x040053F3 RID: 21491
		[NoTranslate]
		public SlateRef<string> storeAs;
	}
}
