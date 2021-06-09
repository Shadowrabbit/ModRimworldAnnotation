using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004CA RID: 1226
	public class Corpse : ThingWithComps, IThingHolder, IThoughtGiver, IStrippable, IBillGiver
	{
		// Token: 0x1700059B RID: 1435
		// (get) Token: 0x06001E67 RID: 7783 RVA: 0x0001AF35 File Offset: 0x00019135
		// (set) Token: 0x06001E68 RID: 7784 RVA: 0x000FC928 File Offset: 0x000FAB28
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
					Log.Error("Setting InnerPawn in corpse that already has one.", false);
					this.innerContainer.Clear();
				}
				this.innerContainer.TryAdd(value, true);
			}
		}

		// Token: 0x1700059C RID: 1436
		// (get) Token: 0x06001E69 RID: 7785 RVA: 0x0001AF53 File Offset: 0x00019153
		// (set) Token: 0x06001E6A RID: 7786 RVA: 0x0001AF66 File Offset: 0x00019166
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

		// Token: 0x1700059D RID: 1437
		// (get) Token: 0x06001E6B RID: 7787 RVA: 0x000FC978 File Offset: 0x000FAB78
		public override string LabelNoCount
		{
			get
			{
				if (this.Bugged)
				{
					Log.ErrorOnce("Corpse.Label while Bugged", 57361644, false);
					return "";
				}
				return "DeadLabel".Translate(this.InnerPawn.Label, this.InnerPawn);
			}
		}

		// Token: 0x1700059E RID: 1438
		// (get) Token: 0x06001E6C RID: 7788 RVA: 0x0001AF7A File Offset: 0x0001917A
		public override bool IngestibleNow
		{
			get
			{
				if (this.Bugged)
				{
					Log.Error("IngestibleNow on Corpse while Bugged.", false);
					return false;
				}
				return base.IngestibleNow && this.InnerPawn.RaceProps.IsFlesh && this.GetRotStage() == RotStage.Fresh;
			}
		}

		// Token: 0x1700059F RID: 1439
		// (get) Token: 0x06001E6D RID: 7789 RVA: 0x000FC9D0 File Offset: 0x000FABD0
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

		// Token: 0x170005A0 RID: 1440
		// (get) Token: 0x06001E6E RID: 7790 RVA: 0x000FCA00 File Offset: 0x000FAC00
		private bool ShouldVanish
		{
			get
			{
				return this.InnerPawn.RaceProps.Animal && this.vanishAfterTimestamp > 0 && this.Age >= this.vanishAfterTimestamp && base.Spawned && this.GetRoom(RegionType.Set_Passable) != null && this.GetRoom(RegionType.Set_Passable).TouchesMapEdge && !base.Map.roofGrid.Roofed(base.Position);
			}
		}

		// Token: 0x170005A1 RID: 1441
		// (get) Token: 0x06001E6F RID: 7791 RVA: 0x0001AFBA File Offset: 0x000191BA
		public BillStack BillStack
		{
			get
			{
				return this.operationsBillStack;
			}
		}

		// Token: 0x170005A2 RID: 1442
		// (get) Token: 0x06001E70 RID: 7792 RVA: 0x0001AFC2 File Offset: 0x000191C2
		public IEnumerable<IntVec3> IngredientStackCells
		{
			get
			{
				yield return this.InteractionCell;
				yield break;
			}
		}

		// Token: 0x170005A3 RID: 1443
		// (get) Token: 0x06001E71 RID: 7793 RVA: 0x000FCA70 File Offset: 0x000FAC70
		public bool Bugged
		{
			get
			{
				return this.innerContainer.Count == 0 || this.innerContainer[0] == null || this.innerContainer[0].def == null || this.innerContainer[0].kindDef == null;
			}
		}

		// Token: 0x06001E72 RID: 7794 RVA: 0x0001AFD2 File Offset: 0x000191D2
		public Corpse()
		{
			this.operationsBillStack = new BillStack(this);
			this.innerContainer = new ThingOwner<Pawn>(this, true, LookMode.Reference);
		}

		// Token: 0x06001E73 RID: 7795 RVA: 0x000FCAC4 File Offset: 0x000FACC4
		public bool CurrentlyUsableForBills()
		{
			return this.InteractionCell.IsValid;
		}

		// Token: 0x06001E74 RID: 7796 RVA: 0x0001B002 File Offset: 0x00019202
		public bool UsableForBillsAfterFueling()
		{
			return this.CurrentlyUsableForBills();
		}

		// Token: 0x06001E75 RID: 7797 RVA: 0x0001B00A File Offset: 0x0001920A
		public bool AnythingToStrip()
		{
			return this.InnerPawn.AnythingToStrip();
		}

		// Token: 0x06001E76 RID: 7798 RVA: 0x0001B017 File Offset: 0x00019217
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.innerContainer;
		}

		// Token: 0x06001E77 RID: 7799 RVA: 0x0001B01F File Offset: 0x0001921F
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x06001E78 RID: 7800 RVA: 0x0001B02D File Offset: 0x0001922D
		public override void PostMake()
		{
			base.PostMake();
			this.timeOfDeath = Find.TickManager.TicksGame;
		}

		// Token: 0x06001E79 RID: 7801 RVA: 0x0001B045 File Offset: 0x00019245
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			if (this.Bugged)
			{
				Log.Error(this + " spawned in bugged state.", false);
				return;
			}
			base.SpawnSetup(map, respawningAfterLoad);
			this.InnerPawn.Rotation = Rot4.South;
			this.NotifyColonistBar();
		}

		// Token: 0x06001E7A RID: 7802 RVA: 0x0001B07F File Offset: 0x0001927F
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			base.DeSpawn(mode);
			if (!this.Bugged)
			{
				this.NotifyColonistBar();
			}
		}

		// Token: 0x06001E7B RID: 7803 RVA: 0x000FCAE0 File Offset: 0x000FACE0
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

		// Token: 0x06001E7C RID: 7804 RVA: 0x000FCB20 File Offset: 0x000FAD20
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
		}

		// Token: 0x06001E7D RID: 7805 RVA: 0x000FCB74 File Offset: 0x000FAD74
		public override void TickRare()
		{
			base.TickRare();
			if (base.Destroyed)
			{
				return;
			}
			if (this.Bugged)
			{
				Log.Error(this + " has null innerPawn. Destroying.", false);
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

		// Token: 0x06001E7E RID: 7806 RVA: 0x000FCBF0 File Offset: 0x000FADF0
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
				}), false);
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

		// Token: 0x06001E7F RID: 7807 RVA: 0x0001B096 File Offset: 0x00019296
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
				if (butcher.needs.mood != null)
				{
					butcher.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.ButcheredHumanlikeCorpse, null);
				}
				foreach (Pawn pawn in butcher.Map.mapPawns.SpawnedPawnsInFaction(butcher.Faction))
				{
					if (pawn != butcher && pawn.needs != null && pawn.needs.mood != null && pawn.needs.mood.thoughts != null)
					{
						pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.KnowButcheredHumanlikeCorpse, null);
					}
				}
				TaleRecorder.RecordTale(TaleDefOf.ButcheredHumanlikeCorpse, new object[]
				{
					butcher
				});
			}
			yield break;
			yield break;
		}

		// Token: 0x06001E80 RID: 7808 RVA: 0x000FCD2C File Offset: 0x000FAF2C
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

		// Token: 0x06001E81 RID: 7809 RVA: 0x0001B0B4 File Offset: 0x000192B4
		public void Strip()
		{
			this.InnerPawn.Strip();
		}

		// Token: 0x06001E82 RID: 7810 RVA: 0x0001B0C1 File Offset: 0x000192C1
		public override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			this.InnerPawn.Drawer.renderer.RenderPawnAt(drawLoc);
		}

		// Token: 0x06001E83 RID: 7811 RVA: 0x000FCDAC File Offset: 0x000FAFAC
		public Thought_Memory GiveObservedThought()
		{
			if (!this.InnerPawn.RaceProps.Humanlike)
			{
				return null;
			}
			if (this.StoringThing() == null)
			{
				Thought_MemoryObservation thought_MemoryObservation;
				if (this.IsNotFresh())
				{
					thought_MemoryObservation = (Thought_MemoryObservation)ThoughtMaker.MakeThought(ThoughtDefOf.ObservedLayingRottingCorpse);
				}
				else
				{
					thought_MemoryObservation = (Thought_MemoryObservation)ThoughtMaker.MakeThought(ThoughtDefOf.ObservedLayingCorpse);
				}
				thought_MemoryObservation.Target = this;
				return thought_MemoryObservation;
			}
			return null;
		}

		// Token: 0x06001E84 RID: 7812 RVA: 0x000FCE0C File Offset: 0x000FB00C
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

		// Token: 0x06001E85 RID: 7813 RVA: 0x0001B0D9 File Offset: 0x000192D9
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

		// Token: 0x06001E86 RID: 7814 RVA: 0x0001B0E9 File Offset: 0x000192E9
		public void RotStageChanged()
		{
			PortraitsCache.SetDirty(this.InnerPawn);
			this.NotifyColonistBar();
		}

		// Token: 0x06001E87 RID: 7815 RVA: 0x000FCF00 File Offset: 0x000FB100
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

		// Token: 0x06001E88 RID: 7816 RVA: 0x0001B0FC File Offset: 0x000192FC
		private void NotifyColonistBar()
		{
			if (this.InnerPawn.Faction == Faction.OfPlayer && Current.ProgramState == ProgramState.Playing)
			{
				Find.ColonistBar.MarkColonistsDirty();
			}
		}

		// Token: 0x04001599 RID: 5529
		private ThingOwner<Pawn> innerContainer;

		// Token: 0x0400159A RID: 5530
		public int timeOfDeath = -1;

		// Token: 0x0400159B RID: 5531
		private int vanishAfterTimestamp = -1;

		// Token: 0x0400159C RID: 5532
		private BillStack operationsBillStack;

		// Token: 0x0400159D RID: 5533
		public bool everBuriedInSarcophagus;

		// Token: 0x0400159E RID: 5534
		private const int VanishAfterTicksSinceDessicated = 6000000;
	}
}
