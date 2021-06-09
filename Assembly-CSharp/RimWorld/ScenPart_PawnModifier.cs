using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020015E8 RID: 5608
	public class ScenPart_PawnModifier : ScenPart
	{
		// Token: 0x060079D9 RID: 31193 RVA: 0x0024DCF0 File Offset: 0x0024BEF0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.chance, "chance", 0f, false);
			Scribe_Values.Look<PawnGenerationContext>(ref this.context, "context", PawnGenerationContext.All, false);
			Scribe_Values.Look<bool>(ref this.hideOffMap, "hideOffMap", false, false);
		}

		// Token: 0x060079DA RID: 31194 RVA: 0x0024DD40 File Offset: 0x0024BF40
		protected void DoPawnModifierEditInterface(Rect rect)
		{
			Rect rect2 = rect.TopHalf();
			Rect rect3 = rect2.LeftPart(0.333f).Rounded();
			Rect rect4 = rect2.RightPart(0.666f).Rounded();
			Text.Anchor = TextAnchor.MiddleRight;
			Widgets.Label(rect3, "chance".Translate());
			Text.Anchor = TextAnchor.UpperLeft;
			Widgets.TextFieldPercent(rect4, ref this.chance, ref this.chanceBuf, 0f, 1f);
			Rect rect5 = rect.BottomHalf();
			Rect rect6 = rect5.LeftPart(0.333f).Rounded();
			Rect rect7 = rect5.RightPart(0.666f).Rounded();
			Text.Anchor = TextAnchor.MiddleRight;
			Widgets.Label(rect6, "context".Translate());
			Text.Anchor = TextAnchor.UpperLeft;
			if (Widgets.ButtonText(rect7, this.context.ToStringHuman(), true, true, true))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				foreach (object obj in Enum.GetValues(typeof(PawnGenerationContext)))
				{
					PawnGenerationContext localCont2 = (PawnGenerationContext)obj;
					PawnGenerationContext localCont = localCont2;
					list.Add(new FloatMenuOption(localCont.ToStringHuman(), delegate()
					{
						this.context = localCont;
					}, MenuOptionPriority.Default, null, null, 0f, null, null));
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}
		}

		// Token: 0x060079DB RID: 31195 RVA: 0x00052018 File Offset: 0x00050218
		public override void Randomize()
		{
			this.chance = GenMath.RoundedHundredth(Rand.Range(0.05f, 1f));
			this.context = PawnGenerationContextUtility.GetRandom();
			this.hideOffMap = false;
		}

		// Token: 0x060079DC RID: 31196 RVA: 0x00052046 File Offset: 0x00050246
		public override void Notify_NewPawnGenerating(Pawn pawn, PawnGenerationContext context)
		{
			if (!this.context.Includes(context))
			{
				return;
			}
			if (this.hideOffMap && context == PawnGenerationContext.PlayerStarter)
			{
				return;
			}
			if (Rand.Chance(this.chance) && pawn.RaceProps.Humanlike)
			{
				this.ModifyNewPawn(pawn);
			}
		}

		// Token: 0x060079DD RID: 31197 RVA: 0x00052085 File Offset: 0x00050285
		public override void Notify_PawnGenerated(Pawn pawn, PawnGenerationContext context, bool redressed)
		{
			if (!this.context.Includes(context))
			{
				return;
			}
			if (this.hideOffMap && context == PawnGenerationContext.PlayerStarter)
			{
				return;
			}
			if (Rand.Chance(this.chance) && pawn.RaceProps.Humanlike)
			{
				this.ModifyPawnPostGenerate(pawn, redressed);
			}
		}

		// Token: 0x060079DE RID: 31198 RVA: 0x0024DEB4 File Offset: 0x0024C0B4
		public override void PostMapGenerate(Map map)
		{
			if (Find.GameInitData == null)
			{
				return;
			}
			if (this.hideOffMap && this.context.Includes(PawnGenerationContext.PlayerStarter))
			{
				foreach (Pawn pawn in Find.GameInitData.startingAndOptionalPawns)
				{
					if (Rand.Chance(this.chance) && pawn.RaceProps.Humanlike)
					{
						this.ModifyHideOffMapStartingPawnPostMapGenerate(pawn);
					}
				}
			}
		}

		// Token: 0x060079DF RID: 31199 RVA: 0x00006A05 File Offset: 0x00004C05
		protected virtual void ModifyNewPawn(Pawn p)
		{
		}

		// Token: 0x060079E0 RID: 31200 RVA: 0x00006A05 File Offset: 0x00004C05
		protected virtual void ModifyPawnPostGenerate(Pawn p, bool redressed)
		{
		}

		// Token: 0x060079E1 RID: 31201 RVA: 0x00006A05 File Offset: 0x00004C05
		protected virtual void ModifyHideOffMapStartingPawnPostMapGenerate(Pawn p)
		{
		}

		// Token: 0x0400500C RID: 20492
		protected float chance = 1f;

		// Token: 0x0400500D RID: 20493
		protected PawnGenerationContext context;

		// Token: 0x0400500E RID: 20494
		protected bool hideOffMap;

		// Token: 0x0400500F RID: 20495
		private string chanceBuf;
	}
}
