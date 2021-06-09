using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200051F RID: 1311
	public class UnfinishedThing : ThingWithComps
	{
		// Token: 0x17000651 RID: 1617
		// (get) Token: 0x06002191 RID: 8593 RVA: 0x0001D0E3 File Offset: 0x0001B2E3
		// (set) Token: 0x06002192 RID: 8594 RVA: 0x0001D0EB File Offset: 0x0001B2EB
		public Pawn Creator
		{
			get
			{
				return this.creatorInt;
			}
			set
			{
				if (value == null)
				{
					Log.Error("Cannot set creator to null.", false);
					return;
				}
				this.creatorInt = value;
				this.creatorName = value.LabelShort;
			}
		}

		// Token: 0x17000652 RID: 1618
		// (get) Token: 0x06002193 RID: 8595 RVA: 0x0001D10F File Offset: 0x0001B30F
		public RecipeDef Recipe
		{
			get
			{
				return this.recipeInt;
			}
		}

		// Token: 0x17000653 RID: 1619
		// (get) Token: 0x06002194 RID: 8596 RVA: 0x0001D117 File Offset: 0x0001B317
		// (set) Token: 0x06002195 RID: 8597 RVA: 0x0010719C File Offset: 0x0010539C
		public Bill_ProductionWithUft BoundBill
		{
			get
			{
				if (this.boundBillInt != null && (this.boundBillInt.DeletedOrDereferenced || this.boundBillInt.BoundUft != this))
				{
					this.boundBillInt = null;
				}
				return this.boundBillInt;
			}
			set
			{
				if (value == this.boundBillInt)
				{
					return;
				}
				Bill_ProductionWithUft bill_ProductionWithUft = this.boundBillInt;
				this.boundBillInt = value;
				if (bill_ProductionWithUft != null && bill_ProductionWithUft.BoundUft == this)
				{
					bill_ProductionWithUft.SetBoundUft(null, false);
				}
				if (value != null)
				{
					this.recipeInt = value.recipe;
					if (value.BoundUft != this)
					{
						value.SetBoundUft(this, false);
					}
				}
			}
		}

		// Token: 0x17000654 RID: 1620
		// (get) Token: 0x06002196 RID: 8598 RVA: 0x001071F8 File Offset: 0x001053F8
		public Thing BoundWorkTable
		{
			get
			{
				if (this.BoundBill == null)
				{
					return null;
				}
				Thing thing = this.BoundBill.billStack.billGiver as Thing;
				if (thing.Destroyed)
				{
					return null;
				}
				return thing;
			}
		}

		// Token: 0x17000655 RID: 1621
		// (get) Token: 0x06002197 RID: 8599 RVA: 0x00107230 File Offset: 0x00105430
		public override string LabelNoCount
		{
			get
			{
				if (this.Recipe == null)
				{
					return base.LabelNoCount;
				}
				if (base.Stuff == null)
				{
					return "UnfinishedItem".Translate(this.Recipe.products[0].thingDef.label);
				}
				return "UnfinishedItemWithStuff".Translate(base.Stuff.LabelAsStuff, this.Recipe.products[0].thingDef.label);
			}
		}

		// Token: 0x17000656 RID: 1622
		// (get) Token: 0x06002198 RID: 8600 RVA: 0x0001D149 File Offset: 0x0001B349
		public override string DescriptionDetailed
		{
			get
			{
				if (this.Recipe == null)
				{
					return base.LabelNoCount;
				}
				return this.Recipe.ProducedThingDef.DescriptionDetailed;
			}
		}

		// Token: 0x17000657 RID: 1623
		// (get) Token: 0x06002199 RID: 8601 RVA: 0x0001D16A File Offset: 0x0001B36A
		public override string DescriptionFlavor
		{
			get
			{
				if (this.Recipe == null)
				{
					return base.LabelNoCount;
				}
				return this.Recipe.ProducedThingDef.description;
			}
		}

		// Token: 0x17000658 RID: 1624
		// (get) Token: 0x0600219A RID: 8602 RVA: 0x0001D18B File Offset: 0x0001B38B
		public bool Initialized
		{
			get
			{
				return this.workLeft > -5000f;
			}
		}

		// Token: 0x0600219B RID: 8603 RVA: 0x001072C4 File Offset: 0x001054C4
		public override void ExposeData()
		{
			base.ExposeData();
			if (Scribe.mode == LoadSaveMode.Saving && this.boundBillInt != null && this.boundBillInt.DeletedOrDereferenced)
			{
				this.boundBillInt = null;
			}
			Scribe_References.Look<Pawn>(ref this.creatorInt, "creator", false);
			Scribe_Values.Look<string>(ref this.creatorName, "creatorName", null, false);
			Scribe_References.Look<Bill_ProductionWithUft>(ref this.boundBillInt, "bill", false);
			Scribe_Defs.Look<RecipeDef>(ref this.recipeInt, "recipe");
			Scribe_Values.Look<float>(ref this.workLeft, "workLeft", 0f, false);
			Scribe_Collections.Look<Thing>(ref this.ingredients, "ingredients", LookMode.Deep, Array.Empty<object>());
		}

		// Token: 0x0600219C RID: 8604 RVA: 0x0010736C File Offset: 0x0010556C
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			if (mode == DestroyMode.Cancel)
			{
				for (int i = 0; i < this.ingredients.Count; i++)
				{
					int num = GenMath.RoundRandom((float)this.ingredients[i].stackCount * 0.75f);
					if (num > 0)
					{
						this.ingredients[i].stackCount = num;
						GenPlace.TryPlaceThing(this.ingredients[i], base.Position, base.Map, ThingPlaceMode.Near, null, null, default(Rot4));
					}
				}
				this.ingredients.Clear();
			}
			base.Destroy(mode);
			this.BoundBill = null;
		}

		// Token: 0x0600219D RID: 8605 RVA: 0x0001D19A File Offset: 0x0001B39A
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			yield return new Command_Action
			{
				defaultLabel = "CommandCancelConstructionLabel".Translate(),
				defaultDesc = "CommandCancelConstructionDesc".Translate(),
				icon = ContentFinder<Texture2D>.Get("UI/Designators/Cancel", true),
				hotKey = KeyBindingDefOf.Designator_Cancel,
				action = delegate()
				{
					this.Destroy(DestroyMode.Cancel);
				}
			};
			yield break;
			yield break;
		}

		// Token: 0x0600219E RID: 8606 RVA: 0x0010740C File Offset: 0x0010560C
		public Bill_ProductionWithUft BillOnTableForMe(Thing workTable)
		{
			if (this.Recipe.AllRecipeUsers.Contains(workTable.def))
			{
				IBillGiver billGiver = (IBillGiver)workTable;
				for (int i = 0; i < billGiver.BillStack.Count; i++)
				{
					Bill_ProductionWithUft bill_ProductionWithUft = billGiver.BillStack[i] as Bill_ProductionWithUft;
					if (bill_ProductionWithUft != null && bill_ProductionWithUft.ShouldDoNow() && bill_ProductionWithUft != null && bill_ProductionWithUft.recipe == this.Recipe)
					{
						return bill_ProductionWithUft;
					}
				}
			}
			return null;
		}

		// Token: 0x0600219F RID: 8607 RVA: 0x0001D1AA File Offset: 0x0001B3AA
		public override void DrawExtraSelectionOverlays()
		{
			base.DrawExtraSelectionOverlays();
			if (this.BoundWorkTable != null)
			{
				GenDraw.DrawLineBetween(this.TrueCenter(), this.BoundWorkTable.TrueCenter());
			}
		}

		// Token: 0x060021A0 RID: 8608 RVA: 0x00107480 File Offset: 0x00105680
		public override string GetInspectString()
		{
			string text = base.GetInspectString();
			if (!text.NullOrEmpty())
			{
				text += "\n";
			}
			text += "Author".Translate() + ": " + this.creatorName;
			return text + ("\n" + "WorkLeft".Translate() + ": " + this.workLeft.ToStringWorkAmount());
		}

		// Token: 0x040016E6 RID: 5862
		private Pawn creatorInt;

		// Token: 0x040016E7 RID: 5863
		private string creatorName = "ErrorCreatorName";

		// Token: 0x040016E8 RID: 5864
		private RecipeDef recipeInt;

		// Token: 0x040016E9 RID: 5865
		public List<Thing> ingredients = new List<Thing>();

		// Token: 0x040016EA RID: 5866
		private Bill_ProductionWithUft boundBillInt;

		// Token: 0x040016EB RID: 5867
		public float workLeft = -10000f;

		// Token: 0x040016EC RID: 5868
		private const float CancelIngredientRecoveryFraction = 0.75f;
	}
}
