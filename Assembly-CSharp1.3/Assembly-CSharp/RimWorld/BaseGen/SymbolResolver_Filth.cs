using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020015BD RID: 5565
	public class SymbolResolver_Filth : SymbolResolver
	{
		// Token: 0x06008321 RID: 33569 RVA: 0x002EB378 File Offset: 0x002E9578
		public override bool CanResolve(ResolveParams rp)
		{
			return base.CanResolve(rp) && rp.filthDef != null && rp.filthDensity != null;
		}

		// Token: 0x06008322 RID: 33570 RVA: 0x002EB39C File Offset: 0x002E959C
		public override void Resolve(ResolveParams rp)
		{
			foreach (IntVec3 intVec in rp.rect)
			{
				if (this.CanPlaceFilth(intVec, rp))
				{
					float num;
					for (num = rp.filthDensity.Value.RandomInRange; num > 1f; num -= 1f)
					{
						FilthMaker.TryMakeFilth(intVec, BaseGen.globalSettings.map, rp.filthDef, 1, FilthSourceFlags.None);
					}
					if (Rand.Chance(num))
					{
						FilthMaker.TryMakeFilth(intVec, BaseGen.globalSettings.map, rp.filthDef, 1, FilthSourceFlags.None);
					}
				}
			}
		}

		// Token: 0x06008323 RID: 33571 RVA: 0x002EB454 File Offset: 0x002E9654
		private bool CanPlaceFilth(IntVec3 cell, ResolveParams rp)
		{
			return rp.ignoreDoorways == null || !rp.ignoreDoorways.Value || cell.GetDoor(BaseGen.globalSettings.map) == null;
		}
	}
}
