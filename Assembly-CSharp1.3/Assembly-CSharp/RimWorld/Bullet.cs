using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020010C2 RID: 4290
	public class Bullet : Projectile
	{
		// Token: 0x1700119F RID: 4511
		// (get) Token: 0x060066AE RID: 26286 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool AnimalsFleeImpact
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060066AF RID: 26287 RVA: 0x0022B044 File Offset: 0x00229244
		protected override void Impact(Thing hitThing)
		{
			Map map = base.Map;
			IntVec3 position = base.Position;
			base.Impact(hitThing);
			BattleLogEntry_RangedImpact battleLogEntry_RangedImpact = new BattleLogEntry_RangedImpact(this.launcher, hitThing, this.intendedTarget.Thing, this.equipmentDef, this.def, this.targetCoverDef);
			Find.BattleLog.Add(battleLogEntry_RangedImpact);
			this.NotifyImpact(hitThing, map, position);
			if (hitThing != null)
			{
				DamageInfo dinfo = new DamageInfo(this.def.projectile.damageDef, (float)base.DamageAmount, base.ArmorPenetration, this.ExactRotation.eulerAngles.y, this.launcher, null, this.equipmentDef, DamageInfo.SourceCategory.ThingOrUnknown, this.intendedTarget.Thing, true, true);
				hitThing.TakeDamage(dinfo).AssociateWithLog(battleLogEntry_RangedImpact);
				Pawn pawn = hitThing as Pawn;
				if (pawn != null && pawn.stances != null && pawn.BodySize <= this.def.projectile.StoppingPower + 0.001f)
				{
					pawn.stances.StaggerFor(95);
				}
				if (this.def.projectile.extraDamages == null)
				{
					return;
				}
				using (List<ExtraDamage>.Enumerator enumerator = this.def.projectile.extraDamages.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ExtraDamage extraDamage = enumerator.Current;
						if (Rand.Chance(extraDamage.chance))
						{
							DamageInfo dinfo2 = new DamageInfo(extraDamage.def, extraDamage.amount, extraDamage.AdjustedArmorPenetration(), this.ExactRotation.eulerAngles.y, this.launcher, null, this.equipmentDef, DamageInfo.SourceCategory.ThingOrUnknown, this.intendedTarget.Thing, true, true);
							hitThing.TakeDamage(dinfo2).AssociateWithLog(battleLogEntry_RangedImpact);
						}
					}
					return;
				}
			}
			SoundDefOf.BulletImpact_Ground.PlayOneShot(new TargetInfo(base.Position, map, false));
			if (base.Position.GetTerrain(map).takeSplashes)
			{
				FleckMaker.WaterSplash(this.ExactPosition, map, Mathf.Sqrt((float)base.DamageAmount) * 1f, 4f);
				return;
			}
			FleckMaker.Static(this.ExactPosition, map, FleckDefOf.ShotHit_Dirt, 1f);
		}

		// Token: 0x060066B0 RID: 26288 RVA: 0x0022B27C File Offset: 0x0022947C
		private void NotifyImpact(Thing hitThing, Map map, IntVec3 position)
		{
			BulletImpactData impactData = new BulletImpactData
			{
				bullet = this,
				hitThing = hitThing,
				impactPosition = position
			};
			if (hitThing != null)
			{
				hitThing.Notify_BulletImpactNearby(impactData);
			}
			int num = 9;
			for (int i = 0; i < num; i++)
			{
				IntVec3 c = position + GenRadial.RadialPattern[i];
				if (c.InBounds(map))
				{
					List<Thing> thingList = c.GetThingList(map);
					for (int j = 0; j < thingList.Count; j++)
					{
						if (thingList[j] != hitThing)
						{
							thingList[j].Notify_BulletImpactNearby(impactData);
						}
					}
				}
			}
		}
	}
}
