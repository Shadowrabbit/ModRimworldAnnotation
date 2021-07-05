using System;
using System.Collections.Generic;
using RimWorld.QuestGen;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02000AAC RID: 2732
	public class QuestScriptDef : Def
	{
		// Token: 0x17000B63 RID: 2915
		// (get) Token: 0x060040E0 RID: 16608 RVA: 0x0015E366 File Offset: 0x0015C566
		public bool IsRootRandomSelected
		{
			get
			{
				return this.rootSelectionWeight != 0f;
			}
		}

		// Token: 0x17000B64 RID: 2916
		// (get) Token: 0x060040E1 RID: 16609 RVA: 0x0015E378 File Offset: 0x0015C578
		public bool IsRootDecree
		{
			get
			{
				return this.decreeSelectionWeight != 0f;
			}
		}

		// Token: 0x17000B65 RID: 2917
		// (get) Token: 0x060040E2 RID: 16610 RVA: 0x0015E38A File Offset: 0x0015C58A
		public bool IsRootAny
		{
			get
			{
				return this.IsRootRandomSelected || this.IsRootDecree || this.isRootSpecial;
			}
		}

		// Token: 0x17000B66 RID: 2918
		// (get) Token: 0x060040E3 RID: 16611 RVA: 0x0015E3A4 File Offset: 0x0015C5A4
		public bool IsEpic
		{
			get
			{
				return this.epic;
			}
		}

		// Token: 0x060040E4 RID: 16612 RVA: 0x0015E3AC File Offset: 0x0015C5AC
		public void Run()
		{
			if (this.questDescriptionRules != null)
			{
				QuestGen.AddQuestDescriptionRules(this.questDescriptionRules);
			}
			if (this.questNameRules != null)
			{
				QuestGen.AddQuestNameRules(this.questNameRules);
			}
			if (this.questDescriptionAndNameRules != null)
			{
				QuestGen.AddQuestDescriptionRules(this.questDescriptionAndNameRules);
				QuestGen.AddQuestNameRules(this.questDescriptionAndNameRules);
			}
			if (this.questContentRules != null)
			{
				QuestGen.AddQuestContentRules(this.questContentRules);
			}
			this.root.Run();
		}

		// Token: 0x060040E5 RID: 16613 RVA: 0x0015E41C File Offset: 0x0015C61C
		public bool CanRun(Slate slate)
		{
			try
			{
				return this.root.TestRun(slate.DeepCopy());
			}
			catch (Exception arg)
			{
				Log.Error("Error while checking if can generate quest: " + arg);
			}
			return false;
		}

		// Token: 0x060040E6 RID: 16614 RVA: 0x0015E464 File Offset: 0x0015C664
		public bool CanRun(float points)
		{
			Slate slate = new Slate();
			slate.Set<float>("points", points, false);
			return this.CanRun(slate);
		}

		// Token: 0x060040E7 RID: 16615 RVA: 0x0015E48B File Offset: 0x0015C68B
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.rootSelectionWeight > 0f && !this.autoAccept && this.expireDaysRange.TrueMax <= 0f)
			{
				yield return "rootSelectionWeight > 0 but expireDaysRange not set";
			}
			if (this.autoAccept && this.expireDaysRange.TrueMax > 0f)
			{
				yield return "autoAccept but there is an expireDaysRange set";
			}
			if (this.defaultChallengeRating > 0 && !this.IsRootAny)
			{
				yield return "non-root quest has defaultChallengeRating";
			}
			yield break;
			yield break;
		}

		// Token: 0x0400261C RID: 9756
		public QuestNode root;

		// Token: 0x0400261D RID: 9757
		public float rootSelectionWeight;

		// Token: 0x0400261E RID: 9758
		public SimpleCurve rootSelectionWeightFactorFromPointsCurve;

		// Token: 0x0400261F RID: 9759
		public float rootMinPoints;

		// Token: 0x04002620 RID: 9760
		public float rootMinProgressScore;

		// Token: 0x04002621 RID: 9761
		public bool rootIncreasesPopulation;

		// Token: 0x04002622 RID: 9762
		public float minRefireDays;

		// Token: 0x04002623 RID: 9763
		public float decreeSelectionWeight;

		// Token: 0x04002624 RID: 9764
		public List<string> decreeTags;

		// Token: 0x04002625 RID: 9765
		public RulePack questDescriptionRules;

		// Token: 0x04002626 RID: 9766
		public RulePack questNameRules;

		// Token: 0x04002627 RID: 9767
		public RulePack questDescriptionAndNameRules;

		// Token: 0x04002628 RID: 9768
		public RulePack questContentRules;

		// Token: 0x04002629 RID: 9769
		public bool autoAccept;

		// Token: 0x0400262A RID: 9770
		public bool hideOnCleanup;

		// Token: 0x0400262B RID: 9771
		public FloatRange expireDaysRange = new FloatRange(-1f, -1f);

		// Token: 0x0400262C RID: 9772
		public bool nameMustBeUnique;

		// Token: 0x0400262D RID: 9773
		public int defaultChallengeRating = -1;

		// Token: 0x0400262E RID: 9774
		public bool defaultHidden;

		// Token: 0x0400262F RID: 9775
		public bool isRootSpecial;

		// Token: 0x04002630 RID: 9776
		public bool canGiveRoyalFavor;

		// Token: 0x04002631 RID: 9777
		public LetterDef questAvailableLetterDef;

		// Token: 0x04002632 RID: 9778
		public bool hideInvolvedFactionsInfo;

		// Token: 0x04002633 RID: 9779
		public bool affectedByPopulation;

		// Token: 0x04002634 RID: 9780
		public bool affectedByPoints = true;

		// Token: 0x04002635 RID: 9781
		public bool defaultCharity;

		// Token: 0x04002636 RID: 9782
		public HistoryEventDef successHistoryEvent;

		// Token: 0x04002637 RID: 9783
		public HistoryEventDef failedOrExpiredHistoryEvent;

		// Token: 0x04002638 RID: 9784
		public bool sendAvailableLetter = true;

		// Token: 0x04002639 RID: 9785
		public bool epic;

		// Token: 0x0400263A RID: 9786
		public QuestScriptDef epicParent;

		// Token: 0x0400263B RID: 9787
		public bool endOnColonyMove = true;
	}
}
