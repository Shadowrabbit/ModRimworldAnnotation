using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D55 RID: 3413
	public class CompAbilityEffect_RaidEnemy : CompAbilityEffect_WithDest
	{
		// Token: 0x17000DBF RID: 3519
		// (get) Token: 0x06004F83 RID: 20355 RVA: 0x001AA41A File Offset: 0x001A861A
		public new CompProperties_RaidEnemy Props
		{
			get
			{
				return (CompProperties_RaidEnemy)this.props;
			}
		}

		// Token: 0x06004F84 RID: 20356 RVA: 0x001AA428 File Offset: 0x001A8628
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			IncidentParms incidentParms = new IncidentParms();
			incidentParms.target = this.parent.pawn.Map;
			incidentParms.points = this.Props.points;
			incidentParms.faction = Find.FactionManager.FirstFactionOfDef(this.Props.factionDef);
			incidentParms.raidStrategy = this.Props.raidStrategyDef;
			incidentParms.raidArrivalMode = this.Props.pawnArrivalModeDef;
			incidentParms.raidStrategy = this.Props.raidStrategyDef;
			incidentParms.spawnCenter = target.Cell;
			IncidentDefOf.RaidEnemy.Worker.TryExecute(incidentParms);
		}
	}
}
