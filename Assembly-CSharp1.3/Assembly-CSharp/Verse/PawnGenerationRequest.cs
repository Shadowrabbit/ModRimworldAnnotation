using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x020002E9 RID: 745
	public struct PawnGenerationRequest
	{
		// Token: 0x17000450 RID: 1104
		// (get) Token: 0x060014E6 RID: 5350 RVA: 0x0007B19E File Offset: 0x0007939E
		// (set) Token: 0x060014E7 RID: 5351 RVA: 0x0007B1A6 File Offset: 0x000793A6
		public PawnKindDef KindDef { get; set; }

		// Token: 0x17000451 RID: 1105
		// (get) Token: 0x060014E8 RID: 5352 RVA: 0x0007B1AF File Offset: 0x000793AF
		// (set) Token: 0x060014E9 RID: 5353 RVA: 0x0007B1B7 File Offset: 0x000793B7
		public PawnGenerationContext Context { get; set; }

		// Token: 0x17000452 RID: 1106
		// (get) Token: 0x060014EA RID: 5354 RVA: 0x0007B1C0 File Offset: 0x000793C0
		// (set) Token: 0x060014EB RID: 5355 RVA: 0x0007B1C8 File Offset: 0x000793C8
		public Faction Faction { get; set; }

		// Token: 0x17000453 RID: 1107
		// (get) Token: 0x060014EC RID: 5356 RVA: 0x0007B1D1 File Offset: 0x000793D1
		// (set) Token: 0x060014ED RID: 5357 RVA: 0x0007B1D9 File Offset: 0x000793D9
		public int Tile { get; set; }

		// Token: 0x17000454 RID: 1108
		// (get) Token: 0x060014EE RID: 5358 RVA: 0x0007B1E2 File Offset: 0x000793E2
		// (set) Token: 0x060014EF RID: 5359 RVA: 0x0007B1EA File Offset: 0x000793EA
		public bool ForceGenerateNewPawn { get; set; }

		// Token: 0x17000455 RID: 1109
		// (get) Token: 0x060014F0 RID: 5360 RVA: 0x0007B1F3 File Offset: 0x000793F3
		// (set) Token: 0x060014F1 RID: 5361 RVA: 0x0007B1FB File Offset: 0x000793FB
		public bool Newborn { get; set; }

		// Token: 0x17000456 RID: 1110
		// (get) Token: 0x060014F2 RID: 5362 RVA: 0x0007B204 File Offset: 0x00079404
		// (set) Token: 0x060014F3 RID: 5363 RVA: 0x0007B20C File Offset: 0x0007940C
		public BodyTypeDef ForceBodyType { get; set; }

		// Token: 0x17000457 RID: 1111
		// (get) Token: 0x060014F4 RID: 5364 RVA: 0x0007B215 File Offset: 0x00079415
		// (set) Token: 0x060014F5 RID: 5365 RVA: 0x0007B21D File Offset: 0x0007941D
		public bool AllowDead { get; set; }

		// Token: 0x17000458 RID: 1112
		// (get) Token: 0x060014F6 RID: 5366 RVA: 0x0007B226 File Offset: 0x00079426
		// (set) Token: 0x060014F7 RID: 5367 RVA: 0x0007B22E File Offset: 0x0007942E
		public bool AllowDowned { get; set; }

		// Token: 0x17000459 RID: 1113
		// (get) Token: 0x060014F8 RID: 5368 RVA: 0x0007B237 File Offset: 0x00079437
		// (set) Token: 0x060014F9 RID: 5369 RVA: 0x0007B23F File Offset: 0x0007943F
		public bool CanGeneratePawnRelations { get; set; }

		// Token: 0x1700045A RID: 1114
		// (get) Token: 0x060014FA RID: 5370 RVA: 0x0007B248 File Offset: 0x00079448
		// (set) Token: 0x060014FB RID: 5371 RVA: 0x0007B250 File Offset: 0x00079450
		public bool MustBeCapableOfViolence { get; set; }

		// Token: 0x1700045B RID: 1115
		// (get) Token: 0x060014FC RID: 5372 RVA: 0x0007B259 File Offset: 0x00079459
		// (set) Token: 0x060014FD RID: 5373 RVA: 0x0007B261 File Offset: 0x00079461
		public float ColonistRelationChanceFactor { get; set; }

		// Token: 0x1700045C RID: 1116
		// (get) Token: 0x060014FE RID: 5374 RVA: 0x0007B26A File Offset: 0x0007946A
		// (set) Token: 0x060014FF RID: 5375 RVA: 0x0007B272 File Offset: 0x00079472
		public bool ForceAddFreeWarmLayerIfNeeded { get; set; }

		// Token: 0x1700045D RID: 1117
		// (get) Token: 0x06001500 RID: 5376 RVA: 0x0007B27B File Offset: 0x0007947B
		// (set) Token: 0x06001501 RID: 5377 RVA: 0x0007B283 File Offset: 0x00079483
		public bool AllowGay { get; set; }

		// Token: 0x1700045E RID: 1118
		// (get) Token: 0x06001502 RID: 5378 RVA: 0x0007B28C File Offset: 0x0007948C
		// (set) Token: 0x06001503 RID: 5379 RVA: 0x0007B294 File Offset: 0x00079494
		public bool AllowFood { get; set; }

		// Token: 0x1700045F RID: 1119
		// (get) Token: 0x06001504 RID: 5380 RVA: 0x0007B29D File Offset: 0x0007949D
		// (set) Token: 0x06001505 RID: 5381 RVA: 0x0007B2A5 File Offset: 0x000794A5
		public bool AllowAddictions { get; set; }

		// Token: 0x17000460 RID: 1120
		// (get) Token: 0x06001506 RID: 5382 RVA: 0x0007B2AE File Offset: 0x000794AE
		// (set) Token: 0x06001507 RID: 5383 RVA: 0x0007B2B6 File Offset: 0x000794B6
		public IEnumerable<TraitDef> ForcedTraits { get; set; }

		// Token: 0x17000461 RID: 1121
		// (get) Token: 0x06001508 RID: 5384 RVA: 0x0007B2BF File Offset: 0x000794BF
		// (set) Token: 0x06001509 RID: 5385 RVA: 0x0007B2C7 File Offset: 0x000794C7
		public IEnumerable<TraitDef> ProhibitedTraits { get; set; }

		// Token: 0x17000462 RID: 1122
		// (get) Token: 0x0600150A RID: 5386 RVA: 0x0007B2D0 File Offset: 0x000794D0
		// (set) Token: 0x0600150B RID: 5387 RVA: 0x0007B2D8 File Offset: 0x000794D8
		public bool Inhabitant { get; set; }

		// Token: 0x17000463 RID: 1123
		// (get) Token: 0x0600150C RID: 5388 RVA: 0x0007B2E1 File Offset: 0x000794E1
		// (set) Token: 0x0600150D RID: 5389 RVA: 0x0007B2E9 File Offset: 0x000794E9
		public bool CertainlyBeenInCryptosleep { get; set; }

		// Token: 0x17000464 RID: 1124
		// (get) Token: 0x0600150E RID: 5390 RVA: 0x0007B2F2 File Offset: 0x000794F2
		// (set) Token: 0x0600150F RID: 5391 RVA: 0x0007B2FA File Offset: 0x000794FA
		public bool ForceRedressWorldPawnIfFormerColonist { get; set; }

		// Token: 0x17000465 RID: 1125
		// (get) Token: 0x06001510 RID: 5392 RVA: 0x0007B303 File Offset: 0x00079503
		// (set) Token: 0x06001511 RID: 5393 RVA: 0x0007B30B File Offset: 0x0007950B
		public bool WorldPawnFactionDoesntMatter { get; set; }

		// Token: 0x17000466 RID: 1126
		// (get) Token: 0x06001512 RID: 5394 RVA: 0x0007B314 File Offset: 0x00079514
		// (set) Token: 0x06001513 RID: 5395 RVA: 0x0007B31C File Offset: 0x0007951C
		public float BiocodeWeaponChance { get; set; }

		// Token: 0x17000467 RID: 1127
		// (get) Token: 0x06001514 RID: 5396 RVA: 0x0007B325 File Offset: 0x00079525
		// (set) Token: 0x06001515 RID: 5397 RVA: 0x0007B32D File Offset: 0x0007952D
		public float BiocodeApparelChance { get; set; }

		// Token: 0x17000468 RID: 1128
		// (get) Token: 0x06001516 RID: 5398 RVA: 0x0007B336 File Offset: 0x00079536
		// (set) Token: 0x06001517 RID: 5399 RVA: 0x0007B33E File Offset: 0x0007953E
		public Pawn ExtraPawnForExtraRelationChance { get; set; }

		// Token: 0x17000469 RID: 1129
		// (get) Token: 0x06001518 RID: 5400 RVA: 0x0007B347 File Offset: 0x00079547
		// (set) Token: 0x06001519 RID: 5401 RVA: 0x0007B34F File Offset: 0x0007954F
		public float RelationWithExtraPawnChanceFactor { get; set; }

		// Token: 0x1700046A RID: 1130
		// (get) Token: 0x0600151A RID: 5402 RVA: 0x0007B358 File Offset: 0x00079558
		// (set) Token: 0x0600151B RID: 5403 RVA: 0x0007B360 File Offset: 0x00079560
		public Predicate<Pawn> RedressValidator { get; set; }

		// Token: 0x1700046B RID: 1131
		// (get) Token: 0x0600151C RID: 5404 RVA: 0x0007B369 File Offset: 0x00079569
		// (set) Token: 0x0600151D RID: 5405 RVA: 0x0007B371 File Offset: 0x00079571
		public Predicate<Pawn> ValidatorPreGear { get; set; }

		// Token: 0x1700046C RID: 1132
		// (get) Token: 0x0600151E RID: 5406 RVA: 0x0007B37A File Offset: 0x0007957A
		// (set) Token: 0x0600151F RID: 5407 RVA: 0x0007B382 File Offset: 0x00079582
		public Predicate<Pawn> ValidatorPostGear { get; set; }

		// Token: 0x1700046D RID: 1133
		// (get) Token: 0x06001520 RID: 5408 RVA: 0x0007B38B File Offset: 0x0007958B
		// (set) Token: 0x06001521 RID: 5409 RVA: 0x0007B393 File Offset: 0x00079593
		public float? MinChanceToRedressWorldPawn { get; set; }

		// Token: 0x1700046E RID: 1134
		// (get) Token: 0x06001522 RID: 5410 RVA: 0x0007B39C File Offset: 0x0007959C
		// (set) Token: 0x06001523 RID: 5411 RVA: 0x0007B3A4 File Offset: 0x000795A4
		public float? FixedBiologicalAge { get; set; }

		// Token: 0x1700046F RID: 1135
		// (get) Token: 0x06001524 RID: 5412 RVA: 0x0007B3AD File Offset: 0x000795AD
		// (set) Token: 0x06001525 RID: 5413 RVA: 0x0007B3B5 File Offset: 0x000795B5
		public float? FixedChronologicalAge { get; set; }

		// Token: 0x17000470 RID: 1136
		// (get) Token: 0x06001526 RID: 5414 RVA: 0x0007B3BE File Offset: 0x000795BE
		// (set) Token: 0x06001527 RID: 5415 RVA: 0x0007B3C6 File Offset: 0x000795C6
		public Gender? FixedGender { get; set; }

		// Token: 0x17000471 RID: 1137
		// (get) Token: 0x06001528 RID: 5416 RVA: 0x0007B3CF File Offset: 0x000795CF
		// (set) Token: 0x06001529 RID: 5417 RVA: 0x0007B3D7 File Offset: 0x000795D7
		public float? FixedMelanin { get; set; }

		// Token: 0x17000472 RID: 1138
		// (get) Token: 0x0600152A RID: 5418 RVA: 0x0007B3E0 File Offset: 0x000795E0
		// (set) Token: 0x0600152B RID: 5419 RVA: 0x0007B3E8 File Offset: 0x000795E8
		public string FixedLastName { get; set; }

		// Token: 0x17000473 RID: 1139
		// (get) Token: 0x0600152C RID: 5420 RVA: 0x0007B3F1 File Offset: 0x000795F1
		// (set) Token: 0x0600152D RID: 5421 RVA: 0x0007B3F9 File Offset: 0x000795F9
		public string FixedBirthName { get; set; }

		// Token: 0x17000474 RID: 1140
		// (get) Token: 0x0600152E RID: 5422 RVA: 0x0007B402 File Offset: 0x00079602
		// (set) Token: 0x0600152F RID: 5423 RVA: 0x0007B40A File Offset: 0x0007960A
		public RoyalTitleDef FixedTitle { get; set; }

		// Token: 0x17000475 RID: 1141
		// (get) Token: 0x06001530 RID: 5424 RVA: 0x0007B413 File Offset: 0x00079613
		// (set) Token: 0x06001531 RID: 5425 RVA: 0x0007B41B File Offset: 0x0007961B
		public bool ForbidAnyTitle { get; set; }

		// Token: 0x17000476 RID: 1142
		// (get) Token: 0x06001532 RID: 5426 RVA: 0x0007B424 File Offset: 0x00079624
		// (set) Token: 0x06001533 RID: 5427 RVA: 0x0007B42C File Offset: 0x0007962C
		public Ideo FixedIdeo { get; set; }

		// Token: 0x17000477 RID: 1143
		// (get) Token: 0x06001534 RID: 5428 RVA: 0x0007B435 File Offset: 0x00079635
		// (set) Token: 0x06001535 RID: 5429 RVA: 0x0007B43D File Offset: 0x0007963D
		public bool ForceNoIdeo { get; set; }

		// Token: 0x17000478 RID: 1144
		// (get) Token: 0x06001536 RID: 5430 RVA: 0x0007B446 File Offset: 0x00079646
		// (set) Token: 0x06001537 RID: 5431 RVA: 0x0007B44E File Offset: 0x0007964E
		public bool ForceNoBackstory { get; set; }

		// Token: 0x06001538 RID: 5432 RVA: 0x0007B458 File Offset: 0x00079658
		public PawnGenerationRequest(PawnKindDef kind, Faction faction = null, PawnGenerationContext context = PawnGenerationContext.NonPlayer, int tile = -1, bool forceGenerateNewPawn = false, bool newborn = false, bool allowDead = false, bool allowDowned = false, bool canGeneratePawnRelations = true, bool mustBeCapableOfViolence = false, float colonistRelationChanceFactor = 1f, bool forceAddFreeWarmLayerIfNeeded = false, bool allowGay = true, bool allowFood = true, bool allowAddictions = true, bool inhabitant = false, bool certainlyBeenInCryptosleep = false, bool forceRedressWorldPawnIfFormerColonist = false, bool worldPawnFactionDoesntMatter = false, float biocodeWeaponChance = 0f, float biocodeApparelChance = 0f, Pawn extraPawnForExtraRelationChance = null, float relationWithExtraPawnChanceFactor = 1f, Predicate<Pawn> validatorPreGear = null, Predicate<Pawn> validatorPostGear = null, IEnumerable<TraitDef> forcedTraits = null, IEnumerable<TraitDef> prohibitedTraits = null, float? minChanceToRedressWorldPawn = null, float? fixedBiologicalAge = null, float? fixedChronologicalAge = null, Gender? fixedGender = null, float? fixedMelanin = null, string fixedLastName = null, string fixedBirthName = null, RoyalTitleDef fixedTitle = null, Ideo fixedIdeo = null, bool forceNoIdeo = false, bool forceNoBackstory = false)
		{
			this = default(PawnGenerationRequest);
			if (context == PawnGenerationContext.All)
			{
				Log.Error("Should not generate pawns with context 'All'");
				context = PawnGenerationContext.NonPlayer;
			}
			if (inhabitant && (tile == -1 || Current.Game.FindMap(tile) == null))
			{
				Log.Error("Trying to generate an inhabitant but map is null.");
				inhabitant = false;
			}
			if (forceNoIdeo && fixedIdeo != null)
			{
				Log.Error("Trying to generate a pawn with no ideo and a fixed ideo.");
				forceNoIdeo = false;
			}
			this.KindDef = kind;
			this.Context = context;
			this.Faction = faction;
			this.Tile = tile;
			this.ForceGenerateNewPawn = forceGenerateNewPawn;
			this.Newborn = newborn;
			this.AllowDead = allowDead;
			this.AllowDowned = allowDowned;
			this.CanGeneratePawnRelations = canGeneratePawnRelations;
			this.MustBeCapableOfViolence = mustBeCapableOfViolence;
			this.ColonistRelationChanceFactor = colonistRelationChanceFactor;
			this.ForceAddFreeWarmLayerIfNeeded = forceAddFreeWarmLayerIfNeeded;
			this.AllowGay = allowGay;
			this.AllowFood = allowFood;
			this.AllowAddictions = allowAddictions;
			this.ForcedTraits = forcedTraits;
			this.ProhibitedTraits = prohibitedTraits;
			this.Inhabitant = inhabitant;
			this.CertainlyBeenInCryptosleep = certainlyBeenInCryptosleep;
			this.ForceRedressWorldPawnIfFormerColonist = forceRedressWorldPawnIfFormerColonist;
			this.WorldPawnFactionDoesntMatter = worldPawnFactionDoesntMatter;
			this.ExtraPawnForExtraRelationChance = extraPawnForExtraRelationChance;
			this.RelationWithExtraPawnChanceFactor = relationWithExtraPawnChanceFactor;
			this.BiocodeWeaponChance = biocodeWeaponChance;
			this.BiocodeApparelChance = biocodeApparelChance;
			this.ForceNoIdeo = forceNoIdeo;
			this.ForceNoBackstory = forceNoBackstory;
			this.ValidatorPreGear = validatorPreGear;
			this.ValidatorPostGear = validatorPostGear;
			this.MinChanceToRedressWorldPawn = minChanceToRedressWorldPawn;
			this.FixedBiologicalAge = fixedBiologicalAge;
			this.FixedChronologicalAge = fixedChronologicalAge;
			this.FixedGender = fixedGender;
			this.FixedMelanin = fixedMelanin;
			this.FixedLastName = fixedLastName;
			this.FixedBirthName = fixedBirthName;
			this.FixedTitle = fixedTitle;
			this.FixedIdeo = fixedIdeo;
		}

		// Token: 0x06001539 RID: 5433 RVA: 0x0007B5E4 File Offset: 0x000797E4
		public static PawnGenerationRequest MakeDefault()
		{
			return new PawnGenerationRequest
			{
				Context = PawnGenerationContext.NonPlayer,
				Tile = -1,
				CanGeneratePawnRelations = true,
				ColonistRelationChanceFactor = 1f,
				AllowGay = true,
				AllowFood = true,
				AllowAddictions = true,
				RelationWithExtraPawnChanceFactor = 1f
			};
		}

		// Token: 0x0600153A RID: 5434 RVA: 0x0007B642 File Offset: 0x00079842
		public void SetFixedLastName(string fixedLastName)
		{
			if (this.FixedLastName != null)
			{
				Log.Error("Last name is already a fixed value: " + this.FixedLastName + ".");
				return;
			}
			this.FixedLastName = fixedLastName;
		}

		// Token: 0x0600153B RID: 5435 RVA: 0x0007B66E File Offset: 0x0007986E
		public void SetFixedBirthName(string fixedBirthName)
		{
			if (this.FixedBirthName != null)
			{
				Log.Error("birth name is already a fixed value: " + this.FixedBirthName + ".");
				return;
			}
			this.FixedBirthName = fixedBirthName;
		}

		// Token: 0x0600153C RID: 5436 RVA: 0x0007B69C File Offset: 0x0007989C
		public void SetFixedMelanin(float fixedMelanin)
		{
			if (this.FixedMelanin != null)
			{
				Log.Error("Melanin is already a fixed value: " + this.FixedMelanin + ".");
				return;
			}
			this.FixedMelanin = new float?(fixedMelanin);
		}

		// Token: 0x0600153D RID: 5437 RVA: 0x0007B6E8 File Offset: 0x000798E8
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"kindDef=",
				this.KindDef,
				", context=",
				this.Context,
				", faction=",
				this.Faction,
				", tile=",
				this.Tile,
				", forceGenerateNewPawn=",
				this.ForceGenerateNewPawn.ToString(),
				", newborn=",
				this.Newborn.ToString(),
				", allowDead=",
				this.AllowDead.ToString(),
				", allowDowned=",
				this.AllowDowned.ToString(),
				", canGeneratePawnRelations=",
				this.CanGeneratePawnRelations.ToString(),
				", mustBeCapableOfViolence=",
				this.MustBeCapableOfViolence.ToString(),
				", colonistRelationChanceFactor=",
				this.ColonistRelationChanceFactor,
				", forceAddFreeWarmLayerIfNeeded=",
				this.ForceAddFreeWarmLayerIfNeeded.ToString(),
				", allowGay=",
				this.AllowGay.ToString(),
				", prohibitedTraits=",
				this.ProhibitedTraits,
				", allowFood=",
				this.AllowFood.ToString(),
				", allowAddictions=",
				this.AllowAddictions.ToString(),
				", inhabitant=",
				this.Inhabitant.ToString(),
				", certainlyBeenInCryptosleep=",
				this.CertainlyBeenInCryptosleep.ToString(),
				", biocodeWeaponChance=",
				this.BiocodeWeaponChance,
				", validatorPreGear=",
				this.ValidatorPreGear,
				", validatorPostGear=",
				this.ValidatorPostGear,
				", fixedBiologicalAge=",
				this.FixedBiologicalAge,
				", fixedChronologicalAge=",
				this.FixedChronologicalAge,
				", fixedGender=",
				this.FixedGender,
				", fixedMelanin=",
				this.FixedMelanin,
				", fixedLastName=",
				this.FixedLastName,
				", fixedBirthName=",
				this.FixedBirthName
			});
		}
	}
}
