using System;

namespace RimWorld
{
	// Token: 0x020014C5 RID: 5317
	public class StatPart_Biocoded : StatPart
	{
		// Token: 0x06007EED RID: 32493 RVA: 0x002CEF6B File Offset: 0x002CD16B
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing && CompBiocodable.IsBiocoded(req.Thing))
			{
				val *= 0f;
			}
		}

		// Token: 0x06007EEE RID: 32494 RVA: 0x00002688 File Offset: 0x00000888
		public override string ExplanationPart(StatRequest req)
		{
			return null;
		}
	}
}
