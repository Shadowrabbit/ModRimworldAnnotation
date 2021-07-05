using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000FAC RID: 4012
	public class RitualTargetFilter_IdeoBuildingOrRitualSpot : RitualTargetFilter_IdeoBuilding
	{
		// Token: 0x06005EBE RID: 24254 RVA: 0x002071A3 File Offset: 0x002053A3
		public RitualTargetFilter_IdeoBuildingOrRitualSpot()
		{
		}

		// Token: 0x06005EBF RID: 24255 RVA: 0x002071AB File Offset: 0x002053AB
		public RitualTargetFilter_IdeoBuildingOrRitualSpot(RitualTargetFilterDef def) : base(def)
		{
		}

		// Token: 0x06005EC0 RID: 24256 RVA: 0x002071B4 File Offset: 0x002053B4
		protected override IEnumerable<Thing> ExtraCandidates(TargetInfo initiator)
		{
			Pawn pawn = (Pawn)initiator.Thing;
			return from s in initiator.Map.listerThings.ThingsOfDef(ThingDefOf.RitualSpot)
			where pawn.CanReach(s, PathEndMode.Touch, pawn.NormalMaxDanger(), false, false, TraverseMode.ByPawn)
			select s;
		}

		// Token: 0x06005EC1 RID: 24257 RVA: 0x00207200 File Offset: 0x00205400
		public override IEnumerable<string> GetTargetInfos(TargetInfo initiator)
		{
			foreach (string text in base.GetTargetInfos(initiator))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			yield return ThingDefOf.RitualSpot.LabelCap;
			yield break;
			yield break;
		}
	}
}
