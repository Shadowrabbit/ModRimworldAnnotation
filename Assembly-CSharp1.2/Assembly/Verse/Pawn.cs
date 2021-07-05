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
	// Token: 0x02000435 RID: 1077
	public class Pawn : ThingWithComps, IStrippable, IBillGiver, IVerbOwner, ITrader, IAttackTarget, ILoadReferenceable, IAttackTargetSearcher, IThingHolder
	{
		// Token: 0x170004E0 RID: 1248
		// (get) Token: 0x06001A25 RID: 6693 RVA: 0x0001840D File Offset: 0x0001660D
		// (set) Token: 0x06001A26 RID: 6694 RVA: 0x00018415 File Offset: 0x00016615
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

		// Token: 0x170004E1 RID: 1249
		// (get) Token: 0x06001A27 RID: 6695 RVA: 0x0001841E File Offset: 0x0001661E
		public RaceProperties RaceProps
		{
			get
			{
				return this.def.race;
			}
		}

		// Token: 0x170004E2 RID: 1250
		// (get) Token: 0x06001A28 RID: 6696 RVA: 0x0001842B File Offset: 0x0001662B
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

		// Token: 0x170004E3 RID: 1251
		// (get) Token: 0x06001A29 RID: 6697 RVA: 0x00018442 File Offset: 0x00016642
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

		// Token: 0x170004E4 RID: 1252
		// (get) Token: 0x06001A2A RID: 6698 RVA: 0x00018459 File Offset: 0x00016659
		public bool Downed
		{
			get
			{
				return this.health.Downed;
			}
		}

		// Token: 0x170004E5 RID: 1253
		// (get) Token: 0x06001A2B RID: 6699 RVA: 0x00018466 File Offset: 0x00016666
		public bool Dead
		{
			get
			{
				return this.health.Dead;
			}
		}

		// Token: 0x170004E6 RID: 1254
		// (get) Token: 0x06001A2C RID: 6700 RVA: 0x00018473 File Offset: 0x00016673
		public string KindLabel
		{
			get
			{
				return GenLabel.BestKindLabel(this, false, false, false, -1);
			}
		}

		// Token: 0x170004E7 RID: 1255
		// (get) Token: 0x06001A2D RID: 6701 RVA: 0x0001847F File Offset: 0x0001667F
		public bool InMentalState
		{
			get
			{
				return !this.Dead && this.mindState.mentalStateHandler.InMentalState;
			}
		}

		// Token: 0x170004E8 RID: 1256
		// (get) Token: 0x06001A2E RID: 6702 RVA: 0x0001849B File Offset: 0x0001669B
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

		// Token: 0x170004E9 RID: 1257
		// (get) Token: 0x06001A2F RID: 6703 RVA: 0x000184B7 File Offset: 0x000166B7
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

		// Token: 0x170004EA RID: 1258
		// (get) Token: 0x06001A30 RID: 6704 RVA: 0x000184D3 File Offset: 0x000166D3
		public bool InAggroMentalState
		{
			get
			{
				return !this.Dead && this.mindState.mentalStateHandler.InMentalState && this.mindState.mentalStateHandler.CurStateDef.IsAggro;
			}
		}

		// Token: 0x170004EB RID: 1259
		// (get) Token: 0x06001A31 RID: 6705 RVA: 0x00018508 File Offset: 0x00016708
		public bool Inspired
		{
			get
			{
				return !this.Dead && this.mindState.inspirationHandler.Inspired;
			}
		}

		// Token: 0x170004EC RID: 1260
		// (get) Token: 0x06001A32 RID: 6706 RVA: 0x00018524 File Offset: 0x00016724
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

		// Token: 0x170004ED RID: 1261
		// (get) Token: 0x06001A33 RID: 6707 RVA: 0x00018540 File Offset: 0x00016740
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

		// Token: 0x170004EE RID: 1262
		// (get) Token: 0x06001A34 RID: 6708 RVA: 0x0001855C File Offset: 0x0001675C
		public override Vector3 DrawPos
		{
			get
			{
				return this.Drawer.DrawPos;
			}
		}

		// Token: 0x170004EF RID: 1263
		// (get) Token: 0x06001A35 RID: 6709 RVA: 0x00018569 File Offset: 0x00016769
		public VerbTracker VerbTracker
		{
			get
			{
				return this.verbTracker;
			}
		}

		// Token: 0x170004F0 RID: 1264
		// (get) Token: 0x06001A36 RID: 6710 RVA: 0x00018571 File Offset: 0x00016771
		public List<VerbProperties> VerbProperties
		{
			get
			{
				return this.def.Verbs;
			}
		}

		// Token: 0x170004F1 RID: 1265
		// (get) Token: 0x06001A37 RID: 6711 RVA: 0x0001857E File Offset: 0x0001677E
		public List<Tool> Tools
		{
			get
			{
				return this.def.tools;
			}
		}

		// Token: 0x170004F2 RID: 1266
		// (get) Token: 0x06001A38 RID: 6712 RVA: 0x0001858B File Offset: 0x0001678B
		public bool IsColonist
		{
			get
			{
				return base.Faction != null && base.Faction.IsPlayer && this.RaceProps.Humanlike;
			}
		}

		// Token: 0x170004F3 RID: 1267
		// (get) Token: 0x06001A39 RID: 6713 RVA: 0x000185AF File Offset: 0x000167AF
		public bool IsFreeColonist
		{
			get
			{
				return this.IsColonist && this.HostFaction == null;
			}
		}

		// Token: 0x170004F4 RID: 1268
		// (get) Token: 0x06001A3A RID: 6714 RVA: 0x000185C4 File Offset: 0x000167C4
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

		// Token: 0x170004F5 RID: 1269
		// (get) Token: 0x06001A3B RID: 6715 RVA: 0x000185DB File Offset: 0x000167DB
		public bool Drafted
		{
			get
			{
				return this.drafter != null && this.drafter.Drafted;
			}
		}

		// Token: 0x170004F6 RID: 1270
		// (get) Token: 0x06001A3C RID: 6716 RVA: 0x000185F2 File Offset: 0x000167F2
		public bool IsPrisoner
		{
			get
			{
				return this.guest != null && this.guest.IsPrisoner;
			}
		}

		// Token: 0x170004F7 RID: 1271
		// (get) Token: 0x06001A3D RID: 6717 RVA: 0x00018609 File Offset: 0x00016809
		public bool IsPrisonerOfColony
		{
			get
			{
				return this.guest != null && this.guest.IsPrisoner && this.guest.HostFaction.IsPlayer;
			}
		}

		// Token: 0x170004F8 RID: 1272
		// (get) Token: 0x06001A3E RID: 6718 RVA: 0x00018632 File Offset: 0x00016832
		public bool IsColonistPlayerControlled
		{
			get
			{
				return base.Spawned && this.IsColonist && this.MentalStateDef == null && this.HostFaction == null;
			}
		}

		// Token: 0x170004F9 RID: 1273
		// (get) Token: 0x06001A3F RID: 6719 RVA: 0x00018657 File Offset: 0x00016857
		public IEnumerable<IntVec3> IngredientStackCells
		{
			get
			{
				yield return this.InteractionCell;
				yield break;
			}
		}

		// Token: 0x170004FA RID: 1274
		// (get) Token: 0x06001A40 RID: 6720 RVA: 0x00018667 File Offset: 0x00016867
		public bool InContainerEnclosed
		{
			get
			{
				return base.ParentHolder.IsEnclosingContainer();
			}
		}

		// Token: 0x170004FB RID: 1275
		// (get) Token: 0x06001A41 RID: 6721 RVA: 0x00018674 File Offset: 0x00016874
		public Corpse Corpse
		{
			get
			{
				return base.ParentHolder as Corpse;
			}
		}

		// Token: 0x170004FC RID: 1276
		// (get) Token: 0x06001A42 RID: 6722 RVA: 0x000E4CDC File Offset: 0x000E2EDC
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

		// Token: 0x170004FD RID: 1277
		// (get) Token: 0x06001A43 RID: 6723 RVA: 0x000E4D0C File Offset: 0x000E2F0C
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

		// Token: 0x170004FE RID: 1278
		// (get) Token: 0x06001A44 RID: 6724 RVA: 0x00018681 File Offset: 0x00016881
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

		// Token: 0x170004FF RID: 1279
		// (get) Token: 0x06001A45 RID: 6725 RVA: 0x000E4D70 File Offset: 0x000E2F70
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

		// Token: 0x17000500 RID: 1280
		// (get) Token: 0x06001A46 RID: 6726 RVA: 0x0001869D File Offset: 0x0001689D
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

		// Token: 0x17000501 RID: 1281
		// (get) Token: 0x06001A47 RID: 6727 RVA: 0x000186CD File Offset: 0x000168CD
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

		// Token: 0x17000502 RID: 1282
		// (get) Token: 0x06001A48 RID: 6728 RVA: 0x000186FD File Offset: 0x000168FD
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

		// Token: 0x17000503 RID: 1283
		// (get) Token: 0x06001A49 RID: 6729 RVA: 0x00018719 File Offset: 0x00016919
		public Faction FactionOrExtraMiniOrHomeFaction
		{
			get
			{
				if (base.Faction == null || !base.Faction.IsPlayer)
				{
					return base.Faction;
				}
				if (this.HasExtraMiniFaction(null))
				{
					return this.GetExtraMiniFaction(null);
				}
				return this.GetExtraHomeFaction(null) ?? base.Faction;
			}
		}

		// Token: 0x17000504 RID: 1284
		// (get) Token: 0x06001A4A RID: 6730 RVA: 0x00018759 File Offset: 0x00016959
		public BillStack BillStack
		{
			get
			{
				return this.health.surgeryBills;
			}
		}

		// Token: 0x17000505 RID: 1285
		// (get) Token: 0x06001A4B RID: 6731 RVA: 0x000E4DF8 File Offset: 0x000E2FF8
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

		// Token: 0x17000506 RID: 1286
		// (get) Token: 0x06001A4C RID: 6732 RVA: 0x00018766 File Offset: 0x00016966
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

		// Token: 0x17000507 RID: 1287
		// (get) Token: 0x06001A4D RID: 6733 RVA: 0x0001877D File Offset: 0x0001697D
		public IEnumerable<Thing> Goods
		{
			get
			{
				return this.trader.Goods;
			}
		}

		// Token: 0x17000508 RID: 1288
		// (get) Token: 0x06001A4E RID: 6734 RVA: 0x0001878A File Offset: 0x0001698A
		public int RandomPriceFactorSeed
		{
			get
			{
				return this.trader.RandomPriceFactorSeed;
			}
		}

		// Token: 0x17000509 RID: 1289
		// (get) Token: 0x06001A4F RID: 6735 RVA: 0x00018797 File Offset: 0x00016997
		public string TraderName
		{
			get
			{
				return this.trader.TraderName;
			}
		}

		// Token: 0x1700050A RID: 1290
		// (get) Token: 0x06001A50 RID: 6736 RVA: 0x000187A4 File Offset: 0x000169A4
		public bool CanTradeNow
		{
			get
			{
				return this.trader != null && this.trader.CanTradeNow;
			}
		}

		// Token: 0x1700050B RID: 1291
		// (get) Token: 0x06001A51 RID: 6737 RVA: 0x00016647 File Offset: 0x00014847
		public float TradePriceImprovementOffsetForPlayer
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x1700050C RID: 1292
		// (get) Token: 0x06001A52 RID: 6738 RVA: 0x000187BB File Offset: 0x000169BB
		public float BodySize
		{
			get
			{
				return this.ageTracker.CurLifeStage.bodySizeFactor * this.RaceProps.baseBodySize;
			}
		}

		// Token: 0x1700050D RID: 1293
		// (get) Token: 0x06001A53 RID: 6739 RVA: 0x000187D9 File Offset: 0x000169D9
		public float HealthScale
		{
			get
			{
				return this.ageTracker.CurLifeStage.healthScaleFactor * this.RaceProps.baseHealthScale;
			}
		}

		// Token: 0x1700050E RID: 1294
		// (get) Token: 0x06001A54 RID: 6740 RVA: 0x000187F7 File Offset: 0x000169F7
		Thing IAttackTarget.Thing
		{
			get
			{
				return this;
			}
		}

		// Token: 0x1700050F RID: 1295
		// (get) Token: 0x06001A55 RID: 6741 RVA: 0x0000CE6C File Offset: 0x0000B06C
		public float TargetPriorityFactor
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x17000510 RID: 1296
		// (get) Token: 0x06001A56 RID: 6742 RVA: 0x000E513C File Offset: 0x000E333C
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

		// Token: 0x17000511 RID: 1297
		// (get) Token: 0x06001A57 RID: 6743 RVA: 0x000187F7 File Offset: 0x000169F7
		Thing IAttackTargetSearcher.Thing
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000512 RID: 1298
		// (get) Token: 0x06001A58 RID: 6744 RVA: 0x000187FA File Offset: 0x000169FA
		public LocalTargetInfo LastAttackedTarget
		{
			get
			{
				return this.mindState.lastAttackedTarget;
			}
		}

		// Token: 0x17000513 RID: 1299
		// (get) Token: 0x06001A59 RID: 6745 RVA: 0x00018807 File Offset: 0x00016A07
		public int LastAttackTargetTick
		{
			get
			{
				return this.mindState.lastAttackTargetTick;
			}
		}

		// Token: 0x17000514 RID: 1300
		// (get) Token: 0x06001A5A RID: 6746 RVA: 0x000E5184 File Offset: 0x000E3384
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

		// Token: 0x06001A5B RID: 6747 RVA: 0x00018814 File Offset: 0x00016A14
		string IVerbOwner.UniqueVerbOwnerID()
		{
			return base.GetUniqueLoadID();
		}

		// Token: 0x06001A5C RID: 6748 RVA: 0x0001881C File Offset: 0x00016A1C
		bool IVerbOwner.VerbsStillUsableBy(Pawn p)
		{
			return p == this;
		}

		// Token: 0x17000515 RID: 1301
		// (get) Token: 0x06001A5D RID: 6749 RVA: 0x000187F7 File Offset: 0x000169F7
		Thing IVerbOwner.ConstantCaster
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000516 RID: 1302
		// (get) Token: 0x06001A5E RID: 6750 RVA: 0x00018822 File Offset: 0x00016A22
		ImplementOwnerTypeDef IVerbOwner.ImplementOwnerTypeDef
		{
			get
			{
				return ImplementOwnerTypeDefOf.Bodypart;
			}
		}

		// Token: 0x06001A5F RID: 6751 RVA: 0x00018829 File Offset: 0x00016A29
		public int GetRootTile()
		{
			return base.Tile;
		}

		// Token: 0x06001A60 RID: 6752 RVA: 0x0000C32E File Offset: 0x0000A52E
		public ThingOwner GetDirectlyHeldThings()
		{
			return null;
		}

		// Token: 0x06001A61 RID: 6753 RVA: 0x000E51B8 File Offset: 0x000E33B8
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

		// Token: 0x06001A62 RID: 6754 RVA: 0x00018831 File Offset: 0x00016A31
		public string GetKindLabelPlural(int count = -1)
		{
			return GenLabel.BestKindLabel(this, false, false, true, count);
		}

		// Token: 0x06001A63 RID: 6755 RVA: 0x0001883D File Offset: 0x00016A3D
		public static void ResetStaticData()
		{
			Pawn.NotSurgeryReadyTrans = "NotSurgeryReady".Translate();
			Pawn.CannotReachTrans = "CannotReach".Translate();
		}

		// Token: 0x06001A64 RID: 6756 RVA: 0x000E5224 File Offset: 0x000E3424
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
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x06001A65 RID: 6757 RVA: 0x000E5640 File Offset: 0x000E3840
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

		// Token: 0x06001A66 RID: 6758 RVA: 0x000E56A8 File Offset: 0x000E38A8
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			if (this.Dead)
			{
				Log.Warning("Tried to spawn Dead Pawn " + this.ToStringSafe<Pawn>() + ". Replacing with corpse.", false);
				Corpse corpse = (Corpse)ThingMaker.MakeThing(this.RaceProps.corpseDef, null);
				corpse.InnerPawn = this;
				GenSpawn.Spawn(corpse, base.Position, map, WipeMode.Vanish);
				return;
			}
			if (this.def == null || this.kindDef == null)
			{
				Log.Warning("Tried to spawn pawn without def " + this.ToStringSafe<Pawn>() + ".", false);
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
				Log.Error("Pawn " + this.ToStringSafe<Pawn>() + " spawned in invalid state. Destroying...", false);
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
					}), false);
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
			if (!respawningAfterLoad)
			{
				this.records.AccumulateStoryEvent(StoryEventDefOf.Seen);
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

		// Token: 0x06001A67 RID: 6759 RVA: 0x00018867 File Offset: 0x00016A67
		public override void PostMapInit()
		{
			base.PostMapInit();
			this.pather.TryResumePathingAfterLoading();
		}

		// Token: 0x06001A68 RID: 6760 RVA: 0x0001887A File Offset: 0x00016A7A
		public override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			this.Drawer.DrawAt(drawLoc);
		}

		// Token: 0x06001A69 RID: 6761 RVA: 0x00018888 File Offset: 0x00016A88
		public override void DrawGUIOverlay()
		{
			this.Drawer.ui.DrawPawnGUIOverlay();
		}

		// Token: 0x06001A6A RID: 6762 RVA: 0x0001889A File Offset: 0x00016A9A
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

		// Token: 0x06001A6B RID: 6763 RVA: 0x000E5934 File Offset: 0x000E3B34
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

		// Token: 0x06001A6C RID: 6764 RVA: 0x000E59CC File Offset: 0x000E3BCC
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
				if (this.royalty != null && ModsConfig.RoyaltyActive)
				{
					this.royalty.RoyaltyTrackerTick();
				}
				this.ageTracker.AgeTick();
				this.records.RecordsTick();
			}
		}

		// Token: 0x06001A6D RID: 6765 RVA: 0x000188D3 File Offset: 0x00016AD3
		public void TickMothballed(int interval)
		{
			if (!base.Suspended)
			{
				this.ageTracker.AgeTickMothballed(interval);
				this.records.RecordsTickMothballed(interval);
			}
		}

		// Token: 0x06001A6E RID: 6766 RVA: 0x000E5BF4 File Offset: 0x000E3DF4
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

		// Token: 0x06001A6F RID: 6767 RVA: 0x000E5C48 File Offset: 0x000E3E48
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
				else if (Find.FactionManager.TryGetRandomNonColonyHumanlikeFaction_NewTemp(out faction, tryMedievalOrBetter, false, TechLevel.Undefined, false))
				{
					if (base.Faction != faction)
					{
						this.SetFaction(faction, null);
					}
				}
				else if (Find.FactionManager.TryGetRandomNonColonyHumanlikeFaction_NewTemp(out faction, tryMedievalOrBetter, true, TechLevel.Undefined, false))
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

		// Token: 0x06001A70 RID: 6768 RVA: 0x000188F5 File Offset: 0x00016AF5
		public override void PreApplyDamage(ref DamageInfo dinfo, out bool absorbed)
		{
			base.PreApplyDamage(ref dinfo, out absorbed);
			if (absorbed)
			{
				return;
			}
			this.health.PreApplyDamage(dinfo, out absorbed);
		}

		// Token: 0x06001A71 RID: 6769 RVA: 0x000E5DA4 File Offset: 0x000E3FA4
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
			this.records.AccumulateStoryEvent(StoryEventDefOf.DamageTaken);
			this.health.PostApplyDamage(dinfo, totalDamageDealt);
			if (!this.Dead)
			{
				this.mindState.Notify_DamageTaken(dinfo);
			}
		}

		// Token: 0x06001A72 RID: 6770 RVA: 0x00018916 File Offset: 0x00016B16
		public override Thing SplitOff(int count)
		{
			if (count <= 0 || count >= this.stackCount)
			{
				return base.SplitOff(count);
			}
			throw new NotImplementedException("Split off on Pawns is not supported (unless we're taking a full stack).");
		}

		// Token: 0x17000517 RID: 1303
		// (get) Token: 0x06001A73 RID: 6771 RVA: 0x00018937 File Offset: 0x00016B37
		public int TicksPerMoveCardinal
		{
			get
			{
				return this.TicksPerMove(false);
			}
		}

		// Token: 0x17000518 RID: 1304
		// (get) Token: 0x06001A74 RID: 6772 RVA: 0x00018940 File Offset: 0x00016B40
		public int TicksPerMoveDiagonal
		{
			get
			{
				return this.TicksPerMove(true);
			}
		}

		// Token: 0x06001A75 RID: 6773 RVA: 0x000E5E40 File Offset: 0x000E4040
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

		// Token: 0x06001A76 RID: 6774 RVA: 0x000E5F10 File Offset: 0x000E4110
		public override void Kill(DamageInfo? dinfo, Hediff exactCulprit = null)
		{
			IntVec3 positionHeld = base.PositionHeld;
			Map map = base.Map;
			Map mapHeld = base.MapHeld;
			bool flag = base.Spawned;
			bool spawnedOrAnyParentSpawned = base.SpawnedOrAnyParentSpawned;
			bool wasWorldPawn = this.IsWorldPawn();
			Caravan caravan = this.GetCaravan();
			Building_Grave assignedGrave = null;
			if (this.ownership != null)
			{
				assignedGrave = this.ownership.AssignedGrave;
			}
			bool flag2 = this.InBed();
			float bedRotation = 0f;
			if (flag2)
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
			bool flag3 = false;
			bool flag4 = false;
			if (Current.ProgramState == ProgramState.Playing && map != null)
			{
				flag3 = (map.designationManager.DesignationOn(this, DesignationDefOf.Hunt) != null);
				flag4 = (map.designationManager.DesignationOn(this, DesignationDefOf.Slaughter) != null);
			}
			bool flag5 = PawnUtility.ShouldSendNotificationAbout(this) && (!flag4 || dinfo == null || dinfo.Value.Def != DamageDefOf.ExecutionCut);
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
					if (this.IsColonist)
					{
						pawn.records.AccumulateStoryEvent(StoryEventDefOf.KilledPlayer);
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
			this.meleeVerbs.Notify_PawnKilled();
			for (int i = 0; i < this.health.hediffSet.hediffs.Count; i++)
			{
				this.health.hediffSet.hediffs[i].Notify_PawnKilled();
			}
			Pawn_CarryTracker pawn_CarryTracker = base.ParentHolder as Pawn_CarryTracker;
			Thing thing;
			if (pawn_CarryTracker != null && this.holdingOwner.TryDrop_NewTmp(this, pawn_CarryTracker.pawn.Position, pawn_CarryTracker.pawn.Map, ThingPlaceMode.Near, out thing, null, null, true))
			{
				map = pawn_CarryTracker.pawn.Map;
				flag = true;
			}
			PawnDiedOrDownedThoughtsUtility.RemoveLostThoughts(this);
			PawnDiedOrDownedThoughtsUtility.RemoveResuedRelativeThought(this);
			PawnDiedOrDownedThoughtsUtility.TryGiveThoughts(this, dinfo, PawnDiedOrDownedThoughtsKind.Died);
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
					corpse = this.MakeCorpse(assignedGrave, flag2, bedRotation);
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
					corpse = this.MakeCorpse(assignedGrave, flag2, bedRotation);
					if (GenPlace.TryPlaceThing(corpse, positionHeld, mapHeld, ThingPlaceMode.Direct, null, null, default(Rot4)))
					{
						corpse.Rotation = base.Rotation;
						if (HuntJobUtility.WasKilledByHunter(this, dinfo))
						{
							((Pawn)dinfo.Value.Instigator).Reserve(corpse, ((Pawn)dinfo.Value.Instigator).CurJob, 1, -1, null, true);
						}
						else if (!flag3 && !flag4)
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
					corpse = this.MakeCorpse(assignedGrave, flag2, bedRotation);
					caravan.AddPawnOrItem(corpse, true);
				}
				else if (this.holdingOwner != null || this.IsWorldPawn())
				{
					Corpse.PostCorpseDestroy(this);
				}
				else
				{
					corpse = this.MakeCorpse(assignedGrave, flag2, bedRotation);
				}
			}
			if (corpse != null)
			{
				Hediff firstHediffOfDef = this.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.ToxicBuildup, false);
				Hediff firstHediffOfDef2 = this.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Scaria, false);
				CompRottable comp = corpse.GetComp<CompRottable>();
				if ((firstHediffOfDef != null && Rand.Value < firstHediffOfDef.Severity && comp != null) || (firstHediffOfDef2 != null && Rand.Chance(Find.Storyteller.difficultyValues.scariaRotChance)))
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
			for (int j = this.health.hediffSet.hediffs.Count - 1; j >= 0; j--)
			{
				this.health.hediffSet.hediffs[j].Notify_PawnDied();
			}
			Faction factionOrExtraMiniOrHomeFaction = this.FactionOrExtraMiniOrHomeFaction;
			if (factionOrExtraMiniOrHomeFaction != null)
			{
				factionOrExtraMiniOrHomeFaction.Notify_MemberDied(this, dinfo, wasWorldPawn, mapHeld);
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
			if (flag5)
			{
				this.health.NotifyPlayerOfKilled(dinfo, exactCulprit, caravan);
			}
			Find.QuestManager.Notify_PawnKilled(this, dinfo);
			Find.FactionManager.Notify_PawnKilled(this);
		}

		// Token: 0x06001A77 RID: 6775 RVA: 0x000E6654 File Offset: 0x000E4854
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
				}), false);
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
		}

		// Token: 0x06001A78 RID: 6776 RVA: 0x000E67F0 File Offset: 0x000E49F0
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
			}
			PawnComponentsUtility.RemoveComponentsOnDespawned(this);
		}

		// Token: 0x06001A79 RID: 6777 RVA: 0x000E68A0 File Offset: 0x000E4AA0
		public override void Discard(bool silentlyRemoveReferences = false)
		{
			if (Find.WorldPawns.Contains(this))
			{
				Log.Warning("Tried to discard a world pawn " + this + ".", false);
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

		// Token: 0x06001A7A RID: 6778 RVA: 0x000E6998 File Offset: 0x000E4B98
		public Corpse MakeCorpse(Building_Grave assignedGrave, bool inBed, float bedRotation)
		{
			if (this.holdingOwner != null)
			{
				Log.Warning("We can't make corpse because the pawn is in a ThingOwner. Remove him from the container first. This should have been already handled before calling this method. holder=" + base.ParentHolder, false);
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

		// Token: 0x06001A7B RID: 6779 RVA: 0x000E6A20 File Offset: 0x000E4C20
		public void ExitMap(bool allowedToJoinOrCreateCaravan, Rot4 exitDir)
		{
			if (this.IsWorldPawn())
			{
				Log.Warning("Called ExitMap() on world pawn " + this, false);
				return;
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
			if (base.Faction != null)
			{
				base.Faction.Notify_MemberExitedMap(this, flag);
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
					this.guest.SetGuestStatus(null, false);
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
		}

		// Token: 0x06001A7C RID: 6780 RVA: 0x000E6C28 File Offset: 0x000E4E28
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
					faction.TrySetRelationKind(Faction.OfPlayer, FactionRelationKind.Hostile, true, "GoodwillChangedReason_SoldPawn".Translate(this), new GlobalTargetInfo?(this));
				}
			}
			if (this.guest != null)
			{
				this.guest.SetGuestStatus(null, false);
			}
			if (action == TradeAction.PlayerBuys)
			{
				if (this.needs.mood != null)
				{
					this.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.FreedFromSlavery, null);
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
				if (this.RaceProps.Humanlike)
				{
					GenGuest.AddPrisonerSoldThoughts(this);
				}
			}
			this.ClearMind(false, false, true);
		}

		// Token: 0x06001A7D RID: 6781 RVA: 0x000E6D78 File Offset: 0x000E4F78
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
				this.guest.SetGuestStatus(null, false);
			}
			if (this.RaceProps.IsFlesh)
			{
				this.relations.Notify_PawnKidnapped();
			}
			this.ClearMind(false, false, true);
		}

		// Token: 0x06001A7E RID: 6782 RVA: 0x000E6E08 File Offset: 0x000E5008
		public override void SetFaction(Faction newFaction, Pawn recruiter = null)
		{
			if (newFaction == base.Faction)
			{
				Log.Warning("Used SetFaction to change " + this.ToStringSafe<Pawn>() + " to same faction " + newFaction.ToStringSafe<Faction>(), false);
				return;
			}
			Faction faction = base.Faction;
			if (this.guest != null)
			{
				this.guest.SetGuestStatus(null, false);
			}
			if (base.Spawned)
			{
				base.Map.mapPawns.DeRegisterPawn(this);
				base.Map.pawnDestinationReservationManager.ReleaseAllClaimedBy(this);
				base.Map.designationManager.RemoveAllDesignationsOn(this, false);
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
		}

		// Token: 0x06001A7F RID: 6783 RVA: 0x000E70DC File Offset: 0x000E52DC
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

		// Token: 0x06001A80 RID: 6784 RVA: 0x000E712C File Offset: 0x000E532C
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

		// Token: 0x06001A81 RID: 6785 RVA: 0x000E71B0 File Offset: 0x000E53B0
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

		// Token: 0x06001A82 RID: 6786 RVA: 0x000E7220 File Offset: 0x000E5420
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
					Log.ErrorOnce(string.Format("Reservation manager failed to clean up properly; {0} still reserving {1}", this.ToStringSafe<Pawn>(), obj.ToStringSafe<LocalTargetInfo>()), 97771429 ^ this.thingIDNumber, false);
					flag = true;
				}
				LocalTargetInfo obj2 = maps[i].physicalInteractionReservationManager.FirstReservationFor(this);
				if (obj2.IsValid)
				{
					Log.ErrorOnce(string.Format("Physical interaction reservation manager failed to clean up properly; {0} still reserving {1}", this.ToStringSafe<Pawn>(), obj2.ToStringSafe<LocalTargetInfo>()), 19586765 ^ this.thingIDNumber, false);
					flag = true;
				}
				IAttackTarget attackTarget = maps[i].attackTargetReservationManager.FirstReservationFor(this);
				if (attackTarget != null)
				{
					Log.ErrorOnce(string.Format("Attack target reservation manager failed to clean up properly; {0} still reserving {1}", this.ToStringSafe<Pawn>(), attackTarget.ToStringSafe<IAttackTarget>()), 100495878 ^ this.thingIDNumber, false);
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
					}), 1958674 ^ this.thingIDNumber, false);
					flag = true;
				}
			}
			if (flag)
			{
				this.ClearAllReservations(true);
			}
		}

		// Token: 0x06001A83 RID: 6787 RVA: 0x000E73DC File Offset: 0x000E55DC
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

		// Token: 0x06001A84 RID: 6788 RVA: 0x00018949 File Offset: 0x00016B49
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

		// Token: 0x06001A85 RID: 6789 RVA: 0x000E752C File Offset: 0x000E572C
		public Verb TryGetAttackVerb(Thing target, bool allowManualCastWeapons = false)
		{
			if (this.equipment != null && this.equipment.Primary != null && this.equipment.PrimaryEq.PrimaryVerb.Available() && (!this.equipment.PrimaryEq.PrimaryVerb.verbProps.onlyManualCast || (this.CurJob != null && this.CurJob.def != JobDefOf.Wait_Combat) || allowManualCastWeapons))
			{
				return this.equipment.PrimaryEq.PrimaryVerb;
			}
			return this.meleeVerbs.TryGetMeleeVerb(target);
		}

		// Token: 0x06001A86 RID: 6790 RVA: 0x000E75C8 File Offset: 0x000E57C8
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
			return verb != null && verb.TryStartCastOn(targ, false, true);
		}

		// Token: 0x06001A87 RID: 6791 RVA: 0x0001897C File Offset: 0x00016B7C
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
				Pawn.<>c__DisplayClass204_0 CS$<>8__locals1 = new Pawn.<>c__DisplayClass204_0();
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

		// Token: 0x06001A88 RID: 6792 RVA: 0x000E7618 File Offset: 0x000E5818
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

		// Token: 0x06001A89 RID: 6793 RVA: 0x000E7844 File Offset: 0x000E5A44
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
				stringBuilder.AppendLine(string.Format("{0}: {1}", "State".Translate(), Pawn.states.ToCommaList(false).CapitalizeFirst()));
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
						Log.Error("JobDriver.GetReport() exception: " + arg, false);
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
					Log.Error("JobDriver.GetReport() exception: " + arg2, false);
				}
			}
			if (RestraintsUtility.ShouldShowRestraintsInfo(this))
			{
				stringBuilder.AppendLine("InRestraints".Translate());
			}
			return stringBuilder.ToString().TrimEndNewlines();
		}

		// Token: 0x06001A8A RID: 6794 RVA: 0x0001899A File Offset: 0x00016B9A
		public override IEnumerable<Gizmo> GetGizmos()
		{
			IEnumerator<Gizmo> enumerator;
			if (this.IsColonistPlayerControlled)
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
			if (Find.Selector.SingleSelectedThing == this && this.psychicEntropy != null && this.psychicEntropy.NeedToShowGizmo())
			{
				yield return this.psychicEntropy.GetGizmo();
			}
			if (this.IsColonistPlayerControlled)
			{
				if (this.abilities != null)
				{
					foreach (Gizmo gizmo5 in this.abilities.GetGizmos())
					{
						yield return gizmo5;
					}
					enumerator = null;
				}
				if (this.playerSettings != null)
				{
					foreach (Gizmo gizmo6 in this.playerSettings.GetGizmos())
					{
						yield return gizmo6;
					}
					enumerator = null;
				}
			}
			if (this.apparel != null)
			{
				foreach (Gizmo gizmo7 in this.apparel.GetGizmos())
				{
					yield return gizmo7;
				}
				enumerator = null;
			}
			if (this.inventory != null)
			{
				foreach (Gizmo gizmo8 in this.inventory.GetGizmos())
				{
					yield return gizmo8;
				}
				enumerator = null;
			}
			foreach (Gizmo gizmo9 in this.mindState.GetGizmos())
			{
				yield return gizmo9;
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
						foreach (Gizmo gizmo10 in pawnGizmos)
						{
							yield return gizmo10;
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
								foreach (Gizmo gizmo11 in pawnGizmos2)
								{
									yield return gizmo11;
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
			foreach (Gizmo gizmo12 in QuestUtility.GetQuestRelatedGizmos(this))
			{
				yield return gizmo12;
			}
			enumerator = null;
			if (this.royalty != null && ModsConfig.RoyaltyActive)
			{
				foreach (Gizmo gizmo13 in this.royalty.GetGizmos())
				{
					yield return gizmo13;
				}
				enumerator = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x06001A8B RID: 6795 RVA: 0x000189AA File Offset: 0x00016BAA
		public virtual IEnumerable<FloatMenuOption> GetExtraFloatMenuOptionsFor(IntVec3 sq)
		{
			yield break;
		}

		// Token: 0x06001A8C RID: 6796 RVA: 0x000E7CB0 File Offset: 0x000E5EB0
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

		// Token: 0x06001A8D RID: 6797 RVA: 0x000189B3 File Offset: 0x00016BB3
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			foreach (StatDrawEntry statDrawEntry in base.SpecialDisplayStats())
			{
				yield return statDrawEntry;
			}
			IEnumerator<StatDrawEntry> enumerator = null;
			yield return new StatDrawEntry(StatCategoryDefOf.BasicsPawn, "BodySize".Translate(), this.BodySize.ToString("F2"), "Stat_Race_BodySize_Desc".Translate(), 500, null, null, false);
			if (this.IsWildMan())
			{
				yield return new StatDrawEntry(StatCategoryDefOf.BasicsPawn, "Wildness".Translate(), 0.75f.ToStringPercent(), TrainableUtility.GetWildnessExplanation(this.def), 2050, null, null, false);
			}
			if (ModsConfig.RoyaltyActive && this.RaceProps.intelligence == Intelligence.Humanlike)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.BasicsPawn, "MeditationFocuses".Translate(), MeditationUtility.FocusTypesAvailableForPawnString(this).CapitalizeFirst(), ("MeditationFocusesPawnDesc".Translate() + "\n\n" + MeditationUtility.FocusTypeAvailableExplanation(this)).Resolve(), 99995, null, MeditationUtility.FocusObjectsForPawnHyperlinks(this), false);
			}
			yield break;
			yield break;
		}

		// Token: 0x06001A8E RID: 6798 RVA: 0x000E7E20 File Offset: 0x000E6020
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

		// Token: 0x06001A8F RID: 6799 RVA: 0x000189C3 File Offset: 0x00016BC3
		public bool UsableForBillsAfterFueling()
		{
			return this.CurrentlyUsableForBills();
		}

		// Token: 0x06001A90 RID: 6800 RVA: 0x000E7E60 File Offset: 0x000E6060
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

		// Token: 0x06001A91 RID: 6801 RVA: 0x000E7ED0 File Offset: 0x000E60D0
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

		// Token: 0x17000519 RID: 1305
		// (get) Token: 0x06001A92 RID: 6802 RVA: 0x000189CB File Offset: 0x00016BCB
		public TradeCurrency TradeCurrency
		{
			get
			{
				return this.TraderKind.tradeCurrency;
			}
		}

		// Token: 0x06001A93 RID: 6803 RVA: 0x000189D8 File Offset: 0x00016BD8
		public IEnumerable<Thing> ColonyThingsWillingToBuy(Pawn playerNegotiator)
		{
			return this.trader.ColonyThingsWillingToBuy(playerNegotiator);
		}

		// Token: 0x06001A94 RID: 6804 RVA: 0x000189E6 File Offset: 0x00016BE6
		public void GiveSoldThingToTrader(Thing toGive, int countToGive, Pawn playerNegotiator)
		{
			this.trader.GiveSoldThingToTrader(toGive, countToGive, playerNegotiator);
		}

		// Token: 0x06001A95 RID: 6805 RVA: 0x000189F6 File Offset: 0x00016BF6
		public void GiveSoldThingToPlayer(Thing toGive, int countToGive, Pawn playerNegotiator)
		{
			this.trader.GiveSoldThingToPlayer(toGive, countToGive, playerNegotiator);
		}

		// Token: 0x06001A96 RID: 6806 RVA: 0x000E7FA0 File Offset: 0x000E61A0
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
				this.mindState.canSleepTick = Find.TickManager.TicksGame + 1000;
				if (this.CurJob != null && !this.Awake())
				{
					this.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
				}
				this.NotifyLordOfClamor(source, type);
			}
		}

		// Token: 0x06001A97 RID: 6807 RVA: 0x000E816C File Offset: 0x000E636C
		private void NotifyLordOfClamor(Thing source, ClamorDef type)
		{
			Lord lord = this.GetLord();
			if (lord != null)
			{
				lord.Notify_Clamor(source, type);
			}
		}

		// Token: 0x06001A98 RID: 6808 RVA: 0x00018A06 File Offset: 0x00016C06
		public override void Notify_Explosion(Explosion explosion)
		{
			base.Notify_Explosion(explosion);
			this.mindState.Notify_Explosion(explosion);
		}

		// Token: 0x06001A99 RID: 6809 RVA: 0x00018A1B File Offset: 0x00016C1B
		public override void Notify_BulletImpactNearby(BulletImpactData impactData)
		{
			Pawn_ApparelTracker pawn_ApparelTracker = this.apparel;
			if (pawn_ApparelTracker == null)
			{
				return;
			}
			pawn_ApparelTracker.Notify_BulletImpactNearby(impactData);
		}

		// Token: 0x06001A9A RID: 6810 RVA: 0x000E818C File Offset: 0x000E638C
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
			this.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.SleepDisturbed, null);
		}

		// Token: 0x06001A9B RID: 6811 RVA: 0x000E8264 File Offset: 0x000E6464
		public float GetAcceptArrestChance(Pawn arrester)
		{
			float num = StatDefOf.ArrestSuccessChance.Worker.IsDisabledFor(arrester) ? StatDefOf.ArrestSuccessChance.valueIfMissing : arrester.GetStatValue(StatDefOf.ArrestSuccessChance, true);
			if (this.IsWildMan())
			{
				return num * 0.5f;
			}
			return num;
		}

		// Token: 0x06001A9C RID: 6812 RVA: 0x000E82B0 File Offset: 0x000E64B0
		public bool CheckAcceptArrest(Pawn arrester)
		{
			Faction factionOrExtraMiniOrHomeFaction = this.FactionOrExtraMiniOrHomeFaction;
			if (factionOrExtraMiniOrHomeFaction != null && factionOrExtraMiniOrHomeFaction != arrester.factionInt)
			{
				factionOrExtraMiniOrHomeFaction.Notify_MemberCaptured(this, arrester.Faction);
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
				this.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Berserk, null, false, false, null, false);
			}
			return false;
		}

		// Token: 0x06001A9D RID: 6813 RVA: 0x000E8368 File Offset: 0x000E6568
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
			if (this.Downed)
			{
				if (disabledFor == null)
				{
					return true;
				}
				Pawn pawn = disabledFor.Thing as Pawn;
				if (pawn == null || pawn.mindState == null || pawn.mindState.duty == null || !pawn.mindState.duty.attackDownedIfStarving || !pawn.Starving())
				{
					return true;
				}
			}
			return this.IsInvisible();
		}

		// Token: 0x06001A9E RID: 6814 RVA: 0x000E8434 File Offset: 0x000E6634
		public List<WorkTypeDef> GetDisabledWorkTypes(bool permanentOnly = false)
		{
			Pawn.<>c__DisplayClass234_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.permanentOnly = permanentOnly;
			if (CS$<>8__locals1.permanentOnly)
			{
				if (this.cachedDisabledWorkTypesPermanent == null)
				{
					this.cachedDisabledWorkTypesPermanent = new List<WorkTypeDef>();
				}
				this.<GetDisabledWorkTypes>g__FillList|234_0(this.cachedDisabledWorkTypesPermanent, ref CS$<>8__locals1);
				return this.cachedDisabledWorkTypesPermanent;
			}
			if (this.cachedDisabledWorkTypes == null)
			{
				this.cachedDisabledWorkTypes = new List<WorkTypeDef>();
			}
			this.<GetDisabledWorkTypes>g__FillList|234_0(this.cachedDisabledWorkTypes, ref CS$<>8__locals1);
			return this.cachedDisabledWorkTypes;
		}

		// Token: 0x06001A9F RID: 6815 RVA: 0x00018A2E File Offset: 0x00016C2E
		public bool WorkTypeIsDisabled(WorkTypeDef w)
		{
			return this.GetDisabledWorkTypes(false).Contains(w);
		}

		// Token: 0x06001AA0 RID: 6816 RVA: 0x000E84A8 File Offset: 0x000E66A8
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

		// Token: 0x06001AA1 RID: 6817 RVA: 0x00018A3D File Offset: 0x00016C3D
		public void Notify_DisabledWorkTypesChanged()
		{
			this.cachedDisabledWorkTypes = null;
			this.cachedDisabledWorkTypesPermanent = null;
			Pawn_WorkSettings pawn_WorkSettings = this.workSettings;
			if (pawn_WorkSettings == null)
			{
				return;
			}
			pawn_WorkSettings.Notify_DisabledWorkTypesChanged();
		}

		// Token: 0x1700051A RID: 1306
		// (get) Token: 0x06001AA2 RID: 6818 RVA: 0x000E84D8 File Offset: 0x000E66D8
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

		// Token: 0x06001AA3 RID: 6819 RVA: 0x00018A5D File Offset: 0x00016C5D
		public bool WorkTagIsDisabled(WorkTags w)
		{
			return (this.CombinedDisabledWorkTags & w) > WorkTags.None;
		}

		// Token: 0x06001AA4 RID: 6820 RVA: 0x000E8614 File Offset: 0x000E6814
		public override bool PreventPlayerSellingThingsNearby(out string reason)
		{
			if (this.InAggroMentalState || (base.Faction.HostileTo(Faction.OfPlayer) && this.HostFaction == null && !this.Downed && !this.InMentalState))
			{
				reason = "Enemies".Translate();
				return true;
			}
			reason = null;
			return false;
		}

		// Token: 0x06001AA5 RID: 6821 RVA: 0x00018A6A File Offset: 0x00016C6A
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

		// Token: 0x1700051B RID: 1307
		// (get) Token: 0x06001AA6 RID: 6822 RVA: 0x00018A9C File Offset: 0x00016C9C
		public bool HasPsylink
		{
			get
			{
				return this.psychicEntropy != null && this.psychicEntropy.Psylink != null;
			}
		}

		// Token: 0x06001AAE RID: 6830 RVA: 0x000E86C4 File Offset: 0x000E68C4
		[CompilerGenerated]
		private void <GetDisabledWorkTypes>g__FillList|234_0(List<WorkTypeDef> list, ref Pawn.<>c__DisplayClass234_0 A_2)
		{
			if (this.story != null)
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
			if (this.royalty != null && !A_2.permanentOnly)
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
			foreach (QuestPart_WorkDisabled questPart_WorkDisabled in QuestUtility.GetWorkDisabledQuestPart(this))
			{
				foreach (WorkTypeDef item4 in questPart_WorkDisabled.DisabledWorkTypes)
				{
					if (!list.Contains(item4))
					{
						list.Add(item4);
					}
				}
			}
		}

		// Token: 0x04001357 RID: 4951
		public PawnKindDef kindDef;

		// Token: 0x04001358 RID: 4952
		private Name nameInt;

		// Token: 0x04001359 RID: 4953
		public Gender gender;

		// Token: 0x0400135A RID: 4954
		public Pawn_AgeTracker ageTracker;

		// Token: 0x0400135B RID: 4955
		public Pawn_HealthTracker health;

		// Token: 0x0400135C RID: 4956
		public Pawn_RecordsTracker records;

		// Token: 0x0400135D RID: 4957
		public Pawn_InventoryTracker inventory;

		// Token: 0x0400135E RID: 4958
		public Pawn_MeleeVerbs meleeVerbs;

		// Token: 0x0400135F RID: 4959
		public VerbTracker verbTracker;

		// Token: 0x04001360 RID: 4960
		public Pawn_CarryTracker carryTracker;

		// Token: 0x04001361 RID: 4961
		public Pawn_NeedsTracker needs;

		// Token: 0x04001362 RID: 4962
		public Pawn_MindState mindState;

		// Token: 0x04001363 RID: 4963
		public Pawn_RotationTracker rotationTracker;

		// Token: 0x04001364 RID: 4964
		public Pawn_PathFollower pather;

		// Token: 0x04001365 RID: 4965
		public Pawn_Thinker thinker;

		// Token: 0x04001366 RID: 4966
		public Pawn_JobTracker jobs;

		// Token: 0x04001367 RID: 4967
		public Pawn_StanceTracker stances;

		// Token: 0x04001368 RID: 4968
		public Pawn_NativeVerbs natives;

		// Token: 0x04001369 RID: 4969
		public Pawn_FilthTracker filth;

		// Token: 0x0400136A RID: 4970
		public Pawn_EquipmentTracker equipment;

		// Token: 0x0400136B RID: 4971
		public Pawn_ApparelTracker apparel;

		// Token: 0x0400136C RID: 4972
		public Pawn_Ownership ownership;

		// Token: 0x0400136D RID: 4973
		public Pawn_SkillTracker skills;

		// Token: 0x0400136E RID: 4974
		public Pawn_StoryTracker story;

		// Token: 0x0400136F RID: 4975
		public Pawn_GuestTracker guest;

		// Token: 0x04001370 RID: 4976
		public Pawn_GuiltTracker guilt;

		// Token: 0x04001371 RID: 4977
		public Pawn_RoyaltyTracker royalty;

		// Token: 0x04001372 RID: 4978
		public Pawn_AbilityTracker abilities;

		// Token: 0x04001373 RID: 4979
		public Pawn_WorkSettings workSettings;

		// Token: 0x04001374 RID: 4980
		public Pawn_TraderTracker trader;

		// Token: 0x04001375 RID: 4981
		public Pawn_TrainingTracker training;

		// Token: 0x04001376 RID: 4982
		public Pawn_CallTracker caller;

		// Token: 0x04001377 RID: 4983
		public Pawn_RelationsTracker relations;

		// Token: 0x04001378 RID: 4984
		public Pawn_PsychicEntropyTracker psychicEntropy;

		// Token: 0x04001379 RID: 4985
		public Pawn_InteractionsTracker interactions;

		// Token: 0x0400137A RID: 4986
		public Pawn_PlayerSettings playerSettings;

		// Token: 0x0400137B RID: 4987
		public Pawn_OutfitTracker outfits;

		// Token: 0x0400137C RID: 4988
		public Pawn_DrugPolicyTracker drugs;

		// Token: 0x0400137D RID: 4989
		public Pawn_FoodRestrictionTracker foodRestriction;

		// Token: 0x0400137E RID: 4990
		public Pawn_TimetableTracker timetable;

		// Token: 0x0400137F RID: 4991
		public Pawn_DraftController drafter;

		// Token: 0x04001380 RID: 4992
		private Pawn_DrawTracker drawer;

		// Token: 0x04001381 RID: 4993
		public int becameWorldPawnTickAbs = -1;

		// Token: 0x04001382 RID: 4994
		public bool teleporting;

		// Token: 0x04001383 RID: 4995
		private const float HumanSizedHeatOutput = 0.3f;

		// Token: 0x04001384 RID: 4996
		private const float AnimalHeatOutputFactor = 0.6f;

		// Token: 0x04001385 RID: 4997
		private static string NotSurgeryReadyTrans;

		// Token: 0x04001386 RID: 4998
		private static string CannotReachTrans;

		// Token: 0x04001387 RID: 4999
		public const int MaxMoveTicks = 450;

		// Token: 0x04001388 RID: 5000
		private static List<ExtraFaction> tmpExtraFactions = new List<ExtraFaction>();

		// Token: 0x04001389 RID: 5001
		private static List<string> states = new List<string>();

		// Token: 0x0400138A RID: 5002
		private int lastSleepDisturbedTick;

		// Token: 0x0400138B RID: 5003
		private const int SleepDisturbanceMinInterval = 300;

		// Token: 0x0400138C RID: 5004
		private List<WorkTypeDef> cachedDisabledWorkTypes;

		// Token: 0x0400138D RID: 5005
		private List<WorkTypeDef> cachedDisabledWorkTypesPermanent;
	}
}
