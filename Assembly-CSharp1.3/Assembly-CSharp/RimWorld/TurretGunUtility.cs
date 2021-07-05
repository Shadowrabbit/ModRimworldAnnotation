using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001055 RID: 4181
	public static class TurretGunUtility
	{
		// Token: 0x06006316 RID: 25366 RVA: 0x00218D8A File Offset: 0x00216F8A
		public static bool NeedsShells(ThingDef turret)
		{
			return turret.category == ThingCategory.Building && turret.building.IsTurret && turret.building.turretGunDef.HasComp(typeof(CompChangeableProjectile));
		}

		// Token: 0x06006317 RID: 25367 RVA: 0x00218DC0 File Offset: 0x00216FC0
		public static ThingDef TryFindRandomShellDef(ThingDef turret, bool allowEMP = true, bool mustHarmHealth = true, TechLevel techLevel = TechLevel.Undefined, bool allowAntigrainWarhead = false, float maxMarketValue = -1f)
		{
			if (!TurretGunUtility.NeedsShells(turret))
			{
				return null;
			}
			ThingFilter fixedFilter = turret.building.turretGunDef.building.fixedStorageSettings.filter;
			ThingDef result;
			if ((from x in DefDatabase<ThingDef>.AllDefsListForReading
			where fixedFilter.Allows(x) && (allowEMP || x.projectileWhenLoaded.projectile.damageDef != DamageDefOf.EMP) && (!mustHarmHealth || x.projectileWhenLoaded.projectile.damageDef.harmsHealth) && (techLevel == TechLevel.Undefined || x.techLevel <= techLevel) && (allowAntigrainWarhead || x != ThingDefOf.Shell_AntigrainWarhead) && (maxMarketValue < 0f || x.BaseMarketValue <= maxMarketValue)
			select x).TryRandomElement(out result))
			{
				return result;
			}
			return null;
		}
	}
}
