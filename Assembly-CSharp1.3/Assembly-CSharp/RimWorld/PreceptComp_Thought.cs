using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EE2 RID: 3810
	public abstract class PreceptComp_Thought : PreceptComp
	{
		// Token: 0x17000FC2 RID: 4034
		// (get) Token: 0x06005A8B RID: 23179 RVA: 0x001F520C File Offset: 0x001F340C
		public bool AffectsMood
		{
			get
			{
				for (int i = 0; i < this.thought.stages.Count; i++)
				{
					if (this.thought.stages[i].baseMoodEffect != 0f)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x06005A8C RID: 23180 RVA: 0x001F5254 File Offset: 0x001F3454
		private string ParseDescription(string description, int thoughtStage = -1)
		{
			bool flag = thoughtStage != -1;
			if (!flag)
			{
				thoughtStage = 0;
			}
			IPreceptCompDescriptionArgs preceptCompDescriptionArgs;
			if (!description.NullOrEmpty() && (preceptCompDescriptionArgs = (this.thought.Worker as IPreceptCompDescriptionArgs)) != null)
			{
				description = description.Formatted(preceptCompDescriptionArgs.GetDescriptionArgs());
			}
			if (description.NullOrEmpty())
			{
				description = ((!this.thought.stages[thoughtStage].LabelCap.NullOrEmpty()) ? this.thought.stages[thoughtStage].LabelCap : this.thought.LabelCap.Resolve());
			}
			if (this.AffectsMood)
			{
				ThoughtStage thoughtStage2 = this.thought.stages[thoughtStage];
				if (this.thought.minExpectationForNegativeThought != null && thoughtStage2.baseMoodEffect < 0f)
				{
					description += " (" + "MinExpectation".Translate() + ": " + this.thought.minExpectationForNegativeThought.LabelCap + ")";
				}
				string str;
				if (this.tooltipShowMoodRange && !flag)
				{
					float num = float.PositiveInfinity;
					float num2 = float.NegativeInfinity;
					for (int i = 0; i < this.thought.stages.Count; i++)
					{
						num = Mathf.Min(num, this.thought.stages[i].baseMoodEffect);
						num2 = Mathf.Max(num2, this.thought.stages[i].baseMoodEffect);
					}
					str = "PreceptThoughtMoodRange".Translate(num.ToStringWithSign("F0"), num2.ToStringWithSign("F0"));
				}
				else
				{
					str = thoughtStage2.baseMoodEffect.ToStringWithSign("F0");
				}
				return description + ": " + str;
			}
			return description + ": " + this.thought.stages[thoughtStage].baseOpinionOffset.ToStringWithSign("F0");
		}

		// Token: 0x06005A8D RID: 23181 RVA: 0x001F5476 File Offset: 0x001F3676
		public override IEnumerable<string> GetDescriptions()
		{
			if (!this.thoughtStageDescriptions.NullOrEmpty<string>())
			{
				int stage = 0;
				foreach (string description in this.thoughtStageDescriptions)
				{
					yield return this.ParseDescription(description, stage);
					int num = stage;
					stage = num + 1;
				}
				List<string>.Enumerator enumerator = default(List<string>.Enumerator);
			}
			else
			{
				foreach (string description2 in base.GetDescriptions())
				{
					yield return this.ParseDescription(description2, -1);
				}
				IEnumerator<string> enumerator2 = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x04003504 RID: 13572
		public ThoughtDef thought;

		// Token: 0x04003505 RID: 13573
		[MustTranslate]
		public List<string> thoughtStageDescriptions;

		// Token: 0x04003506 RID: 13574
		public bool tooltipShowMoodRange;
	}
}
