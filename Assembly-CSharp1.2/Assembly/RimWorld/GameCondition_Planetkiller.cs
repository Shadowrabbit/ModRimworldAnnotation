using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001177 RID: 4471
	public class GameCondition_Planetkiller : GameCondition
	{
		// Token: 0x17000F72 RID: 3954
		// (get) Token: 0x0600627F RID: 25215 RVA: 0x001EC228 File Offset: 0x001EA428
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

		// Token: 0x06006280 RID: 25216 RVA: 0x001EC318 File Offset: 0x001EA518
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

		// Token: 0x06006281 RID: 25217 RVA: 0x00043CA7 File Offset: 0x00041EA7
		public override void End()
		{
			base.End();
			this.Impact();
		}

		// Token: 0x06006282 RID: 25218 RVA: 0x00043CB5 File Offset: 0x00041EB5
		private void Impact()
		{
			ScreenFader.SetColor(Color.clear);
			GenGameEnd.EndGameDialogMessage("GameOverPlanetkillerImpact".Translate(Find.World.info.name), false, GameCondition_Planetkiller.FadeColor);
		}

		// Token: 0x04004205 RID: 16901
		private const int SoundDuration = 179;

		// Token: 0x04004206 RID: 16902
		private const int FadeDuration = 90;

		// Token: 0x04004207 RID: 16903
		private static readonly Color FadeColor = Color.white;
	}
}
