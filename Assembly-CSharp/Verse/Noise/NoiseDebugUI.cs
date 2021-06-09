using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x020008C4 RID: 2244
	public static class NoiseDebugUI
	{
		// Token: 0x170008C9 RID: 2249
		// (set) Token: 0x060037D5 RID: 14293 RVA: 0x0002B28F File Offset: 0x0002948F
		public static IntVec2 RenderSize
		{
			set
			{
				NoiseRenderer.renderSize = value;
			}
		}

		// Token: 0x060037D6 RID: 14294 RVA: 0x00161AB8 File Offset: 0x0015FCB8
		public static void StoreTexture(Texture2D texture, string name)
		{
			NoiseDebugUI.Noise2D item = new NoiseDebugUI.Noise2D(texture, name);
			NoiseDebugUI.noises2D.Add(item);
		}

		// Token: 0x060037D7 RID: 14295 RVA: 0x0002B297 File Offset: 0x00029497
		public static void StoreNoiseRender(ModuleBase noise, string name, IntVec2 renderSize)
		{
			NoiseDebugUI.RenderSize = renderSize;
			NoiseDebugUI.StoreNoiseRender(noise, name);
		}

		// Token: 0x060037D8 RID: 14296 RVA: 0x00161AD8 File Offset: 0x0015FCD8
		public static void StoreNoiseRender(ModuleBase noise, string name)
		{
			if (!Prefs.DevMode || !DebugViewSettings.drawRecordedNoise)
			{
				return;
			}
			NoiseDebugUI.Noise2D item = new NoiseDebugUI.Noise2D(noise, name);
			NoiseDebugUI.noises2D.Add(item);
		}

		// Token: 0x060037D9 RID: 14297 RVA: 0x00161B08 File Offset: 0x0015FD08
		public static void StorePlanetNoise(ModuleBase noise, string name)
		{
			if (!Prefs.DevMode || !DebugViewSettings.drawRecordedNoise)
			{
				return;
			}
			NoiseDebugUI.NoisePlanet item = new NoiseDebugUI.NoisePlanet(noise, name);
			NoiseDebugUI.planetNoises.Add(item);
		}

		// Token: 0x060037DA RID: 14298 RVA: 0x00161B38 File Offset: 0x0015FD38
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

		// Token: 0x060037DB RID: 14299 RVA: 0x00161E54 File Offset: 0x00160054
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

		// Token: 0x060037DC RID: 14300 RVA: 0x00161F14 File Offset: 0x00160114
		public static void Clear()
		{
			for (int i = 0; i < NoiseDebugUI.noises2D.Count; i++)
			{
				UnityEngine.Object.Destroy(NoiseDebugUI.noises2D[i].Texture);
			}
			NoiseDebugUI.noises2D.Clear();
			NoiseDebugUI.ClearPlanetNoises();
		}

		// Token: 0x060037DD RID: 14301 RVA: 0x00161F5C File Offset: 0x0016015C
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

		// Token: 0x060037DE RID: 14302 RVA: 0x00161FB4 File Offset: 0x001601B4
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

		// Token: 0x040026B6 RID: 9910
		private static List<NoiseDebugUI.Noise2D> noises2D = new List<NoiseDebugUI.Noise2D>();

		// Token: 0x040026B7 RID: 9911
		private static List<NoiseDebugUI.NoisePlanet> planetNoises = new List<NoiseDebugUI.NoisePlanet>();

		// Token: 0x040026B8 RID: 9912
		private static Mesh planetNoiseMesh;

		// Token: 0x040026B9 RID: 9913
		private static NoiseDebugUI.NoisePlanet currentPlanetNoise;

		// Token: 0x040026BA RID: 9914
		private static NoiseDebugUI.NoisePlanet lastDrawnPlanetNoise;

		// Token: 0x040026BB RID: 9915
		private static List<Color32> planetNoiseMeshColors = new List<Color32>();

		// Token: 0x040026BC RID: 9916
		private static List<Vector3> planetNoiseMeshVerts;

		// Token: 0x020008C5 RID: 2245
		private class Noise2D
		{
			// Token: 0x170008CA RID: 2250
			// (get) Token: 0x060037E0 RID: 14304 RVA: 0x0002B2C6 File Offset: 0x000294C6
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

			// Token: 0x060037E1 RID: 14305 RVA: 0x0002B2ED File Offset: 0x000294ED
			public Noise2D(Texture2D tex, string name)
			{
				this.tex = tex;
				this.name = name;
			}

			// Token: 0x060037E2 RID: 14306 RVA: 0x0002B303 File Offset: 0x00029503
			public Noise2D(ModuleBase noise, string name)
			{
				this.noise = noise;
				this.name = name;
			}

			// Token: 0x040026BD RID: 9917
			public string name;

			// Token: 0x040026BE RID: 9918
			private Texture2D tex;

			// Token: 0x040026BF RID: 9919
			private ModuleBase noise;
		}

		// Token: 0x020008C6 RID: 2246
		private class NoisePlanet
		{
			// Token: 0x060037E3 RID: 14307 RVA: 0x0002B319 File Offset: 0x00029519
			public NoisePlanet(ModuleBase noise, string name)
			{
				this.name = name;
				this.noise = noise;
			}

			// Token: 0x040026C0 RID: 9920
			public string name;

			// Token: 0x040026C1 RID: 9921
			public ModuleBase noise;
		}
	}
}
