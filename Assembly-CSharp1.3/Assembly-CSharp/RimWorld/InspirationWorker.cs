using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A81 RID: 2689
	public class InspirationWorker
	{
		// Token: 0x06004047 RID: 16455 RVA: 0x0015BE10 File Offset: 0x0015A010
		public virtual float CommonalityFor(Pawn pawn)
		{
			float num = 1f;
			if (pawn.skills != null && this.def.associatedSkills != null)
			{
				for (int i = 0; i < this.def.associatedSkills.Count; i++)
				{
					SkillDef skillDef = this.def.associatedSkills[i];
					for (int j = 0; j < pawn.skills.skills.Count; j++)
					{
						SkillRecord skillRecord = pawn.skills.skills[j];
						if (skillDef == skillRecord.def)
						{
							switch (pawn.skills.skills[j].passion)
							{
							case Passion.None:
								num = Mathf.Max(num, 1f);
								break;
							case Passion.Minor:
								num = Mathf.Max(num, 2.5f);
								break;
							case Passion.Major:
								num = Mathf.Max(num, 5f);
								break;
							}
						}
					}
				}
			}
			return this.def.baseCommonality * num;
		}

		// Token: 0x06004048 RID: 16456 RVA: 0x0015BF10 File Offset: 0x0015A110
		public virtual bool InspirationCanOccur(Pawn pawn)
		{
			if (!this.def.allowedOnAnimals && pawn.RaceProps.Animal)
			{
				return false;
			}
			if (!this.def.allowedOnNonColonists && !pawn.IsColonist)
			{
				return false;
			}
			if (!this.def.allowedOnDownedPawns && pawn.Downed)
			{
				return false;
			}
			if (this.def.requiredNonDisabledStats != null)
			{
				for (int i = 0; i < this.def.requiredNonDisabledStats.Count; i++)
				{
					if (this.def.requiredNonDisabledStats[i].Worker.IsDisabledFor(pawn))
					{
						return false;
					}
				}
			}
			if (this.def.requiredSkills != null)
			{
				for (int j = 0; j < this.def.requiredSkills.Count; j++)
				{
					if (!this.def.requiredSkills[j].PawnSatisfies(pawn))
					{
						return false;
					}
				}
			}
			if (!this.def.requiredAnySkill.NullOrEmpty<SkillRequirement>())
			{
				bool flag = false;
				for (int k = 0; k < this.def.requiredAnySkill.Count; k++)
				{
					if (this.def.requiredAnySkill[k].PawnSatisfies(pawn))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					return false;
				}
			}
			if (this.def.requiredNonDisabledWorkTypes != null)
			{
				for (int l = 0; l < this.def.requiredNonDisabledWorkTypes.Count; l++)
				{
					if (pawn.WorkTypeIsDisabled(this.def.requiredNonDisabledWorkTypes[l]))
					{
						return false;
					}
				}
			}
			if (!this.def.requiredAnyNonDisabledWorkType.NullOrEmpty<WorkTypeDef>())
			{
				bool flag2 = false;
				for (int m = 0; m < this.def.requiredAnyNonDisabledWorkType.Count; m++)
				{
					if (!pawn.WorkTypeIsDisabled(this.def.requiredAnyNonDisabledWorkType[m]))
					{
						flag2 = true;
						break;
					}
				}
				if (!flag2)
				{
					return false;
				}
			}
			if (this.def.requiredCapacities != null)
			{
				for (int n = 0; n < this.def.requiredCapacities.Count; n++)
				{
					if (!pawn.health.capacities.CapableOf(this.def.requiredCapacities[n]))
					{
						return false;
					}
				}
			}
			if (pawn.story != null)
			{
				for (int num = 0; num < pawn.story.traits.allTraits.Count; num++)
				{
					Trait trait = pawn.story.traits.allTraits[num];
					if (trait.CurrentData.disallowedInspirations != null && trait.CurrentData.disallowedInspirations.Contains(this.def))
					{
						return false;
					}
				}
			}
			return this.def.requiredNonDisabledWorkTags == WorkTags.None || (pawn.CombinedDisabledWorkTags & this.def.requiredNonDisabledWorkTags) == WorkTags.None;
		}

		// Token: 0x040024BF RID: 9407
		public InspirationDef def;

		// Token: 0x040024C0 RID: 9408
		private const float ChanceFactorPassionNone = 1f;

		// Token: 0x040024C1 RID: 9409
		private const float ChanceFactorPassionMinor = 2.5f;

		// Token: 0x040024C2 RID: 9410
		private const float ChanceFactorPassionMajor = 5f;
	}
}
