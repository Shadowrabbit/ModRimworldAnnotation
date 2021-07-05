using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200152E RID: 5422
	public class RoomRequirement_AllThingsAreGlowing : RoomRequirement
	{
		// Token: 0x06008107 RID: 33031 RVA: 0x002DA534 File Offset: 0x002D8734
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

		// Token: 0x06008108 RID: 33032 RVA: 0x002DA594 File Offset: 0x002D8794
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

		// Token: 0x06008109 RID: 33033 RVA: 0x002DA5A4 File Offset: 0x002D87A4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.thingDef, "thingDef");
		}

		// Token: 0x04005059 RID: 20569
		public ThingDef thingDef;
	}
}
