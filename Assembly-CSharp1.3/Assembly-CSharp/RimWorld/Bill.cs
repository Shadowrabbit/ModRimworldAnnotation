using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020006B5 RID: 1717
	public abstract class Bill : IExposable, ILoadReferenceable
	{
		// Token: 0x170008E2 RID: 2274
		// (get) Token: 0x06002F8A RID: 12170 RVA: 0x00119774 File Offset: 0x00117974
		public Map Map
		{
			get
			{
				return this.billStack.billGiver.Map;
			}
		}

		// Token: 0x170008E3 RID: 2275
		// (get) Token: 0x06002F8B RID: 12171 RVA: 0x00119786 File Offset: 0x00117986
		public virtual string Label
		{
			get
			{
				if (this.precept == null)
				{
					return this.recipe.label;
				}
				return this.precept.Label;
			}
		}

		// Token: 0x170008E4 RID: 2276
		// (get) Token: 0x06002F8C RID: 12172 RVA: 0x001197A7 File Offset: 0x001179A7
		public virtual string LabelCap
		{
			get
			{
				if (this.precept == null)
				{
					return this.Label.CapitalizeFirst(this.recipe);
				}
				return this.precept.LabelCap;
			}
		}

		// Token: 0x170008E5 RID: 2277
		// (get) Token: 0x06002F8D RID: 12173 RVA: 0x000126F5 File Offset: 0x000108F5
		public virtual bool CheckIngredientsIfSociallyProper
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170008E6 RID: 2278
		// (get) Token: 0x06002F8E RID: 12174 RVA: 0x000126F5 File Offset: 0x000108F5
		public virtual bool CompletableEver
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170008E7 RID: 2279
		// (get) Token: 0x06002F8F RID: 12175 RVA: 0x00002688 File Offset: 0x00000888
		protected virtual string StatusString
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170008E8 RID: 2280
		// (get) Token: 0x06002F90 RID: 12176 RVA: 0x000682C5 File Offset: 0x000664C5
		protected virtual float StatusLineMinHeight
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x170008E9 RID: 2281
		// (get) Token: 0x06002F91 RID: 12177 RVA: 0x000126F5 File Offset: 0x000108F5
		protected virtual bool CanCopy
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170008EA RID: 2282
		// (get) Token: 0x06002F92 RID: 12178 RVA: 0x001197D0 File Offset: 0x001179D0
		public bool DeletedOrDereferenced
		{
			get
			{
				if (this.deleted)
				{
					return true;
				}
				Thing thing = this.billStack.billGiver as Thing;
				return thing != null && thing.Destroyed;
			}
		}

		// Token: 0x170008EB RID: 2283
		// (get) Token: 0x06002F93 RID: 12179 RVA: 0x00119806 File Offset: 0x00117A06
		public Pawn PawnRestriction
		{
			get
			{
				return this.pawnRestriction;
			}
		}

		// Token: 0x170008EC RID: 2284
		// (get) Token: 0x06002F94 RID: 12180 RVA: 0x0011980E File Offset: 0x00117A0E
		public bool SlavesOnly
		{
			get
			{
				return this.slavesOnly;
			}
		}

		// Token: 0x06002F95 RID: 12181 RVA: 0x00119816 File Offset: 0x00117A16
		public Bill()
		{
		}

		// Token: 0x06002F96 RID: 12182 RVA: 0x0011984C File Offset: 0x00117A4C
		public Bill(RecipeDef recipe, Precept_ThingStyle precept = null)
		{
			this.recipe = recipe;
			this.precept = precept;
			this.ingredientFilter = new ThingFilter();
			this.ingredientFilter.CopyAllowancesFrom(recipe.defaultIngredientFilter);
			this.InitializeAfterClone();
		}

		// Token: 0x06002F97 RID: 12183 RVA: 0x001198BA File Offset: 0x00117ABA
		public void InitializeAfterClone()
		{
			this.loadID = Find.UniqueIDsManager.GetNextBillID();
		}

		// Token: 0x06002F98 RID: 12184 RVA: 0x001198CC File Offset: 0x00117ACC
		public void SetPawnRestriction(Pawn pawn)
		{
			this.pawnRestriction = pawn;
			this.slavesOnly = false;
		}

		// Token: 0x06002F99 RID: 12185 RVA: 0x001198DC File Offset: 0x00117ADC
		public void SetAnySlaveRestriction()
		{
			this.pawnRestriction = null;
			this.slavesOnly = true;
		}

		// Token: 0x06002F9A RID: 12186 RVA: 0x001198EC File Offset: 0x00117AEC
		public void SetAnyPawnRestriction()
		{
			this.slavesOnly = false;
			this.pawnRestriction = null;
		}

		// Token: 0x06002F9B RID: 12187 RVA: 0x001198FC File Offset: 0x00117AFC
		public virtual void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.loadID, "loadID", 0, false);
			Scribe_Defs.Look<RecipeDef>(ref this.recipe, "recipe");
			Scribe_Values.Look<bool>(ref this.suspended, "suspended", false, false);
			Scribe_Values.Look<float>(ref this.ingredientSearchRadius, "ingredientSearchRadius", 999f, false);
			Scribe_Values.Look<IntRange>(ref this.allowedSkillRange, "allowedSkillRange", default(IntRange), false);
			Scribe_References.Look<Pawn>(ref this.pawnRestriction, "pawnRestriction", false);
			Scribe_References.Look<Precept_ThingStyle>(ref this.precept, "precept", false);
			Scribe_Values.Look<bool>(ref this.slavesOnly, "slavesOnly", false, false);
			if (Scribe.mode == LoadSaveMode.Saving && this.recipe.fixedIngredientFilter != null)
			{
				foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
				{
					if (!this.recipe.fixedIngredientFilter.Allows(thingDef))
					{
						this.ingredientFilter.SetAllow(thingDef, false);
					}
				}
			}
			Scribe_Deep.Look<ThingFilter>(ref this.ingredientFilter, "ingredientFilter", Array.Empty<object>());
		}

		// Token: 0x06002F9C RID: 12188 RVA: 0x00119A24 File Offset: 0x00117C24
		public virtual bool PawnAllowedToStartAnew(Pawn p)
		{
			if (this.pawnRestriction != null)
			{
				return this.pawnRestriction == p;
			}
			if (this.slavesOnly)
			{
				return p.IsSlave;
			}
			if (this.recipe.workSkill != null)
			{
				int level = p.skills.GetSkill(this.recipe.workSkill).Level;
				if (level < this.allowedSkillRange.min)
				{
					JobFailReason.Is("UnderAllowedSkill".Translate(this.allowedSkillRange.min), this.Label);
					return false;
				}
				if (level > this.allowedSkillRange.max)
				{
					JobFailReason.Is("AboveAllowedSkill".Translate(this.allowedSkillRange.max), this.Label);
					return false;
				}
			}
			return true;
		}

		// Token: 0x06002F9D RID: 12189 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Notify_PawnDidWork(Pawn p)
		{
		}

		// Token: 0x06002F9E RID: 12190 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Notify_IterationCompleted(Pawn billDoer, List<Thing> ingredients)
		{
		}

		// Token: 0x06002F9F RID: 12191
		public abstract bool ShouldDoNow();

		// Token: 0x06002FA0 RID: 12192 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Notify_DoBillStarted(Pawn billDoer)
		{
		}

		// Token: 0x06002FA1 RID: 12193 RVA: 0x00119AF4 File Offset: 0x00117CF4
		protected virtual void DoConfigInterface(Rect rect, Color baseColor)
		{
			rect.yMin += 29f;
			float y = rect.center.y;
			Widgets.InfoCardButton(rect.xMax - (rect.yMax - y) - 12f, y - 12f, this.recipe);
		}

		// Token: 0x06002FA2 RID: 12194 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void DoStatusLineInterface(Rect rect)
		{
		}

		// Token: 0x06002FA3 RID: 12195 RVA: 0x00119B4C File Offset: 0x00117D4C
		public Rect DoInterface(float x, float y, float width, int index)
		{
			Rect rect = new Rect(x, y, width, 53f);
			float num = 0f;
			if (!this.StatusString.NullOrEmpty())
			{
				num = Mathf.Max(17f, this.StatusLineMinHeight);
			}
			rect.height += num;
			Color white = Color.white;
			if (!this.ShouldDoNow())
			{
				white = new Color(1f, 0.7f, 0.7f, 0.7f);
			}
			GUI.color = white;
			Text.Font = GameFont.Small;
			if (index % 2 == 0)
			{
				Widgets.DrawAltRect(rect);
			}
			GUI.BeginGroup(rect);
			Rect rect2 = new Rect(0f, 0f, 24f, 24f);
			if (this.billStack.IndexOf(this) > 0)
			{
				if (Widgets.ButtonImage(rect2, TexButton.ReorderUp, white, true))
				{
					this.billStack.Reorder(this, -1);
					SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
				}
				TooltipHandler.TipRegionByKey(rect2, "ReorderBillUpTip");
			}
			if (this.billStack.IndexOf(this) < this.billStack.Count - 1)
			{
				Rect rect3 = new Rect(0f, 24f, 24f, 24f);
				if (Widgets.ButtonImage(rect3, TexButton.ReorderDown, white, true))
				{
					this.billStack.Reorder(this, 1);
					SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
				}
				TooltipHandler.TipRegionByKey(rect3, "ReorderBillDownTip");
			}
			Widgets.Label(new Rect(28f, 0f, rect.width - 48f - 20f, rect.height + 5f), this.LabelCap);
			this.DoConfigInterface(rect.AtZero(), white);
			Rect rect4 = new Rect(rect.width - 24f, 0f, 24f, 24f);
			if (Widgets.ButtonImage(rect4, TexButton.DeleteX, white, white * GenUI.SubtleMouseoverColor, true))
			{
				this.billStack.Delete(this);
				SoundDefOf.Click.PlayOneShotOnCamera(null);
			}
			TooltipHandler.TipRegionByKey(rect4, "DeleteBillTip");
			Rect rect6;
			if (this.CanCopy)
			{
				Rect rect5 = new Rect(rect4);
				rect5.x -= rect5.width + 4f;
				if (Widgets.ButtonImageFitted(rect5, TexButton.Copy, white))
				{
					BillUtility.Clipboard = this.Clone();
					SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
				}
				TooltipHandler.TipRegionByKey(rect5, "CopyBillTip");
				rect6 = new Rect(rect5);
			}
			else
			{
				rect6 = new Rect(rect4);
			}
			rect6.x -= rect6.width + 4f;
			if (Widgets.ButtonImage(rect6, TexButton.Suspend, white, true))
			{
				this.suspended = !this.suspended;
				SoundDefOf.Click.PlayOneShotOnCamera(null);
			}
			TooltipHandler.TipRegionByKey(rect6, "SuspendBillTip");
			if (!this.StatusString.NullOrEmpty())
			{
				Text.Font = GameFont.Tiny;
				Rect rect7 = new Rect(24f, rect.height - num, rect.width - 24f, num);
				Widgets.Label(rect7, this.StatusString);
				this.DoStatusLineInterface(rect7);
			}
			GUI.EndGroup();
			if (this.suspended)
			{
				Text.Font = GameFont.Medium;
				Text.Anchor = TextAnchor.MiddleCenter;
				Rect rect8 = new Rect(rect.x + rect.width / 2f - 70f, rect.y + rect.height / 2f - 20f, 140f, 40f);
				GUI.DrawTexture(rect8, TexUI.GrayTextBG);
				Widgets.Label(rect8, "SuspendedCaps".Translate());
				Text.Anchor = TextAnchor.UpperLeft;
				Text.Font = GameFont.Small;
			}
			Text.Font = GameFont.Small;
			GUI.color = Color.white;
			return rect;
		}

		// Token: 0x06002FA4 RID: 12196 RVA: 0x00119EF0 File Offset: 0x001180F0
		public bool IsFixedOrAllowedIngredient(Thing thing)
		{
			for (int i = 0; i < this.recipe.ingredients.Count; i++)
			{
				IngredientCount ingredientCount = this.recipe.ingredients[i];
				if (ingredientCount.IsFixedIngredient && ingredientCount.filter.Allows(thing))
				{
					return true;
				}
			}
			return this.recipe.fixedIngredientFilter.Allows(thing) && this.ingredientFilter.Allows(thing);
		}

		// Token: 0x06002FA5 RID: 12197 RVA: 0x00119F64 File Offset: 0x00118164
		public bool IsFixedOrAllowedIngredient(ThingDef def)
		{
			for (int i = 0; i < this.recipe.ingredients.Count; i++)
			{
				IngredientCount ingredientCount = this.recipe.ingredients[i];
				if (ingredientCount.IsFixedIngredient && ingredientCount.filter.Allows(def))
				{
					return true;
				}
			}
			return this.recipe.fixedIngredientFilter.Allows(def) && this.ingredientFilter.Allows(def);
		}

		// Token: 0x06002FA6 RID: 12198 RVA: 0x00119FD8 File Offset: 0x001181D8
		public static void CreateNoPawnsWithSkillDialog(RecipeDef recipe)
		{
			string text = "RecipeRequiresSkills".Translate(recipe.LabelCap);
			text += "\n\n";
			text += recipe.MinSkillString;
			Find.WindowStack.Add(new Dialog_MessageBox(text, null, null, null, null, null, false, null, null));
		}

		// Token: 0x06002FA7 RID: 12199 RVA: 0x0011A036 File Offset: 0x00118236
		public virtual BillStoreModeDef GetStoreMode()
		{
			return BillStoreModeDefOf.BestStockpile;
		}

		// Token: 0x06002FA8 RID: 12200 RVA: 0x00002688 File Offset: 0x00000888
		public virtual Zone_Stockpile GetStoreZone()
		{
			return null;
		}

		// Token: 0x06002FA9 RID: 12201 RVA: 0x0011A03D File Offset: 0x0011823D
		public virtual void SetStoreMode(BillStoreModeDef mode, Zone_Stockpile zone = null)
		{
			Log.ErrorOnce("Tried to set store mode of a non-production bill", 27190980);
		}

		// Token: 0x06002FAA RID: 12202 RVA: 0x0011A050 File Offset: 0x00118250
		public virtual Bill Clone()
		{
			Bill bill = (Bill)Activator.CreateInstance(base.GetType());
			bill.recipe = this.recipe;
			bill.precept = this.precept;
			bill.suspended = this.suspended;
			bill.ingredientFilter = new ThingFilter();
			bill.ingredientFilter.CopyAllowancesFrom(this.ingredientFilter);
			bill.ingredientSearchRadius = this.ingredientSearchRadius;
			bill.allowedSkillRange = this.allowedSkillRange;
			bill.pawnRestriction = this.pawnRestriction;
			bill.slavesOnly = this.slavesOnly;
			return bill;
		}

		// Token: 0x06002FAB RID: 12203 RVA: 0x0011A0E0 File Offset: 0x001182E0
		public virtual void ValidateSettings()
		{
			if (this.pawnRestriction != null && (this.pawnRestriction.Dead || this.pawnRestriction.Faction != Faction.OfPlayer || this.pawnRestriction.IsKidnapped()))
			{
				if (this != BillUtility.Clipboard)
				{
					Messages.Message("MessageBillValidationPawnUnavailable".Translate(this.pawnRestriction.LabelShortCap, this.Label, this.billStack.billGiver.LabelShort), this.billStack.billGiver as Thing, MessageTypeDefOf.NegativeEvent, true);
				}
				this.pawnRestriction = null;
				return;
			}
		}

		// Token: 0x06002FAC RID: 12204 RVA: 0x0011A193 File Offset: 0x00118393
		public string GetUniqueLoadID()
		{
			return string.Concat(new object[]
			{
				"Bill_",
				this.recipe.defName,
				"_",
				this.loadID
			});
		}

		// Token: 0x06002FAD RID: 12205 RVA: 0x0011A1CC File Offset: 0x001183CC
		public override string ToString()
		{
			return this.GetUniqueLoadID();
		}

		// Token: 0x04001D01 RID: 7425
		[Unsaved(false)]
		public BillStack billStack;

		// Token: 0x04001D02 RID: 7426
		private int loadID = -1;

		// Token: 0x04001D03 RID: 7427
		public RecipeDef recipe;

		// Token: 0x04001D04 RID: 7428
		public Precept_ThingStyle precept;

		// Token: 0x04001D05 RID: 7429
		public bool suspended;

		// Token: 0x04001D06 RID: 7430
		public ThingFilter ingredientFilter;

		// Token: 0x04001D07 RID: 7431
		public float ingredientSearchRadius = 999f;

		// Token: 0x04001D08 RID: 7432
		public IntRange allowedSkillRange = new IntRange(0, 20);

		// Token: 0x04001D09 RID: 7433
		private Pawn pawnRestriction;

		// Token: 0x04001D0A RID: 7434
		private bool slavesOnly;

		// Token: 0x04001D0B RID: 7435
		public bool deleted;

		// Token: 0x04001D0C RID: 7436
		public int lastIngredientSearchFailTicks = -99999;

		// Token: 0x04001D0D RID: 7437
		public const int MaxIngredientSearchRadius = 999;

		// Token: 0x04001D0E RID: 7438
		public const float ButSize = 24f;

		// Token: 0x04001D0F RID: 7439
		private const float InterfaceBaseHeight = 53f;

		// Token: 0x04001D10 RID: 7440
		private const float InterfaceStatusLineHeight = 17f;
	}
}
