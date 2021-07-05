using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B9C RID: 2972
	public class QuestPart_RequirementsToAcceptNoOngoingBestowingCeremony : QuestPart_RequirementsToAccept
	{
		// Token: 0x06004571 RID: 17777 RVA: 0x00170588 File Offset: 0x0016E788
		public override AcceptanceReport CanAccept()
		{
			if ((from q in Find.QuestManager.QuestsListForReading
			where q.State == QuestState.Ongoing && q.root == QuestScriptDefOf.BestowingCeremony
			select q).Any<Quest>())
			{
				return new AcceptanceReport("QuestCanNotStartUntilBestowingCeremonyFinished".Translate());
			}
			return true;
		}
	}
}
