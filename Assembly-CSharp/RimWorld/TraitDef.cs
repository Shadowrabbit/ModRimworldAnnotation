using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001003 RID: 4099
	public class TraitDef : Def
	{
		// Token: 0x06005970 RID: 22896 RVA: 0x0003E216 File Offset: 0x0003C416
		public static TraitDef Named(string defName)
		{
			return DefDatabase<TraitDef>.GetNamed(defName, true);
		}

		// Token: 0x06005971 RID: 22897 RVA: 0x001D2510 File Offset: 0x001D0710
		public TraitDegreeData DataAtDegree(int degree)
		{
			for (int i = 0; i < this.degreeDatas.Count; i++)
			{
				if (this.degreeDatas[i].degree == degree)
				{
					return this.degreeDatas[i];
				}
			}
			Log.Error(string.Concat(new object[]
			{
				this.defName,
				" found no data at degree ",
				degree,
				", returning first defined."
			}), false);
			return this.degreeDatas[0];
		}

		// Token: 0x06005972 RID: 22898 RVA: 0x0003E21F File Offset: 0x0003C41F
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.commonality < 0.001f && this.commonalityFemale < 0.001f)
			{
				yield return "TraitDef " + this.defName + " has 0 commonality.";
			}
			if (!this.degreeDatas.Any<TraitDegreeData>())
			{
				yield return this.defName + " has no degree datas.";
			}
			int num;
			for (int i = 0; i < this.degreeDatas.Count; i = num + 1)
			{
				TraitDegreeData dd = this.degreeDatas[i];
				if ((from dd2 in this.degreeDatas
				where dd2.degree == dd.degree
				select dd2).Count<TraitDegreeData>() > 1)
				{
					yield return ">1 datas for degree " + dd.degree;
				}
				num = i;
			}
			yield break;
			yield break;
		}

		// Token: 0x06005973 RID: 22899 RVA: 0x0003E22F File Offset: 0x0003C42F
		public bool ConflictsWith(Trait other)
		{
			return this.ConflictsWith(other.def);
		}

		// Token: 0x06005974 RID: 22900 RVA: 0x001D2594 File Offset: 0x001D0794
		public bool ConflictsWith(TraitDef other)
		{
			if ((other.conflictingTraits != null && other.conflictingTraits.Contains(this)) || (this.conflictingTraits != null && this.conflictingTraits.Contains(other)))
			{
				return true;
			}
			if (this.exclusionTags != null && other.exclusionTags != null)
			{
				for (int i = 0; i < this.exclusionTags.Count; i++)
				{
					if (other.exclusionTags.Contains(this.exclusionTags[i]))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06005975 RID: 22901 RVA: 0x0003E23D File Offset: 0x0003C43D
		public bool ConflictsWithPassion(SkillDef passion)
		{
			return this.conflictingPassions != null && this.conflictingPassions.Contains(passion);
		}

		// Token: 0x06005976 RID: 22902 RVA: 0x0003E255 File Offset: 0x0003C455
		public bool RequiresPassion(SkillDef passion)
		{
			return this.forcedPassions != null && this.forcedPassions.Contains(passion);
		}

		// Token: 0x06005977 RID: 22903 RVA: 0x0003E26D File Offset: 0x0003C46D
		public float GetGenderSpecificCommonality(Gender gender)
		{
			if (gender == Gender.Female && this.commonalityFemale >= 0f)
			{
				return this.commonalityFemale;
			}
			return this.commonality;
		}

		// Token: 0x04003C16 RID: 15382
		public List<TraitDegreeData> degreeDatas = new List<TraitDegreeData>();

		// Token: 0x04003C17 RID: 15383
		public List<TraitDef> conflictingTraits = new List<TraitDef>();

		// Token: 0x04003C18 RID: 15384
		public List<string> exclusionTags = new List<string>();

		// Token: 0x04003C19 RID: 15385
		public List<SkillDef> conflictingPassions = new List<SkillDef>();

		// Token: 0x04003C1A RID: 15386
		public List<SkillDef> forcedPassions = new List<SkillDef>();

		// Token: 0x04003C1B RID: 15387
		public List<WorkTypeDef> requiredWorkTypes = new List<WorkTypeDef>();

		// Token: 0x04003C1C RID: 15388
		public WorkTags requiredWorkTags;

		// Token: 0x04003C1D RID: 15389
		public List<WorkTypeDef> disabledWorkTypes = new List<WorkTypeDef>();

		// Token: 0x04003C1E RID: 15390
		public WorkTags disabledWorkTags;

		// Token: 0x04003C1F RID: 15391
		private float commonality = 1f;

		// Token: 0x04003C20 RID: 15392
		private float commonalityFemale = -1f;

		// Token: 0x04003C21 RID: 15393
		public bool allowOnHostileSpawn = true;
	}
}
