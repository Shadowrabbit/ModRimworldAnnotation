using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200140F RID: 5135
	[DefOf]
	public static class JobDefOf
	{
		// Token: 0x06007D02 RID: 32002 RVA: 0x002C463E File Offset: 0x002C283E
		static JobDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(JobDefOf));
		}

		// Token: 0x040047B2 RID: 18354
		public static JobDef Goto;

		// Token: 0x040047B3 RID: 18355
		public static JobDef Wait;

		// Token: 0x040047B4 RID: 18356
		public static JobDef Wait_MaintainPosture;

		// Token: 0x040047B5 RID: 18357
		public static JobDef Wait_Downed;

		// Token: 0x040047B6 RID: 18358
		public static JobDef GotoWander;

		// Token: 0x040047B7 RID: 18359
		public static JobDef Wait_Wander;

		// Token: 0x040047B8 RID: 18360
		public static JobDef GotoSafeTemperature;

		// Token: 0x040047B9 RID: 18361
		public static JobDef Wait_SafeTemperature;

		// Token: 0x040047BA RID: 18362
		public static JobDef Wait_Combat;

		// Token: 0x040047BB RID: 18363
		public static JobDef Equip;

		// Token: 0x040047BC RID: 18364
		public static JobDef AttackMelee;

		// Token: 0x040047BD RID: 18365
		public static JobDef AttackStatic;

		// Token: 0x040047BE RID: 18366
		public static JobDef UseVerbOnThing;

		// Token: 0x040047BF RID: 18367
		public static JobDef UseVerbOnThingStatic;

		// Token: 0x040047C0 RID: 18368
		public static JobDef CastJump;

		// Token: 0x040047C1 RID: 18369
		public static JobDef CastAbilityOnThing;

		// Token: 0x040047C2 RID: 18370
		public static JobDef CastAbilityOnWorldTile;

		// Token: 0x040047C3 RID: 18371
		public static JobDef TakeInventory;

		// Token: 0x040047C4 RID: 18372
		public static JobDef Follow;

		// Token: 0x040047C5 RID: 18373
		public static JobDef FollowClose;

		// Token: 0x040047C6 RID: 18374
		public static JobDef Wear;

		// Token: 0x040047C7 RID: 18375
		public static JobDef RemoveApparel;

		// Token: 0x040047C8 RID: 18376
		public static JobDef DropEquipment;

		// Token: 0x040047C9 RID: 18377
		public static JobDef Strip;

		// Token: 0x040047CA RID: 18378
		public static JobDef Open;

		// Token: 0x040047CB RID: 18379
		public static JobDef Hunt;

		// Token: 0x040047CC RID: 18380
		public static JobDef ManTurret;

		// Token: 0x040047CD RID: 18381
		public static JobDef EnterCryptosleepCasket;

		// Token: 0x040047CE RID: 18382
		public static JobDef UseNeurotrainer;

		// Token: 0x040047CF RID: 18383
		public static JobDef UseArtifact;

		// Token: 0x040047D0 RID: 18384
		public static JobDef TriggerFirefoamPopper;

		// Token: 0x040047D1 RID: 18385
		public static JobDef ClearSnow;

		// Token: 0x040047D2 RID: 18386
		public static JobDef Vomit;

		// Token: 0x040047D3 RID: 18387
		public static JobDef Flick;

		// Token: 0x040047D4 RID: 18388
		public static JobDef DoBill;

		// Token: 0x040047D5 RID: 18389
		public static JobDef Research;

		// Token: 0x040047D6 RID: 18390
		public static JobDef Mine;

		// Token: 0x040047D7 RID: 18391
		public static JobDef OperateDeepDrill;

		// Token: 0x040047D8 RID: 18392
		public static JobDef OperateScanner;

		// Token: 0x040047D9 RID: 18393
		public static JobDef Repair;

		// Token: 0x040047DA RID: 18394
		public static JobDef FixBrokenDownBuilding;

		// Token: 0x040047DB RID: 18395
		public static JobDef UseCommsConsole;

		// Token: 0x040047DC RID: 18396
		public static JobDef Clean;

		// Token: 0x040047DD RID: 18397
		public static JobDef TradeWithPawn;

		// Token: 0x040047DE RID: 18398
		public static JobDef Flee;

		// Token: 0x040047DF RID: 18399
		public static JobDef FleeAndCower;

		// Token: 0x040047E0 RID: 18400
		public static JobDef Lovin;

		// Token: 0x040047E1 RID: 18401
		public static JobDef SocialFight;

		// Token: 0x040047E2 RID: 18402
		public static JobDef Maintain;

		// Token: 0x040047E3 RID: 18403
		public static JobDef GiveToPackAnimal;

		// Token: 0x040047E4 RID: 18404
		public static JobDef EnterTransporter;

		// Token: 0x040047E5 RID: 18405
		public static JobDef Resurrect;

		// Token: 0x040047E6 RID: 18406
		public static JobDef Insult;

		// Token: 0x040047E7 RID: 18407
		public static JobDef HaulCorpseToPublicPlace;

		// Token: 0x040047E8 RID: 18408
		public static JobDef InducePrisonerToEscape;

		// Token: 0x040047E9 RID: 18409
		public static JobDef OfferHelp;

		// Token: 0x040047EA RID: 18410
		public static JobDef ApplyTechprint;

		// Token: 0x040047EB RID: 18411
		public static JobDef GotoMindControlled;

		// Token: 0x040047EC RID: 18412
		public static JobDef EmptyThingContainer;

		// Token: 0x040047ED RID: 18413
		[MayRequireIdeology]
		public static JobDef ActivateArchonexusCore;

		// Token: 0x040047EE RID: 18414
		public static JobDef StudyThing;

		// Token: 0x040047EF RID: 18415
		public static JobDef MarryAdjacentPawn;

		// Token: 0x040047F0 RID: 18416
		public static JobDef SpectateCeremony;

		// Token: 0x040047F1 RID: 18417
		public static JobDef StandAndBeSociallyActive;

		// Token: 0x040047F2 RID: 18418
		public static JobDef GiveSpeech;

		// Token: 0x040047F3 RID: 18419
		[MayRequireIdeology]
		public static JobDef Dance;

		// Token: 0x040047F4 RID: 18420
		[MayRequireIdeology]
		public static JobDef EatAtCannibalPlatter;

		// Token: 0x040047F5 RID: 18421
		public static JobDef PrepareCaravan_GatherItems;

		// Token: 0x040047F6 RID: 18422
		public static JobDef PrepareCaravan_GatherAnimals;

		// Token: 0x040047F7 RID: 18423
		public static JobDef PrepareCaravan_CollectAnimals;

		// Token: 0x040047F8 RID: 18424
		public static JobDef PrepareCaravan_GatherDownedPawns;

		// Token: 0x040047F9 RID: 18425
		public static JobDef ReturnedCaravan_PenAnimals;

		// Token: 0x040047FA RID: 18426
		public static JobDef Ignite;

		// Token: 0x040047FB RID: 18427
		public static JobDef BeatFire;

		// Token: 0x040047FC RID: 18428
		public static JobDef ExtinguishSelf;

		// Token: 0x040047FD RID: 18429
		public static JobDef LayDown;

		// Token: 0x040047FE RID: 18430
		public static JobDef LayDownAwake;

		// Token: 0x040047FF RID: 18431
		public static JobDef LayDownResting;

		// Token: 0x04004800 RID: 18432
		public static JobDef Ingest;

		// Token: 0x04004801 RID: 18433
		public static JobDef SocialRelax;

		// Token: 0x04004802 RID: 18434
		public static JobDef HaulToCell;

		// Token: 0x04004803 RID: 18435
		public static JobDef HaulToContainer;

		// Token: 0x04004804 RID: 18436
		public static JobDef Steal;

		// Token: 0x04004805 RID: 18437
		public static JobDef Reload;

		// Token: 0x04004806 RID: 18438
		public static JobDef Refuel;

		// Token: 0x04004807 RID: 18439
		public static JobDef RefuelAtomic;

		// Token: 0x04004808 RID: 18440
		public static JobDef RearmTurret;

		// Token: 0x04004809 RID: 18441
		public static JobDef RearmTurretAtomic;

		// Token: 0x0400480A RID: 18442
		public static JobDef FillFermentingBarrel;

		// Token: 0x0400480B RID: 18443
		public static JobDef TakeBeerOutOfFermentingBarrel;

		// Token: 0x0400480C RID: 18444
		public static JobDef UnloadInventory;

		// Token: 0x0400480D RID: 18445
		public static JobDef UnloadYourInventory;

		// Token: 0x0400480E RID: 18446
		public static JobDef HaulToTransporter;

		// Token: 0x0400480F RID: 18447
		[MayRequireIdeology]
		public static JobDef GiveToPawn;

		// Token: 0x04004810 RID: 18448
		[MayRequireIdeology]
		public static JobDef ExtractRelic;

		// Token: 0x04004811 RID: 18449
		[MayRequireIdeology]
		public static JobDef InstallRelic;

		// Token: 0x04004812 RID: 18450
		[MayRequireIdeology]
		public static JobDef ExtractToInventory;

		// Token: 0x04004813 RID: 18451
		public static JobDef Rescue;

		// Token: 0x04004814 RID: 18452
		public static JobDef Arrest;

		// Token: 0x04004815 RID: 18453
		public static JobDef Capture;

		// Token: 0x04004816 RID: 18454
		public static JobDef TakeWoundedPrisonerToBed;

		// Token: 0x04004817 RID: 18455
		public static JobDef TakeToBedToOperate;

		// Token: 0x04004818 RID: 18456
		public static JobDef TakeDownedPawnToBedDrafted;

		// Token: 0x04004819 RID: 18457
		public static JobDef EscortPrisonerToBed;

		// Token: 0x0400481A RID: 18458
		public static JobDef CarryToCryptosleepCasket;

		// Token: 0x0400481B RID: 18459
		public static JobDef CarryToCryptosleepCasketDrafted;

		// Token: 0x0400481C RID: 18460
		public static JobDef ReleasePrisoner;

		// Token: 0x0400481D RID: 18461
		public static JobDef Kidnap;

		// Token: 0x0400481E RID: 18462
		public static JobDef CarryDownedPawnToExit;

		// Token: 0x0400481F RID: 18463
		public static JobDef CarryDownedPawnDrafted;

		// Token: 0x04004820 RID: 18464
		public static JobDef CarryToPrisonerBedDrafted;

		// Token: 0x04004821 RID: 18465
		public static JobDef DeliverToCell;

		// Token: 0x04004822 RID: 18466
		[MayRequireIdeology]
		public static JobDef DeliverToAltar;

		// Token: 0x04004823 RID: 18467
		[MayRequireIdeology]
		public static JobDef Sacrifice;

		// Token: 0x04004824 RID: 18468
		[MayRequireIdeology]
		public static JobDef Scarify;

		// Token: 0x04004825 RID: 18469
		[MayRequireIdeology]
		public static JobDef Blind;

		// Token: 0x04004826 RID: 18470
		public static JobDef PlaceNoCostFrame;

		// Token: 0x04004827 RID: 18471
		public static JobDef FinishFrame;

		// Token: 0x04004828 RID: 18472
		public static JobDef Deconstruct;

		// Token: 0x04004829 RID: 18473
		public static JobDef Uninstall;

		// Token: 0x0400482A RID: 18474
		public static JobDef SmoothFloor;

		// Token: 0x0400482B RID: 18475
		public static JobDef RemoveFloor;

		// Token: 0x0400482C RID: 18476
		public static JobDef BuildRoof;

		// Token: 0x0400482D RID: 18477
		public static JobDef RemoveRoof;

		// Token: 0x0400482E RID: 18478
		public static JobDef SmoothWall;

		// Token: 0x0400482F RID: 18479
		public static JobDef PrisonerAttemptRecruit;

		// Token: 0x04004830 RID: 18480
		public static JobDef PrisonerExecution;

		// Token: 0x04004831 RID: 18481
		public static JobDef GuiltyColonistExecution;

		// Token: 0x04004832 RID: 18482
		public static JobDef DeliverFood;

		// Token: 0x04004833 RID: 18483
		[MayRequireIdeology]
		public static JobDef PrisonerEnslave;

		// Token: 0x04004834 RID: 18484
		[MayRequireIdeology]
		public static JobDef PrisonerConvert;

		// Token: 0x04004835 RID: 18485
		[MayRequireIdeology]
		public static JobDef SlaveSuppress;

		// Token: 0x04004836 RID: 18486
		[MayRequireIdeology]
		public static JobDef SlaveEmancipation;

		// Token: 0x04004837 RID: 18487
		[MayRequireIdeology]
		public static JobDef SlaveExecution;

		// Token: 0x04004838 RID: 18488
		public static JobDef FeedPatient;

		// Token: 0x04004839 RID: 18489
		public static JobDef TendPatient;

		// Token: 0x0400483A RID: 18490
		public static JobDef VisitSickPawn;

		// Token: 0x0400483B RID: 18491
		public static JobDef Sow;

		// Token: 0x0400483C RID: 18492
		public static JobDef Harvest;

		// Token: 0x0400483D RID: 18493
		public static JobDef CutPlant;

		// Token: 0x0400483E RID: 18494
		public static JobDef HarvestDesignated;

		// Token: 0x0400483F RID: 18495
		public static JobDef CutPlantDesignated;

		// Token: 0x04004840 RID: 18496
		[MayRequireIdeology]
		public static JobDef PlantSeed;

		// Token: 0x04004841 RID: 18497
		[MayRequireIdeology]
		public static JobDef PruneGauranlenTree;

		// Token: 0x04004842 RID: 18498
		public static JobDef Slaughter;

		// Token: 0x04004843 RID: 18499
		public static JobDef Milk;

		// Token: 0x04004844 RID: 18500
		public static JobDef Shear;

		// Token: 0x04004845 RID: 18501
		public static JobDef Tame;

		// Token: 0x04004846 RID: 18502
		public static JobDef Train;

		// Token: 0x04004847 RID: 18503
		public static JobDef RopeToPen;

		// Token: 0x04004848 RID: 18504
		public static JobDef RopeRoamerToUnenclosedPen;

		// Token: 0x04004849 RID: 18505
		public static JobDef Unrope;

		// Token: 0x0400484A RID: 18506
		public static JobDef ReleaseAnimalToWild;

		// Token: 0x0400484B RID: 18507
		[MayRequireIdeology]
		public static JobDef ExtractSkull;

		// Token: 0x0400484C RID: 18508
		public static JobDef Nuzzle;

		// Token: 0x0400484D RID: 18509
		public static JobDef Mate;

		// Token: 0x0400484E RID: 18510
		public static JobDef LayEgg;

		// Token: 0x0400484F RID: 18511
		public static JobDef PredatorHunt;

		// Token: 0x04004850 RID: 18512
		public static JobDef FollowRoper;

		// Token: 0x04004851 RID: 18513
		[MayRequireRoyalty]
		public static JobDef Reign;

		// Token: 0x04004852 RID: 18514
		[MayRequireRoyalty]
		public static JobDef Meditate;

		// Token: 0x04004853 RID: 18515
		[MayRequireRoyalty]
		public static JobDef Play_MusicalInstrument;

		// Token: 0x04004854 RID: 18516
		[MayRequireRoyalty]
		public static JobDef LinkPsylinkable;

		// Token: 0x04004855 RID: 18517
		[MayRequireRoyalty]
		public static JobDef BestowingCeremony;

		// Token: 0x04004856 RID: 18518
		[MayRequireIdeology]
		public static JobDef InduceSlaveToRebel;

		// Token: 0x04004857 RID: 18519
		[MayRequireIdeology]
		public static JobDef OpenStylingStationDialog;

		// Token: 0x04004858 RID: 18520
		[MayRequireIdeology]
		public static JobDef UseStylingStation;

		// Token: 0x04004859 RID: 18521
		[MayRequireIdeology]
		public static JobDef UseStylingStationAutomatic;

		// Token: 0x0400485A RID: 18522
		[MayRequireIdeology]
		public static JobDef Hack;

		// Token: 0x0400485B RID: 18523
		[MayRequireIdeology]
		public static JobDef TakeCountToInventory;

		// Token: 0x0400485C RID: 18524
		[MayRequireIdeology]
		public static JobDef GotoAndBeSociallyActive;

		// Token: 0x0400485D RID: 18525
		[MayRequireIdeology]
		public static JobDef PrepareSkylantern;

		// Token: 0x0400485E RID: 18526
		[MayRequireIdeology]
		public static JobDef MeditatePray;

		// Token: 0x0400485F RID: 18527
		[MayRequireIdeology]
		public static JobDef GetNeuralSupercharge;

		// Token: 0x04004860 RID: 18528
		[MayRequireIdeology]
		public static JobDef CreateAndEnterCocoon;

		// Token: 0x04004861 RID: 18529
		[MayRequireIdeology]
		public static JobDef ReturnToGauranlenTree;

		// Token: 0x04004862 RID: 18530
		[MayRequireIdeology]
		public static JobDef MergeIntoGaumakerPod;

		// Token: 0x04004863 RID: 18531
		[MayRequireIdeology]
		public static JobDef ChangeTreeMode;

		// Token: 0x04004864 RID: 18532
		[MayRequireIdeology]
		public static JobDef RecolorApparel;

		// Token: 0x04004865 RID: 18533
		[MayRequireIdeology]
		public static JobDef EnterBiosculpterPod;

		// Token: 0x04004866 RID: 18534
		[MayRequireIdeology]
		public static JobDef CarryToBiosculpterPod;
	}
}
