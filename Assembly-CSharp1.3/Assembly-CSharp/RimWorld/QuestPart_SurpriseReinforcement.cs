using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BB1 RID: 2993
	public class QuestPart_SurpriseReinforcement : QuestPartActivable
	{
		// Token: 0x060045D9 RID: 17881 RVA: 0x00171F30 File Offset: 0x00170130
		public override void Notify_PawnKilled(Pawn pawn, DamageInfo? dinfo)
		{
			if (this.reinforcementChance == 0f)
			{
				return;
			}
			if (pawn.Faction == this.faction && pawn.MapHeld != null && pawn.MapHeld.Parent != null && pawn.MapHeld.Parent == this.mapParent)
			{
				if (Rand.Chance(this.reinforcementChance))
				{
					this.ticksTillRaid = (int)QuestPart_SurpriseReinforcement.RandomDelayRange.RandomInRange;
				}
				this.reinforcementChance = 0f;
			}
		}

		// Token: 0x060045DA RID: 17882 RVA: 0x00171FB0 File Offset: 0x001701B0
		public override void QuestPartTick()
		{
			base.QuestPartTick();
			if (this.ticksTillRaid > -1)
			{
				this.ticksTillRaid--;
				if (this.ticksTillRaid == 0)
				{
					IncidentParms incidentParms = new IncidentParms();
					incidentParms.target = this.mapParent.Map;
					incidentParms.points = StorytellerUtility.DefaultThreatPointsNow(incidentParms.target) * 2.5f;
					incidentParms.faction = this.faction;
					incidentParms.customLetterLabel = "LetterLabelSurpriseReinforcements".Translate();
					incidentParms.customLetterText = "LetterSurpriseReinforcements".Translate(incidentParms.faction.def.pawnsPlural, incidentParms.faction).Resolve();
					IncidentDefOf.RaidEnemy.Worker.TryExecute(incidentParms);
				}
			}
		}

		// Token: 0x060045DB RID: 17883 RVA: 0x00172080 File Offset: 0x00170280
		public override void ExposeData()
		{
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Values.Look<float>(ref this.reinforcementChance, "reinforcementChance", 0f, false);
			Scribe_Values.Look<int>(ref this.ticksTillRaid, "ticksTillRaid", -1, false);
		}

		// Token: 0x04002A8D RID: 10893
		public MapParent mapParent;

		// Token: 0x04002A8E RID: 10894
		public Faction faction;

		// Token: 0x04002A8F RID: 10895
		public float reinforcementChance;

		// Token: 0x04002A90 RID: 10896
		private int ticksTillRaid = -1;

		// Token: 0x04002A91 RID: 10897
		public const float RaidThreatPointsMultiplier = 2.5f;

		// Token: 0x04002A92 RID: 10898
		private static readonly FloatRange RandomDelayRange = new FloatRange(300f, 900f);
	}
}
