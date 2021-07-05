using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000BE0 RID: 3040
	public class GameCondition_Planetkiller : GameCondition
	{
		// Token: 0x17000C89 RID: 3209
		// (get) Token: 0x06004793 RID: 18323 RVA: 0x0017AAD8 File Offset: 0x00178CD8
		public override string TooltipString
		{
			get
			{
				Vector2 location;
				if (Find.CurrentMap != null)
				{
					location = Find.WorldGrid.LongLatOf(Find.CurrentMap.Tile);
				}
				else
				{
					location = default(Vector2);
				}
				return this.def.LabelCap + "\n" + "\n" + this.Description + ("\n" + "ImpactDate".Translate().CapitalizeFirst() + ": " + GenDate.DateFullStringAt((long)GenDate.TickGameToAbs(this.startTick + base.Duration), location)) + ("\n" + "TimeLeft".Translate().CapitalizeFirst() + ": " + base.TicksLeft.ToStringTicksToPeriod(true, false, true, true));
			}
		}

		// Token: 0x06004794 RID: 18324 RVA: 0x0017ABC8 File Offset: 0x00178DC8
		public override void GameConditionTick()
		{
			base.GameConditionTick();
			if (base.TicksLeft <= 179)
			{
				Find.ActiveLesson.Deactivate();
				if (base.TicksLeft == 179)
				{
					SoundDefOf.PlanetkillerImpact.PlayOneShotOnCamera(null);
				}
				if (base.TicksLeft == 90)
				{
					ScreenFader.StartFade(GameCondition_Planetkiller.FadeColor, 1f);
				}
			}
		}

		// Token: 0x06004795 RID: 18325 RVA: 0x0017AC23 File Offset: 0x00178E23
		public override void End()
		{
			base.End();
			this.Impact();
		}

		// Token: 0x06004796 RID: 18326 RVA: 0x0017AC31 File Offset: 0x00178E31
		private void Impact()
		{
			ScreenFader.SetColor(Color.clear);
			GenGameEnd.EndGameDialogMessage("GameOverPlanetkillerImpact".Translate(Find.World.info.name), false, GameCondition_Planetkiller.FadeColor);
		}

		// Token: 0x04002BED RID: 11245
		private const int SoundDuration = 179;

		// Token: 0x04002BEE RID: 11246
		private const int FadeDuration = 90;

		// Token: 0x04002BEF RID: 11247
		private static readonly Color FadeColor = Color.white;
	}
}
