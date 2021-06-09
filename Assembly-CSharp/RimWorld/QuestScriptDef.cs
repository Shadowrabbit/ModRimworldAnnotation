using System;
using System.Collections.Generic;
using RimWorld.QuestGen;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02000FC9 RID: 4041
	public class QuestScriptDef : Def
	{
		// Token: 0x17000DA2 RID: 3490
		// (get) Token: 0x06005854 RID: 22612 RVA: 0x0003D620 File Offset: 0x0003B820
		public bool IsRootRandomSelected
		{
			get
			{
				return this.rootSelectionWeight != 0f;
			}
		}

		// Token: 0x17000DA3 RID: 3491
		// (get) Token: 0x06005855 RID: 22613 RVA: 0x0003D632 File Offset: 0x0003B832
		public bool IsRootDecree
		{
			get
			{
				return this.decreeSelectionWeight != 0f;
			}
		}

		// Token: 0x17000DA4 RID: 3492
		// (get) Token: 0x06005856 RID: 22614 RVA: 0x0003D644 File Offset: 0x0003B844
		public bool IsRootAny
		{
			get
			{
				return this.IsRootRandomSelected || this.IsRootDecree || this.isRootSpecial;
			}
		}

		// Token: 0x06005857 RID: 22615 RVA: 0x001CFD58 File Offset: 0x001CDF58
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

		// Token: 0x06005858 RID: 22616 RVA: 0x001CFDC8 File Offset: 0x001CDFC8
		public bool CanRun(Slate slate)
		{
			try
			{
				return this.root.TestRun(slate.DeepCopy());
			}
			catch (Exception arg)
			{
				Log.Error("Error while checking if can generate quest: " + arg, false);
			}
			return false;
		}

		// Token: 0x06005859 RID: 22617 RVA: 0x001CFE10 File Offset: 0x001CE010
		public bool CanRun(float points)
		{
			Slate slate = new Slate();
			slate.Set<float>("points", points, false);
			return this.CanRun(slate);
		}

		// Token: 0x0600585A RID: 22618 RVA: 0x0003D65E File Offset: 0x0003B85E
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

		// Token: 0x04003A5E RID: 14942
		public QuestNode root;

		// Token: 0x04003A5F RID: 14943
		public float rootSelectionWeight;

		// Token: 0x04003A60 RID: 14944
		public SimpleCurve rootSelectionWeightFactorFromPointsCurve;

		// Token: 0x04003A61 RID: 14945
		public float rootMinPoints;

		// Token: 0x04003A62 RID: 14946
		public float rootMinProgressScore;

		// Token: 0x04003A63 RID: 14947
		public bool rootIncreasesPopulation;

		// Token: 0x04003A64 RID: 14948
		public float decreeSelectionWeight;

		// Token: 0x04003A65 RID: 14949
		public List<string> decreeTags;

		// Token: 0x04003A66 RID: 14950
		public RulePack questDescriptionRules;

		// Token: 0x04003A67 RID: 14951
		public RulePack questNameRules;

		// Token: 0x04003A68 RID: 14952
		public RulePack questDescriptionAndNameRules;

		// Token: 0x04003A69 RID: 14953
		public RulePack questContentRules;

		// Token: 0x04003A6A RID: 14954
		public bool autoAccept;

		// Token: 0x04003A6B RID: 14955
		public FloatRange expireDaysRange = new FloatRange(-1f, -1f);

		// Token: 0x04003A6C RID: 14956
		public bool nameMustBeUnique;

		// Token: 0x04003A6D RID: 14957
		public int defaultChallengeRating = -1;

		// Token: 0x04003A6E RID: 14958
		public bool defaultHidden;

		// Token: 0x04003A6F RID: 14959
		public bool isRootSpecial;

		// Token: 0x04003A70 RID: 14960
		public bool canGiveRoyalFavor;

		// Token: 0x04003A71 RID: 14961
		public LetterDef questAvailableLetterDef;

		// Token: 0x04003A72 RID: 14962
		public bool hideFactionInfoInWindow;

		// Token: 0x04003A73 RID: 14963
		public bool affectedByPopulation;

		// Token: 0x04003A74 RID: 14964
		public bool affectedByPoints = true;
	}
}
