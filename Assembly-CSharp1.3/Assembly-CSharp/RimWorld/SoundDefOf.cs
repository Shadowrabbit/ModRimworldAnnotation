using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001404 RID: 5124
	[DefOf]
	public static class SoundDefOf
	{
		// Token: 0x06007CF8 RID: 31992 RVA: 0x002C4594 File Offset: 0x002C2794
		static SoundDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(SoundDefOf));
		}

		// Token: 0x04004654 RID: 18004
		public static SoundDef Tick_High;

		// Token: 0x04004655 RID: 18005
		public static SoundDef Tick_Low;

		// Token: 0x04004656 RID: 18006
		public static SoundDef Tick_Tiny;

		// Token: 0x04004657 RID: 18007
		public static SoundDef DragElement;

		// Token: 0x04004658 RID: 18008
		public static SoundDef DropElement;

		// Token: 0x04004659 RID: 18009
		public static SoundDef Crunch;

		// Token: 0x0400465A RID: 18010
		public static SoundDef Click;

		// Token: 0x0400465B RID: 18011
		public static SoundDef ClickReject;

		// Token: 0x0400465C RID: 18012
		public static SoundDef CancelMode;

		// Token: 0x0400465D RID: 18013
		public static SoundDef TabClose;

		// Token: 0x0400465E RID: 18014
		public static SoundDef TabOpen;

		// Token: 0x0400465F RID: 18015
		public static SoundDef Checkbox_TurnedOff;

		// Token: 0x04004660 RID: 18016
		public static SoundDef Checkbox_TurnedOn;

		// Token: 0x04004661 RID: 18017
		public static SoundDef RowTabSelect;

		// Token: 0x04004662 RID: 18018
		public static SoundDef ArchitectCategorySelect;

		// Token: 0x04004663 RID: 18019
		public static SoundDef ExecuteTrade;

		// Token: 0x04004664 RID: 18020
		public static SoundDef FloatMenu_Open;

		// Token: 0x04004665 RID: 18021
		public static SoundDef FloatMenu_Cancel;

		// Token: 0x04004666 RID: 18022
		public static SoundDef DialogBoxAppear;

		// Token: 0x04004667 RID: 18023
		public static SoundDef TutorMessageAppear;

		// Token: 0x04004668 RID: 18024
		public static SoundDef TinyBell;

		// Token: 0x04004669 RID: 18025
		public static SoundDef PageChange;

		// Token: 0x0400466A RID: 18026
		public static SoundDef DragSlider;

		// Token: 0x0400466B RID: 18027
		public static SoundDef DragGoto;

		// Token: 0x0400466C RID: 18028
		public static SoundDef Lesson_Activated;

		// Token: 0x0400466D RID: 18029
		public static SoundDef Lesson_Deactivated;

		// Token: 0x0400466E RID: 18030
		public static SoundDef DraftOn;

		// Token: 0x0400466F RID: 18031
		public static SoundDef DraftOff;

		// Token: 0x04004670 RID: 18032
		public static SoundDef CommsWindow_Open;

		// Token: 0x04004671 RID: 18033
		public static SoundDef CommsWindow_Close;

		// Token: 0x04004672 RID: 18034
		public static SoundDef RadioComms_Ambience;

		// Token: 0x04004673 RID: 18035
		public static SoundDef InfoCard_Open;

		// Token: 0x04004674 RID: 18036
		public static SoundDef InfoCard_Close;

		// Token: 0x04004675 RID: 18037
		public static SoundDef Clock_Stop;

		// Token: 0x04004676 RID: 18038
		public static SoundDef Clock_Normal;

		// Token: 0x04004677 RID: 18039
		public static SoundDef Clock_Fast;

		// Token: 0x04004678 RID: 18040
		public static SoundDef Clock_Superfast;

		// Token: 0x04004679 RID: 18041
		public static SoundDef Quest_Accepted;

		// Token: 0x0400467A RID: 18042
		public static SoundDef Quest_Succeded;

		// Token: 0x0400467B RID: 18043
		public static SoundDef Quest_Concluded;

		// Token: 0x0400467C RID: 18044
		public static SoundDef Quest_Failed;

		// Token: 0x0400467D RID: 18045
		public static SoundDef RitualConclusion_Positive;

		// Token: 0x0400467E RID: 18046
		public static SoundDef RitualConclusion_Negative;

		// Token: 0x0400467F RID: 18047
		[MayRequireIdeology]
		public static SoundDef DislikedWorkTypeActivated;

		// Token: 0x04004680 RID: 18048
		[MayRequireIdeology]
		public static SoundDef GameStartSting_FirstArchonexusCycle;

		// Token: 0x04004681 RID: 18049
		[MayRequireIdeology]
		public static SoundDef GameStartSting_SecondArchonexusCycle;

		// Token: 0x04004682 RID: 18050
		public static SoundDef Mouseover_Standard;

		// Token: 0x04004683 RID: 18051
		public static SoundDef Mouseover_Thump;

		// Token: 0x04004684 RID: 18052
		public static SoundDef Mouseover_Category;

		// Token: 0x04004685 RID: 18053
		public static SoundDef Mouseover_Command;

		// Token: 0x04004686 RID: 18054
		public static SoundDef Mouseover_ButtonToggle;

		// Token: 0x04004687 RID: 18055
		public static SoundDef Mouseover_Tab;

		// Token: 0x04004688 RID: 18056
		public static SoundDef ThingSelected;

		// Token: 0x04004689 RID: 18057
		public static SoundDef MapSelected;

		// Token: 0x0400468A RID: 18058
		public static SoundDef ColonistSelected;

		// Token: 0x0400468B RID: 18059
		public static SoundDef ColonistOrdered;

		// Token: 0x0400468C RID: 18060
		public static SoundDef LetterArrive_BadUrgent;

		// Token: 0x0400468D RID: 18061
		public static SoundDef LetterArrive;

		// Token: 0x0400468E RID: 18062
		public static SoundDef Designate_DragStandard;

		// Token: 0x0400468F RID: 18063
		public static SoundDef Designate_DragStandard_Changed;

		// Token: 0x04004690 RID: 18064
		public static SoundDef Designate_DragStandard_Changed_NoCam;

		// Token: 0x04004691 RID: 18065
		public static SoundDef Designate_DragBuilding;

		// Token: 0x04004692 RID: 18066
		public static SoundDef Designate_DragAreaAdd;

		// Token: 0x04004693 RID: 18067
		public static SoundDef Designate_DragAreaDelete;

		// Token: 0x04004694 RID: 18068
		public static SoundDef Designate_Failed;

		// Token: 0x04004695 RID: 18069
		public static SoundDef Designate_ZoneAdd;

		// Token: 0x04004696 RID: 18070
		public static SoundDef Designate_ZoneDelete;

		// Token: 0x04004697 RID: 18071
		public static SoundDef Designate_Cancel;

		// Token: 0x04004698 RID: 18072
		public static SoundDef Designate_Haul;

		// Token: 0x04004699 RID: 18073
		public static SoundDef Designate_Mine;

		// Token: 0x0400469A RID: 18074
		public static SoundDef Designate_SmoothSurface;

		// Token: 0x0400469B RID: 18075
		public static SoundDef Designate_PlanRemove;

		// Token: 0x0400469C RID: 18076
		public static SoundDef Designate_PlanAdd;

		// Token: 0x0400469D RID: 18077
		public static SoundDef Designate_Claim;

		// Token: 0x0400469E RID: 18078
		public static SoundDef Designate_Deconstruct;

		// Token: 0x0400469F RID: 18079
		public static SoundDef Designate_Hunt;

		// Token: 0x040046A0 RID: 18080
		public static SoundDef Designate_PlaceBuilding;

		// Token: 0x040046A1 RID: 18081
		public static SoundDef Designate_CutPlants;

		// Token: 0x040046A2 RID: 18082
		public static SoundDef Designate_Harvest;

		// Token: 0x040046A3 RID: 18083
		public static SoundDef Designate_ReleaseToWild;

		// Token: 0x040046A4 RID: 18084
		[MayRequireIdeology]
		public static SoundDef Designate_ExtractSkull;

		// Token: 0x040046A5 RID: 18085
		public static SoundDef Standard_Drop;

		// Token: 0x040046A6 RID: 18086
		public static SoundDef Standard_Pickup;

		// Token: 0x040046A7 RID: 18087
		public static SoundDef BulletImpact_Ground;

		// Token: 0x040046A8 RID: 18088
		public static SoundDef Ambient_AltitudeWind;

		// Token: 0x040046A9 RID: 18089
		public static SoundDef Ambient_Space;

		// Token: 0x040046AA RID: 18090
		public static SoundDef Power_OnSmall;

		// Token: 0x040046AB RID: 18091
		public static SoundDef Power_OffSmall;

		// Token: 0x040046AC RID: 18092
		public static SoundDef Thunder_OnMap;

		// Token: 0x040046AD RID: 18093
		public static SoundDef Thunder_OffMap;

		// Token: 0x040046AE RID: 18094
		public static SoundDef Interact_CleanFilth;

		// Token: 0x040046AF RID: 18095
		public static SoundDef Interact_Sow;

		// Token: 0x040046B0 RID: 18096
		public static SoundDef Interact_Tend;

		// Token: 0x040046B1 RID: 18097
		public static SoundDef Interact_BeatFire;

		// Token: 0x040046B2 RID: 18098
		public static SoundDef Interact_Ignite;

		// Token: 0x040046B3 RID: 18099
		public static SoundDef Roof_Start;

		// Token: 0x040046B4 RID: 18100
		public static SoundDef Roof_Finish;

		// Token: 0x040046B5 RID: 18101
		public static SoundDef Roof_Collapse;

		// Token: 0x040046B6 RID: 18102
		public static SoundDef PsychicPulseGlobal;

		// Token: 0x040046B7 RID: 18103
		public static SoundDef PsychicSootheGlobal;

		// Token: 0x040046B8 RID: 18104
		public static SoundDef GeyserSpray;

		// Token: 0x040046B9 RID: 18105
		public static SoundDef TurretAcquireTarget;

		// Token: 0x040046BA RID: 18106
		public static SoundDef FlickSwitch;

		// Token: 0x040046BB RID: 18107
		public static SoundDef PlayBilliards;

		// Token: 0x040046BC RID: 18108
		public static SoundDef Building_Complete;

		// Token: 0x040046BD RID: 18109
		public static SoundDef RawMeat_Eat;

		// Token: 0x040046BE RID: 18110
		public static SoundDef HissSmall;

		// Token: 0x040046BF RID: 18111
		public static SoundDef HissJet;

		// Token: 0x040046C0 RID: 18112
		public static SoundDef MetalHitImportant;

		// Token: 0x040046C1 RID: 18113
		public static SoundDef Door_OpenPowered;

		// Token: 0x040046C2 RID: 18114
		public static SoundDef Door_ClosePowered;

		// Token: 0x040046C3 RID: 18115
		public static SoundDef Door_OpenManual;

		// Token: 0x040046C4 RID: 18116
		public static SoundDef Door_CloseManual;

		// Token: 0x040046C5 RID: 18117
		public static SoundDef EnergyShield_AbsorbDamage;

		// Token: 0x040046C6 RID: 18118
		public static SoundDef EnergyShield_Reset;

		// Token: 0x040046C7 RID: 18119
		public static SoundDef EnergyShield_Broken;

		// Token: 0x040046C8 RID: 18120
		public static SoundDef Pawn_Melee_Punch_HitPawn;

		// Token: 0x040046C9 RID: 18121
		public static SoundDef Pawn_Melee_Punch_HitBuilding;

		// Token: 0x040046CA RID: 18122
		public static SoundDef Pawn_Melee_Punch_Miss;

		// Token: 0x040046CB RID: 18123
		public static SoundDef Artillery_ShellLoaded;

		// Token: 0x040046CC RID: 18124
		public static SoundDef TechMedicineUsed;

		// Token: 0x040046CD RID: 18125
		public static SoundDef OrbitalBeam;

		// Token: 0x040046CE RID: 18126
		public static SoundDef DropPod_Open;

		// Token: 0x040046CF RID: 18127
		public static SoundDef Building_Deconstructed;

		// Token: 0x040046D0 RID: 18128
		public static SoundDef CryptosleepCasket_Accept;

		// Token: 0x040046D1 RID: 18129
		public static SoundDef CryptosleepCasket_Eject;

		// Token: 0x040046D2 RID: 18130
		public static SoundDef TrapSpring;

		// Token: 0x040046D3 RID: 18131
		public static SoundDef TrapArm;

		// Token: 0x040046D4 RID: 18132
		public static SoundDef FireBurning;

		// Token: 0x040046D5 RID: 18133
		public static SoundDef Vomit;

		// Token: 0x040046D6 RID: 18134
		public static SoundDef Roping;

		// Token: 0x040046D7 RID: 18135
		public static SoundDef ResearchStart;

		// Token: 0x040046D8 RID: 18136
		public static SoundDef ThingUninstalled;

		// Token: 0x040046D9 RID: 18137
		public static SoundDef ShipTakeoff;

		// Token: 0x040046DA RID: 18138
		public static SoundDef Corpse_Drop;

		// Token: 0x040046DB RID: 18139
		public static SoundDef Tornado;

		// Token: 0x040046DC RID: 18140
		public static SoundDef Tunnel;

		// Token: 0x040046DD RID: 18141
		public static SoundDef Hive_Spawn;

		// Token: 0x040046DE RID: 18142
		public static SoundDef Interceptor_BlockProjectile;

		// Token: 0x040046DF RID: 18143
		public static SoundDef MechanoidsWakeUp;

		// Token: 0x040046E0 RID: 18144
		public static SoundDef FlashstormAmbience;

		// Token: 0x040046E1 RID: 18145
		public static SoundDef Recipe_Surgery;

		// Token: 0x040046E2 RID: 18146
		public static SoundDef Execute_Cut;

		// Token: 0x040046E3 RID: 18147
		[MayRequireIdeology]
		public static SoundDef Archotech_Invoked;

		// Token: 0x040046E4 RID: 18148
		[MayRequireIdeology]
		public static SoundDef InsectsWakeUp;

		// Token: 0x040046E5 RID: 18149
		public static SoundDef MechSerumUsed;

		// Token: 0x040046E6 RID: 18150
		public static SoundDef WaterMill_Ambience;

		// Token: 0x040046E7 RID: 18151
		public static SoundDef ShipReactor_Startup;

		// Token: 0x040046E8 RID: 18152
		public static SoundDef Standard_Reload;

		// Token: 0x040046E9 RID: 18153
		public static SoundDef WindTurbine_Ambience;

		// Token: 0x040046EA RID: 18154
		public static SoundDef Bombardment_PreImpact;

		// Token: 0x040046EB RID: 18155
		[MayRequireRoyalty]
		public static SoundDef PsycastPsychicEffect;

		// Token: 0x040046EC RID: 18156
		[MayRequireRoyalty]
		public static SoundDef PsycastPsychicPulse;

		// Token: 0x040046ED RID: 18157
		[MayRequireRoyalty]
		public static SoundDef Psycast_Skip_Pulse;

		// Token: 0x040046EE RID: 18158
		[MayRequireRoyalty]
		public static SoundDef Psycast_Skip_Exit;

		// Token: 0x040046EF RID: 18159
		[MayRequireRoyalty]
		public static SoundDef Psycast_Skip_Entry;

		// Token: 0x040046F0 RID: 18160
		[MayRequireRoyalty]
		public static SoundDef PsycastCastLoop;

		// Token: 0x040046F1 RID: 18161
		[MayRequireRoyalty]
		public static SoundDef PsychicEntropyOverloaded;

		// Token: 0x040046F2 RID: 18162
		[MayRequireRoyalty]
		public static SoundDef PsychicEntropyHyperloaded;

		// Token: 0x040046F3 RID: 18163
		[MayRequireRoyalty]
		public static SoundDef PsychicEntropyBrainCharring;

		// Token: 0x040046F4 RID: 18164
		[MayRequireRoyalty]
		public static SoundDef PsychicEntropyBrainRoasting;

		// Token: 0x040046F5 RID: 18165
		[MayRequireRoyalty]
		public static SoundDef MeditationGainPsyfocus;

		// Token: 0x040046F6 RID: 18166
		[MayRequireRoyalty]
		public static SoundDef MechClusterDefeated;

		// Token: 0x040046F7 RID: 18167
		[MayRequireRoyalty]
		public static SoundDef TechprintApplied;

		// Token: 0x040046F8 RID: 18168
		[MayRequireRoyalty]
		public static SoundDef Bestowing_Start;

		// Token: 0x040046F9 RID: 18169
		[MayRequireRoyalty]
		public static SoundDef Bestowing_Warmup;

		// Token: 0x040046FA RID: 18170
		[MayRequireRoyalty]
		public static SoundDef Bestowing_Finished;

		// Token: 0x040046FB RID: 18171
		[MayRequireRoyalty]
		public static SoundDef OrbitalStrike_Ordered;

		// Token: 0x040046FC RID: 18172
		[MayRequireRoyalty]
		public static SoundDef Broadshield_Startup;

		// Token: 0x040046FD RID: 18173
		[MayRequireIdeology]
		public static SoundDef Hacking_Started;

		// Token: 0x040046FE RID: 18174
		[MayRequireIdeology]
		public static SoundDef Hacking_InProgress;

		// Token: 0x040046FF RID: 18175
		[MayRequireIdeology]
		public static SoundDef Hacking_Completed;

		// Token: 0x04004700 RID: 18176
		[MayRequireIdeology]
		public static SoundDef Hacking_Suspended;

		// Token: 0x04004701 RID: 18177
		[MayRequireIdeology]
		public static SoundDef Speech_Leader_Male;

		// Token: 0x04004702 RID: 18178
		[MayRequireIdeology]
		public static SoundDef Speech_Leader_Female;

		// Token: 0x04004703 RID: 18179
		[MayRequireIdeology]
		public static SoundDef Relic_Installed;

		// Token: 0x04004704 RID: 18180
		[MayRequireIdeology]
		public static SoundDef Interact_Prune;

		// Token: 0x04004705 RID: 18181
		[MayRequireIdeology]
		public static SoundDef HairCutting;

		// Token: 0x04004706 RID: 18182
		[MayRequireIdeology]
		public static SoundDef Pawn_Dryad_Spawn;

		// Token: 0x04004707 RID: 18183
		[MayRequireIdeology]
		public static SoundDef GauranlenProductionModeSet;

		// Token: 0x04004708 RID: 18184
		[MayRequireIdeology]
		public static SoundDef GauranlenConnectionTorn;

		// Token: 0x04004709 RID: 18185
		[MayRequireIdeology]
		public static SoundDef Pawn_EnterDryadPod;

		// Token: 0x0400470A RID: 18186
		[MayRequireIdeology]
		public static SoundDef AncientRelicRoomReveal;

		// Token: 0x0400470B RID: 18187
		[MayRequireIdeology]
		public static SoundDef AncientRelicTakenAlarm;

		// Token: 0x0400470C RID: 18188
		[MayRequireIdeology]
		public static SoundDef ArchonexusThreatsAwakened_Alarm;

		// Token: 0x0400470D RID: 18189
		[MayRequireIdeology]
		public static SoundDef CrateOpeningStarted;

		// Token: 0x0400470E RID: 18190
		[MayRequireIdeology]
		public static SoundDef RelicHuntInstallationFound;

		// Token: 0x0400470F RID: 18191
		[MayRequireIdeology]
		public static SoundDef DuelMusic;

		// Token: 0x04004710 RID: 18192
		[MayRequireIdeology]
		public static SoundDef DanceParty_NoMusic;

		// Token: 0x04004711 RID: 18193
		[MayRequireIdeology]
		public static SoundDef Interact_ReleaseSkylantern;

		// Token: 0x04004712 RID: 18194
		[MayRequireIdeology]
		public static SoundDef Interact_RecolorApparel;

		// Token: 0x04004713 RID: 18195
		[MayRequireIdeology]
		public static SoundDef RitualSustainer_Theist;

		// Token: 0x04004714 RID: 18196
		public static SoundDef GameStartSting;

		// Token: 0x04004715 RID: 18197
		public static SoundDef PlanetkillerImpact;
	}
}
