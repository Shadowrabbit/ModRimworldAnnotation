using System;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x020001D2 RID: 466
	public class GameComponent_OnetimeNotification : GameComponent
	{
		// Token: 0x06000C12 RID: 3090 RVA: 0x0000F4B5 File Offset: 0x0000D6B5
		public GameComponent_OnetimeNotification(Game game)
		{
		}

		// Token: 0x06000C13 RID: 3091 RVA: 0x000A330C File Offset: 0x000A150C
		public override void GameComponentTick()
		{
			if (Find.TickManager.TicksGame % 2000 != 0 || !Rand.Chance(0.05f))
			{
				return;
			}
			if (this.sendAICoreRequestReminder)
			{
				if (ResearchProjectTagDefOf.ShipRelated.CompletedProjects() < 2)
				{
					return;
				}
				if (PlayerItemAccessibilityUtility.PlayerOrQuestRewardHas(ThingDefOf.AIPersonaCore, 1) || PlayerItemAccessibilityUtility.PlayerOrQuestRewardHas(ThingDefOf.Ship_ComputerCore, 1))
				{
					return;
				}
				Faction faction = Find.FactionManager.RandomNonHostileFaction(false, false, true, TechLevel.Undefined);
				if (faction == null || faction.leader == null)
				{
					return;
				}
				Find.LetterStack.ReceiveLetter("LetterLabelAICoreOffer".Translate(), "LetterAICoreOffer".Translate(faction.leader.LabelDefinite(), faction.Name, faction.leader.Named("PAWN")).CapitalizeFirst(), LetterDefOf.NeutralEvent, GlobalTargetInfo.Invalid, faction, null, null, null);
				this.sendAICoreRequestReminder = false;
			}
		}

		// Token: 0x06000C14 RID: 3092 RVA: 0x0000F4C4 File Offset: 0x0000D6C4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.sendAICoreRequestReminder, "sendAICoreRequestReminder", false, false);
		}

		// Token: 0x04000A8A RID: 2698
		public bool sendAICoreRequestReminder = true;
	}
}
