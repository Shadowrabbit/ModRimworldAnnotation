using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x0200036F RID: 879
	[StaticConstructorOnStartup]
	public abstract class Projectile : ThingWithComps
	{
		// Token: 0x1700050D RID: 1293
		// (get) Token: 0x060018DB RID: 6363 RVA: 0x000927F9 File Offset: 0x000909F9
		// (set) Token: 0x060018DC RID: 6364 RVA: 0x00092829 File Offset: 0x00090A29
		public ProjectileHitFlags HitFlags
		{
			get
			{
				if (this.def.projectile.alwaysFreeIntercept)
				{
					return ProjectileHitFlags.All;
				}
				if (this.def.projectile.flyOverhead)
				{
					return ProjectileHitFlags.None;
				}
				return this.desiredHitFlags;
			}
			set
			{
				this.desiredHitFlags = value;
			}
		}

		// Token: 0x1700050E RID: 1294
		// (get) Token: 0x060018DD RID: 6365 RVA: 0x00092834 File Offset: 0x00090A34
		protected float StartingTicksToImpact
		{
			get
			{
				float num = (this.origin - this.destination).magnitude / this.def.projectile.SpeedTilesPerTick;
				if (num <= 0f)
				{
					num = 0.001f;
				}
				return num;
			}
		}

		// Token: 0x1700050F RID: 1295
		// (get) Token: 0x060018DE RID: 6366 RVA: 0x0009287B File Offset: 0x00090A7B
		protected IntVec3 DestinationCell
		{
			get
			{
				return new IntVec3(this.destination);
			}
		}

		// Token: 0x17000510 RID: 1296
		// (get) Token: 0x060018DF RID: 6367 RVA: 0x00092888 File Offset: 0x00090A88
		public virtual Vector3 ExactPosition
		{
			get
			{
				Vector3 b = (this.destination - this.origin).Yto0() * this.DistanceCoveredFraction;
				return this.origin.Yto0() + b + Vector3.up * this.def.Altitude;
			}
		}

		// Token: 0x17000511 RID: 1297
		// (get) Token: 0x060018E0 RID: 6368 RVA: 0x000928E2 File Offset: 0x00090AE2
		protected float DistanceCoveredFraction
		{
			get
			{
				return Mathf.Clamp01(1f - (float)this.ticksToImpact / this.StartingTicksToImpact);
			}
		}

		// Token: 0x17000512 RID: 1298
		// (get) Token: 0x060018E1 RID: 6369 RVA: 0x000928FD File Offset: 0x00090AFD
		public virtual Quaternion ExactRotation
		{
			get
			{
				return Quaternion.LookRotation((this.destination - this.origin).Yto0());
			}
		}

		// Token: 0x17000513 RID: 1299
		// (get) Token: 0x060018E2 RID: 6370 RVA: 0x0001276E File Offset: 0x0001096E
		public virtual bool AnimalsFleeImpact
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000514 RID: 1300
		// (get) Token: 0x060018E3 RID: 6371 RVA: 0x0009291A File Offset: 0x00090B1A
		public override Vector3 DrawPos
		{
			get
			{
				return this.ExactPosition;
			}
		}

		// Token: 0x17000515 RID: 1301
		// (get) Token: 0x060018E4 RID: 6372 RVA: 0x00092922 File Offset: 0x00090B22
		public int DamageAmount
		{
			get
			{
				return this.def.projectile.GetDamageAmount(this.weaponDamageMultiplier, null);
			}
		}

		// Token: 0x17000516 RID: 1302
		// (get) Token: 0x060018E5 RID: 6373 RVA: 0x0009293B File Offset: 0x00090B3B
		public float ArmorPenetration
		{
			get
			{
				return this.def.projectile.GetArmorPenetration(this.weaponDamageMultiplier, null);
			}
		}

		// Token: 0x17000517 RID: 1303
		// (get) Token: 0x060018E6 RID: 6374 RVA: 0x00092954 File Offset: 0x00090B54
		public ThingDef EquipmentDef
		{
			get
			{
				return this.equipmentDef;
			}
		}

		// Token: 0x17000518 RID: 1304
		// (get) Token: 0x060018E7 RID: 6375 RVA: 0x0009295C File Offset: 0x00090B5C
		public Thing Launcher
		{
			get
			{
				return this.launcher;
			}
		}

		// Token: 0x060018E8 RID: 6376 RVA: 0x00092964 File Offset: 0x00090B64
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<Vector3>(ref this.origin, "origin", default(Vector3), false);
			Scribe_Values.Look<Vector3>(ref this.destination, "destination", default(Vector3), false);
			Scribe_Values.Look<int>(ref this.ticksToImpact, "ticksToImpact", 0, false);
			Scribe_TargetInfo.Look(ref this.usedTarget, "usedTarget");
			Scribe_TargetInfo.Look(ref this.intendedTarget, "intendedTarget");
			Scribe_References.Look<Thing>(ref this.launcher, "launcher", false);
			Scribe_Defs.Look<ThingDef>(ref this.equipmentDef, "equipmentDef");
			Scribe_Defs.Look<ThingDef>(ref this.targetCoverDef, "targetCoverDef");
			Scribe_Values.Look<ProjectileHitFlags>(ref this.desiredHitFlags, "desiredHitFlags", ProjectileHitFlags.All, false);
			Scribe_Values.Look<float>(ref this.weaponDamageMultiplier, "weaponDamageMultiplier", 1f, false);
			Scribe_Values.Look<bool>(ref this.preventFriendlyFire, "preventFriendlyFire", false, false);
			Scribe_Values.Look<bool>(ref this.landed, "landed", false, false);
		}

		// Token: 0x060018E9 RID: 6377 RVA: 0x00092A5C File Offset: 0x00090C5C
		public void Launch(Thing launcher, LocalTargetInfo usedTarget, LocalTargetInfo intendedTarget, ProjectileHitFlags hitFlags, bool preventFriendlyFire = false, Thing equipment = null)
		{
			this.Launch(launcher, base.Position.ToVector3Shifted(), usedTarget, intendedTarget, hitFlags, preventFriendlyFire, equipment, null);
		}

		// Token: 0x060018EA RID: 6378 RVA: 0x00092A88 File Offset: 0x00090C88
		public void Launch(Thing launcher, Vector3 origin, LocalTargetInfo usedTarget, LocalTargetInfo intendedTarget, ProjectileHitFlags hitFlags, bool preventFriendlyFire = false, Thing equipment = null, ThingDef targetCoverDef = null)
		{
			this.launcher = launcher;
			this.origin = origin;
			this.usedTarget = usedTarget;
			this.intendedTarget = intendedTarget;
			this.targetCoverDef = targetCoverDef;
			this.preventFriendlyFire = preventFriendlyFire;
			this.HitFlags = hitFlags;
			if (equipment != null)
			{
				this.equipmentDef = equipment.def;
				this.weaponDamageMultiplier = equipment.GetStatValue(StatDefOf.RangedWeapon_DamageMultiplier, true);
			}
			else
			{
				this.equipmentDef = null;
				this.weaponDamageMultiplier = 1f;
			}
			this.destination = usedTarget.Cell.ToVector3Shifted() + Gen.RandomHorizontalVector(0.3f);
			this.ticksToImpact = Mathf.CeilToInt(this.StartingTicksToImpact);
			if (this.ticksToImpact < 1)
			{
				this.ticksToImpact = 1;
			}
			if (!this.def.projectile.soundAmbient.NullOrUndefined())
			{
				SoundInfo info = SoundInfo.InMap(this, MaintenanceType.PerTick);
				this.ambientSustainer = this.def.projectile.soundAmbient.TrySpawnSustainer(info);
			}
		}

		// Token: 0x060018EB RID: 6379 RVA: 0x00092B88 File Offset: 0x00090D88
		public override void Tick()
		{
			base.Tick();
			if (this.landed)
			{
				return;
			}
			Vector3 exactPosition = this.ExactPosition;
			this.ticksToImpact--;
			if (!this.ExactPosition.InBounds(base.Map))
			{
				this.ticksToImpact++;
				base.Position = this.ExactPosition.ToIntVec3();
				this.Destroy(DestroyMode.Vanish);
				return;
			}
			Vector3 exactPosition2 = this.ExactPosition;
			if (this.CheckForFreeInterceptBetween(exactPosition, exactPosition2))
			{
				return;
			}
			base.Position = this.ExactPosition.ToIntVec3();
			if (this.ticksToImpact == 60 && Find.TickManager.CurTimeSpeed == TimeSpeed.Normal && this.def.projectile.soundImpactAnticipate != null)
			{
				this.def.projectile.soundImpactAnticipate.PlayOneShot(this);
			}
			if (this.ticksToImpact <= 0)
			{
				if (this.DestinationCell.InBounds(base.Map))
				{
					base.Position = this.DestinationCell;
				}
				this.ImpactSomething();
				return;
			}
			if (this.ambientSustainer != null)
			{
				this.ambientSustainer.Maintain();
			}
		}

		// Token: 0x060018EC RID: 6380 RVA: 0x00092C9C File Offset: 0x00090E9C
		private bool CheckForFreeInterceptBetween(Vector3 lastExactPos, Vector3 newExactPos)
		{
			if (lastExactPos == newExactPos)
			{
				return false;
			}
			List<Thing> list = base.Map.listerThings.ThingsInGroup(ThingRequestGroup.ProjectileInterceptor);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].TryGetComp<CompProjectileInterceptor>().CheckIntercept(this, lastExactPos, newExactPos))
				{
					this.Destroy(DestroyMode.Vanish);
					return true;
				}
			}
			IntVec3 intVec = lastExactPos.ToIntVec3();
			IntVec3 intVec2 = newExactPos.ToIntVec3();
			if (intVec2 == intVec)
			{
				return false;
			}
			if (!intVec.InBounds(base.Map) || !intVec2.InBounds(base.Map))
			{
				return false;
			}
			if (intVec2.AdjacentToCardinal(intVec))
			{
				return this.CheckForFreeIntercept(intVec2);
			}
			if (VerbUtility.InterceptChanceFactorFromDistance(this.origin, intVec2) <= 0f)
			{
				return false;
			}
			Vector3 vector = lastExactPos;
			Vector3 v = newExactPos - lastExactPos;
			Vector3 b = v.normalized * 0.2f;
			int num = (int)(v.MagnitudeHorizontal() / 0.2f);
			Projectile.checkedCells.Clear();
			int num2 = 0;
			for (;;)
			{
				vector += b;
				IntVec3 intVec3 = vector.ToIntVec3();
				if (!Projectile.checkedCells.Contains(intVec3))
				{
					if (this.CheckForFreeIntercept(intVec3))
					{
						break;
					}
					Projectile.checkedCells.Add(intVec3);
				}
				num2++;
				if (num2 > num)
				{
					return false;
				}
				if (intVec3 == intVec2)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060018ED RID: 6381 RVA: 0x00092DE8 File Offset: 0x00090FE8
		private bool CheckForFreeIntercept(IntVec3 c)
		{
			if (this.destination.ToIntVec3() == c)
			{
				return false;
			}
			float num = VerbUtility.InterceptChanceFactorFromDistance(this.origin, c);
			if (num <= 0f)
			{
				return false;
			}
			bool flag = false;
			List<Thing> thingList = c.GetThingList(base.Map);
			for (int i = 0; i < thingList.Count; i++)
			{
				Thing thing = thingList[i];
				if (this.CanHit(thing))
				{
					bool flag2 = false;
					if (thing.def.Fillage == FillCategory.Full)
					{
						Building_Door building_Door = thing as Building_Door;
						if (building_Door == null || !building_Door.Open)
						{
							this.ThrowDebugText("int-wall", c);
							this.Impact(thing);
							return true;
						}
						flag2 = true;
					}
					float num2 = 0f;
					Pawn pawn = thing as Pawn;
					if (pawn != null)
					{
						num2 = 0.4f * Mathf.Clamp(pawn.BodySize, 0.1f, 2f);
						if (pawn.GetPosture() != PawnPosture.Standing)
						{
							num2 *= 0.1f;
						}
						if (this.launcher != null && pawn.Faction != null && this.launcher.Faction != null && !pawn.Faction.HostileTo(this.launcher.Faction))
						{
							if (this.preventFriendlyFire)
							{
								num2 = 0f;
								this.ThrowDebugText("ff-miss", c);
							}
							else
							{
								num2 *= Find.Storyteller.difficulty.friendlyFireChanceFactor;
							}
						}
					}
					else if (thing.def.fillPercent > 0.2f)
					{
						if (flag2)
						{
							num2 = 0.05f;
						}
						else if (this.DestinationCell.AdjacentTo8Way(c))
						{
							num2 = thing.def.fillPercent * 1f;
						}
						else
						{
							num2 = thing.def.fillPercent * 0.15f;
						}
					}
					num2 *= num;
					if (num2 > 1E-05f)
					{
						if (Rand.Chance(num2))
						{
							this.ThrowDebugText("int-" + num2.ToStringPercent(), c);
							this.Impact(thing);
							return true;
						}
						flag = true;
						this.ThrowDebugText(num2.ToStringPercent(), c);
					}
				}
			}
			if (!flag)
			{
				this.ThrowDebugText("o", c);
			}
			return false;
		}

		// Token: 0x060018EE RID: 6382 RVA: 0x00093010 File Offset: 0x00091210
		private void ThrowDebugText(string text, IntVec3 c)
		{
			if (DebugViewSettings.drawShooting)
			{
				MoteMaker.ThrowText(c.ToVector3Shifted(), base.Map, text, -1f);
			}
		}

		// Token: 0x060018EF RID: 6383 RVA: 0x00093034 File Offset: 0x00091234
		public override void Draw()
		{
			float num = this.ArcHeightFactor * GenMath.InverseParabola(this.DistanceCoveredFraction);
			Vector3 drawPos = this.DrawPos;
			Vector3 position = drawPos + new Vector3(0f, 0f, 1f) * num;
			if (this.def.projectile.shadowSize > 0f)
			{
				this.DrawShadow(drawPos, num);
			}
			Graphics.DrawMesh(MeshPool.GridPlane(this.def.graphicData.drawSize), position, this.ExactRotation, this.def.DrawMatSingle, 0);
			base.Comps_PostDraw();
		}

		// Token: 0x17000519 RID: 1305
		// (get) Token: 0x060018F0 RID: 6384 RVA: 0x000930D0 File Offset: 0x000912D0
		private float ArcHeightFactor
		{
			get
			{
				float num = this.def.projectile.arcHeightFactor;
				float num2 = (this.destination - this.origin).MagnitudeHorizontalSquared();
				if (num * num > num2 * 0.2f * 0.2f)
				{
					num = Mathf.Sqrt(num2) * 0.2f;
				}
				return num;
			}
		}

		// Token: 0x060018F1 RID: 6385 RVA: 0x00093128 File Offset: 0x00091328
		protected bool CanHit(Thing thing)
		{
			if (!thing.Spawned)
			{
				return false;
			}
			if (thing == this.launcher)
			{
				return false;
			}
			ProjectileHitFlags hitFlags = this.HitFlags;
			if (hitFlags == ProjectileHitFlags.None)
			{
				return false;
			}
			bool flag = false;
			foreach (IntVec3 c in thing.OccupiedRect())
			{
				List<Thing> thingList = c.GetThingList(base.Map);
				bool flag2 = false;
				for (int i = 0; i < thingList.Count; i++)
				{
					if (thingList[i] != thing && thingList[i].def.Fillage == FillCategory.Full && thingList[i].def.Altitude >= thing.def.Altitude)
					{
						flag2 = true;
						break;
					}
				}
				if (!flag2)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				return false;
			}
			if (thing == this.intendedTarget && (hitFlags & ProjectileHitFlags.IntendedTarget) != ProjectileHitFlags.None)
			{
				return true;
			}
			if (thing != this.intendedTarget)
			{
				if (thing is Pawn)
				{
					if ((hitFlags & ProjectileHitFlags.NonTargetPawns) != ProjectileHitFlags.None)
					{
						return true;
					}
				}
				else if ((hitFlags & ProjectileHitFlags.NonTargetWorld) != ProjectileHitFlags.None)
				{
					return true;
				}
			}
			return thing == this.intendedTarget && thing.def.Fillage == FillCategory.Full;
		}

		// Token: 0x060018F2 RID: 6386 RVA: 0x00093280 File Offset: 0x00091480
		private void ImpactSomething()
		{
			if (this.def.projectile.flyOverhead)
			{
				RoofDef roofDef = base.Map.roofGrid.RoofAt(base.Position);
				if (roofDef != null)
				{
					if (roofDef.isThickRoof)
					{
						this.ThrowDebugText("hit-thick-roof", base.Position);
						this.def.projectile.soundHitThickRoof.PlayOneShot(new TargetInfo(base.Position, base.Map, false));
						this.Destroy(DestroyMode.Vanish);
						return;
					}
					if (base.Position.GetEdifice(base.Map) == null || base.Position.GetEdifice(base.Map).def.Fillage != FillCategory.Full)
					{
						RoofCollapserImmediate.DropRoofInCells(base.Position, base.Map, null);
					}
				}
			}
			if (!this.usedTarget.HasThing || !this.CanHit(this.usedTarget.Thing))
			{
				Projectile.cellThingsFiltered.Clear();
				List<Thing> thingList = base.Position.GetThingList(base.Map);
				for (int i = 0; i < thingList.Count; i++)
				{
					Thing thing = thingList[i];
					if ((thing.def.category == ThingCategory.Building || thing.def.category == ThingCategory.Pawn || thing.def.category == ThingCategory.Item || thing.def.category == ThingCategory.Plant) && this.CanHit(thing))
					{
						Projectile.cellThingsFiltered.Add(thing);
					}
				}
				Projectile.cellThingsFiltered.Shuffle<Thing>();
				for (int j = 0; j < Projectile.cellThingsFiltered.Count; j++)
				{
					Thing thing2 = Projectile.cellThingsFiltered[j];
					Pawn pawn = thing2 as Pawn;
					float num;
					if (pawn != null)
					{
						num = 0.5f * Mathf.Clamp(pawn.BodySize, 0.1f, 2f);
						if (pawn.GetPosture() != PawnPosture.Standing && (this.origin - this.destination).MagnitudeHorizontalSquared() >= 20.25f)
						{
							num *= 0.2f;
						}
						if (this.launcher != null && pawn.Faction != null && this.launcher.Faction != null && !pawn.Faction.HostileTo(this.launcher.Faction))
						{
							num *= VerbUtility.InterceptChanceFactorFromDistance(this.origin, base.Position);
						}
					}
					else
					{
						num = 1.5f * thing2.def.fillPercent;
					}
					if (Rand.Chance(num))
					{
						this.ThrowDebugText("hit-" + num.ToStringPercent(), base.Position);
						this.Impact(Projectile.cellThingsFiltered.RandomElement<Thing>());
						return;
					}
					this.ThrowDebugText("miss-" + num.ToStringPercent(), base.Position);
				}
				this.Impact(null);
				return;
			}
			Pawn pawn2 = this.usedTarget.Thing as Pawn;
			if (pawn2 != null && pawn2.GetPosture() != PawnPosture.Standing && (this.origin - this.destination).MagnitudeHorizontalSquared() >= 20.25f && !Rand.Chance(0.2f))
			{
				this.ThrowDebugText("miss-laying", base.Position);
				this.Impact(null);
				return;
			}
			this.Impact(this.usedTarget.Thing);
		}

		// Token: 0x060018F3 RID: 6387 RVA: 0x000935BD File Offset: 0x000917BD
		protected virtual void Impact(Thing hitThing)
		{
			GenClamor.DoClamor(this, 12f, ClamorDefOf.Impact);
			this.Destroy(DestroyMode.Vanish);
		}

		// Token: 0x060018F4 RID: 6388 RVA: 0x000935D8 File Offset: 0x000917D8
		private void DrawShadow(Vector3 drawLoc, float height)
		{
			if (Projectile.shadowMaterial == null)
			{
				return;
			}
			float num = this.def.projectile.shadowSize * Mathf.Lerp(1f, 0.6f, height);
			Vector3 s = new Vector3(num, 1f, num);
			Vector3 b = new Vector3(0f, -0.01f, 0f);
			Matrix4x4 matrix = default(Matrix4x4);
			matrix.SetTRS(drawLoc + b, Quaternion.identity, s);
			Graphics.DrawMesh(MeshPool.plane10, matrix, Projectile.shadowMaterial, 0);
		}

		// Token: 0x040010D7 RID: 4311
		private static readonly Material shadowMaterial = MaterialPool.MatFrom("Things/Skyfaller/SkyfallerShadowCircle", ShaderDatabase.Transparent);

		// Token: 0x040010D8 RID: 4312
		protected Vector3 origin;

		// Token: 0x040010D9 RID: 4313
		protected Vector3 destination;

		// Token: 0x040010DA RID: 4314
		public LocalTargetInfo usedTarget;

		// Token: 0x040010DB RID: 4315
		public LocalTargetInfo intendedTarget;

		// Token: 0x040010DC RID: 4316
		protected ThingDef equipmentDef;

		// Token: 0x040010DD RID: 4317
		protected Thing launcher;

		// Token: 0x040010DE RID: 4318
		protected ThingDef targetCoverDef;

		// Token: 0x040010DF RID: 4319
		private ProjectileHitFlags desiredHitFlags = ProjectileHitFlags.All;

		// Token: 0x040010E0 RID: 4320
		protected float weaponDamageMultiplier = 1f;

		// Token: 0x040010E1 RID: 4321
		protected bool preventFriendlyFire;

		// Token: 0x040010E2 RID: 4322
		protected bool landed;

		// Token: 0x040010E3 RID: 4323
		protected int ticksToImpact;

		// Token: 0x040010E4 RID: 4324
		private Sustainer ambientSustainer;

		// Token: 0x040010E5 RID: 4325
		private static List<IntVec3> checkedCells = new List<IntVec3>();

		// Token: 0x040010E6 RID: 4326
		private static readonly List<Thing> cellThingsFiltered = new List<Thing>();
	}
}
