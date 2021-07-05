using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AF2 RID: 2802
	public class GameRules : IExposable
	{
		// Token: 0x060041EB RID: 16875 RVA: 0x001615A4 File Offset: 0x0015F7A4
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

		// Token: 0x060041EC RID: 16876 RVA: 0x001615F7 File Offset: 0x0015F7F7
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

		// Token: 0x060041ED RID: 16877 RVA: 0x00161638 File Offset: 0x0015F838
		public bool DesignatorAllowed(Designator d)
		{
			Designator_Place designator_Place = d as Designator_Place;
			if (designator_Place != null)
			{
				return !this.disallowedBuildings.Contains(designator_Place.PlacingDef);
			}
			return !this.disallowedDesignatorTypes.Contains(d.GetType());
		}

		// Token: 0x060041EE RID: 16878 RVA: 0x00161678 File Offset: 0x0015F878
		public void ExposeData()
		{
			Scribe_Collections.Look<ThingDef>(ref this.disallowedBuildings, "disallowedBuildings", LookMode.Undefined);
			Scribe_Collections.Look<Type>(ref this.disallowedDesignatorTypes, "disallowedDesignatorTypes", LookMode.Undefined);
		}

		// Token: 0x0400282F RID: 10287
		private HashSet<Type> disallowedDesignatorTypes = new HashSet<Type>();

		// Token: 0x04002830 RID: 10288
		private HashSet<ThingDef> disallowedBuildings = new HashSet<ThingDef>();
	}
}
