using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D45 RID: 3397
	public class CompAbilityEffect_Infestation : CompAbilityEffect_WithDest
	{
		// Token: 0x17000DB7 RID: 3511
		// (get) Token: 0x06004F58 RID: 20312 RVA: 0x001A980F File Offset: 0x001A7A0F
		public new CompProperties_Infestation Props
		{
			get
			{
				return (CompProperties_Infestation)this.props;
			}
		}

		// Token: 0x06004F59 RID: 20313 RVA: 0x001A981C File Offset: 0x001A7A1C
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			IncidentParms incidentParms = new IncidentParms();
			incidentParms.target = this.parent.pawn.Map;
			incidentParms.points = this.Props.points;
			incidentParms.infestationLocOverride = new IntVec3?(target.Cell);
			incidentParms.forced = true;
			IncidentDefOf.Infestation.Worker.TryExecute(incidentParms);
		}
	}
}
