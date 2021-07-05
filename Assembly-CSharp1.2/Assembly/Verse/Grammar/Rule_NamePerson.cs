using System;
using RimWorld;

namespace Verse.Grammar
{
	// Token: 0x0200090A RID: 2314
	public class Rule_NamePerson : Rule
	{
		// Token: 0x17000922 RID: 2338
		// (get) Token: 0x06003979 RID: 14713 RVA: 0x0000CE6C File Offset: 0x0000B06C
		public override float BaseSelectionWeight
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x0600397A RID: 14714 RVA: 0x0002C5D5 File Offset: 0x0002A7D5
		public override Rule DeepCopy()
		{
			Rule_NamePerson rule_NamePerson = (Rule_NamePerson)base.DeepCopy();
			rule_NamePerson.gender = this.gender;
			return rule_NamePerson;
		}

		// Token: 0x0600397B RID: 14715 RVA: 0x001679F8 File Offset: 0x00165BF8
		public override string Generate()
		{
			NameBank nameBank = PawnNameDatabaseShuffled.BankOf(PawnNameCategory.HumanStandard);
			Gender gender = this.gender;
			if (gender == Gender.None)
			{
				gender = ((Rand.Value < 0.5f) ? Gender.Male : Gender.Female);
			}
			return nameBank.GetName(PawnNameSlot.First, gender, false);
		}

		// Token: 0x0600397C RID: 14716 RVA: 0x0002C5EE File Offset: 0x0002A7EE
		public override string ToString()
		{
			return this.keyword + "->(personname)";
		}

		// Token: 0x040027D1 RID: 10193
		public Gender gender;
	}
}
