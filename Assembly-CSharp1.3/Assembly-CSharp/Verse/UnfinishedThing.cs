using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200037D RID: 893
	public class UnfinishedThing : ThingWithComps
	{
		// Token: 0x1700055D RID: 1373
		// (get) Token: 0x06001A29 RID: 6697 RVA: 0x00098C65 File Offset: 0x00096E65
		// (set) Token: 0x06001A2A RID: 6698 RVA: 0x00098C6D File Offset: 0x00096E6D
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
					Log.Error("Cannot set creator to null.");
					return;
				}
				this.creatorInt = value;
				this.creatorName = value.LabelShort;
			}
		}

		// Token: 0x1700055E RID: 1374
		// (get) Token: 0x06001A2B RID: 6699 RVA: 0x00098C90 File Offset: 0x00096E90
		public RecipeDef Recipe
		{
			get
			{
				return this.recipeInt;
			}
		}

		// Token: 0x1700055F RID: 1375
		// (get) Token: 0x06001A2C RID: 6700 RVA: 0x00098C98 File Offset: 0x00096E98
		// (set) Token: 0x06001A2D RID: 6701 RVA: 0x00098CCC File Offset: 0x00096ECC
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

		// Token: 0x17000560 RID: 1376
		// (get) Token: 0x06001A2E RID: 6702 RVA: 0x00098D28 File Offset: 0x00096F28
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

		// Token: 0x17000561 RID: 1377
		// (get) Token: 0x06001A2F RID: 6703 RVA: 0x00098D60 File Offset: 0x00096F60
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

		// Token: 0x17000562 RID: 1378
		// (get) Token: 0x06001A30 RID: 6704 RVA: 0x00098DF3 File Offset: 0x00096FF3
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

		// Token: 0x17000563 RID: 1379
		// (get) Token: 0x06001A31 RID: 6705 RVA: 0x00098E14 File Offset: 0x00097014
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

		// Token: 0x17000564 RID: 1380
		// (get) Token: 0x06001A32 RID: 6706 RVA: 0x00098E35 File Offset: 0x00097035
		public bool Initialized
		{
			get
			{
				return this.workLeft > -5000f;
			}
		}

		// Token: 0x06001A33 RID: 6707 RVA: 0x00098E44 File Offset: 0x00097044
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

		// Token: 0x06001A34 RID: 6708 RVA: 0x00098EEC File Offset: 0x000970EC
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

		// Token: 0x06001A35 RID: 6709 RVA: 0x00098F8A File Offset: 0x0009718A
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

		// Token: 0x06001A36 RID: 6710 RVA: 0x00098F9C File Offset: 0x0009719C
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

		// Token: 0x06001A37 RID: 6711 RVA: 0x0009900F File Offset: 0x0009720F
		public override void DrawExtraSelectionOverlays()
		{
			base.DrawExtraSelectionOverlays();
			if (this.BoundWorkTable != null)
			{
				GenDraw.DrawLineBetween(this.TrueCenter(), this.BoundWorkTable.TrueCenter());
			}
		}

		// Token: 0x06001A38 RID: 6712 RVA: 0x00099038 File Offset: 0x00097238
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

		// Token: 0x0400111E RID: 4382
		private Pawn creatorInt;

		// Token: 0x0400111F RID: 4383
		private string creatorName = "ErrorCreatorName";

		// Token: 0x04001120 RID: 4384
		private RecipeDef recipeInt;

		// Token: 0x04001121 RID: 4385
		public List<Thing> ingredients = new List<Thing>();

		// Token: 0x04001122 RID: 4386
		private Bill_ProductionWithUft boundBillInt;

		// Token: 0x04001123 RID: 4387
		public float workLeft = -10000f;

		// Token: 0x04001124 RID: 4388
		private const float CancelIngredientRecoveryFraction = 0.75f;
	}
}
