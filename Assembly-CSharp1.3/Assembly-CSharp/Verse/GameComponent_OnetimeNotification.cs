using System;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x02000133 RID: 307
	public class GameComponent_OnetimeNotification : GameComponent
	{
		// Token: 0x0600086C RID: 2156 RVA: 0x00027740 File Offset: 0x00025940
		public GameComponent_OnetimeNotification(Game game)
		{
		}

		// Token: 0x0600086D RID: 2157 RVA: 0x00027750 File Offset: 0x00025950
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

		// Token: 0x0600086E RID: 2158 RVA: 0x00027836 File Offset: 0x00025A36
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.sendAICoreRequestReminder, "sendAICoreRequestReminder", false, false);
		}

		// Token: 0x040007E2 RID: 2018
		public bool sendAICoreRequestReminder = true;
	}
}
