using System;

namespace RimWorld
{
	// Token: 0x02001D2D RID: 7469
	public class StatPart_Biocoded : StatPart
	{
		// Token: 0x0600A262 RID: 41570 RVA: 0x0006BE20 File Offset: 0x0006A020
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing && EquipmentUtility.IsBiocoded(req.Thing))
			{
				val *= 0f;
			}
		}

		// Token: 0x0600A263 RID: 41571 RVA: 0x0000C32E File Offset: 0x0000A52E
		public override string ExplanationPart(StatRequest req)
		{
			return null;
		}
	}
}
