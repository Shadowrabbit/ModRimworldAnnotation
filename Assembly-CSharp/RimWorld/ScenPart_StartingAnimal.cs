using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200160D RID: 5645
	public class ScenPart_StartingAnimal : ScenPart
	{
		// Token: 0x06007ABC RID: 31420 RVA: 0x0005280B File Offset: 0x00050A0B
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<PawnKindDef>(ref this.animalKind, "animalKind");
			Scribe_Values.Look<int>(ref this.count, "count", 0, false);
			Scribe_Values.Look<float>(ref this.bondToRandomPlayerPawnChance, "bondToRandomPlayerPawnChance", 0f, false);
		}

		// Token: 0x06007ABD RID: 31421 RVA: 0x0024F464 File Offset: 0x0024D664
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Rect scenPartRect = listing.GetScenPartRect(this, ScenPart.RowHeight * 2f);
			Listing_Standard listing_Standard = new Listing_Standard();
			listing_Standard.Begin(scenPartRect.TopHalf());
			listing_Standard.ColumnWidth = scenPartRect.width;
			listing_Standard.TextFieldNumeric<int>(ref this.count, ref this.countBuf, 1f, 1E+09f);
			listing_Standard.End();
			if (Widgets.ButtonText(scenPartRect.BottomHalf(), this.CurrentAnimalLabel().CapitalizeFirst(), true, true, true))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				list.Add(new FloatMenuOption("RandomPet".Translate().CapitalizeFirst(), delegate()
				{
					this.animalKind = null;
				}, MenuOptionPriority.Default, null, null, 0f, null, null));
				foreach (PawnKindDef localKind2 in this.PossibleAnimals(false))
				{
					PawnKindDef localKind = localKind2;
					list.Add(new FloatMenuOption(localKind.LabelCap, delegate()
					{
						this.animalKind = localKind;
					}, MenuOptionPriority.Default, null, null, 0f, null, null));
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}
		}

		// Token: 0x06007ABE RID: 31422 RVA: 0x0024F5B8 File Offset: 0x0024D7B8
		private IEnumerable<PawnKindDef> PossibleAnimals(bool checkForTamer = true)
		{
			return from td in DefDatabase<PawnKindDef>.AllDefs
			where td.RaceProps.Animal && (!checkForTamer || ScenPart_StartingAnimal.CanKeepPetTame(td))
			select td;
		}

		// Token: 0x06007ABF RID: 31423 RVA: 0x0024F5E8 File Offset: 0x0024D7E8
		private static bool CanKeepPetTame(PawnKindDef def)
		{
			float level = (float)Find.GameInitData.startingAndOptionalPawns.Take(Find.GameInitData.startingPawnCount).MaxBy((Pawn c) => c.skills.GetSkill(SkillDefOf.Animals).Level).skills.GetSkill(SkillDefOf.Animals).Level;
			float statValueAbstract = def.race.GetStatValueAbstract(StatDefOf.MinimumHandlingSkill, null);
			return level >= statValueAbstract;
		}

		// Token: 0x06007AC0 RID: 31424 RVA: 0x0005284B File Offset: 0x00050A4B
		private IEnumerable<PawnKindDef> RandomPets()
		{
			return from td in this.PossibleAnimals(true)
			where td.RaceProps.petness > 0f
			select td;
		}

		// Token: 0x06007AC1 RID: 31425 RVA: 0x00052878 File Offset: 0x00050A78
		private string CurrentAnimalLabel()
		{
			if (this.animalKind == null)
			{
				return "RandomPet".TranslateSimple();
			}
			return this.animalKind.label;
		}

		// Token: 0x06007AC2 RID: 31426 RVA: 0x00052898 File Offset: 0x00050A98
		public override string Summary(Scenario scen)
		{
			return ScenSummaryList.SummaryWithList(scen, "PlayerStartsWith", ScenPart_StartingThing_Defined.PlayerStartWithIntro);
		}

		// Token: 0x06007AC3 RID: 31427 RVA: 0x000528AA File Offset: 0x00050AAA
		public override IEnumerable<string> GetSummaryListEntries(string tag)
		{
			if (tag == "PlayerStartsWith")
			{
				yield return this.CurrentAnimalLabel().CapitalizeFirst() + " x" + this.count;
			}
			yield break;
		}

		// Token: 0x06007AC4 RID: 31428 RVA: 0x0024F660 File Offset: 0x0024D860
		public override void Randomize()
		{
			if (Rand.Value < 0.5f)
			{
				this.animalKind = null;
			}
			else
			{
				this.animalKind = this.PossibleAnimals(false).RandomElement<PawnKindDef>();
			}
			this.count = ScenPart_StartingAnimal.PetCountChances.RandomElementByWeight((Pair<int, float> pa) => pa.Second).First;
			this.bondToRandomPlayerPawnChance = 0f;
		}

		// Token: 0x06007AC5 RID: 31429 RVA: 0x0024F6D8 File Offset: 0x0024D8D8
		public override bool TryMerge(ScenPart other)
		{
			ScenPart_StartingAnimal scenPart_StartingAnimal = other as ScenPart_StartingAnimal;
			if (scenPart_StartingAnimal != null && scenPart_StartingAnimal.animalKind == this.animalKind)
			{
				this.count += scenPart_StartingAnimal.count;
				return true;
			}
			return false;
		}

		// Token: 0x06007AC6 RID: 31430 RVA: 0x000528C1 File Offset: 0x00050AC1
		public override IEnumerable<Thing> PlayerStartingThings()
		{
			int num;
			for (int i = 0; i < this.count; i = num + 1)
			{
				PawnKindDef kindDef;
				if (this.animalKind != null)
				{
					kindDef = this.animalKind;
				}
				else
				{
					kindDef = this.RandomPets().RandomElementByWeight((PawnKindDef td) => td.RaceProps.petness);
				}
				Pawn animal = PawnGenerator.GeneratePawn(kindDef, Faction.OfPlayer);
				if (animal.Name == null || animal.Name.Numerical)
				{
					animal.Name = PawnBioAndNameGenerator.GeneratePawnName(animal, NameStyle.Full, null);
				}
				if (Rand.Value < this.bondToRandomPlayerPawnChance && animal.training.CanAssignToTrain(TrainableDefOf.Obedience).Accepted)
				{
					Pawn pawn = (from p in Find.GameInitData.startingAndOptionalPawns.Take(Find.GameInitData.startingPawnCount)
					where TrainableUtility.CanBeMaster(p, animal, false) && !p.story.traits.HasTrait(TraitDefOf.Psychopath)
					select p).RandomElementWithFallback(null);
					if (pawn != null)
					{
						animal.training.Train(TrainableDefOf.Obedience, null, true);
						animal.training.SetWantedRecursive(TrainableDefOf.Obedience, true);
						pawn.relations.AddDirectRelation(PawnRelationDefOf.Bond, animal);
						animal.playerSettings.Master = pawn;
					}
				}
				yield return animal;
				num = i;
			}
			yield break;
		}

		// Token: 0x0400506D RID: 20589
		private PawnKindDef animalKind;

		// Token: 0x0400506E RID: 20590
		private int count = 1;

		// Token: 0x0400506F RID: 20591
		private float bondToRandomPlayerPawnChance = 0.5f;

		// Token: 0x04005070 RID: 20592
		private string countBuf;

		// Token: 0x04005071 RID: 20593
		private static readonly List<Pair<int, float>> PetCountChances = new List<Pair<int, float>>
		{
			new Pair<int, float>(1, 20f),
			new Pair<int, float>(2, 10f),
			new Pair<int, float>(3, 5f),
			new Pair<int, float>(4, 3f),
			new Pair<int, float>(5, 1f),
			new Pair<int, float>(6, 1f),
			new Pair<int, float>(7, 1f),
			new Pair<int, float>(8, 1f),
			new Pair<int, float>(9, 1f),
			new Pair<int, float>(10, 0.1f),
			new Pair<int, float>(11, 0.1f),
			new Pair<int, float>(12, 0.1f),
			new Pair<int, float>(13, 0.1f),
			new Pair<int, float>(14, 0.1f)
		};
	}
}
