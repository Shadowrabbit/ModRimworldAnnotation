using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x0200044C RID: 1100
	public struct PawnGenerationRequest
	{
		// Token: 0x17000528 RID: 1320
		// (get) Token: 0x06001B52 RID: 6994 RVA: 0x00018FC4 File Offset: 0x000171C4
		// (set) Token: 0x06001B53 RID: 6995 RVA: 0x00018FCC File Offset: 0x000171CC
		public PawnKindDef KindDef { get; set; }

		// Token: 0x17000529 RID: 1321
		// (get) Token: 0x06001B54 RID: 6996 RVA: 0x00018FD5 File Offset: 0x000171D5
		// (set) Token: 0x06001B55 RID: 6997 RVA: 0x00018FDD File Offset: 0x000171DD
		public PawnGenerationContext Context { get; set; }

		// Token: 0x1700052A RID: 1322
		// (get) Token: 0x06001B56 RID: 6998 RVA: 0x00018FE6 File Offset: 0x000171E6
		// (set) Token: 0x06001B57 RID: 6999 RVA: 0x00018FEE File Offset: 0x000171EE
		public Faction Faction { get; set; }

		// Token: 0x1700052B RID: 1323
		// (get) Token: 0x06001B58 RID: 7000 RVA: 0x00018FF7 File Offset: 0x000171F7
		// (set) Token: 0x06001B59 RID: 7001 RVA: 0x00018FFF File Offset: 0x000171FF
		public int Tile { get; set; }

		// Token: 0x1700052C RID: 1324
		// (get) Token: 0x06001B5A RID: 7002 RVA: 0x00019008 File Offset: 0x00017208
		// (set) Token: 0x06001B5B RID: 7003 RVA: 0x00019010 File Offset: 0x00017210
		public bool ForceGenerateNewPawn { get; set; }

		// Token: 0x1700052D RID: 1325
		// (get) Token: 0x06001B5C RID: 7004 RVA: 0x00019019 File Offset: 0x00017219
		// (set) Token: 0x06001B5D RID: 7005 RVA: 0x00019021 File Offset: 0x00017221
		public bool Newborn { get; set; }

		// Token: 0x1700052E RID: 1326
		// (get) Token: 0x06001B5E RID: 7006 RVA: 0x0001902A File Offset: 0x0001722A
		// (set) Token: 0x06001B5F RID: 7007 RVA: 0x00019032 File Offset: 0x00017232
		public BodyTypeDef ForceBodyType { get; set; }

		// Token: 0x1700052F RID: 1327
		// (get) Token: 0x06001B60 RID: 7008 RVA: 0x0001903B File Offset: 0x0001723B
		// (set) Token: 0x06001B61 RID: 7009 RVA: 0x00019043 File Offset: 0x00017243
		public bool AllowDead { get; set; }

		// Token: 0x17000530 RID: 1328
		// (get) Token: 0x06001B62 RID: 7010 RVA: 0x0001904C File Offset: 0x0001724C
		// (set) Token: 0x06001B63 RID: 7011 RVA: 0x00019054 File Offset: 0x00017254
		public bool AllowDowned { get; set; }

		// Token: 0x17000531 RID: 1329
		// (get) Token: 0x06001B64 RID: 7012 RVA: 0x0001905D File Offset: 0x0001725D
		// (set) Token: 0x06001B65 RID: 7013 RVA: 0x00019065 File Offset: 0x00017265
		public bool CanGeneratePawnRelations { get; set; }

		// Token: 0x17000532 RID: 1330
		// (get) Token: 0x06001B66 RID: 7014 RVA: 0x0001906E File Offset: 0x0001726E
		// (set) Token: 0x06001B67 RID: 7015 RVA: 0x00019076 File Offset: 0x00017276
		public bool MustBeCapableOfViolence { get; set; }

		// Token: 0x17000533 RID: 1331
		// (get) Token: 0x06001B68 RID: 7016 RVA: 0x0001907F File Offset: 0x0001727F
		// (set) Token: 0x06001B69 RID: 7017 RVA: 0x00019087 File Offset: 0x00017287
		public float ColonistRelationChanceFactor { get; set; }

		// Token: 0x17000534 RID: 1332
		// (get) Token: 0x06001B6A RID: 7018 RVA: 0x00019090 File Offset: 0x00017290
		// (set) Token: 0x06001B6B RID: 7019 RVA: 0x00019098 File Offset: 0x00017298
		public bool ForceAddFreeWarmLayerIfNeeded { get; set; }

		// Token: 0x17000535 RID: 1333
		// (get) Token: 0x06001B6C RID: 7020 RVA: 0x000190A1 File Offset: 0x000172A1
		// (set) Token: 0x06001B6D RID: 7021 RVA: 0x000190A9 File Offset: 0x000172A9
		public bool AllowGay { get; set; }

		// Token: 0x17000536 RID: 1334
		// (get) Token: 0x06001B6E RID: 7022 RVA: 0x000190B2 File Offset: 0x000172B2
		// (set) Token: 0x06001B6F RID: 7023 RVA: 0x000190BA File Offset: 0x000172BA
		public bool AllowFood { get; set; }

		// Token: 0x17000537 RID: 1335
		// (get) Token: 0x06001B70 RID: 7024 RVA: 0x000190C3 File Offset: 0x000172C3
		// (set) Token: 0x06001B71 RID: 7025 RVA: 0x000190CB File Offset: 0x000172CB
		public bool AllowAddictions { get; set; }

		// Token: 0x17000538 RID: 1336
		// (get) Token: 0x06001B72 RID: 7026 RVA: 0x000190D4 File Offset: 0x000172D4
		// (set) Token: 0x06001B73 RID: 7027 RVA: 0x000190DC File Offset: 0x000172DC
		public IEnumerable<TraitDef> ForcedTraits { get; set; }

		// Token: 0x17000539 RID: 1337
		// (get) Token: 0x06001B74 RID: 7028 RVA: 0x000190E5 File Offset: 0x000172E5
		// (set) Token: 0x06001B75 RID: 7029 RVA: 0x000190ED File Offset: 0x000172ED
		public IEnumerable<TraitDef> ProhibitedTraits { get; set; }

		// Token: 0x1700053A RID: 1338
		// (get) Token: 0x06001B76 RID: 7030 RVA: 0x000190F6 File Offset: 0x000172F6
		// (set) Token: 0x06001B77 RID: 7031 RVA: 0x000190FE File Offset: 0x000172FE
		public bool Inhabitant { get; set; }

		// Token: 0x1700053B RID: 1339
		// (get) Token: 0x06001B78 RID: 7032 RVA: 0x00019107 File Offset: 0x00017307
		// (set) Token: 0x06001B79 RID: 7033 RVA: 0x0001910F File Offset: 0x0001730F
		public bool CertainlyBeenInCryptosleep { get; set; }

		// Token: 0x1700053C RID: 1340
		// (get) Token: 0x06001B7A RID: 7034 RVA: 0x00019118 File Offset: 0x00017318
		// (set) Token: 0x06001B7B RID: 7035 RVA: 0x00019120 File Offset: 0x00017320
		public bool ForceRedressWorldPawnIfFormerColonist { get; set; }

		// Token: 0x1700053D RID: 1341
		// (get) Token: 0x06001B7C RID: 7036 RVA: 0x00019129 File Offset: 0x00017329
		// (set) Token: 0x06001B7D RID: 7037 RVA: 0x00019131 File Offset: 0x00017331
		public bool WorldPawnFactionDoesntMatter { get; set; }

		// Token: 0x1700053E RID: 1342
		// (get) Token: 0x06001B7E RID: 7038 RVA: 0x0001913A File Offset: 0x0001733A
		// (set) Token: 0x06001B7F RID: 7039 RVA: 0x00019142 File Offset: 0x00017342
		public float BiocodeWeaponChance { get; set; }

		// Token: 0x1700053F RID: 1343
		// (get) Token: 0x06001B80 RID: 7040 RVA: 0x0001914B File Offset: 0x0001734B
		// (set) Token: 0x06001B81 RID: 7041 RVA: 0x00019153 File Offset: 0x00017353
		public float BiocodeApparelChance { get; set; }

		// Token: 0x17000540 RID: 1344
		// (get) Token: 0x06001B82 RID: 7042 RVA: 0x0001915C File Offset: 0x0001735C
		// (set) Token: 0x06001B83 RID: 7043 RVA: 0x00019164 File Offset: 0x00017364
		public Pawn ExtraPawnForExtraRelationChance { get; set; }

		// Token: 0x17000541 RID: 1345
		// (get) Token: 0x06001B84 RID: 7044 RVA: 0x0001916D File Offset: 0x0001736D
		// (set) Token: 0x06001B85 RID: 7045 RVA: 0x00019175 File Offset: 0x00017375
		public float RelationWithExtraPawnChanceFactor { get; set; }

		// Token: 0x17000542 RID: 1346
		// (get) Token: 0x06001B86 RID: 7046 RVA: 0x0001917E File Offset: 0x0001737E
		// (set) Token: 0x06001B87 RID: 7047 RVA: 0x00019186 File Offset: 0x00017386
		public Predicate<Pawn> RedressValidator { get; set; }

		// Token: 0x17000543 RID: 1347
		// (get) Token: 0x06001B88 RID: 7048 RVA: 0x0001918F File Offset: 0x0001738F
		// (set) Token: 0x06001B89 RID: 7049 RVA: 0x00019197 File Offset: 0x00017397
		public Predicate<Pawn> ValidatorPreGear { get; set; }

		// Token: 0x17000544 RID: 1348
		// (get) Token: 0x06001B8A RID: 7050 RVA: 0x000191A0 File Offset: 0x000173A0
		// (set) Token: 0x06001B8B RID: 7051 RVA: 0x000191A8 File Offset: 0x000173A8
		public Predicate<Pawn> ValidatorPostGear { get; set; }

		// Token: 0x17000545 RID: 1349
		// (get) Token: 0x06001B8C RID: 7052 RVA: 0x000191B1 File Offset: 0x000173B1
		// (set) Token: 0x06001B8D RID: 7053 RVA: 0x000191B9 File Offset: 0x000173B9
		public float? MinChanceToRedressWorldPawn { get; set; }

		// Token: 0x17000546 RID: 1350
		// (get) Token: 0x06001B8E RID: 7054 RVA: 0x000191C2 File Offset: 0x000173C2
		// (set) Token: 0x06001B8F RID: 7055 RVA: 0x000191CA File Offset: 0x000173CA
		public float? FixedBiologicalAge { get; set; }

		// Token: 0x17000547 RID: 1351
		// (get) Token: 0x06001B90 RID: 7056 RVA: 0x000191D3 File Offset: 0x000173D3
		// (set) Token: 0x06001B91 RID: 7057 RVA: 0x000191DB File Offset: 0x000173DB
		public float? FixedChronologicalAge { get; set; }

		// Token: 0x17000548 RID: 1352
		// (get) Token: 0x06001B92 RID: 7058 RVA: 0x000191E4 File Offset: 0x000173E4
		// (set) Token: 0x06001B93 RID: 7059 RVA: 0x000191EC File Offset: 0x000173EC
		public Gender? FixedGender { get; set; }

		// Token: 0x17000549 RID: 1353
		// (get) Token: 0x06001B94 RID: 7060 RVA: 0x000191F5 File Offset: 0x000173F5
		// (set) Token: 0x06001B95 RID: 7061 RVA: 0x000191FD File Offset: 0x000173FD
		public float? FixedMelanin { get; set; }

		// Token: 0x1700054A RID: 1354
		// (get) Token: 0x06001B96 RID: 7062 RVA: 0x00019206 File Offset: 0x00017406
		// (set) Token: 0x06001B97 RID: 7063 RVA: 0x0001920E File Offset: 0x0001740E
		public string FixedLastName { get; set; }

		// Token: 0x1700054B RID: 1355
		// (get) Token: 0x06001B98 RID: 7064 RVA: 0x00019217 File Offset: 0x00017417
		// (set) Token: 0x06001B99 RID: 7065 RVA: 0x0001921F File Offset: 0x0001741F
		public string FixedBirthName { get; set; }

		// Token: 0x1700054C RID: 1356
		// (get) Token: 0x06001B9A RID: 7066 RVA: 0x00019228 File Offset: 0x00017428
		// (set) Token: 0x06001B9B RID: 7067 RVA: 0x00019230 File Offset: 0x00017430
		public RoyalTitleDef FixedTitle { get; set; }

		// Token: 0x1700054D RID: 1357
		// (get) Token: 0x06001B9C RID: 7068 RVA: 0x00019239 File Offset: 0x00017439
		// (set) Token: 0x06001B9D RID: 7069 RVA: 0x00019241 File Offset: 0x00017441
		public bool ForbidAnyTitle { get; set; }

		// Token: 0x06001B9E RID: 7070 RVA: 0x000ECEBC File Offset: 0x000EB0BC
		public PawnGenerationRequest(PawnKindDef kind, Faction faction = null, PawnGenerationContext context = PawnGenerationContext.NonPlayer, int tile = -1, bool forceGenerateNewPawn = false, bool newborn = false, bool allowDead = false, bool allowDowned = false, bool canGeneratePawnRelations = true, bool mustBeCapableOfViolence = false, float colonistRelationChanceFactor = 1f, bool forceAddFreeWarmLayerIfNeeded = false, bool allowGay = true, bool allowFood = true, bool allowAddictions = true, bool inhabitant = false, bool certainlyBeenInCryptosleep = false, bool forceRedressWorldPawnIfFormerColonist = false, bool worldPawnFactionDoesntMatter = false, float biocodeWeaponChance = 0f, Pawn extraPawnForExtraRelationChance = null, float relationWithExtraPawnChanceFactor = 1f, Predicate<Pawn> validatorPreGear = null, Predicate<Pawn> validatorPostGear = null, IEnumerable<TraitDef> forcedTraits = null, IEnumerable<TraitDef> prohibitedTraits = null, float? minChanceToRedressWorldPawn = null, float? fixedBiologicalAge = null, float? fixedChronologicalAge = null, Gender? fixedGender = null, float? fixedMelanin = null, string fixedLastName = null, string fixedBirthName = null, RoyalTitleDef fixedTitle = null)
		{
			this = default(PawnGenerationRequest);
			if (context == PawnGenerationContext.All)
			{
				Log.Error("Should not generate pawns with context 'All'", false);
				context = PawnGenerationContext.NonPlayer;
			}
			if (inhabitant && (tile == -1 || Current.Game.FindMap(tile) == null))
			{
				Log.Error("Trying to generate an inhabitant but map is null.", false);
				inhabitant = false;
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
		}

		// Token: 0x06001B9F RID: 7071 RVA: 0x000ED014 File Offset: 0x000EB214
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

		// Token: 0x06001BA0 RID: 7072 RVA: 0x0001924A File Offset: 0x0001744A
		public void SetFixedLastName(string fixedLastName)
		{
			if (this.FixedLastName != null)
			{
				Log.Error("Last name is already a fixed value: " + this.FixedLastName + ".", false);
				return;
			}
			this.FixedLastName = fixedLastName;
		}

		// Token: 0x06001BA1 RID: 7073 RVA: 0x00019277 File Offset: 0x00017477
		public void SetFixedBirthName(string fixedBirthName)
		{
			if (this.FixedBirthName != null)
			{
				Log.Error("birth name is already a fixed value: " + this.FixedBirthName + ".", false);
				return;
			}
			this.FixedBirthName = fixedBirthName;
		}

		// Token: 0x06001BA2 RID: 7074 RVA: 0x000ED074 File Offset: 0x000EB274
		public void SetFixedMelanin(float fixedMelanin)
		{
			if (this.FixedMelanin != null)
			{
				Log.Error("Melanin is already a fixed value: " + this.FixedMelanin + ".", false);
				return;
			}
			this.FixedMelanin = new float?(fixedMelanin);
		}

		// Token: 0x06001BA3 RID: 7075 RVA: 0x000ED0C0 File Offset: 0x000EB2C0
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
