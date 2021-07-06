using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02001635 RID: 5685
	public class TaleData_Pawn : TaleData
	{
		// Token: 0x06007B97 RID: 31639 RVA: 0x00251564 File Offset: 0x0024F764
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

		// Token: 0x06007B98 RID: 31640 RVA: 0x00251680 File Offset: 0x0024F880
		public override IEnumerable<Rule> GetRules(string prefix)
		{
			return GrammarUtility.RulesForPawn(prefix, this.name, this.title, this.kind, this.gender, this.faction, this.age, this.chronologicalAge, this.relationInfo, this.everBeenColonistOrTameAnimal, this.everBeenQuestLodger, this.isFactionLeader, this.royalTitles, null, true);
		}

		// Token: 0x06007B99 RID: 31641 RVA: 0x002516E0 File Offset: 0x0024F8E0
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

		// Token: 0x06007B9A RID: 31642 RVA: 0x00251888 File Offset: 0x0024FA88
		public static TaleData_Pawn GenerateRandom()
		{
			PawnKindDef random = DefDatabase<PawnKindDef>.GetRandom();
			Faction faction = FactionUtility.DefaultFactionFrom(random.defaultFactionType);
			return TaleData_Pawn.GenerateFrom(PawnGenerator.GeneratePawn(random, faction));
		}

		// Token: 0x040050EE RID: 20718
		public Pawn pawn;

		// Token: 0x040050EF RID: 20719
		public PawnKindDef kind;

		// Token: 0x040050F0 RID: 20720
		public Faction faction;

		// Token: 0x040050F1 RID: 20721
		public Gender gender;

		// Token: 0x040050F2 RID: 20722
		public int age;

		// Token: 0x040050F3 RID: 20723
		public int chronologicalAge;

		// Token: 0x040050F4 RID: 20724
		public string relationInfo;

		// Token: 0x040050F5 RID: 20725
		public bool everBeenColonistOrTameAnimal;

		// Token: 0x040050F6 RID: 20726
		public bool everBeenQuestLodger;

		// Token: 0x040050F7 RID: 20727
		public bool isFactionLeader;

		// Token: 0x040050F8 RID: 20728
		public List<RoyalTitle> royalTitles;

		// Token: 0x040050F9 RID: 20729
		public Name name;

		// Token: 0x040050FA RID: 20730
		public string title;

		// Token: 0x040050FB RID: 20731
		public ThingDef primaryEquipment;

		// Token: 0x040050FC RID: 20732
		public ThingDef notableApparel;

		// Token: 0x040050FD RID: 20733
		private List<Faction> tmpFactions;

		// Token: 0x040050FE RID: 20734
		private List<RoyalTitleDef> tmpRoyalTitles;
	}
}
