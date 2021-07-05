using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200104D RID: 4173
	[StaticConstructorOnStartup]
	public class OverlayDrawer
	{
		// Token: 0x060062A2 RID: 25250 RVA: 0x00216EE4 File Offset: 0x002150E4
		public ThingOverlaysHandle GetOverlaysHandle(Thing thing)
		{
			if (!thing.Spawned)
			{
				return null;
			}
			ThingOverlaysHandle thingOverlaysHandle;
			if (!this.overlayHandles.TryGetValue(thing, out thingOverlaysHandle))
			{
				thingOverlaysHandle = new ThingOverlaysHandle(this, thing);
				this.overlayHandles.Add(thing, thingOverlaysHandle);
			}
			return thingOverlaysHandle;
		}

		// Token: 0x060062A3 RID: 25251 RVA: 0x00216F24 File Offset: 0x00215124
		public void DisposeHandle(Thing thing)
		{
			ThingOverlaysHandle thingOverlaysHandle;
			if (this.overlayHandles.TryGetValue(thing, out thingOverlaysHandle))
			{
				thingOverlaysHandle.Dispose();
			}
			this.overlayHandles.Remove(thing);
		}

		// Token: 0x060062A4 RID: 25252 RVA: 0x00216F54 File Offset: 0x00215154
		public OverlayHandle Enable(Thing thing, OverlayTypes types)
		{
			return this.GetOverlaysHandle(thing).Enable(types);
		}

		// Token: 0x060062A5 RID: 25253 RVA: 0x00216F63 File Offset: 0x00215163
		public void Disable(Thing thing, ref OverlayHandle? handle)
		{
			this.GetOverlaysHandle(thing).Disable(ref handle);
		}

		// Token: 0x060062A6 RID: 25254 RVA: 0x00216F74 File Offset: 0x00215174
		public void DrawOverlay(Thing t, OverlayTypes overlayType)
		{
			if (overlayType == OverlayTypes.None)
			{
				return;
			}
			OverlayTypes overlayTypes;
			if (this.overlaysToDraw.TryGetValue(t, out overlayTypes))
			{
				this.overlaysToDraw[t] = (overlayTypes | overlayType);
				return;
			}
			this.overlaysToDraw.Add(t, overlayType);
		}

		// Token: 0x060062A7 RID: 25255 RVA: 0x00216FB4 File Offset: 0x002151B4
		public void DrawAllOverlays()
		{
			try
			{
				foreach (KeyValuePair<Thing, ThingOverlaysHandle> keyValuePair in this.overlayHandles)
				{
					if (!keyValuePair.Key.Fogged())
					{
						this.DrawOverlay(keyValuePair.Key, keyValuePair.Value.OverlayTypes);
					}
				}
				foreach (KeyValuePair<Thing, OverlayTypes> keyValuePair2 in this.overlaysToDraw)
				{
					this.curOffset = Vector3.zero;
					Thing key = keyValuePair2.Key;
					OverlayTypes value = keyValuePair2.Value;
					if ((value & OverlayTypes.BurningWick) != OverlayTypes.None)
					{
						this.RenderBurningWick(key);
					}
					else
					{
						OverlayTypes overlayTypes = OverlayTypes.NeedsPower | OverlayTypes.PowerOff;
						int bitCountOf = Gen.GetBitCountOf((long)(value & overlayTypes));
						float num = this.StackOffsetFor(key);
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
						if ((value & OverlayTypes.NeedsPower) != OverlayTypes.None)
						{
							this.RenderNeedsPowerOverlay(key);
						}
						if ((value & OverlayTypes.PowerOff) != OverlayTypes.None)
						{
							this.RenderPowerOffOverlay(key);
						}
						if ((value & OverlayTypes.BrokenDown) != OverlayTypes.None)
						{
							this.RenderBrokenDownOverlay(key);
						}
						if ((value & OverlayTypes.OutOfFuel) != OverlayTypes.None)
						{
							this.RenderOutOfFuelOverlay(key);
						}
					}
					if ((value & OverlayTypes.ForbiddenBig) != OverlayTypes.None)
					{
						this.RenderForbiddenBigOverlay(key);
					}
					if ((value & OverlayTypes.Forbidden) != OverlayTypes.None)
					{
						this.RenderForbiddenOverlay(key);
					}
					if ((value & OverlayTypes.ForbiddenRefuel) != OverlayTypes.None)
					{
						this.RenderForbiddenRefuelOverlay(key);
					}
					if ((value & OverlayTypes.QuestionMark) != OverlayTypes.None)
					{
						this.RenderQuestionMarkOverlay(key);
					}
				}
			}
			finally
			{
				this.overlaysToDraw.Clear();
			}
			this.drawBatch.Flush(true);
		}

		// Token: 0x060062A8 RID: 25256 RVA: 0x002171D8 File Offset: 0x002153D8
		private float StackOffsetFor(Thing t)
		{
			return (float)t.RotatedSize.x * 0.25f;
		}

		// Token: 0x060062A9 RID: 25257 RVA: 0x002171EC File Offset: 0x002153EC
		private void RenderNeedsPowerOverlay(Thing t)
		{
			this.RenderPulsingOverlay(t, OverlayDrawer.NeedsPowerMat, 2, true);
		}

		// Token: 0x060062AA RID: 25258 RVA: 0x002171FC File Offset: 0x002153FC
		private void RenderPowerOffOverlay(Thing t)
		{
			this.RenderPulsingOverlay(t, OverlayDrawer.PowerOffMat, 3, true);
		}

		// Token: 0x060062AB RID: 25259 RVA: 0x0021720C File Offset: 0x0021540C
		private void RenderBrokenDownOverlay(Thing t)
		{
			this.RenderPulsingOverlay(t, OverlayDrawer.BrokenDownMat, 4, true);
		}

		// Token: 0x060062AC RID: 25260 RVA: 0x0021721C File Offset: 0x0021541C
		private void RenderOutOfFuelOverlay(Thing t)
		{
			CompRefuelable compRefuelable = t.TryGetComp<CompRefuelable>();
			Material mat = MaterialPool.MatFrom((compRefuelable != null) ? compRefuelable.Props.FuelIcon : ThingDefOf.Chemfuel.uiIcon, ShaderDatabase.MetaOverlay, Color.white);
			this.RenderPulsingOverlay(t, mat, 5, false);
			this.RenderPulsingOverlay(t, OverlayDrawer.OutOfFuelMat, 6, true);
		}

		// Token: 0x060062AD RID: 25261 RVA: 0x00217274 File Offset: 0x00215474
		private void RenderPulsingOverlay(Thing thing, Material mat, int altInd, bool incrementOffset = true)
		{
			Mesh plane = MeshPool.plane08;
			this.RenderPulsingOverlay(thing, mat, altInd, plane, incrementOffset);
		}

		// Token: 0x060062AE RID: 25262 RVA: 0x00217294 File Offset: 0x00215494
		private void RenderPulsingOverlay(Thing thing, Material mat, int altInd, Mesh mesh, bool incrementOffset = true)
		{
			Vector3 vector = thing.TrueCenter();
			vector.y = OverlayDrawer.BaseAlt + 0.04054054f * (float)altInd;
			vector += this.curOffset;
			if (incrementOffset)
			{
				this.curOffset.x = this.curOffset.x + this.StackOffsetFor(thing);
			}
			this.RenderPulsingOverlayInternal(thing, mat, vector, mesh);
		}

		// Token: 0x060062AF RID: 25263 RVA: 0x002172F0 File Offset: 0x002154F0
		private void RenderPulsingOverlayInternal(Thing thing, Material mat, Vector3 drawPos, Mesh mesh)
		{
			float num = ((float)Math.Sin((double)((Time.realtimeSinceStartup + 397f * (float)(thing.thingIDNumber % 571)) * 4f)) + 1f) * 0.5f;
			num = 0.3f + num * 0.7f;
			Material material = FadedMaterialPool.FadedVersionOf(mat, num);
			this.drawBatch.DrawMesh(mesh, Matrix4x4.TRS(drawPos, Quaternion.identity, Vector3.one), material, 0, true);
		}

		// Token: 0x060062B0 RID: 25264 RVA: 0x00217368 File Offset: 0x00215568
		private void RenderForbiddenRefuelOverlay(Thing t)
		{
			CompRefuelable compRefuelable = t.TryGetComp<CompRefuelable>();
			Material material = MaterialPool.MatFrom((compRefuelable != null) ? compRefuelable.Props.FuelIcon : ThingDefOf.Chemfuel.uiIcon, ShaderDatabase.MetaOverlayDesaturated, Color.white);
			Vector3 vector = t.TrueCenter();
			vector.y = OverlayDrawer.BaseAlt + 0.2027027f;
			new Vector3(vector.x, vector.y + 0.04054054f, vector.z);
			this.drawBatch.DrawMesh(MeshPool.plane08, Matrix4x4.TRS(vector, Quaternion.identity, Vector3.one), material, 0, true);
			this.drawBatch.DrawMesh(MeshPool.plane08, Matrix4x4.TRS(vector, Quaternion.identity, Vector3.one), OverlayDrawer.ForbiddenMat, 0, true);
		}

		// Token: 0x060062B1 RID: 25265 RVA: 0x00217428 File Offset: 0x00215628
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
			drawPos.y = OverlayDrawer.BaseAlt + 0.16216215f;
			this.drawBatch.DrawMesh(MeshPool.plane05, Matrix4x4.TRS(drawPos, Quaternion.identity, Vector3.one), OverlayDrawer.ForbiddenMat, 0, true);
		}

		// Token: 0x060062B2 RID: 25266 RVA: 0x002174B4 File Offset: 0x002156B4
		private void RenderForbiddenBigOverlay(Thing t)
		{
			Vector3 drawPos = t.DrawPos;
			drawPos.y = OverlayDrawer.BaseAlt + 0.16216215f;
			this.drawBatch.DrawMesh(MeshPool.plane10, Matrix4x4.TRS(drawPos, Quaternion.identity, Vector3.one), OverlayDrawer.ForbiddenMat, 0, true);
		}

		// Token: 0x060062B3 RID: 25267 RVA: 0x00217504 File Offset: 0x00215704
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
			drawPos.y = OverlayDrawer.BaseAlt + 0.2027027f;
			this.drawBatch.DrawMesh(MeshPool.plane20, Matrix4x4.TRS(drawPos, Quaternion.identity, Vector3.one), material, 0, true);
		}

		// Token: 0x060062B4 RID: 25268 RVA: 0x00217574 File Offset: 0x00215774
		private void RenderQuestionMarkOverlay(Thing t)
		{
			Vector3 drawPos = t.DrawPos;
			drawPos.y = OverlayDrawer.BaseAlt + 0.24324323f;
			if (t is Pawn)
			{
				drawPos.x += (float)t.def.size.x - 0.52f;
				drawPos.z += (float)t.def.size.z - 0.45f;
			}
			this.RenderPulsingOverlayInternal(t, OverlayDrawer.QuestionMarkMat, drawPos, MeshPool.plane05);
		}

		// Token: 0x04003804 RID: 14340
		private Dictionary<Thing, OverlayTypes> overlaysToDraw = new Dictionary<Thing, OverlayTypes>();

		// Token: 0x04003805 RID: 14341
		private Dictionary<Thing, ThingOverlaysHandle> overlayHandles = new Dictionary<Thing, ThingOverlaysHandle>();

		// Token: 0x04003806 RID: 14342
		private Vector3 curOffset;

		// Token: 0x04003807 RID: 14343
		private DrawBatch drawBatch = new DrawBatch();

		// Token: 0x04003808 RID: 14344
		private static readonly Material ForbiddenMat = MaterialPool.MatFrom("Things/Special/ForbiddenOverlay", ShaderDatabase.MetaOverlay);

		// Token: 0x04003809 RID: 14345
		private static readonly Material NeedsPowerMat = MaterialPool.MatFrom("UI/Overlays/NeedsPower", ShaderDatabase.MetaOverlay);

		// Token: 0x0400380A RID: 14346
		private static readonly Material PowerOffMat = MaterialPool.MatFrom("UI/Overlays/PowerOff", ShaderDatabase.MetaOverlay);

		// Token: 0x0400380B RID: 14347
		private static readonly Material QuestionMarkMat = MaterialPool.MatFrom("UI/Overlays/QuestionMark", ShaderDatabase.MetaOverlay);

		// Token: 0x0400380C RID: 14348
		private static readonly Material BrokenDownMat = MaterialPool.MatFrom("UI/Overlays/BrokenDown", ShaderDatabase.MetaOverlay);

		// Token: 0x0400380D RID: 14349
		private static readonly Material OutOfFuelMat = MaterialPool.MatFrom("UI/Overlays/OutOfFuel", ShaderDatabase.MetaOverlay);

		// Token: 0x0400380E RID: 14350
		private static readonly Material WickMaterialA = MaterialPool.MatFrom("Things/Special/BurningWickA", ShaderDatabase.MetaOverlay);

		// Token: 0x0400380F RID: 14351
		private static readonly Material WickMaterialB = MaterialPool.MatFrom("Things/Special/BurningWickB", ShaderDatabase.MetaOverlay);

		// Token: 0x04003810 RID: 14352
		private const int AltitudeIndex_Forbidden = 4;

		// Token: 0x04003811 RID: 14353
		private const int AltitudeIndex_BurningWick = 5;

		// Token: 0x04003812 RID: 14354
		private const int AltitudeIndex_QuestionMark = 6;

		// Token: 0x04003813 RID: 14355
		private static float SingleCellForbiddenOffset = 0.3f;

		// Token: 0x04003814 RID: 14356
		private const float PulseFrequency = 4f;

		// Token: 0x04003815 RID: 14357
		private const float PulseAmplitude = 0.7f;

		// Token: 0x04003816 RID: 14358
		private static readonly float BaseAlt = AltitudeLayer.MetaOverlays.AltitudeFor();

		// Token: 0x04003817 RID: 14359
		private const float StackOffsetMultipiler = 0.25f;
	}
}
