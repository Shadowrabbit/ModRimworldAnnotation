using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000477 RID: 1143
	public struct MaterialRequest : IEquatable<MaterialRequest>
	{
		// Token: 0x17000583 RID: 1411
		// (set) Token: 0x06001CE7 RID: 7399 RVA: 0x0001A15A File Offset: 0x0001835A
		public string BaseTexPath
		{
			set
			{
				this.mainTex = ContentFinder<Texture2D>.Get(value, true);
			}
		}

		// Token: 0x06001CE8 RID: 7400 RVA: 0x0001A169 File Offset: 0x00018369
		public MaterialRequest(Texture2D tex)
		{
			this.shader = ShaderDatabase.Cutout;
			this.mainTex = tex;
			this.color = Color.white;
			this.colorTwo = Color.white;
			this.maskTex = null;
			this.renderQueue = 0;
			this.shaderParameters = null;
		}

		// Token: 0x06001CE9 RID: 7401 RVA: 0x0001A1A8 File Offset: 0x000183A8
		public MaterialRequest(Texture2D tex, Shader shader)
		{
			this.shader = shader;
			this.mainTex = tex;
			this.color = Color.white;
			this.colorTwo = Color.white;
			this.maskTex = null;
			this.renderQueue = 0;
			this.shaderParameters = null;
		}

		// Token: 0x06001CEA RID: 7402 RVA: 0x0001A1E3 File Offset: 0x000183E3
		public MaterialRequest(Texture2D tex, Shader shader, Color color)
		{
			this.shader = shader;
			this.mainTex = tex;
			this.color = color;
			this.colorTwo = Color.white;
			this.maskTex = null;
			this.renderQueue = 0;
			this.shaderParameters = null;
		}

		// Token: 0x06001CEB RID: 7403 RVA: 0x000F2238 File Offset: 0x000F0438
		public override int GetHashCode()
		{
			return Gen.HashCombine<List<ShaderParameter>>(Gen.HashCombineInt(Gen.HashCombine<Texture2D>(Gen.HashCombine<Texture2D>(Gen.HashCombineStruct<Color>(Gen.HashCombineStruct<Color>(Gen.HashCombine<Shader>(0, this.shader), this.color), this.colorTwo), this.mainTex), this.maskTex), this.renderQueue), this.shaderParameters);
		}

		// Token: 0x06001CEC RID: 7404 RVA: 0x0001A21A File Offset: 0x0001841A
		public override bool Equals(object obj)
		{
			return obj is MaterialRequest && this.Equals((MaterialRequest)obj);
		}

		// Token: 0x06001CED RID: 7405 RVA: 0x000F2294 File Offset: 0x000F0494
		public bool Equals(MaterialRequest other)
		{
			return other.shader == this.shader && other.mainTex == this.mainTex && other.color == this.color && other.colorTwo == this.colorTwo && other.maskTex == this.maskTex && other.renderQueue == this.renderQueue && other.shaderParameters == this.shaderParameters;
		}

		// Token: 0x06001CEE RID: 7406 RVA: 0x0001A232 File Offset: 0x00018432
		public static bool operator ==(MaterialRequest lhs, MaterialRequest rhs)
		{
			return lhs.Equals(rhs);
		}

		// Token: 0x06001CEF RID: 7407 RVA: 0x0001A23C File Offset: 0x0001843C
		public static bool operator !=(MaterialRequest lhs, MaterialRequest rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x06001CF0 RID: 7408 RVA: 0x000F2320 File Offset: 0x000F0520
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"MaterialRequest(",
				this.shader.name,
				", ",
				this.mainTex.name,
				", ",
				this.color.ToString(),
				", ",
				this.colorTwo.ToString(),
				", ",
				this.maskTex.ToString(),
				", ",
				this.renderQueue.ToString(),
				")"
			});
		}

		// Token: 0x0400149D RID: 5277
		public Shader shader;

		// Token: 0x0400149E RID: 5278
		public Texture2D mainTex;

		// Token: 0x0400149F RID: 5279
		public Color color;

		// Token: 0x040014A0 RID: 5280
		public Color colorTwo;

		// Token: 0x040014A1 RID: 5281
		public Texture2D maskTex;

		// Token: 0x040014A2 RID: 5282
		public int renderQueue;

		// Token: 0x040014A3 RID: 5283
		public List<ShaderParameter> shaderParameters;
	}
}
