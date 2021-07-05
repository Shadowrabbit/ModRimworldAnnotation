using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DBB RID: 7611
	public class RoomRequirement_AllThingsAreGlowing : RoomRequirement
	{
		// Token: 0x0600A581 RID: 42369 RVA: 0x00300B5C File Offset: 0x002FED5C
		public override bool Met(Room r, Pawn p = null)
		{
			using (IEnumerator<Thing> enumerator = r.ContainedThings(this.thingDef).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.TryGetComp<CompGlower>().Glows)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600A582 RID: 42370 RVA: 0x0006DA39 File Offset: 0x0006BC39
		public override IEnumerable<string> ConfigErrors()
		{
			if (this.thingDef == null)
			{
				yield return "thingDef is null";
				yield break;
			}
			if (this.thingDef.GetCompProperties<CompProperties_Glower>() == null)
			{
				yield return "No comp glower on thingDef";
			}
			yield break;
		}

		// Token: 0x04007031 RID: 28721
		public ThingDef thingDef;
	}
}
