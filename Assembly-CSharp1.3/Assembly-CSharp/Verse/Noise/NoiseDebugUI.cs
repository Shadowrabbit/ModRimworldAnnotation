using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x02000508 RID: 1288
	public static class NoiseDebugUI
	{
		// Token: 0x170007A2 RID: 1954
		// (set) Token: 0x06002703 RID: 9987 RVA: 0x000F108F File Offset: 0x000EF28F
		public static IntVec2 RenderSize
		{
			set
			{
				NoiseRenderer.renderSize = value;
			}
		}

		// Token: 0x06002704 RID: 9988 RVA: 0x000F1098 File Offset: 0x000EF298
		public static void StoreTexture(Texture2D texture, string name)
		{
			NoiseDebugUI.Noise2D item = new NoiseDebugUI.Noise2D(texture, name);
			NoiseDebugUI.noises2D.Add(item);
		}

		// Token: 0x06002705 RID: 9989 RVA: 0x000F10B8 File Offset: 0x000EF2B8
		public static void StoreNoiseRender(ModuleBase noise, string name, IntVec2 renderSize)
		{
			NoiseDebugUI.RenderSize = renderSize;
			NoiseDebugUI.StoreNoiseRender(noise, name);
		}

		// Token: 0x06002706 RID: 9990 RVA: 0x000F10C8 File Offset: 0x000EF2C8
		public static void StoreNoiseRender(ModuleBase noise, string name)
		{
			if (!Prefs.DevMode || !DebugViewSettings.drawRecordedNoise)
			{
				return;
			}
			NoiseDebugUI.Noise2D item = new NoiseDebugUI.Noise2D(noise, name);
			NoiseDebugUI.noises2D.Add(item);
		}

		// Token: 0x06002707 RID: 9991 RVA: 0x000F10F8 File Offset: 0x000EF2F8
		public static void StorePlanetNoise(ModuleBase noise, string name)
		{
			if (!Prefs.DevMode || !DebugViewSettings.drawRecordedNoise)
			{
				return;
			}
			NoiseDebugUI.NoisePlanet item = new NoiseDebugUI.NoisePlanet(noise, name);
			NoiseDebugUI.planetNoises.Add(item);
		}

		// Token: 0x06002708 RID: 9992 RVA: 0x000F1128 File Offset: 0x000EF328
		public static void NoiseDebugOnGUI()
		{
			if (!Prefs.DevMode || !DebugViewSettings.drawRecordedNoise)
			{
				return;
			}
			if (Widgets.ButtonText(new Rect(0f, 40f, 200f, 30f), "Clear noise renders", true, true, true))
			{
				NoiseDebugUI.Clear();
			}
			if (Widgets.ButtonText(new Rect(200f, 40f, 200f, 30f), "Hide noise renders", true, true, true))
			{
				DebugViewSettings.drawRecordedNoise = false;
			}
			if (WorldRendererUtility.WorldRenderedNow)
			{
				if (NoiseDebugUI.planetNoises.Any<NoiseDebugUI.NoisePlanet>() && Widgets.ButtonText(new Rect(400f, 40f, 200f, 30f), "Next planet noise", true, true, true))
				{
					if (Event.current.button == 1)
					{
						if (NoiseDebugUI.currentPlanetNoise == null || NoiseDebugUI.planetNoises.IndexOf(NoiseDebugUI.currentPlanetNoise) == -1)
						{
							NoiseDebugUI.currentPlanetNoise = NoiseDebugUI.planetNoises[NoiseDebugUI.planetNoises.Count - 1];
						}
						else if (NoiseDebugUI.planetNoises.IndexOf(NoiseDebugUI.currentPlanetNoise) == 0)
						{
							NoiseDebugUI.currentPlanetNoise = null;
						}
						else
						{
							NoiseDebugUI.currentPlanetNoise = NoiseDebugUI.planetNoises[NoiseDebugUI.planetNoises.IndexOf(NoiseDebugUI.currentPlanetNoise) - 1];
						}
					}
					else if (NoiseDebugUI.currentPlanetNoise == null || NoiseDebugUI.planetNoises.IndexOf(NoiseDebugUI.currentPlanetNoise) == -1)
					{
						NoiseDebugUI.currentPlanetNoise = NoiseDebugUI.planetNoises[0];
					}
					else if (NoiseDebugUI.planetNoises.IndexOf(NoiseDebugUI.currentPlanetNoise) == NoiseDebugUI.planetNoises.Count - 1)
					{
						NoiseDebugUI.currentPlanetNoise = null;
					}
					else
					{
						NoiseDebugUI.currentPlanetNoise = NoiseDebugUI.planetNoises[NoiseDebugUI.planetNoises.IndexOf(NoiseDebugUI.currentPlanetNoise) + 1];
					}
				}
				if (NoiseDebugUI.currentPlanetNoise != null)
				{
					Rect rect = new Rect(605f, 40f, 300f, 30f);
					Text.Font = GameFont.Medium;
					Widgets.Label(rect, NoiseDebugUI.currentPlanetNoise.name);
					Text.Font = GameFont.Small;
				}
			}
			float num = 0f;
			float num2 = 90f;
			Text.Font = GameFont.Tiny;
			foreach (NoiseDebugUI.Noise2D noise2D in NoiseDebugUI.noises2D)
			{
				Texture2D texture = noise2D.Texture;
				if (num + (float)texture.width + 5f > (float)UI.screenWidth)
				{
					num = 0f;
					num2 += (float)(texture.height + 5 + 25);
				}
				GUI.DrawTexture(new Rect(num, num2, (float)texture.width, (float)texture.height), texture);
				Rect rect2 = new Rect(num, num2 - 15f, (float)texture.width, (float)texture.height);
				GUI.color = Color.black;
				Widgets.Label(rect2, noise2D.name);
				GUI.color = Color.white;
				Widgets.Label(new Rect(rect2.x + 1f, rect2.y + 1f, rect2.width, rect2.height), noise2D.name);
				num += (float)(texture.width + 5);
			}
		}

		// Token: 0x06002709 RID: 9993 RVA: 0x000F1444 File Offset: 0x000EF644
		public static void RenderPlanetNoise()
		{
			if (!Prefs.DevMode || !DebugViewSettings.drawRecordedNoise)
			{
				return;
			}
			if (NoiseDebugUI.currentPlanetNoise == null)
			{
				return;
			}
			if (NoiseDebugUI.planetNoiseMesh == null)
			{
				List<int> triangles;
				SphereGenerator.Generate(6, 100.3f, Vector3.forward, 360f, out NoiseDebugUI.planetNoiseMeshVerts, out triangles);
				NoiseDebugUI.planetNoiseMesh = new Mesh();
				NoiseDebugUI.planetNoiseMesh.name = "NoiseDebugUI";
				NoiseDebugUI.planetNoiseMesh.SetVertices(NoiseDebugUI.planetNoiseMeshVerts);
				NoiseDebugUI.planetNoiseMesh.SetTriangles(triangles, 0);
				NoiseDebugUI.lastDrawnPlanetNoise = null;
			}
			if (NoiseDebugUI.lastDrawnPlanetNoise != NoiseDebugUI.currentPlanetNoise)
			{
				NoiseDebugUI.UpdatePlanetNoiseVertexColors();
				NoiseDebugUI.lastDrawnPlanetNoise = NoiseDebugUI.currentPlanetNoise;
			}
			Graphics.DrawMesh(NoiseDebugUI.planetNoiseMesh, Vector3.zero, Quaternion.identity, WorldMaterials.VertexColor, WorldCameraManager.WorldLayer);
		}

		// Token: 0x0600270A RID: 9994 RVA: 0x000F1504 File Offset: 0x000EF704
		public static void Clear()
		{
			for (int i = 0; i < NoiseDebugUI.noises2D.Count; i++)
			{
				UnityEngine.Object.Destroy(NoiseDebugUI.noises2D[i].Texture);
			}
			NoiseDebugUI.noises2D.Clear();
			NoiseDebugUI.ClearPlanetNoises();
		}

		// Token: 0x0600270B RID: 9995 RVA: 0x000F154C File Offset: 0x000EF74C
		public static void ClearPlanetNoises()
		{
			NoiseDebugUI.planetNoises.Clear();
			NoiseDebugUI.currentPlanetNoise = null;
			NoiseDebugUI.lastDrawnPlanetNoise = null;
			if (NoiseDebugUI.planetNoiseMesh != null)
			{
				Mesh localPlanetNoiseMesh = NoiseDebugUI.planetNoiseMesh;
				LongEventHandler.ExecuteWhenFinished(delegate
				{
					UnityEngine.Object.Destroy(localPlanetNoiseMesh);
				});
				NoiseDebugUI.planetNoiseMesh = null;
			}
		}

		// Token: 0x0600270C RID: 9996 RVA: 0x000F15A4 File Offset: 0x000EF7A4
		private static void UpdatePlanetNoiseVertexColors()
		{
			NoiseDebugUI.planetNoiseMeshColors.Clear();
			for (int i = 0; i < NoiseDebugUI.planetNoiseMeshVerts.Count; i++)
			{
				byte b = (byte)Mathf.Clamp((NoiseDebugUI.currentPlanetNoise.noise.GetValue(NoiseDebugUI.planetNoiseMeshVerts[i]) * 0.5f + 0.5f) * 255f, 0f, 255f);
				NoiseDebugUI.planetNoiseMeshColors.Add(new Color32(b, b, b, byte.MaxValue));
			}
			NoiseDebugUI.planetNoiseMesh.SetColors(NoiseDebugUI.planetNoiseMeshColors);
		}

		// Token: 0x04001845 RID: 6213
		private static List<NoiseDebugUI.Noise2D> noises2D = new List<NoiseDebugUI.Noise2D>();

		// Token: 0x04001846 RID: 6214
		private static List<NoiseDebugUI.NoisePlanet> planetNoises = new List<NoiseDebugUI.NoisePlanet>();

		// Token: 0x04001847 RID: 6215
		private static Mesh planetNoiseMesh;

		// Token: 0x04001848 RID: 6216
		private static NoiseDebugUI.NoisePlanet currentPlanetNoise;

		// Token: 0x04001849 RID: 6217
		private static NoiseDebugUI.NoisePlanet lastDrawnPlanetNoise;

		// Token: 0x0400184A RID: 6218
		private static List<Color32> planetNoiseMeshColors = new List<Color32>();

		// Token: 0x0400184B RID: 6219
		private static List<Vector3> planetNoiseMeshVerts;

		// Token: 0x02001CE8 RID: 7400
		private class Noise2D
		{
			// Token: 0x17001A1B RID: 6683
			// (get) Token: 0x0600A89B RID: 43163 RVA: 0x00386B9B File Offset: 0x00384D9B
			public Texture2D Texture
			{
				get
				{
					if (this.tex == null)
					{
						this.tex = NoiseRenderer.NoiseRendered(this.noise);
					}
					return this.tex;
				}
			}

			// Token: 0x0600A89C RID: 43164 RVA: 0x00386BC2 File Offset: 0x00384DC2
			public Noise2D(Texture2D tex, string name)
			{
				this.tex = tex;
				this.name = name;
			}

			// Token: 0x0600A89D RID: 43165 RVA: 0x00386BD8 File Offset: 0x00384DD8
			public Noise2D(ModuleBase noise, string name)
			{
				this.noise = noise;
				this.name = name;
			}

			// Token: 0x04006F85 RID: 28549
			public string name;

			// Token: 0x04006F86 RID: 28550
			private Texture2D tex;

			// Token: 0x04006F87 RID: 28551
			private ModuleBase noise;
		}

		// Token: 0x02001CE9 RID: 7401
		private class NoisePlanet
		{
			// Token: 0x0600A89E RID: 43166 RVA: 0x00386BEE File Offset: 0x00384DEE
			public NoisePlanet(ModuleBase noise, string name)
			{
				this.name = name;
				this.noise = noise;
			}

			// Token: 0x04006F88 RID: 28552
			public string name;

			// Token: 0x04006F89 RID: 28553
			public ModuleBase noise;
		}
	}
}
