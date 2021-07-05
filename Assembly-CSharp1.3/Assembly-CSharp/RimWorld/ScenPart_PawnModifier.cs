using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001002 RID: 4098
	public class ScenPart_PawnModifier : ScenPart
	{
		// Token: 0x0600607F RID: 24703 RVA: 0x0020DE88 File Offset: 0x0020C088
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.chance, "chance", 0f, false);
			Scribe_Values.Look<PawnGenerationContext>(ref this.context, "context", PawnGenerationContext.All, false);
			Scribe_Values.Look<bool>(ref this.hideOffMap, "hideOffMap", false, false);
		}

		// Token: 0x06006080 RID: 24704 RVA: 0x0020DED8 File Offset: 0x0020C0D8
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
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}
		}

		// Token: 0x06006081 RID: 24705 RVA: 0x0020E04C File Offset: 0x0020C24C
		public override void Randomize()
		{
			this.chance = GenMath.RoundedHundredth(Rand.Range(0.05f, 1f));
			this.context = PawnGenerationContextUtility.GetRandom();
			this.hideOffMap = false;
		}

		// Token: 0x06006082 RID: 24706 RVA: 0x0020E07A File Offset: 0x0020C27A
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

		// Token: 0x06006083 RID: 24707 RVA: 0x0020E0B9 File Offset: 0x0020C2B9
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

		// Token: 0x06006084 RID: 24708 RVA: 0x0020E0FC File Offset: 0x0020C2FC
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

		// Token: 0x06006085 RID: 24709 RVA: 0x0000313F File Offset: 0x0000133F
		protected virtual void ModifyNewPawn(Pawn p)
		{
		}

		// Token: 0x06006086 RID: 24710 RVA: 0x0000313F File Offset: 0x0000133F
		protected virtual void ModifyPawnPostGenerate(Pawn p, bool redressed)
		{
		}

		// Token: 0x06006087 RID: 24711 RVA: 0x0000313F File Offset: 0x0000133F
		protected virtual void ModifyHideOffMapStartingPawnPostMapGenerate(Pawn p)
		{
		}

		// Token: 0x04003737 RID: 14135
		protected float chance = 1f;

		// Token: 0x04003738 RID: 14136
		protected PawnGenerationContext context;

		// Token: 0x04003739 RID: 14137
		protected bool hideOffMap;

		// Token: 0x0400373A RID: 14138
		private string chanceBuf;
	}
}
