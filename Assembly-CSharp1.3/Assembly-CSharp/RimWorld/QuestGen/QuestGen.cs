using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x0200160C RID: 5644
	public static class QuestGen
	{
		// Token: 0x17001609 RID: 5641
		// (get) Token: 0x06008417 RID: 33815 RVA: 0x002F520B File Offset: 0x002F340B
		public static QuestScriptDef Root
		{
			get
			{
				return QuestGen.root;
			}
		}

		// Token: 0x1700160A RID: 5642
		// (get) Token: 0x06008418 RID: 33816 RVA: 0x002F5212 File Offset: 0x002F3412
		public static bool Working
		{
			get
			{
				return QuestGen.working;
			}
		}

		// Token: 0x1700160B RID: 5643
		// (get) Token: 0x06008419 RID: 33817 RVA: 0x002F5219 File Offset: 0x002F3419
		public static List<Rule> QuestDescriptionRulesReadOnly
		{
			get
			{
				return QuestGen.questDescriptionRules;
			}
		}

		// Token: 0x1700160C RID: 5644
		// (get) Token: 0x0600841A RID: 33818 RVA: 0x002F5220 File Offset: 0x002F3420
		public static Dictionary<string, string> QuestDescriptionConstantsReadOnly
		{
			get
			{
				return QuestGen.questDescriptionConstants;
			}
		}

		// Token: 0x1700160D RID: 5645
		// (get) Token: 0x0600841B RID: 33819 RVA: 0x002F5227 File Offset: 0x002F3427
		public static List<Rule> QuestNameRulesReadOnly
		{
			get
			{
				return QuestGen.questNameRules;
			}
		}

		// Token: 0x1700160E RID: 5646
		// (get) Token: 0x0600841C RID: 33820 RVA: 0x002F522E File Offset: 0x002F342E
		public static Dictionary<string, string> QuestNameConstantsReadOnly
		{
			get
			{
				return QuestGen.questNameConstants;
			}
		}

		// Token: 0x1700160F RID: 5647
		// (get) Token: 0x0600841D RID: 33821 RVA: 0x002F5235 File Offset: 0x002F3435
		public static List<QuestTextRequest> TextRequestsReadOnly
		{
			get
			{
				return QuestGen.textRequests;
			}
		}

		// Token: 0x17001610 RID: 5648
		// (get) Token: 0x0600841E RID: 33822 RVA: 0x002F523C File Offset: 0x002F343C
		public static List<Rule> QuestContentRulesReadOnly
		{
			get
			{
				return QuestGen.questContentRules;
			}
		}

		// Token: 0x0600841F RID: 33823 RVA: 0x002F5244 File Offset: 0x002F3444
		public static string GenerateNewSignal(string signalString, bool ensureUnique = true)
		{
			int num;
			if (!ensureUnique || !QuestGen.generatedSignals.TryGetValue(signalString, out num))
			{
				num = 0;
			}
			string result = string.Concat(new object[]
			{
				"Quest",
				QuestGen.quest.id,
				".",
				signalString,
				(num == 0) ? "" : (num + 1).ToString()
			});
			QuestGen.generatedSignals[signalString] = num + 1;
			return result;
		}

		// Token: 0x06008420 RID: 33824 RVA: 0x002F52BC File Offset: 0x002F34BC
		public static string GenerateNewTargetQuestTag(string targetString, bool ensureUnique = true)
		{
			int num;
			if (!ensureUnique || !QuestGen.generatedTargetQuestTags.TryGetValue(targetString, out num))
			{
				num = 0;
			}
			string result = string.Concat(new object[]
			{
				"Quest",
				QuestGen.quest.id,
				".",
				targetString,
				(num == 0) ? "" : (num + 1).ToString()
			});
			QuestGen.generatedTargetQuestTags[targetString] = num + 1;
			return result;
		}

		// Token: 0x06008421 RID: 33825 RVA: 0x002F5334 File Offset: 0x002F3534
		private static void ResetIdCounters()
		{
			QuestGen.generatedSignals.Clear();
			QuestGen.generatedTargetQuestTags.Clear();
		}

		// Token: 0x06008422 RID: 33826 RVA: 0x002F534C File Offset: 0x002F354C
		public static Quest Generate(QuestScriptDef root, Slate initialVars)
		{
			if (DeepProfiler.enabled)
			{
				DeepProfiler.Start("Generate quest");
			}
			Quest result = null;
			try
			{
				if (QuestGen.working)
				{
					throw new Exception("Called Generate() while already working.");
				}
				QuestGen.working = true;
				QuestGen.root = root;
				QuestGen.slate.Reset();
				QuestGen.slate.SetAll(initialVars);
				QuestGen.quest = Quest.MakeRaw();
				QuestGen.quest.ticksUntilAcceptanceExpiry = (int)(root.expireDaysRange.RandomInRange * 60000f);
				if (root.defaultChallengeRating > 0)
				{
					QuestGen.quest.challengeRating = root.defaultChallengeRating;
				}
				QuestGen.quest.root = root;
				QuestGen.quest.hidden = root.defaultHidden;
				QuestGen.quest.charity = root.defaultCharity;
				QuestGen.slate.SetIfNone<string>("inSignal", QuestGen.quest.InitiateSignal, false);
				root.Run();
				try
				{
					QuestNode_ResolveQuestName.Resolve();
				}
				catch (Exception arg)
				{
					Log.Error("Error while generating quest name: " + arg);
				}
				try
				{
					QuestNode_ResolveQuestDescription.Resolve();
				}
				catch (Exception arg2)
				{
					Log.Error("Error while generating quest description: " + arg2);
				}
				try
				{
					QuestNode_ResolveTextRequests.Resolve();
				}
				catch (Exception arg3)
				{
					Log.Error("Error while resolving text requests: " + arg3);
				}
				QuestGen.AddSlateQuestTags();
				bool flag = root.autoAccept;
				if (flag)
				{
					List<QuestPart> partsListForReading = QuestGen.quest.PartsListForReading;
					for (int i = 0; i < partsListForReading.Count; i++)
					{
						if (partsListForReading[i].PreventsAutoAccept)
						{
							flag = false;
							break;
						}
					}
				}
				if (flag)
				{
					QuestGen.quest.SetInitiallyAccepted();
				}
				result = QuestGen.quest;
			}
			catch (Exception arg4)
			{
				Log.Error("Error in QuestGen: " + arg4);
			}
			finally
			{
				if (DeepProfiler.enabled)
				{
					DeepProfiler.End();
				}
				QuestGen.quest = null;
				QuestGen.root = null;
				QuestGen.working = false;
				QuestGen.generatedPawns.Clear();
				QuestGen.textRequests.Clear();
				QuestGen.slate.Reset();
				QuestGen.questDescriptionRules.Clear();
				QuestGen.questDescriptionConstants.Clear();
				QuestGen.questNameRules.Clear();
				QuestGen.questNameConstants.Clear();
				QuestGen.questContentRules.Clear();
				QuestGen.slateQuestTagsToAddWhenFinished.Clear();
				QuestGen.ResetIdCounters();
			}
			return result;
		}

		// Token: 0x06008423 RID: 33827 RVA: 0x002F55F0 File Offset: 0x002F37F0
		public static void AddToGeneratedPawns(Pawn pawn)
		{
			if (!QuestGen.working)
			{
				Log.Error("Tried to add a pawn to generated pawns while not resolving any quest.");
				return;
			}
			if (!QuestGen.generatedPawns.Contains(pawn))
			{
				QuestGen.generatedPawns.Add(pawn);
			}
		}

		// Token: 0x06008424 RID: 33828 RVA: 0x002F561C File Offset: 0x002F381C
		public static bool WasGeneratedForQuestBeingGenerated(Pawn pawn)
		{
			return QuestGen.working && QuestGen.generatedPawns.Contains(pawn);
		}

		// Token: 0x06008425 RID: 33829 RVA: 0x002F5632 File Offset: 0x002F3832
		public static void AddQuestDescriptionRules(RulePack rulePack)
		{
			QuestGen.AddQuestDescriptionRules(rulePack.Rules);
		}

		// Token: 0x06008426 RID: 33830 RVA: 0x002F563F File Offset: 0x002F383F
		public static void AddQuestDescriptionRules(List<Rule> rules)
		{
			if (!QuestGen.working)
			{
				Log.Error("Tried to add quest description rules while not resolving any quest.");
				return;
			}
			QuestGen.questDescriptionRules.AddRange(QuestGenUtility.AppendCurrentPrefix(rules));
		}

		// Token: 0x06008427 RID: 33831 RVA: 0x002F5664 File Offset: 0x002F3864
		public static void AddQuestDescriptionConstants(Dictionary<string, string> constants)
		{
			if (!QuestGen.working)
			{
				Log.Error("Tried to add quest description constants while not resolving any quest.");
				return;
			}
			foreach (KeyValuePair<string, string> keyValuePair in QuestGenUtility.AppendCurrentPrefix(constants))
			{
				if (!QuestGen.questDescriptionConstants.ContainsKey(keyValuePair.Key))
				{
					QuestGen.questDescriptionConstants.Add(keyValuePair.Key, keyValuePair.Value);
				}
			}
		}

		// Token: 0x06008428 RID: 33832 RVA: 0x002F56F0 File Offset: 0x002F38F0
		public static void AddQuestNameRules(RulePack rulePack)
		{
			QuestGen.AddQuestNameRules(rulePack.Rules);
		}

		// Token: 0x06008429 RID: 33833 RVA: 0x002F56FD File Offset: 0x002F38FD
		public static void AddQuestNameRules(List<Rule> rules)
		{
			if (!QuestGen.working)
			{
				Log.Error("Tried to add quest name rules while not resolving any quest.");
				return;
			}
			QuestGen.questNameRules.AddRange(QuestGenUtility.AppendCurrentPrefix(rules));
		}

		// Token: 0x0600842A RID: 33834 RVA: 0x002F5724 File Offset: 0x002F3924
		public static void AddQuestNameConstants(Dictionary<string, string> constants)
		{
			if (!QuestGen.working)
			{
				Log.Error("Tried to add quest name constants while not resolving any quest.");
				return;
			}
			foreach (KeyValuePair<string, string> keyValuePair in QuestGenUtility.AppendCurrentPrefix(constants))
			{
				if (!QuestGen.questNameConstants.ContainsKey(keyValuePair.Key))
				{
					QuestGen.questNameConstants.Add(keyValuePair.Key, keyValuePair.Value);
				}
			}
		}

		// Token: 0x0600842B RID: 33835 RVA: 0x002F57B0 File Offset: 0x002F39B0
		public static void AddQuestContentRules(RulePack rulePack)
		{
			QuestGen.AddQuestContentRules(rulePack.Rules);
		}

		// Token: 0x0600842C RID: 33836 RVA: 0x002F57BD File Offset: 0x002F39BD
		public static void AddQuestContentRules(List<Rule> rules)
		{
			if (!QuestGen.working)
			{
				Log.Error("Tried to add quest content rules while not resolving any quest.");
				return;
			}
			QuestGen.questContentRules.AddRange(QuestGenUtility.AppendCurrentPrefix(rules));
		}

		// Token: 0x0600842D RID: 33837 RVA: 0x002F57E1 File Offset: 0x002F39E1
		public static void AddSlateQuestTagToAddWhenFinished(string slateVarNameWithPrefix)
		{
			if (!QuestGen.slateQuestTagsToAddWhenFinished.Contains(slateVarNameWithPrefix))
			{
				QuestGen.slateQuestTagsToAddWhenFinished.Add(slateVarNameWithPrefix);
			}
		}

		// Token: 0x0600842E RID: 33838 RVA: 0x002F57FB File Offset: 0x002F39FB
		public static void AddTextRequest(string localKeyword, Action<string> setter, RulePack extraLocalRules = null)
		{
			QuestGen.AddTextRequest(localKeyword, setter, (extraLocalRules != null) ? extraLocalRules.Rules : null);
		}

		// Token: 0x0600842F RID: 33839 RVA: 0x002F5810 File Offset: 0x002F3A10
		public static void AddTextRequest(string localKeyword, Action<string> setter, List<Rule> extraLocalRules)
		{
			if (!QuestGen.working)
			{
				Log.Error("Tried to add a text request while not resolving any quest.");
				return;
			}
			QuestTextRequest questTextRequest = new QuestTextRequest();
			questTextRequest.keyword = localKeyword;
			if (!QuestGen.slate.CurrentPrefix.NullOrEmpty())
			{
				questTextRequest.keyword = QuestGen.slate.CurrentPrefix + "/" + questTextRequest.keyword;
			}
			questTextRequest.keyword = QuestGenUtility.NormalizeVarPath(questTextRequest.keyword);
			questTextRequest.setter = setter;
			if (extraLocalRules != null)
			{
				questTextRequest.extraRules = QuestGenUtility.AppendCurrentPrefix(extraLocalRules);
			}
			QuestGen.textRequests.Add(questTextRequest);
		}

		// Token: 0x06008430 RID: 33840 RVA: 0x002F58A0 File Offset: 0x002F3AA0
		private static void AddSlateQuestTags()
		{
			for (int i = 0; i < QuestGen.slateQuestTagsToAddWhenFinished.Count; i++)
			{
				object obj;
				if (QuestGen.slate.TryGet<object>(QuestGen.slateQuestTagsToAddWhenFinished[i], out obj, true))
				{
					string questTagToAdd = QuestGen.GenerateNewTargetQuestTag(QuestGen.slateQuestTagsToAddWhenFinished[i], false);
					QuestUtility.AddQuestTag(obj, questTagToAdd);
				}
			}
			QuestGen.slateQuestTagsToAddWhenFinished.Clear();
		}

		// Token: 0x0400525A RID: 21082
		public static Quest quest;

		// Token: 0x0400525B RID: 21083
		public static Slate slate = new Slate();

		// Token: 0x0400525C RID: 21084
		private static QuestScriptDef root;

		// Token: 0x0400525D RID: 21085
		private static bool working;

		// Token: 0x0400525E RID: 21086
		private static List<QuestTextRequest> textRequests = new List<QuestTextRequest>();

		// Token: 0x0400525F RID: 21087
		private static List<Pawn> generatedPawns = new List<Pawn>();

		// Token: 0x04005260 RID: 21088
		private static Dictionary<string, int> generatedSignals = new Dictionary<string, int>();

		// Token: 0x04005261 RID: 21089
		private static Dictionary<string, int> generatedTargetQuestTags = new Dictionary<string, int>();

		// Token: 0x04005262 RID: 21090
		private static List<Rule> questDescriptionRules = new List<Rule>();

		// Token: 0x04005263 RID: 21091
		private static Dictionary<string, string> questDescriptionConstants = new Dictionary<string, string>();

		// Token: 0x04005264 RID: 21092
		private static List<Rule> questNameRules = new List<Rule>();

		// Token: 0x04005265 RID: 21093
		private static Dictionary<string, string> questNameConstants = new Dictionary<string, string>();

		// Token: 0x04005266 RID: 21094
		private static List<string> slateQuestTagsToAddWhenFinished = new List<string>();

		// Token: 0x04005267 RID: 21095
		private static List<Rule> questContentRules = new List<Rule>();
	}
}
