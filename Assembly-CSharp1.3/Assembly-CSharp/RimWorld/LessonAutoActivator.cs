using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020013BD RID: 5053
	public static class LessonAutoActivator
	{
		// Token: 0x17001580 RID: 5504
		// (get) Token: 0x06007ADE RID: 31454 RVA: 0x002B694A File Offset: 0x002B4B4A
		private static float SecondsSinceLesson
		{
			get
			{
				return LessonAutoActivator.timeSinceLastLesson;
			}
		}

		// Token: 0x17001581 RID: 5505
		// (get) Token: 0x06007ADF RID: 31455 RVA: 0x002B6951 File Offset: 0x002B4B51
		private static float RelaxDesire
		{
			get
			{
				return 100f - LessonAutoActivator.SecondsSinceLesson * 0.11111111f;
			}
		}

		// Token: 0x06007AE0 RID: 31456 RVA: 0x002B6964 File Offset: 0x002B4B64
		public static void Reset()
		{
			LessonAutoActivator.alertingConcepts.Clear();
		}

		// Token: 0x06007AE1 RID: 31457 RVA: 0x002B6970 File Offset: 0x002B4B70
		public static void TeachOpportunity(ConceptDef conc, OpportunityType opp)
		{
			LessonAutoActivator.TeachOpportunity(conc, null, opp);
		}

		// Token: 0x06007AE2 RID: 31458 RVA: 0x002B697C File Offset: 0x002B4B7C
		public static void TeachOpportunity(ConceptDef conc, Thing subject, OpportunityType opp)
		{
			if (!TutorSystem.AdaptiveTrainingEnabled || PlayerKnowledgeDatabase.IsComplete(conc))
			{
				return;
			}
			float value = 999f;
			switch (opp)
			{
			case OpportunityType.GoodToKnow:
				value = 60f;
				break;
			case OpportunityType.Important:
				value = 80f;
				break;
			case OpportunityType.Critical:
				value = 100f;
				break;
			default:
				Log.Error("Unknown need");
				break;
			}
			LessonAutoActivator.opportunities[conc] = value;
			if (opp >= OpportunityType.Important || Find.Tutor.learningReadout.ActiveConceptsCount < 4)
			{
				LessonAutoActivator.TryInitiateLesson(conc);
			}
		}

		// Token: 0x06007AE3 RID: 31459 RVA: 0x002B69FD File Offset: 0x002B4BFD
		public static void Notify_KnowledgeDemonstrated(ConceptDef conc)
		{
			if (PlayerKnowledgeDatabase.IsComplete(conc))
			{
				LessonAutoActivator.opportunities[conc] = 0f;
			}
		}

		// Token: 0x06007AE4 RID: 31460 RVA: 0x002B6A18 File Offset: 0x002B4C18
		public static void LessonAutoActivatorUpdate()
		{
			if (!TutorSystem.AdaptiveTrainingEnabled || Current.Game == null || Find.Tutor.learningReadout.ShowAllMode)
			{
				return;
			}
			LessonAutoActivator.timeSinceLastLesson += RealTime.realDeltaTime;
			if (Current.ProgramState == ProgramState.Playing && (Time.timeSinceLevelLoad < 8f || Find.WindowStack.SecondsSinceClosedGameStartDialog < 8f || Find.TickManager.NotPlaying))
			{
				return;
			}
			for (int i = LessonAutoActivator.alertingConcepts.Count - 1; i >= 0; i--)
			{
				if (PlayerKnowledgeDatabase.IsComplete(LessonAutoActivator.alertingConcepts[i]))
				{
					LessonAutoActivator.alertingConcepts.RemoveAt(i);
				}
			}
			if (Time.frameCount % 15 == 0 && Find.ActiveLesson.Current == null)
			{
				for (int j = 0; j < DefDatabase<ConceptDef>.AllDefsListForReading.Count; j++)
				{
					ConceptDef conceptDef = DefDatabase<ConceptDef>.AllDefsListForReading[j];
					if (!PlayerKnowledgeDatabase.IsComplete(conceptDef))
					{
						float num = PlayerKnowledgeDatabase.GetKnowledge(conceptDef);
						num -= 0.00015f * Time.deltaTime * 15f;
						if (num < 0f)
						{
							num = 0f;
						}
						PlayerKnowledgeDatabase.SetKnowledge(conceptDef, num);
						if (conceptDef.opportunityDecays)
						{
							float num2 = LessonAutoActivator.GetOpportunity(conceptDef);
							num2 -= 0.4f * Time.deltaTime * 15f;
							if (num2 < 0f)
							{
								num2 = 0f;
							}
							LessonAutoActivator.opportunities[conceptDef] = num2;
						}
					}
				}
				if (Find.Tutor.learningReadout.ActiveConceptsCount < 3)
				{
					ConceptDef conceptDef2 = LessonAutoActivator.MostDesiredConcept();
					if (conceptDef2 != null)
					{
						float desire = LessonAutoActivator.GetDesire(conceptDef2);
						if (desire > 0.1f && LessonAutoActivator.RelaxDesire < desire)
						{
							LessonAutoActivator.TryInitiateLesson(conceptDef2);
							return;
						}
					}
				}
				else
				{
					LessonAutoActivator.SetLastLessonTimeToNow();
				}
			}
		}

		// Token: 0x06007AE5 RID: 31461 RVA: 0x002B6BC0 File Offset: 0x002B4DC0
		private static ConceptDef MostDesiredConcept()
		{
			float num = -9999f;
			ConceptDef result = null;
			List<ConceptDef> allDefsListForReading = DefDatabase<ConceptDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				ConceptDef conceptDef = allDefsListForReading[i];
				float desire = LessonAutoActivator.GetDesire(conceptDef);
				if (desire > num && (!conceptDef.needsOpportunity || LessonAutoActivator.GetOpportunity(conceptDef) >= 0.1f) && PlayerKnowledgeDatabase.GetKnowledge(conceptDef) <= 0.15f)
				{
					num = desire;
					result = conceptDef;
				}
			}
			return result;
		}

		// Token: 0x06007AE6 RID: 31462 RVA: 0x002B6C30 File Offset: 0x002B4E30
		private static float GetDesire(ConceptDef conc)
		{
			if (PlayerKnowledgeDatabase.IsComplete(conc))
			{
				return 0f;
			}
			if (Find.Tutor.learningReadout.IsActive(conc))
			{
				return 0f;
			}
			if (Current.ProgramState != conc.gameMode)
			{
				return 0f;
			}
			if (conc.needsOpportunity && LessonAutoActivator.GetOpportunity(conc) < 0.1f)
			{
				return 0f;
			}
			return (0f + conc.priority + LessonAutoActivator.GetOpportunity(conc) / 100f * 60f) * (1f - PlayerKnowledgeDatabase.GetKnowledge(conc));
		}

		// Token: 0x06007AE7 RID: 31463 RVA: 0x002B6CC0 File Offset: 0x002B4EC0
		private static float GetOpportunity(ConceptDef conc)
		{
			float result;
			if (LessonAutoActivator.opportunities.TryGetValue(conc, out result))
			{
				return result;
			}
			LessonAutoActivator.opportunities[conc] = 0f;
			return 0f;
		}

		// Token: 0x06007AE8 RID: 31464 RVA: 0x002B6CF3 File Offset: 0x002B4EF3
		private static void TryInitiateLesson(ConceptDef conc)
		{
			if (Find.Tutor.learningReadout.TryActivateConcept(conc))
			{
				LessonAutoActivator.SetLastLessonTimeToNow();
			}
		}

		// Token: 0x06007AE9 RID: 31465 RVA: 0x002B6D0C File Offset: 0x002B4F0C
		private static void SetLastLessonTimeToNow()
		{
			LessonAutoActivator.timeSinceLastLesson = 0f;
		}

		// Token: 0x06007AEA RID: 31466 RVA: 0x002B6D18 File Offset: 0x002B4F18
		public static void Notify_TutorialEnding()
		{
			LessonAutoActivator.SetLastLessonTimeToNow();
		}

		// Token: 0x06007AEB RID: 31467 RVA: 0x002B6D20 File Offset: 0x002B4F20
		public static string DebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("RelaxDesire: " + LessonAutoActivator.RelaxDesire);
			foreach (ConceptDef conceptDef in from co in DefDatabase<ConceptDef>.AllDefs
			orderby LessonAutoActivator.GetDesire(co) descending
			select co)
			{
				if (PlayerKnowledgeDatabase.IsComplete(conceptDef))
				{
					stringBuilder.AppendLine(conceptDef.defName + " complete");
				}
				else
				{
					stringBuilder.AppendLine(string.Concat(new string[]
					{
						conceptDef.defName,
						"\n   know ",
						PlayerKnowledgeDatabase.GetKnowledge(conceptDef).ToString("F3"),
						"\n   need ",
						LessonAutoActivator.opportunities[conceptDef].ToString("F3"),
						"\n   des ",
						LessonAutoActivator.GetDesire(conceptDef).ToString("F3")
					}));
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06007AEC RID: 31468 RVA: 0x002B6E54 File Offset: 0x002B5054
		public static void DebugForceInitiateBestLessonNow()
		{
			LessonAutoActivator.TryInitiateLesson((from def in DefDatabase<ConceptDef>.AllDefs
			orderby LessonAutoActivator.GetDesire(def) descending
			select def).First<ConceptDef>());
		}

		// Token: 0x04004432 RID: 17458
		private static Dictionary<ConceptDef, float> opportunities = new Dictionary<ConceptDef, float>();

		// Token: 0x04004433 RID: 17459
		private static float timeSinceLastLesson = 10000f;

		// Token: 0x04004434 RID: 17460
		private static List<ConceptDef> alertingConcepts = new List<ConceptDef>();

		// Token: 0x04004435 RID: 17461
		private const float MapStartGracePeriod = 8f;

		// Token: 0x04004436 RID: 17462
		private const float KnowledgeDecayRate = 0.00015f;

		// Token: 0x04004437 RID: 17463
		private const float OpportunityDecayRate = 0.4f;

		// Token: 0x04004438 RID: 17464
		private const float OpportunityMaxDesireAdd = 60f;

		// Token: 0x04004439 RID: 17465
		private const int CheckInterval = 15;

		// Token: 0x0400443A RID: 17466
		private const float MaxLessonInterval = 900f;
	}
}
