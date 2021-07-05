using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D8F RID: 3471
	public class Bill_Medical : Bill
	{
		// Token: 0x17000DF3 RID: 3571
		// (get) Token: 0x06005084 RID: 20612 RVA: 0x0001276E File Offset: 0x0001096E
		public override bool CheckIngredientsIfSociallyProper
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000DF4 RID: 3572
		// (get) Token: 0x06005085 RID: 20613 RVA: 0x0001276E File Offset: 0x0001096E
		protected override bool CanCopy
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000DF5 RID: 3573
		// (get) Token: 0x06005086 RID: 20614 RVA: 0x001AF477 File Offset: 0x001AD677
		public override bool CompletableEver
		{
			get
			{
				return !this.recipe.targetsBodyPart || this.recipe.Worker.GetPartsToApplyOn(this.GiverPawn, this.recipe).Contains(this.part);
			}
		}

		// Token: 0x17000DF6 RID: 3574
		// (get) Token: 0x06005087 RID: 20615 RVA: 0x001AF4B2 File Offset: 0x001AD6B2
		// (set) Token: 0x06005088 RID: 20616 RVA: 0x001AF4BA File Offset: 0x001AD6BA
		public BodyPartRecord Part
		{
			get
			{
				return this.part;
			}
			set
			{
				if (this.billStack == null && this.part != null)
				{
					Log.Error("Can only set Bill_Medical.Part after the bill has been added to a pawn's bill stack.");
					return;
				}
				this.part = value;
			}
		}

		// Token: 0x17000DF7 RID: 3575
		// (get) Token: 0x06005089 RID: 20617 RVA: 0x001AF4E0 File Offset: 0x001AD6E0
		public Pawn GiverPawn
		{
			get
			{
				Pawn pawn = this.billStack.billGiver as Pawn;
				Corpse corpse = this.billStack.billGiver as Corpse;
				if (corpse != null)
				{
					pawn = corpse.InnerPawn;
				}
				if (pawn == null)
				{
					throw new InvalidOperationException("Medical bill on non-pawn.");
				}
				return pawn;
			}
		}

		// Token: 0x17000DF8 RID: 3576
		// (get) Token: 0x0600508A RID: 20618 RVA: 0x001AF528 File Offset: 0x001AD728
		public override string Label
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(this.recipe.Worker.GetLabelWhenUsedOn(this.GiverPawn, this.part));
				if (this.Part != null && !this.recipe.hideBodyPartNames)
				{
					stringBuilder.Append(" (" + this.Part.Label + ")");
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x0600508B RID: 20619 RVA: 0x001AF59A File Offset: 0x001AD79A
		public Bill_Medical()
		{
		}

		// Token: 0x0600508C RID: 20620 RVA: 0x001AF5A2 File Offset: 0x001AD7A2
		public Bill_Medical(RecipeDef recipe) : base(recipe, null)
		{
		}

		// Token: 0x0600508D RID: 20621 RVA: 0x001AF5AC File Offset: 0x001AD7AC
		public override bool ShouldDoNow()
		{
			return !this.suspended;
		}

		// Token: 0x0600508E RID: 20622 RVA: 0x001AF5BC File Offset: 0x001AD7BC
		public override void Notify_IterationCompleted(Pawn billDoer, List<Thing> ingredients)
		{
			base.Notify_IterationCompleted(billDoer, ingredients);
			if (this.CompletableEver)
			{
				Pawn giverPawn = this.GiverPawn;
				this.recipe.Worker.ApplyOnPawn(giverPawn, this.Part, billDoer, ingredients, this);
				if (giverPawn.RaceProps.IsFlesh)
				{
					giverPawn.records.Increment(RecordDefOf.OperationsReceived);
					billDoer.records.Increment(RecordDefOf.OperationsPerformed);
				}
			}
			this.billStack.Delete(this);
		}

		// Token: 0x0600508F RID: 20623 RVA: 0x001AF634 File Offset: 0x001AD834
		public override void Notify_DoBillStarted(Pawn billDoer)
		{
			base.Notify_DoBillStarted(billDoer);
			this.consumedInitialMedicineDef = null;
			if (!this.GiverPawn.Dead && this.recipe.anesthetize && HealthUtility.TryAnesthetize(this.GiverPawn))
			{
				List<ThingCountClass> placedThings = billDoer.CurJob.placedThings;
				if (placedThings != null)
				{
					int i = 0;
					while (i < placedThings.Count)
					{
						if (placedThings[i].thing is Medicine)
						{
							this.recipe.Worker.ConsumeIngredient(placedThings[i].thing.SplitOff(1), this.recipe, billDoer.MapHeld);
							ThingCountClass thingCountClass = placedThings[i];
							int count = thingCountClass.Count;
							thingCountClass.Count = count - 1;
							this.consumedInitialMedicineDef = placedThings[i].thing.def;
							if (placedThings[i].thing.Destroyed || placedThings[i].Count <= 0)
							{
								placedThings.RemoveAt(i);
								return;
							}
							break;
						}
						else
						{
							i++;
						}
					}
				}
			}
		}

		// Token: 0x06005090 RID: 20624 RVA: 0x001AF744 File Offset: 0x001AD944
		public override bool PawnAllowedToStartAnew(Pawn pawn)
		{
			if (!base.PawnAllowedToStartAnew(pawn))
			{
				return false;
			}
			if (this.recipe.Worker is Recipe_AdministerIngestible)
			{
				ThingDef singleDef = this.recipe.ingredients[0].filter.BestThingRequest.singleDef;
				if (singleDef.IsDrug && !new HistoryEvent(HistoryEventDefOf.AdministeredDrug, pawn.Named(HistoryEventArgsNames.Doer)).Notify_PawnAboutToDo_Job())
				{
					return false;
				}
				if (singleDef.IsDrug && singleDef.ingestible.drugCategory == DrugCategory.Hard && !new HistoryEvent(HistoryEventDefOf.AdministeredHardDrug, pawn.Named(HistoryEventArgsNames.Doer)).Notify_PawnAboutToDo_Job())
				{
					return false;
				}
				if (singleDef.IsNonMedicalDrug && !new HistoryEvent(HistoryEventDefOf.AdministeredRecreationalDrug, pawn.Named(HistoryEventArgsNames.Doer)).Notify_PawnAboutToDo_Job())
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06005091 RID: 20625 RVA: 0x001AF813 File Offset: 0x001ADA13
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_BodyParts.Look(ref this.part, "part", null);
			Scribe_Defs.Look<ThingDef>(ref this.consumedInitialMedicineDef, "consumedInitialMedicineDef");
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x06005092 RID: 20626 RVA: 0x001AF842 File Offset: 0x001ADA42
		public override Bill Clone()
		{
			Bill_Medical bill_Medical = (Bill_Medical)base.Clone();
			bill_Medical.part = this.part;
			bill_Medical.consumedInitialMedicineDef = this.consumedInitialMedicineDef;
			return bill_Medical;
		}

		// Token: 0x04002FF9 RID: 12281
		private BodyPartRecord part;

		// Token: 0x04002FFA RID: 12282
		public ThingDef consumedInitialMedicineDef;

		// Token: 0x04002FFB RID: 12283
		public int temp_partIndexToSetLater;
	}
}
