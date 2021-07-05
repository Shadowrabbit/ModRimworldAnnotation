using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02001028 RID: 4136
	public class TaleData_Trader : TaleData
	{
		// Token: 0x1700109E RID: 4254
		// (get) Token: 0x060061A7 RID: 24999 RVA: 0x00212D15 File Offset: 0x00210F15
		private bool IsPawn
		{
			get
			{
				return this.pawnID >= 0;
			}
		}

		// Token: 0x060061A8 RID: 25000 RVA: 0x00212D23 File Offset: 0x00210F23
		public override void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.name, "name", null, false);
			Scribe_Values.Look<int>(ref this.pawnID, "pawnID", -1, false);
			Scribe_Values.Look<Gender>(ref this.gender, "gender", Gender.Male, false);
		}

		// Token: 0x060061A9 RID: 25001 RVA: 0x00212D5B File Offset: 0x00210F5B
		public override IEnumerable<Rule> GetRules(string prefix, Dictionary<string, string> constants = null)
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

		// Token: 0x060061AA RID: 25002 RVA: 0x00212D74 File Offset: 0x00210F74
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

		// Token: 0x060061AB RID: 25003 RVA: 0x00212DB8 File Offset: 0x00210FB8
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

		// Token: 0x040037B9 RID: 14265
		public string name;

		// Token: 0x040037BA RID: 14266
		public int pawnID = -1;

		// Token: 0x040037BB RID: 14267
		public Gender gender = Gender.Male;
	}
}
