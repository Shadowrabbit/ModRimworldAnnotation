using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020010D1 RID: 4305
	[StaticConstructorOnStartup]
	public class Skyfaller : ThingWithComps, IThingHolder
	{
		// Token: 0x170011AD RID: 4525
		// (get) Token: 0x060066FB RID: 26363 RVA: 0x0022C9DD File Offset: 0x0022ABDD
		public int LeaveMapAfterTicks
		{
			get
			{
				if (this.ticksToDiscard <= 0)
				{
					return 220;
				}
				return this.ticksToDiscard;
			}
		}

		// Token: 0x170011AE RID: 4526
		// (get) Token: 0x060066FC RID: 26364 RVA: 0x0022C9F4 File Offset: 0x0022ABF4
		public CompSkyfallerRandomizeDirection RandomizeDirectionComp
		{
			get
			{
				return this.randomizeDirectionComp;
			}
		}

		// Token: 0x060066FD RID: 26365 RVA: 0x0022C9FC File Offset: 0x0022ABFC
		public override void PostPostMake()
		{
			base.PostPostMake();
			this.randomizeDirectionComp = base.GetComp<CompSkyfallerRandomizeDirection>();
		}

		// Token: 0x170011AF RID: 4527
		// (get) Token: 0x060066FE RID: 26366 RVA: 0x0022CA10 File Offset: 0x0022AC10
		public override Graphic Graphic
		{
			get
			{
				Thing thingForGraphic = this.GetThingForGraphic();
				if (this.def.skyfaller.fadeInTicks > 0 || this.def.skyfaller.fadeOutTicks > 0)
				{
					return this.def.graphicData.GraphicColoredFor(thingForGraphic);
				}
				if (thingForGraphic == this)
				{
					return base.Graphic;
				}
				return thingForGraphic.Graphic.ExtractInnerGraphicFor(thingForGraphic).GetShadowlessGraphic();
			}
		}

		// Token: 0x170011B0 RID: 4528
		// (get) Token: 0x060066FF RID: 26367 RVA: 0x0022CA78 File Offset: 0x0022AC78
		public override Vector3 DrawPos
		{
			get
			{
				switch (this.def.skyfaller.movementType)
				{
				case SkyfallerMovementType.Accelerate:
					return SkyfallerDrawPosUtility.DrawPos_Accelerate(base.DrawPos, this.ticksToImpact, this.angle, this.CurrentSpeed, this.randomizeDirectionComp);
				case SkyfallerMovementType.ConstantSpeed:
					return SkyfallerDrawPosUtility.DrawPos_ConstantSpeed(base.DrawPos, this.ticksToImpact, this.angle, this.CurrentSpeed, this.randomizeDirectionComp);
				case SkyfallerMovementType.Decelerate:
					return SkyfallerDrawPosUtility.DrawPos_Decelerate(base.DrawPos, this.ticksToImpact, this.angle, this.CurrentSpeed, this.randomizeDirectionComp);
				default:
					Log.ErrorOnce("SkyfallerMovementType not handled: " + this.def.skyfaller.movementType, this.thingIDNumber ^ 1948576711);
					return SkyfallerDrawPosUtility.DrawPos_Accelerate(base.DrawPos, this.ticksToImpact, this.angle, this.CurrentSpeed, this.randomizeDirectionComp);
				}
			}
		}

		// Token: 0x170011B1 RID: 4529
		// (get) Token: 0x06006700 RID: 26368 RVA: 0x0022CB6C File Offset: 0x0022AD6C
		// (set) Token: 0x06006701 RID: 26369 RVA: 0x0022CC57 File Offset: 0x0022AE57
		public override Color DrawColor
		{
			get
			{
				if (this.def.skyfaller.fadeInTicks > 0 && this.ageTicks < this.def.skyfaller.fadeInTicks)
				{
					Color drawColor = base.DrawColor;
					drawColor.a *= Mathf.Lerp(0f, 1f, Mathf.Min((float)this.ageTicks / (float)this.def.skyfaller.fadeInTicks, 1f));
					return drawColor;
				}
				if (this.FadingOut)
				{
					Color drawColor2 = base.DrawColor;
					drawColor2.a *= Mathf.Lerp(1f, 0f, Mathf.Max((float)this.ageTicks - (float)(this.LeaveMapAfterTicks - this.def.skyfaller.fadeOutTicks), 0f) / (float)this.def.skyfaller.fadeOutTicks);
					return drawColor2;
				}
				return base.DrawColor;
			}
			set
			{
				base.DrawColor = value;
			}
		}

		// Token: 0x170011B2 RID: 4530
		// (get) Token: 0x06006702 RID: 26370 RVA: 0x0022CC60 File Offset: 0x0022AE60
		public bool FadingOut
		{
			get
			{
				return this.def.skyfaller.fadeOutTicks > 0 && this.ageTicks >= this.LeaveMapAfterTicks - this.def.skyfaller.fadeOutTicks;
			}
		}

		// Token: 0x170011B3 RID: 4531
		// (get) Token: 0x06006703 RID: 26371 RVA: 0x0022CC9C File Offset: 0x0022AE9C
		private Material ShadowMaterial
		{
			get
			{
				if (this.cachedShadowMaterial == null && !this.def.skyfaller.shadow.NullOrEmpty())
				{
					this.cachedShadowMaterial = MaterialPool.MatFrom(this.def.skyfaller.shadow, ShaderDatabase.Transparent);
				}
				return this.cachedShadowMaterial;
			}
		}

		// Token: 0x170011B4 RID: 4532
		// (get) Token: 0x06006704 RID: 26372 RVA: 0x0022CCF4 File Offset: 0x0022AEF4
		protected float TimeInAnimation
		{
			get
			{
				if (this.def.skyfaller.reversed)
				{
					return (float)this.ticksToImpact / (float)this.LeaveMapAfterTicks;
				}
				return 1f - (float)this.ticksToImpact / (float)this.ticksToImpactMax;
			}
		}

		// Token: 0x170011B5 RID: 4533
		// (get) Token: 0x06006705 RID: 26373 RVA: 0x0022CD30 File Offset: 0x0022AF30
		private float CurrentSpeed
		{
			get
			{
				if (this.def.skyfaller.speedCurve == null)
				{
					return this.def.skyfaller.speed;
				}
				return this.def.skyfaller.speedCurve.Evaluate(this.TimeInAnimation) * this.def.skyfaller.speed;
			}
		}

		// Token: 0x170011B6 RID: 4534
		// (get) Token: 0x06006706 RID: 26374 RVA: 0x0022CD8C File Offset: 0x0022AF8C
		private bool SpawnTimedMotes
		{
			get
			{
				return this.def.skyfaller.moteSpawnTime != float.MinValue && Mathf.Approximately(this.def.skyfaller.moteSpawnTime, this.TimeInAnimation);
			}
		}

		// Token: 0x06006707 RID: 26375 RVA: 0x0022CDC2 File Offset: 0x0022AFC2
		public Skyfaller()
		{
			this.innerContainer = new ThingOwner<Thing>(this);
		}

		// Token: 0x06006708 RID: 26376 RVA: 0x0022CDE4 File Offset: 0x0022AFE4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<ThingOwner>(ref this.innerContainer, "innerContainer", new object[]
			{
				this
			});
			Scribe_Values.Look<int>(ref this.ticksToImpact, "ticksToImpact", 0, false);
			Scribe_Values.Look<int>(ref this.ticksToDiscard, "ticksToDiscard", 0, false);
			Scribe_Values.Look<int>(ref this.ageTicks, "ageTicks", 0, false);
			Scribe_Values.Look<int>(ref this.ticksToImpactMax, "ticksToImpactMax", this.LeaveMapAfterTicks, false);
			Scribe_Values.Look<float>(ref this.angle, "angle", 0f, false);
			Scribe_Values.Look<float>(ref this.shrapnelDirection, "shrapnelDirection", 0f, false);
		}

		// Token: 0x06006709 RID: 26377 RVA: 0x0022CE8A File Offset: 0x0022B08A
		public override void PostMake()
		{
			base.PostMake();
			if (this.def.skyfaller.MakesShrapnel)
			{
				this.shrapnelDirection = Rand.Range(0f, 360f);
			}
		}

		// Token: 0x0600670A RID: 26378 RVA: 0x0022CEBC File Offset: 0x0022B0BC
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (!respawningAfterLoad)
			{
				this.ticksToImpact = (this.ticksToImpactMax = this.def.skyfaller.ticksToImpactRange.RandomInRange);
				this.ticksToDiscard = ((this.def.skyfaller.ticksToDiscardInReverse != IntRange.zero) ? this.def.skyfaller.ticksToDiscardInReverse.RandomInRange : -1);
				if (this.def.skyfaller.MakesShrapnel)
				{
					float num = GenMath.PositiveMod(this.shrapnelDirection, 360f);
					if (num < 270f && num >= 90f)
					{
						this.angle = Rand.Range(0f, 33f);
					}
					else
					{
						this.angle = Rand.Range(-33f, 0f);
					}
				}
				else if (this.def.skyfaller.angleCurve != null)
				{
					this.angle = this.def.skyfaller.angleCurve.Evaluate(0f);
				}
				else
				{
					this.angle = -33.7f;
				}
				if (this.def.rotatable && this.innerContainer.Any)
				{
					base.Rotation = this.innerContainer[0].Rotation;
				}
			}
		}

		// Token: 0x0600670B RID: 26379 RVA: 0x0022D006 File Offset: 0x0022B206
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			base.Destroy(mode);
			this.innerContainer.ClearAndDestroyContents(DestroyMode.Vanish);
		}

		// Token: 0x0600670C RID: 26380 RVA: 0x0022D01C File Offset: 0x0022B21C
		public override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			Thing thingForGraphic = this.GetThingForGraphic();
			float num = 0f;
			if (this.def.skyfaller.rotateGraphicTowardsDirection)
			{
				num = this.angle;
			}
			if (this.randomizeDirectionComp != null)
			{
				num += this.randomizeDirectionComp.ExtraDrawAngle;
			}
			if (this.def.skyfaller.angleCurve != null)
			{
				this.angle = this.def.skyfaller.angleCurve.Evaluate(this.TimeInAnimation);
			}
			if (this.def.skyfaller.rotationCurve != null)
			{
				num += this.def.skyfaller.rotationCurve.Evaluate(this.TimeInAnimation);
			}
			if (this.def.skyfaller.xPositionCurve != null)
			{
				drawLoc.x += this.def.skyfaller.xPositionCurve.Evaluate(this.TimeInAnimation);
			}
			if (this.def.skyfaller.zPositionCurve != null)
			{
				drawLoc.z += this.def.skyfaller.zPositionCurve.Evaluate(this.TimeInAnimation);
			}
			this.Graphic.Draw(drawLoc, flip ? thingForGraphic.Rotation.Opposite : thingForGraphic.Rotation, thingForGraphic, num);
			this.DrawDropSpotShadow();
		}

		// Token: 0x0600670D RID: 26381 RVA: 0x0022D168 File Offset: 0x0022B368
		public float DrawAngle()
		{
			float num = 0f;
			if (this.def.skyfaller.rotateGraphicTowardsDirection)
			{
				num = this.angle;
			}
			num += this.def.skyfaller.rotationCurve.Evaluate(this.TimeInAnimation);
			if (this.randomizeDirectionComp != null)
			{
				num += this.randomizeDirectionComp.ExtraDrawAngle;
			}
			return num;
		}

		// Token: 0x0600670E RID: 26382 RVA: 0x0022D1CC File Offset: 0x0022B3CC
		public override void Tick()
		{
			base.Tick();
			this.innerContainer.ThingOwnerTick(true);
			if (this.SpawnTimedMotes)
			{
				CellRect cellRect = this.OccupiedRect();
				for (int i = 0; i < cellRect.Area * this.def.skyfaller.motesPerCell; i++)
				{
					FleckMaker.ThrowDustPuff(cellRect.RandomVector3, base.Map, 2f);
				}
			}
			if (this.def.skyfaller.floatingSound != null && (this.floatingSoundPlaying == null || this.floatingSoundPlaying.Ended))
			{
				this.floatingSoundPlaying = this.def.skyfaller.floatingSound.TrySpawnSustainer(SoundInfo.InMap(new TargetInfo(this), MaintenanceType.PerTick));
			}
			Sustainer sustainer = this.floatingSoundPlaying;
			if (sustainer != null)
			{
				sustainer.Maintain();
			}
			if (this.def.skyfaller.reversed)
			{
				this.ticksToImpact++;
				if (!this.anticipationSoundPlayed && this.def.skyfaller.anticipationSound != null && this.ticksToImpact > this.def.skyfaller.anticipationSoundTicks)
				{
					this.anticipationSoundPlayed = true;
					this.def.skyfaller.anticipationSound.PlayOneShot(new TargetInfo(base.Position, base.Map, false));
				}
				if (this.ticksToImpact == this.LeaveMapAfterTicks)
				{
					this.LeaveMap();
				}
				else if (this.ticksToImpact > this.LeaveMapAfterTicks)
				{
					Log.Error("ticksToImpact > LeaveMapAfterTicks. Was there an exception? Destroying skyfaller.");
					this.Destroy(DestroyMode.Vanish);
				}
			}
			else
			{
				this.ticksToImpact--;
				if (this.ticksToImpact == 15)
				{
					this.HitRoof();
				}
				if (!this.anticipationSoundPlayed && this.def.skyfaller.anticipationSound != null && this.ticksToImpact < this.def.skyfaller.anticipationSoundTicks)
				{
					this.anticipationSoundPlayed = true;
					this.def.skyfaller.anticipationSound.PlayOneShot(new TargetInfo(base.Position, base.Map, false));
				}
				if (this.ticksToImpact == 0)
				{
					this.Impact();
				}
				else if (this.ticksToImpact < 0)
				{
					Log.Error("ticksToImpact < 0. Was there an exception? Destroying skyfaller.");
					this.Destroy(DestroyMode.Vanish);
				}
			}
			this.ageTicks++;
		}

		// Token: 0x0600670F RID: 26383 RVA: 0x0022D418 File Offset: 0x0022B618
		protected virtual void HitRoof()
		{
			if (!this.def.skyfaller.hitRoof)
			{
				return;
			}
			CellRect cr = this.OccupiedRect();
			if (cr.Cells.Any((IntVec3 x) => x.Roofed(this.Map)))
			{
				RoofDef roof = cr.Cells.First((IntVec3 x) => x.Roofed(this.Map)).GetRoof(base.Map);
				if (!roof.soundPunchThrough.NullOrUndefined())
				{
					roof.soundPunchThrough.PlayOneShot(new TargetInfo(base.Position, base.Map, false));
				}
				RoofCollapserImmediate.DropRoofInCells(cr.ExpandedBy(1).ClipInsideMap(base.Map).Cells.Where(delegate(IntVec3 c)
				{
					if (!c.InBounds(this.Map))
					{
						return false;
					}
					if (cr.Contains(c))
					{
						return true;
					}
					if (c.GetFirstPawn(this.Map) != null)
					{
						return false;
					}
					Building edifice = c.GetEdifice(this.Map);
					return edifice == null || !edifice.def.holdsRoof;
				}), base.Map, null);
			}
		}

		// Token: 0x06006710 RID: 26384 RVA: 0x0022D508 File Offset: 0x0022B708
		protected virtual void SpawnThings()
		{
			for (int i = this.innerContainer.Count - 1; i >= 0; i--)
			{
				GenPlace.TryPlaceThing(this.innerContainer[i], base.Position, base.Map, ThingPlaceMode.Near, delegate(Thing thing, int count)
				{
					PawnUtility.RecoverFromUnwalkablePositionOrKill(thing.Position, thing.Map);
					if (thing.def.Fillage == FillCategory.Full && this.def.skyfaller.CausesExplosion && this.def.skyfaller.explosionDamage.isExplosive && thing.Position.InHorDistOf(base.Position, this.def.skyfaller.explosionRadius))
					{
						base.Map.terrainGrid.Notify_TerrainDestroyed(thing.Position);
					}
				}, null, this.innerContainer[i].def.defaultPlacingRot);
			}
		}

		// Token: 0x06006711 RID: 26385 RVA: 0x0022D570 File Offset: 0x0022B770
		protected virtual void Impact()
		{
			if (this.def.skyfaller.CausesExplosion)
			{
				GenExplosion.DoExplosion(base.Position, base.Map, this.def.skyfaller.explosionRadius, this.def.skyfaller.explosionDamage, null, GenMath.RoundRandom((float)this.def.skyfaller.explosionDamage.defaultDamage * this.def.skyfaller.explosionDamageFactor), -1f, null, null, null, null, null, 0f, 1, false, null, 0f, 1, 0f, false, null, (!this.def.skyfaller.damageSpawnedThings) ? this.innerContainer.ToList<Thing>() : null);
			}
			this.SpawnThings();
			this.innerContainer.ClearAndDestroyContents(DestroyMode.Vanish);
			CellRect cellRect = this.OccupiedRect();
			for (int i = 0; i < cellRect.Area * this.def.skyfaller.motesPerCell; i++)
			{
				FleckMaker.ThrowDustPuff(cellRect.RandomVector3, base.Map, 2f);
			}
			if (this.def.skyfaller.MakesShrapnel)
			{
				SkyfallerShrapnelUtility.MakeShrapnel(base.Position, base.Map, this.shrapnelDirection, this.def.skyfaller.shrapnelDistanceFactor, this.def.skyfaller.metalShrapnelCountRange.RandomInRange, this.def.skyfaller.rubbleShrapnelCountRange.RandomInRange, true);
			}
			if (this.def.skyfaller.cameraShake > 0f && base.Map == Find.CurrentMap)
			{
				Find.CameraDriver.shaker.DoShake(this.def.skyfaller.cameraShake);
			}
			if (this.def.skyfaller.impactSound != null)
			{
				this.def.skyfaller.impactSound.PlayOneShot(SoundInfo.InMap(new TargetInfo(base.Position, base.Map, false), MaintenanceType.None));
			}
			this.Destroy(DestroyMode.Vanish);
		}

		// Token: 0x06006712 RID: 26386 RVA: 0x0022D777 File Offset: 0x0022B977
		protected virtual void LeaveMap()
		{
			this.Destroy(DestroyMode.Vanish);
		}

		// Token: 0x06006713 RID: 26387 RVA: 0x0022D780 File Offset: 0x0022B980
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.innerContainer;
		}

		// Token: 0x06006714 RID: 26388 RVA: 0x0022D788 File Offset: 0x0022B988
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x06006715 RID: 26389 RVA: 0x0022D796 File Offset: 0x0022B996
		private Thing GetThingForGraphic()
		{
			if (this.def.graphicData != null || !this.innerContainer.Any)
			{
				return this;
			}
			return this.innerContainer[0];
		}

		// Token: 0x06006716 RID: 26390 RVA: 0x0022D7C0 File Offset: 0x0022B9C0
		protected void DrawDropSpotShadow()
		{
			Material shadowMaterial = this.ShadowMaterial;
			if (shadowMaterial == null)
			{
				return;
			}
			Skyfaller.DrawDropSpotShadow(base.DrawPos, base.Rotation, shadowMaterial, this.def.skyfaller.shadowSize, this.ticksToImpact);
		}

		// Token: 0x06006717 RID: 26391 RVA: 0x0022D808 File Offset: 0x0022BA08
		public static void DrawDropSpotShadow(Vector3 center, Rot4 rot, Material material, Vector2 shadowSize, int ticksToImpact)
		{
			if (rot.IsHorizontal)
			{
				Gen.Swap<float>(ref shadowSize.x, ref shadowSize.y);
			}
			ticksToImpact = Mathf.Max(ticksToImpact, 0);
			Vector3 pos = center;
			pos.y = AltitudeLayer.Shadows.AltitudeFor();
			float num = 1f + (float)ticksToImpact / 100f;
			Vector3 s = new Vector3(num * shadowSize.x, 1f, num * shadowSize.y);
			Color white = Color.white;
			if (ticksToImpact > 150)
			{
				white.a = Mathf.InverseLerp(200f, 150f, (float)ticksToImpact);
			}
			Skyfaller.shadowPropertyBlock.SetColor(ShaderPropertyIDs.Color, white);
			Matrix4x4 matrix = default(Matrix4x4);
			matrix.SetTRS(pos, rot.AsQuat, s);
			Graphics.DrawMesh(MeshPool.plane10Back, matrix, material, 0, null, 0, Skyfaller.shadowPropertyBlock);
		}

		// Token: 0x04003A2A RID: 14890
		public ThingOwner innerContainer;

		// Token: 0x04003A2B RID: 14891
		public int ticksToImpact;

		// Token: 0x04003A2C RID: 14892
		public int ageTicks;

		// Token: 0x04003A2D RID: 14893
		public int ticksToDiscard;

		// Token: 0x04003A2E RID: 14894
		public float angle;

		// Token: 0x04003A2F RID: 14895
		public float shrapnelDirection;

		// Token: 0x04003A30 RID: 14896
		private int ticksToImpactMax = 220;

		// Token: 0x04003A31 RID: 14897
		private Material cachedShadowMaterial;

		// Token: 0x04003A32 RID: 14898
		private bool anticipationSoundPlayed;

		// Token: 0x04003A33 RID: 14899
		private Sustainer floatingSoundPlaying;

		// Token: 0x04003A34 RID: 14900
		private static MaterialPropertyBlock shadowPropertyBlock = new MaterialPropertyBlock();

		// Token: 0x04003A35 RID: 14901
		public const float DefaultAngle = -33.7f;

		// Token: 0x04003A36 RID: 14902
		private const int RoofHitPreDelay = 15;

		// Token: 0x04003A37 RID: 14903
		private const int LeaveMapAfterTicksDefault = 220;

		// Token: 0x04003A38 RID: 14904
		private CompSkyfallerRandomizeDirection randomizeDirectionComp;
	}
}
