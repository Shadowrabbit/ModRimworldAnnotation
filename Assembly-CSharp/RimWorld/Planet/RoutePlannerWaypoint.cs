using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200213D RID: 8509
	public class RoutePlannerWaypoint : WorldObject
	{
		// Token: 0x17001AA7 RID: 6823
		// (get) Token: 0x0600B4F6 RID: 46326 RVA: 0x00346FA0 File Offset: 0x003451A0
		public override string Label
		{
			get
			{
				WorldRoutePlanner worldRoutePlanner = Find.WorldRoutePlanner;
				if (worldRoutePlanner.Active)
				{
					int num = worldRoutePlanner.waypoints.IndexOf(this);
					if (num >= 0)
					{
						return base.Label + " " + (num + 1);
					}
				}
				return base.Label;
			}
		}

		// Token: 0x0600B4F7 RID: 46327 RVA: 0x00346FEC File Offset: 0x003451EC
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.GetInspectString());
			WorldRoutePlanner worldRoutePlanner = Find.WorldRoutePlanner;
			if (worldRoutePlanner.Active)
			{
				int num = worldRoutePlanner.waypoints.IndexOf(this);
				if (num >= 1)
				{
					int ticksToWaypoint = worldRoutePlanner.GetTicksToWaypoint(num);
					if (stringBuilder.Length != 0)
					{
						stringBuilder.AppendLine();
					}
					stringBuilder.Append("EstimatedTimeToWaypoint".Translate(ticksToWaypoint.ToStringTicksToDays("0.#")));
					if (num >= 2)
					{
						int ticksToWaypoint2 = worldRoutePlanner.GetTicksToWaypoint(num - 1);
						stringBuilder.AppendLine();
						stringBuilder.Append("EstimatedTimeToWaypointFromPrevious".Translate((ticksToWaypoint - ticksToWaypoint2).ToStringTicksToDays("0.#")));
					}
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600B4F8 RID: 46328 RVA: 0x00075818 File Offset: 0x00073A18
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			yield return new Command_Action
			{
				defaultLabel = "CommandRemoveWaypointLabel".Translate(),
				defaultDesc = "CommandRemoveWaypointDesc".Translate(),
				icon = TexCommand.RemoveRoutePlannerWaypoint,
				action = delegate()
				{
					Find.WorldRoutePlanner.TryRemoveWaypoint(this, true);
				}
			};
			yield break;
			yield break;
		}
	}
}
