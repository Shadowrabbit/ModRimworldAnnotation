using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001165 RID: 4453
	public class CompProperties_MoteEmitterRandomColor : CompProperties_MoteEmitter
	{
		// Token: 0x06006AF6 RID: 27382 RVA: 0x0023E922 File Offset: 0x0023CB22
		public CompProperties_MoteEmitterRandomColor()
		{
			this.compClass = typeof(CompMoteEmitterRandomColor);
		}

		// Token: 0x06006AF7 RID: 27383 RVA: 0x0023E93A File Offset: 0x0023CB3A
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			foreach (string text in base.ConfigErrors(parentDef))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.colors.NullOrEmpty<Color>())
			{
				yield return "colors list is empty or null";
			}
			yield break;
			yield break;
		}

		// Token: 0x04003B78 RID: 15224
		public List<Color> colors;
	}
}
