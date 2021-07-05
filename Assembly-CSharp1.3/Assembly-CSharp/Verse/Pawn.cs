using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse.AI;
using Verse.AI.Group;

namespace Verse
{
	// Token: 0x020002E6 RID: 742
	public class Pawn : ThingWithComps, IStrippable, IBillGiver, IVerbOwner, ITrader, IAttackTarget, ILoadReferenceable, IAttackTargetSearcher, IThingHolder
	{
		// Token: 0x1700040D RID: 1037
		// (get) Token: 0x0600141E RID: 5150 RVA: 0x00072153 File Offset: 0x00070353
		// (set) Token: 0x0600141F RID: 5151 RVA: 0x0007215B File Offset: 0x0007035B
		public Name Name
		{
			get
			{
				return this.nameInt;
			}
			set
			{
				this.nameInt = value;
			}
		}

		// Token: 0x1700040E RID: 1038
		// (get) Token: 0x06001420 RID: 5152 RVA: 0x00072164 File Offset: 0x00070364
		public RaceProperties RaceProps
		{
			get
			{
				return this.def.race;
			}
		}

		// Token: 0x1700040F RID: 1039
		// (get) Token: 0x06001421 RID: 5153 RVA: 0x00072171 File Offset: 0x00070371
		public Job CurJob
		{
			get
			{
				if (this.jobs == null)
				{
					return null;
				}
				return this.jobs.curJob;
			}
		}

		// Token: 0x17000410 RID: 1040
		// (get) Token: 0x06001422 RID: 5154 RVA: 0x00072188 File Offset: 0x00070388
		public JobDef CurJobDef
		{
			get
			{
				if (this.CurJob == null)
				{
					return null;
				}
				return this.CurJob.def;
			}
		}

		// Token: 0x17000411 RID: 1041
		// (get) Token: 0x06001423 RID: 5155 RVA: 0x0007219F File Offset: 0x0007039F
		public bool Downed
		{
			get
			{
				return this.health.Downed;
			}
		}

		// Token: 0x17000412 RID: 1042
		// (get) Token: 0x06001424 RID: 5156 RVA: 0x000721AC File Offset: 0x000703AC
		public bool Dead
		{
			get
			{
				return this.health.Dead;
			}
		}

		// Token: 0x17000413 RID: 1043
		// (get) Token: 0x06001425 RID: 5157 RVA: 0x000721B9 File Offset: 0x000703B9
		public string KindLabel
		{
			get
			{
				return GenLabel.BestKindLabel(this, false, false, false, -1);
			}
		}

		// Token: 0x17000414 RID: 1044
		// (get) Token: 0x06001426 RID: 5158 RVA: 0x000721C5 File Offset: 0x000703C5
		public bool InMentalState
		{
			get
			{
				return !this.Dead && this.mindState.mentalStateHandler.InMentalState;
			}
		}

		// Token: 0x17000415 RID: 1045
		// (get) Token: 0x06001427 RID: 5159 RVA: 0x000721E1 File Offset: 0x000703E1
		public MentalState MentalState
		{
			get
			{
				if (this.Dead)
				{
					return null;
				}
				return this.mindState.mentalStateHandler.CurState;
			}
		}

		// Token: 0x17000416 RID: 1046
		// (get) Token: 0x06001428 RID: 5160 RVA: 0x000721FD File Offset: 0x000703FD
		public MentalStateDef MentalStateDef
		{
			get
			{
				if (this.Dead)
				{
					return null;
				}
				return this.mindState.mentalStateHandler.CurStateDef;
			}
		}

		// Token: 0x17000417 RID: 1047
		// (get) Token: 0x06001429 RID: 5161 RVA: 0x00072219 File Offset: 0x00070419
		public bool InAggroMentalState
		{
			get
			{
				return !this.Dead && this.mindState.mentalStateHandler.InMentalState && this.mindState.mentalStateHandler.CurStateDef.IsAggro;
			}
		}

		// Token: 0x17000418 RID: 1048
		// (get) Token: 0x0600142A RID: 5162 RVA: 0x0007224E File Offset: 0x0007044E
		public bool Inspired
		{
			get
			{
				return !this.Dead && this.mindState.inspirationHandler.Inspired;
			}
		}

		// Token: 0x17000419 RID: 1049
		// (get) Token: 0x0600142B RID: 5163 RVA: 0x0007226A File Offset: 0x0007046A
		public Inspiration Inspiration
		{
			get
			{
				if (this.Dead)
				{
					return null;
				}
				return this.mindState.inspirationHandler.CurState;
			}
		}

		// Token: 0x1700041A RID: 1050
		// (get) Token: 0x0600142C RID: 5164 RVA: 0x00072286 File Offset: 0x00070486
		public InspirationDef InspirationDef
		{
			get
			{
				if (this.Dead)
				{
					return null;
				}
				return this.mindState.inspirationHandler.CurStateDef;
			}
		}

		// Token: 0x1700041B RID: 1051
		// (get) Token: 0x0600142D RID: 5165 RVA: 0x000722A2 File Offset: 0x000704A2
		public override Vector3 DrawPos
		{
			get
			{
				return this.Drawer.DrawPos;
			}
		}

		// Token: 0x1700041C RID: 1052
		// (get) Token: 0x0600142E RID: 5166 RVA: 0x000722AF File Offset: 0x000704AF
		public VerbTracker VerbTracker
		{
			get
			{
				return this.verbTracker;
			}
		}

		// Token: 0x1700041D RID: 1053
		// (get) Token: 0x0600142F RID: 5167 RVA: 0x000722B7 File Offset: 0x000704B7
		public List<VerbProperties> VerbProperties
		{
			get
			{
				return this.def.Verbs;
			}
		}

		// Token: 0x1700041E RID: 1054
		// (get) Token: 0x06001430 RID: 5168 RVA: 0x000722C4 File Offset: 0x000704C4
		public List<Tool> Tools
		{
			get
			{
				return this.def.tools;
			}
		}

		// Token: 0x1700041F RID: 1055
		// (get) Token: 0x06001431 RID: 5169 RVA: 0x000722D1 File Offset: 0x000704D1
		public bool ShouldAvoidFences
		{
			get
			{
				return this.def.race.FenceBlocked || this.roping.AnyRopeesFenceBlocked;
			}
		}

		// Token: 0x17000420 RID: 1056
		// (get) Token: 0x06001432 RID: 5170 RVA: 0x000722F2 File Offset: 0x000704F2
		public bool IsColonist
		{
			get
			{
				return base.Faction != null && base.Faction.IsPlayer && this.RaceProps.Humanlike && (!this.IsSlave || this.guest.SlaveIsSecure);
			}
		}

		// Token: 0x17000421 RID: 1057
		// (get) Token: 0x06001433 RID: 5171 RVA: 0x0007232D File Offset: 0x0007052D
		public bool IsFreeColonist
		{
			get
			{
				return this.IsColonist && this.HostFaction == null;
			}
		}

		// Token: 0x17000422 RID: 1058
		// (get) Token: 0x06001434 RID: 5172 RVA: 0x00072342 File Offset: 0x00070542
		public bool IsFreeNonSlaveColonist
		{
			get
			{
				return this.IsFreeColonist && !this.IsSlave;
			}
		}

		// Token: 0x17000423 RID: 1059
		// (get) Token: 0x06001435 RID: 5173 RVA: 0x00072357 File Offset: 0x00070557
		public Faction HostFaction
		{
			get
			{
				if (this.guest == null)
				{
					return null;
				}
				return this.guest.HostFaction;
			}
		}

		// Token: 0x17000424 RID: 1060
		// (get) Token: 0x06001436 RID: 5174 RVA: 0x0007236E File Offset: 0x0007056E
		public Faction SlaveFaction
		{
			get
			{
				Pawn_GuestTracker pawn_GuestTracker = this.guest;
				if (pawn_GuestTracker == null)
				{
					return null;
				}
				return pawn_GuestTracker.SlaveFaction;
			}
		}

		// Token: 0x17000425 RID: 1061
		// (get) Token: 0x06001437 RID: 5175 RVA: 0x00072381 File Offset: 0x00070581
		public Ideo Ideo
		{
			get
			{
				if (this.ideo == null)
				{
					return null;
				}
				return this.ideo.Ideo;
			}
		}

		// Token: 0x17000426 RID: 1062
		// (get) Token: 0x06001438 RID: 5176 RVA: 0x00072398 File Offset: 0x00070598
		public bool Drafted
		{
			get
			{
				return this.drafter != null && this.drafter.Drafted;
			}
		}

		// Token: 0x17000427 RID: 1063
		// (get) Token: 0x06001439 RID: 5177 RVA: 0x000723AF File Offset: 0x000705AF
		public bool IsPrisoner
		{
			get
			{
				return this.guest != null && this.guest.IsPrisoner;
			}
		}

		// Token: 0x17000428 RID: 1064
		// (get) Token: 0x0600143A RID: 5178 RVA: 0x000723C6 File Offset: 0x000705C6
		public bool IsPrisonerOfColony
		{
			get
			{
				return this.guest != null && this.guest.IsPrisoner && this.guest.HostFaction.IsPlayer;
			}
		}

		// Token: 0x17000429 RID: 1065
		// (get) Token: 0x0600143B RID: 5179 RVA: 0x000723EF File Offset: 0x000705EF
		public bool IsSlave
		{
			get
			{
				return this.guest != null && this.guest.IsSlave;
			}
		}

		// Token: 0x1700042A RID: 1066
		// (get) Token: 0x0600143C RID: 5180 RVA: 0x00072406 File Offset: 0x00070606
		public bool IsSlaveOfColony
		{
			get
			{
				return this.IsSlave && base.Faction.IsPlayer;
			}
		}

		// Token: 0x1700042B RID: 1067
		// (get) Token: 0x0600143D RID: 5181 RVA: 0x00072420 File Offset: 0x00070620
		public GuestStatus? GuestStatus
		{
			get
			{
				if (this.guest != null && (this.HostFaction != null || this.guest.GuestStatus != RimWorld.GuestStatus.Guest))
				{
					return new GuestStatus?(this.guest.GuestStatus);
				}
				return null;
			}
		}

		// Token: 0x1700042C RID: 1068
		// (get) Token: 0x0600143E RID: 5182 RVA: 0x00072464 File Offset: 0x00070664
		public bool IsColonistPlayerControlled
		{
			get
			{
				return base.Spawned && this.IsColonist && this.MentalStateDef == null && (this.HostFaction == null || this.IsSlave);
			}
		}

		// Token: 0x1700042D RID: 1069
		// (get) Token: 0x0600143F RID: 5183 RVA: 0x00072490 File Offset: 0x00070690
		public IEnumerable<IntVec3> IngredientStackCells
		{
			get
			{
				yield return this.InteractionCell;
				yield break;
			}
		}

		// Token: 0x1700042E RID: 1070
		// (get) Token: 0x06001440 RID: 5184 RVA: 0x000724A0 File Offset: 0x000706A0
		public bool InContainerEnclosed
		{
			get
			{
				return base.ParentHolder.IsEnclosingContainer();
			}
		}

		// Token: 0x1700042F RID: 1071
		// (get) Token: 0x06001441 RID: 5185 RVA: 0x000724AD File Offset: 0x000706AD
		public Corpse Corpse
		{
			get
			{
				return base.ParentHolder as Corpse;
			}
		}

		// Token: 0x17000430 RID: 1072
		// (get) Token: 0x06001442 RID: 5186 RVA: 0x000724BC File Offset: 0x000706BC
		public Pawn CarriedBy
		{
			get
			{
				if (base.ParentHolder == null)
				{
					return null;
				}
				Pawn_CarryTracker pawn_CarryTracker = base.ParentHolder as Pawn_CarryTracker;
				if (pawn_CarryTracker != null)
				{
					return pawn_CarryTracker.pawn;
				}
				return null;
			}
		}

		// Token: 0x17000431 RID: 1073
		// (get) Token: 0x06001443 RID: 5187 RVA: 0x000724EC File Offset: 0x000706EC
		public override string LabelNoCount
		{
			get
			{
				if (this.Name == null)
				{
					return this.KindLabel;
				}
				if (this.story == null || this.story.TitleShortCap.NullOrEmpty())
				{
					return this.Name.ToStringShort;
				}
				return this.Name.ToStringShort + ", " + this.story.TitleShortCap;
			}
		}

		// Token: 0x17000432 RID: 1074
		// (get) Token: 0x06001444 RID: 5188 RVA: 0x0007254E File Offset: 0x0007074E
		public override string LabelShort
		{
			get
			{
				if (this.Name != null)
				{
					return this.Name.ToStringShort;
				}
				return this.LabelNoCount;
			}
		}

		// Token: 0x17000433 RID: 1075
		// (get) Token: 0x06001445 RID: 5189 RVA: 0x0007256C File Offset: 0x0007076C
		public TaggedString LabelNoCountColored
		{
			get
			{
				if (this.Name == null)
				{
					return this.KindLabel;
				}
				if (this.story == null || this.story.TitleShortCap.NullOrEmpty())
				{
					return this.Name.ToStringShort.Colorize(ColoredText.NameColor);
				}
				return this.Name.ToStringShort.Colorize(ColoredText.NameColor) + ", " + this.story.TitleShortCap;
			}
		}

		// Token: 0x17000434 RID: 1076
		// (get) Token: 0x06001446 RID: 5190 RVA: 0x000725F1 File Offset: 0x000707F1
		public TaggedString NameShortColored
		{
			get
			{
				if (this.Name != null)
				{
					return this.Name.ToStringShort.Colorize(ColoredText.NameColor);
				}
				return this.KindLabel;
			}
		}

		// Token: 0x17000435 RID: 1077
		// (get) Token: 0x06001447 RID: 5191 RVA: 0x00072621 File Offset: 0x00070821
		public TaggedString NameFullColored
		{
			get
			{
				if (this.Name != null)
				{
					return this.Name.ToStringFull.Colorize(ColoredText.NameColor);
				}
				return this.KindLabel;
			}
		}

		// Token: 0x17000436 RID: 1078
		// (get) Token: 0x06001448 RID: 5192 RVA: 0x00072651 File Offset: 0x00070851
		public Pawn_DrawTracker Drawer
		{
			get
			{
				if (this.drawer == null)
				{
					this.drawer = new Pawn_DrawTracker(this);
				}
				return this.drawer;
			}
		}

		// Token: 0x17000437 RID: 1079
		// (get) Token: 0x06001449 RID: 5193 RVA: 0x00072670 File Offset: 0x00070870
		public Faction HomeFaction
		{
			get
			{
				if (base.Faction == null || !base.Faction.IsPlayer)
				{
					return base.Faction;
				}
				if (this.SlaveFaction != null)
				{
					return this.SlaveFaction;
				}
				if (this.HasExtraMiniFaction(null))
				{
					return this.GetExtraMiniFaction(null);
				}
				return this.GetExtraHomeFaction(null) ?? base.Faction;
			}
		}

		// Token: 0x17000438 RID: 1080
		// (get) Token: 0x0600144A RID: 5194 RVA: 0x000726CA File Offset: 0x000708CA
		public BillStack BillStack
		{
			get
			{
				return this.health.surgeryBills;
			}
		}

		// Token: 0x17000439 RID: 1081
		// (get) Token: 0x0600144B RID: 5195 RVA: 0x000726D8 File Offset: 0x000708D8
		public override IntVec3 InteractionCell
		{
			get
			{
				Building_Bed building_Bed = this.CurrentBed();
				if (building_Bed != null)
				{
					IntVec3 position = base.Position;
					IntVec3 position2 = base.Position;
					IntVec3 position3 = base.Position;
					IntVec3 position4 = base.Position;
					if (building_Bed.Rotation.IsHorizontal)
					{
						position.z++;
						position2.z--;
						position3.x--;
						position4.x++;
					}
					else
					{
						position.x--;
						position2.x++;
						position3.z++;
						position4.z--;
					}
					if (position.Standable(base.Map))
					{
						if (position.GetThingList(base.Map).Find((Thing x) => x.def.IsBed) == null && position.GetDoor(base.Map) == null)
						{
							return position;
						}
					}
					if (position2.Standable(base.Map))
					{
						if (position2.GetThingList(base.Map).Find((Thing x) => x.def.IsBed) == null && position2.GetDoor(base.Map) == null)
						{
							return position2;
						}
					}
					if (position3.Standable(base.Map))
					{
						if (position3.GetThingList(base.Map).Find((Thing x) => x.def.IsBed) == null && position3.GetDoor(base.Map) == null)
						{
							return position3;
						}
					}
					if (position4.Standable(base.Map))
					{
						if (position4.GetThingList(base.Map).Find((Thing x) => x.def.IsBed) == null && position4.GetDoor(base.Map) == null)
						{
							return position4;
						}
					}
					if (position.Standable(base.Map))
					{
						if (position.GetThingList(base.Map).Find((Thing x) => x.def.IsBed) == null)
						{
							return position;
						}
					}
					if (position2.Standable(base.Map))
					{
						if (position2.GetThingList(base.Map).Find((Thing x) => x.def.IsBed) == null)
						{
							return position2;
						}
					}
					if (position3.Standable(base.Map))
					{
						if (position3.GetThingList(base.Map).Find((Thing x) => x.def.IsBed) == null)
						{
							return position3;
						}
					}
					if (position4.Standable(base.Map))
					{
						if (position4.GetThingList(base.Map).Find((Thing x) => x.def.IsBed) == null)
						{
							return position4;
						}
					}
					if (position.Standable(base.Map))
					{
						return position;
					}
					if (position2.Standable(base.Map))
					{
						return position2;
					}
					if (position3.Standable(base.Map))
					{
						return position3;
					}
					if (position4.Standable(base.Map))
					{
						return position4;
					}
				}
				return base.InteractionCell;
			}
		}

		// Token: 0x1700043A RID: 1082
		// (get) Token: 0x0600144C RID: 5196 RVA: 0x00072A19 File Offset: 0x00070C19
		public TraderKindDef TraderKind
		{
			get
			{
				if (this.trader == null)
				{
					return null;
				}
				return this.trader.traderKind;
			}
		}

		// Token: 0x1700043B RID: 1083
		// (get) Token: 0x0600144D RID: 5197 RVA: 0x00072A30 File Offset: 0x00070C30
		public IEnumerable<Thing> Goods
		{
			get
			{
				return this.trader.Goods;
			}
		}

		// Token: 0x1700043C RID: 1084
		// (get) Token: 0x0600144E RID: 5198 RVA: 0x00072A3D File Offset: 0x00070C3D
		public int RandomPriceFactorSeed
		{
			get
			{
				return this.trader.RandomPriceFactorSeed;
			}
		}

		// Token: 0x1700043D RID: 1085
		// (get) Token: 0x0600144F RID: 5199 RVA: 0x00072A4A File Offset: 0x00070C4A
		public string TraderName
		{
			get
			{
				return this.trader.TraderName;
			}
		}

		// Token: 0x1700043E RID: 1086
		// (get) Token: 0x06001450 RID: 5200 RVA: 0x00072A57 File Offset: 0x00070C57
		public bool CanTradeNow
		{
			get
			{
				return this.trader != null && this.trader.CanTradeNow;
			}
		}

		// Token: 0x1700043F RID: 1087
		// (get) Token: 0x06001451 RID: 5201 RVA: 0x000682C5 File Offset: 0x000664C5
		public float TradePriceImprovementOffsetForPlayer
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17000440 RID: 1088
		// (get) Token: 0x06001452 RID: 5202 RVA: 0x00072A6E File Offset: 0x00070C6E
		public float BodySize
		{
			get
			{
				return this.ageTracker.CurLifeStage.bodySizeFactor * this.RaceProps.baseBodySize;
			}
		}

		// Token: 0x17000441 RID: 1089
		// (get) Token: 0x06001453 RID: 5203 RVA: 0x00072A8C File Offset: 0x00070C8C
		public float HealthScale
		{
			get
			{
				return this.ageTracker.CurLifeStage.healthScaleFactor * this.RaceProps.baseHealthScale;
			}
		}

		// Token: 0x17000442 RID: 1090
		// (get) Token: 0x06001454 RID: 5204 RVA: 0x00072AAA File Offset: 0x00070CAA
		Thing IAttackTarget.Thing
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000443 RID: 1091
		// (get) Token: 0x06001455 RID: 5205 RVA: 0x0001F15E File Offset: 0x0001D35E
		public float TargetPriorityFactor
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x17000444 RID: 1092
		// (get) Token: 0x06001456 RID: 5206 RVA: 0x00072AB0 File Offset: 0x00070CB0
		public LocalTargetInfo TargetCurrentlyAimingAt
		{
			get
			{
				if (!base.Spawned)
				{
					return LocalTargetInfo.Invalid;
				}
				Stance curStance = this.stances.curStance;
				if (curStance is Stance_Warmup || curStance is Stance_Cooldown)
				{
					return ((Stance_Busy)curStance).focusTarg;
				}
				return LocalTargetInfo.Invalid;
			}
		}

		// Token: 0x17000445 RID: 1093
		// (get) Token: 0x06001457 RID: 5207 RVA: 0x00072AAA File Offset: 0x00070CAA
		Thing IAttackTargetSearcher.Thing
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000446 RID: 1094
		// (get) Token: 0x06001458 RID: 5208 RVA: 0x00072AF8 File Offset: 0x00070CF8
		public LocalTargetInfo LastAttackedTarget
		{
			get
			{
				return this.mindState.lastAttackedTarget;
			}
		}

		// Token: 0x17000447 RID: 1095
		// (get) Token: 0x06001459 RID: 5209 RVA: 0x00072B05 File Offset: 0x00070D05
		public int LastAttackTargetTick
		{
			get
			{
				return this.mindState.lastAttackTargetTick;
			}
		}

		// Token: 0x17000448 RID: 1096
		// (get) Token: 0x0600145A RID: 5210 RVA: 0x00072B14 File Offset: 0x00070D14
		public Verb CurrentEffectiveVerb
		{
			get
			{
				Building_Turret building_Turret = this.MannedThing() as Building_Turret;
				if (building_Turret != null)
				{
					return building_Turret.AttackVerb;
				}
				return this.TryGetAttackVerb(null, !this.IsColonist);
			}
		}

		// Token: 0x0600145B RID: 5211 RVA: 0x00072B47 File Offset: 0x00070D47
		string IVerbOwner.UniqueVerbOwnerID()
		{
			return base.GetUniqueLoadID();
		}

		// Token: 0x0600145C RID: 5212 RVA: 0x00072B4F File Offset: 0x00070D4F
		bool IVerbOwner.VerbsStillUsableBy(Pawn p)
		{
			return p == this;
		}

		// Token: 0x17000449 RID: 1097
		// (get) Token: 0x0600145D RID: 5213 RVA: 0x00072AAA File Offset: 0x00070CAA
		Thing IVerbOwner.ConstantCaster
		{
			get
			{
				return this;
			}
		}

		// Token: 0x1700044A RID: 1098
		// (get) Token: 0x0600145E RID: 5214 RVA: 0x00072B55 File Offset: 0x00070D55
		ImplementOwnerTypeDef IVerbOwner.ImplementOwnerTypeDef
		{
			get
			{
				return ImplementOwnerTypeDefOf.Bodypart;
			}
		}

		// Token: 0x0600145F RID: 5215 RVA: 0x00072B5C File Offset: 0x00070D5C
		public int GetRootTile()
		{
			return base.Tile;
		}

		// Token: 0x06001460 RID: 5216 RVA: 0x00002688 File Offset: 0x00000888
		public ThingOwner GetDirectlyHeldThings()
		{
			return null;
		}

		// Token: 0x06001461 RID: 5217 RVA: 0x00072B64 File Offset: 0x00070D64
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
			if (this.inventory != null)
			{
				outChildren.Add(this.inventory);
			}
			if (this.carryTracker != null)
			{
				outChildren.Add(this.carryTracker);
			}
			if (this.equipment != null)
			{
				outChildren.Add(this.equipment);
			}
			if (this.apparel != null)
			{
				outChildren.Add(this.apparel);
			}
		}

		// Token: 0x06001462 RID: 5218 RVA: 0x00072BCD File Offset: 0x00070DCD
		public string GetKindLabelPlural(int count = -1)
		{
			return GenLabel.BestKindLabel(this, false, false, true, count);
		}

		// Token: 0x06001463 RID: 5219 RVA: 0x00072BD9 File Offset: 0x00070DD9
		public static void ResetStaticData()
		{
			Pawn.NotSurgeryReadyTrans = "NotSurgeryReady".Translate();
			Pawn.CannotReachTrans = "CannotReach".Translate();
		}

		// Token: 0x06001464 RID: 5220 RVA: 0x00072C04 File Offset: 0x00070E04
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<PawnKindDef>(ref this.kindDef, "kindDef");
			Scribe_Values.Look<Gender>(ref this.gender, "gender", Gender.Male, false);
			Scribe_Values.Look<int>(ref this.becameWorldPawnTickAbs, "becameWorldPawnTickAbs", -1, false);
			Scribe_Values.Look<bool>(ref this.teleporting, "teleporting", false, false);
			Scribe_Deep.Look<Name>(ref this.nameInt, "name", Array.Empty<object>());
			Scribe_Deep.Look<Pawn_MindState>(ref this.mindState, "mindState", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_JobTracker>(ref this.jobs, "jobs", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_StanceTracker>(ref this.stances, "stances", new object[]
			{
				this
			});
			Scribe_Deep.Look<VerbTracker>(ref this.verbTracker, "verbTracker", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_NativeVerbs>(ref this.natives, "natives", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_MeleeVerbs>(ref this.meleeVerbs, "meleeVerbs", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_RotationTracker>(ref this.rotationTracker, "rotationTracker", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_PathFollower>(ref this.pather, "pather", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_CarryTracker>(ref this.carryTracker, "carryTracker", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_ApparelTracker>(ref this.apparel, "apparel", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_StoryTracker>(ref this.story, "story", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_EquipmentTracker>(ref this.equipment, "equipment", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_DraftController>(ref this.drafter, "drafter", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_AgeTracker>(ref this.ageTracker, "ageTracker", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_HealthTracker>(ref this.health, "healthTracker", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_RecordsTracker>(ref this.records, "records", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_InventoryTracker>(ref this.inventory, "inventory", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_FilthTracker>(ref this.filth, "filth", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_RopeTracker>(ref this.roping, "roping", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_NeedsTracker>(ref this.needs, "needs", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_GuestTracker>(ref this.guest, "guest", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_GuiltTracker>(ref this.guilt, "guilt", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_RoyaltyTracker>(ref this.royalty, "royalty", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_RelationsTracker>(ref this.relations, "social", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_PsychicEntropyTracker>(ref this.psychicEntropy, "psychicEntropy", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_Ownership>(ref this.ownership, "ownership", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_InteractionsTracker>(ref this.interactions, "interactions", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_SkillTracker>(ref this.skills, "skills", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_AbilityTracker>(ref this.abilities, "abilities", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_IdeoTracker>(ref this.ideo, "ideo", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_WorkSettings>(ref this.workSettings, "workSettings", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_TraderTracker>(ref this.trader, "trader", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_OutfitTracker>(ref this.outfits, "outfits", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_DrugPolicyTracker>(ref this.drugs, "drugs", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_FoodRestrictionTracker>(ref this.foodRestriction, "foodRestriction", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_TimetableTracker>(ref this.timetable, "timetable", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_PlayerSettings>(ref this.playerSettings, "playerSettings", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_TrainingTracker>(ref this.training, "training", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_StyleTracker>(ref this.style, "style", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_StyleObserverTracker>(ref this.styleObserver, "styleObserver", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_ConnectionsTracker>(ref this.connections, "connections", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_InventoryStockTracker>(ref this.inventoryStock, "inventoryStock", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_SurroundingsTracker>(ref this.surroundings, "treeSightings", new object[]
			{
				this
			});
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x06001465 RID: 5221 RVA: 0x000730D8 File Offset: 0x000712D8
		public override string ToString()
		{
			if (this.story != null)
			{
				return this.LabelShort;
			}
			if (this.thingIDNumber > 0)
			{
				return base.ThingID;
			}
			if (this.kindDef != null)
			{
				return this.KindLabel + "_" + base.ThingID;
			}
			if (this.def != null)
			{
				return base.ThingID;
			}
			return base.GetType().ToString();
		}

		// Token: 0x06001466 RID: 5222 RVA: 0x00073140 File Offset: 0x00071340
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			if (this.Dead)
			{
				Log.Warning("Tried to spawn Dead Pawn " + this.ToStringSafe<Pawn>() + ". Replacing with corpse.");
				Corpse corpse = (Corpse)ThingMaker.MakeThing(this.RaceProps.corpseDef, null);
				corpse.InnerPawn = this;
				GenSpawn.Spawn(corpse, base.Position, map, WipeMode.Vanish);
				return;
			}
			if (this.def == null || this.kindDef == null)
			{
				Log.Warning("Tried to spawn pawn without def " + this.ToStringSafe<Pawn>() + ".");
				return;
			}
			base.SpawnSetup(map, respawningAfterLoad);
			if (Find.WorldPawns.Contains(this))
			{
				Find.WorldPawns.RemovePawn(this);
			}
			PawnComponentsUtility.AddComponentsForSpawn(this);
			if (!PawnUtility.InValidState(this))
			{
				Log.Error("Pawn " + this.ToStringSafe<Pawn>() + " spawned in invalid state. Destroying...");
				try
				{
					this.DeSpawn(DestroyMode.Vanish);
				}
				catch (Exception ex)
				{
					Log.Error(string.Concat(new object[]
					{
						"Tried to despawn ",
						this.ToStringSafe<Pawn>(),
						" because of the previous error but couldn't: ",
						ex
					}));
				}
				Find.WorldPawns.PassToWorld(this, PawnDiscardDecideMode.Discard);
				return;
			}
			this.Drawer.Notify_Spawned();
			this.rotationTracker.Notify_Spawned();
			if (!respawningAfterLoad)
			{
				this.pather.ResetToCurrentPosition();
			}
			base.Map.mapPawns.RegisterPawn(this);
			base.Map.autoSlaughterManager.Notify_PawnSpawned();
			if (this.RaceProps.IsFlesh)
			{
				this.relations.everSeenByPlayer = true;
			}
			AddictionUtility.CheckDrugAddictionTeachOpportunity(this);
			if (this.needs != null && this.needs.mood != null && this.needs.mood.recentMemory != null)
			{
				this.needs.mood.recentMemory.Notify_Spawned(respawningAfterLoad);
			}
			if (this.equipment != null)
			{
				this.equipment.Notify_PawnSpawned();
			}
			if (base.Faction == Faction.OfPlayer)
			{
				Ideo ideo = this.Ideo;
				if (ideo != null)
				{
					ideo.RecacheColonistBelieverCount();
				}
			}
			if (!respawningAfterLoad)
			{
				Find.GameEnder.CheckOrUpdateGameOver();
				if (base.Faction == Faction.OfPlayer)
				{
					Find.StoryWatcher.statsRecord.UpdateGreatestPopulation();
					Find.World.StoryState.RecordPopulationIncrease();
				}
				PawnDiedOrDownedThoughtsUtility.RemoveDiedThoughts(this);
				if (this.IsQuestLodger())
				{
					for (int i = this.health.hediffSet.hediffs.Count - 1; i >= 0; i--)
					{
						if (this.health.hediffSet.hediffs[i].def.removeOnQuestLodgers)
						{
							this.health.RemoveHediff(this.health.hediffSet.hediffs[i]);
						}
					}
				}
			}
		}

		// Token: 0x06001467 RID: 5223 RVA: 0x000733E8 File Offset: 0x000715E8
		public override void PostMapInit()
		{
			base.PostMapInit();
			this.pather.TryResumePathingAfterLoading();
		}

		// Token: 0x06001468 RID: 5224 RVA: 0x000733FB File Offset: 0x000715FB
		public override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			this.Drawer.DrawAt(drawLoc);
		}

		// Token: 0x06001469 RID: 5225 RVA: 0x00073409 File Offset: 0x00071609
		public override void DrawGUIOverlay()
		{
			this.Drawer.ui.DrawPawnGUIOverlay();
		}

		// Token: 0x0600146A RID: 5226 RVA: 0x0007341B File Offset: 0x0007161B
		public override void DrawExtraSelectionOverlays()
		{
			base.DrawExtraSelectionOverlays();
			if (this.IsColonistPlayerControlled)
			{
				if (this.pather.curPath != null)
				{
					this.pather.curPath.DrawPath(this);
				}
				this.jobs.DrawLinesBetweenTargets();
			}
		}

		// Token: 0x0600146B RID: 5227 RVA: 0x00073454 File Offset: 0x00071654
		public override void TickRare()
		{
			base.TickRare();
			if (!base.Suspended)
			{
				if (this.apparel != null)
				{
					this.apparel.ApparelTrackerTickRare();
				}
				this.inventory.InventoryTrackerTickRare();
			}
			if (this.training != null)
			{
				this.training.TrainingTrackerTickRare();
			}
			if (base.Spawned && this.RaceProps.IsFlesh)
			{
				GenTemperature.PushHeat(this, 0.3f * this.BodySize * 4.1666665f * (this.def.race.Humanlike ? 1f : 0.6f));
			}
		}

		// Token: 0x0600146C RID: 5228 RVA: 0x000734EC File Offset: 0x000716EC
		public override void Tick()
		{
			if (DebugSettings.noAnimals && base.Spawned && this.RaceProps.Animal)
			{
				this.Destroy(DestroyMode.Vanish);
				return;
			}
			base.Tick();
			if (Find.TickManager.TicksGame % 250 == 0)
			{
				this.TickRare();
			}
			bool suspended = base.Suspended;
			if (!suspended)
			{
				if (base.Spawned)
				{
					this.pather.PatherTick();
				}
				if (base.Spawned)
				{
					this.stances.StanceTrackerTick();
					this.verbTracker.VerbsTick();
				}
				if (base.Spawned)
				{
					this.roping.RopingTick();
					this.natives.NativeVerbsTick();
				}
				if (base.Spawned)
				{
					this.jobs.JobTrackerTick();
				}
				if (base.Spawned)
				{
					this.Drawer.DrawTrackerTick();
					this.rotationTracker.RotationTrackerTick();
				}
				this.health.HealthTick();
				if (!this.Dead)
				{
					this.mindState.MindStateTick();
					this.carryTracker.CarryHandsTick();
				}
			}
			if (!this.Dead)
			{
				this.needs.NeedsTrackerTick();
			}
			if (!suspended)
			{
				if (this.equipment != null)
				{
					this.equipment.EquipmentTrackerTick();
				}
				if (this.apparel != null)
				{
					this.apparel.ApparelTrackerTick();
				}
				if (this.interactions != null && base.Spawned)
				{
					this.interactions.InteractionsTrackerTick();
				}
				if (this.caller != null)
				{
					this.caller.CallTrackerTick();
				}
				if (this.skills != null)
				{
					this.skills.SkillsTick();
				}
				if (this.abilities != null)
				{
					this.abilities.AbilitiesTick();
				}
				if (this.inventory != null)
				{
					this.inventory.InventoryTrackerTick();
				}
				if (this.drafter != null)
				{
					this.drafter.DraftControllerTick();
				}
				if (this.relations != null)
				{
					this.relations.RelationsTrackerTick();
				}
				if (ModsConfig.RoyaltyActive && this.psychicEntropy != null)
				{
					this.psychicEntropy.PsychicEntropyTrackerTick();
				}
				if (this.RaceProps.Humanlike)
				{
					this.guest.GuestTrackerTick();
				}
				if (this.ideo != null)
				{
					this.ideo.IdeoTrackerTick();
				}
				if (this.royalty != null && ModsConfig.RoyaltyActive)
				{
					this.royalty.RoyaltyTrackerTick();
				}
				if (this.style != null && ModsConfig.IdeologyActive)
				{
					this.style.StyleTrackerTick();
				}
				if (this.styleObserver != null && ModsConfig.IdeologyActive)
				{
					this.styleObserver.StyleObserverTick();
				}
				if (this.surroundings != null && ModsConfig.IdeologyActive)
				{
					this.surroundings.SurroundingsTrackerTick();
				}
				this.ageTracker.AgeTick();
				this.records.RecordsTick();
			}
			Pawn_GuiltTracker pawn_GuiltTracker = this.guilt;
			if (pawn_GuiltTracker == null)
			{
				return;
			}
			pawn_GuiltTracker.GuiltTrackerTick();
		}

		// Token: 0x0600146D RID: 5229 RVA: 0x0007378E File Offset: 0x0007198E
		public void TickMothballed(int interval)
		{
			if (!base.Suspended)
			{
				this.ageTracker.AgeTickMothballed(interval);
				this.records.RecordsTickMothballed(interval);
			}
		}

		// Token: 0x0600146E RID: 5230 RVA: 0x000737B0 File Offset: 0x000719B0
		public void Notify_Teleported(bool endCurrentJob = true, bool resetTweenedPos = true)
		{
			if (resetTweenedPos)
			{
				this.Drawer.tweener.ResetTweenedPosToRoot();
			}
			this.pather.Notify_Teleported_Int();
			if (endCurrentJob && this.jobs != null && this.jobs.curJob != null)
			{
				this.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
			}
		}

		// Token: 0x0600146F RID: 5231 RVA: 0x00073804 File Offset: 0x00071A04
		public void Notify_PassedToWorld()
		{
			if (((base.Faction == null && this.RaceProps.Humanlike) || (base.Faction != null && base.Faction.IsPlayer) || base.Faction == Faction.OfAncients || base.Faction == Faction.OfAncientsHostile) && !this.Dead && Find.WorldPawns.GetSituation(this) == WorldPawnSituation.Free)
			{
				bool tryMedievalOrBetter = base.Faction != null && base.Faction.def.techLevel >= TechLevel.Medieval;
				Faction faction;
				if (this.HasExtraHomeFaction(null) && !this.GetExtraHomeFaction(null).IsPlayer)
				{
					if (base.Faction != this.GetExtraHomeFaction(null))
					{
						this.SetFaction(this.GetExtraHomeFaction(null), null);
					}
				}
				else if (Find.FactionManager.TryGetRandomNonColonyHumanlikeFaction(out faction, tryMedievalOrBetter, false, TechLevel.Undefined, false))
				{
					if (base.Faction != faction)
					{
						this.SetFaction(faction, null);
					}
				}
				else if (Find.FactionManager.TryGetRandomNonColonyHumanlikeFaction(out faction, tryMedievalOrBetter, true, TechLevel.Undefined, false))
				{
					if (base.Faction != faction)
					{
						this.SetFaction(faction, null);
					}
				}
				else if (base.Faction != null)
				{
					this.SetFaction(null, null);
				}
			}
			this.becameWorldPawnTickAbs = GenTicks.TicksAbs;
			if (!this.IsCaravanMember() && !PawnUtility.IsTravelingInTransportPodWorldObject(this))
			{
				this.ClearMind(false, false, true);
			}
			if (this.relations != null)
			{
				this.relations.Notify_PassedToWorld();
			}
		}

		// Token: 0x06001470 RID: 5232 RVA: 0x00073960 File Offset: 0x00071B60
		public void Notify_AddBedThoughts()
		{
			foreach (ThingComp thingComp in base.AllComps)
			{
				thingComp.Notify_AddBedThoughts(this);
			}
			Ideo ideo = this.Ideo;
			if (ideo == null)
			{
				return;
			}
			ideo.Notify_AddBedThoughts(this);
		}

		// Token: 0x06001471 RID: 5233 RVA: 0x000739C4 File Offset: 0x00071BC4
		public override void PreApplyDamage(ref DamageInfo dinfo, out bool absorbed)
		{
			base.PreApplyDamage(ref dinfo, out absorbed);
			if (absorbed)
			{
				return;
			}
			this.health.PreApplyDamage(dinfo, out absorbed);
		}

		// Token: 0x06001472 RID: 5234 RVA: 0x000739E8 File Offset: 0x00071BE8
		public override void PostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
			base.PostApplyDamage(dinfo, totalDamageDealt);
			if (dinfo.Def.ExternalViolenceFor(this))
			{
				this.records.AddTo(RecordDefOf.DamageTaken, totalDamageDealt);
			}
			if (dinfo.Def.makesBlood && !dinfo.InstantPermanentInjury && totalDamageDealt > 0f && Rand.Chance(0.5f))
			{
				this.health.DropBloodFilth();
			}
			this.health.PostApplyDamage(dinfo, totalDamageDealt);
			if (!this.Dead)
			{
				this.mindState.Notify_DamageTaken(dinfo);
			}
		}

		// Token: 0x06001473 RID: 5235 RVA: 0x00073A74 File Offset: 0x00071C74
		public override Thing SplitOff(int count)
		{
			if (count <= 0 || count >= this.stackCount)
			{
				return base.SplitOff(count);
			}
			throw new NotImplementedException("Split off on Pawns is not supported (unless we're taking a full stack).");
		}

		// Token: 0x1700044B RID: 1099
		// (get) Token: 0x06001474 RID: 5236 RVA: 0x00073A95 File Offset: 0x00071C95
		public int TicksPerMoveCardinal
		{
			get
			{
				return this.TicksPerMove(false);
			}
		}

		// Token: 0x1700044C RID: 1100
		// (get) Token: 0x06001475 RID: 5237 RVA: 0x00073A9E File Offset: 0x00071C9E
		public int TicksPerMoveDiagonal
		{
			get
			{
				return this.TicksPerMove(true);
			}
		}

		// Token: 0x06001476 RID: 5238 RVA: 0x00073AA8 File Offset: 0x00071CA8
		private int TicksPerMove(bool diagonal)
		{
			float num = this.GetStatValue(StatDefOf.MoveSpeed, true);
			if (RestraintsUtility.InRestraints(this))
			{
				num *= 0.35f;
			}
			if (this.carryTracker != null && this.carryTracker.CarriedThing != null && this.carryTracker.CarriedThing.def.category == ThingCategory.Pawn)
			{
				num *= 0.6f;
			}
			float num2 = num / 60f;
			float num3;
			if (num2 == 0f)
			{
				num3 = 450f;
			}
			else
			{
				num3 = 1f / num2;
				if (base.Spawned && !base.Map.roofGrid.Roofed(base.Position))
				{
					num3 /= base.Map.weatherManager.CurMoveSpeedMultiplier;
				}
				if (diagonal)
				{
					num3 *= 1.41421f;
				}
			}
			return Mathf.Clamp(Mathf.RoundToInt(num3), 1, 450);
		}

		// Token: 0x06001477 RID: 5239 RVA: 0x00073B78 File Offset: 0x00071D78
		public override void Kill(DamageInfo? dinfo, Hediff exactCulprit = null)
		{
			IntVec3 positionHeld = base.PositionHeld;
			Map map = base.Map;
			Map mapHeld = base.MapHeld;
			bool flag = base.Spawned;
			bool spawnedOrAnyParentSpawned = base.SpawnedOrAnyParentSpawned;
			bool flag2 = this.IsWorldPawn();
			Pawn_GuiltTracker pawn_GuiltTracker = this.guilt;
			bool? flag3 = (pawn_GuiltTracker != null) ? new bool?(pawn_GuiltTracker.IsGuilty) : null;
			Caravan caravan = this.GetCaravan();
			Building_Grave assignedGrave = null;
			if (this.ownership != null)
			{
				assignedGrave = this.ownership.AssignedGrave;
			}
			bool flag4 = this.InBed();
			float bedRotation = 0f;
			if (flag4)
			{
				bedRotation = this.CurrentBed().Rotation.AsAngle;
			}
			ThingOwner thingOwner = null;
			bool inContainerEnclosed = this.InContainerEnclosed;
			if (inContainerEnclosed)
			{
				thingOwner = this.holdingOwner;
				thingOwner.Remove(this);
			}
			bool flag5 = false;
			bool flag6 = false;
			bool flag7 = false;
			if (Current.ProgramState == ProgramState.Playing && map != null)
			{
				flag5 = (map.designationManager.DesignationOn(this, DesignationDefOf.Hunt) != null);
				flag6 = this.ShouldBeSlaughtered();
				using (List<Lord>.Enumerator enumerator = map.lordManager.lords.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						LordJob_Ritual lordJob_Ritual;
						if ((lordJob_Ritual = (enumerator.Current.LordJob as LordJob_Ritual)) != null && lordJob_Ritual.pawnsDeathIgnored.Contains(this))
						{
							flag7 = true;
							break;
						}
					}
				}
			}
			bool flag8 = PawnUtility.ShouldSendNotificationAbout(this) && ((!flag6 && !flag7) || dinfo == null || dinfo.Value.Def != DamageDefOf.ExecutionCut) && !this.forceNoDeathNotification;
			float num = 0f;
			Thing attachment = this.GetAttachment(ThingDefOf.Fire);
			if (attachment != null)
			{
				num = ((Fire)attachment).CurrentSize();
			}
			if (Current.ProgramState == ProgramState.Playing)
			{
				Find.Storyteller.Notify_PawnEvent(this, AdaptationEvent.Died, null);
			}
			if (this.IsColonist)
			{
				Find.StoryWatcher.statsRecord.Notify_ColonistKilled();
			}
			if (flag && dinfo != null && dinfo.Value.Def.ExternalViolenceFor(this))
			{
				LifeStageUtility.PlayNearestLifestageSound(this, (LifeStageAge ls) => ls.soundDeath, 1f);
			}
			if (dinfo != null && dinfo.Value.Instigator != null)
			{
				Pawn pawn = dinfo.Value.Instigator as Pawn;
				if (pawn != null)
				{
					RecordsUtility.Notify_PawnKilled(this, pawn);
					if (pawn.equipment != null)
					{
						pawn.equipment.Notify_KilledPawn();
					}
					if (HistoryEventUtility.IsKillingInnocentAnimal(pawn, this))
					{
						Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.KilledInnocentAnimal, pawn.Named(HistoryEventArgsNames.Doer), this.Named(HistoryEventArgsNames.Victim)), true);
					}
				}
			}
			TaleUtility.Notify_PawnDied(this, dinfo);
			if (flag)
			{
				Find.BattleLog.Add(new BattleLogEntry_StateTransition(this, this.RaceProps.DeathActionWorker.DeathRules, (dinfo != null) ? (dinfo.Value.Instigator as Pawn) : null, exactCulprit, (dinfo != null) ? dinfo.Value.HitPart : null));
			}
			this.health.surgeryBills.Clear();
			if (this.apparel != null)
			{
				this.apparel.Notify_PawnKilled(dinfo);
			}
			if (this.RaceProps.IsFlesh)
			{
				this.relations.Notify_PawnKilled(dinfo, map);
			}
			if (this.connections != null)
			{
				this.connections.Notify_PawnKilled();
			}
			this.meleeVerbs.Notify_PawnKilled();
			for (int i = 0; i < this.health.hediffSet.hediffs.Count; i++)
			{
				this.health.hediffSet.hediffs[i].Notify_PawnKilled();
			}
			Pawn_CarryTracker pawn_CarryTracker = base.ParentHolder as Pawn_CarryTracker;
			Thing thing;
			if (pawn_CarryTracker != null && this.holdingOwner.TryDrop(this, pawn_CarryTracker.pawn.Position, pawn_CarryTracker.pawn.Map, ThingPlaceMode.Near, out thing, null, null, true))
			{
				map = pawn_CarryTracker.pawn.Map;
				flag = true;
			}
			PawnDiedOrDownedThoughtsUtility.RemoveLostThoughts(this);
			PawnDiedOrDownedThoughtsUtility.RemoveResuedRelativeThought(this);
			PawnDiedOrDownedThoughtsUtility.TryGiveThoughts(this, dinfo, PawnDiedOrDownedThoughtsKind.Died);
			if (this.RaceProps.Animal)
			{
				PawnDiedOrDownedThoughtsUtility.GiveVeneratedAnimalDiedThoughts(this, map);
			}
			this.health.SetDead();
			if (this.health.deflectionEffecter != null)
			{
				this.health.deflectionEffecter.Cleanup();
				this.health.deflectionEffecter = null;
			}
			if (this.health.woundedEffecter != null)
			{
				this.health.woundedEffecter.Cleanup();
				this.health.woundedEffecter = null;
			}
			if (caravan != null)
			{
				caravan.Notify_MemberDied(this);
			}
			Lord lord = this.GetLord();
			if (lord != null)
			{
				lord.Notify_PawnLost(this, PawnLostCondition.IncappedOrKilled, dinfo);
			}
			if (flag)
			{
				this.DropAndForbidEverything(false);
			}
			if (flag)
			{
				this.DeSpawn(DestroyMode.Vanish);
			}
			if (this.royalty != null)
			{
				this.royalty.Notify_PawnKilled();
			}
			Corpse corpse = null;
			if (!PawnGenerator.IsBeingGenerated(this))
			{
				if (inContainerEnclosed)
				{
					corpse = this.MakeCorpse(assignedGrave, flag4, bedRotation);
					if (!thingOwner.TryAdd(corpse, true))
					{
						corpse.Destroy(DestroyMode.Vanish);
						corpse = null;
					}
				}
				else if (spawnedOrAnyParentSpawned)
				{
					if (this.holdingOwner != null)
					{
						this.holdingOwner.Remove(this);
					}
					corpse = this.MakeCorpse(assignedGrave, flag4, bedRotation);
					if (GenPlace.TryPlaceThing(corpse, positionHeld, mapHeld, ThingPlaceMode.Direct, null, null, default(Rot4)))
					{
						corpse.Rotation = base.Rotation;
						if (HuntJobUtility.WasKilledByHunter(this, dinfo))
						{
							((Pawn)dinfo.Value.Instigator).Reserve(corpse, ((Pawn)dinfo.Value.Instigator).CurJob, 1, -1, null, true);
						}
						else if (!flag5 && !flag6)
						{
							corpse.SetForbiddenIfOutsideHomeArea();
						}
						if (num > 0f)
						{
							FireUtility.TryStartFireIn(corpse.Position, corpse.Map, num);
						}
					}
					else
					{
						corpse.Destroy(DestroyMode.Vanish);
						corpse = null;
					}
				}
				else if (caravan != null && caravan.Spawned)
				{
					corpse = this.MakeCorpse(assignedGrave, flag4, bedRotation);
					caravan.AddPawnOrItem(corpse, true);
				}
				else if (this.holdingOwner != null || this.IsWorldPawn())
				{
					Corpse.PostCorpseDestroy(this);
				}
				else
				{
					corpse = this.MakeCorpse(assignedGrave, flag4, bedRotation);
				}
			}
			if (corpse != null)
			{
				Hediff firstHediffOfDef = this.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.ToxicBuildup, false);
				Hediff firstHediffOfDef2 = this.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Scaria, false);
				CompRottable comp = corpse.GetComp<CompRottable>();
				if ((firstHediffOfDef != null && Rand.Value < firstHediffOfDef.Severity && comp != null) || (firstHediffOfDef2 != null && Rand.Chance(Find.Storyteller.difficulty.scariaRotChance)))
				{
					comp.RotImmediately();
				}
			}
			if (!base.Destroyed)
			{
				this.Destroy(DestroyMode.KillFinalize);
			}
			PawnComponentsUtility.RemoveComponentsOnKilled(this);
			this.health.hediffSet.DirtyCache();
			PortraitsCache.SetDirty(this);
			GlobalTextureAtlasManager.TryMarkPawnFrameSetDirty(this);
			for (int j = this.health.hediffSet.hediffs.Count - 1; j >= 0; j--)
			{
				this.health.hediffSet.hediffs[j].Notify_PawnDied();
			}
			Faction homeFaction = this.HomeFaction;
			if (homeFaction != null)
			{
				Faction faction = homeFaction;
				DamageInfo? dinfo2 = dinfo;
				bool wasWorldPawn = flag2;
				bool? flag9 = flag3;
				bool flag10 = true;
				faction.Notify_MemberDied(this, dinfo2, wasWorldPawn, flag9.GetValueOrDefault() == flag10 & flag9 != null, mapHeld);
			}
			if (corpse != null)
			{
				if (this.RaceProps.DeathActionWorker != null && flag)
				{
					this.RaceProps.DeathActionWorker.PawnDied(corpse);
				}
				if (Find.Scenario != null)
				{
					Find.Scenario.Notify_PawnDied(corpse);
				}
			}
			if (base.Faction != null && base.Faction.IsPlayer)
			{
				BillUtility.Notify_ColonistUnavailable(this);
			}
			if (spawnedOrAnyParentSpawned)
			{
				GenHostility.Notify_PawnLostForTutor(this, mapHeld);
			}
			if (base.Faction != null && base.Faction.IsPlayer && Current.ProgramState == ProgramState.Playing)
			{
				Find.ColonistBar.MarkColonistsDirty();
			}
			Pawn_PsychicEntropyTracker pawn_PsychicEntropyTracker = this.psychicEntropy;
			if (pawn_PsychicEntropyTracker != null)
			{
				pawn_PsychicEntropyTracker.Notify_PawnDied();
			}
			try
			{
				Ideo ideo = this.Ideo;
				if (ideo != null)
				{
					ideo.Notify_MemberDied(this);
				}
				Ideo ideo2 = this.Ideo;
				if (ideo2 != null)
				{
					ideo2.Notify_MemberLost(this, map);
				}
			}
			catch (Exception arg)
			{
				Log.Error("Error while notifying ideo of pawn death: " + arg);
			}
			if (flag8)
			{
				this.health.NotifyPlayerOfKilled(dinfo, exactCulprit, caravan);
			}
			Find.QuestManager.Notify_PawnKilled(this, dinfo);
			Find.FactionManager.Notify_PawnKilled(this);
			Find.IdeoManager.Notify_PawnKilled(this);
		}

		// Token: 0x06001478 RID: 5240 RVA: 0x000743FC File Offset: 0x000725FC
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			if (mode != DestroyMode.Vanish && mode != DestroyMode.KillFinalize)
			{
				Log.Error(string.Concat(new object[]
				{
					"Destroyed pawn ",
					this,
					" with unsupported mode ",
					mode,
					"."
				}));
			}
			base.Destroy(mode);
			Find.WorldPawns.Notify_PawnDestroyed(this);
			if (this.ownership != null)
			{
				Building_Grave assignedGrave = this.ownership.AssignedGrave;
				this.ownership.UnclaimAll();
				if (mode == DestroyMode.KillFinalize && assignedGrave != null)
				{
					assignedGrave.CompAssignableToPawn.TryAssignPawn(this);
				}
			}
			this.ClearMind(false, true, true);
			Lord lord = this.GetLord();
			if (lord != null)
			{
				PawnLostCondition cond = (mode == DestroyMode.KillFinalize) ? PawnLostCondition.IncappedOrKilled : PawnLostCondition.Vanished;
				lord.Notify_PawnLost(this, cond, null);
			}
			if (Current.ProgramState == ProgramState.Playing)
			{
				Find.GameEnder.CheckOrUpdateGameOver();
				Find.TaleManager.Notify_PawnDestroyed(this);
			}
			foreach (Pawn pawn in from p in PawnsFinder.AllMapsWorldAndTemporary_Alive
			where p.playerSettings != null && p.playerSettings.Master == this
			select p)
			{
				pawn.playerSettings.Master = null;
			}
			if (this.equipment != null)
			{
				this.equipment.Notify_PawnDied();
			}
			if (mode != DestroyMode.KillFinalize)
			{
				if (this.equipment != null)
				{
					this.equipment.DestroyAllEquipment(DestroyMode.Vanish);
				}
				this.inventory.DestroyAll(DestroyMode.Vanish);
				if (this.apparel != null)
				{
					this.apparel.DestroyAll(DestroyMode.Vanish);
				}
			}
			WorldPawns worldPawns = Find.WorldPawns;
			if (!worldPawns.IsBeingDiscarded(this) && !worldPawns.Contains(this))
			{
				worldPawns.PassToWorld(this, PawnDiscardDecideMode.Decide);
			}
			if (base.Faction.IsPlayerSafe())
			{
				Ideo ideo = this.Ideo;
				if (ideo == null)
				{
					return;
				}
				ideo.RecacheColonistBelieverCount();
			}
		}

		// Token: 0x06001479 RID: 5241 RVA: 0x000745B8 File Offset: 0x000727B8
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			Map map = base.Map;
			if (this.jobs != null && this.jobs.curJob != null)
			{
				this.jobs.StopAll(false, true);
			}
			base.DeSpawn(mode);
			if (this.pather != null)
			{
				this.pather.StopDead();
			}
			Pawn_RopeTracker pawn_RopeTracker = this.roping;
			if (pawn_RopeTracker != null)
			{
				pawn_RopeTracker.Notify_DeSpawned();
			}
			if (this.needs != null && this.needs.mood != null)
			{
				this.needs.mood.thoughts.situational.Notify_SituationalThoughtsDirty();
			}
			if (this.meleeVerbs != null)
			{
				this.meleeVerbs.Notify_PawnDespawned();
			}
			this.ClearAllReservations(false);
			if (map != null)
			{
				map.mapPawns.DeRegisterPawn(this);
				map.autoSlaughterManager.Notify_PawnDespawned();
			}
			PawnComponentsUtility.RemoveComponentsOnDespawned(this);
		}

		// Token: 0x0600147A RID: 5242 RVA: 0x00074684 File Offset: 0x00072884
		public override void Discard(bool silentlyRemoveReferences = false)
		{
			if (Find.WorldPawns.Contains(this))
			{
				Log.Warning("Tried to discard a world pawn " + this + ".");
				return;
			}
			base.Discard(silentlyRemoveReferences);
			if (this.relations != null)
			{
				this.relations.ClearAllRelations();
			}
			if (Current.ProgramState == ProgramState.Playing)
			{
				Find.PlayLog.Notify_PawnDiscarded(this, silentlyRemoveReferences);
				Find.BattleLog.Notify_PawnDiscarded(this, silentlyRemoveReferences);
				Find.TaleManager.Notify_PawnDiscarded(this, silentlyRemoveReferences);
				Find.QuestManager.Notify_PawnDiscarded(this);
			}
			foreach (Pawn pawn in PawnsFinder.AllMapsWorldAndTemporary_Alive)
			{
				if (pawn.needs != null && pawn.needs.mood != null)
				{
					pawn.needs.mood.thoughts.memories.Notify_PawnDiscarded(this);
				}
			}
			Corpse.PostCorpseDestroy(this);
		}

		// Token: 0x0600147B RID: 5243 RVA: 0x0007477C File Offset: 0x0007297C
		public Corpse MakeCorpse(Building_Grave assignedGrave, bool inBed, float bedRotation)
		{
			if (this.holdingOwner != null)
			{
				Log.Warning("We can't make corpse because the pawn is in a ThingOwner. Remove him from the container first. This should have been already handled before calling this method. holder=" + base.ParentHolder);
				return null;
			}
			Corpse corpse = (Corpse)ThingMaker.MakeThing(this.RaceProps.corpseDef, null);
			corpse.InnerPawn = this;
			if (assignedGrave != null)
			{
				corpse.InnerPawn.ownership.ClaimGrave(assignedGrave);
			}
			if (inBed)
			{
				corpse.InnerPawn.Drawer.renderer.wiggler.SetToCustomRotation(bedRotation + 180f);
			}
			return corpse;
		}

		// Token: 0x0600147C RID: 5244 RVA: 0x00074800 File Offset: 0x00072A00
		public void ExitMap(bool allowedToJoinOrCreateCaravan, Rot4 exitDir)
		{
			if (this.IsWorldPawn())
			{
				Log.Warning("Called ExitMap() on world pawn " + this);
				return;
			}
			Ideo ideo = this.Ideo;
			if (ideo != null)
			{
				ideo.Notify_MemberLost(this, base.Map);
			}
			if (allowedToJoinOrCreateCaravan && CaravanExitMapUtility.CanExitMapAndJoinOrCreateCaravanNow(this))
			{
				CaravanExitMapUtility.ExitMapAndJoinOrCreateCaravan(this, exitDir);
				return;
			}
			Lord lord = this.GetLord();
			if (lord != null)
			{
				lord.Notify_PawnLost(this, PawnLostCondition.ExitedMap, null);
			}
			if (this.carryTracker != null && this.carryTracker.CarriedThing != null)
			{
				Pawn pawn = this.carryTracker.CarriedThing as Pawn;
				if (pawn != null)
				{
					if (base.Faction != null && base.Faction != pawn.Faction)
					{
						base.Faction.kidnapped.Kidnap(pawn, this);
					}
					else
					{
						this.carryTracker.innerContainer.Remove(pawn);
						pawn.ExitMap(false, exitDir);
					}
				}
				else
				{
					this.carryTracker.CarriedThing.Destroy(DestroyMode.Vanish);
				}
				this.carryTracker.innerContainer.Clear();
			}
			bool flag = !this.IsCaravanMember() && !PawnUtility.IsTravelingInTransportPodWorldObject(this) && (!this.IsPrisoner || base.ParentHolder == null || base.ParentHolder is CompShuttle || (this.guest != null && this.guest.Released));
			if (flag)
			{
				IEnumerable<Thing> innerContainer = this.inventory.innerContainer;
				Pawn_ApparelTracker pawn_ApparelTracker = this.apparel;
				IEnumerable<Thing> first = innerContainer.ConcatIfNotNull((pawn_ApparelTracker != null) ? pawn_ApparelTracker.WornApparel : null);
				Pawn_EquipmentTracker pawn_EquipmentTracker = this.equipment;
				foreach (Thing thing in first.ConcatIfNotNull((pawn_EquipmentTracker != null) ? pawn_EquipmentTracker.AllEquipmentListForReading : null))
				{
					Precept_ThingStyle styleSourcePrecept = thing.GetStyleSourcePrecept();
					if (styleSourcePrecept != null)
					{
						styleSourcePrecept.Notify_ThingLost(thing, false);
					}
				}
			}
			if (base.Faction != null)
			{
				base.Faction.Notify_MemberExitedMap(this, flag);
			}
			if (base.Faction == Faction.OfPlayer && this.IsSlave && this.SlaveFaction != null && this.SlaveFaction != Faction.OfPlayer && this.guest.Released)
			{
				this.SlaveFaction.Notify_MemberExitedMap(this, flag);
			}
			if (this.ownership != null && flag)
			{
				this.ownership.UnclaimAll();
			}
			if (this.guest != null)
			{
				bool isPrisonerOfColony = this.IsPrisonerOfColony;
				if (flag)
				{
					this.guest.SetGuestStatus(null, RimWorld.GuestStatus.Guest);
				}
				this.guest.Released = false;
				if (isPrisonerOfColony)
				{
					this.guest.interactionMode = PrisonerInteractionModeDefOf.NoInteraction;
				}
			}
			if (base.Spawned)
			{
				this.DeSpawn(DestroyMode.Vanish);
			}
			this.inventory.UnloadEverything = false;
			if (flag)
			{
				this.ClearMind(false, false, true);
			}
			if (this.relations != null)
			{
				this.relations.Notify_ExitedMap();
			}
			Find.WorldPawns.PassToWorld(this, PawnDiscardDecideMode.Decide);
			QuestUtility.SendQuestTargetSignals(this.questTags, "LeftMap", this.Named("SUBJECT"));
			Find.FactionManager.Notify_PawnLeftMap(this);
			Find.IdeoManager.Notify_PawnLeftMap(this);
		}

		// Token: 0x0600147D RID: 5245 RVA: 0x00074AFC File Offset: 0x00072CFC
		public override void PreTraded(TradeAction action, Pawn playerNegotiator, ITrader trader)
		{
			base.PreTraded(action, playerNegotiator, trader);
			if (base.SpawnedOrAnyParentSpawned)
			{
				this.DropAndForbidEverything(false);
			}
			if (this.ownership != null)
			{
				this.ownership.UnclaimAll();
			}
			if (action == TradeAction.PlayerSells)
			{
				Faction faction = this.GetExtraHomeFaction(null) ?? this.GetExtraHostFaction(null);
				if (faction != null && faction != Faction.OfPlayer)
				{
					Faction.OfPlayer.TryAffectGoodwillWith(faction, Faction.OfPlayer.GoodwillToMakeHostile(faction), true, true, HistoryEventDefOf.MemberSold, new GlobalTargetInfo?(this));
				}
			}
			if (this.guest != null)
			{
				this.guest.SetGuestStatus(null, RimWorld.GuestStatus.Guest);
			}
			if (action == TradeAction.PlayerBuys)
			{
				if (this.guest != null && this.guest.joinStatus == JoinStatus.JoinAsSlave)
				{
					this.guest.SetGuestStatus(Faction.OfPlayer, RimWorld.GuestStatus.Slave);
				}
				else if (this.needs.mood != null)
				{
					this.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.FreedFromSlavery, null, null);
				}
				this.SetFaction(Faction.OfPlayer, null);
			}
			else if (action == TradeAction.PlayerSells)
			{
				if (this.RaceProps.Humanlike)
				{
					TaleRecorder.RecordTale(TaleDefOf.SoldPrisoner, new object[]
					{
						playerNegotiator,
						this,
						trader
					});
				}
				if (base.Faction != null)
				{
					this.SetFaction(null, null);
				}
				if (this.RaceProps.IsFlesh)
				{
					this.relations.Notify_PawnSold(playerNegotiator);
				}
			}
			this.ClearMind(false, false, true);
		}

		// Token: 0x0600147E RID: 5246 RVA: 0x00074C60 File Offset: 0x00072E60
		public void PreKidnapped(Pawn kidnapper)
		{
			Find.Storyteller.Notify_PawnEvent(this, AdaptationEvent.Kidnapped, null);
			if (this.IsColonist && kidnapper != null)
			{
				TaleRecorder.RecordTale(TaleDefOf.KidnappedColonist, new object[]
				{
					kidnapper,
					this
				});
			}
			if (this.ownership != null)
			{
				this.ownership.UnclaimAll();
			}
			if (this.guest != null)
			{
				this.guest.SetGuestStatus(null, RimWorld.GuestStatus.Guest);
			}
			if (this.RaceProps.IsFlesh)
			{
				this.relations.Notify_PawnKidnapped();
			}
			this.ClearMind(false, false, true);
		}

		// Token: 0x0600147F RID: 5247 RVA: 0x00074CF0 File Offset: 0x00072EF0
		public override void SetFaction(Faction newFaction, Pawn recruiter = null)
		{
			if (newFaction == base.Faction)
			{
				Log.Warning("Used SetFaction to change " + this.ToStringSafe<Pawn>() + " to same faction " + newFaction.ToStringSafe<Faction>());
				return;
			}
			Faction faction = base.Faction;
			if (this.guest != null)
			{
				this.guest.SetGuestStatus(null, RimWorld.GuestStatus.Guest);
			}
			if (base.Spawned)
			{
				base.Map.mapPawns.DeRegisterPawn(this);
				base.Map.pawnDestinationReservationManager.ReleaseAllClaimedBy(this);
				base.Map.designationManager.RemoveAllDesignationsOn(this, false);
				base.Map.autoSlaughterManager.Notify_PawnChangedFaction();
			}
			if ((newFaction == Faction.OfPlayer || base.Faction == Faction.OfPlayer) && Current.ProgramState == ProgramState.Playing)
			{
				Find.ColonistBar.MarkColonistsDirty();
			}
			Lord lord = this.GetLord();
			if (lord != null)
			{
				lord.Notify_PawnLost(this, PawnLostCondition.ChangedFaction, null);
			}
			if (PawnUtility.IsFactionLeader(this))
			{
				Faction factionLeaderFaction = PawnUtility.GetFactionLeaderFaction(this);
				if (newFaction != factionLeaderFaction && !this.HasExtraHomeFaction(factionLeaderFaction) && !this.HasExtraMiniFaction(factionLeaderFaction))
				{
					factionLeaderFaction.Notify_LeaderLost();
				}
			}
			if (newFaction == Faction.OfPlayer && this.RaceProps.Humanlike && !this.IsQuestLodger())
			{
				this.ChangeKind(newFaction.def.basicMemberKind);
			}
			base.SetFaction(newFaction, null);
			PawnComponentsUtility.AddAndRemoveDynamicComponents(this, false);
			if (base.Faction != null && base.Faction.IsPlayer)
			{
				if (this.workSettings != null)
				{
					this.workSettings.EnableAndInitialize();
				}
				Find.StoryWatcher.watcherPopAdaptation.Notify_PawnEvent(this, PopAdaptationEvent.GainedColonist);
			}
			if (this.Drafted)
			{
				this.drafter.Drafted = false;
			}
			ReachabilityUtility.ClearCacheFor(this);
			this.health.surgeryBills.Clear();
			if (base.Spawned)
			{
				base.Map.mapPawns.RegisterPawn(this);
			}
			this.GenerateNecessaryName();
			if (this.playerSettings != null)
			{
				this.playerSettings.ResetMedicalCare();
			}
			this.ClearMind(true, false, true);
			if (!this.Dead && this.needs.mood != null)
			{
				this.needs.mood.thoughts.situational.Notify_SituationalThoughtsDirty();
			}
			if (base.Spawned)
			{
				base.Map.attackTargetsCache.UpdateTarget(this);
			}
			Find.GameEnder.CheckOrUpdateGameOver();
			AddictionUtility.CheckDrugAddictionTeachOpportunity(this);
			if (this.needs != null)
			{
				this.needs.AddOrRemoveNeedsAsAppropriate();
			}
			if (this.playerSettings != null)
			{
				this.playerSettings.Notify_FactionChanged();
			}
			if (this.relations != null)
			{
				this.relations.Notify_ChangedFaction();
			}
			if (this.RaceProps.Animal && newFaction == Faction.OfPlayer)
			{
				this.training.SetWantedRecursive(TrainableDefOf.Tameness, true);
				this.training.Train(TrainableDefOf.Tameness, recruiter, true);
				if (this.RaceProps.Roamer && this.mindState != null)
				{
					this.mindState.lastStartRoamCooldownTick = new int?(Find.TickManager.TicksGame);
				}
			}
			if (faction == Faction.OfPlayer)
			{
				BillUtility.Notify_ColonistUnavailable(this);
			}
			if (newFaction == Faction.OfPlayer)
			{
				Find.StoryWatcher.statsRecord.UpdateGreatestPopulation();
				Find.World.StoryState.RecordPopulationIncrease();
			}
			if (newFaction != null)
			{
				newFaction.Notify_PawnJoined(this);
			}
			if (this.Ideo != null)
			{
				this.Ideo.Notify_MemberChangedFaction(this, faction, newFaction);
			}
			Pawn_AgeTracker pawn_AgeTracker = this.ageTracker;
			if (pawn_AgeTracker == null)
			{
				return;
			}
			pawn_AgeTracker.ResetAgeReversalDemand(Pawn_AgeTracker.AgeReversalReason.Recruited, false);
		}

		// Token: 0x06001480 RID: 5248 RVA: 0x00075038 File Offset: 0x00073238
		public void ClearMind(bool ifLayingKeepLaying = false, bool clearInspiration = false, bool clearMentalState = true)
		{
			if (this.pather != null)
			{
				this.pather.StopDead();
			}
			if (this.mindState != null)
			{
				this.mindState.Reset(clearInspiration, clearMentalState);
			}
			if (this.jobs != null)
			{
				this.jobs.StopAll(ifLayingKeepLaying, true);
			}
			this.VerifyReservations();
		}

		// Token: 0x06001481 RID: 5249 RVA: 0x00075088 File Offset: 0x00073288
		public void ClearAllReservations(bool releaseDestinationsOnlyIfObsolete = true)
		{
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				if (releaseDestinationsOnlyIfObsolete)
				{
					maps[i].pawnDestinationReservationManager.ReleaseAllObsoleteClaimedBy(this);
				}
				else
				{
					maps[i].pawnDestinationReservationManager.ReleaseAllClaimedBy(this);
				}
				maps[i].reservationManager.ReleaseAllClaimedBy(this);
				maps[i].physicalInteractionReservationManager.ReleaseAllClaimedBy(this);
				maps[i].attackTargetReservationManager.ReleaseAllClaimedBy(this);
			}
		}

		// Token: 0x06001482 RID: 5250 RVA: 0x0007510C File Offset: 0x0007330C
		public void ClearReservationsForJob(Job job)
		{
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				maps[i].pawnDestinationReservationManager.ReleaseClaimedBy(this, job);
				maps[i].reservationManager.ReleaseClaimedBy(this, job);
				maps[i].physicalInteractionReservationManager.ReleaseClaimedBy(this, job);
				maps[i].attackTargetReservationManager.ReleaseClaimedBy(this, job);
			}
		}

		// Token: 0x06001483 RID: 5251 RVA: 0x0007517C File Offset: 0x0007337C
		public void VerifyReservations()
		{
			if (this.jobs == null)
			{
				return;
			}
			if (this.CurJob != null || this.jobs.jobQueue.Count > 0 || this.jobs.startingNewJob)
			{
				return;
			}
			bool flag = false;
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				LocalTargetInfo obj = maps[i].reservationManager.FirstReservationFor(this);
				if (obj.IsValid)
				{
					Log.ErrorOnce(string.Format("Reservation manager failed to clean up properly; {0} still reserving {1}", this.ToStringSafe<Pawn>(), obj.ToStringSafe<LocalTargetInfo>()), 97771429 ^ this.thingIDNumber);
					flag = true;
				}
				LocalTargetInfo obj2 = maps[i].physicalInteractionReservationManager.FirstReservationFor(this);
				if (obj2.IsValid)
				{
					Log.ErrorOnce(string.Format("Physical interaction reservation manager failed to clean up properly; {0} still reserving {1}", this.ToStringSafe<Pawn>(), obj2.ToStringSafe<LocalTargetInfo>()), 19586765 ^ this.thingIDNumber);
					flag = true;
				}
				IAttackTarget attackTarget = maps[i].attackTargetReservationManager.FirstReservationFor(this);
				if (attackTarget != null)
				{
					Log.ErrorOnce(string.Format("Attack target reservation manager failed to clean up properly; {0} still reserving {1}", this.ToStringSafe<Pawn>(), attackTarget.ToStringSafe<IAttackTarget>()), 100495878 ^ this.thingIDNumber);
					flag = true;
				}
				IntVec3 obj3 = maps[i].pawnDestinationReservationManager.FirstObsoleteReservationFor(this);
				if (obj3.IsValid)
				{
					Job job = maps[i].pawnDestinationReservationManager.FirstObsoleteReservationJobFor(this);
					Log.ErrorOnce(string.Format("Pawn destination reservation manager failed to clean up properly; {0}/{1}/{2} still reserving {3}", new object[]
					{
						this.ToStringSafe<Pawn>(),
						job.ToStringSafe<Job>(),
						job.def.ToStringSafe<JobDef>(),
						obj3.ToStringSafe<IntVec3>()
					}), 1958674 ^ this.thingIDNumber);
					flag = true;
				}
			}
			if (flag)
			{
				this.ClearAllReservations(true);
			}
		}

		// Token: 0x06001484 RID: 5252 RVA: 0x00075334 File Offset: 0x00073534
		public void DropAndForbidEverything(bool keepInventoryAndEquipmentIfInBed = false)
		{
			if (this.kindDef.destroyGearOnDrop)
			{
				this.equipment.DestroyAllEquipment(DestroyMode.Vanish);
				this.apparel.DestroyAll(DestroyMode.Vanish);
			}
			if (this.InContainerEnclosed)
			{
				if (this.carryTracker != null && this.carryTracker.CarriedThing != null)
				{
					this.carryTracker.innerContainer.TryTransferToContainer(this.carryTracker.CarriedThing, this.holdingOwner, true);
				}
				if (this.equipment != null && this.equipment.Primary != null)
				{
					this.equipment.TryTransferEquipmentToContainer(this.equipment.Primary, this.holdingOwner);
				}
				if (this.inventory != null)
				{
					this.inventory.innerContainer.TryTransferAllToContainer(this.holdingOwner, true);
					return;
				}
			}
			else if (base.SpawnedOrAnyParentSpawned)
			{
				if (this.carryTracker != null && this.carryTracker.CarriedThing != null)
				{
					Thing thing;
					this.carryTracker.TryDropCarriedThing(base.PositionHeld, ThingPlaceMode.Near, out thing, null);
				}
				if (!keepInventoryAndEquipmentIfInBed || !this.InBed())
				{
					if (this.equipment != null)
					{
						this.equipment.DropAllEquipment(base.PositionHeld, true);
					}
					if (this.inventory != null && this.inventory.innerContainer.TotalStackCount > 0)
					{
						this.inventory.DropAllNearPawn(base.PositionHeld, true, false);
					}
				}
			}
		}

		// Token: 0x06001485 RID: 5253 RVA: 0x00075484 File Offset: 0x00073684
		public void GenerateNecessaryName()
		{
			if (base.Faction != Faction.OfPlayer || !this.RaceProps.Animal)
			{
				return;
			}
			if (this.Name == null)
			{
				this.Name = PawnBioAndNameGenerator.GeneratePawnName(this, NameStyle.Numeric, null);
			}
		}

		// Token: 0x06001486 RID: 5254 RVA: 0x000754B8 File Offset: 0x000736B8
		public Verb TryGetAttackVerb(Thing target, bool allowManualCastWeapons = false)
		{
			if (this.equipment != null && this.equipment.Primary != null && this.equipment.PrimaryEq.PrimaryVerb.Available() && (!this.equipment.PrimaryEq.PrimaryVerb.verbProps.onlyManualCast || (this.CurJob != null && this.CurJob.def != JobDefOf.Wait_Combat) || allowManualCastWeapons))
			{
				return this.equipment.PrimaryEq.PrimaryVerb;
			}
			if (allowManualCastWeapons && this.apparel != null)
			{
				Verb firstApparelVerb = this.apparel.FirstApparelVerb;
				if (firstApparelVerb != null && firstApparelVerb.Available())
				{
					return firstApparelVerb;
				}
			}
			return this.meleeVerbs.TryGetMeleeVerb(target);
		}

		// Token: 0x06001487 RID: 5255 RVA: 0x00075578 File Offset: 0x00073778
		public bool TryStartAttack(LocalTargetInfo targ)
		{
			if (this.stances.FullBodyBusy)
			{
				return false;
			}
			if (this.WorkTagIsDisabled(WorkTags.Violent))
			{
				return false;
			}
			bool allowManualCastWeapons = !this.IsColonist;
			Verb verb = this.TryGetAttackVerb(targ.Thing, allowManualCastWeapons);
			return verb != null && verb.TryStartCastOn(targ, false, true, false);
		}

		// Token: 0x06001488 RID: 5256 RVA: 0x000755C7 File Offset: 0x000737C7
		public override IEnumerable<Thing> ButcherProducts(Pawn butcher, float efficiency)
		{
			if (this.RaceProps.meatDef != null)
			{
				int num = GenMath.RoundRandom(this.GetStatValue(StatDefOf.MeatAmount, true) * efficiency);
				if (num > 0)
				{
					Thing thing = ThingMaker.MakeThing(this.RaceProps.meatDef, null);
					thing.stackCount = num;
					yield return thing;
				}
			}
			foreach (Thing thing2 in base.ButcherProducts(butcher, efficiency))
			{
				yield return thing2;
			}
			IEnumerator<Thing> enumerator = null;
			if (this.RaceProps.leatherDef != null)
			{
				int num2 = GenMath.RoundRandom(this.GetStatValue(StatDefOf.LeatherAmount, true) * efficiency);
				if (num2 > 0)
				{
					Thing thing3 = ThingMaker.MakeThing(this.RaceProps.leatherDef, null);
					thing3.stackCount = num2;
					yield return thing3;
				}
			}
			if (!this.RaceProps.Humanlike)
			{
				Pawn.<>c__DisplayClass227_0 CS$<>8__locals1 = new Pawn.<>c__DisplayClass227_0();
				CS$<>8__locals1.lifeStage = this.ageTracker.CurKindLifeStage;
				if (CS$<>8__locals1.lifeStage.butcherBodyPart != null && (this.gender == Gender.None || (this.gender == Gender.Male && CS$<>8__locals1.lifeStage.butcherBodyPart.allowMale) || (this.gender == Gender.Female && CS$<>8__locals1.lifeStage.butcherBodyPart.allowFemale)))
				{
					for (;;)
					{
						IEnumerable<BodyPartRecord> notMissingParts = this.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null);
						Func<BodyPartRecord, bool> predicate;
						if ((predicate = CS$<>8__locals1.<>9__0) == null)
						{
							predicate = (CS$<>8__locals1.<>9__0 = ((BodyPartRecord x) => x.IsInGroup(CS$<>8__locals1.lifeStage.butcherBodyPart.bodyPartGroup)));
						}
						BodyPartRecord bodyPartRecord = notMissingParts.Where(predicate).FirstOrDefault<BodyPartRecord>();
						if (bodyPartRecord == null)
						{
							break;
						}
						this.health.AddHediff(HediffMaker.MakeHediff(HediffDefOf.MissingBodyPart, this, bodyPartRecord), null, null, null);
						Thing thing4;
						if (CS$<>8__locals1.lifeStage.butcherBodyPart.thing != null)
						{
							thing4 = ThingMaker.MakeThing(CS$<>8__locals1.lifeStage.butcherBodyPart.thing, null);
						}
						else
						{
							thing4 = ThingMaker.MakeThing(bodyPartRecord.def.spawnThingOnRemoved, null);
						}
						yield return thing4;
					}
				}
				CS$<>8__locals1 = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x06001489 RID: 5257 RVA: 0x000755E8 File Offset: 0x000737E8
		public string MainDesc(bool writeFaction)
		{
			bool flag = base.Faction == null || !base.Faction.IsPlayer;
			string text = (this.gender == Gender.None) ? "" : this.gender.GetLabel(this.AnimalOrWildMan());
			if (this.RaceProps.Animal || this.RaceProps.IsMechanoid)
			{
				string str = GenLabel.BestKindLabel(this, false, true, false, -1);
				if (this.Name != null)
				{
					text = text + " " + str;
				}
			}
			if (this.ageTracker != null)
			{
				if (text.Length > 0)
				{
					text += ", ";
				}
				text += "AgeIndicator".Translate(this.ageTracker.AgeNumberString);
			}
			if (!this.RaceProps.Animal && !this.RaceProps.IsMechanoid && flag)
			{
				if (text.Length > 0)
				{
					text += ", ";
				}
				text += GenLabel.BestKindLabel(this, false, true, false, -1);
			}
			if (writeFaction)
			{
				Pawn.tmpExtraFactions.Clear();
				QuestUtility.GetExtraFactionsFromQuestParts(this, Pawn.tmpExtraFactions, null);
				GuestUtility.GetExtraFactionsFromGuestStatus(this, Pawn.tmpExtraFactions);
				if (base.Faction != null && !base.Faction.Hidden)
				{
					if (Pawn.tmpExtraFactions.Count == 0)
					{
						text = "PawnMainDescFactionedWrap".Translate(text, base.Faction.NameColored).Resolve();
					}
					else
					{
						text = "PawnMainDescUnderFactionedWrap".Translate(text, base.Faction.NameColored).Resolve();
					}
				}
				for (int i = 0; i < Pawn.tmpExtraFactions.Count; i++)
				{
					if (base.Faction != Pawn.tmpExtraFactions[i].faction)
					{
						text += string.Format("\n{0}: {1}", Pawn.tmpExtraFactions[i].factionType.GetLabel().CapitalizeFirst(), Pawn.tmpExtraFactions[i].faction.NameColored.Resolve());
					}
				}
				Pawn.tmpExtraFactions.Clear();
			}
			return text.CapitalizeFirst();
		}

		// Token: 0x0600148A RID: 5258 RVA: 0x00075820 File Offset: 0x00073A20
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(this.MainDesc(true));
			Pawn_RoyaltyTracker pawn_RoyaltyTracker = this.royalty;
			RoyalTitle royalTitle = (pawn_RoyaltyTracker != null) ? pawn_RoyaltyTracker.MostSeniorTitle : null;
			if (royalTitle != null)
			{
				stringBuilder.AppendLine("PawnTitleDescWrap".Translate(royalTitle.def.GetLabelCapFor(this), royalTitle.faction.NameColored).Resolve());
			}
			string inspectString = base.GetInspectString();
			if (!inspectString.NullOrEmpty())
			{
				stringBuilder.AppendLine(inspectString);
			}
			if (this.TraderKind != null)
			{
				stringBuilder.AppendLine(this.TraderKind.LabelCap);
			}
			if (this.InMentalState)
			{
				stringBuilder.AppendLine(this.MentalState.InspectLine);
			}
			Pawn.states.Clear();
			if (this.stances != null && this.stances.stunner != null && this.stances.stunner.Stunned)
			{
				Pawn.states.AddDistinct("StunLower".Translate());
			}
			if (this.health != null && this.health.hediffSet != null)
			{
				List<Hediff> hediffs = this.health.hediffSet.hediffs;
				for (int i = 0; i < hediffs.Count; i++)
				{
					Hediff hediff = hediffs[i];
					if (!hediff.def.battleStateLabel.NullOrEmpty())
					{
						Pawn.states.AddDistinct(hediff.def.battleStateLabel);
					}
				}
			}
			if (Pawn.states.Count > 0)
			{
				Pawn.states.Sort();
				stringBuilder.AppendLine(string.Format("{0}: {1}", "State".Translate(), Pawn.states.ToCommaList(false, false).CapitalizeFirst()));
				Pawn.states.Clear();
			}
			if (this.Inspired)
			{
				stringBuilder.AppendLine(this.Inspiration.InspectLine);
			}
			if (this.equipment != null && this.equipment.Primary != null)
			{
				stringBuilder.AppendLine("Equipped".TranslateSimple() + ": " + ((this.equipment.Primary != null) ? this.equipment.Primary.Label : "EquippedNothing".TranslateSimple()).CapitalizeFirst());
			}
			if (this.carryTracker != null && this.carryTracker.CarriedThing != null)
			{
				stringBuilder.Append("Carrying".Translate() + ": ");
				stringBuilder.AppendLine(this.carryTracker.CarriedThing.LabelCap);
			}
			Pawn_RopeTracker pawn_RopeTracker = this.roping;
			if (pawn_RopeTracker != null && pawn_RopeTracker.IsRoped)
			{
				stringBuilder.AppendLine(this.roping.InspectLine);
			}
			if ((base.Faction == Faction.OfPlayer || this.HostFaction == Faction.OfPlayer) && !this.InMentalState)
			{
				string text = null;
				Lord lord = this.GetLord();
				if (lord != null && lord.LordJob != null)
				{
					text = lord.LordJob.GetReport(this);
				}
				if (this.jobs.curJob != null)
				{
					try
					{
						string text2 = this.jobs.curDriver.GetReport().CapitalizeFirst();
						if (!text.NullOrEmpty())
						{
							text = text + ": " + text2;
						}
						else
						{
							text = text2;
						}
					}
					catch (Exception arg)
					{
						Log.Error("JobDriver.GetReport() exception: " + arg);
					}
				}
				if (!text.NullOrEmpty())
				{
					stringBuilder.AppendLine(text);
				}
			}
			if (this.jobs.curJob != null && this.jobs.jobQueue.Count > 0)
			{
				try
				{
					string text3 = this.jobs.jobQueue[0].job.GetReport(this).CapitalizeFirst();
					if (this.jobs.jobQueue.Count > 1)
					{
						text3 = string.Concat(new object[]
						{
							text3,
							" (+",
							this.jobs.jobQueue.Count - 1,
							")"
						});
					}
					stringBuilder.AppendLine("Queued".Translate() + ": " + text3);
				}
				catch (Exception arg2)
				{
					Log.Error("JobDriver.GetReport() exception: " + arg2);
				}
			}
			if (RestraintsUtility.ShouldShowRestraintsInfo(this))
			{
				stringBuilder.AppendLine("InRestraints".Translate());
			}
			if (Prefs.DevMode && DebugSettings.showLocomotionUrgency && this.CurJob != null)
			{
				stringBuilder.AppendLine("Locomotion Urgency: " + this.CurJob.locomotionUrgency.ToString());
			}
			return stringBuilder.ToString().TrimEndNewlines();
		}

		// Token: 0x0600148B RID: 5259 RVA: 0x00075CEC File Offset: 0x00073EEC
		public override IEnumerable<Gizmo> GetGizmos()
		{
			Lord lord2 = this.GetLord();
			IEnumerator<Gizmo> enumerator;
			if (this.IsColonistPlayerControlled && (lord2 == null || !(lord2.LordJob is LordJob_Ritual)))
			{
				foreach (Gizmo gizmo in base.GetGizmos())
				{
					yield return gizmo;
				}
				enumerator = null;
				if (this.drafter != null)
				{
					foreach (Gizmo gizmo2 in this.drafter.GetGizmos())
					{
						yield return gizmo2;
					}
					enumerator = null;
				}
				foreach (Gizmo gizmo3 in PawnAttackGizmoUtility.GetAttackGizmos(this))
				{
					yield return gizmo3;
				}
				enumerator = null;
			}
			if (this.equipment != null)
			{
				foreach (Gizmo gizmo4 in this.equipment.GetGizmos())
				{
					yield return gizmo4;
				}
				enumerator = null;
			}
			if (this.carryTracker != null)
			{
				foreach (Gizmo gizmo5 in this.carryTracker.GetGizmos())
				{
					yield return gizmo5;
				}
				enumerator = null;
			}
			if (Find.Selector.SingleSelectedThing == this && this.psychicEntropy != null && this.psychicEntropy.NeedToShowGizmo())
			{
				yield return this.psychicEntropy.GetGizmo();
			}
			if (this.IsColonistPlayerControlled)
			{
				if (this.abilities != null)
				{
					foreach (Gizmo gizmo6 in this.abilities.GetGizmos())
					{
						yield return gizmo6;
					}
					enumerator = null;
				}
				if (this.playerSettings != null)
				{
					foreach (Gizmo gizmo7 in this.playerSettings.GetGizmos())
					{
						yield return gizmo7;
					}
					enumerator = null;
				}
				foreach (Gizmo gizmo8 in this.health.GetGizmos())
				{
					yield return gizmo8;
				}
				enumerator = null;
			}
			if (this.apparel != null)
			{
				foreach (Gizmo gizmo9 in this.apparel.GetGizmos())
				{
					yield return gizmo9;
				}
				enumerator = null;
			}
			if (this.inventory != null)
			{
				foreach (Gizmo gizmo10 in this.inventory.GetGizmos())
				{
					yield return gizmo10;
				}
				enumerator = null;
			}
			foreach (Gizmo gizmo11 in this.mindState.GetGizmos())
			{
				yield return gizmo11;
			}
			enumerator = null;
			if (this.royalty != null && this.IsColonistPlayerControlled)
			{
				bool anyPermitOnCooldown = false;
				foreach (FactionPermit factionPermit in this.royalty.AllFactionPermits)
				{
					if (factionPermit.OnCooldown)
					{
						anyPermitOnCooldown = true;
					}
					IEnumerable<Gizmo> pawnGizmos = factionPermit.Permit.Worker.GetPawnGizmos(this, factionPermit.Faction);
					if (pawnGizmos != null)
					{
						foreach (Gizmo gizmo12 in pawnGizmos)
						{
							yield return gizmo12;
						}
						enumerator = null;
					}
				}
				List<FactionPermit>.Enumerator enumerator2 = default(List<FactionPermit>.Enumerator);
				if (this.royalty.HasAidPermit)
				{
					yield return this.royalty.RoyalAidGizmo();
				}
				if (Prefs.DevMode && anyPermitOnCooldown)
				{
					yield return new Command_Action
					{
						defaultLabel = "Reset permit cooldowns",
						action = delegate()
						{
							foreach (FactionPermit factionPermit2 in this.royalty.AllFactionPermits)
							{
								factionPermit2.ResetCooldown();
							}
						}
					};
				}
				foreach (RoyalTitle royalTitle in this.royalty.AllTitlesForReading)
				{
					if (royalTitle.def.permits != null)
					{
						Faction faction = royalTitle.faction;
						foreach (RoyalTitlePermitDef royalTitlePermitDef in royalTitle.def.permits)
						{
							IEnumerable<Gizmo> pawnGizmos2 = royalTitlePermitDef.Worker.GetPawnGizmos(this, faction);
							if (pawnGizmos2 != null)
							{
								foreach (Gizmo gizmo13 in pawnGizmos2)
								{
									yield return gizmo13;
								}
								enumerator = null;
							}
						}
						List<RoyalTitlePermitDef>.Enumerator enumerator4 = default(List<RoyalTitlePermitDef>.Enumerator);
						faction = null;
					}
				}
				List<RoyalTitle>.Enumerator enumerator3 = default(List<RoyalTitle>.Enumerator);
			}
			foreach (Gizmo gizmo14 in QuestUtility.GetQuestRelatedGizmos(this))
			{
				yield return gizmo14;
			}
			enumerator = null;
			if (this.royalty != null && ModsConfig.RoyaltyActive)
			{
				foreach (Gizmo gizmo15 in this.royalty.GetGizmos())
				{
					yield return gizmo15;
				}
				enumerator = null;
			}
			if (this.connections != null && ModsConfig.IdeologyActive)
			{
				foreach (Gizmo gizmo16 in this.connections.GetGizmos())
				{
					yield return gizmo16;
				}
				enumerator = null;
			}
			Lord lord = this.GetLord();
			if (lord != null && lord.LordJob != null)
			{
				foreach (Gizmo gizmo17 in lord.LordJob.GetPawnGizmos(this))
				{
					yield return gizmo17;
				}
				enumerator = null;
				if (lord.CurLordToil != null)
				{
					foreach (Gizmo gizmo18 in lord.CurLordToil.GetPawnGizmos(this))
					{
						yield return gizmo18;
					}
					enumerator = null;
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x0600148C RID: 5260 RVA: 0x00075CFC File Offset: 0x00073EFC
		public virtual IEnumerable<FloatMenuOption> GetExtraFloatMenuOptionsFor(IntVec3 sq)
		{
			yield break;
		}

		// Token: 0x0600148D RID: 5261 RVA: 0x00075D08 File Offset: 0x00073F08
		public override TipSignal GetTooltip()
		{
			string value = "";
			if (this.gender != Gender.None)
			{
				if (!this.LabelCap.EqualsIgnoreCase(this.KindLabel))
				{
					value = "PawnTooltipGenderAndKindLabel".Translate(this.GetGenderLabel(), this.KindLabel);
				}
				else
				{
					value = this.GetGenderLabel();
				}
			}
			else if (!this.LabelCap.EqualsIgnoreCase(this.KindLabel))
			{
				value = this.KindLabel;
			}
			string generalConditionLabel = HealthUtility.GetGeneralConditionLabel(this, false);
			bool flag = !string.IsNullOrEmpty(value);
			string text;
			if (this.equipment != null && this.equipment.Primary != null)
			{
				if (flag)
				{
					text = "PawnTooltipWithDescAndPrimaryEquip".Translate(this.LabelCap, value, this.equipment.Primary.LabelCap, generalConditionLabel);
				}
				else
				{
					text = "PawnTooltipWithPrimaryEquipNoDesc".Translate(this.LabelCap, value, generalConditionLabel);
				}
			}
			else if (flag)
			{
				text = "PawnTooltipWithDescNoPrimaryEquip".Translate(this.LabelCap, value, generalConditionLabel);
			}
			else
			{
				text = "PawnTooltipNoDescNoPrimaryEquip".Translate(this.LabelCap, generalConditionLabel);
			}
			return new TipSignal(text, this.thingIDNumber * 152317, TooltipPriority.Pawn);
		}

		// Token: 0x0600148E RID: 5262 RVA: 0x00075E78 File Offset: 0x00074078
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			foreach (StatDrawEntry statDrawEntry in base.SpecialDisplayStats())
			{
				yield return statDrawEntry;
			}
			IEnumerator<StatDrawEntry> enumerator = null;
			yield return new StatDrawEntry(StatCategoryDefOf.BasicsPawn, "BodySize".Translate(), this.BodySize.ToString("F2"), "Stat_Race_BodySize_Desc".Translate(), 500, null, null, false);
			if (this.RaceProps.lifeStageAges.Count > 1)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.BasicsPawn, "Growth".Translate(), this.ageTracker.Growth.ToStringPercent(), "Stat_Race_Growth_Desc".Translate(), 2206, null, null, false);
			}
			if (this.IsWildMan())
			{
				yield return new StatDrawEntry(StatCategoryDefOf.BasicsPawn, "Wildness".Translate(), 0.75f.ToStringPercent(), TrainableUtility.GetWildnessExplanation(this.def), 2050, null, null, false);
			}
			if (ModsConfig.RoyaltyActive && this.RaceProps.intelligence == Intelligence.Humanlike)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.BasicsPawn, "MeditationFocuses".Translate(), MeditationUtility.FocusTypesAvailableForPawnString(this).CapitalizeFirst(), ("MeditationFocusesPawnDesc".Translate() + "\n\n" + MeditationUtility.FocusTypeAvailableExplanation(this)).Resolve(), 99995, null, MeditationUtility.FocusObjectsForPawnHyperlinks(this), false);
			}
			if (this.apparel != null && !this.apparel.AllRequirements.EnumerableNullOrEmpty<ApparelRequirementWithSource>())
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (ApparelRequirementWithSource apparelRequirementWithSource in this.apparel.AllRequirements)
				{
					string text = null;
					string t;
					if (!ApparelUtility.IsRequirementActive(apparelRequirementWithSource.requirement, apparelRequirementWithSource.Source, this, out t))
					{
						text = " [" + "ApparelRequirementDisabledLabel".Translate() + ": " + t + "]";
					}
					stringBuilder.Append("- ");
					bool flag = true;
					foreach (ThingDef thingDef in apparelRequirementWithSource.requirement.AllRequiredApparelForPawn(this, false, false))
					{
						if (!flag)
						{
							stringBuilder.Append(", ");
						}
						stringBuilder.Append(thingDef.LabelCap);
						flag = false;
					}
					if (apparelRequirementWithSource.Source == ApparelRequirementSource.Title)
					{
						stringBuilder.Append(" ");
						stringBuilder.Append("ApparelRequirementOrAnyPsycasterOrPrestigeApparel".Translate());
					}
					stringBuilder.Append(" (");
					stringBuilder.Append("Source".Translate());
					stringBuilder.Append(": ");
					stringBuilder.Append(apparelRequirementWithSource.SourceLabelCap);
					stringBuilder.Append(")");
					if (text != null)
					{
						stringBuilder.Append(text);
					}
					stringBuilder.AppendLine();
				}
				yield return new StatDrawEntry(StatCategoryDefOf.BasicsPawn, "Stat_Pawn_RequiredApparel_Name".Translate(), "", "Stat_Pawn_RequiredApparel_Name".Translate() + ":\n\n" + stringBuilder.ToString(), 100, null, null, false);
			}
			if (ModsConfig.IdeologyActive && this.Ideo != null)
			{
				foreach (StatDrawEntry statDrawEntry2 in DarknessCombatUtility.GetStatEntriesForPawn(this))
				{
					yield return statDrawEntry2;
				}
				enumerator = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x0600148F RID: 5263 RVA: 0x00075E88 File Offset: 0x00074088
		public bool CurrentlyUsableForBills()
		{
			if (!this.InBed())
			{
				JobFailReason.Is(Pawn.NotSurgeryReadyTrans, null);
				return false;
			}
			if (!this.InteractionCell.IsValid)
			{
				JobFailReason.Is(Pawn.CannotReachTrans, null);
				return false;
			}
			return true;
		}

		// Token: 0x06001490 RID: 5264 RVA: 0x00075EC8 File Offset: 0x000740C8
		public bool UsableForBillsAfterFueling()
		{
			return this.CurrentlyUsableForBills();
		}

		// Token: 0x06001491 RID: 5265 RVA: 0x00075ED0 File Offset: 0x000740D0
		public bool AnythingToStrip()
		{
			if (this.equipment != null && this.equipment.HasAnything())
			{
				return true;
			}
			if (this.inventory != null && this.inventory.innerContainer.Count > 0)
			{
				return true;
			}
			if (this.apparel != null)
			{
				if (base.Destroyed)
				{
					if (this.apparel.AnyApparel)
					{
						return true;
					}
				}
				else if (this.apparel.AnyApparelUnlocked)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001492 RID: 5266 RVA: 0x00075F40 File Offset: 0x00074140
		public void Strip()
		{
			Caravan caravan = this.GetCaravan();
			if (caravan != null)
			{
				CaravanInventoryUtility.MoveAllInventoryToSomeoneElse(this, caravan.PawnsListForReading, null);
				if (this.apparel != null)
				{
					CaravanInventoryUtility.MoveAllApparelToSomeonesInventory(this, caravan.PawnsListForReading, base.Destroyed);
				}
				if (this.equipment != null)
				{
					CaravanInventoryUtility.MoveAllEquipmentToSomeonesInventory(this, caravan.PawnsListForReading);
				}
			}
			else
			{
				IntVec3 pos = (this.Corpse != null) ? this.Corpse.PositionHeld : base.PositionHeld;
				if (this.equipment != null)
				{
					this.equipment.DropAllEquipment(pos, false);
				}
				if (this.apparel != null)
				{
					this.apparel.DropAll(pos, false, base.Destroyed);
				}
				if (this.inventory != null)
				{
					this.inventory.DropAllNearPawn(pos, false, false);
				}
			}
			if (base.Faction != null)
			{
				base.Faction.Notify_MemberStripped(this, Faction.OfPlayer);
			}
		}

		// Token: 0x1700044D RID: 1101
		// (get) Token: 0x06001493 RID: 5267 RVA: 0x0007600F File Offset: 0x0007420F
		public TradeCurrency TradeCurrency
		{
			get
			{
				return this.TraderKind.tradeCurrency;
			}
		}

		// Token: 0x06001494 RID: 5268 RVA: 0x0007601C File Offset: 0x0007421C
		public IEnumerable<Thing> ColonyThingsWillingToBuy(Pawn playerNegotiator)
		{
			return this.trader.ColonyThingsWillingToBuy(playerNegotiator);
		}

		// Token: 0x06001495 RID: 5269 RVA: 0x0007602A File Offset: 0x0007422A
		public void GiveSoldThingToTrader(Thing toGive, int countToGive, Pawn playerNegotiator)
		{
			this.trader.GiveSoldThingToTrader(toGive, countToGive, playerNegotiator);
		}

		// Token: 0x06001496 RID: 5270 RVA: 0x0007603A File Offset: 0x0007423A
		public void GiveSoldThingToPlayer(Thing toGive, int countToGive, Pawn playerNegotiator)
		{
			this.trader.GiveSoldThingToPlayer(toGive, countToGive, playerNegotiator);
		}

		// Token: 0x06001497 RID: 5271 RVA: 0x0007604C File Offset: 0x0007424C
		public void HearClamor(Thing source, ClamorDef type)
		{
			if (this.Dead || this.Downed)
			{
				return;
			}
			if (type == ClamorDefOf.Movement)
			{
				Pawn pawn = source as Pawn;
				if (pawn != null)
				{
					this.CheckForDisturbedSleep(pawn);
				}
				this.NotifyLordOfClamor(source, type);
			}
			if (type == ClamorDefOf.Harm && base.Faction != Faction.OfPlayer && !this.Awake() && base.Faction == source.Faction && this.HostFaction == null)
			{
				this.mindState.canSleepTick = Find.TickManager.TicksGame + 1000;
				if (this.CurJob != null)
				{
					this.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
				}
				this.NotifyLordOfClamor(source, type);
			}
			if (type == ClamorDefOf.Construction && base.Faction != Faction.OfPlayer && !this.Awake() && base.Faction != source.Faction && this.HostFaction == null)
			{
				this.mindState.canSleepTick = Find.TickManager.TicksGame + 1000;
				if (this.CurJob != null)
				{
					this.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
				}
				this.NotifyLordOfClamor(source, type);
			}
			if (type == ClamorDefOf.Ability && base.Faction != Faction.OfPlayer && base.Faction != source.Faction && this.HostFaction == null)
			{
				if (!this.Awake())
				{
					this.mindState.canSleepTick = Find.TickManager.TicksGame + 1000;
					if (this.CurJob != null)
					{
						this.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
					}
				}
				this.NotifyLordOfClamor(source, type);
			}
			if (type == ClamorDefOf.Impact)
			{
				this.mindState.Notify_ClamorImpact(source);
				if (this.CurJob != null && !this.Awake())
				{
					this.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
				}
				this.NotifyLordOfClamor(source, type);
			}
		}

		// Token: 0x06001498 RID: 5272 RVA: 0x0007620C File Offset: 0x0007440C
		private void NotifyLordOfClamor(Thing source, ClamorDef type)
		{
			Lord lord = this.GetLord();
			if (lord != null)
			{
				lord.Notify_Clamor(source, type);
			}
		}

		// Token: 0x06001499 RID: 5273 RVA: 0x0007622B File Offset: 0x0007442B
		public override void Notify_Explosion(Explosion explosion)
		{
			base.Notify_Explosion(explosion);
			this.mindState.Notify_Explosion(explosion);
		}

		// Token: 0x0600149A RID: 5274 RVA: 0x00076240 File Offset: 0x00074440
		public override void Notify_BulletImpactNearby(BulletImpactData impactData)
		{
			Pawn_ApparelTracker pawn_ApparelTracker = this.apparel;
			if (pawn_ApparelTracker == null)
			{
				return;
			}
			pawn_ApparelTracker.Notify_BulletImpactNearby(impactData);
		}

		// Token: 0x0600149B RID: 5275 RVA: 0x00076254 File Offset: 0x00074454
		private void CheckForDisturbedSleep(Pawn source)
		{
			if (this.needs.mood == null)
			{
				return;
			}
			if (this.Awake())
			{
				return;
			}
			if (base.Faction != Faction.OfPlayer)
			{
				return;
			}
			if (Find.TickManager.TicksGame < this.lastSleepDisturbedTick + 300)
			{
				return;
			}
			if (source != null)
			{
				if (LovePartnerRelationUtility.LovePartnerRelationExists(this, source))
				{
					return;
				}
				if (source.RaceProps.petness > 0f)
				{
					return;
				}
				if (source.relations != null)
				{
					if (source.relations.DirectRelations.Any((DirectPawnRelation dr) => dr.def == PawnRelationDefOf.Bond))
					{
						return;
					}
				}
			}
			this.lastSleepDisturbedTick = Find.TickManager.TicksGame;
			this.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.SleepDisturbed, null, null);
		}

		// Token: 0x0600149C RID: 5276 RVA: 0x0007632C File Offset: 0x0007452C
		public float GetAcceptArrestChance(Pawn arrester)
		{
			if (this.health.Downed || this.WorkTagIsDisabled(WorkTags.Violent) || (this.guilt != null && this.guilt.IsGuilty && this.IsColonist && !this.IsQuestLodger()))
			{
				return 1f;
			}
			return (StatDefOf.ArrestSuccessChance.Worker.IsDisabledFor(arrester) ? StatDefOf.ArrestSuccessChance.valueIfMissing : arrester.GetStatValue(StatDefOf.ArrestSuccessChance, true)) * this.kindDef.acceptArrestChanceFactor;
		}

		// Token: 0x0600149D RID: 5277 RVA: 0x000763B0 File Offset: 0x000745B0
		public bool CheckAcceptArrest(Pawn arrester)
		{
			Faction homeFaction = this.HomeFaction;
			if (homeFaction != null && homeFaction != arrester.factionInt)
			{
				homeFaction.Notify_MemberCaptured(this, arrester.Faction);
			}
			if (this.health.Downed)
			{
				return true;
			}
			if (this.WorkTagIsDisabled(WorkTags.Violent))
			{
				return true;
			}
			float acceptArrestChance = this.GetAcceptArrestChance(arrester);
			if (Rand.Value < acceptArrestChance)
			{
				return true;
			}
			Messages.Message("MessageRefusedArrest".Translate(this.LabelShort, this), this, MessageTypeDefOf.ThreatSmall, true);
			if (base.Faction == null || !arrester.HostileTo(this))
			{
				this.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Berserk, null, false, false, null, false, false, false);
			}
			return false;
		}

		// Token: 0x0600149E RID: 5278 RVA: 0x0007646C File Offset: 0x0007466C
		public bool ThreatDisabled(IAttackTargetSearcher disabledFor)
		{
			if (!base.Spawned)
			{
				return true;
			}
			if (!this.InMentalState && this.GetTraderCaravanRole() == TraderCaravanRole.Carrier && !(this.jobs.curDriver is JobDriver_AttackMelee))
			{
				return true;
			}
			if (this.mindState.duty != null && this.mindState.duty.def.threatDisabled)
			{
				return true;
			}
			if (!this.mindState.Active)
			{
				return true;
			}
			Pawn pawn = ((disabledFor != null) ? disabledFor.Thing : null) as Pawn;
			if (this.Downed)
			{
				if (disabledFor == null)
				{
					return true;
				}
				if (pawn == null || pawn.mindState == null || pawn.mindState.duty == null || !pawn.mindState.duty.attackDownedIfStarving || !pawn.Starving())
				{
					return true;
				}
			}
			return this.IsInvisible() || (pawn != null && (this.ThreatDisabledBecauseNonAggressiveRoamer(pawn) || pawn.ThreatDisabledBecauseNonAggressiveRoamer(this)));
		}

		// Token: 0x0600149F RID: 5279 RVA: 0x00076558 File Offset: 0x00074758
		public bool ThreatDisabledBecauseNonAggressiveRoamer(Pawn otherPawn)
		{
			if (!this.RaceProps.Roamer || base.Faction != Faction.OfPlayer)
			{
				return false;
			}
			Lord lord = otherPawn.GetLord();
			return lord != null && !lord.CurLordToil.AllowAggressiveTargettingOfRoamers && !this.InAggroMentalState && !this.IsFighting() && Find.TickManager.TicksGame >= this.mindState.lastEngageTargetTick + 360;
		}

		// Token: 0x060014A0 RID: 5280 RVA: 0x000765CC File Offset: 0x000747CC
		public List<WorkTypeDef> GetDisabledWorkTypes(bool permanentOnly = false)
		{
			Pawn.<>c__DisplayClass258_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.permanentOnly = permanentOnly;
			if (CS$<>8__locals1.permanentOnly)
			{
				if (this.cachedDisabledWorkTypesPermanent == null)
				{
					this.cachedDisabledWorkTypesPermanent = new List<WorkTypeDef>();
				}
				this.<GetDisabledWorkTypes>g__FillList|258_0(this.cachedDisabledWorkTypesPermanent, ref CS$<>8__locals1);
				return this.cachedDisabledWorkTypesPermanent;
			}
			if (this.cachedDisabledWorkTypes == null)
			{
				this.cachedDisabledWorkTypes = new List<WorkTypeDef>();
			}
			this.<GetDisabledWorkTypes>g__FillList|258_0(this.cachedDisabledWorkTypes, ref CS$<>8__locals1);
			return this.cachedDisabledWorkTypes;
		}

		// Token: 0x060014A1 RID: 5281 RVA: 0x00076640 File Offset: 0x00074840
		public List<string> GetReasonsForDisabledWorkType(WorkTypeDef workType)
		{
			if (this.cachedReasonsForDisabledWorkTypes != null)
			{
				return this.cachedReasonsForDisabledWorkTypes;
			}
			this.cachedReasonsForDisabledWorkTypes = new List<string>();
			foreach (Backstory backstory in this.story.AllBackstories)
			{
				foreach (WorkTypeDef workTypeDef in backstory.DisabledWorkTypes)
				{
					if (workType == workTypeDef)
					{
						this.cachedReasonsForDisabledWorkTypes.Add("WorkDisabledByBackstory".Translate(backstory.TitleCapFor(this.gender)));
						break;
					}
				}
			}
			for (int i = 0; i < this.story.traits.allTraits.Count; i++)
			{
				Trait trait = this.story.traits.allTraits[i];
				using (IEnumerator<WorkTypeDef> enumerator2 = trait.GetDisabledWorkTypes().GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						if (enumerator2.Current == workType)
						{
							this.cachedReasonsForDisabledWorkTypes.Add("WorkDisabledByTrait".Translate(trait.LabelCap));
							break;
						}
					}
				}
			}
			if (this.royalty != null)
			{
				foreach (RoyalTitle royalTitle in this.royalty.AllTitlesForReading)
				{
					if (royalTitle.conceited)
					{
						foreach (WorkTypeDef workTypeDef2 in royalTitle.def.DisabledWorkTypes)
						{
							if (workType == workTypeDef2)
							{
								this.cachedReasonsForDisabledWorkTypes.Add("WorkDisabledByRoyalTitle".Translate(royalTitle.Label));
								break;
							}
						}
					}
				}
			}
			if (ModsConfig.IdeologyActive && this.Ideo != null)
			{
				Precept_Role role = this.Ideo.GetRole(this);
				if (role != null)
				{
					foreach (WorkTypeDef workTypeDef3 in role.DisabledWorkTypes)
					{
						if (workType == workTypeDef3)
						{
							this.cachedReasonsForDisabledWorkTypes.Add("WorkDisabledRole".Translate(role.LabelForPawn(this)));
							break;
						}
					}
				}
			}
			foreach (QuestPart_WorkDisabled questPart_WorkDisabled in QuestUtility.GetWorkDisabledQuestPart(this))
			{
				foreach (WorkTypeDef workTypeDef4 in questPart_WorkDisabled.DisabledWorkTypes)
				{
					if (workType == workTypeDef4)
					{
						this.cachedReasonsForDisabledWorkTypes.Add("WorkDisabledByQuest".Translate(questPart_WorkDisabled.quest.name));
						break;
					}
				}
			}
			if (this.guest != null && this.guest.IsSlave)
			{
				foreach (WorkTypeDef workTypeDef5 in this.guest.GetDisabledWorkTypes())
				{
					if (workType == workTypeDef5)
					{
						this.cachedReasonsForDisabledWorkTypes.Add("WorkDisabledSlave".Translate());
						break;
					}
				}
			}
			return this.cachedReasonsForDisabledWorkTypes;
		}

		// Token: 0x060014A2 RID: 5282 RVA: 0x00076A0C File Offset: 0x00074C0C
		public bool WorkTypeIsDisabled(WorkTypeDef w)
		{
			return this.GetDisabledWorkTypes(false).Contains(w);
		}

		// Token: 0x060014A3 RID: 5283 RVA: 0x00076A1C File Offset: 0x00074C1C
		public bool OneOfWorkTypesIsDisabled(List<WorkTypeDef> wts)
		{
			for (int i = 0; i < wts.Count; i++)
			{
				if (this.WorkTypeIsDisabled(wts[i]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060014A4 RID: 5284 RVA: 0x00076A4C File Offset: 0x00074C4C
		public void Notify_DisabledWorkTypesChanged()
		{
			this.cachedDisabledWorkTypes = null;
			this.cachedDisabledWorkTypesPermanent = null;
			this.cachedReasonsForDisabledWorkTypes = null;
			Pawn_WorkSettings pawn_WorkSettings = this.workSettings;
			if (pawn_WorkSettings != null)
			{
				pawn_WorkSettings.Notify_DisabledWorkTypesChanged();
			}
			this.skills.Notify_SkillDisablesChanged();
		}

		// Token: 0x1700044E RID: 1102
		// (get) Token: 0x060014A5 RID: 5285 RVA: 0x00076A80 File Offset: 0x00074C80
		public WorkTags CombinedDisabledWorkTags
		{
			get
			{
				WorkTags workTags = (this.story != null) ? this.story.DisabledWorkTagsBackstoryAndTraits : WorkTags.None;
				if (this.royalty != null)
				{
					foreach (RoyalTitle royalTitle in this.royalty.AllTitlesForReading)
					{
						if (royalTitle.conceited)
						{
							workTags |= royalTitle.def.disabledWorkTags;
						}
					}
				}
				if (ModsConfig.IdeologyActive && this.Ideo != null)
				{
					Precept_Role role = this.Ideo.GetRole(this);
					if (role != null)
					{
						workTags |= role.def.roleDisabledWorkTags;
					}
				}
				if (this.health != null && this.health.hediffSet != null)
				{
					foreach (Hediff hediff in this.health.hediffSet.hediffs)
					{
						HediffStage curStage = hediff.CurStage;
						if (curStage != null)
						{
							workTags |= curStage.disabledWorkTags;
						}
					}
				}
				foreach (QuestPart_WorkDisabled questPart_WorkDisabled in QuestUtility.GetWorkDisabledQuestPart(this))
				{
					workTags |= questPart_WorkDisabled.disabledWorkTags;
				}
				return workTags;
			}
		}

		// Token: 0x060014A6 RID: 5286 RVA: 0x00076BE8 File Offset: 0x00074DE8
		public bool WorkTagIsDisabled(WorkTags w)
		{
			return (this.CombinedDisabledWorkTags & w) > WorkTags.None;
		}

		// Token: 0x060014A7 RID: 5287 RVA: 0x00076BF8 File Offset: 0x00074DF8
		public override bool PreventPlayerSellingThingsNearby(out string reason)
		{
			if (base.Faction.HostileTo(Faction.OfPlayer) && this.HostFaction == null && !this.Downed && !this.InMentalState)
			{
				reason = "Enemies".Translate();
				return true;
			}
			reason = null;
			return false;
		}

		// Token: 0x060014A8 RID: 5288 RVA: 0x00076C46 File Offset: 0x00074E46
		public void ChangeKind(PawnKindDef newKindDef)
		{
			if (this.kindDef == newKindDef)
			{
				return;
			}
			this.kindDef = newKindDef;
			if (this.kindDef == PawnKindDefOf.WildMan)
			{
				this.mindState.WildManEverReachedOutside = false;
				ReachabilityUtility.ClearCacheFor(this);
			}
		}

		// Token: 0x1700044F RID: 1103
		// (get) Token: 0x060014A9 RID: 5289 RVA: 0x00076C78 File Offset: 0x00074E78
		public bool HasPsylink
		{
			get
			{
				return this.psychicEntropy != null && this.psychicEntropy.Psylink != null;
			}
		}

		// Token: 0x060014B1 RID: 5297 RVA: 0x00076D44 File Offset: 0x00074F44
		[CompilerGenerated]
		private void <GetDisabledWorkTypes>g__FillList|258_0(List<WorkTypeDef> list, ref Pawn.<>c__DisplayClass258_0 A_2)
		{
			if (this.story != null && !this.IsSlave)
			{
				foreach (Backstory backstory in this.story.AllBackstories)
				{
					foreach (WorkTypeDef item in backstory.DisabledWorkTypes)
					{
						if (!list.Contains(item))
						{
							list.Add(item);
						}
					}
				}
				for (int i = 0; i < this.story.traits.allTraits.Count; i++)
				{
					foreach (WorkTypeDef item2 in this.story.traits.allTraits[i].GetDisabledWorkTypes())
					{
						if (!list.Contains(item2))
						{
							list.Add(item2);
						}
					}
				}
			}
			if (this.royalty != null && !A_2.permanentOnly && !this.IsSlave)
			{
				foreach (RoyalTitle royalTitle in this.royalty.AllTitlesForReading)
				{
					if (royalTitle.conceited)
					{
						foreach (WorkTypeDef item3 in royalTitle.def.DisabledWorkTypes)
						{
							if (!list.Contains(item3))
							{
								list.Add(item3);
							}
						}
					}
				}
			}
			if (ModsConfig.IdeologyActive && this.Ideo != null)
			{
				Precept_Role role = this.Ideo.GetRole(this);
				if (role != null)
				{
					foreach (WorkTypeDef item4 in role.DisabledWorkTypes)
					{
						if (!list.Contains(item4))
						{
							list.Add(item4);
						}
					}
				}
			}
			foreach (QuestPart_WorkDisabled questPart_WorkDisabled in QuestUtility.GetWorkDisabledQuestPart(this))
			{
				foreach (WorkTypeDef item5 in questPart_WorkDisabled.DisabledWorkTypes)
				{
					if (!list.Contains(item5))
					{
						list.Add(item5);
					}
				}
			}
			if (this.guest != null)
			{
				foreach (WorkTypeDef item6 in this.guest.GetDisabledWorkTypes())
				{
					if (!list.Contains(item6))
					{
						list.Add(item6);
					}
				}
			}
		}

		// Token: 0x04000E92 RID: 3730
		public PawnKindDef kindDef;

		// Token: 0x04000E93 RID: 3731
		private Name nameInt;

		// Token: 0x04000E94 RID: 3732
		public Gender gender;

		// Token: 0x04000E95 RID: 3733
		public Pawn_AgeTracker ageTracker;

		// Token: 0x04000E96 RID: 3734
		public Pawn_HealthTracker health;

		// Token: 0x04000E97 RID: 3735
		public Pawn_RecordsTracker records;

		// Token: 0x04000E98 RID: 3736
		public Pawn_InventoryTracker inventory;

		// Token: 0x04000E99 RID: 3737
		public Pawn_MeleeVerbs meleeVerbs;

		// Token: 0x04000E9A RID: 3738
		public VerbTracker verbTracker;

		// Token: 0x04000E9B RID: 3739
		public Pawn_Ownership ownership;

		// Token: 0x04000E9C RID: 3740
		public Pawn_CarryTracker carryTracker;

		// Token: 0x04000E9D RID: 3741
		public Pawn_NeedsTracker needs;

		// Token: 0x04000E9E RID: 3742
		public Pawn_MindState mindState;

		// Token: 0x04000E9F RID: 3743
		public Pawn_SurroundingsTracker surroundings;

		// Token: 0x04000EA0 RID: 3744
		public Pawn_RotationTracker rotationTracker;

		// Token: 0x04000EA1 RID: 3745
		public Pawn_PathFollower pather;

		// Token: 0x04000EA2 RID: 3746
		public Pawn_Thinker thinker;

		// Token: 0x04000EA3 RID: 3747
		public Pawn_JobTracker jobs;

		// Token: 0x04000EA4 RID: 3748
		public Pawn_StanceTracker stances;

		// Token: 0x04000EA5 RID: 3749
		public Pawn_NativeVerbs natives;

		// Token: 0x04000EA6 RID: 3750
		public Pawn_FilthTracker filth;

		// Token: 0x04000EA7 RID: 3751
		public Pawn_RopeTracker roping;

		// Token: 0x04000EA8 RID: 3752
		public Pawn_EquipmentTracker equipment;

		// Token: 0x04000EA9 RID: 3753
		public Pawn_ApparelTracker apparel;

		// Token: 0x04000EAA RID: 3754
		public Pawn_SkillTracker skills;

		// Token: 0x04000EAB RID: 3755
		public Pawn_StoryTracker story;

		// Token: 0x04000EAC RID: 3756
		public Pawn_GuestTracker guest;

		// Token: 0x04000EAD RID: 3757
		public Pawn_GuiltTracker guilt;

		// Token: 0x04000EAE RID: 3758
		public Pawn_RoyaltyTracker royalty;

		// Token: 0x04000EAF RID: 3759
		public Pawn_AbilityTracker abilities;

		// Token: 0x04000EB0 RID: 3760
		public Pawn_IdeoTracker ideo;

		// Token: 0x04000EB1 RID: 3761
		public Pawn_WorkSettings workSettings;

		// Token: 0x04000EB2 RID: 3762
		public Pawn_TraderTracker trader;

		// Token: 0x04000EB3 RID: 3763
		public Pawn_StyleTracker style;

		// Token: 0x04000EB4 RID: 3764
		public Pawn_StyleObserverTracker styleObserver;

		// Token: 0x04000EB5 RID: 3765
		public Pawn_ConnectionsTracker connections;

		// Token: 0x04000EB6 RID: 3766
		public Pawn_TrainingTracker training;

		// Token: 0x04000EB7 RID: 3767
		public Pawn_CallTracker caller;

		// Token: 0x04000EB8 RID: 3768
		public Pawn_RelationsTracker relations;

		// Token: 0x04000EB9 RID: 3769
		public Pawn_PsychicEntropyTracker psychicEntropy;

		// Token: 0x04000EBA RID: 3770
		public Pawn_InteractionsTracker interactions;

		// Token: 0x04000EBB RID: 3771
		public Pawn_PlayerSettings playerSettings;

		// Token: 0x04000EBC RID: 3772
		public Pawn_OutfitTracker outfits;

		// Token: 0x04000EBD RID: 3773
		public Pawn_DrugPolicyTracker drugs;

		// Token: 0x04000EBE RID: 3774
		public Pawn_FoodRestrictionTracker foodRestriction;

		// Token: 0x04000EBF RID: 3775
		public Pawn_TimetableTracker timetable;

		// Token: 0x04000EC0 RID: 3776
		public Pawn_InventoryStockTracker inventoryStock;

		// Token: 0x04000EC1 RID: 3777
		public Pawn_DraftController drafter;

		// Token: 0x04000EC2 RID: 3778
		private Pawn_DrawTracker drawer;

		// Token: 0x04000EC3 RID: 3779
		public int becameWorldPawnTickAbs = -1;

		// Token: 0x04000EC4 RID: 3780
		public bool teleporting;

		// Token: 0x04000EC5 RID: 3781
		public bool forceNoDeathNotification;

		// Token: 0x04000EC6 RID: 3782
		private const float HumanSizedHeatOutput = 0.3f;

		// Token: 0x04000EC7 RID: 3783
		private const float AnimalHeatOutputFactor = 0.6f;

		// Token: 0x04000EC8 RID: 3784
		private static string NotSurgeryReadyTrans;

		// Token: 0x04000EC9 RID: 3785
		private static string CannotReachTrans;

		// Token: 0x04000ECA RID: 3786
		public const int MaxMoveTicks = 450;

		// Token: 0x04000ECB RID: 3787
		private static List<ExtraFaction> tmpExtraFactions = new List<ExtraFaction>();

		// Token: 0x04000ECC RID: 3788
		private static List<string> states = new List<string>();

		// Token: 0x04000ECD RID: 3789
		private int lastSleepDisturbedTick;

		// Token: 0x04000ECE RID: 3790
		private const int SleepDisturbanceMinInterval = 300;

		// Token: 0x04000ECF RID: 3791
		private List<WorkTypeDef> cachedDisabledWorkTypes;

		// Token: 0x04000ED0 RID: 3792
		private List<WorkTypeDef> cachedDisabledWorkTypesPermanent;

		// Token: 0x04000ED1 RID: 3793
		private List<string> cachedReasonsForDisabledWorkTypes;
	}
}
