using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x02001EB6 RID: 7862
	public static class QuestGen
	{
		// Token: 0x17001972 RID: 6514
		// (get) Token: 0x0600A8C2 RID: 43202 RVA: 0x0006F0CF File Offset: 0x0006D2CF
		public static QuestScriptDef Root
		{
			get
			{
				return QuestGen.root;
			}
		}

		// Token: 0x17001973 RID: 6515
		// (get) Token: 0x0600A8C3 RID: 43203 RVA: 0x0006F0D6 File Offset: 0x0006D2D6
		public static bool Working
		{
			get
			{
				return QuestGen.working;
			}
		}

		// Token: 0x17001974 RID: 6516
		// (get) Token: 0x0600A8C4 RID: 43204 RVA: 0x0006F0DD File Offset: 0x0006D2DD
		public static List<Rule> QuestDescriptionRulesReadOnly
		{
			get
			{
				return QuestGen.questDescriptionRules;
			}
		}

		// Token: 0x17001975 RID: 6517
		// (get) Token: 0x0600A8C5 RID: 43205 RVA: 0x0006F0E4 File Offset: 0x0006D2E4
		public static Dictionary<string, string> QuestDescriptionConstantsReadOnly
		{
			get
			{
				return QuestGen.questDescriptionConstants;
			}
		}

		// Token: 0x17001976 RID: 6518
		// (get) Token: 0x0600A8C6 RID: 43206 RVA: 0x0006F0EB File Offset: 0x0006D2EB
		public static List<Rule> QuestNameRulesReadOnly
		{
			get
			{
				return QuestGen.questNameRules;
			}
		}

		// Token: 0x17001977 RID: 6519
		// (get) Token: 0x0600A8C7 RID: 43207 RVA: 0x0006F0F2 File Offset: 0x0006D2F2
		public static Dictionary<string, string> QuestNameConstantsReadOnly
		{
			get
			{
				return QuestGen.questNameConstants;
			}
		}

		// Token: 0x17001978 RID: 6520
		// (get) Token: 0x0600A8C8 RID: 43208 RVA: 0x0006F0F9 File Offset: 0x0006D2F9
		public static List<QuestTextRequest> TextRequestsReadOnly
		{
			get
			{
				return QuestGen.textRequests;
			}
		}

		// Token: 0x17001979 RID: 6521
		// (get) Token: 0x0600A8C9 RID: 43209 RVA: 0x0006F100 File Offset: 0x0006D300
		public static List<Rule> QuestContentRulesReadOnly
		{
			get
			{
				return QuestGen.questContentRules;
			}
		}

		// Token: 0x0600A8CA RID: 43210 RVA: 0x00313460 File Offset: 0x00311660
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

		// Token: 0x0600A8CB RID: 43211 RVA: 0x003134D8 File Offset: 0x003116D8
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

		// Token: 0x0600A8CC RID: 43212 RVA: 0x0006F107 File Offset: 0x0006D307
		private static void ResetIdCounters()
		{
			QuestGen.generatedSignals.Clear();
			QuestGen.generatedTargetQuestTags.Clear();
		}

		// Token: 0x0600A8CD RID: 43213 RVA: 0x00313550 File Offset: 0x00311750
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
				QuestGen.slate.SetIfNone<string>("inSignal", QuestGen.quest.InitiateSignal, false);
				root.Run();
				try
				{
					QuestNode_ResolveQuestName.Resolve();
				}
				catch (Exception arg)
				{
					Log.Error("Error while generating quest name: " + arg, false);
				}
				try
				{
					QuestNode_ResolveQuestDescription.Resolve();
				}
				catch (Exception arg2)
				{
					Log.Error("Error while generating quest description: " + arg2, false);
				}
				try
				{
					QuestNode_ResolveTextRequests.Resolve();
				}
				catch (Exception arg3)
				{
					Log.Error("Error while resolving text requests: " + arg3, false);
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
				Log.Error("Error in QuestGen: " + arg4, false);
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

		// Token: 0x0600A8CE RID: 43214 RVA: 0x0006F11D File Offset: 0x0006D31D
		public static void AddToGeneratedPawns(Pawn pawn)
		{
			if (!QuestGen.working)
			{
				Log.Error("Tried to add a pawn to generated pawns while not resolving any quest.", false);
				return;
			}
			if (!QuestGen.generatedPawns.Contains(pawn))
			{
				QuestGen.generatedPawns.Add(pawn);
			}
		}

		// Token: 0x0600A8CF RID: 43215 RVA: 0x0006F14A File Offset: 0x0006D34A
		public static bool WasGeneratedForQuestBeingGenerated(Pawn pawn)
		{
			return QuestGen.working && QuestGen.generatedPawns.Contains(pawn);
		}

		// Token: 0x0600A8D0 RID: 43216 RVA: 0x0006F160 File Offset: 0x0006D360
		public static void AddQuestDescriptionRules(RulePack rulePack)
		{
			QuestGen.AddQuestDescriptionRules(rulePack.Rules);
		}

		// Token: 0x0600A8D1 RID: 43217 RVA: 0x0006F16D File Offset: 0x0006D36D
		public static void AddQuestDescriptionRules(List<Rule> rules)
		{
			if (!QuestGen.working)
			{
				Log.Error("Tried to add quest description rules while not resolving any quest.", false);
				return;
			}
			QuestGen.questDescriptionRules.AddRange(QuestGenUtility.AppendCurrentPrefix(rules));
		}

		// Token: 0x0600A8D2 RID: 43218 RVA: 0x003137E8 File Offset: 0x003119E8
		public static void AddQuestDescriptionConstants(Dictionary<string, string> constants)
		{
			if (!QuestGen.working)
			{
				Log.Error("Tried to add quest description constants while not resolving any quest.", false);
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

		// Token: 0x0600A8D3 RID: 43219 RVA: 0x0006F192 File Offset: 0x0006D392
		public static void AddQuestNameRules(RulePack rulePack)
		{
			QuestGen.AddQuestNameRules(rulePack.Rules);
		}

		// Token: 0x0600A8D4 RID: 43220 RVA: 0x0006F19F File Offset: 0x0006D39F
		public static void AddQuestNameRules(List<Rule> rules)
		{
			if (!QuestGen.working)
			{
				Log.Error("Tried to add quest name rules while not resolving any quest.", false);
				return;
			}
			QuestGen.questNameRules.AddRange(QuestGenUtility.AppendCurrentPrefix(rules));
		}

		// Token: 0x0600A8D5 RID: 43221 RVA: 0x00313874 File Offset: 0x00311A74
		public static void AddQuestNameConstants(Dictionary<string, string> constants)
		{
			if (!QuestGen.working)
			{
				Log.Error("Tried to add quest name constants while not resolving any quest.", false);
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

		// Token: 0x0600A8D6 RID: 43222 RVA: 0x0006F1C4 File Offset: 0x0006D3C4
		public static void AddQuestContentRules(RulePack rulePack)
		{
			QuestGen.AddQuestContentRules(rulePack.Rules);
		}

		// Token: 0x0600A8D7 RID: 43223 RVA: 0x0006F1D1 File Offset: 0x0006D3D1
		public static void AddQuestContentRules(List<Rule> rules)
		{
			if (!QuestGen.working)
			{
				Log.Error("Tried to add quest content rules while not resolving any quest.", false);
				return;
			}
			QuestGen.questContentRules.AddRange(QuestGenUtility.AppendCurrentPrefix(rules));
		}

		// Token: 0x0600A8D8 RID: 43224 RVA: 0x0006F1F6 File Offset: 0x0006D3F6
		public static void AddSlateQuestTagToAddWhenFinished(string slateVarNameWithPrefix)
		{
			if (!QuestGen.slateQuestTagsToAddWhenFinished.Contains(slateVarNameWithPrefix))
			{
				QuestGen.slateQuestTagsToAddWhenFinished.Add(slateVarNameWithPrefix);
			}
		}

		// Token: 0x0600A8D9 RID: 43225 RVA: 0x0006F210 File Offset: 0x0006D410
		public static void AddTextRequest(string localKeyword, Action<string> setter, RulePack extraLocalRules = null)
		{
			QuestGen.AddTextRequest(localKeyword, setter, (extraLocalRules != null) ? extraLocalRules.Rules : null);
		}

		// Token: 0x0600A8DA RID: 43226 RVA: 0x00313900 File Offset: 0x00311B00
		public static void AddTextRequest(string localKeyword, Action<string> setter, List<Rule> extraLocalRules)
		{
			if (!QuestGen.working)
			{
				Log.Error("Tried to add a text request while not resolving any quest.", false);
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

		// Token: 0x0600A8DB RID: 43227 RVA: 0x00313990 File Offset: 0x00311B90
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

		// Token: 0x04007271 RID: 29297
		public static Quest quest;

		// Token: 0x04007272 RID: 29298
		public static Slate slate = new Slate();

		// Token: 0x04007273 RID: 29299
		private static QuestScriptDef root;

		// Token: 0x04007274 RID: 29300
		private static bool working;

		// Token: 0x04007275 RID: 29301
		private static List<QuestTextRequest> textRequests = new List<QuestTextRequest>();

		// Token: 0x04007276 RID: 29302
		private static List<Pawn> generatedPawns = new List<Pawn>();

		// Token: 0x04007277 RID: 29303
		private static Dictionary<string, int> generatedSignals = new Dictionary<string, int>();

		// Token: 0x04007278 RID: 29304
		private static Dictionary<string, int> generatedTargetQuestTags = new Dictionary<string, int>();

		// Token: 0x04007279 RID: 29305
		private static List<Rule> questDescriptionRules = new List<Rule>();

		// Token: 0x0400727A RID: 29306
		private static Dictionary<string, string> questDescriptionConstants = new Dictionary<string, string>();

		// Token: 0x0400727B RID: 29307
		private static List<Rule> questNameRules = new List<Rule>();

		// Token: 0x0400727C RID: 29308
		private static Dictionary<string, string> questNameConstants = new Dictionary<string, string>();

		// Token: 0x0400727D RID: 29309
		private static List<string> slateQuestTagsToAddWhenFinished = new List<string>();

		// Token: 0x0400727E RID: 29310
		private static List<Rule> questContentRules = new List<Rule>();
	}
}
