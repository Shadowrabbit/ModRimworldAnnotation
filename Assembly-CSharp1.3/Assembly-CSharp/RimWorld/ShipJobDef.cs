using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000ABD RID: 2749
	public class ShipJobDef : Def
	{
		// Token: 0x0600410C RID: 16652 RVA: 0x0015EA0D File Offset: 0x0015CC0D
		public override IEnumerable<string> ConfigErrors()
		{
			IEnumerable<string> enumerable = base.ConfigErrors();
			foreach (string text in enumerable)
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (!typeof(ShipJob).IsAssignableFrom(this.jobClass))
			{
				yield return this.jobClass.Name + " does not inherit from ShipJob";
			}
			yield break;
			yield break;
		}

		// Token: 0x04002687 RID: 9863
		public Type jobClass;
	}
}
