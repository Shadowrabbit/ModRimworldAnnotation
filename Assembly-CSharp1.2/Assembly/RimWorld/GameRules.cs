using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200101B RID: 4123
	public class GameRules : IExposable
	{
		// Token: 0x060059F2 RID: 23026 RVA: 0x001D40D0 File Offset: 0x001D22D0
		public void SetAllowDesignator(Type type, bool allowed)
		{
			if (allowed && this.disallowedDesignatorTypes.Contains(type))
			{
				this.disallowedDesignatorTypes.Remove(type);
			}
			if (!allowed && !this.disallowedDesignatorTypes.Contains(type))
			{
				this.disallowedDesignatorTypes.Add(type);
			}
			Find.ReverseDesignatorDatabase.Reinit();
		}

		// Token: 0x060059F3 RID: 23027 RVA: 0x0003E72F File Offset: 0x0003C92F
		public void SetAllowBuilding(ThingDef building, bool allowed)
		{
			if (allowed && this.disallowedBuildings.Contains(building))
			{
				this.disallowedBuildings.Remove(building);
			}
			if (!allowed && !this.disallowedBuildings.Contains(building))
			{
				this.disallowedBuildings.Add(building);
			}
		}

		// Token: 0x060059F4 RID: 23028 RVA: 0x001D4124 File Offset: 0x001D2324
		public bool DesignatorAllowed(Designator d)
		{
			Designator_Place designator_Place = d as Designator_Place;
			if (designator_Place != null)
			{
				return !this.disallowedBuildings.Contains(designator_Place.PlacingDef);
			}
			return !this.disallowedDesignatorTypes.Contains(d.GetType());
		}

		// Token: 0x060059F5 RID: 23029 RVA: 0x0003E76D File Offset: 0x0003C96D
		public void ExposeData()
		{
			Scribe_Collections.Look<ThingDef>(ref this.disallowedBuildings, "disallowedBuildings", LookMode.Undefined);
			Scribe_Collections.Look<Type>(ref this.disallowedDesignatorTypes, "disallowedDesignatorTypes", LookMode.Undefined);
		}

		// Token: 0x04003C90 RID: 15504
		private HashSet<Type> disallowedDesignatorTypes = new HashSet<Type>();

		// Token: 0x04003C91 RID: 15505
		private HashSet<ThingDef> disallowedBuildings = new HashSet<ThingDef>();
	}
}
