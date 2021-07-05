using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001011 RID: 4113
	public class ScenPart_StartingAnimal : ScenPart
	{
		// Token: 0x060060F2 RID: 24818 RVA: 0x0020F47D File Offset: 0x0020D67D
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<PawnKindDef>(ref this.animalKind, "animalKind");
			Scribe_Values.Look<int>(ref this.count, "count", 0, false);
			Scribe_Values.Look<float>(ref this.bondToRandomPlayerPawnChance, "bondToRandomPlayerPawnChance", 0f, false);
		}

		// Token: 0x060060F3 RID: 24819 RVA: 0x0020F4C0 File Offset: 0x0020D6C0
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
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				foreach (PawnKindDef localKind2 in this.PossibleAnimals(false))
				{
					PawnKindDef localKind = localKind2;
					list.Add(new FloatMenuOption(localKind.LabelCap, delegate()
					{
						this.animalKind = localKind;
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}
		}

		// Token: 0x060060F4 RID: 24820 RVA: 0x0020F618 File Offset: 0x0020D818
		private IEnumerable<PawnKindDef> PossibleAnimals(bool checkForTamer = true)
		{
			return from td in DefDatabase<PawnKindDef>.AllDefs
			where td.RaceProps.Animal && (!checkForTamer || ScenPart_StartingAnimal.CanKeepPetTame(td))
			select td;
		}

		// Token: 0x060060F5 RID: 24821 RVA: 0x0020F648 File Offset: 0x0020D848
		private static bool CanKeepPetTame(PawnKindDef def)
		{
			float level = (float)Find.GameInitData.startingAndOptionalPawns.Take(Find.GameInitData.startingPawnCount).MaxBy((Pawn c) => c.skills.GetSkill(SkillDefOf.Animals).Level).skills.GetSkill(SkillDefOf.Animals).Level;
			float statValueAbstract = def.race.GetStatValueAbstract(StatDefOf.MinimumHandlingSkill, null);
			return level >= statValueAbstract;
		}

		// Token: 0x060060F6 RID: 24822 RVA: 0x0020F6BF File Offset: 0x0020D8BF
		private IEnumerable<PawnKindDef> RandomPets()
		{
			return from td in this.PossibleAnimals(true)
			where td.RaceProps.petness > 0f
			select td;
		}

		// Token: 0x060060F7 RID: 24823 RVA: 0x0020F6EC File Offset: 0x0020D8EC
		private string CurrentAnimalLabel()
		{
			if (this.animalKind == null)
			{
				return "RandomPet".TranslateSimple();
			}
			return this.animalKind.label;
		}

		// Token: 0x060060F8 RID: 24824 RVA: 0x0020F70C File Offset: 0x0020D90C
		public override string Summary(Scenario scen)
		{
			return ScenSummaryList.SummaryWithList(scen, "PlayerStartsWith", ScenPart_StartingThing_Defined.PlayerStartWithIntro);
		}

		// Token: 0x060060F9 RID: 24825 RVA: 0x0020F71E File Offset: 0x0020D91E
		public override IEnumerable<string> GetSummaryListEntries(string tag)
		{
			if (tag == "PlayerStartsWith")
			{
				yield return this.CurrentAnimalLabel().CapitalizeFirst() + " x" + this.count;
			}
			yield break;
		}

		// Token: 0x060060FA RID: 24826 RVA: 0x0020F738 File Offset: 0x0020D938
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

		// Token: 0x060060FB RID: 24827 RVA: 0x0020F7B0 File Offset: 0x0020D9B0
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

		// Token: 0x060060FC RID: 24828 RVA: 0x0020F7EC File Offset: 0x0020D9EC
		private float PetWeight(PawnKindDef animal)
		{
			FactionIdeosTracker ideos = Find.GameInitData.playerFaction.ideos;
			Ideo ideo = (ideos != null) ? ideos.PrimaryIdeo : null;
			if (ideo != null)
			{
				using (List<ThingDef>.Enumerator enumerator = ideo.VeneratedAnimals.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current == animal.race)
						{
							return 8f;
						}
					}
				}
			}
			return animal.RaceProps.petness;
		}

		// Token: 0x060060FD RID: 24829 RVA: 0x0020F874 File Offset: 0x0020DA74
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
					kindDef = this.RandomPets().RandomElementByWeight((PawnKindDef td) => this.PetWeight(td));
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

		// Token: 0x04003757 RID: 14167
		private PawnKindDef animalKind;

		// Token: 0x04003758 RID: 14168
		private int count = 1;

		// Token: 0x04003759 RID: 14169
		private float bondToRandomPlayerPawnChance = 0.5f;

		// Token: 0x0400375A RID: 14170
		private string countBuf;

		// Token: 0x0400375B RID: 14171
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

		// Token: 0x0400375C RID: 14172
		public const float VeneratedAnimalWeight = 8f;
	}
}
