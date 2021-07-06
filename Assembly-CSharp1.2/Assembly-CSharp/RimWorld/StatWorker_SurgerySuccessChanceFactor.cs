using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D6C RID: 7532
	public class StatWorker_SurgerySuccessChanceFactor : StatWorker
	{
		// Token: 0x0600A3BD RID: 41917 RVA: 0x002FAF64 File Offset: 0x002F9164
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
