using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EEF RID: 3823
	public class Precept_RitualSeat : Precept_ThingDef
	{
		// Token: 0x17000FC9 RID: 4041
		// (get) Token: 0x06005ACA RID: 23242 RVA: 0x001F64CE File Offset: 0x001F46CE
		public override string UIInfoSecondLine
		{
			get
			{
				return base.ThingDef.LabelCap;
			}
		}

		// Token: 0x06005ACB RID: 23243 RVA: 0x001F64E0 File Offset: 0x001F46E0
		public override IEnumerable<FloatMenuOption> EditFloatMenuOptions()
		{
			using (IEnumerator<PreceptThingChance> enumerator = this.def.Worker.ThingDefs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					PreceptThingChance d = enumerator.Current;
					if (d.def != base.ThingDef)
					{
						yield return new FloatMenuOption("PreceptReplaceWith".Translate() + ": " + d.def.LabelCap, delegate()
						{
							this.ThingDef = d.def;
						}, d.def, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
					}
				}
			}
			IEnumerator<PreceptThingChance> enumerator = null;
			yield break;
			yield break;
		}
	}
}
