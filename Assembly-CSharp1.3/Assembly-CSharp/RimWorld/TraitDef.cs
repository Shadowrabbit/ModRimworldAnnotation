using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AE0 RID: 2784
	public class TraitDef : Def
	{
		// Token: 0x0600419B RID: 16795 RVA: 0x0015FFC2 File Offset: 0x0015E1C2
		public static TraitDef Named(string defName)
		{
			return DefDatabase<TraitDef>.GetNamed(defName, true);
		}

		// Token: 0x0600419C RID: 16796 RVA: 0x0015FFCC File Offset: 0x0015E1CC
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
			}));
			return this.degreeDatas[0];
		}

		// Token: 0x0600419D RID: 16797 RVA: 0x0016004E File Offset: 0x0015E24E
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (!this.degreeDatas.Any<TraitDegreeData>())
			{
				yield return this.defName + " has no degree datas.";
			}
			int num;
			for (int i = 0; i < this.degreeDatas.Count; i = num + 1)
			{
				TraitDef.<>c__DisplayClass16_0 CS$<>8__locals1 = new TraitDef.<>c__DisplayClass16_0();
				CS$<>8__locals1.dd = this.degreeDatas[i];
				if ((from dd2 in this.degreeDatas
				where dd2.degree == CS$<>8__locals1.dd.degree
				select dd2).Count<TraitDegreeData>() > 1)
				{
					yield return ">1 datas for degree " + CS$<>8__locals1.dd.degree;
				}
				if (!CS$<>8__locals1.dd.ingestibleModifiers.NullOrEmpty<IngestibleModifiers>())
				{
					foreach (IngestibleModifiers ingestibleModifiers in CS$<>8__locals1.dd.ingestibleModifiers)
					{
						if (ingestibleModifiers == null || ingestibleModifiers.ingestible == null)
						{
							yield return "ingestible override has a null target ingestible";
						}
					}
					List<IngestibleModifiers>.Enumerator enumerator2 = default(List<IngestibleModifiers>.Enumerator);
				}
				CS$<>8__locals1 = null;
				num = i;
			}
			yield break;
			yield break;
		}

		// Token: 0x0600419E RID: 16798 RVA: 0x0016005E File Offset: 0x0015E25E
		public bool ConflictsWith(Trait other)
		{
			return this.ConflictsWith(other.def);
		}

		// Token: 0x0600419F RID: 16799 RVA: 0x0016006C File Offset: 0x0015E26C
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

		// Token: 0x060041A0 RID: 16800 RVA: 0x001600E9 File Offset: 0x0015E2E9
		public bool ConflictsWithPassion(SkillDef passion)
		{
			return this.conflictingPassions != null && this.conflictingPassions.Contains(passion);
		}

		// Token: 0x060041A1 RID: 16801 RVA: 0x00160101 File Offset: 0x0015E301
		public bool RequiresPassion(SkillDef passion)
		{
			return this.forcedPassions != null && this.forcedPassions.Contains(passion);
		}

		// Token: 0x060041A2 RID: 16802 RVA: 0x00160119 File Offset: 0x0015E319
		public float GetGenderSpecificCommonality(Gender gender)
		{
			if (gender == Gender.Female && this.commonalityFemale >= 0f)
			{
				return this.commonalityFemale;
			}
			return this.commonality;
		}

		// Token: 0x040027C8 RID: 10184
		public List<TraitDegreeData> degreeDatas = new List<TraitDegreeData>();

		// Token: 0x040027C9 RID: 10185
		public List<TraitDef> conflictingTraits = new List<TraitDef>();

		// Token: 0x040027CA RID: 10186
		public List<string> exclusionTags = new List<string>();

		// Token: 0x040027CB RID: 10187
		public List<SkillDef> conflictingPassions = new List<SkillDef>();

		// Token: 0x040027CC RID: 10188
		public List<SkillDef> forcedPassions = new List<SkillDef>();

		// Token: 0x040027CD RID: 10189
		public List<WorkTypeDef> requiredWorkTypes = new List<WorkTypeDef>();

		// Token: 0x040027CE RID: 10190
		public WorkTags requiredWorkTags;

		// Token: 0x040027CF RID: 10191
		public List<WorkTypeDef> disabledWorkTypes = new List<WorkTypeDef>();

		// Token: 0x040027D0 RID: 10192
		public WorkTags disabledWorkTags;

		// Token: 0x040027D1 RID: 10193
		public AnimalType? disableHostilityFromAnimalType;

		// Token: 0x040027D2 RID: 10194
		public FactionDef disableHostilityFromFaction;

		// Token: 0x040027D3 RID: 10195
		private float commonality = 1f;

		// Token: 0x040027D4 RID: 10196
		private float commonalityFemale = -1f;

		// Token: 0x040027D5 RID: 10197
		public bool allowOnHostileSpawn = true;
	}
}
