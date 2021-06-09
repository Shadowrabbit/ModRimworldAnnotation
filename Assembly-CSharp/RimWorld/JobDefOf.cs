using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C4F RID: 7247
	[DefOf]
	public static class JobDefOf
	{
		// Token: 0x06009F52 RID: 40786 RVA: 0x0006A1DF File Offset: 0x000683DF
		static JobDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(JobDefOf));
		}

		// Token: 0x04006787 RID: 26503
		public static JobDef Goto;

		// Token: 0x04006788 RID: 26504
		public static JobDef Wait;

		// Token: 0x04006789 RID: 26505
		public static JobDef Wait_MaintainPosture;

		// Token: 0x0400678A RID: 26506
		public static JobDef Wait_Downed;

		// Token: 0x0400678B RID: 26507
		public static JobDef GotoWander;

		// Token: 0x0400678C RID: 26508
		public static JobDef Wait_Wander;

		// Token: 0x0400678D RID: 26509
		public static JobDef GotoSafeTemperature;

		// Token: 0x0400678E RID: 26510
		public static JobDef Wait_SafeTemperature;

		// Token: 0x0400678F RID: 26511
		public static JobDef Wait_Combat;

		// Token: 0x04006790 RID: 26512
		public static JobDef Equip;

		// Token: 0x04006791 RID: 26513
		public static JobDef AttackMelee;

		// Token: 0x04006792 RID: 26514
		public static JobDef AttackStatic;

		// Token: 0x04006793 RID: 26515
		public static JobDef UseVerbOnThing;

		// Token: 0x04006794 RID: 26516
		public static JobDef UseVerbOnThingStatic;

		// Token: 0x04006795 RID: 26517
		public static JobDef CastJump;

		// Token: 0x04006796 RID: 26518
		public static JobDef CastAbilityOnThing;

		// Token: 0x04006797 RID: 26519
		public static JobDef CastAbilityOnWorldTile;

		// Token: 0x04006798 RID: 26520
		public static JobDef TakeInventory;

		// Token: 0x04006799 RID: 26521
		public static JobDef Follow;

		// Token: 0x0400679A RID: 26522
		public static JobDef FollowClose;

		// Token: 0x0400679B RID: 26523
		public static JobDef Wear;

		// Token: 0x0400679C RID: 26524
		public static JobDef RemoveApparel;

		// Token: 0x0400679D RID: 26525
		public static JobDef DropEquipment;

		// Token: 0x0400679E RID: 26526
		public static JobDef Strip;

		// Token: 0x0400679F RID: 26527
		public static JobDef Open;

		// Token: 0x040067A0 RID: 26528
		public static JobDef Hunt;

		// Token: 0x040067A1 RID: 26529
		public static JobDef ManTurret;

		// Token: 0x040067A2 RID: 26530
		public static JobDef EnterCryptosleepCasket;

		// Token: 0x040067A3 RID: 26531
		public static JobDef UseNeurotrainer;

		// Token: 0x040067A4 RID: 26532
		public static JobDef UseArtifact;

		// Token: 0x040067A5 RID: 26533
		public static JobDef TriggerFirefoamPopper;

		// Token: 0x040067A6 RID: 26534
		public static JobDef ClearSnow;

		// Token: 0x040067A7 RID: 26535
		public static JobDef Vomit;

		// Token: 0x040067A8 RID: 26536
		public static JobDef Flick;

		// Token: 0x040067A9 RID: 26537
		public static JobDef DoBill;

		// Token: 0x040067AA RID: 26538
		public static JobDef Research;

		// Token: 0x040067AB RID: 26539
		public static JobDef Mine;

		// Token: 0x040067AC RID: 26540
		public static JobDef OperateDeepDrill;

		// Token: 0x040067AD RID: 26541
		public static JobDef OperateScanner;

		// Token: 0x040067AE RID: 26542
		public static JobDef Repair;

		// Token: 0x040067AF RID: 26543
		public static JobDef FixBrokenDownBuilding;

		// Token: 0x040067B0 RID: 26544
		public static JobDef UseCommsConsole;

		// Token: 0x040067B1 RID: 26545
		public static JobDef Clean;

		// Token: 0x040067B2 RID: 26546
		public static JobDef TradeWithPawn;

		// Token: 0x040067B3 RID: 26547
		public static JobDef Flee;

		// Token: 0x040067B4 RID: 26548
		public static JobDef FleeAndCower;

		// Token: 0x040067B5 RID: 26549
		public static JobDef Lovin;

		// Token: 0x040067B6 RID: 26550
		public static JobDef SocialFight;

		// Token: 0x040067B7 RID: 26551
		public static JobDef Maintain;

		// Token: 0x040067B8 RID: 26552
		public static JobDef GiveToPackAnimal;

		// Token: 0x040067B9 RID: 26553
		public static JobDef EnterTransporter;

		// Token: 0x040067BA RID: 26554
		public static JobDef Resurrect;

		// Token: 0x040067BB RID: 26555
		public static JobDef Insult;

		// Token: 0x040067BC RID: 26556
		public static JobDef HaulCorpseToPublicPlace;

		// Token: 0x040067BD RID: 26557
		public static JobDef InducePrisonerToEscape;

		// Token: 0x040067BE RID: 26558
		public static JobDef OfferHelp;

		// Token: 0x040067BF RID: 26559
		public static JobDef ApplyTechprint;

		// Token: 0x040067C0 RID: 26560
		public static JobDef GotoMindControlled;

		// Token: 0x040067C1 RID: 26561
		public static JobDef MarryAdjacentPawn;

		// Token: 0x040067C2 RID: 26562
		public static JobDef SpectateCeremony;

		// Token: 0x040067C3 RID: 26563
		public static JobDef StandAndBeSociallyActive;

		// Token: 0x040067C4 RID: 26564
		public static JobDef GiveSpeech;

		// Token: 0x040067C5 RID: 26565
		public static JobDef PrepareCaravan_GatherItems;

		// Token: 0x040067C6 RID: 26566
		public static JobDef PrepareCaravan_GatherPawns;

		// Token: 0x040067C7 RID: 26567
		public static JobDef PrepareCaravan_GatherDownedPawns;

		// Token: 0x040067C8 RID: 26568
		public static JobDef Ignite;

		// Token: 0x040067C9 RID: 26569
		public static JobDef BeatFire;

		// Token: 0x040067CA RID: 26570
		public static JobDef ExtinguishSelf;

		// Token: 0x040067CB RID: 26571
		public static JobDef LayDown;

		// Token: 0x040067CC RID: 26572
		public static JobDef Ingest;

		// Token: 0x040067CD RID: 26573
		public static JobDef SocialRelax;

		// Token: 0x040067CE RID: 26574
		public static JobDef HaulToCell;

		// Token: 0x040067CF RID: 26575
		public static JobDef HaulToContainer;

		// Token: 0x040067D0 RID: 26576
		public static JobDef Steal;

		// Token: 0x040067D1 RID: 26577
		public static JobDef Reload;

		// Token: 0x040067D2 RID: 26578
		public static JobDef Refuel;

		// Token: 0x040067D3 RID: 26579
		public static JobDef RefuelAtomic;

		// Token: 0x040067D4 RID: 26580
		public static JobDef RearmTurret;

		// Token: 0x040067D5 RID: 26581
		public static JobDef RearmTurretAtomic;

		// Token: 0x040067D6 RID: 26582
		public static JobDef FillFermentingBarrel;

		// Token: 0x040067D7 RID: 26583
		public static JobDef TakeBeerOutOfFermentingBarrel;

		// Token: 0x040067D8 RID: 26584
		public static JobDef UnloadInventory;

		// Token: 0x040067D9 RID: 26585
		public static JobDef UnloadYourInventory;

		// Token: 0x040067DA RID: 26586
		public static JobDef HaulToTransporter;

		// Token: 0x040067DB RID: 26587
		public static JobDef Rescue;

		// Token: 0x040067DC RID: 26588
		public static JobDef Arrest;

		// Token: 0x040067DD RID: 26589
		public static JobDef Capture;

		// Token: 0x040067DE RID: 26590
		public static JobDef TakeWoundedPrisonerToBed;

		// Token: 0x040067DF RID: 26591
		public static JobDef TakeToBedToOperate;

		// Token: 0x040067E0 RID: 26592
		public static JobDef EscortPrisonerToBed;

		// Token: 0x040067E1 RID: 26593
		public static JobDef CarryToCryptosleepCasket;

		// Token: 0x040067E2 RID: 26594
		public static JobDef ReleasePrisoner;

		// Token: 0x040067E3 RID: 26595
		public static JobDef Kidnap;

		// Token: 0x040067E4 RID: 26596
		public static JobDef CarryDownedPawnToExit;

		// Token: 0x040067E5 RID: 26597
		public static JobDef PlaceNoCostFrame;

		// Token: 0x040067E6 RID: 26598
		public static JobDef FinishFrame;

		// Token: 0x040067E7 RID: 26599
		public static JobDef Deconstruct;

		// Token: 0x040067E8 RID: 26600
		public static JobDef Uninstall;

		// Token: 0x040067E9 RID: 26601
		public static JobDef SmoothFloor;

		// Token: 0x040067EA RID: 26602
		public static JobDef RemoveFloor;

		// Token: 0x040067EB RID: 26603
		public static JobDef BuildRoof;

		// Token: 0x040067EC RID: 26604
		public static JobDef RemoveRoof;

		// Token: 0x040067ED RID: 26605
		public static JobDef SmoothWall;

		// Token: 0x040067EE RID: 26606
		public static JobDef PrisonerAttemptRecruit;

		// Token: 0x040067EF RID: 26607
		public static JobDef PrisonerExecution;

		// Token: 0x040067F0 RID: 26608
		public static JobDef DeliverFood;

		// Token: 0x040067F1 RID: 26609
		public static JobDef FeedPatient;

		// Token: 0x040067F2 RID: 26610
		public static JobDef TendPatient;

		// Token: 0x040067F3 RID: 26611
		public static JobDef VisitSickPawn;

		// Token: 0x040067F4 RID: 26612
		public static JobDef Sow;

		// Token: 0x040067F5 RID: 26613
		public static JobDef Harvest;

		// Token: 0x040067F6 RID: 26614
		public static JobDef CutPlant;

		// Token: 0x040067F7 RID: 26615
		public static JobDef HarvestDesignated;

		// Token: 0x040067F8 RID: 26616
		public static JobDef CutPlantDesignated;

		// Token: 0x040067F9 RID: 26617
		public static JobDef Slaughter;

		// Token: 0x040067FA RID: 26618
		public static JobDef Milk;

		// Token: 0x040067FB RID: 26619
		public static JobDef Shear;

		// Token: 0x040067FC RID: 26620
		public static JobDef Tame;

		// Token: 0x040067FD RID: 26621
		public static JobDef Train;

		// Token: 0x040067FE RID: 26622
		public static JobDef Nuzzle;

		// Token: 0x040067FF RID: 26623
		public static JobDef Mate;

		// Token: 0x04006800 RID: 26624
		public static JobDef LayEgg;

		// Token: 0x04006801 RID: 26625
		public static JobDef PredatorHunt;

		// Token: 0x04006802 RID: 26626
		[MayRequireRoyalty]
		public static JobDef Reign;

		// Token: 0x04006803 RID: 26627
		[MayRequireRoyalty]
		public static JobDef Meditate;

		// Token: 0x04006804 RID: 26628
		[MayRequireRoyalty]
		public static JobDef Play_MusicalInstrument;

		// Token: 0x04006805 RID: 26629
		[MayRequireRoyalty]
		public static JobDef LinkPsylinkable;

		// Token: 0x04006806 RID: 26630
		[MayRequireRoyalty]
		public static JobDef BestowingCeremony;
	}
}
