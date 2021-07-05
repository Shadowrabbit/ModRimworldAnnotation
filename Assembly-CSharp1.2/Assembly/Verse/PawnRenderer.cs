using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.AI.Group;

namespace Verse
{
	// Token: 0x0200038A RID: 906
	public class PawnRenderer
	{
		// Token: 0x17000424 RID: 1060
		// (get) Token: 0x060016AC RID: 5804 RVA: 0x0001610C File Offset: 0x0001430C
		private RotDrawMode CurRotDrawMode
		{
			get
			{
				if (this.pawn.Dead && this.pawn.Corpse != null)
				{
					return this.pawn.Corpse.CurRotDrawMode;
				}
				return RotDrawMode.Fresh;
			}
		}

		// Token: 0x060016AD RID: 5805 RVA: 0x000D7B38 File Offset: 0x000D5D38
		public PawnRenderer(Pawn pawn)
		{
			this.pawn = pawn;
			this.wiggler = new PawnDownedWiggler(pawn);
			this.statusOverlays = new PawnHeadOverlays(pawn);
			this.woundOverlays = new PawnWoundDrawer(pawn);
			this.graphics = new PawnGraphicSet(pawn);
			this.effecters = new PawnStatusEffecters(pawn);
		}

		// Token: 0x060016AE RID: 5806 RVA: 0x0001613A File Offset: 0x0001433A
		public void RenderPawnAt(Vector3 drawLoc)
		{
			this.RenderPawnAt(drawLoc, this.CurRotDrawMode, !this.pawn.health.hediffSet.HasHead, this.pawn.IsInvisible());
		}

		// Token: 0x060016AF RID: 5807 RVA: 0x000D7B90 File Offset: 0x000D5D90
		public void RenderPawnAt(Vector3 drawLoc, RotDrawMode bodyDrawType, bool headStump, bool invisible)
		{
			if (!this.graphics.AllResolved)
			{
				this.graphics.ResolveAllGraphics();
			}
			if (this.pawn.GetPosture() == PawnPosture.Standing)
			{
				this.RenderPawnInternal(drawLoc, 0f, true, bodyDrawType, headStump, invisible);
				if (this.pawn.carryTracker != null)
				{
					Thing carriedThing = this.pawn.carryTracker.CarriedThing;
					if (carriedThing != null)
					{
						Vector3 vector = drawLoc;
						bool flag = false;
						bool flip = false;
						if (this.pawn.CurJob == null || !this.pawn.jobs.curDriver.ModifyCarriedThingDrawPos(ref vector, ref flag, ref flip))
						{
							if (carriedThing is Pawn || carriedThing is Corpse)
							{
								vector += new Vector3(0.44f, 0f, 0f);
							}
							else
							{
								vector += new Vector3(0.18f, 0f, 0.05f);
							}
						}
						if (flag)
						{
							vector.y -= 0.036734693f;
						}
						else
						{
							vector.y += 0.036734693f;
						}
						carriedThing.DrawAt(vector, flip);
					}
				}
				if (!invisible)
				{
					if (this.pawn.def.race.specialShadowData != null)
					{
						if (this.shadowGraphic == null)
						{
							this.shadowGraphic = new Graphic_Shadow(this.pawn.def.race.specialShadowData);
						}
						this.shadowGraphic.Draw(drawLoc, Rot4.North, this.pawn, 0f);
					}
					if (this.graphics.nakedGraphic != null && this.graphics.nakedGraphic.ShadowGraphic != null)
					{
						this.graphics.nakedGraphic.ShadowGraphic.Draw(drawLoc, Rot4.North, this.pawn, 0f);
					}
				}
			}
			else
			{
				float angle = this.BodyAngle();
				Rot4 rot = this.LayingFacing();
				Building_Bed building_Bed = this.pawn.CurrentBed();
				bool renderBody;
				Vector3 rootLoc;
				if (building_Bed != null && this.pawn.RaceProps.Humanlike)
				{
					renderBody = building_Bed.def.building.bed_showSleeperBody;
					AltitudeLayer altLayer = (AltitudeLayer)Mathf.Max((int)building_Bed.def.altitudeLayer, 17);
					Vector3 vector2;
					Vector3 a = vector2 = this.pawn.Position.ToVector3ShiftedWithAltitude(altLayer);
					vector2.y += 0.024489796f;
					Rot4 rotation = building_Bed.Rotation;
					rotation.AsInt += 2;
					float d = -this.BaseHeadOffsetAt(Rot4.South).z;
					Vector3 a2 = rotation.FacingCell.ToVector3();
					rootLoc = a + a2 * d;
					rootLoc.y += 0.009183673f;
				}
				else
				{
					renderBody = true;
					rootLoc = drawLoc;
					if (!this.pawn.Dead && this.pawn.CarriedBy == null)
					{
						rootLoc.y = AltitudeLayer.LayingPawn.AltitudeFor() + 0.009183673f;
					}
				}
				this.RenderPawnInternal(rootLoc, angle, renderBody, rot, rot, bodyDrawType, false, headStump, invisible);
			}
			if (this.pawn.Spawned && !this.pawn.Dead)
			{
				this.pawn.stances.StanceTrackerDraw();
				this.pawn.pather.PatherDraw();
			}
			this.DrawDebug();
		}

		// Token: 0x060016B0 RID: 5808 RVA: 0x000D7EC8 File Offset: 0x000D60C8
		public void RenderPortrait()
		{
			Vector3 zero = Vector3.zero;
			float angle;
			if (this.pawn.Dead || this.pawn.Downed)
			{
				angle = 85f;
				zero.x -= 0.18f;
				zero.z -= 0.18f;
			}
			else
			{
				angle = 0f;
			}
			this.RenderPawnInternal(zero, angle, true, Rot4.South, Rot4.South, this.CurRotDrawMode, true, !this.pawn.health.hediffSet.HasHead, this.pawn.IsInvisible());
		}

		// Token: 0x060016B1 RID: 5809 RVA: 0x000D7F60 File Offset: 0x000D6160
		private void RenderPawnInternal(Vector3 rootLoc, float angle, bool renderBody, RotDrawMode draw, bool headStump, bool invisible)
		{
			this.RenderPawnInternal(rootLoc, angle, renderBody, this.pawn.Rotation, this.pawn.Rotation, draw, false, headStump, invisible);
		}

		// Token: 0x060016B2 RID: 5810 RVA: 0x000D7F94 File Offset: 0x000D6194
		private void RenderPawnInternal(Vector3 rootLoc, float angle, bool renderBody, Rot4 bodyFacing, Rot4 headFacing, RotDrawMode bodyDrawType, bool portrait, bool headStump, bool invisible)
		{
			if (!this.graphics.AllResolved)
			{
				this.graphics.ResolveAllGraphics();
			}
			Quaternion quaternion = Quaternion.AngleAxis(angle, Vector3.up);
			Mesh mesh = null;
			if (renderBody)
			{
				Vector3 loc = rootLoc;
				loc.y += 0.009183673f;
				if (bodyDrawType == RotDrawMode.Dessicated && !this.pawn.RaceProps.Humanlike && this.graphics.dessicatedGraphic != null && !portrait)
				{
					this.graphics.dessicatedGraphic.Draw(loc, bodyFacing, this.pawn, angle);
				}
				else
				{
					if (this.pawn.RaceProps.Humanlike)
					{
						mesh = MeshPool.humanlikeBodySet.MeshAt(bodyFacing);
					}
					else
					{
						mesh = this.graphics.nakedGraphic.MeshAt(bodyFacing);
					}
					List<Material> list = this.graphics.MatsBodyBaseAt(bodyFacing, bodyDrawType);
					for (int i = 0; i < list.Count; i++)
					{
						Material mat = this.OverrideMaterialIfNeeded_NewTemp(list[i], this.pawn, portrait);
						GenDraw.DrawMeshNowOrLater(mesh, loc, quaternion, mat, portrait);
						loc.y += 0.0030612245f;
					}
					if (bodyDrawType == RotDrawMode.Fresh)
					{
						Vector3 drawLoc = rootLoc;
						drawLoc.y += 0.018367346f;
						this.woundOverlays.RenderOverBody(drawLoc, mesh, quaternion, portrait);
					}
				}
			}
			Vector3 vector = rootLoc;
			Vector3 a = rootLoc;
			if (bodyFacing != Rot4.North)
			{
				a.y += 0.024489796f;
				vector.y += 0.021428572f;
			}
			else
			{
				a.y += 0.021428572f;
				vector.y += 0.024489796f;
			}
			Vector3 vector2 = rootLoc;
			vector2.y += ((bodyFacing == Rot4.South) ? 0.006122449f : 0.02755102f);
			List<ApparelGraphicRecord> apparelGraphics = this.graphics.apparelGraphics;
			if (this.graphics.headGraphic != null)
			{
				Vector3 b = quaternion * this.BaseHeadOffsetAt(headFacing);
				Material material = this.graphics.HeadMatAt_NewTemp(headFacing, bodyDrawType, headStump, portrait);
				if (material != null)
				{
					GenDraw.DrawMeshNowOrLater(MeshPool.humanlikeHeadSet.MeshAt(headFacing), a + b, quaternion, material, portrait);
				}
				Vector3 loc2 = rootLoc + b;
				loc2.y += 0.030612245f;
				bool flag = false;
				if (!portrait || !Prefs.HatsOnlyOnMap)
				{
					Mesh mesh2 = this.graphics.HairMeshSet.MeshAt(headFacing);
					for (int j = 0; j < apparelGraphics.Count; j++)
					{
						if (apparelGraphics[j].sourceApparel.def.apparel.LastLayer == ApparelLayerDefOf.Overhead)
						{
							if (!apparelGraphics[j].sourceApparel.def.apparel.hatRenderedFrontOfFace)
							{
								flag = true;
								Material material2 = apparelGraphics[j].graphic.MatAt(bodyFacing, null);
								material2 = this.OverrideMaterialIfNeeded_NewTemp(material2, this.pawn, portrait);
								GenDraw.DrawMeshNowOrLater(mesh2, loc2, quaternion, material2, portrait);
							}
							else
							{
								Material material3 = apparelGraphics[j].graphic.MatAt(bodyFacing, null);
								material3 = this.OverrideMaterialIfNeeded_NewTemp(material3, this.pawn, portrait);
								Vector3 loc3 = rootLoc + b;
								loc3.y += ((bodyFacing == Rot4.North) ? 0.0030612245f : 0.03367347f);
								GenDraw.DrawMeshNowOrLater(mesh2, loc3, quaternion, material3, portrait);
							}
						}
					}
				}
				if (!flag && bodyDrawType != RotDrawMode.Dessicated && !headStump)
				{
					Mesh mesh3 = this.graphics.HairMeshSet.MeshAt(headFacing);
					Material mat2 = this.graphics.HairMatAt_NewTemp(headFacing, portrait);
					GenDraw.DrawMeshNowOrLater(mesh3, loc2, quaternion, mat2, portrait);
				}
			}
			if (renderBody)
			{
				for (int k = 0; k < apparelGraphics.Count; k++)
				{
					ApparelGraphicRecord apparelGraphicRecord = apparelGraphics[k];
					if (apparelGraphicRecord.sourceApparel.def.apparel.LastLayer == ApparelLayerDefOf.Shell && !apparelGraphicRecord.sourceApparel.def.apparel.shellRenderedBehindHead)
					{
						Material material4 = apparelGraphicRecord.graphic.MatAt(bodyFacing, null);
						material4 = this.OverrideMaterialIfNeeded_NewTemp(material4, this.pawn, portrait);
						GenDraw.DrawMeshNowOrLater(mesh, vector, quaternion, material4, portrait);
					}
					if (PawnRenderer.RenderAsPack(apparelGraphicRecord.sourceApparel))
					{
						Material material5 = apparelGraphicRecord.graphic.MatAt(bodyFacing, null);
						material5 = this.OverrideMaterialIfNeeded_NewTemp(material5, this.pawn, portrait);
						if (apparelGraphicRecord.sourceApparel.def.apparel.wornGraphicData != null)
						{
							Vector2 vector3 = apparelGraphicRecord.sourceApparel.def.apparel.wornGraphicData.BeltOffsetAt(bodyFacing, this.pawn.story.bodyType);
							Vector2 vector4 = apparelGraphicRecord.sourceApparel.def.apparel.wornGraphicData.BeltScaleAt(this.pawn.story.bodyType);
							Matrix4x4 matrix = Matrix4x4.Translate(vector2) * Matrix4x4.Rotate(quaternion) * Matrix4x4.Translate(new Vector3(vector3.x, 0f, vector3.y)) * Matrix4x4.Scale(new Vector3(vector4.x, 1f, vector4.y));
							GenDraw.DrawMeshNowOrLater_NewTemp(mesh, matrix, material5, portrait);
						}
						else
						{
							GenDraw.DrawMeshNowOrLater(mesh, vector, quaternion, material5, portrait);
						}
					}
				}
			}
			if (!portrait && this.pawn.RaceProps.Animal && this.pawn.inventory != null && this.pawn.inventory.innerContainer.Count > 0 && this.graphics.packGraphic != null)
			{
				Graphics.DrawMesh(mesh, vector, quaternion, this.graphics.packGraphic.MatAt(bodyFacing, null), 0);
			}
			if (!portrait)
			{
				this.DrawEquipment(rootLoc);
				if (this.pawn.apparel != null)
				{
					List<Apparel> wornApparel = this.pawn.apparel.WornApparel;
					for (int l = 0; l < wornApparel.Count; l++)
					{
						wornApparel[l].DrawWornExtras();
					}
				}
				Vector3 bodyLoc = rootLoc;
				bodyLoc.y += 0.039795917f;
				this.statusOverlays.RenderStatusOverlays(bodyLoc, quaternion, MeshPool.humanlikeHeadSet.MeshAt(headFacing));
			}
		}

		// Token: 0x060016B3 RID: 5811 RVA: 0x000D85D8 File Offset: 0x000D67D8
		private void DrawEquipment(Vector3 rootLoc)
		{
			if (this.pawn.Dead || !this.pawn.Spawned)
			{
				return;
			}
			if (this.pawn.equipment == null || this.pawn.equipment.Primary == null)
			{
				return;
			}
			if (this.pawn.CurJob != null && this.pawn.CurJob.def.neverShowWeapon)
			{
				return;
			}
			Stance_Busy stance_Busy = this.pawn.stances.curStance as Stance_Busy;
			if (stance_Busy != null && !stance_Busy.neverAimWeapon && stance_Busy.focusTarg.IsValid)
			{
				Vector3 a;
				if (stance_Busy.focusTarg.HasThing)
				{
					a = stance_Busy.focusTarg.Thing.DrawPos;
				}
				else
				{
					a = stance_Busy.focusTarg.Cell.ToVector3Shifted();
				}
				float num = 0f;
				if ((a - this.pawn.DrawPos).MagnitudeHorizontalSquared() > 0.001f)
				{
					num = (a - this.pawn.DrawPos).AngleFlat();
				}
				Vector3 drawLoc = rootLoc + new Vector3(0f, 0f, 0.4f).RotatedBy(num);
				drawLoc.y += 0.036734693f;
				this.DrawEquipmentAiming(this.pawn.equipment.Primary, drawLoc, num);
				return;
			}
			if (this.CarryWeaponOpenly())
			{
				if (this.pawn.Rotation == Rot4.South)
				{
					Vector3 drawLoc2 = rootLoc + new Vector3(0f, 0f, -0.22f);
					drawLoc2.y += 0.036734693f;
					this.DrawEquipmentAiming(this.pawn.equipment.Primary, drawLoc2, 143f);
					return;
				}
				if (this.pawn.Rotation == Rot4.North)
				{
					Vector3 drawLoc3 = rootLoc + new Vector3(0f, 0f, -0.11f);
					drawLoc3.y += 0f;
					this.DrawEquipmentAiming(this.pawn.equipment.Primary, drawLoc3, 143f);
					return;
				}
				if (this.pawn.Rotation == Rot4.East)
				{
					Vector3 drawLoc4 = rootLoc + new Vector3(0.2f, 0f, -0.22f);
					drawLoc4.y += 0.036734693f;
					this.DrawEquipmentAiming(this.pawn.equipment.Primary, drawLoc4, 143f);
					return;
				}
				if (this.pawn.Rotation == Rot4.West)
				{
					Vector3 drawLoc5 = rootLoc + new Vector3(-0.2f, 0f, -0.22f);
					drawLoc5.y += 0.036734693f;
					this.DrawEquipmentAiming(this.pawn.equipment.Primary, drawLoc5, 217f);
				}
			}
		}

		// Token: 0x060016B4 RID: 5812 RVA: 0x000D88C4 File Offset: 0x000D6AC4
		public void DrawEquipmentAiming(Thing eq, Vector3 drawLoc, float aimAngle)
		{
			float num = aimAngle - 90f;
			Mesh mesh;
			if (aimAngle > 20f && aimAngle < 160f)
			{
				mesh = MeshPool.plane10;
				num += eq.def.equippedAngleOffset;
			}
			else if (aimAngle > 200f && aimAngle < 340f)
			{
				mesh = MeshPool.plane10Flip;
				num -= 180f;
				num -= eq.def.equippedAngleOffset;
			}
			else
			{
				mesh = MeshPool.plane10;
				num += eq.def.equippedAngleOffset;
			}
			num %= 360f;
			Graphic_StackCount graphic_StackCount = eq.Graphic as Graphic_StackCount;
			Material matSingle;
			if (graphic_StackCount != null)
			{
				matSingle = graphic_StackCount.SubGraphicForStackCount(1, eq.def).MatSingle;
			}
			else
			{
				matSingle = eq.Graphic.MatSingle;
			}
			Graphics.DrawMesh(mesh, drawLoc, Quaternion.AngleAxis(num, Vector3.up), matSingle, 0);
		}

		// Token: 0x060016B5 RID: 5813 RVA: 0x000D8994 File Offset: 0x000D6B94
		private Material OverrideMaterialIfNeeded_NewTemp(Material original, Pawn pawn, bool portrait = false)
		{
			Material baseMat = (!portrait && pawn.IsInvisible()) ? InvisibilityMatPool.GetInvisibleMat(original) : original;
			return this.graphics.flasher.GetDamagedMat(baseMat);
		}

		// Token: 0x060016B6 RID: 5814 RVA: 0x0001616C File Offset: 0x0001436C
		[Obsolete("Only need this overload to not break mod compatibility.")]
		private Material OverrideMaterialIfNeeded(Material original, Pawn pawn)
		{
			return this.OverrideMaterialIfNeeded_NewTemp(original, pawn, false);
		}

		// Token: 0x060016B7 RID: 5815 RVA: 0x000D89CC File Offset: 0x000D6BCC
		private bool CarryWeaponOpenly()
		{
			if (this.pawn.carryTracker != null && this.pawn.carryTracker.CarriedThing != null)
			{
				return false;
			}
			if (this.pawn.Drafted)
			{
				return true;
			}
			if (this.pawn.CurJob != null && this.pawn.CurJob.def.alwaysShowWeapon)
			{
				return true;
			}
			if (this.pawn.mindState.duty != null && this.pawn.mindState.duty.def.alwaysShowWeapon)
			{
				return true;
			}
			Lord lord = this.pawn.GetLord();
			return lord != null && lord.LordJob != null && lord.LordJob.AlwaysShowWeapon;
		}

		// Token: 0x060016B8 RID: 5816 RVA: 0x000D8A88 File Offset: 0x000D6C88
		public Rot4 LayingFacing()
		{
			if (this.pawn.GetPosture() == PawnPosture.LayingOnGroundFaceUp)
			{
				return Rot4.South;
			}
			if (this.pawn.RaceProps.Humanlike)
			{
				switch (this.pawn.thingIDNumber % 4)
				{
				case 0:
					return Rot4.South;
				case 1:
					return Rot4.South;
				case 2:
					return Rot4.East;
				case 3:
					return Rot4.West;
				}
			}
			else
			{
				switch (this.pawn.thingIDNumber % 4)
				{
				case 0:
					return Rot4.South;
				case 1:
					return Rot4.East;
				case 2:
					return Rot4.West;
				case 3:
					return Rot4.West;
				}
			}
			return Rot4.Random;
		}

		// Token: 0x060016B9 RID: 5817 RVA: 0x000D8B3C File Offset: 0x000D6D3C
		public float BodyAngle()
		{
			if (this.pawn.GetPosture() == PawnPosture.Standing)
			{
				return 0f;
			}
			Building_Bed building_Bed = this.pawn.CurrentBed();
			if (building_Bed != null && this.pawn.RaceProps.Humanlike)
			{
				Rot4 rotation = building_Bed.Rotation;
				rotation.AsInt += 2;
				return rotation.AsAngle;
			}
			if (this.pawn.Downed || this.pawn.Dead)
			{
				return this.wiggler.downedAngle;
			}
			if (this.pawn.RaceProps.Humanlike)
			{
				return this.LayingFacing().AsAngle;
			}
			Rot4 rot = Rot4.West;
			int num = this.pawn.thingIDNumber % 2;
			if (num != 0)
			{
				if (num == 1)
				{
					rot = Rot4.East;
				}
			}
			else
			{
				rot = Rot4.West;
			}
			return rot.AsAngle;
		}

		// Token: 0x060016BA RID: 5818 RVA: 0x000D8C18 File Offset: 0x000D6E18
		public Vector3 BaseHeadOffsetAt(Rot4 rotation)
		{
			Vector2 headOffset = this.pawn.story.bodyType.headOffset;
			switch (rotation.AsInt)
			{
			case 0:
				return new Vector3(0f, 0f, headOffset.y);
			case 1:
				return new Vector3(headOffset.x, 0f, headOffset.y);
			case 2:
				return new Vector3(0f, 0f, headOffset.y);
			case 3:
				return new Vector3(-headOffset.x, 0f, headOffset.y);
			default:
				Log.Error("BaseHeadOffsetAt error in " + this.pawn, false);
				return Vector3.zero;
			}
		}

		// Token: 0x060016BB RID: 5819 RVA: 0x00016177 File Offset: 0x00014377
		public void Notify_DamageApplied(DamageInfo dam)
		{
			this.graphics.flasher.Notify_DamageApplied(dam);
			this.wiggler.Notify_DamageApplied(dam);
		}

		// Token: 0x060016BC RID: 5820 RVA: 0x00016196 File Offset: 0x00014396
		public void RendererTick()
		{
			this.wiggler.WigglerTick();
			this.effecters.EffectersTick();
		}

		// Token: 0x060016BD RID: 5821 RVA: 0x000D8CD4 File Offset: 0x000D6ED4
		public static bool RenderAsPack(Apparel apparel)
		{
			return apparel.def.apparel.LastLayer.IsUtilityLayer && (apparel.def.apparel.wornGraphicData == null || apparel.def.apparel.wornGraphicData.renderUtilityAsPack);
		}

		// Token: 0x060016BE RID: 5822 RVA: 0x000D8D24 File Offset: 0x000D6F24
		private void DrawDebug()
		{
			if (DebugViewSettings.drawDuties && Find.Selector.IsSelected(this.pawn) && this.pawn.mindState != null && this.pawn.mindState.duty != null)
			{
				this.pawn.mindState.duty.DrawDebug(this.pawn);
			}
		}

		// Token: 0x0400117E RID: 4478
		private Pawn pawn;

		// Token: 0x0400117F RID: 4479
		public PawnGraphicSet graphics;

		// Token: 0x04001180 RID: 4480
		public PawnDownedWiggler wiggler;

		// Token: 0x04001181 RID: 4481
		private PawnHeadOverlays statusOverlays;

		// Token: 0x04001182 RID: 4482
		private PawnStatusEffecters effecters;

		// Token: 0x04001183 RID: 4483
		private PawnWoundDrawer woundOverlays;

		// Token: 0x04001184 RID: 4484
		private Graphic_Shadow shadowGraphic;

		// Token: 0x04001185 RID: 4485
		private const float CarriedThingDrawAngle = 16f;

		// Token: 0x04001186 RID: 4486
		private const float SubInterval = 0.0030612245f;

		// Token: 0x04001187 RID: 4487
		private const float YOffset_PrimaryEquipmentUnder = 0f;

		// Token: 0x04001188 RID: 4488
		private const float YOffset_Behind = 0.0030612245f;

		// Token: 0x04001189 RID: 4489
		private const float YOffset_Utility_South = 0.006122449f;

		// Token: 0x0400118A RID: 4490
		private const float YOffset_Body = 0.009183673f;

		// Token: 0x0400118B RID: 4491
		private const float YOffsetInterval_Clothes = 0.0030612245f;

		// Token: 0x0400118C RID: 4492
		private const float YOffset_Wounds = 0.018367346f;

		// Token: 0x0400118D RID: 4493
		private const float YOffset_Shell = 0.021428572f;

		// Token: 0x0400118E RID: 4494
		private const float YOffset_Head = 0.024489796f;

		// Token: 0x0400118F RID: 4495
		private const float YOffset_Utility = 0.02755102f;

		// Token: 0x04001190 RID: 4496
		private const float YOffset_OnHead = 0.030612245f;

		// Token: 0x04001191 RID: 4497
		private const float YOffset_PostHead = 0.03367347f;

		// Token: 0x04001192 RID: 4498
		private const float YOffset_CarriedThing = 0.036734693f;

		// Token: 0x04001193 RID: 4499
		private const float YOffset_PrimaryEquipmentOver = 0.036734693f;

		// Token: 0x04001194 RID: 4500
		private const float YOffset_Status = 0.039795917f;
	}
}
