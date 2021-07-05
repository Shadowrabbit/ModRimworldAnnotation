using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D57 RID: 3415
	public class CompAbilityEffect_RandomAlliedMilitaryAid : CompAbilityEffect_WithDest
	{
		// Token: 0x17000DC0 RID: 3520
		// (get) Token: 0x06004F87 RID: 20359 RVA: 0x001AA4D6 File Offset: 0x001A86D6
		public new CompProperties_RandomAlliedMilitaryAid Props
		{
			get
			{
				return (CompProperties_RandomAlliedMilitaryAid)this.props;
			}
		}

		// Token: 0x06004F88 RID: 20360 RVA: 0x001AA4E4 File Offset: 0x001A86E4
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			IncidentParms incidentParms = new IncidentParms();
			incidentParms.target = this.parent.pawn.Map;
			incidentParms.faction = this.GetRandomAlliedFaction();
			incidentParms.raidArrivalModeForQuickMilitaryAid = true;
			incidentParms.points = this.Props.points;
			incidentParms.spawnCenter = target.Cell;
			if (!IncidentDefOf.RaidFriendly.Worker.TryExecute(incidentParms))
			{
				Log.Error("Failed to request military aid");
			}
		}

		// Token: 0x06004F89 RID: 20361 RVA: 0x001AA562 File Offset: 0x001A8762
		private Faction GetRandomAlliedFaction()
		{
			return Find.FactionManager.RandomAlliedFaction(true, false, true, TechLevel.Undefined);
		}

		// Token: 0x06004F8A RID: 20362 RVA: 0x001AA572 File Offset: 0x001A8772
		public override bool CanApplyOn(LocalTargetInfo target, LocalTargetInfo dest)
		{
			return this.GetRandomAlliedFaction() != null && base.CanApplyOn(target, dest);
		}
	}
}
