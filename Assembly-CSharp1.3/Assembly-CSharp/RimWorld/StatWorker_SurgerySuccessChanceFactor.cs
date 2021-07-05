using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020014FF RID: 5375
	public class StatWorker_SurgerySuccessChanceFactor : StatWorker
	{
		// Token: 0x06008013 RID: 32787 RVA: 0x002D5B28 File Offset: 0x002D3D28
		public override bool ShouldShowFor(StatRequest req)
		{
			if (!base.ShouldShowFor(req))
			{
				return false;
			}
			Def def = req.Def;
			if (!(def is ThingDef))
			{
				return false;
			}
			ThingDef thingDef = def as ThingDef;
			return typeof(Building_Bed).IsAssignableFrom(thingDef.thingClass);
		}
	}
}
