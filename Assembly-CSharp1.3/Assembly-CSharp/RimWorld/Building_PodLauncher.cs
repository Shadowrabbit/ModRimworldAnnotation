using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001082 RID: 4226
	public class Building_PodLauncher : Building
	{
		// Token: 0x0600649E RID: 25758 RVA: 0x0021E7BA File Offset: 0x0021C9BA
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			AcceptanceReport acceptanceReport = GenConstruct.CanPlaceBlueprintAt(ThingDefOf.TransportPod, FuelingPortUtility.GetFuelingPortCell(this), ThingDefOf.TransportPod.defaultPlacingRot, base.Map, false, null, null, null);
			Designator_Build designator_Build = BuildCopyCommandUtility.FindAllowedDesignator(ThingDefOf.TransportPod, true);
			if (designator_Build == null)
			{
				yield break;
			}
			Command_Action command_Action = new Command_Action();
			command_Action.defaultLabel = "BuildThing".Translate(ThingDefOf.TransportPod.label);
			command_Action.icon = designator_Build.icon;
			command_Action.defaultDesc = designator_Build.Desc;
			command_Action.action = delegate()
			{
				IntVec3 fuelingPortCell = FuelingPortUtility.GetFuelingPortCell(this);
				GenConstruct.PlaceBlueprintForBuild(ThingDefOf.TransportPod, fuelingPortCell, base.Map, ThingDefOf.TransportPod.defaultPlacingRot, Faction.OfPlayer, null);
			};
			if (!acceptanceReport.Accepted)
			{
				command_Action.Disable(acceptanceReport.Reason);
			}
			yield return command_Action;
			yield break;
			yield break;
		}
	}
}
