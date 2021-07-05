using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001086 RID: 4230
	internal class Building_SunLamp : Building
	{
		// Token: 0x17001139 RID: 4409
		// (get) Token: 0x060064AA RID: 25770 RVA: 0x0021E95F File Offset: 0x0021CB5F
		public IEnumerable<IntVec3> GrowableCells
		{
			get
			{
				return GenRadial.RadialCellsAround(base.Position, this.def.specialDisplayRadius, true);
			}
		}

		// Token: 0x060064AB RID: 25771 RVA: 0x0021E978 File Offset: 0x0021CB78
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (DesignatorUtility.FindAllowedDesignator<Designator_ZoneAdd_Growing>() != null)
			{
				yield return new Command_Action
				{
					action = new Action(this.MakeMatchingGrowZone),
					hotKey = KeyBindingDefOf.Misc2,
					defaultDesc = "CommandSunLampMakeGrowingZoneDesc".Translate(),
					icon = ContentFinder<Texture2D>.Get("UI/Designators/ZoneCreate_Growing", true),
					defaultLabel = "CommandSunLampMakeGrowingZoneLabel".Translate()
				};
			}
			yield break;
			yield break;
		}

		// Token: 0x060064AC RID: 25772 RVA: 0x0021E988 File Offset: 0x0021CB88
		private void MakeMatchingGrowZone()
		{
			Designator designator = DesignatorUtility.FindAllowedDesignator<Designator_ZoneAdd_Growing>();
			designator.DesignateMultiCell(from tempCell in this.GrowableCells
			where designator.CanDesignateCell(tempCell).Accepted
			select tempCell);
		}
	}
}
