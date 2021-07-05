using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x0200163B RID: 5691
	public class TaleData_Trader : TaleData
	{
		// Token: 0x1700130B RID: 4875
		// (get) Token: 0x06007BBB RID: 31675 RVA: 0x000531E4 File Offset: 0x000513E4
		private bool IsPawn
		{
			get
			{
				return this.pawnID >= 0;
			}
		}

		// Token: 0x06007BBC RID: 31676 RVA: 0x000531F2 File Offset: 0x000513F2
		public override void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.name, "name", null, false);
			Scribe_Values.Look<int>(ref this.pawnID, "pawnID", -1, false);
			Scribe_Values.Look<Gender>(ref this.gender, "gender", Gender.Male, false);
		}

		// Token: 0x06007BBD RID: 31677 RVA: 0x0005322A File Offset: 0x0005142A
		public override IEnumerable<Rule> GetRules(string prefix)
		{
			string output;
			if (this.IsPawn)
			{
				output = this.name;
			}
			else
			{
				output = Find.ActiveLanguageWorker.WithIndefiniteArticle(this.name, false, false);
			}
			yield return new Rule_String(prefix + "_nameFull", output);
			string nameShortIndefinite;
			if (this.IsPawn)
			{
				nameShortIndefinite = this.name;
			}
			else
			{
				nameShortIndefinite = Find.ActiveLanguageWorker.WithIndefiniteArticle(this.name, false, false);
			}
			yield return new Rule_String(prefix + "_indefinite", nameShortIndefinite);
			yield return new Rule_String(prefix + "_nameIndef", nameShortIndefinite);
			nameShortIndefinite = null;
			if (this.IsPawn)
			{
				nameShortIndefinite = this.name;
			}
			else
			{
				nameShortIndefinite = Find.ActiveLanguageWorker.WithDefiniteArticle(this.name, false, false);
			}
			yield return new Rule_String(prefix + "_definite", nameShortIndefinite);
			yield return new Rule_String(prefix + "_nameDef", nameShortIndefinite);
			nameShortIndefinite = null;
			yield return new Rule_String(prefix + "_pronoun", this.gender.GetPronoun());
			yield return new Rule_String(prefix + "_possessive", this.gender.GetPossessive());
			yield break;
		}

		// Token: 0x06007BBE RID: 31678 RVA: 0x0025208C File Offset: 0x0025028C
		public static TaleData_Trader GenerateFrom(ITrader trader)
		{
			TaleData_Trader taleData_Trader = new TaleData_Trader();
			taleData_Trader.name = trader.TraderName;
			Pawn pawn = trader as Pawn;
			if (pawn != null)
			{
				taleData_Trader.pawnID = pawn.thingIDNumber;
				taleData_Trader.gender = pawn.gender;
			}
			return taleData_Trader;
		}

		// Token: 0x06007BBF RID: 31679 RVA: 0x002520D0 File Offset: 0x002502D0
		public static TaleData_Trader GenerateRandom()
		{
			PawnKindDef pawnKindDef = (from d in DefDatabase<PawnKindDef>.AllDefs
			where d.trader
			select d).RandomElement<PawnKindDef>();
			Pawn pawn = PawnGenerator.GeneratePawn(pawnKindDef, FactionUtility.DefaultFactionFrom(pawnKindDef.defaultFactionType));
			pawn.mindState.wantsToTradeWithColony = true;
			PawnComponentsUtility.AddAndRemoveDynamicComponents(pawn, true);
			return TaleData_Trader.GenerateFrom(pawn);
		}

		// Token: 0x0400511C RID: 20764
		public string name;

		// Token: 0x0400511D RID: 20765
		public int pawnID = -1;

		// Token: 0x0400511E RID: 20766
		public Gender gender = Gender.Male;
	}
}
