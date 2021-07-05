using System;
using RimWorld;

namespace Verse.Grammar
{
	// Token: 0x0200053B RID: 1339
	public class Rule_NamePerson : Rule
	{
		// Token: 0x170007EC RID: 2028
		// (get) Token: 0x0600285C RID: 10332 RVA: 0x0001F15E File Offset: 0x0001D35E
		public override float BaseSelectionWeight
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x0600285D RID: 10333 RVA: 0x000F6696 File Offset: 0x000F4896
		public override Rule DeepCopy()
		{
			Rule_NamePerson rule_NamePerson = (Rule_NamePerson)base.DeepCopy();
			rule_NamePerson.gender = this.gender;
			return rule_NamePerson;
		}

		// Token: 0x0600285E RID: 10334 RVA: 0x000F66B0 File Offset: 0x000F48B0
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

		// Token: 0x0600285F RID: 10335 RVA: 0x000F66E6 File Offset: 0x000F48E6
		public override string ToString()
		{
			return this.keyword + "->(personname)";
		}

		// Token: 0x040018EA RID: 6378
		public Gender gender;
	}
}
