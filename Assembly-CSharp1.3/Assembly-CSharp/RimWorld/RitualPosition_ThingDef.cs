using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F88 RID: 3976
	public abstract class RitualPosition_ThingDef : RitualPosition_NearbyThing
	{
		// Token: 0x1700103C RID: 4156
		// (get) Token: 0x06005E19 RID: 24089
		protected abstract ThingDef ThingDef { get; }

		// Token: 0x06005E1A RID: 24090 RVA: 0x00204C78 File Offset: 0x00202E78
		public override IEnumerable<Thing> CandidateThings(LordJob_Ritual ritual)
		{
			foreach (Thing thing in ritual.Map.listerThings.ThingsOfDef(this.ThingDef))
			{
				if (thing.Position.InHorDistOf(ritual.selectedTarget.Cell, 50f))
				{
					yield return thing;
				}
			}
			List<Thing>.Enumerator enumerator = default(List<Thing>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06005E1B RID: 24091 RVA: 0x00204C90 File Offset: 0x00202E90
		public override IntVec3 PositionForThing(Thing t)
		{
			return t.Position + t.Rotation.Opposite.FacingCell;
		}

		// Token: 0x06005E1C RID: 24092 RVA: 0x00204CC0 File Offset: 0x00202EC0
		public override bool IsUsableThing(Thing thing, IntVec3 spot, TargetInfo ritualTarget)
		{
			IntVec3 c = this.PositionForThing(thing);
			Map map = ritualTarget.Map;
			Building edifice = c.GetEdifice(map);
			return c.Standable(map) && (edifice == null || edifice.def.Fillage == FillCategory.None) && base.IsUsableThing(thing, spot, ritualTarget);
		}

		// Token: 0x06005E1D RID: 24093 RVA: 0x00204D07 File Offset: 0x00202F07
		protected override Rot4 FacingDir(Thing t)
		{
			return t.Rotation;
		}
	}
}
