using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009B3 RID: 2483
	public class ThoughtWorker_IsIndoorsForUndergrounder : ThoughtWorker
	{
		// Token: 0x06003DF5 RID: 15861 RVA: 0x00153D78 File Offset: 0x00151F78
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

		// Token: 0x06003DF6 RID: 15862 RVA: 0x00153DCC File Offset: 0x00151FCC
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			bool flag;
			return ThoughtWorker_IsIndoorsForUndergrounder.IsAwakeAndIndoors(p, out flag) && !flag;
		}
	}
}
