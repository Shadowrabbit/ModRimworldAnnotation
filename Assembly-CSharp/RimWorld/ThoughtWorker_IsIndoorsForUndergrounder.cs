using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EBD RID: 3773
	public class ThoughtWorker_IsIndoorsForUndergrounder : ThoughtWorker
	{
		// Token: 0x060053CB RID: 21451 RVA: 0x001C1BE0 File Offset: 0x001BFDE0
		public static bool IsAwakeAndIndoors(Pawn p, out bool isNaturalRoof)
		{
			isNaturalRoof = false;
			if (!p.Awake())
			{
				return false;
			}
			if (p.Position.UsesOutdoorTemperature(p.Map))
			{
				return false;
			}
			RoofDef roofDef = p.Map.roofGrid.RoofAt(p.Position);
			if (roofDef == null)
			{
				return false;
			}
			isNaturalRoof = roofDef.isNatural;
			return true;
		}

		// Token: 0x060053CC RID: 21452 RVA: 0x001C1C34 File Offset: 0x001BFE34
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			bool flag;
			return ThoughtWorker_IsIndoorsForUndergrounder.IsAwakeAndIndoors(p, out flag) && !flag;
		}
	}
}
