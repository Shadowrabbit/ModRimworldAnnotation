using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001677 RID: 5751
	[StaticConstructorOnStartup]
	public class OverlayDrawer
	{
		// Token: 0x06007D65 RID: 32101 RVA: 0x00256EF4 File Offset: 0x002550F4
		public void DrawOverlay(Thing t, OverlayTypes overlayType)
		{
			if (this.overlaysToDraw.ContainsKey(t))
			{
				Dictionary<Thing, OverlayTypes> dictionary = this.overlaysToDraw;
				dictionary[t] |= overlayType;
				return;
			}
			this.overlaysToDraw.Add(t, overlayType);
		}

		// Token: 0x06007D66 RID: 32102 RVA: 0x00256F38 File Offset: 0x00255138
		public void DrawAllOverlays()
		{
			foreach (KeyValuePair<Thing, OverlayTypes> keyValuePair in this.overlaysToDraw)
			{
				this.curOffset = Vector3.zero;
				Thing key = keyValuePair.Key;
				OverlayTypes value = keyValuePair.Value;
				if ((value & OverlayTypes.BurningWick) != (OverlayTypes)0)
				{
					this.RenderBurningWick(key);
				}
				else
				{
					OverlayTypes overlayTypes = OverlayTypes.NeedsPower | OverlayTypes.PowerOff;
					int bitCountOf = Gen.GetBitCountOf((long)(value & overlayTypes));
					float num = this.StackOffsetFor(keyValuePair.Key);
					switch (bitCountOf)
					{
					case 1:
						this.curOffset = Vector3.zero;
						break;
					case 2:
						this.curOffset = new Vector3(-0.5f * num, 0f, 0f);
						break;
					case 3:
						this.curOffset = new Vector3(-1.5f * num, 0f, 0f);
						break;
					}
					if ((value & OverlayTypes.NeedsPower) != (OverlayTypes)0)
					{
						this.RenderNeedsPowerOverlay(key);
					}
					if ((value & OverlayTypes.PowerOff) != (OverlayTypes)0)
					{
						this.RenderPowerOffOverlay(key);
					}
					if ((value & OverlayTypes.BrokenDown) != (OverlayTypes)0)
					{
						this.RenderBrokenDownOverlay(key);
					}
					if ((value & OverlayTypes.OutOfFuel) != (OverlayTypes)0)
					{
						this.RenderOutOfFuelOverlay(key);
					}
				}
				if ((value & OverlayTypes.ForbiddenBig) != (OverlayTypes)0)
				{
					this.RenderForbiddenBigOverlay(key);
				}
				if ((value & OverlayTypes.Forbidden) != (OverlayTypes)0)
				{
					this.RenderForbiddenOverlay(key);
				}
				if ((value & OverlayTypes.ForbiddenRefuel) != (OverlayTypes)0)
				{
					this.RenderForbiddenRefuelOverlay(key);
				}
				if ((value & OverlayTypes.QuestionMark) != (OverlayTypes)0)
				{
					this.RenderQuestionMarkOverlay(key);
				}
			}
			this.overlaysToDraw.Clear();
		}

		// Token: 0x06007D67 RID: 32103 RVA: 0x00054477 File Offset: 0x00052677
		private float StackOffsetFor(Thing t)
		{
			return (float)t.RotatedSize.x * 0.25f;
		}

		// Token: 0x06007D68 RID: 32104 RVA: 0x0005448B File Offset: 0x0005268B
		private void RenderNeedsPowerOverlay(Thing t)
		{
			this.RenderPulsingOverlay(t, OverlayDrawer.NeedsPowerMat, 2, true);
		}

		// Token: 0x06007D69 RID: 32105 RVA: 0x0005449B File Offset: 0x0005269B
		private void RenderPowerOffOverlay(Thing t)
		{
			this.RenderPulsingOverlay(t, OverlayDrawer.PowerOffMat, 3, true);
		}

		// Token: 0x06007D6A RID: 32106 RVA: 0x000544AB File Offset: 0x000526AB
		private void RenderBrokenDownOverlay(Thing t)
		{
			this.RenderPulsingOverlay(t, OverlayDrawer.BrokenDownMat, 4, true);
		}

		// Token: 0x06007D6B RID: 32107 RVA: 0x002570B8 File Offset: 0x002552B8
		private void RenderOutOfFuelOverlay(Thing t)
		{
			CompRefuelable compRefuelable = t.TryGetComp<CompRefuelable>();
			Material mat = MaterialPool.MatFrom((compRefuelable != null) ? compRefuelable.Props.FuelIcon : ThingDefOf.Chemfuel.uiIcon, ShaderDatabase.MetaOverlay, Color.white);
			this.RenderPulsingOverlay(t, mat, 5, false);
			this.RenderPulsingOverlay(t, OverlayDrawer.OutOfFuelMat, 6, true);
		}

		// Token: 0x06007D6C RID: 32108 RVA: 0x00257110 File Offset: 0x00255310
		private void RenderPulsingOverlay(Thing thing, Material mat, int altInd, bool incrementOffset = true)
		{
			Mesh plane = MeshPool.plane08;
			this.RenderPulsingOverlay(thing, mat, altInd, plane, incrementOffset);
		}

		// Token: 0x06007D6D RID: 32109 RVA: 0x00257130 File Offset: 0x00255330
		private void RenderPulsingOverlay(Thing thing, Material mat, int altInd, Mesh mesh, bool incrementOffset = true)
		{
			Vector3 vector = thing.TrueCenter();
			vector.y = OverlayDrawer.BaseAlt + 0.042857144f * (float)altInd;
			vector += this.curOffset;
			if (incrementOffset)
			{
				this.curOffset.x = this.curOffset.x + this.StackOffsetFor(thing);
			}
			this.RenderPulsingOverlayInternal(thing, mat, vector, mesh);
		}

		// Token: 0x06007D6E RID: 32110 RVA: 0x0025718C File Offset: 0x0025538C
		private void RenderPulsingOverlayInternal(Thing thing, Material mat, Vector3 drawPos, Mesh mesh)
		{
			float num = ((float)Math.Sin((double)((Time.realtimeSinceStartup + 397f * (float)(thing.thingIDNumber % 571)) * 4f)) + 1f) * 0.5f;
			num = 0.3f + num * 0.7f;
			Material material = FadedMaterialPool.FadedVersionOf(mat, num);
			Graphics.DrawMesh(mesh, drawPos, Quaternion.identity, material, 0);
		}

		// Token: 0x06007D6F RID: 32111 RVA: 0x002571F4 File Offset: 0x002553F4
		private void RenderForbiddenRefuelOverlay(Thing t)
		{
			CompRefuelable compRefuelable = t.TryGetComp<CompRefuelable>();
			Material material = MaterialPool.MatFrom((compRefuelable != null) ? compRefuelable.Props.FuelIcon : ThingDefOf.Chemfuel.uiIcon, ShaderDatabase.MetaOverlayDesaturated, Color.white);
			Vector3 vector = t.TrueCenter();
			vector.y = OverlayDrawer.BaseAlt + 0.21428572f;
			Vector3 position = new Vector3(vector.x, vector.y + 0.042857144f, vector.z);
			Graphics.DrawMesh(MeshPool.plane08, vector, Quaternion.identity, material, 0);
			Graphics.DrawMesh(MeshPool.plane08, position, Quaternion.identity, OverlayDrawer.ForbiddenMat, 0);
		}

		// Token: 0x06007D70 RID: 32112 RVA: 0x00257294 File Offset: 0x00255494
		private void RenderForbiddenOverlay(Thing t)
		{
			Vector3 drawPos = t.DrawPos;
			if (t.RotatedSize.z == 1)
			{
				drawPos.z -= OverlayDrawer.SingleCellForbiddenOffset;
			}
			else
			{
				drawPos.z -= (float)t.RotatedSize.z * 0.3f;
			}
			drawPos.y = OverlayDrawer.BaseAlt + 0.17142858f;
			Graphics.DrawMesh(MeshPool.plane05, drawPos, Quaternion.identity, OverlayDrawer.ForbiddenMat, 0);
		}

		// Token: 0x06007D71 RID: 32113 RVA: 0x00257310 File Offset: 0x00255510
		private void RenderForbiddenBigOverlay(Thing t)
		{
			Vector3 drawPos = t.DrawPos;
			drawPos.y = OverlayDrawer.BaseAlt + 0.17142858f;
			Graphics.DrawMesh(MeshPool.plane10, drawPos, Quaternion.identity, OverlayDrawer.ForbiddenMat, 0);
		}

		// Token: 0x06007D72 RID: 32114 RVA: 0x0025734C File Offset: 0x0025554C
		private void RenderBurningWick(Thing parent)
		{
			Material material;
			if ((parent.thingIDNumber + Find.TickManager.TicksGame) % 6 < 3)
			{
				material = OverlayDrawer.WickMaterialA;
			}
			else
			{
				material = OverlayDrawer.WickMaterialB;
			}
			Vector3 drawPos = parent.DrawPos;
			drawPos.y = OverlayDrawer.BaseAlt + 0.21428572f;
			Graphics.DrawMesh(MeshPool.plane20, drawPos, Quaternion.identity, material, 0);
		}

		// Token: 0x06007D73 RID: 32115 RVA: 0x002573A8 File Offset: 0x002555A8
		private void RenderQuestionMarkOverlay(Thing t)
		{
			Vector3 drawPos = t.DrawPos;
			drawPos.y = OverlayDrawer.BaseAlt + 0.25714287f;
			if (t is Pawn)
			{
				drawPos.x += (float)t.def.size.x - 0.52f;
				drawPos.z += (float)t.def.size.z - 0.45f;
			}
			this.RenderPulsingOverlayInternal(t, OverlayDrawer.QuestionMarkMat, drawPos, MeshPool.plane05);
		}

		// Token: 0x040051CA RID: 20938
		private Dictionary<Thing, OverlayTypes> overlaysToDraw = new Dictionary<Thing, OverlayTypes>();

		// Token: 0x040051CB RID: 20939
		private Vector3 curOffset;

		// Token: 0x040051CC RID: 20940
		private static readonly Material ForbiddenMat = MaterialPool.MatFrom("Things/Special/ForbiddenOverlay", ShaderDatabase.MetaOverlay);

		// Token: 0x040051CD RID: 20941
		private static readonly Material NeedsPowerMat = MaterialPool.MatFrom("UI/Overlays/NeedsPower", ShaderDatabase.MetaOverlay);

		// Token: 0x040051CE RID: 20942
		private static readonly Material PowerOffMat = MaterialPool.MatFrom("UI/Overlays/PowerOff", ShaderDatabase.MetaOverlay);

		// Token: 0x040051CF RID: 20943
		private static readonly Material QuestionMarkMat = MaterialPool.MatFrom("UI/Overlays/QuestionMark", ShaderDatabase.MetaOverlay);

		// Token: 0x040051D0 RID: 20944
		private static readonly Material BrokenDownMat = MaterialPool.MatFrom("UI/Overlays/BrokenDown", ShaderDatabase.MetaOverlay);

		// Token: 0x040051D1 RID: 20945
		private static readonly Material OutOfFuelMat = MaterialPool.MatFrom("UI/Overlays/OutOfFuel", ShaderDatabase.MetaOverlay);

		// Token: 0x040051D2 RID: 20946
		private static readonly Material WickMaterialA = MaterialPool.MatFrom("Things/Special/BurningWickA", ShaderDatabase.MetaOverlay);

		// Token: 0x040051D3 RID: 20947
		private static readonly Material WickMaterialB = MaterialPool.MatFrom("Things/Special/BurningWickB", ShaderDatabase.MetaOverlay);

		// Token: 0x040051D4 RID: 20948
		private const int AltitudeIndex_Forbidden = 4;

		// Token: 0x040051D5 RID: 20949
		private const int AltitudeIndex_BurningWick = 5;

		// Token: 0x040051D6 RID: 20950
		private const int AltitudeIndex_QuestionMark = 6;

		// Token: 0x040051D7 RID: 20951
		private static float SingleCellForbiddenOffset = 0.3f;

		// Token: 0x040051D8 RID: 20952
		private const float PulseFrequency = 4f;

		// Token: 0x040051D9 RID: 20953
		private const float PulseAmplitude = 0.7f;

		// Token: 0x040051DA RID: 20954
		private static readonly float BaseAlt = AltitudeLayer.MetaOverlays.AltitudeFor();

		// Token: 0x040051DB RID: 20955
		private const float StackOffsetMultipiler = 0.25f;
	}
}
