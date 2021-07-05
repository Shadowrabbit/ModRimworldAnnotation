using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02001025 RID: 4133
	public class TaleData_Pawn : TaleData
	{
		// Token: 0x06006197 RID: 24983 RVA: 0x002126B8 File Offset: 0x002108B8
		public override void ExposeData()
		{
			Scribe_References.Look<Pawn>(ref this.pawn, "pawn", true);
			Scribe_Defs.Look<PawnKindDef>(ref this.kind, "kind");
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Values.Look<Gender>(ref this.gender, "gender", Gender.None, false);
			Scribe_Values.Look<int>(ref this.age, "age", 0, false);
			Scribe_Values.Look<int>(ref this.chronologicalAge, "chronologicalAge", 0, false);
			Scribe_Values.Look<string>(ref this.relationInfo, "relationInfo", null, false);
			Scribe_Values.Look<bool>(ref this.everBeenColonistOrTameAnimal, "everBeenColonistOrTameAnimal", false, false);
			Scribe_Values.Look<bool>(ref this.everBeenQuestLodger, "everBeenQuestLodger", false, false);
			Scribe_Values.Look<bool>(ref this.isFactionLeader, "isFactionLeader", false, false);
			Scribe_Collections.Look<RoyalTitle>(ref this.royalTitles, "royalTitles", LookMode.Deep, Array.Empty<object>());
			Scribe_Deep.Look<Name>(ref this.name, "name", Array.Empty<object>());
			Scribe_Values.Look<string>(ref this.title, "title", null, false);
			Scribe_Defs.Look<ThingDef>(ref this.primaryEquipment, "peq");
			Scribe_Defs.Look<ThingDef>(ref this.notableApparel, "app");
		}

		// Token: 0x06006198 RID: 24984 RVA: 0x002127D4 File Offset: 0x002109D4
		public override IEnumerable<Rule> GetRules(string prefix, Dictionary<string, string> constants = null)
		{
			return GrammarUtility.RulesForPawn(prefix, this.name, this.title, this.kind, this.gender, this.faction, this.age, this.chronologicalAge, this.relationInfo, this.everBeenColonistOrTameAnimal, this.everBeenQuestLodger, this.isFactionLeader, this.royalTitles, constants, true);
		}

		// Token: 0x06006199 RID: 24985 RVA: 0x00212834 File Offset: 0x00210A34
		public static TaleData_Pawn GenerateFrom(Pawn pawn)
		{
			TaleData_Pawn taleData_Pawn = new TaleData_Pawn();
			taleData_Pawn.pawn = pawn;
			taleData_Pawn.kind = pawn.kindDef;
			taleData_Pawn.faction = pawn.Faction;
			taleData_Pawn.gender = (pawn.RaceProps.hasGenders ? pawn.gender : Gender.None);
			taleData_Pawn.age = pawn.ageTracker.AgeBiologicalYears;
			taleData_Pawn.chronologicalAge = pawn.ageTracker.AgeChronologicalYears;
			taleData_Pawn.everBeenColonistOrTameAnimal = PawnUtility.EverBeenColonistOrTameAnimal(pawn);
			taleData_Pawn.everBeenQuestLodger = PawnUtility.EverBeenQuestLodger(pawn);
			taleData_Pawn.isFactionLeader = (pawn.Faction != null && pawn.Faction.leader == pawn);
			if (pawn.royalty != null)
			{
				taleData_Pawn.royalTitles = new List<RoyalTitle>();
				foreach (RoyalTitle other in pawn.royalty.AllTitlesForReading)
				{
					taleData_Pawn.royalTitles.Add(new RoyalTitle(other));
				}
			}
			TaggedString taggedString = "";
			PawnRelationUtility.TryAppendRelationsWithColonistsInfo(ref taggedString, pawn);
			taleData_Pawn.relationInfo = taggedString.Resolve();
			if (pawn.story != null)
			{
				taleData_Pawn.title = pawn.story.title;
			}
			if (pawn.RaceProps.Humanlike)
			{
				taleData_Pawn.name = pawn.Name;
				if (pawn.equipment.Primary != null)
				{
					taleData_Pawn.primaryEquipment = pawn.equipment.Primary.def;
				}
				Apparel apparel;
				if (pawn.apparel.WornApparel.TryRandomElement(out apparel))
				{
					taleData_Pawn.notableApparel = apparel.def;
				}
			}
			return taleData_Pawn;
		}

		// Token: 0x0600619A RID: 24986 RVA: 0x002129DC File Offset: 0x00210BDC
		public static TaleData_Pawn GenerateRandom()
		{
			PawnKindDef random = DefDatabase<PawnKindDef>.GetRandom();
			Faction faction = FactionUtility.DefaultFactionFrom(random.defaultFactionType);
			return TaleData_Pawn.GenerateFrom(PawnGenerator.GeneratePawn(random, faction));
		}

		// Token: 0x0400379B RID: 14235
		public Pawn pawn;

		// Token: 0x0400379C RID: 14236
		public PawnKindDef kind;

		// Token: 0x0400379D RID: 14237
		public Faction faction;

		// Token: 0x0400379E RID: 14238
		public Gender gender;

		// Token: 0x0400379F RID: 14239
		public int age;

		// Token: 0x040037A0 RID: 14240
		public int chronologicalAge;

		// Token: 0x040037A1 RID: 14241
		public string relationInfo;

		// Token: 0x040037A2 RID: 14242
		public bool everBeenColonistOrTameAnimal;

		// Token: 0x040037A3 RID: 14243
		public bool everBeenQuestLodger;

		// Token: 0x040037A4 RID: 14244
		public bool isFactionLeader;

		// Token: 0x040037A5 RID: 14245
		public List<RoyalTitle> royalTitles;

		// Token: 0x040037A6 RID: 14246
		public Name name;

		// Token: 0x040037A7 RID: 14247
		public string title;

		// Token: 0x040037A8 RID: 14248
		public ThingDef primaryEquipment;

		// Token: 0x040037A9 RID: 14249
		public ThingDef notableApparel;

		// Token: 0x040037AA RID: 14250
		private List<Faction> tmpFactions;

		// Token: 0x040037AB RID: 14251
		private List<RoyalTitleDef> tmpRoyalTitles;
	}
}
