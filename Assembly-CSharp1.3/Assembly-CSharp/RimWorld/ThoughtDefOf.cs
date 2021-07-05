using System;

namespace RimWorld
{
	// Token: 0x0200142D RID: 5165
	[DefOf]
	public static class ThoughtDefOf
	{
		// Token: 0x06007D20 RID: 32032 RVA: 0x002C483C File Offset: 0x002C2A3C
		static ThoughtDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ThoughtDefOf));
		}

		// Token: 0x04004A1C RID: 18972
		public static ThoughtDef AteRawFood;

		// Token: 0x04004A1D RID: 18973
		public static ThoughtDef AteFineMeal;

		// Token: 0x04004A1E RID: 18974
		public static ThoughtDef AteLavishMeal;

		// Token: 0x04004A1F RID: 18975
		public static ThoughtDef AteHumanlikeMeatDirect;

		// Token: 0x04004A20 RID: 18976
		public static ThoughtDef AteHumanlikeMeatDirectCannibal;

		// Token: 0x04004A21 RID: 18977
		public static ThoughtDef AteHumanlikeMeatAsIngredient;

		// Token: 0x04004A22 RID: 18978
		public static ThoughtDef AteHumanlikeMeatAsIngredientCannibal;

		// Token: 0x04004A23 RID: 18979
		public static ThoughtDef AteInsectMeatDirect;

		// Token: 0x04004A24 RID: 18980
		public static ThoughtDef AteInsectMeatAsIngredient;

		// Token: 0x04004A25 RID: 18981
		public static ThoughtDef AteCorpse;

		// Token: 0x04004A26 RID: 18982
		public static ThoughtDef AteRottenFood;

		// Token: 0x04004A27 RID: 18983
		[MayRequireRoyalty]
		public static ThoughtDef AteFoodInappropriateForTitle;

		// Token: 0x04004A28 RID: 18984
		public static ThoughtDef SleptInBedroom;

		// Token: 0x04004A29 RID: 18985
		public static ThoughtDef SleptInBarracks;

		// Token: 0x04004A2A RID: 18986
		public static ThoughtDef SleptOutside;

		// Token: 0x04004A2B RID: 18987
		public static ThoughtDef SleptOnGround;

		// Token: 0x04004A2C RID: 18988
		public static ThoughtDef SleptInCold;

		// Token: 0x04004A2D RID: 18989
		public static ThoughtDef SleptInHeat;

		// Token: 0x04004A2E RID: 18990
		public static ThoughtDef SleepDisturbed;

		// Token: 0x04004A2F RID: 18991
		[MayRequireIdeology]
		public static ThoughtDef SleptInRoomWithSlave;

		// Token: 0x04004A30 RID: 18992
		[MayRequireRoyalty]
		public static ThoughtDef ReignedInThroneroom;

		// Token: 0x04004A31 RID: 18993
		[MayRequireIdeology]
		public static ThoughtDef ObservedTerror;

		// Token: 0x04004A32 RID: 18994
		public static ThoughtDef WasImprisoned;

		// Token: 0x04004A33 RID: 18995
		public static ThoughtDef NewColonyOptimism;

		// Token: 0x04004A34 RID: 18996
		public static ThoughtDef AteWithoutTable;

		// Token: 0x04004A35 RID: 18997
		public static ThoughtDef AteInImpressiveDiningRoom;

		// Token: 0x04004A36 RID: 18998
		public static ThoughtDef JoyActivityInImpressiveRecRoom;

		// Token: 0x04004A37 RID: 18999
		public static ThoughtDef Catharsis;

		// Token: 0x04004A38 RID: 19000
		public static ThoughtDef MyOrganHarvested;

		// Token: 0x04004A39 RID: 19001
		public static ThoughtDef HarvestedOrgan_Bloodlust;

		// Token: 0x04004A3A RID: 19002
		public static ThoughtDef KnowColonistDied;

		// Token: 0x04004A3B RID: 19003
		public static ThoughtDef KnowPrisonerDiedInnocent;

		// Token: 0x04004A3C RID: 19004
		public static ThoughtDef KnowGuestOrganHarvested;

		// Token: 0x04004A3D RID: 19005
		public static ThoughtDef KnowColonistOrganHarvested;

		// Token: 0x04004A3E RID: 19006
		public static ThoughtDef KnowGuestExecuted;

		// Token: 0x04004A3F RID: 19007
		public static ThoughtDef KnowColonistExecuted;

		// Token: 0x04004A40 RID: 19008
		public static ThoughtDef KnowPrisonerSold;

		// Token: 0x04004A41 RID: 19009
		public static ThoughtDef FreedFromSlavery;

		// Token: 0x04004A42 RID: 19010
		public static ThoughtDef ReleasedHealthyPrisoner;

		// Token: 0x04004A43 RID: 19011
		public static ThoughtDef SoakingWet;

		// Token: 0x04004A44 RID: 19012
		public static ThoughtDef SoldMyBondedAnimalMood;

		// Token: 0x04004A45 RID: 19013
		public static ThoughtDef WitnessedDeathNonAlly;

		// Token: 0x04004A46 RID: 19014
		public static ThoughtDef WitnessedDeathAlly;

		// Token: 0x04004A47 RID: 19015
		public static ThoughtDef WitnessedDeathBloodlust;

		// Token: 0x04004A48 RID: 19016
		public static ThoughtDef WitnessedDeathFamily;

		// Token: 0x04004A49 RID: 19017
		public static ThoughtDef KilledHumanlikeBloodlust;

		// Token: 0x04004A4A RID: 19018
		public static ThoughtDef ButcheredHumanlikeCorpse;

		// Token: 0x04004A4B RID: 19019
		public static ThoughtDef KnowButcheredHumanlikeCorpse;

		// Token: 0x04004A4C RID: 19020
		public static ThoughtDef RapportBuilt;

		// Token: 0x04004A4D RID: 19021
		public static ThoughtDef Nuzzled;

		// Token: 0x04004A4E RID: 19022
		public static ThoughtDef ArtifactMoodBoost;

		// Token: 0x04004A4F RID: 19023
		public static ThoughtDef KnowBuriedInSarcophagus;

		// Token: 0x04004A50 RID: 19024
		public static ThoughtDef DebugGood;

		// Token: 0x04004A51 RID: 19025
		public static ThoughtDef DebugBad;

		// Token: 0x04004A52 RID: 19026
		public static ThoughtDef DeniedJoining;

		// Token: 0x04004A53 RID: 19027
		public static ThoughtDef ColonistBanished;

		// Token: 0x04004A54 RID: 19028
		public static ThoughtDef ColonistBanishedToDie;

		// Token: 0x04004A55 RID: 19029
		public static ThoughtDef PrisonerBanishedToDie;

		// Token: 0x04004A56 RID: 19030
		public static ThoughtDef BondedAnimalBanished;

		// Token: 0x04004A57 RID: 19031
		public static ThoughtDef BondedAnimalReleased;

		// Token: 0x04004A58 RID: 19032
		public static ThoughtDef FailedToRescueRelative;

		// Token: 0x04004A59 RID: 19033
		public static ThoughtDef RescuedRelative;

		// Token: 0x04004A5A RID: 19034
		public static ThoughtDef Rescued;

		// Token: 0x04004A5B RID: 19035
		[MayRequireRoyalty]
		public static ThoughtDef DecreeMet;

		// Token: 0x04004A5C RID: 19036
		[MayRequireRoyalty]
		public static ThoughtDef KillThirst;

		// Token: 0x04004A5D RID: 19037
		[MayRequireRoyalty]
		public static ThoughtDef JealousRage;

		// Token: 0x04004A5E RID: 19038
		[MayRequireIdeology]
		public static ThoughtDef FailedConvertIdeoAttemptResentment;

		// Token: 0x04004A5F RID: 19039
		[MayRequireIdeology]
		public static ThoughtDef RitualDelayed;

		// Token: 0x04004A60 RID: 19040
		[MayRequireIdeology]
		public static ThoughtDef IdeoBuildingMissing;

		// Token: 0x04004A61 RID: 19041
		[MayRequireIdeology]
		public static ThoughtDef IdeoBuildingDisrespected;

		// Token: 0x04004A62 RID: 19042
		[MayRequireIdeology]
		public static ThoughtDef IdeoRoleLost;

		// Token: 0x04004A63 RID: 19043
		[MayRequireIdeology]
		public static ThoughtDef IdeoRoleEmpty;

		// Token: 0x04004A64 RID: 19044
		[MayRequireIdeology]
		public static ThoughtDef IdeoRoleApparelRequirementNotMet;

		// Token: 0x04004A65 RID: 19045
		[MayRequireIdeology]
		public static ThoughtDef Counselled;

		// Token: 0x04004A66 RID: 19046
		[MayRequireIdeology]
		public static ThoughtDef Counselled_MoodBoost;

		// Token: 0x04004A67 RID: 19047
		[MayRequireIdeology]
		public static ThoughtDef ObservedGibbetCage;

		// Token: 0x04004A68 RID: 19048
		[MayRequireIdeology]
		public static ThoughtDef ObservedSkullspike;

		// Token: 0x04004A69 RID: 19049
		[MayRequireIdeology]
		public static ThoughtDef RelicDestroyed;

		// Token: 0x04004A6A RID: 19050
		[MayRequireIdeology]
		public static ThoughtDef RelicLost;

		// Token: 0x04004A6B RID: 19051
		[MayRequireIdeology]
		public static ThoughtDef RelicAtRitual;

		// Token: 0x04004A6C RID: 19052
		[MayRequireIdeology]
		public static ThoughtDef WasEnslaved;

		// Token: 0x04004A6D RID: 19053
		[MayRequireIdeology]
		public static ThoughtDef NoRecentAnimalSlaughter;

		// Token: 0x04004A6E RID: 19054
		[MayRequireIdeology]
		public static ThoughtDef TameVeneratedAnimalDied;

		// Token: 0x04004A6F RID: 19055
		[MayRequireIdeology]
		public static ThoughtDef ConnectedTreeDied;

		// Token: 0x04004A70 RID: 19056
		[MayRequireIdeology]
		public static ThoughtDef RelicsCollected;

		// Token: 0x04004A71 RID: 19057
		[MayRequireIdeology]
		public static ThoughtDef BiosculpterPleasure;

		// Token: 0x04004A72 RID: 19058
		[MayRequireIdeology]
		public static ThoughtDef AgeReversalDemanded;

		// Token: 0x04004A73 RID: 19059
		[MayRequireIdeology]
		public static ThoughtDef AgeReversalReceived;

		// Token: 0x04004A74 RID: 19060
		[MayRequireIdeology]
		public static ThoughtDef DryadDied;

		// Token: 0x04004A75 RID: 19061
		public static ThoughtDef ColonistLost;

		// Token: 0x04004A76 RID: 19062
		public static ThoughtDef PawnWithGoodOpinionLost;

		// Token: 0x04004A77 RID: 19063
		public static ThoughtDef PawnWithBadOpinionLost;

		// Token: 0x04004A78 RID: 19064
		public static ThoughtDef ApparelDamaged;

		// Token: 0x04004A79 RID: 19065
		public static ThoughtDef Naked;

		// Token: 0x04004A7A RID: 19066
		public static ThoughtDef ClothedNudist;

		// Token: 0x04004A7B RID: 19067
		public static ThoughtDef DeadMansApparel;

		// Token: 0x04004A7C RID: 19068
		public static ThoughtDef HumanLeatherApparelSad;

		// Token: 0x04004A7D RID: 19069
		public static ThoughtDef HumanLeatherApparelHappy;

		// Token: 0x04004A7E RID: 19070
		[MayRequireRoyalty]
		public static ThoughtDef DecreeUnmet;

		// Token: 0x04004A7F RID: 19071
		[MayRequireIdeology]
		public static ThoughtDef NoRecentHumanMeat_RequiredRavenous;

		// Token: 0x04004A80 RID: 19072
		[MayRequireIdeology]
		public static ThoughtDef NoRecentHumanMeat_RequiredStrong;

		// Token: 0x04004A81 RID: 19073
		[MayRequireIdeology]
		public static ThoughtDef NoRecentHumanMeat_Preferred;

		// Token: 0x04004A82 RID: 19074
		[MayRequireIdeology]
		public static ThoughtDef TreesDesired;

		// Token: 0x04004A83 RID: 19075
		[MayRequireIdeology]
		public static ThoughtDef Skullspike_Desired;

		// Token: 0x04004A84 RID: 19076
		[MayRequireIdeology]
		public static ThoughtDef Skullspike_Disapproved;

		// Token: 0x04004A85 RID: 19077
		public static ThoughtDef Chitchat;

		// Token: 0x04004A86 RID: 19078
		public static ThoughtDef PawnWithGoodOpinionDied;

		// Token: 0x04004A87 RID: 19079
		public static ThoughtDef PawnWithBadOpinionDied;

		// Token: 0x04004A88 RID: 19080
		public static ThoughtDef HadAngeringFight;

		// Token: 0x04004A89 RID: 19081
		public static ThoughtDef HadCatharticFight;

		// Token: 0x04004A8A RID: 19082
		public static ThoughtDef HarmedMe;

		// Token: 0x04004A8B RID: 19083
		public static ThoughtDef BotchedMySurgery;

		// Token: 0x04004A8C RID: 19084
		public static ThoughtDef CheatedOnMe;

		// Token: 0x04004A8D RID: 19085
		public static ThoughtDef RebuffedMyRomanceAttempt;

		// Token: 0x04004A8E RID: 19086
		public static ThoughtDef FailedRomanceAttemptOnMe;

		// Token: 0x04004A8F RID: 19087
		public static ThoughtDef FailedRomanceAttemptOnMeLowOpinionMood;

		// Token: 0x04004A90 RID: 19088
		public static ThoughtDef BrokeUpWithMe;

		// Token: 0x04004A91 RID: 19089
		public static ThoughtDef DivorcedMe;

		// Token: 0x04004A92 RID: 19090
		public static ThoughtDef RejectedMyProposal;

		// Token: 0x04004A93 RID: 19091
		public static ThoughtDef RejectedMyProposalMood;

		// Token: 0x04004A94 RID: 19092
		public static ThoughtDef IRejectedTheirProposal;

		// Token: 0x04004A95 RID: 19093
		public static ThoughtDef GotMarried;

		// Token: 0x04004A96 RID: 19094
		public static ThoughtDef HoneymoonPhase;

		// Token: 0x04004A97 RID: 19095
		public static ThoughtDef RescuedMe;

		// Token: 0x04004A98 RID: 19096
		public static ThoughtDef RescuedMeByOfferingHelp;

		// Token: 0x04004A99 RID: 19097
		public static ThoughtDef RecruitedMe;

		// Token: 0x04004A9A RID: 19098
		public static ThoughtDef AttendedWedding;

		// Token: 0x04004A9B RID: 19099
		public static ThoughtDef AttendedParty;

		// Token: 0x04004A9C RID: 19100
		public static ThoughtDef AttendedConcert;

		// Token: 0x04004A9D RID: 19101
		public static ThoughtDef HeldConcert;

		// Token: 0x04004A9E RID: 19102
		public static ThoughtDef TerribleSpeech;

		// Token: 0x04004A9F RID: 19103
		public static ThoughtDef UninspiringSpeech;

		// Token: 0x04004AA0 RID: 19104
		public static ThoughtDef EncouragingSpeech;

		// Token: 0x04004AA1 RID: 19105
		public static ThoughtDef InspirationalSpeech;

		// Token: 0x04004AA2 RID: 19106
		public static ThoughtDef CrashedTogether;

		// Token: 0x04004AA3 RID: 19107
		public static ThoughtDef Insulted;

		// Token: 0x04004AA4 RID: 19108
		public static ThoughtDef KindWords;

		// Token: 0x04004AA5 RID: 19109
		public static ThoughtDef GotSomeLovin;

		// Token: 0x04004AA6 RID: 19110
		public static ThoughtDef KilledMyFriend;

		// Token: 0x04004AA7 RID: 19111
		public static ThoughtDef KilledMyRival;

		// Token: 0x04004AA8 RID: 19112
		public static ThoughtDef DefeatedHostileFactionLeader;

		// Token: 0x04004AA9 RID: 19113
		public static ThoughtDef DefeatedMechCluster;

		// Token: 0x04004AAA RID: 19114
		public static ThoughtDef ForcedMeToTakeDrugs;

		// Token: 0x04004AAB RID: 19115
		public static ThoughtDef ForcedMeToTakeLuciferium;

		// Token: 0x04004AAC RID: 19116
		[MayRequireRoyalty]
		public static ThoughtDef OtherTravelerDied;

		// Token: 0x04004AAD RID: 19117
		[MayRequireRoyalty]
		public static ThoughtDef OtherTravelerArrested;

		// Token: 0x04004AAE RID: 19118
		[MayRequireRoyalty]
		public static ThoughtDef OtherTravelerSurgicallyViolated;

		// Token: 0x04004AAF RID: 19119
		[MayRequireRoyalty]
		public static ThoughtDef NeuroquakeEcho;

		// Token: 0x04004AB0 RID: 19120
		[MayRequireIdeology]
		public static ThoughtDef TrialExonerated;

		// Token: 0x04004AB1 RID: 19121
		[MayRequireIdeology]
		public static ThoughtDef TrialFailed;

		// Token: 0x04004AB2 RID: 19122
		[MayRequireIdeology]
		public static ThoughtDef TrialConvicted;
	}
}
