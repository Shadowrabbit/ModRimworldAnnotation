using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000307 RID: 775
	public static class MaterialPool
	{
		// Token: 0x06001654 RID: 5716 RVA: 0x0008220E File Offset: 0x0008040E
		public static Material MatFrom(string texPath, bool reportFailure)
		{
			if (texPath == null || texPath == "null")
			{
				return null;
			}
			return MaterialPool.MatFrom(new MaterialRequest(ContentFinder<Texture2D>.Get(texPath, reportFailure)));
		}

		// Token: 0x06001655 RID: 5717 RVA: 0x00082233 File Offset: 0x00080433
		public static Material MatFrom(string texPath)
		{
			if (texPath == null || texPath == "null")
			{
				return null;
			}
			return MaterialPool.MatFrom(new MaterialRequest(ContentFinder<Texture2D>.Get(texPath, true)));
		}

		// Token: 0x06001656 RID: 5718 RVA: 0x00082258 File Offset: 0x00080458
		public static Material MatFrom(Texture2D srcTex)
		{
			return MaterialPool.MatFrom(new MaterialRequest(srcTex));
		}

		// Token: 0x06001657 RID: 5719 RVA: 0x00082265 File Offset: 0x00080465
		public static Material MatFrom(Texture2D srcTex, Shader shader, Color color)
		{
			return MaterialPool.MatFrom(new MaterialRequest(srcTex, shader, color));
		}

		// Token: 0x06001658 RID: 5720 RVA: 0x00082274 File Offset: 0x00080474
		public static Material MatFrom(Texture2D srcTex, Shader shader, Color color, int renderQueue)
		{
			return MaterialPool.MatFrom(new MaterialRequest(srcTex, shader, color)
			{
				renderQueue = renderQueue
			});
		}

		// Token: 0x06001659 RID: 5721 RVA: 0x00082299 File Offset: 0x00080499
		public static Material MatFrom(string texPath, Shader shader)
		{
			return MaterialPool.MatFrom(new MaterialRequest(ContentFinder<Texture2D>.Get(texPath, true), shader));
		}

		// Token: 0x0600165A RID: 5722 RVA: 0x000822B0 File Offset: 0x000804B0
		public static Material MatFrom(string texPath, Shader shader, int renderQueue)
		{
			return MaterialPool.MatFrom(new MaterialRequest(ContentFinder<Texture2D>.Get(texPath, true), shader)
			{
				renderQueue = renderQueue
			});
		}

		// Token: 0x0600165B RID: 5723 RVA: 0x000822DA File Offset: 0x000804DA
		public static Material MatFrom(string texPath, Shader shader, Color color)
		{
			return MaterialPool.MatFrom(new MaterialRequest(ContentFinder<Texture2D>.Get(texPath, true), shader, color));
		}

		// Token: 0x0600165C RID: 5724 RVA: 0x000822F0 File Offset: 0x000804F0
		public static Material MatFrom(string texPath, Shader shader, Color color, int renderQueue)
		{
			return MaterialPool.MatFrom(new MaterialRequest(ContentFinder<Texture2D>.Get(texPath, true), shader, color)
			{
				renderQueue = renderQueue
			});
		}

		// Token: 0x0600165D RID: 5725 RVA: 0x0008231B File Offset: 0x0008051B
		public static Material MatFrom(Shader shader)
		{
			return MaterialPool.MatFrom(new MaterialRequest(shader));
		}

		// Token: 0x0600165E RID: 5726 RVA: 0x00082328 File Offset: 0x00080528
		public static Material MatFrom(MaterialRequest req)
		{
			if (!UnityData.IsInMainThread)
			{
				Log.Error("Tried to get a material from a different thread.");
				return null;
			}
			if (req.mainTex == null && req.needsMainTex)
			{
				Log.Error("MatFrom with null sourceTex.");
				return BaseContent.BadMat;
			}
			if (req.shader == null)
			{
				Log.Warning("Matfrom with null shader.");
				return BaseContent.BadMat;
			}
			if (req.maskTex != null && !req.shader.SupportsMaskTex())
			{
				Log.Error("MaterialRequest has maskTex but shader does not support it. req=" + req.ToString());
				req.maskTex = null;
			}
			req.color = req.color;
			req.colorTwo = req.colorTwo;
			Material material;
			if (!MaterialPool.matDictionary.TryGetValue(req, out material))
			{
				material = MaterialAllocator.Create(req.shader);
				material.name = req.shader.name;
				if (req.mainTex != null)
				{
					Material material2 = material;
					material2.name = material2.name + "_" + req.mainTex.name;
					material.mainTexture = req.mainTex;
				}
				material.color = req.color;
				if (req.maskTex != null)
				{
					material.SetTexture(ShaderPropertyIDs.MaskTex, req.maskTex);
					material.SetColor(ShaderPropertyIDs.ColorTwo, req.colorTwo);
				}
				if (req.renderQueue != 0)
				{
					material.renderQueue = req.renderQueue;
				}
				if (!req.shaderParameters.NullOrEmpty<ShaderParameter>())
				{
					for (int i = 0; i < req.shaderParameters.Count; i++)
					{
						req.shaderParameters[i].Apply(material);
					}
				}
				MaterialPool.matDictionary.Add(req, material);
				MaterialPool.matDictionaryReverse.Add(material, req);
				if (req.shader == ShaderDatabase.CutoutPlant || req.shader == ShaderDatabase.TransparentPlant)
				{
					WindManager.Notify_PlantMaterialCreated(material);
				}
			}
			return material;
		}

		// Token: 0x0600165F RID: 5727 RVA: 0x0008252B File Offset: 0x0008072B
		public static bool TryGetRequestForMat(Material material, out MaterialRequest request)
		{
			return MaterialPool.matDictionaryReverse.TryGetValue(material, out request);
		}

		// Token: 0x04000F87 RID: 3975
		private static Dictionary<MaterialRequest, Material> matDictionary = new Dictionary<MaterialRequest, Material>();

		// Token: 0x04000F88 RID: 3976
		private static Dictionary<Material, MaterialRequest> matDictionaryReverse = new Dictionary<Material, MaterialRequest>();
	}
}
