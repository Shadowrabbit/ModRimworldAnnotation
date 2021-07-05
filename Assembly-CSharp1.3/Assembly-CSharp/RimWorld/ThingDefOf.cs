using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001403 RID: 5123
	[DefOf]
	public static class ThingDefOf
	{
		// Token: 0x06007CF7 RID: 31991 RVA: 0x002C4583 File Offset: 0x002C2783
		static ThingDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ThingDefOf));
		}

		// Token: 0x04004518 RID: 17688
		public static ThingDef Silver;

		// Token: 0x04004519 RID: 17689
		public static ThingDef Gold;

		// Token: 0x0400451A RID: 17690
		public static ThingDef Steel;

		// Token: 0x0400451B RID: 17691
		public static ThingDef WoodLog;

		// Token: 0x0400451C RID: 17692
		public static ThingDef MedicineHerbal;

		// Token: 0x0400451D RID: 17693
		public static ThingDef MedicineIndustrial;

		// Token: 0x0400451E RID: 17694
		public static ThingDef MedicineUltratech;

		// Token: 0x0400451F RID: 17695
		public static ThingDef BlocksGranite;

		// Token: 0x04004520 RID: 17696
		public static ThingDef Plasteel;

		// Token: 0x04004521 RID: 17697
		public static ThingDef Beer;

		// Token: 0x04004522 RID: 17698
		public static ThingDef SmokeleafJoint;

		// Token: 0x04004523 RID: 17699
		public static ThingDef Chocolate;

		// Token: 0x04004524 RID: 17700
		public static ThingDef ComponentIndustrial;

		// Token: 0x04004525 RID: 17701
		public static ThingDef ComponentSpacer;

		// Token: 0x04004526 RID: 17702
		public static ThingDef InsectJelly;

		// Token: 0x04004527 RID: 17703
		public static ThingDef Cloth;

		// Token: 0x04004528 RID: 17704
		public static ThingDef Leather_Plain;

		// Token: 0x04004529 RID: 17705
		public static ThingDef Hyperweave;

		// Token: 0x0400452A RID: 17706
		public static ThingDef RawPotatoes;

		// Token: 0x0400452B RID: 17707
		public static ThingDef RawBerries;

		// Token: 0x0400452C RID: 17708
		public static ThingDef Granite;

		// Token: 0x0400452D RID: 17709
		public static ThingDef Wort;

		// Token: 0x0400452E RID: 17710
		public static ThingDef AIPersonaCore;

		// Token: 0x0400452F RID: 17711
		public static ThingDef TechprofSubpersonaCore;

		// Token: 0x04004530 RID: 17712
		public static ThingDef OrbitalTargeterBombardment;

		// Token: 0x04004531 RID: 17713
		public static ThingDef OrbitalTargeterPowerBeam;

		// Token: 0x04004532 RID: 17714
		public static ThingDef Chemfuel;

		// Token: 0x04004533 RID: 17715
		public static ThingDef Uranium;

		// Token: 0x04004534 RID: 17716
		public static ThingDef Jade;

		// Token: 0x04004535 RID: 17717
		public static ThingDef Shell_HighExplosive;

		// Token: 0x04004536 RID: 17718
		public static ThingDef Shell_AntigrainWarhead;

		// Token: 0x04004537 RID: 17719
		public static ThingDef ReinforcedBarrel;

		// Token: 0x04004538 RID: 17720
		public static ThingDef MealSurvivalPack;

		// Token: 0x04004539 RID: 17721
		public static ThingDef MealNutrientPaste;

		// Token: 0x0400453A RID: 17722
		public static ThingDef MealSimple;

		// Token: 0x0400453B RID: 17723
		public static ThingDef MealFine;

		// Token: 0x0400453C RID: 17724
		public static ThingDef Pemmican;

		// Token: 0x0400453D RID: 17725
		public static ThingDef Kibble;

		// Token: 0x0400453E RID: 17726
		public static ThingDef Hay;

		// Token: 0x0400453F RID: 17727
		public static ThingDef Meat_Human;

		// Token: 0x04004540 RID: 17728
		public static ThingDef Luciferium;

		// Token: 0x04004541 RID: 17729
		public static ThingDef Penoxycyline;

		// Token: 0x04004542 RID: 17730
		public static ThingDef DropPodIncoming;

		// Token: 0x04004543 RID: 17731
		public static ThingDef DropPodLeaving;

		// Token: 0x04004544 RID: 17732
		public static ThingDef ShipChunkIncoming;

		// Token: 0x04004545 RID: 17733
		public static ThingDef CrashedShipPartIncoming;

		// Token: 0x04004546 RID: 17734
		public static ThingDef MeteoriteIncoming;

		// Token: 0x04004547 RID: 17735
		[MayRequireRoyalty]
		public static ThingDef ShuttleIncoming;

		// Token: 0x04004548 RID: 17736
		[MayRequireRoyalty]
		public static ThingDef ShuttleLeaving;

		// Token: 0x04004549 RID: 17737
		[MayRequireRoyalty]
		public static ThingDef ShuttleCrashing;

		// Token: 0x0400454A RID: 17738
		[MayRequireIdeology]
		public static ThingDef SpacedroneIncoming;

		// Token: 0x0400454B RID: 17739
		[MayRequireRoyalty]
		public static ThingDef PawnJumper;

		// Token: 0x0400454C RID: 17740
		public static ThingDef ActiveDropPod;

		// Token: 0x0400454D RID: 17741
		public static ThingDef Fire;

		// Token: 0x0400454E RID: 17742
		public static ThingDef Heart;

		// Token: 0x0400454F RID: 17743
		public static ThingDef ChunkSlagSteel;

		// Token: 0x04004550 RID: 17744
		public static ThingDef SteamGeyser;

		// Token: 0x04004551 RID: 17745
		public static ThingDef Hive;

		// Token: 0x04004552 RID: 17746
		public static ThingDef ShipChunk;

		// Token: 0x04004553 RID: 17747
		public static ThingDef ElephantTusk;

		// Token: 0x04004554 RID: 17748
		public static ThingDef GlowPod;

		// Token: 0x04004555 RID: 17749
		public static ThingDef MinifiedThing;

		// Token: 0x04004556 RID: 17750
		[MayRequireRoyalty]
		public static ThingDef MonumentMarker;

		// Token: 0x04004557 RID: 17751
		[MayRequireRoyalty]
		public static ThingDef PsychicAmplifier;

		// Token: 0x04004558 RID: 17752
		[MayRequireIdeology]
		public static ThingDef Skull;

		// Token: 0x04004559 RID: 17753
		[MayRequireIdeology]
		public static ThingDef Skullspike;

		// Token: 0x0400455A RID: 17754
		public static ThingDef Filth_Blood;

		// Token: 0x0400455B RID: 17755
		public static ThingDef Filth_AmnioticFluid;

		// Token: 0x0400455C RID: 17756
		public static ThingDef Filth_Dirt;

		// Token: 0x0400455D RID: 17757
		public static ThingDef Filth_Vomit;

		// Token: 0x0400455E RID: 17758
		public static ThingDef Filth_AnimalFilth;

		// Token: 0x0400455F RID: 17759
		public static ThingDef Filth_Trash;

		// Token: 0x04004560 RID: 17760
		public static ThingDef Filth_Slime;

		// Token: 0x04004561 RID: 17761
		public static ThingDef Filth_FireFoam;

		// Token: 0x04004562 RID: 17762
		public static ThingDef Filth_Fuel;

		// Token: 0x04004563 RID: 17763
		public static ThingDef Filth_RubbleRock;

		// Token: 0x04004564 RID: 17764
		public static ThingDef Filth_RubbleBuilding;

		// Token: 0x04004565 RID: 17765
		public static ThingDef Filth_CorpseBile;

		// Token: 0x04004566 RID: 17766
		public static ThingDef Filth_Ash;

		// Token: 0x04004567 RID: 17767
		public static ThingDef Filth_MachineBits;

		// Token: 0x04004568 RID: 17768
		public static ThingDef Filth_Water;

		// Token: 0x04004569 RID: 17769
		public static ThingDef Filth_Hair;

		// Token: 0x0400456A RID: 17770
		[MayRequireIdeology]
		public static ThingDef Filth_DriedBlood;

		// Token: 0x0400456B RID: 17771
		[MayRequireIdeology]
		public static ThingDef Filth_ScatteredDocuments;

		// Token: 0x0400456C RID: 17772
		[MayRequireIdeology]
		public static ThingDef Filth_MoldyUniform;

		// Token: 0x0400456D RID: 17773
		[MayRequireIdeology]
		public static ThingDef Filth_PodSlime;

		// Token: 0x0400456E RID: 17774
		[MayRequireIdeology]
		public static ThingDef Filth_OilSmear;

		// Token: 0x0400456F RID: 17775
		public static ThingDef RectTrigger;

		// Token: 0x04004570 RID: 17776
		public static ThingDef TriggerUnfogged;

		// Token: 0x04004571 RID: 17777
		public static ThingDef TriggerContainerEmptied;

		// Token: 0x04004572 RID: 17778
		public static ThingDef Explosion;

		// Token: 0x04004573 RID: 17779
		public static ThingDef Bombardment;

		// Token: 0x04004574 RID: 17780
		public static ThingDef PowerBeam;

		// Token: 0x04004575 RID: 17781
		public static ThingDef SignalAction_Letter;

		// Token: 0x04004576 RID: 17782
		public static ThingDef SignalAction_Ambush;

		// Token: 0x04004577 RID: 17783
		public static ThingDef SignalAction_OpenCasket;

		// Token: 0x04004578 RID: 17784
		public static ThingDef SignalAction_OpenDoor;

		// Token: 0x04004579 RID: 17785
		public static ThingDef SignalAction_Message;

		// Token: 0x0400457A RID: 17786
		public static ThingDef SignalAction_Infestation;

		// Token: 0x0400457B RID: 17787
		public static ThingDef SignalAction_SoundOneShot;

		// Token: 0x0400457C RID: 17788
		public static ThingDef SignalAction_DormancyWakeUp;

		// Token: 0x0400457D RID: 17789
		public static ThingDef SignalAction_Incident;

		// Token: 0x0400457E RID: 17790
		public static ThingDef SignalAction_StartWick;

		// Token: 0x0400457F RID: 17791
		public static ThingDef SignalAction_Delay;

		// Token: 0x04004580 RID: 17792
		public static ThingDef Blight;

		// Token: 0x04004581 RID: 17793
		public static ThingDef Tornado;

		// Token: 0x04004582 RID: 17794
		public static ThingDef TunnelHiveSpawner;

		// Token: 0x04004583 RID: 17795
		[MayRequireRoyalty]
		public static ThingDef Flashstorm;

		// Token: 0x04004584 RID: 17796
		public static ThingDef RadialTrigger;

		// Token: 0x04004585 RID: 17797
		[MayRequireIdeology]
		public static ThingDef TunnelJellySpawner;

		// Token: 0x04004586 RID: 17798
		public static ThingDef Sandstone;

		// Token: 0x04004587 RID: 17799
		public static ThingDef Ship_Beam;

		// Token: 0x04004588 RID: 17800
		public static ThingDef Ship_Reactor;

		// Token: 0x04004589 RID: 17801
		public static ThingDef Ship_CryptosleepCasket;

		// Token: 0x0400458A RID: 17802
		public static ThingDef Ship_ComputerCore;

		// Token: 0x0400458B RID: 17803
		public static ThingDef Ship_Engine;

		// Token: 0x0400458C RID: 17804
		public static ThingDef Ship_SensorCluster;

		// Token: 0x0400458D RID: 17805
		public static ThingDef MineableSteel;

		// Token: 0x0400458E RID: 17806
		public static ThingDef MineableComponentsIndustrial;

		// Token: 0x0400458F RID: 17807
		public static ThingDef MineableGold;

		// Token: 0x04004590 RID: 17808
		public static ThingDef RaisedRocks;

		// Token: 0x04004591 RID: 17809
		public static ThingDef Door;

		// Token: 0x04004592 RID: 17810
		public static ThingDef Wall;

		// Token: 0x04004593 RID: 17811
		public static ThingDef Bed;

		// Token: 0x04004594 RID: 17812
		public static ThingDef Bedroll;

		// Token: 0x04004595 RID: 17813
		public static ThingDef SleepingSpot;

		// Token: 0x04004596 RID: 17814
		public static ThingDef OrbitalTradeBeacon;

		// Token: 0x04004597 RID: 17815
		public static ThingDef NutrientPasteDispenser;

		// Token: 0x04004598 RID: 17816
		public static ThingDef Grave;

		// Token: 0x04004599 RID: 17817
		public static ThingDef Sandbags;

		// Token: 0x0400459A RID: 17818
		public static ThingDef Barricade;

		// Token: 0x0400459B RID: 17819
		public static ThingDef AncientCryptosleepCasket;

		// Token: 0x0400459C RID: 17820
		public static ThingDef CryptosleepCasket;

		// Token: 0x0400459D RID: 17821
		public static ThingDef SolarGenerator;

		// Token: 0x0400459E RID: 17822
		public static ThingDef WoodFiredGenerator;

		// Token: 0x0400459F RID: 17823
		public static ThingDef PowerConduit;

		// Token: 0x040045A0 RID: 17824
		public static ThingDef Battery;

		// Token: 0x040045A1 RID: 17825
		public static ThingDef GeothermalGenerator;

		// Token: 0x040045A2 RID: 17826
		public static ThingDef WatermillGenerator;

		// Token: 0x040045A3 RID: 17827
		public static ThingDef Hopper;

		// Token: 0x040045A4 RID: 17828
		public static ThingDef BilliardsTable;

		// Token: 0x040045A5 RID: 17829
		public static ThingDef Telescope;

		// Token: 0x040045A6 RID: 17830
		public static ThingDef MarriageSpot;

		// Token: 0x040045A7 RID: 17831
		public static ThingDef PartySpot;

		// Token: 0x040045A8 RID: 17832
		public static ThingDef TransportPod;

		// Token: 0x040045A9 RID: 17833
		[MayRequireRoyalty]
		public static ThingDef MeditationSpot;

		// Token: 0x040045AA RID: 17834
		[MayRequireIdeology]
		public static ThingDef RitualSpot;

		// Token: 0x040045AB RID: 17835
		[MayRequireIdeology]
		public static ThingDef Ideogram;

		// Token: 0x040045AC RID: 17836
		public static ThingDef RoyalBed;

		// Token: 0x040045AD RID: 17837
		public static ThingDef TrapSpike;

		// Token: 0x040045AE RID: 17838
		public static ThingDef Cooler;

		// Token: 0x040045AF RID: 17839
		public static ThingDef Heater;

		// Token: 0x040045B0 RID: 17840
		public static ThingDef Snowman;

		// Token: 0x040045B1 RID: 17841
		public static ThingDef WindTurbine;

		// Token: 0x040045B2 RID: 17842
		public static ThingDef FermentingBarrel;

		// Token: 0x040045B3 RID: 17843
		public static ThingDef DeepDrill;

		// Token: 0x040045B4 RID: 17844
		public static ThingDef LongRangeMineralScanner;

		// Token: 0x040045B5 RID: 17845
		public static ThingDef GroundPenetratingScanner;

		// Token: 0x040045B6 RID: 17846
		public static ThingDef CollapsedRocks;

		// Token: 0x040045B7 RID: 17847
		public static ThingDef TorchLamp;

		// Token: 0x040045B8 RID: 17848
		public static ThingDef StandingLamp;

		// Token: 0x040045B9 RID: 17849
		public static ThingDef Campfire;

		// Token: 0x040045BA RID: 17850
		public static ThingDef FirefoamPopper;

		// Token: 0x040045BB RID: 17851
		public static ThingDef PassiveCooler;

		// Token: 0x040045BC RID: 17852
		public static ThingDef CaravanPackingSpot;

		// Token: 0x040045BD RID: 17853
		public static ThingDef PlantPot;

		// Token: 0x040045BE RID: 17854
		public static ThingDef Table1x2c;

		// Token: 0x040045BF RID: 17855
		public static ThingDef Table2x2c;

		// Token: 0x040045C0 RID: 17856
		public static ThingDef Table3x3c;

		// Token: 0x040045C1 RID: 17857
		public static ThingDef TableButcher;

		// Token: 0x040045C2 RID: 17858
		public static ThingDef DiningChair;

		// Token: 0x040045C3 RID: 17859
		public static ThingDef Stool;

		// Token: 0x040045C4 RID: 17860
		public static ThingDef PsychicEmanator;

		// Token: 0x040045C5 RID: 17861
		public static ThingDef VanometricPowerCell;

		// Token: 0x040045C6 RID: 17862
		public static ThingDef InfiniteChemreactor;

		// Token: 0x040045C7 RID: 17863
		public static ThingDef CommsConsole;

		// Token: 0x040045C8 RID: 17864
		public static ThingDef Sarcophagus;

		// Token: 0x040045C9 RID: 17865
		public static ThingDef Turret_Mortar;

		// Token: 0x040045CA RID: 17866
		public static ThingDef Turret_MiniTurret;

		// Token: 0x040045CB RID: 17867
		[MayRequireRoyalty]
		public static ThingDef Turret_AutoMiniTurret;

		// Token: 0x040045CC RID: 17868
		[MayRequireRoyalty]
		public static ThingDef MechCapsule;

		// Token: 0x040045CD RID: 17869
		public static ThingDef DefoliatorShipPart;

		// Token: 0x040045CE RID: 17870
		public static ThingDef PsychicDronerShipPart;

		// Token: 0x040045CF RID: 17871
		public static ThingDef Column;

		// Token: 0x040045D0 RID: 17872
		public static ThingDef Urn;

		// Token: 0x040045D1 RID: 17873
		public static ThingDef SteleLarge;

		// Token: 0x040045D2 RID: 17874
		public static ThingDef SteleGrand;

		// Token: 0x040045D3 RID: 17875
		public static ThingDef EggBox;

		// Token: 0x040045D4 RID: 17876
		public static ThingDef Fence;

		// Token: 0x040045D5 RID: 17877
		public static ThingDef SculptureLarge;

		// Token: 0x040045D6 RID: 17878
		[MayRequireRoyalty]
		public static ThingDef PsychicDroner;

		// Token: 0x040045D7 RID: 17879
		[MayRequireRoyalty]
		public static ThingDef Shuttle;

		// Token: 0x040045D8 RID: 17880
		[MayRequireRoyalty]
		public static ThingDef Drape;

		// Token: 0x040045D9 RID: 17881
		[MayRequireRoyalty]
		public static ThingDef ActivatorProximity;

		// Token: 0x040045DA RID: 17882
		[MayRequireRoyalty]
		public static ThingDef ShieldGeneratorBullets;

		// Token: 0x040045DB RID: 17883
		[MayRequireRoyalty]
		public static ThingDef ShieldGeneratorMortar;

		// Token: 0x040045DC RID: 17884
		[MayRequireRoyalty]
		public static ThingDef BroadshieldProjector;

		// Token: 0x040045DD RID: 17885
		[MayRequireRoyalty]
		public static ThingDef Brazier;

		// Token: 0x040045DE RID: 17886
		[MayRequireRoyalty]
		public static ThingDef ShuttleCrashed;

		// Token: 0x040045DF RID: 17887
		[MayRequireIdeology]
		public static ThingDef StylingStation;

		// Token: 0x040045E0 RID: 17888
		[MayRequireIdeology]
		public static ThingDef Spacedrone;

		// Token: 0x040045E1 RID: 17889
		[MayRequireIdeology]
		public static ThingDef Reliquary;

		// Token: 0x040045E2 RID: 17890
		[MayRequireIdeology]
		public static ThingDef Lectern;

		// Token: 0x040045E3 RID: 17891
		[MayRequireIdeology]
		public static ThingDef AncientTerminal;

		// Token: 0x040045E4 RID: 17892
		[MayRequireIdeology]
		public static ThingDef AncientSystemRack;

		// Token: 0x040045E5 RID: 17893
		[MayRequireIdeology]
		public static ThingDef AncientEquipmentBlocks;

		// Token: 0x040045E6 RID: 17894
		[MayRequireIdeology]
		public static ThingDef AncientMachine;

		// Token: 0x040045E7 RID: 17895
		[MayRequireIdeology]
		public static ThingDef AncientStorageCylinder;

		// Token: 0x040045E8 RID: 17896
		[MayRequireIdeology]
		public static ThingDef AncientCommsConsole;

		// Token: 0x040045E9 RID: 17897
		[MayRequireIdeology]
		public static ThingDef AncientHermeticCrate;

		// Token: 0x040045EA RID: 17898
		[MayRequireIdeology]
		public static ThingDef LightBall;

		// Token: 0x040045EB RID: 17899
		[MayRequireIdeology]
		public static ThingDef Loudspeaker;

		// Token: 0x040045EC RID: 17900
		[MayRequireIdeology]
		public static ThingDef Drum;

		// Token: 0x040045ED RID: 17901
		[MayRequireIdeology]
		public static ThingDef AncientLamp;

		// Token: 0x040045EE RID: 17902
		[MayRequireIdeology]
		public static ThingDef SkyLantern;

		// Token: 0x040045EF RID: 17903
		[MayRequireIdeology]
		public static ThingDef ArchonexusCore;

		// Token: 0x040045F0 RID: 17904
		[MayRequireIdeology]
		public static ThingDef ArchonexusSuperstructureMajor;

		// Token: 0x040045F1 RID: 17905
		[MayRequireIdeology]
		public static ThingDef ArchonexusSuperstructureMajorStudiable;

		// Token: 0x040045F2 RID: 17906
		[MayRequireIdeology]
		public static ThingDef ArchonexusSuperstructureMinor;

		// Token: 0x040045F3 RID: 17907
		[MayRequireIdeology]
		public static ThingDef AncientBed;

		// Token: 0x040045F4 RID: 17908
		[MayRequireIdeology]
		public static ThingDef AncientLockerBank;

		// Token: 0x040045F5 RID: 17909
		[MayRequireIdeology]
		public static ThingDef AncientBarrel;

		// Token: 0x040045F6 RID: 17910
		[MayRequireIdeology]
		public static ThingDef AncientCrate;

		// Token: 0x040045F7 RID: 17911
		[MayRequireIdeology]
		public static ThingDef AncientFence;

		// Token: 0x040045F8 RID: 17912
		[MayRequireIdeology]
		public static ThingDef AncientRazorWire;

		// Token: 0x040045F9 RID: 17913
		[MayRequireIdeology]
		public static ThingDef AncientMegaCannonTripod;

		// Token: 0x040045FA RID: 17914
		[MayRequireIdeology]
		public static ThingDef AncientMegaCannonBarrel;

		// Token: 0x040045FB RID: 17915
		[MayRequireIdeology]
		public static ThingDef AncientMechanoidShell;

		// Token: 0x040045FC RID: 17916
		[MayRequireIdeology]
		public static ThingDef AncientMechDropBeacon;

		// Token: 0x040045FD RID: 17917
		[MayRequireIdeology]
		public static ThingDef AncientTankTrap;

		// Token: 0x040045FE RID: 17918
		[MayRequireIdeology]
		public static ThingDef AncientRustedCar;

		// Token: 0x040045FF RID: 17919
		[MayRequireIdeology]
		public static ThingDef AncientRustedJeep;

		// Token: 0x04004600 RID: 17920
		[MayRequireIdeology]
		public static ThingDef AncientGenerator;

		// Token: 0x04004601 RID: 17921
		[MayRequireIdeology]
		public static ThingDef AncientOperatingTable;

		// Token: 0x04004602 RID: 17922
		[MayRequireIdeology]
		public static ThingDef AncientDisplayBank;

		// Token: 0x04004603 RID: 17923
		[MayRequireIdeology]
		public static ThingDef AncientSecurityTurret;

		// Token: 0x04004604 RID: 17924
		[MayRequireIdeology]
		public static ThingDef AncientRustedCarFrame;

		// Token: 0x04004605 RID: 17925
		[MayRequireIdeology]
		public static ThingDef AncientRustedEngineBlock;

		// Token: 0x04004606 RID: 17926
		[MayRequireIdeology]
		public static ThingDef BiosculpterPod;

		// Token: 0x04004607 RID: 17927
		[MayRequireIdeology]
		public static ThingDef NeuralSupercharger;

		// Token: 0x04004608 RID: 17928
		[MayRequireIdeology]
		public static ThingDef AncientWarwalkerTorso;

		// Token: 0x04004609 RID: 17929
		[MayRequireIdeology]
		public static ThingDef AncientWarwalkerClaw;

		// Token: 0x0400460A RID: 17930
		[MayRequireIdeology]
		public static ThingDef AncientWarwalkerLeg;

		// Token: 0x0400460B RID: 17931
		[MayRequireIdeology]
		public static ThingDef AncientMiniWarwalkerRemains;

		// Token: 0x0400460C RID: 17932
		[MayRequireIdeology]
		public static ThingDef AncientWarspiderRemains;

		// Token: 0x0400460D RID: 17933
		[MayRequireIdeology]
		public static ThingDef AncientShipBeacon;

		// Token: 0x0400460E RID: 17934
		[MayRequireIdeology]
		public static ThingDef AncientSecurityCrate;

		// Token: 0x0400460F RID: 17935
		[MayRequireIdeology]
		public static ThingDef AncientCryptosleepPod;

		// Token: 0x04004610 RID: 17936
		[MayRequireIdeology]
		public static ThingDef AncientEnemyTerminal;

		// Token: 0x04004611 RID: 17937
		[MayRequireIdeology]
		public static ThingDef AncientFuelNode;

		// Token: 0x04004612 RID: 17938
		public static ThingDef Plant_Potato;

		// Token: 0x04004613 RID: 17939
		public static ThingDef Plant_TreeOak;

		// Token: 0x04004614 RID: 17940
		public static ThingDef Plant_Grass;

		// Token: 0x04004615 RID: 17941
		public static ThingDef Plant_Ambrosia;

		// Token: 0x04004616 RID: 17942
		public static ThingDef Plant_Dandelion;

		// Token: 0x04004617 RID: 17943
		public static ThingDef BurnedTree;

		// Token: 0x04004618 RID: 17944
		[MayRequireRoyalty]
		public static ThingDef Plant_TreeAnima;

		// Token: 0x04004619 RID: 17945
		[MayRequireRoyalty]
		public static ThingDef AnimusStone;

		// Token: 0x0400461A RID: 17946
		[MayRequireRoyalty]
		public static ThingDef NatureShrine_Small;

		// Token: 0x0400461B RID: 17947
		[MayRequireRoyalty]
		public static ThingDef NatureShrine_Large;

		// Token: 0x0400461C RID: 17948
		[MayRequireRoyalty]
		public static ThingDef Plant_GrassAnima;

		// Token: 0x0400461D RID: 17949
		[MayRequireIdeology]
		public static ThingDef Nutrifungus;

		// Token: 0x0400461E RID: 17950
		[MayRequireIdeology]
		public static ThingDef Plant_PodGauranlen;

		// Token: 0x0400461F RID: 17951
		[MayRequireIdeology]
		public static ThingDef Plant_TreeGauranlen;

		// Token: 0x04004620 RID: 17952
		[MayRequireIdeology]
		public static ThingDef DryadCocoon;

		// Token: 0x04004621 RID: 17953
		[MayRequireIdeology]
		public static ThingDef GaumakerCocoon;

		// Token: 0x04004622 RID: 17954
		public static ThingDef Human;

		// Token: 0x04004623 RID: 17955
		public static ThingDef Muffalo;

		// Token: 0x04004624 RID: 17956
		public static ThingDef Dromedary;

		// Token: 0x04004625 RID: 17957
		public static ThingDef Cow;

		// Token: 0x04004626 RID: 17958
		public static ThingDef Goat;

		// Token: 0x04004627 RID: 17959
		public static ThingDef Chicken;

		// Token: 0x04004628 RID: 17960
		public static ThingDef Thrumbo;

		// Token: 0x04004629 RID: 17961
		public static ThingDef Spark;

		// Token: 0x0400462A RID: 17962
		public static ThingDef Apparel_ShieldBelt;

		// Token: 0x0400462B RID: 17963
		public static ThingDef Apparel_SmokepopBelt;

		// Token: 0x0400462C RID: 17964
		public static ThingDef Apparel_Parka;

		// Token: 0x0400462D RID: 17965
		public static ThingDef Apparel_Tuque;

		// Token: 0x0400462E RID: 17966
		[MayRequireRoyalty]
		public static ThingDef Apparel_RobeRoyal;

		// Token: 0x0400462F RID: 17967
		[MayRequireIdeology]
		public static ThingDef Apparel_BodyStrap;

		// Token: 0x04004630 RID: 17968
		[MayRequireIdeology]
		public static ThingDef Apparel_Collar;

		// Token: 0x04004631 RID: 17969
		[MayRequireIdeology]
		public static ThingDef Dye;

		// Token: 0x04004632 RID: 17970
		[MayRequireIdeology]
		public static ThingDef Apparel_Blindfold;

		// Token: 0x04004633 RID: 17971
		public static ThingDef Mote_Text;

		// Token: 0x04004634 RID: 17972
		public static ThingDef Mote_TempRoof;

		// Token: 0x04004635 RID: 17973
		public static ThingDef Mote_ColonistFleeing;

		// Token: 0x04004636 RID: 17974
		public static ThingDef Mote_ColonistAttacking;

		// Token: 0x04004637 RID: 17975
		public static ThingDef Mote_Danger;

		// Token: 0x04004638 RID: 17976
		public static ThingDef Mote_Speech;

		// Token: 0x04004639 RID: 17977
		public static ThingDef Mote_ThoughtBad;

		// Token: 0x0400463A RID: 17978
		public static ThingDef Mote_ThoughtGood;

		// Token: 0x0400463B RID: 17979
		public static ThingDef Mote_Stun;

		// Token: 0x0400463C RID: 17980
		public static ThingDef Mote_Bombardment;

		// Token: 0x0400463D RID: 17981
		public static ThingDef Mote_PowerBeam;

		// Token: 0x0400463E RID: 17982
		public static ThingDef Mote_Leaf;

		// Token: 0x0400463F RID: 17983
		public static ThingDef Mote_ForceJob;

		// Token: 0x04004640 RID: 17984
		public static ThingDef Mote_ForceJobMaintained;

		// Token: 0x04004641 RID: 17985
		[MayRequireRoyalty]
		public static ThingDef Mote_CastPsycast;

		// Token: 0x04004642 RID: 17986
		[MayRequireRoyalty]
		public static ThingDef Mote_PsychicLinkLine;

		// Token: 0x04004643 RID: 17987
		[MayRequireRoyalty]
		public static ThingDef Mote_PsychicLinkPulse;

		// Token: 0x04004644 RID: 17988
		[MayRequireRoyalty]
		public static ThingDef Mote_PsyfocusPulse;

		// Token: 0x04004645 RID: 17989
		[MayRequireRoyalty]
		public static ThingDef Mote_Bestow;

		// Token: 0x04004646 RID: 17990
		[MayRequireIdeology]
		public static ThingDef Mote_LightBall;

		// Token: 0x04004647 RID: 17991
		[MayRequireIdeology]
		public static ThingDef Mote_LightBallLights;

		// Token: 0x04004648 RID: 17992
		[MayRequireIdeology]
		public static ThingDef Mote_LoudspeakerLights;

		// Token: 0x04004649 RID: 17993
		[MayRequireIdeology]
		public static ThingDef Mote_RolePositionHighlight;

		// Token: 0x0400464A RID: 17994
		[MayRequireIdeology]
		public static ThingDef Mote_RolePawnHighlight;

		// Token: 0x0400464B RID: 17995
		[MayRequireIdeology]
		public static ThingDef Mote_GauranlenCasteChanged;

		// Token: 0x0400464C RID: 17996
		public static ThingDef Gas_Smoke;

		// Token: 0x0400464D RID: 17997
		[MayRequireRoyalty]
		public static ThingDef Throne;

		// Token: 0x0400464E RID: 17998
		[MayRequireRoyalty]
		public static ThingDef Harp;

		// Token: 0x0400464F RID: 17999
		[MayRequireRoyalty]
		public static ThingDef Harpsichord;

		// Token: 0x04004650 RID: 18000
		[MayRequireRoyalty]
		public static ThingDef Piano;

		// Token: 0x04004651 RID: 18001
		[MayRequireRoyalty]
		public static ThingDef MeleeWeapon_PsyfocusStaff;

		// Token: 0x04004652 RID: 18002
		[MayRequireRoyalty]
		public static ThingDef ShipLandingBeacon;

		// Token: 0x04004653 RID: 18003
		[MayRequireRoyalty]
		public static ThingDef ActivatorCountdown;
	}
}
