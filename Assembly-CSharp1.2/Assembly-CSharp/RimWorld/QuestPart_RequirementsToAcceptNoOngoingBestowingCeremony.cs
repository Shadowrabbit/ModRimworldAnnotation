using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001103 RID: 4355
	public class QuestPart_RequirementsToAcceptNoOngoingBestowingCeremony : QuestPart_RequirementsToAccept
	{
		// Token: 0x06005F2C RID: 24364 RVA: 0x001E1CEC File Offset: 0x001DFEEC
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
