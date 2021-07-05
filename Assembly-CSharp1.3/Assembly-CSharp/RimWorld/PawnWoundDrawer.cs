using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D8E RID: 3470
	[StaticConstructorOnStartup]
	public class PawnWoundDrawer
	{
		// Token: 0x0600507B RID: 20603 RVA: 0x001AE51A File Offset: 0x001AC71A
		public PawnWoundDrawer(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x0600507C RID: 20604 RVA: 0x001AE52C File Offset: 0x001AC72C
		[DebugAction("General", "Enable wound debug draw", actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void WoundDebug()
		{
			IntVec3 c = UI.MouseCell();
			Pawn pawn = c.GetFirstPawn(Find.CurrentMap);
			if (pawn == null || pawn.def.race == null || pawn.def.race.body == null)
			{
				return;
			}
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			list.Add(new DebugMenuOption("All", DebugMenuOptionMode.Action, delegate()
			{
				pawn.Drawer.renderer.WoundOverlays.debugDrawAllParts = true;
				PortraitsCache.SetDirty(pawn);
				GlobalTextureAtlasManager.TryMarkPawnFrameSetDirty(pawn);
			}));
			List<BodyPartRecord> allParts = pawn.def.race.body.AllParts;
			for (int i = 0; i < allParts.Count; i++)
			{
				BodyPartRecord part = allParts[i];
				list.Add(new DebugMenuOption(part.LabelCap, DebugMenuOptionMode.Action, delegate()
				{
					pawn.Drawer.renderer.WoundOverlays.debugDrawPart = part;
					PortraitsCache.SetDirty(pawn);
					GlobalTextureAtlasManager.TryMarkPawnFrameSetDirty(pawn);
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x0600507D RID: 20605 RVA: 0x001AE630 File Offset: 0x001AC830
		[DebugAction("General", "Wound debug export (non-humanlike)", actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void WoundDebugExport()
		{
			string text = Application.dataPath + "\\woundDump";
			if (!Directory.Exists(text))
			{
				Directory.CreateDirectory(text);
			}
			HashSet<RaceProperties> hashSet = new HashSet<RaceProperties>();
			foreach (PawnKindDef pawnKindDef in from pkd in DefDatabase<PawnKindDef>.AllDefsListForReading
			where !pkd.RaceProps.Humanlike
			select pkd)
			{
				if (!hashSet.Contains(pawnKindDef.RaceProps))
				{
					Pawn pawn = PawnGenerator.GeneratePawn(pawnKindDef, null);
					for (int i = 0; i < 4; i++)
					{
						Rot4 rot = new Rot4((byte)i);
						RenderTexture temporary = RenderTexture.GetTemporary(256, 256, 32, RenderTextureFormat.ARGB32);
						pawn.Drawer.renderer.WoundOverlays.debugDrawAllParts = true;
						Find.PawnCacheRenderer.RenderPawn(pawn, temporary, Vector3.zero, 1f, 0f, rot, true, true, true, true, false, default(Vector3), null, false);
						pawn.Drawer.renderer.WoundOverlays.debugDrawAllParts = false;
						Texture2D texture2D = new Texture2D(temporary.width, temporary.height, TextureFormat.ARGB32, 0, false);
						RenderTexture.active = temporary;
						texture2D.ReadPixels(new Rect(0f, 0f, (float)temporary.width, (float)temporary.height), 0, 0, true);
						RenderTexture.active = null;
						RenderTexture.ReleaseTemporary(temporary);
						File.WriteAllBytes(text + "\\" + pawn.def.LabelCap + "_" + rot + ".png", texture2D.EncodeToPNG());
					}
					pawn.Destroy(DestroyMode.Vanish);
					hashSet.Add(pawnKindDef.RaceProps);
				}
			}
			Log.Message("Dumped to " + text);
		}

		// Token: 0x0600507E RID: 20606 RVA: 0x001AE840 File Offset: 0x001ACA40
		public void RenderOverBody(Vector3 drawLoc, Mesh bodyMesh, Quaternion quat, bool drawNow, BodyTypeDef.WoundLayer layer, Rot4 pawnRot, bool? overApparel = null)
		{
			PawnWoundDrawer.<>c__DisplayClass8_0 CS$<>8__locals1;
			CS$<>8__locals1.pawnRot = pawnRot;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.quat = quat;
			CS$<>8__locals1.bodyMesh = bodyMesh;
			if (this.debugDrawPart != null)
			{
				List<BodyTypeDef.WoundAnchor> list = this.<RenderOverBody>g__FindAnchors|8_1(this.debugDrawPart, ref CS$<>8__locals1);
				if (list.Count > 0)
				{
					using (List<BodyTypeDef.WoundAnchor>.Enumerator enumerator = list.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							BodyTypeDef.WoundAnchor woundAnchor = enumerator.Current;
							if (this.<RenderOverBody>g__AnchorUseable|8_0(woundAnchor, ref CS$<>8__locals1))
							{
								Material mat = MaterialPool.MatFrom(new MaterialRequest(BaseContent.WhiteTex, ShaderDatabase.Wound, woundAnchor.debugColor));
								Vector3 b;
								float d;
								this.<RenderOverBody>g__CalcAnchorData|8_3(woundAnchor, out b, out d, ref CS$<>8__locals1);
								if (woundAnchor.layer == layer)
								{
									GenDraw.DrawMeshNowOrLater(MeshPool.circle, Matrix4x4.TRS(drawLoc + b, CS$<>8__locals1.quat, Vector3.one * d), mat, drawNow);
								}
							}
						}
						return;
					}
				}
				Vector3 b2;
				float d2;
				this.<RenderOverBody>g__GetDefaultAnchor|8_2(out b2, out d2, ref CS$<>8__locals1);
				GenDraw.DrawMeshNowOrLater(MeshPool.circle, Matrix4x4.TRS(drawLoc + b2, CS$<>8__locals1.quat, Vector3.one * d2), BaseContent.BadMat, drawNow);
				return;
			}
			if (this.debugDrawAllParts)
			{
				if (this.pawn.story != null && this.pawn.story.bodyType != null && this.pawn.story.bodyType.woundAnchors != null)
				{
					using (List<BodyTypeDef.WoundAnchor>.Enumerator enumerator = this.pawn.story.bodyType.woundAnchors.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							BodyTypeDef.WoundAnchor woundAnchor2 = enumerator.Current;
							if (this.<RenderOverBody>g__AnchorUseable|8_0(woundAnchor2, ref CS$<>8__locals1))
							{
								Material mat2 = MaterialPool.MatFrom(new MaterialRequest(BaseContent.WhiteTex, ShaderDatabase.Wound, woundAnchor2.debugColor));
								Vector3 b3;
								float d3;
								this.<RenderOverBody>g__CalcAnchorData|8_3(woundAnchor2, out b3, out d3, ref CS$<>8__locals1);
								if (woundAnchor2.layer == layer)
								{
									GenDraw.DrawMeshNowOrLater(MeshPool.circle, Matrix4x4.TRS(drawLoc + b3, CS$<>8__locals1.quat, Vector3.one * d3), mat2, drawNow);
								}
							}
						}
						goto IL_250;
					}
				}
				Vector3 b4;
				float d4;
				this.<RenderOverBody>g__GetDefaultAnchor|8_2(out b4, out d4, ref CS$<>8__locals1);
				GenDraw.DrawMeshNowOrLater(MeshPool.circle, Matrix4x4.TRS(drawLoc + b4, CS$<>8__locals1.quat, Vector3.one * d4), BaseContent.BadMat, drawNow);
			}
			IL_250:
			for (int i = 0; i < this.pawn.health.hediffSet.hediffs.Count; i++)
			{
				Hediff hediff = this.pawn.health.hediffSet.hediffs[i];
				if (hediff.Part != null && hediff.Visible && hediff.def.displayWound)
				{
					float num = 0f;
					Vector3 zero = Vector3.zero;
					string b5 = null;
					if (this.pawn.story != null && this.pawn.story.bodyType != null && this.pawn.story.bodyType.woundAnchors != null)
					{
						List<BodyTypeDef.WoundAnchor> list2 = this.<RenderOverBody>g__FindAnchors|8_1(hediff.Part, ref CS$<>8__locals1);
						if (list2.Count > 0)
						{
							for (int j = list2.Count - 1; j >= 0; j--)
							{
								if (list2[j].layer != layer || !this.<RenderOverBody>g__AnchorUseable|8_0(list2[j], ref CS$<>8__locals1))
								{
									list2.RemoveAt(j);
								}
							}
							if (list2.Count == 0)
							{
								goto IL_8A8;
							}
							BodyTypeDef.WoundAnchor woundAnchor3 = list2.RandomElement<BodyTypeDef.WoundAnchor>();
							this.<RenderOverBody>g__CalcAnchorData|8_3(woundAnchor3, out zero, out num, ref CS$<>8__locals1);
							num = (hediff.def.woundAnchorRange ?? num);
							b5 = woundAnchor3.tag;
						}
					}
					else
					{
						this.<RenderOverBody>g__GetDefaultAnchor|8_2(out zero, out num, ref CS$<>8__locals1);
					}
					Quaternion.AngleAxis((float)Rand.Range(0, 360), Vector3.up);
					Rand.PushState(this.pawn.thingIDNumber * i * CS$<>8__locals1.pawnRot.AsInt);
					try
					{
						FleshTypeDef.ResolvedWound resolvedWound = this.pawn.RaceProps.FleshType.ChooseWoundOverlay(hediff);
						if (resolvedWound != null)
						{
							if (overApparel != null)
							{
								bool? flag = overApparel;
								bool displayOverApparel = resolvedWound.wound.displayOverApparel;
								if (!(flag.GetValueOrDefault() == displayOverApparel & flag != null))
								{
									goto IL_8A8;
								}
							}
							Hediff_Injury hd;
							if (resolvedWound.wound.displayPermanent || (hd = (hediff as Hediff_Injury)) == null || !hd.IsPermanent())
							{
								Vector3 insideUnitCircleVec = Rand.InsideUnitCircleVec3;
								if (CS$<>8__locals1.pawnRot == Rot4.East)
								{
									insideUnitCircleVec.x *= -1f;
								}
								Vector3 pos = drawLoc + zero + CS$<>8__locals1.quat * insideUnitCircleVec * num;
								Vector3 a = drawLoc + zero + insideUnitCircleVec * num;
								Vector4? vector = null;
								Vector4? vector2 = null;
								bool flag2;
								Material material = resolvedWound.GetMaterial(CS$<>8__locals1.pawnRot, out flag2);
								if (resolvedWound.wound.flipOnWoundAnchorTag != null && resolvedWound.wound.flipOnWoundAnchorTag == b5 && resolvedWound.wound.flipOnRotation == CS$<>8__locals1.pawnRot)
								{
									flag2 = !flag2;
								}
								Mesh mesh = flag2 ? MeshPool.GridPlaneFlip(Vector2.one * 0.25f) : MeshPool.GridPlane(Vector2.one * 0.25f);
								if (!this.pawn.def.race.Humanlike)
								{
									MaterialRequest req = default(MaterialRequest);
									bool flag3 = (this.pawn.Drawer.renderer.graphics.nakedGraphic.EastFlipped && CS$<>8__locals1.pawnRot == Rot4.East) || (this.pawn.Drawer.renderer.graphics.nakedGraphic.WestFlipped && CS$<>8__locals1.pawnRot == Rot4.West);
									req.maskTex = (Texture2D)this.pawn.Drawer.renderer.graphics.nakedGraphic.MatAt(CS$<>8__locals1.pawnRot, null).mainTexture;
									req.mainTex = material.mainTexture;
									req.color = material.color;
									req.shader = material.shader;
									material = MaterialPool.MatFrom(req);
									Vector3 size = CS$<>8__locals1.bodyMesh.bounds.size;
									Vector3 extents = CS$<>8__locals1.bodyMesh.bounds.extents;
									Vector3 size2 = mesh.bounds.size;
									Vector3 extents2 = mesh.bounds.extents;
									Vector3 vector3 = a - extents2;
									a + extents2;
									Vector3 vector4 = drawLoc - extents;
									drawLoc + extents;
									vector2 = new Vector4?(new Vector4(size2.x / size.x, size2.z / size.z));
									vector = new Vector4?(new Vector4((vector3.x - vector4.x) / size.x, (vector3.z - vector4.z) / size.z, (float)(flag3 ? 1 : 0)));
								}
								Matrix4x4 matrix = Matrix4x4.TRS(pos, CS$<>8__locals1.quat, Vector3.one * resolvedWound.wound.scale);
								if (drawNow)
								{
									if (vector != null)
									{
										material.SetVector(ShaderPropertyIDs.MaskTextureOffset, vector.Value);
										material.SetVector(ShaderPropertyIDs.MaskTextureScale, vector2.Value);
									}
									if (resolvedWound.wound.tintWithSkinColor && this.pawn.story != null)
									{
										material.SetColor(ShaderPropertyIDs.Color, this.pawn.story.SkinColor);
									}
									material.SetPass(0);
									Graphics.DrawMeshNow(mesh, matrix);
									material.SetVector(ShaderPropertyIDs.MaskTextureOffset, Vector4.zero);
									material.SetVector(ShaderPropertyIDs.MaskTextureScale, Vector4.one);
									material.SetColor(ShaderPropertyIDs.Color, Color.white);
								}
								else
								{
									PawnWoundDrawer.propBlock.Clear();
									if (vector != null)
									{
										PawnWoundDrawer.propBlock.SetVector(ShaderPropertyIDs.MaskTextureOffset, vector.Value);
										PawnWoundDrawer.propBlock.SetVector(ShaderPropertyIDs.MaskTextureScale, vector2.Value);
									}
									if (resolvedWound.wound.tintWithSkinColor && this.pawn.story != null)
									{
										PawnWoundDrawer.propBlock.SetColor(ShaderPropertyIDs.Color, this.pawn.story.SkinColor);
									}
									Graphics.DrawMesh(mesh, matrix, material, 0, null, 0, PawnWoundDrawer.propBlock);
								}
							}
						}
					}
					finally
					{
						Rand.PopState();
					}
				}
				IL_8A8:;
			}
		}

		// Token: 0x06005080 RID: 20608 RVA: 0x001AF180 File Offset: 0x001AD380
		[CompilerGenerated]
		private bool <RenderOverBody>g__AnchorUseable|8_0(BodyTypeDef.WoundAnchor anchor, ref PawnWoundDrawer.<>c__DisplayClass8_0 A_2)
		{
			return (anchor.rotation == null || anchor.rotation.Value == A_2.pawnRot || (anchor.canMirror && anchor.rotation.Value == A_2.pawnRot.Opposite)) && (anchor.crownType == null || (this.pawn.story != null && this.pawn.story.crownType == anchor.crownType.Value));
		}

		// Token: 0x06005081 RID: 20609 RVA: 0x001AF214 File Offset: 0x001AD414
		[CompilerGenerated]
		private List<BodyTypeDef.WoundAnchor> <RenderOverBody>g__FindAnchors|8_1(BodyPartRecord curPart, ref PawnWoundDrawer.<>c__DisplayClass8_0 A_2)
		{
			PawnWoundDrawer.tmpAnchors.Clear();
			if (this.pawn.story == null || this.pawn.story.bodyType == null || this.pawn.story.bodyType.woundAnchors.NullOrEmpty<BodyTypeDef.WoundAnchor>())
			{
				return PawnWoundDrawer.tmpAnchors;
			}
			int num = 0;
			while (PawnWoundDrawer.tmpAnchors.Count == 0 && curPart != null && num < 100)
			{
				if (curPart.woundAnchorTag != null)
				{
					using (List<BodyTypeDef.WoundAnchor>.Enumerator enumerator = this.pawn.story.bodyType.woundAnchors.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							BodyTypeDef.WoundAnchor woundAnchor = enumerator.Current;
							if (woundAnchor.tag == curPart.woundAnchorTag)
							{
								PawnWoundDrawer.tmpAnchors.Add(woundAnchor);
							}
						}
						goto IL_10D;
					}
					goto IL_B6;
				}
				goto IL_B6;
				IL_10D:
				curPart = curPart.parent;
				num++;
				continue;
				IL_B6:
				foreach (BodyTypeDef.WoundAnchor woundAnchor2 in this.pawn.story.bodyType.woundAnchors)
				{
					if (curPart.IsInGroup(woundAnchor2.group))
					{
						PawnWoundDrawer.tmpAnchors.Add(woundAnchor2);
					}
				}
				goto IL_10D;
			}
			if (num == 100)
			{
				Log.Error("PawnWoundDrawer.RenderOverBody.FindAnchors while() loop ran into iteration limit! This is never supposed to happen! Is there a cyclic body part parent reference?");
			}
			return PawnWoundDrawer.tmpAnchors;
		}

		// Token: 0x06005082 RID: 20610 RVA: 0x001AF384 File Offset: 0x001AD584
		[CompilerGenerated]
		private void <RenderOverBody>g__GetDefaultAnchor|8_2(out Vector3 anchorOffset, out float range, ref PawnWoundDrawer.<>c__DisplayClass8_0 A_3)
		{
			anchorOffset = A_3.quat * A_3.bodyMesh.bounds.center;
			range = Mathf.Min(A_3.bodyMesh.bounds.extents.x, A_3.bodyMesh.bounds.extents.z) / 2f;
		}

		// Token: 0x06005083 RID: 20611 RVA: 0x001AF3F4 File Offset: 0x001AD5F4
		[CompilerGenerated]
		private void <RenderOverBody>g__CalcAnchorData|8_3(BodyTypeDef.WoundAnchor anchor, out Vector3 anchorOffset, out float range, ref PawnWoundDrawer.<>c__DisplayClass8_0 A_4)
		{
			anchorOffset = anchor.offset;
			Rot4? rotation = anchor.rotation;
			Rot4 opposite = A_4.pawnRot.Opposite;
			if (rotation != null && (rotation == null || rotation.GetValueOrDefault() == opposite))
			{
				anchorOffset.x *= -1f;
			}
			anchorOffset = A_4.quat * anchorOffset;
			range = anchor.range;
		}

		// Token: 0x04002FF4 RID: 12276
		protected Pawn pawn;

		// Token: 0x04002FF5 RID: 12277
		private static List<BodyTypeDef.WoundAnchor> tmpAnchors = new List<BodyTypeDef.WoundAnchor>();

		// Token: 0x04002FF6 RID: 12278
		private static MaterialPropertyBlock propBlock = new MaterialPropertyBlock();

		// Token: 0x04002FF7 RID: 12279
		private BodyPartRecord debugDrawPart;

		// Token: 0x04002FF8 RID: 12280
		private bool debugDrawAllParts;
	}
}
