using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000308 RID: 776
	public struct MaterialRequest : IEquatable<MaterialRequest>
	{
		// Token: 0x170004AE RID: 1198
		// (set) Token: 0x06001661 RID: 5729 RVA: 0x0008254F File Offset: 0x0008074F
		public string BaseTexPath
		{
			set
			{
				this.mainTex = ContentFinder<Texture2D>.Get(value, true);
			}
		}

		// Token: 0x06001662 RID: 5730 RVA: 0x00082560 File Offset: 0x00080760
		public MaterialRequest(Texture tex)
		{
			this.shader = ShaderDatabase.Cutout;
			this.mainTex = tex;
			this.color = Color.white;
			this.colorTwo = Color.white;
			this.maskTex = null;
			this.renderQueue = 0;
			this.shaderParameters = null;
			this.needsMainTex = true;
		}

		// Token: 0x06001663 RID: 5731 RVA: 0x000825B4 File Offset: 0x000807B4
		public MaterialRequest(Texture tex, Shader shader)
		{
			this.shader = shader;
			this.mainTex = tex;
			this.color = Color.white;
			this.colorTwo = Color.white;
			this.maskTex = null;
			this.renderQueue = 0;
			this.shaderParameters = null;
			this.needsMainTex = true;
		}

		// Token: 0x06001664 RID: 5732 RVA: 0x00082601 File Offset: 0x00080801
		public MaterialRequest(Texture tex, Shader shader, Color color)
		{
			this.shader = shader;
			this.mainTex = tex;
			this.color = color;
			this.colorTwo = Color.white;
			this.maskTex = null;
			this.renderQueue = 0;
			this.shaderParameters = null;
			this.needsMainTex = true;
		}

		// Token: 0x06001665 RID: 5733 RVA: 0x00082640 File Offset: 0x00080840
		public MaterialRequest(Shader shader)
		{
			this.shader = shader;
			this.mainTex = null;
			this.color = Color.white;
			this.colorTwo = Color.white;
			this.maskTex = null;
			this.renderQueue = 0;
			this.shaderParameters = null;
			this.needsMainTex = false;
		}

		// Token: 0x06001666 RID: 5734 RVA: 0x00082690 File Offset: 0x00080890
		public override int GetHashCode()
		{
			return Gen.HashCombine<List<ShaderParameter>>(Gen.HashCombineInt(Gen.HashCombine<Texture2D>(Gen.HashCombine<Texture>(Gen.HashCombineStruct<Color>(Gen.HashCombineStruct<Color>(Gen.HashCombine<Shader>(0, this.shader), this.color), this.colorTwo), this.mainTex), this.maskTex), this.renderQueue), this.shaderParameters);
		}

		// Token: 0x06001667 RID: 5735 RVA: 0x000826EB File Offset: 0x000808EB
		public override bool Equals(object obj)
		{
			return obj is MaterialRequest && this.Equals((MaterialRequest)obj);
		}

		// Token: 0x06001668 RID: 5736 RVA: 0x00082704 File Offset: 0x00080904
		public bool Equals(MaterialRequest other)
		{
			return other.shader == this.shader && other.mainTex == this.mainTex && other.color == this.color && other.colorTwo == this.colorTwo && other.maskTex == this.maskTex && other.renderQueue == this.renderQueue && other.shaderParameters == this.shaderParameters;
		}

		// Token: 0x06001669 RID: 5737 RVA: 0x0008278E File Offset: 0x0008098E
		public static bool operator ==(MaterialRequest lhs, MaterialRequest rhs)
		{
			return lhs.Equals(rhs);
		}

		// Token: 0x0600166A RID: 5738 RVA: 0x00082798 File Offset: 0x00080998
		public static bool operator !=(MaterialRequest lhs, MaterialRequest rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x0600166B RID: 5739 RVA: 0x000827A4 File Offset: 0x000809A4
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

		// Token: 0x04000F89 RID: 3977
		public Shader shader;

		// Token: 0x04000F8A RID: 3978
		public Texture mainTex;

		// Token: 0x04000F8B RID: 3979
		public Color color;

		// Token: 0x04000F8C RID: 3980
		public Color colorTwo;

		// Token: 0x04000F8D RID: 3981
		public Texture2D maskTex;

		// Token: 0x04000F8E RID: 3982
		public int renderQueue;

		// Token: 0x04000F8F RID: 3983
		public bool needsMainTex;

		// Token: 0x04000F90 RID: 3984
		public List<ShaderParameter> shaderParameters;
	}
}
