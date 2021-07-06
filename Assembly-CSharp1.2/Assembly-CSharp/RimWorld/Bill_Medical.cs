using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x020013B7 RID: 5047
	public class Bill_Medical : Bill
	{
		// Token: 0x170010F1 RID: 4337
		// (get) Token: 0x06006D74 RID: 28020 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public override bool CheckIngredientsIfSociallyProper
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170010F2 RID: 4338
		// (get) Token: 0x06006D75 RID: 28021 RVA: 0x0000A2E4 File Offset: 0x000084E4
		protected override bool CanCopy
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170010F3 RID: 4339
		// (get) Token: 0x06006D76 RID: 28022 RVA: 0x0004A632 File Offset: 0x00048832
		public override bool CompletableEver
		{
			get
			{
				return !this.recipe.targetsBodyPart || this.recipe.Worker.GetPartsToApplyOn(this.GiverPawn, this.recipe).Contains(this.part);
			}
		}

		// Token: 0x170010F4 RID: 4340
		// (get) Token: 0x06006D77 RID: 28023 RVA: 0x0004A66D File Offset: 0x0004886D
		// (set) Token: 0x06006D78 RID: 28024 RVA: 0x0004A675 File Offset: 0x00048875
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
					Log.Error("Can only set Bill_Medical.Part after the bill has been added to a pawn's bill stack.", false);
					return;
				}
				this.part = value;
			}
		}

		// Token: 0x170010F5 RID: 4341
		// (get) Token: 0x06006D79 RID: 28025 RVA: 0x00218DAC File Offset: 0x00216FAC
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

		// Token: 0x170010F6 RID: 4342
		// (get) Token: 0x06006D7A RID: 28026 RVA: 0x00218DF4 File Offset: 0x00216FF4
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

		// Token: 0x06006D7B RID: 28027 RVA: 0x0004A69A File Offset: 0x0004889A
		public Bill_Medical()
		{
		}

		// Token: 0x06006D7C RID: 28028 RVA: 0x0004A6A2 File Offset: 0x000488A2
		public Bill_Medical(RecipeDef recipe) : base(recipe)
		{
		}

		// Token: 0x06006D7D RID: 28029 RVA: 0x0004A6AB File Offset: 0x000488AB
		public override bool ShouldDoNow()
		{
			return !this.suspended;
		}

		// Token: 0x06006D7E RID: 28030 RVA: 0x00218E68 File Offset: 0x00217068
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

		// Token: 0x06006D7F RID: 28031 RVA: 0x00218EE0 File Offset: 0x002170E0
		public override void Notify_DoBillStarted(Pawn billDoer)
		{
			base.Notify_DoBillStarted(billDoer);
			this.consumedInitialMedicineDef = null;
			if (!this.GiverPawn.Dead && this.recipe.anesthetize && HealthUtility.TryAnesthetize(this.GiverPawn))
			{
				List<ThingCountClass> placedThings = billDoer.CurJob.placedThings;
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

		// Token: 0x06006D80 RID: 28032 RVA: 0x0004A6B8 File Offset: 0x000488B8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_BodyParts.Look(ref this.part, "part", null);
			Scribe_Defs.Look<ThingDef>(ref this.consumedInitialMedicineDef, "consumedInitialMedicineDef");
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x06006D81 RID: 28033 RVA: 0x0004A6E7 File Offset: 0x000488E7
		public override Bill Clone()
		{
			Bill_Medical bill_Medical = (Bill_Medical)base.Clone();
			bill_Medical.part = this.part;
			bill_Medical.consumedInitialMedicineDef = this.consumedInitialMedicineDef;
			return bill_Medical;
		}

		// Token: 0x04004862 RID: 18530
		private BodyPartRecord part;

		// Token: 0x04004863 RID: 18531
		public ThingDef consumedInitialMedicineDef;

		// Token: 0x04004864 RID: 18532
		public int temp_partIndexToSetLater;
	}
}
