using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200143A RID: 5178
	[DefOf]
	public static class RulePackDefOf
	{
		// Token: 0x06007D2D RID: 32045 RVA: 0x002C4919 File Offset: 0x002C2B19
		static RulePackDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(RulePackDefOf));
		}

		// Token: 0x04004C3E RID: 19518
		public static RulePackDef Sentence_SocialFightStarted;

		// Token: 0x04004C3F RID: 19519
		public static RulePackDef Sentence_RomanceAttemptAccepted;

		// Token: 0x04004C40 RID: 19520
		public static RulePackDef Sentence_RomanceAttemptRejected;

		// Token: 0x04004C41 RID: 19521
		public static RulePackDef Sentence_MarriageProposalAccepted;

		// Token: 0x04004C42 RID: 19522
		public static RulePackDef Sentence_MarriageProposalRejected;

		// Token: 0x04004C43 RID: 19523
		public static RulePackDef Sentence_MarriageProposalRejectedBrokeUp;

		// Token: 0x04004C44 RID: 19524
		public static RulePackDef Sentence_RecruitAttemptAccepted;

		// Token: 0x04004C45 RID: 19525
		public static RulePackDef Sentence_RecruitAttemptRejected;

		// Token: 0x04004C46 RID: 19526
		[MayRequireIdeology]
		public static RulePackDef Sentence_ConvertIdeoAttemptSuccess;

		// Token: 0x04004C47 RID: 19527
		[MayRequireIdeology]
		public static RulePackDef Sentence_ConvertIdeoAttemptFail;

		// Token: 0x04004C48 RID: 19528
		[MayRequireIdeology]
		public static RulePackDef Sentence_ConvertIdeoAttemptFailSocialFight;

		// Token: 0x04004C49 RID: 19529
		[MayRequireIdeology]
		public static RulePackDef Sentence_ConvertIdeoAttemptFailResentment;

		// Token: 0x04004C4A RID: 19530
		public static RulePackDef ArtDescriptionRoot_HasTale;

		// Token: 0x04004C4B RID: 19531
		public static RulePackDef ArtDescriptionRoot_Taleless;

		// Token: 0x04004C4C RID: 19532
		public static RulePackDef ArtDescriptionUtility_Global;

		// Token: 0x04004C4D RID: 19533
		public static RulePackDef GlobalUtility;

		// Token: 0x04004C4E RID: 19534
		public static RulePackDef TalelessImages;

		// Token: 0x04004C4F RID: 19535
		public static RulePackDef NamerWorld;

		// Token: 0x04004C50 RID: 19536
		public static RulePackDef NamerTraderGeneral;

		// Token: 0x04004C51 RID: 19537
		public static RulePackDef NamerScenario;

		// Token: 0x04004C52 RID: 19538
		public static RulePackDef NamerQuestDefault;

		// Token: 0x04004C53 RID: 19539
		public static RulePackDef NamerArtSculpture;

		// Token: 0x04004C54 RID: 19540
		public static RulePackDef ArtDescription_Sculpture;

		// Token: 0x04004C55 RID: 19541
		public static RulePackDef NamerArtWeaponMelee;

		// Token: 0x04004C56 RID: 19542
		public static RulePackDef ArtDescription_WeaponMelee;

		// Token: 0x04004C57 RID: 19543
		public static RulePackDef NamerArtWeaponGun;

		// Token: 0x04004C58 RID: 19544
		public static RulePackDef ArtDescription_WeaponGun;

		// Token: 0x04004C59 RID: 19545
		public static RulePackDef NamerArtFurniture;

		// Token: 0x04004C5A RID: 19546
		public static RulePackDef ArtDescription_Furniture;

		// Token: 0x04004C5B RID: 19547
		public static RulePackDef NamerArtSarcophagusPlate;

		// Token: 0x04004C5C RID: 19548
		public static RulePackDef ArtDescription_SarcophagusPlate;

		// Token: 0x04004C5D RID: 19549
		public static RulePackDef SeedGenerator;

		// Token: 0x04004C5E RID: 19550
		public static RulePackDef GameplayTips;

		// Token: 0x04004C5F RID: 19551
		public static RulePackDef Combat_RangedFire;

		// Token: 0x04004C60 RID: 19552
		public static RulePackDef Combat_RangedDamage;

		// Token: 0x04004C61 RID: 19553
		public static RulePackDef Combat_RangedDeflect;

		// Token: 0x04004C62 RID: 19554
		public static RulePackDef Combat_RangedMiss;

		// Token: 0x04004C63 RID: 19555
		public static RulePackDef Combat_ExplosionImpact;

		// Token: 0x04004C64 RID: 19556
		public static RulePackDef Transition_Downed;

		// Token: 0x04004C65 RID: 19557
		public static RulePackDef Transition_Died;

		// Token: 0x04004C66 RID: 19558
		public static RulePackDef Transition_DiedExplosive;

		// Token: 0x04004C67 RID: 19559
		public static RulePackDef DamageEvent_Ceiling;

		// Token: 0x04004C68 RID: 19560
		public static RulePackDef DamageEvent_Fire;

		// Token: 0x04004C69 RID: 19561
		public static RulePackDef DamageEvent_PowerBeam;

		// Token: 0x04004C6A RID: 19562
		public static RulePackDef DamageEvent_Tornado;

		// Token: 0x04004C6B RID: 19563
		public static RulePackDef DamageEvent_TrapSpike;

		// Token: 0x04004C6C RID: 19564
		public static RulePackDef Event_Stun;

		// Token: 0x04004C6D RID: 19565
		public static RulePackDef Event_AbilityUsed;

		// Token: 0x04004C6E RID: 19566
		public static RulePackDef Event_ItemUsed;

		// Token: 0x04004C6F RID: 19567
		public static RulePackDef Battle_Solo;

		// Token: 0x04004C70 RID: 19568
		public static RulePackDef Battle_Duel;

		// Token: 0x04004C71 RID: 19569
		public static RulePackDef Battle_Internal;

		// Token: 0x04004C72 RID: 19570
		public static RulePackDef Battle_War;

		// Token: 0x04004C73 RID: 19571
		public static RulePackDef Battle_Brawl;

		// Token: 0x04004C74 RID: 19572
		public static RulePackDef DynamicWrapper;
	}
}
