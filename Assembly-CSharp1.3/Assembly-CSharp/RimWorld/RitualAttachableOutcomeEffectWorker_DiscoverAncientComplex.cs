using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F07 RID: 3847
	public class RitualAttachableOutcomeEffectWorker_DiscoverAncientComplex : RitualAttachableOutcomeEffectWorker
	{
		// Token: 0x06005BCB RID: 23499 RVA: 0x001FBBD8 File Offset: 0x001F9DD8
		public override void Apply(Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual, OutcomeChance outcome, out string extraOutcomeDesc, ref LookTargets letterLookTargets)
		{
			using (List<WorldObject>.Enumerator enumerator = Find.WorldObjects.AllWorldObjects.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Site site;
					if ((site = (enumerator.Current as Site)) != null)
					{
						if (site.parts.Any((SitePart p) => p.def == SitePartDefOf.AncientComplex))
						{
							extraOutcomeDesc = null;
							return;
						}
					}
				}
			}
			foreach (Quest quest in Find.QuestManager.QuestsListForReading)
			{
				if (quest.root == QuestScriptDefOf.AncientComplex_Mission && !quest.Historical)
				{
					extraOutcomeDesc = null;
					return;
				}
			}
			float points = StorytellerUtility.DefaultThreatPointsNow(jobRitual.Map);
			if (!QuestScriptDefOf.OpportunitySite_AncientComplex.CanRun(points))
			{
				extraOutcomeDesc = null;
				return;
			}
			extraOutcomeDesc = this.def.letterInfoText;
			Quest quest2 = QuestUtility.GenerateQuestAndMakeAvailable(QuestScriptDefOf.OpportunitySite_AncientComplex, points);
			letterLookTargets = new LookTargets((letterLookTargets.targets ?? new List<GlobalTargetInfo>()).Concat(quest2.QuestLookTargets));
		}
	}
}
