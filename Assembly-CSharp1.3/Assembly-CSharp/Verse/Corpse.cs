using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200033D RID: 829
	public class Corpse : ThingWithComps, IThingHolder, IStrippable, IBillGiver, IObservedThoughtGiver
	{
		// Token: 0x170004C1 RID: 1217
		// (get) Token: 0x06001781 RID: 6017 RVA: 0x0008CA32 File Offset: 0x0008AC32
		// (set) Token: 0x06001782 RID: 6018 RVA: 0x0008CA50 File Offset: 0x0008AC50
		public Pawn InnerPawn
		{
			get
			{
				if (this.innerContainer.Count > 0)
				{
					return this.innerContainer[0];
				}
				return null;
			}
			set
			{
				if (value == null)
				{
					this.innerContainer.Clear();
					return;
				}
				if (this.innerContainer.Count > 0)
				{
					Log.Error("Setting InnerPawn in corpse that already has one.");
					this.innerContainer.Clear();
				}
				this.innerContainer.TryAdd(value, true);
			}
		}

		// Token: 0x170004C2 RID: 1218
		// (get) Token: 0x06001783 RID: 6019 RVA: 0x0008CA9D File Offset: 0x0008AC9D
		// (set) Token: 0x06001784 RID: 6020 RVA: 0x0008CAB0 File Offset: 0x0008ACB0
		public int Age
		{
			get
			{
				return Find.TickManager.TicksGame - this.timeOfDeath;
			}
			set
			{
				this.timeOfDeath = Find.TickManager.TicksGame - value;
			}
		}

		// Token: 0x170004C3 RID: 1219
		// (get) Token: 0x06001785 RID: 6021 RVA: 0x0008CAC4 File Offset: 0x0008ACC4
		public override string LabelNoCount
		{
			get
			{
				if (this.Bugged)
				{
					Log.ErrorOnce("Corpse.Label while Bugged", 57361644);
					return "";
				}
				return "DeadLabel".Translate(this.InnerPawn.Label, this.InnerPawn);
			}
		}

		// Token: 0x170004C4 RID: 1220
		// (get) Token: 0x06001786 RID: 6022 RVA: 0x0008CB18 File Offset: 0x0008AD18
		public override bool IngestibleNow
		{
			get
			{
				if (this.Bugged)
				{
					Log.Error("IngestibleNow on Corpse while Bugged.");
					return false;
				}
				return base.IngestibleNow && this.InnerPawn.RaceProps.IsFlesh && this.GetRotStage() == RotStage.Fresh;
			}
		}

		// Token: 0x170004C5 RID: 1221
		// (get) Token: 0x06001787 RID: 6023 RVA: 0x0008CB58 File Offset: 0x0008AD58
		public RotDrawMode CurRotDrawMode
		{
			get
			{
				CompRottable comp = base.GetComp<CompRottable>();
				if (comp != null)
				{
					if (comp.Stage == RotStage.Rotting)
					{
						return RotDrawMode.Rotting;
					}
					if (comp.Stage == RotStage.Dessicated)
					{
						return RotDrawMode.Dessicated;
					}
				}
				return RotDrawMode.Fresh;
			}
		}

		// Token: 0x170004C6 RID: 1222
		// (get) Token: 0x06001788 RID: 6024 RVA: 0x0008CB88 File Offset: 0x0008AD88
		private bool ShouldVanish
		{
			get
			{
				return this.InnerPawn.RaceProps.Animal && this.vanishAfterTimestamp > 0 && this.Age >= this.vanishAfterTimestamp && base.Spawned && this.GetRoom(RegionType.Set_All) != null && this.GetRoom(RegionType.Set_All).TouchesMapEdge && !base.Map.roofGrid.Roofed(base.Position);
			}
		}

		// Token: 0x170004C7 RID: 1223
		// (get) Token: 0x06001789 RID: 6025 RVA: 0x0008CBFA File Offset: 0x0008ADFA
		public BillStack BillStack
		{
			get
			{
				return this.operationsBillStack;
			}
		}

		// Token: 0x170004C8 RID: 1224
		// (get) Token: 0x0600178A RID: 6026 RVA: 0x0008CC02 File Offset: 0x0008AE02
		public IEnumerable<IntVec3> IngredientStackCells
		{
			get
			{
				yield return this.InteractionCell;
				yield break;
			}
		}

		// Token: 0x170004C9 RID: 1225
		// (get) Token: 0x0600178B RID: 6027 RVA: 0x0008CC14 File Offset: 0x0008AE14
		public bool Bugged
		{
			get
			{
				return this.innerContainer.Count == 0 || this.innerContainer[0] == null || this.innerContainer[0].def == null || this.innerContainer[0].kindDef == null;
			}
		}

		// Token: 0x0600178C RID: 6028 RVA: 0x0008CC65 File Offset: 0x0008AE65
		public Corpse()
		{
			this.operationsBillStack = new BillStack(this);
			this.innerContainer = new ThingOwner<Pawn>(this, true, LookMode.Reference);
		}

		// Token: 0x0600178D RID: 6029 RVA: 0x0008CC98 File Offset: 0x0008AE98
		public bool CurrentlyUsableForBills()
		{
			return this.InteractionCell.IsValid;
		}

		// Token: 0x0600178E RID: 6030 RVA: 0x0008CCB3 File Offset: 0x0008AEB3
		public bool UsableForBillsAfterFueling()
		{
			return this.CurrentlyUsableForBills();
		}

		// Token: 0x0600178F RID: 6031 RVA: 0x0008CCBB File Offset: 0x0008AEBB
		public bool AnythingToStrip()
		{
			return this.InnerPawn.AnythingToStrip();
		}

		// Token: 0x06001790 RID: 6032 RVA: 0x0008CCC8 File Offset: 0x0008AEC8
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.innerContainer;
		}

		// Token: 0x06001791 RID: 6033 RVA: 0x0008CCD0 File Offset: 0x0008AED0
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x06001792 RID: 6034 RVA: 0x0008CCDE File Offset: 0x0008AEDE
		public override void PostMake()
		{
			base.PostMake();
			this.timeOfDeath = Find.TickManager.TicksGame;
		}

		// Token: 0x06001793 RID: 6035 RVA: 0x0008CCF6 File Offset: 0x0008AEF6
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			if (this.Bugged)
			{
				Log.Error(this + " spawned in bugged state.");
				return;
			}
			base.SpawnSetup(map, respawningAfterLoad);
			this.InnerPawn.Rotation = Rot4.South;
			this.NotifyColonistBar();
		}

		// Token: 0x06001794 RID: 6036 RVA: 0x0008CD2F File Offset: 0x0008AF2F
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			base.DeSpawn(mode);
			if (!this.Bugged)
			{
				this.NotifyColonistBar();
			}
		}

		// Token: 0x06001795 RID: 6037 RVA: 0x0008CD48 File Offset: 0x0008AF48
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			Pawn pawn = null;
			if (!this.Bugged)
			{
				pawn = this.InnerPawn;
				this.NotifyColonistBar();
				this.innerContainer.Clear();
			}
			base.Destroy(mode);
			if (pawn != null)
			{
				Corpse.PostCorpseDestroy(pawn);
			}
		}

		// Token: 0x06001796 RID: 6038 RVA: 0x0008CD88 File Offset: 0x0008AF88
		public static void PostCorpseDestroy(Pawn pawn)
		{
			if (pawn.ownership != null)
			{
				pawn.ownership.UnclaimAll();
			}
			if (pawn.equipment != null)
			{
				pawn.equipment.DestroyAllEquipment(DestroyMode.Vanish);
			}
			pawn.inventory.DestroyAll(DestroyMode.Vanish);
			if (pawn.apparel != null)
			{
				pawn.apparel.DestroyAll(DestroyMode.Vanish);
			}
			Ideo ideo = pawn.Ideo;
			if (ideo == null)
			{
				return;
			}
			ideo.Notify_MemberCorpseDestroyed(pawn);
		}

		// Token: 0x06001797 RID: 6039 RVA: 0x0008CDF0 File Offset: 0x0008AFF0
		public override void TickRare()
		{
			base.TickRare();
			if (base.Destroyed)
			{
				return;
			}
			if (this.Bugged)
			{
				Log.Error(this + " has null innerPawn. Destroying.");
				this.Destroy(DestroyMode.Vanish);
				return;
			}
			this.InnerPawn.TickRare();
			if (this.vanishAfterTimestamp < 0 || this.GetRotStage() != RotStage.Dessicated)
			{
				this.vanishAfterTimestamp = this.Age + 6000000;
			}
			if (this.ShouldVanish)
			{
				this.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x06001798 RID: 6040 RVA: 0x0008CE6C File Offset: 0x0008B06C
		protected override void IngestedCalculateAmounts(Pawn ingester, float nutritionWanted, out int numTaken, out float nutritionIngested)
		{
			BodyPartRecord bodyPartRecord = this.GetBestBodyPartToEat(ingester, nutritionWanted);
			if (bodyPartRecord == null)
			{
				Log.Error(string.Concat(new object[]
				{
					ingester,
					" ate ",
					this,
					" but no body part was found. Replacing with core part."
				}));
				bodyPartRecord = this.InnerPawn.RaceProps.body.corePart;
			}
			float bodyPartNutrition = FoodUtility.GetBodyPartNutrition(this, bodyPartRecord);
			if (bodyPartRecord == this.InnerPawn.RaceProps.body.corePart)
			{
				if (PawnUtility.ShouldSendNotificationAbout(this.InnerPawn) && this.InnerPawn.RaceProps.Humanlike)
				{
					Messages.Message("MessageEatenByPredator".Translate(this.InnerPawn.LabelShort, ingester.Named("PREDATOR"), this.InnerPawn.Named("EATEN")).CapitalizeFirst(), ingester, MessageTypeDefOf.NegativeEvent, true);
				}
				numTaken = 1;
			}
			else
			{
				Hediff_MissingPart hediff_MissingPart = (Hediff_MissingPart)HediffMaker.MakeHediff(HediffDefOf.MissingBodyPart, this.InnerPawn, bodyPartRecord);
				hediff_MissingPart.lastInjury = HediffDefOf.Bite;
				hediff_MissingPart.IsFresh = true;
				this.InnerPawn.health.AddHediff(hediff_MissingPart, null, null, null);
				numTaken = 0;
			}
			nutritionIngested = bodyPartNutrition;
		}

		// Token: 0x06001799 RID: 6041 RVA: 0x0008CFA7 File Offset: 0x0008B1A7
		public override IEnumerable<Thing> ButcherProducts(Pawn butcher, float efficiency)
		{
			foreach (Thing thing in this.InnerPawn.ButcherProducts(butcher, efficiency))
			{
				yield return thing;
			}
			IEnumerator<Thing> enumerator = null;
			if (this.InnerPawn.RaceProps.BloodDef != null)
			{
				FilthMaker.TryMakeFilth(butcher.Position, butcher.Map, this.InnerPawn.RaceProps.BloodDef, this.InnerPawn.LabelIndefinite(), 1, FilthSourceFlags.None);
			}
			if (this.InnerPawn.RaceProps.Humanlike)
			{
				Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.ButcheredHuman, new SignalArgs(butcher.Named(HistoryEventArgsNames.Doer), this.InnerPawn.Named(HistoryEventArgsNames.Victim))), true);
				TaleRecorder.RecordTale(TaleDefOf.ButcheredHumanlikeCorpse, new object[]
				{
					butcher
				});
			}
			yield break;
			yield break;
		}

		// Token: 0x0600179A RID: 6042 RVA: 0x0008CFC8 File Offset: 0x0008B1C8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.timeOfDeath, "timeOfDeath", 0, false);
			Scribe_Values.Look<int>(ref this.vanishAfterTimestamp, "vanishAfterTimestamp", 0, false);
			Scribe_Values.Look<bool>(ref this.everBuriedInSarcophagus, "everBuriedInSarcophagus", false, false);
			Scribe_Deep.Look<BillStack>(ref this.operationsBillStack, "operationsBillStack", new object[]
			{
				this
			});
			Scribe_Deep.Look<ThingOwner<Pawn>>(ref this.innerContainer, "innerContainer", new object[]
			{
				this
			});
		}

		// Token: 0x0600179B RID: 6043 RVA: 0x0008D045 File Offset: 0x0008B245
		public void Strip()
		{
			this.InnerPawn.Strip();
		}

		// Token: 0x0600179C RID: 6044 RVA: 0x0008D054 File Offset: 0x0008B254
		public override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			this.InnerPawn.Drawer.renderer.RenderPawnAt(drawLoc, null, false);
		}

		// Token: 0x0600179D RID: 6045 RVA: 0x00002688 File Offset: 0x00000888
		public Thought_Memory GiveObservedThought(Pawn observer)
		{
			return null;
		}

		// Token: 0x0600179E RID: 6046 RVA: 0x0008D084 File Offset: 0x0008B284
		public HistoryEventDef GiveObservedHistoryEvent(Pawn observer)
		{
			if (!this.InnerPawn.RaceProps.Humanlike)
			{
				return null;
			}
			if (this.InnerPawn.health.killedByRitual && Find.TickManager.TicksGame - this.timeOfDeath < 60000)
			{
				return null;
			}
			if (this.StoringThing() != null)
			{
				return null;
			}
			if (this.IsNotFresh())
			{
				return HistoryEventDefOf.ObservedLayingRottingCorpse;
			}
			return HistoryEventDefOf.ObservedLayingCorpse;
		}

		// Token: 0x0600179F RID: 6047 RVA: 0x0008D0F0 File Offset: 0x0008B2F0
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this.InnerPawn.Faction != null)
			{
				stringBuilder.AppendLineTagged("Faction".Translate() + ": " + this.InnerPawn.Faction.NameColored);
			}
			stringBuilder.AppendLine("DeadTime".Translate(this.Age.ToStringTicksToPeriodVague(true, false)));
			float num = 1f - this.InnerPawn.health.hediffSet.GetCoverageOfNotMissingNaturalParts(this.InnerPawn.RaceProps.body.corePart);
			if (num != 0f)
			{
				stringBuilder.AppendLine("CorpsePercentMissing".Translate() + ": " + num.ToStringPercent());
			}
			stringBuilder.AppendLine(base.GetInspectString());
			return stringBuilder.ToString().TrimEndNewlines();
		}

		// Token: 0x060017A0 RID: 6048 RVA: 0x0008D1E3 File Offset: 0x0008B3E3
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			foreach (StatDrawEntry statDrawEntry in base.SpecialDisplayStats())
			{
				yield return statDrawEntry;
			}
			IEnumerator<StatDrawEntry> enumerator = null;
			if (this.GetRotStage() == RotStage.Fresh)
			{
				StatDef meatAmount = StatDefOf.MeatAmount;
				yield return new StatDrawEntry(meatAmount.category, meatAmount, this.InnerPawn.GetStatValue(meatAmount, true), StatRequest.For(this.InnerPawn), ToStringNumberSense.Undefined, null, false);
				StatDef leatherAmount = StatDefOf.LeatherAmount;
				yield return new StatDrawEntry(leatherAmount.category, leatherAmount, this.InnerPawn.GetStatValue(leatherAmount, true), StatRequest.For(this.InnerPawn), ToStringNumberSense.Undefined, null, false);
			}
			yield break;
			yield break;
		}

		// Token: 0x060017A1 RID: 6049 RVA: 0x0008D1F3 File Offset: 0x0008B3F3
		public void RotStageChanged()
		{
			PortraitsCache.SetDirty(this.InnerPawn);
			GlobalTextureAtlasManager.TryMarkPawnFrameSetDirty(this.InnerPawn);
			this.NotifyColonistBar();
		}

		// Token: 0x060017A2 RID: 6050 RVA: 0x0008D214 File Offset: 0x0008B414
		private BodyPartRecord GetBestBodyPartToEat(Pawn ingester, float nutritionWanted)
		{
			IEnumerable<BodyPartRecord> source = from x in this.InnerPawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null)
			where x.depth == BodyPartDepth.Outside && FoodUtility.GetBodyPartNutrition(this, x) > 0.001f
			select x;
			if (!source.Any<BodyPartRecord>())
			{
				return null;
			}
			return source.MinBy((BodyPartRecord x) => Mathf.Abs(FoodUtility.GetBodyPartNutrition(this, x) - nutritionWanted));
		}

		// Token: 0x060017A3 RID: 6051 RVA: 0x0008D27C File Offset: 0x0008B47C
		private void NotifyColonistBar()
		{
			if (this.InnerPawn.Faction == Faction.OfPlayer && Current.ProgramState == ProgramState.Playing)
			{
				Find.ColonistBar.MarkColonistsDirty();
			}
		}

		// Token: 0x04001033 RID: 4147
		private ThingOwner<Pawn> innerContainer;

		// Token: 0x04001034 RID: 4148
		public int timeOfDeath = -1;

		// Token: 0x04001035 RID: 4149
		private int vanishAfterTimestamp = -1;

		// Token: 0x04001036 RID: 4150
		private BillStack operationsBillStack;

		// Token: 0x04001037 RID: 4151
		public bool everBuriedInSarcophagus;

		// Token: 0x04001038 RID: 4152
		private const int VanishAfterTicksSinceDessicated = 6000000;

		// Token: 0x04001039 RID: 4153
		private const int DontCauseObservedCorpseThoughtAfterRitualExecutionTicks = 60000;
	}
}
