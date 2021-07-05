using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.AI.Group;

namespace Verse
{
	// Token: 0x02000267 RID: 615
	public class PawnRenderer
	{
		// Token: 0x17000366 RID: 870
		// (get) Token: 0x0600115B RID: 4443 RVA: 0x00062E70 File Offset: 0x00061070
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

		// Token: 0x17000367 RID: 871
		// (get) Token: 0x0600115C RID: 4444 RVA: 0x00062E9E File Offset: 0x0006109E
		public PawnWoundDrawer WoundOverlays
		{
			get
			{
				return this.woundOverlays;
			}
		}

		// Token: 0x0600115D RID: 4445 RVA: 0x00062EA8 File Offset: 0x000610A8
		public PawnRenderer(Pawn pawn)
		{
			this.pawn = pawn;
			this.wiggler = new PawnDownedWiggler(pawn);
			this.statusOverlays = new PawnHeadOverlays(pawn);
			this.woundOverlays = new PawnWoundDrawer(pawn);
			this.graphics = new PawnGraphicSet(pawn);
			this.effecters = new PawnStatusEffecters(pawn);
		}

		// Token: 0x0600115E RID: 4446 RVA: 0x00062F00 File Offset: 0x00061100
		private PawnRenderFlags GetDefaultRenderFlags(Pawn pawn)
		{
			PawnRenderFlags pawnRenderFlags = PawnRenderFlags.None;
			if (pawn.IsInvisible())
			{
				pawnRenderFlags |= PawnRenderFlags.Invisible;
			}
			if (!pawn.health.hediffSet.HasHead)
			{
				pawnRenderFlags |= PawnRenderFlags.HeadStump;
			}
			return pawnRenderFlags;
		}

		// Token: 0x0600115F RID: 4447 RVA: 0x00062F34 File Offset: 0x00061134
		private Mesh GetBlitMeshUpdatedFrame(PawnTextureAtlasFrameSet frameSet, Rot4 rotation, PawnDrawMode drawMode)
		{
			int index = frameSet.GetIndex(rotation, drawMode);
			if (frameSet.isDirty[index])
			{
				Find.PawnCacheCamera.rect = frameSet.uvRects[index];
				Find.PawnCacheRenderer.RenderPawn(this.pawn, frameSet.atlas, Vector3.zero, 1f, 0f, rotation, true, drawMode == PawnDrawMode.BodyAndHead, true, true, false, default(Vector3), null, false);
				Find.PawnCacheCamera.rect = new Rect(0f, 0f, 1f, 1f);
				frameSet.isDirty[index] = false;
			}
			return frameSet.meshes[index];
		}

		// Token: 0x06001160 RID: 4448 RVA: 0x00062FD8 File Offset: 0x000611D8
		private void DrawCarriedThing(Vector3 drawLoc)
		{
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
						vector.y -= 0.03474903f;
					}
					else
					{
						vector.y += 0.03474903f;
					}
					carriedThing.DrawAt(vector, flip);
				}
			}
		}

		// Token: 0x06001161 RID: 4449 RVA: 0x000630B4 File Offset: 0x000612B4
		private void DrawInvisibleShadow(Vector3 drawLoc)
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

		// Token: 0x06001162 RID: 4450 RVA: 0x00063164 File Offset: 0x00061364
		private Vector3 GetBodyPos(Vector3 drawLoc, out bool showBody)
		{
			Building_Bed building_Bed = this.pawn.CurrentBed();
			Vector3 result;
			if (building_Bed != null && this.pawn.RaceProps.Humanlike)
			{
				showBody = building_Bed.def.building.bed_showSleeperBody;
				AltitudeLayer altLayer = (AltitudeLayer)Mathf.Max((int)building_Bed.def.altitudeLayer, 18);
				Vector3 vector;
				Vector3 a = vector = this.pawn.Position.ToVector3ShiftedWithAltitude(altLayer);
				vector.y += 0.023166021f;
				Rot4 rotation = building_Bed.Rotation;
				rotation.AsInt += 2;
				float d = -this.BaseHeadOffsetAt(Rot4.South).z;
				Vector3 a2 = rotation.FacingCell.ToVector3();
				result = a + a2 * d;
				result.y += 0.008687258f;
			}
			else
			{
				showBody = true;
				result = drawLoc;
				IThingHolderWithDrawnPawn thingHolderWithDrawnPawn;
				if ((thingHolderWithDrawnPawn = (this.pawn.ParentHolder as IThingHolderWithDrawnPawn)) != null)
				{
					result.y = thingHolderWithDrawnPawn.HeldPawnDrawPos_Y;
				}
				else if (!this.pawn.Dead && this.pawn.CarriedBy == null)
				{
					result.y = AltitudeLayer.LayingPawn.AltitudeFor() + 0.008687258f;
				}
			}
			return result;
		}

		// Token: 0x06001163 RID: 4451 RVA: 0x0006329C File Offset: 0x0006149C
		public GraphicMeshSet GetBodyOverlayMeshSet()
		{
			if (!this.pawn.RaceProps.Humanlike)
			{
				return MeshPool.humanlikeBodySet;
			}
			BodyTypeDef bodyType = this.pawn.story.bodyType;
			if (bodyType == BodyTypeDefOf.Male)
			{
				return MeshPool.humanlikeBodySet_Male;
			}
			if (bodyType == BodyTypeDefOf.Female)
			{
				return MeshPool.humanlikeBodySet_Female;
			}
			if (bodyType == BodyTypeDefOf.Thin)
			{
				return MeshPool.humanlikeBodySet_Thin;
			}
			if (bodyType == BodyTypeDefOf.Fat)
			{
				return MeshPool.humanlikeBodySet_Fat;
			}
			if (bodyType == BodyTypeDefOf.Hulk)
			{
				return MeshPool.humanlikeBodySet_Hulk;
			}
			return MeshPool.humanlikeBodySet;
		}

		// Token: 0x06001164 RID: 4452 RVA: 0x00063320 File Offset: 0x00061520
		public void RenderPawnAt(Vector3 drawLoc, Rot4? rotOverride = null, bool neverAimWeapon = false)
		{
			if (!this.graphics.AllResolved)
			{
				this.graphics.ResolveAllGraphics();
			}
			Rot4 rot = rotOverride ?? this.pawn.Rotation;
			PawnRenderFlags pawnRenderFlags = this.GetDefaultRenderFlags(this.pawn);
			if (neverAimWeapon)
			{
				pawnRenderFlags |= PawnRenderFlags.NeverAimWeapon;
			}
			RotDrawMode curRotDrawMode = this.CurRotDrawMode;
			bool flag = this.pawn.RaceProps.Humanlike && curRotDrawMode != RotDrawMode.Dessicated && !this.pawn.IsInvisible();
			PawnTextureAtlasFrameSet pawnTextureAtlasFrameSet = null;
			bool flag2;
			if (flag && !GlobalTextureAtlasManager.TryGetPawnFrameSet(this.pawn, out pawnTextureAtlasFrameSet, out flag2, true))
			{
				flag = false;
			}
			if (this.pawn.GetPosture() == PawnPosture.Standing)
			{
				if (flag)
				{
					Material material = MaterialPool.MatFrom(new MaterialRequest(pawnTextureAtlasFrameSet.atlas, ShaderDatabase.Cutout));
					material = this.OverrideMaterialIfNeeded(material, this.pawn, false);
					GenDraw.DrawMeshNowOrLater(this.GetBlitMeshUpdatedFrame(pawnTextureAtlasFrameSet, rot, PawnDrawMode.BodyAndHead), drawLoc, Quaternion.AngleAxis(0f, Vector3.up), material, false);
					this.DrawDynamicParts(drawLoc, 0f, rot, pawnRenderFlags);
				}
				else
				{
					this.RenderPawnInternal(drawLoc, 0f, true, rot, curRotDrawMode, pawnRenderFlags);
				}
				this.DrawCarriedThing(drawLoc);
				if (!pawnRenderFlags.FlagSet(PawnRenderFlags.Invisible))
				{
					this.DrawInvisibleShadow(drawLoc);
				}
			}
			else
			{
				bool flag3;
				Vector3 bodyPos = this.GetBodyPos(drawLoc, out flag3);
				float angle = this.BodyAngle();
				Rot4 rot2 = this.LayingFacing();
				if (flag)
				{
					Material material2 = MaterialPool.MatFrom(new MaterialRequest(pawnTextureAtlasFrameSet.atlas, ShaderDatabase.Cutout));
					material2 = this.OverrideMaterialIfNeeded(material2, this.pawn, false);
					GenDraw.DrawMeshNowOrLater(this.GetBlitMeshUpdatedFrame(pawnTextureAtlasFrameSet, rot2, flag3 ? PawnDrawMode.BodyAndHead : PawnDrawMode.HeadOnly), bodyPos, Quaternion.AngleAxis(angle, Vector3.up), material2, false);
					this.DrawDynamicParts(bodyPos, angle, rot, pawnRenderFlags);
				}
				else
				{
					this.RenderPawnInternal(bodyPos, angle, flag3, rot2, curRotDrawMode, pawnRenderFlags);
				}
			}
			if (this.pawn.Spawned && !this.pawn.Dead)
			{
				this.pawn.stances.StanceTrackerDraw();
				this.pawn.pather.PatherDraw();
				this.pawn.roping.RopingDraw();
			}
			this.DrawDebug();
		}

		// Token: 0x06001165 RID: 4453 RVA: 0x00063544 File Offset: 0x00061744
		public void RenderCache(Rot4 rotation, float angle, Vector3 positionOffset, bool renderHead, bool renderBody, bool portrait, bool renderHeadgear, bool renderClothes, Dictionary<Apparel, Color> overrideApparelColor = null, bool stylingStation = false)
		{
			Vector3 zero = Vector3.zero;
			PawnRenderFlags pawnRenderFlags = this.GetDefaultRenderFlags(this.pawn);
			if (portrait)
			{
				pawnRenderFlags |= PawnRenderFlags.Portrait;
			}
			pawnRenderFlags |= PawnRenderFlags.Cache;
			pawnRenderFlags |= PawnRenderFlags.DrawNow;
			if (!renderHead)
			{
				pawnRenderFlags |= PawnRenderFlags.HeadStump;
			}
			if (renderHeadgear)
			{
				pawnRenderFlags |= PawnRenderFlags.Headgear;
			}
			if (renderClothes)
			{
				pawnRenderFlags |= PawnRenderFlags.Clothes;
			}
			if (stylingStation)
			{
				pawnRenderFlags |= PawnRenderFlags.StylingStation;
			}
			PawnRenderer.tmpOriginalColors.Clear();
			try
			{
				if (overrideApparelColor != null)
				{
					foreach (KeyValuePair<Apparel, Color> keyValuePair in overrideApparelColor)
					{
						Apparel key = keyValuePair.Key;
						CompColorable compColorable = key.TryGetComp<CompColorable>();
						if (compColorable != null)
						{
							PawnRenderer.tmpOriginalColors.Add(key, new ValueTuple<Color, bool>(compColorable.Color, compColorable.Active));
							key.SetColor(keyValuePair.Value, true);
						}
					}
				}
				this.RenderPawnInternal(zero + positionOffset, angle, renderBody, rotation, this.CurRotDrawMode, pawnRenderFlags);
				foreach (KeyValuePair<Apparel, ValueTuple<Color, bool>> keyValuePair2 in PawnRenderer.tmpOriginalColors)
				{
					if (!keyValuePair2.Value.Item2)
					{
						keyValuePair2.Key.TryGetComp<CompColorable>().Disable();
					}
					else
					{
						keyValuePair2.Key.SetColor(keyValuePair2.Value.Item1, true);
					}
				}
			}
			finally
			{
				PawnRenderer.tmpOriginalColors.Clear();
			}
		}

		// Token: 0x06001166 RID: 4454 RVA: 0x000636D0 File Offset: 0x000618D0
		private void RenderPawnInternal(Vector3 rootLoc, float angle, bool renderBody, Rot4 bodyFacing, RotDrawMode bodyDrawType, PawnRenderFlags flags)
		{
			if (!this.graphics.AllResolved)
			{
				this.graphics.ResolveAllGraphics();
			}
			Quaternion quaternion = Quaternion.AngleAxis(angle, Vector3.up);
			Vector3 vector = rootLoc;
			Vector3 a = rootLoc;
			if (bodyFacing != Rot4.North)
			{
				a.y += 0.023166021f;
				vector.y += 0.02027027f;
			}
			else
			{
				a.y += 0.02027027f;
				vector.y += 0.023166021f;
			}
			Vector3 utilityLoc = rootLoc;
			utilityLoc.y += ((bodyFacing == Rot4.South) ? 0.0057915053f : 0.028957527f);
			Mesh mesh = null;
			Vector3 drawLoc;
			if (renderBody)
			{
				this.DrawPawnBody(rootLoc, angle, bodyFacing, bodyDrawType, flags, out mesh);
				drawLoc = rootLoc;
				drawLoc.y += 0.009687258f;
				if (bodyDrawType == RotDrawMode.Fresh)
				{
					this.woundOverlays.RenderOverBody(drawLoc, mesh, quaternion, flags.FlagSet(PawnRenderFlags.DrawNow), BodyTypeDef.WoundLayer.Body, bodyFacing, new bool?(false));
				}
				if (renderBody && flags.FlagSet(PawnRenderFlags.Clothes))
				{
					this.DrawBodyApparel(vector, utilityLoc, mesh, angle, bodyFacing, flags);
				}
				drawLoc = rootLoc;
				drawLoc.y += 0.022166021f;
				if (bodyDrawType == RotDrawMode.Fresh)
				{
					this.woundOverlays.RenderOverBody(drawLoc, mesh, quaternion, flags.FlagSet(PawnRenderFlags.DrawNow), BodyTypeDef.WoundLayer.Body, bodyFacing, new bool?(true));
				}
			}
			Vector3 vector2 = Vector3.zero;
			drawLoc = rootLoc;
			drawLoc.y += 0.028957527f;
			if (this.graphics.headGraphic != null)
			{
				vector2 = quaternion * this.BaseHeadOffsetAt(bodyFacing);
				Material material = this.graphics.HeadMatAt(bodyFacing, bodyDrawType, flags.FlagSet(PawnRenderFlags.HeadStump), flags.FlagSet(PawnRenderFlags.Portrait), !flags.FlagSet(PawnRenderFlags.Cache));
				if (material != null)
				{
					GenDraw.DrawMeshNowOrLater(MeshPool.humanlikeHeadSet.MeshAt(bodyFacing), a + vector2, quaternion, material, flags.FlagSet(PawnRenderFlags.DrawNow));
				}
			}
			if (bodyDrawType == RotDrawMode.Fresh)
			{
				this.woundOverlays.RenderOverBody(drawLoc, mesh, quaternion, flags.FlagSet(PawnRenderFlags.DrawNow), BodyTypeDef.WoundLayer.Head, bodyFacing, null);
			}
			if (this.graphics.headGraphic != null)
			{
				this.DrawHeadHair(rootLoc, vector2, angle, bodyFacing, bodyFacing, bodyDrawType, flags);
			}
			if (!flags.FlagSet(PawnRenderFlags.Portrait) && this.pawn.RaceProps.Animal && this.pawn.inventory != null && this.pawn.inventory.innerContainer.Count > 0 && this.graphics.packGraphic != null)
			{
				GenDraw.DrawMeshNowOrLater(mesh, Matrix4x4.TRS(vector, quaternion, Vector3.one), this.graphics.packGraphic.MatAt(bodyFacing, null), flags.FlagSet(PawnRenderFlags.DrawNow));
			}
			if (!flags.FlagSet(PawnRenderFlags.Portrait) && !flags.FlagSet(PawnRenderFlags.Cache))
			{
				this.DrawDynamicParts(rootLoc, angle, bodyFacing, flags);
			}
		}

		// Token: 0x06001167 RID: 4455 RVA: 0x000639A0 File Offset: 0x00061BA0
		private void DrawPawnBody(Vector3 rootLoc, float angle, Rot4 facing, RotDrawMode bodyDrawType, PawnRenderFlags flags, out Mesh bodyMesh)
		{
			Quaternion quat = Quaternion.AngleAxis(angle, Vector3.up);
			Vector3 vector = rootLoc;
			vector.y += 0.008687258f;
			Vector3 loc = vector;
			loc.y += 0.0014478763f;
			bodyMesh = null;
			if (bodyDrawType == RotDrawMode.Dessicated && !this.pawn.RaceProps.Humanlike && this.graphics.dessicatedGraphic != null && !flags.FlagSet(PawnRenderFlags.Portrait))
			{
				this.graphics.dessicatedGraphic.Draw(vector, facing, this.pawn, angle);
				return;
			}
			if (this.pawn.RaceProps.Humanlike)
			{
				bodyMesh = MeshPool.humanlikeBodySet.MeshAt(facing);
			}
			else
			{
				bodyMesh = this.graphics.nakedGraphic.MeshAt(facing);
			}
			List<Material> list = this.graphics.MatsBodyBaseAt(facing, bodyDrawType, flags.FlagSet(PawnRenderFlags.Clothes));
			for (int i = 0; i < list.Count; i++)
			{
				Material mat = flags.FlagSet(PawnRenderFlags.Cache) ? list[i] : this.OverrideMaterialIfNeeded(list[i], this.pawn, flags.FlagSet(PawnRenderFlags.Portrait));
				GenDraw.DrawMeshNowOrLater(bodyMesh, vector, quat, mat, flags.FlagSet(PawnRenderFlags.DrawNow));
				vector.y += 0.0028957527f;
			}
			if (ModsConfig.IdeologyActive && this.graphics.bodyTattooGraphic != null && bodyDrawType != RotDrawMode.Dessicated && (facing != Rot4.North || this.pawn.style.FaceTattoo.visibleNorth))
			{
				GenDraw.DrawMeshNowOrLater(this.GetBodyOverlayMeshSet().MeshAt(facing), loc, quat, this.graphics.bodyTattooGraphic.MatAt(facing, null), flags.FlagSet(PawnRenderFlags.DrawNow));
			}
		}

		// Token: 0x06001168 RID: 4456 RVA: 0x00063B50 File Offset: 0x00061D50
		private void DrawHeadHair(Vector3 rootLoc, Vector3 headOffset, float angle, Rot4 bodyFacing, Rot4 headFacing, RotDrawMode bodyDrawType, PawnRenderFlags flags)
		{
			if (this.ShellFullyCoversHead(flags))
			{
				return;
			}
			Vector3 vector = rootLoc + headOffset;
			vector.y += 0.028957527f;
			List<ApparelGraphicRecord> apparelGraphics = this.graphics.apparelGraphics;
			Quaternion quat = Quaternion.AngleAxis(angle, Vector3.up);
			bool flag = false;
			bool flag2 = bodyFacing == Rot4.North || this.pawn.style == null || this.pawn.style.beardDef == BeardDefOf.NoBeard;
			if (flags.FlagSet(PawnRenderFlags.Headgear) && (!flags.FlagSet(PawnRenderFlags.Portrait) || !Prefs.HatsOnlyOnMap || flags.FlagSet(PawnRenderFlags.StylingStation)))
			{
				Mesh mesh = this.graphics.HairMeshSet.MeshAt(headFacing);
				for (int i = 0; i < apparelGraphics.Count; i++)
				{
					if (apparelGraphics[i].sourceApparel.def.apparel.LastLayer == ApparelLayerDefOf.Overhead)
					{
						if (apparelGraphics[i].sourceApparel.def.apparel.bodyPartGroups.Contains(BodyPartGroupDefOf.FullHead))
						{
							flag2 = true;
						}
						if (!apparelGraphics[i].sourceApparel.def.apparel.hatRenderedFrontOfFace)
						{
							flag = true;
							Material material = apparelGraphics[i].graphic.MatAt(bodyFacing, null);
							material = (flags.FlagSet(PawnRenderFlags.Cache) ? material : this.OverrideMaterialIfNeeded(material, this.pawn, flags.FlagSet(PawnRenderFlags.Portrait)));
							GenDraw.DrawMeshNowOrLater(mesh, vector, quat, material, flags.FlagSet(PawnRenderFlags.DrawNow));
						}
						else
						{
							Material material2 = apparelGraphics[i].graphic.MatAt(bodyFacing, null);
							material2 = (flags.FlagSet(PawnRenderFlags.Cache) ? material2 : this.OverrideMaterialIfNeeded(material2, this.pawn, flags.FlagSet(PawnRenderFlags.Portrait)));
							Vector3 loc = rootLoc + headOffset;
							if (apparelGraphics[i].sourceApparel.def.apparel.hatRenderedBehindHead)
							{
								loc.y += 0.022166021f;
							}
							else
							{
								loc.y += ((bodyFacing == Rot4.North && !apparelGraphics[i].sourceApparel.def.apparel.hatRenderedAboveBody) ? 0.0028957527f : 0.03185328f);
							}
							GenDraw.DrawMeshNowOrLater(mesh, loc, quat, material2, flags.FlagSet(PawnRenderFlags.DrawNow));
						}
					}
				}
			}
			if (ModsConfig.IdeologyActive && this.graphics.faceTattooGraphic != null && bodyDrawType != RotDrawMode.Dessicated && (bodyFacing != Rot4.North || this.pawn.style.FaceTattoo.visibleNorth))
			{
				Vector3 loc2 = vector;
				loc2.y -= 0.0014478763f;
				GenDraw.DrawMeshNowOrLater(this.graphics.HairMeshSet.MeshAt(headFacing), loc2, quat, this.graphics.faceTattooGraphic.MatAt(headFacing, null), flags.FlagSet(PawnRenderFlags.DrawNow));
			}
			if (!flag2 && bodyDrawType != RotDrawMode.Dessicated && !flags.FlagSet(PawnRenderFlags.HeadStump) && this.pawn.style != null && this.pawn.style.beardDef != null)
			{
				Vector3 loc3 = this.OffsetBeardLocationForCrownType(this.pawn.story.crownType, headFacing, vector) + this.pawn.style.beardDef.GetOffset(this.pawn.story.crownType, headFacing);
				Mesh mesh2 = this.graphics.HairMeshSet.MeshAt(headFacing);
				Material material3 = this.graphics.BeardMatAt(headFacing, flags.FlagSet(PawnRenderFlags.Portrait), flags.FlagSet(PawnRenderFlags.Cache));
				if (material3 != null)
				{
					GenDraw.DrawMeshNowOrLater(mesh2, loc3, quat, material3, flags.FlagSet(PawnRenderFlags.DrawNow));
				}
			}
			if (!flag && bodyDrawType != RotDrawMode.Dessicated && !flags.FlagSet(PawnRenderFlags.HeadStump))
			{
				Mesh mesh3 = this.graphics.HairMeshSet.MeshAt(headFacing);
				Material material4 = this.graphics.HairMatAt(headFacing, flags.FlagSet(PawnRenderFlags.Portrait), flags.FlagSet(PawnRenderFlags.Cache));
				if (material4 != null)
				{
					GenDraw.DrawMeshNowOrLater(mesh3, vector, quat, material4, flags.FlagSet(PawnRenderFlags.DrawNow));
				}
			}
		}

		// Token: 0x06001169 RID: 4457 RVA: 0x00063F88 File Offset: 0x00062188
		private bool ShellFullyCoversHead(PawnRenderFlags flags)
		{
			if (!flags.FlagSet(PawnRenderFlags.Clothes))
			{
				return false;
			}
			List<ApparelGraphicRecord> apparelGraphics = this.graphics.apparelGraphics;
			for (int i = 0; i < apparelGraphics.Count; i++)
			{
				if (apparelGraphics[i].sourceApparel.def.apparel.LastLayer == ApparelLayerDefOf.Shell && apparelGraphics[i].sourceApparel.def.apparel.shellCoversHead)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600116A RID: 4458 RVA: 0x00064000 File Offset: 0x00062200
		private Vector3 OffsetBeardLocationForCrownType(CrownType crownType, Rot4 headFacing, Vector3 beardLoc)
		{
			if (this.pawn.story.crownType == CrownType.Narrow)
			{
				if (headFacing == Rot4.East)
				{
					beardLoc += Vector3.right * -0.05f;
				}
				else if (headFacing == Rot4.West)
				{
					beardLoc += Vector3.right * 0.05f;
				}
				beardLoc += Vector3.forward * -0.05f;
			}
			return beardLoc;
		}

		// Token: 0x0600116B RID: 4459 RVA: 0x00064084 File Offset: 0x00062284
		private void DrawBodyApparel(Vector3 shellLoc, Vector3 utilityLoc, Mesh bodyMesh, float angle, Rot4 bodyFacing, PawnRenderFlags flags)
		{
			List<ApparelGraphicRecord> apparelGraphics = this.graphics.apparelGraphics;
			Quaternion quaternion = Quaternion.AngleAxis(angle, Vector3.up);
			for (int i = 0; i < apparelGraphics.Count; i++)
			{
				ApparelGraphicRecord apparelGraphicRecord = apparelGraphics[i];
				if (apparelGraphicRecord.sourceApparel.def.apparel.LastLayer == ApparelLayerDefOf.Shell && !apparelGraphicRecord.sourceApparel.def.apparel.shellRenderedBehindHead)
				{
					Material material = apparelGraphicRecord.graphic.MatAt(bodyFacing, null);
					material = (flags.FlagSet(PawnRenderFlags.Cache) ? material : this.OverrideMaterialIfNeeded(material, this.pawn, flags.FlagSet(PawnRenderFlags.Portrait)));
					Vector3 loc = shellLoc;
					if (apparelGraphicRecord.sourceApparel.def.apparel.shellCoversHead)
					{
						loc.y += 0.0028957527f;
					}
					GenDraw.DrawMeshNowOrLater(bodyMesh, loc, quaternion, material, flags.FlagSet(PawnRenderFlags.DrawNow));
				}
				if (PawnRenderer.RenderAsPack(apparelGraphicRecord.sourceApparel))
				{
					Material material2 = apparelGraphicRecord.graphic.MatAt(bodyFacing, null);
					material2 = (flags.FlagSet(PawnRenderFlags.Cache) ? material2 : this.OverrideMaterialIfNeeded(material2, this.pawn, flags.FlagSet(PawnRenderFlags.Portrait)));
					if (apparelGraphicRecord.sourceApparel.def.apparel.wornGraphicData != null)
					{
						Vector2 vector = apparelGraphicRecord.sourceApparel.def.apparel.wornGraphicData.BeltOffsetAt(bodyFacing, this.pawn.story.bodyType);
						Vector2 vector2 = apparelGraphicRecord.sourceApparel.def.apparel.wornGraphicData.BeltScaleAt(this.pawn.story.bodyType);
						Matrix4x4 matrix = Matrix4x4.Translate(utilityLoc) * Matrix4x4.Rotate(quaternion) * Matrix4x4.Translate(new Vector3(vector.x, 0f, vector.y)) * Matrix4x4.Scale(new Vector3(vector2.x, 1f, vector2.y));
						GenDraw.DrawMeshNowOrLater(bodyMesh, matrix, material2, flags.FlagSet(PawnRenderFlags.DrawNow));
					}
					else
					{
						GenDraw.DrawMeshNowOrLater(bodyMesh, shellLoc, quaternion, material2, flags.FlagSet(PawnRenderFlags.DrawNow));
					}
				}
			}
		}

		// Token: 0x0600116C RID: 4460 RVA: 0x000642AC File Offset: 0x000624AC
		private void DrawDynamicParts(Vector3 rootLoc, float angle, Rot4 pawnRotation, PawnRenderFlags flags)
		{
			Quaternion quat = Quaternion.AngleAxis(angle, Vector3.up);
			this.DrawEquipment(rootLoc, pawnRotation, flags);
			if (this.pawn.apparel != null)
			{
				List<Apparel> wornApparel = this.pawn.apparel.WornApparel;
				for (int i = 0; i < wornApparel.Count; i++)
				{
					wornApparel[i].DrawWornExtras();
				}
			}
			Vector3 bodyLoc = rootLoc;
			bodyLoc.y += 0.037644785f;
			this.statusOverlays.RenderStatusOverlays(bodyLoc, quat, MeshPool.humanlikeHeadSet.MeshAt(pawnRotation));
		}

		// Token: 0x0600116D RID: 4461 RVA: 0x00064334 File Offset: 0x00062534
		private void DrawEquipment(Vector3 rootLoc, Rot4 pawnRotation, PawnRenderFlags flags)
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
			Vector3 vector = new Vector3(0f, (pawnRotation == Rot4.North) ? -0.0028957527f : 0.03474903f, 0f);
			Stance_Busy stance_Busy = this.pawn.stances.curStance as Stance_Busy;
			if (stance_Busy != null && !stance_Busy.neverAimWeapon && stance_Busy.focusTarg.IsValid && (flags & PawnRenderFlags.NeverAimWeapon) == PawnRenderFlags.None)
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
				vector += rootLoc + new Vector3(0f, 0f, 0.4f).RotatedBy(num);
				this.DrawEquipmentAiming(this.pawn.equipment.Primary, vector, num);
				return;
			}
			if (this.CarryWeaponOpenly())
			{
				if (pawnRotation == Rot4.South)
				{
					vector += rootLoc + new Vector3(0f, 0f, -0.22f);
					this.DrawEquipmentAiming(this.pawn.equipment.Primary, vector, 143f);
					return;
				}
				if (pawnRotation == Rot4.North)
				{
					vector += rootLoc + new Vector3(0f, 0f, -0.11f);
					this.DrawEquipmentAiming(this.pawn.equipment.Primary, vector, 143f);
					return;
				}
				if (pawnRotation == Rot4.East)
				{
					vector += rootLoc + new Vector3(0.2f, 0f, -0.22f);
					this.DrawEquipmentAiming(this.pawn.equipment.Primary, vector, 143f);
					return;
				}
				if (pawnRotation == Rot4.West)
				{
					vector += rootLoc + new Vector3(-0.2f, 0f, -0.22f);
					this.DrawEquipmentAiming(this.pawn.equipment.Primary, vector, 217f);
				}
			}
		}

		// Token: 0x0600116E RID: 4462 RVA: 0x000645F4 File Offset: 0x000627F4
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
			CompEquippable compEquippable = eq.TryGetComp<CompEquippable>();
			if (compEquippable != null)
			{
				Vector3 b;
				float num2;
				EquipmentUtility.Recoil(eq.def, EquipmentUtility.GetRecoilVerb(compEquippable.AllVerbs), out b, out num2, aimAngle);
				drawLoc += b;
				num += num2;
			}
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

		// Token: 0x0600116F RID: 4463 RVA: 0x000646F8 File Offset: 0x000628F8
		private Material OverrideMaterialIfNeeded(Material original, Pawn pawn, bool portrait = false)
		{
			Material baseMat = (!portrait && pawn.IsInvisible()) ? InvisibilityMatPool.GetInvisibleMat(original) : original;
			return this.graphics.flasher.GetDamagedMat(baseMat);
		}

		// Token: 0x06001170 RID: 4464 RVA: 0x00064730 File Offset: 0x00062930
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

		// Token: 0x06001171 RID: 4465 RVA: 0x000647EC File Offset: 0x000629EC
		private Rot4 RotationForcedByJob()
		{
			if (this.pawn.jobs != null && this.pawn.jobs.curDriver != null && this.pawn.jobs.curDriver.ForcedLayingRotation.IsValid)
			{
				return this.pawn.jobs.curDriver.ForcedLayingRotation;
			}
			return Rot4.Invalid;
		}

		// Token: 0x06001172 RID: 4466 RVA: 0x00064854 File Offset: 0x00062A54
		public Rot4 LayingFacing()
		{
			Rot4 result = this.RotationForcedByJob();
			if (result.IsValid)
			{
				return result;
			}
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

		// Token: 0x06001173 RID: 4467 RVA: 0x0006491C File Offset: 0x00062B1C
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
			IThingHolderWithDrawnPawn thingHolderWithDrawnPawn;
			if ((thingHolderWithDrawnPawn = (this.pawn.ParentHolder as IThingHolderWithDrawnPawn)) != null)
			{
				return thingHolderWithDrawnPawn.HeldPawnBodyAngle;
			}
			if (this.pawn.Downed || this.pawn.Dead)
			{
				return this.wiggler.downedAngle;
			}
			if (this.pawn.RaceProps.Humanlike)
			{
				return this.LayingFacing().AsAngle;
			}
			if (this.RotationForcedByJob().IsValid)
			{
				return 0f;
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

		// Token: 0x06001174 RID: 4468 RVA: 0x00064A2C File Offset: 0x00062C2C
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
				Log.Error("BaseHeadOffsetAt error in " + this.pawn);
				return Vector3.zero;
			}
		}

		// Token: 0x06001175 RID: 4469 RVA: 0x00064AE4 File Offset: 0x00062CE4
		public void Notify_DamageApplied(DamageInfo dam)
		{
			this.graphics.flasher.Notify_DamageApplied(dam);
			this.wiggler.Notify_DamageApplied(dam);
		}

		// Token: 0x06001176 RID: 4470 RVA: 0x00064B03 File Offset: 0x00062D03
		public void RendererTick()
		{
			this.wiggler.WigglerTick();
			this.effecters.EffectersTick();
		}

		// Token: 0x06001177 RID: 4471 RVA: 0x00064B1C File Offset: 0x00062D1C
		public static bool RenderAsPack(Apparel apparel)
		{
			return apparel.def.apparel.LastLayer.IsUtilityLayer && (apparel.def.apparel.wornGraphicData == null || apparel.def.apparel.wornGraphicData.renderUtilityAsPack);
		}

		// Token: 0x06001178 RID: 4472 RVA: 0x00064B6C File Offset: 0x00062D6C
		private void DrawDebug()
		{
			if (DebugViewSettings.drawDuties && Find.Selector.IsSelected(this.pawn) && this.pawn.mindState != null && this.pawn.mindState.duty != null)
			{
				this.pawn.mindState.duty.DrawDebug(this.pawn);
			}
		}

		// Token: 0x04000D57 RID: 3415
		private Pawn pawn;

		// Token: 0x04000D58 RID: 3416
		public PawnGraphicSet graphics;

		// Token: 0x04000D59 RID: 3417
		public PawnDownedWiggler wiggler;

		// Token: 0x04000D5A RID: 3418
		private PawnHeadOverlays statusOverlays;

		// Token: 0x04000D5B RID: 3419
		private PawnStatusEffecters effecters;

		// Token: 0x04000D5C RID: 3420
		private PawnWoundDrawer woundOverlays;

		// Token: 0x04000D5D RID: 3421
		private Graphic_Shadow shadowGraphic;

		// Token: 0x04000D5E RID: 3422
		private const float CarriedThingDrawAngle = 16f;

		// Token: 0x04000D5F RID: 3423
		private const float SubInterval = 0.0028957527f;

		// Token: 0x04000D60 RID: 3424
		private const float YOffset_PrimaryEquipmentUnder = -0.0028957527f;

		// Token: 0x04000D61 RID: 3425
		private const float YOffset_CarriedThingUnder = -0.0028957527f;

		// Token: 0x04000D62 RID: 3426
		private const float YOffset_Behind = 0.0028957527f;

		// Token: 0x04000D63 RID: 3427
		private const float YOffset_Utility_South = 0.0057915053f;

		// Token: 0x04000D64 RID: 3428
		private const float YOffset_Body = 0.008687258f;

		// Token: 0x04000D65 RID: 3429
		private const float YOffsetInterval_Clothes = 0.0028957527f;

		// Token: 0x04000D66 RID: 3430
		private const float YOffset_Shell = 0.02027027f;

		// Token: 0x04000D67 RID: 3431
		private const float YOffset_Head = 0.023166021f;

		// Token: 0x04000D68 RID: 3432
		private const float YOffset_OnHead = 0.028957527f;

		// Token: 0x04000D69 RID: 3433
		private const float YOffset_Utility = 0.028957527f;

		// Token: 0x04000D6A RID: 3434
		private const float YOffset_PostHead = 0.03185328f;

		// Token: 0x04000D6B RID: 3435
		private const float YOffset_CarriedThing = 0.03474903f;

		// Token: 0x04000D6C RID: 3436
		private const float YOffset_PrimaryEquipmentOver = 0.03474903f;

		// Token: 0x04000D6D RID: 3437
		private const float YOffset_Status = 0.037644785f;

		// Token: 0x04000D6E RID: 3438
		private static Dictionary<Apparel, ValueTuple<Color, bool>> tmpOriginalColors = new Dictionary<Apparel, ValueTuple<Color, bool>>();
	}
}
