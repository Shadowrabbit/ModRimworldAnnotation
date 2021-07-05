using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000345 RID: 837
	public struct GraphicRequest : IEquatable<GraphicRequest>
	{
		// Token: 0x060017EC RID: 6124 RVA: 0x0008E668 File Offset: 0x0008C868
		public GraphicRequest(Type graphicClass, string path, Shader shader, Vector2 drawSize, Color color, Color colorTwo, GraphicData graphicData, int renderQueue, List<ShaderParameter> shaderParameters, string maskPath)
		{
			this.graphicClass = graphicClass;
			this.path = path;
			this.maskPath = maskPath;
			this.shader = shader;
			this.drawSize = drawSize;
			this.color = color;
			this.colorTwo = colorTwo;
			this.graphicData = graphicData;
			this.renderQueue = renderQueue;
			this.shaderParameters = (shaderParameters.NullOrEmpty<ShaderParameter>() ? null : shaderParameters);
		}

		// Token: 0x060017ED RID: 6125 RVA: 0x0008E6D0 File Offset: 0x0008C8D0
		public override int GetHashCode()
		{
			if (this.path == null)
			{
				this.path = BaseContent.BadTexPath;
			}
			return Gen.HashCombine<List<ShaderParameter>>(Gen.HashCombine<int>(Gen.HashCombine<GraphicData>(Gen.HashCombineStruct<Color>(Gen.HashCombineStruct<Color>(Gen.HashCombineStruct<Vector2>(Gen.HashCombine<Shader>(Gen.HashCombine<string>(Gen.HashCombine<string>(Gen.HashCombine<Type>(0, this.graphicClass), this.path), this.maskPath), this.shader), this.drawSize), this.color), this.colorTwo), this.graphicData), this.renderQueue), this.shaderParameters);
		}

		// Token: 0x060017EE RID: 6126 RVA: 0x0008E75F File Offset: 0x0008C95F
		public override bool Equals(object obj)
		{
			return obj is GraphicRequest && this.Equals((GraphicRequest)obj);
		}

		// Token: 0x060017EF RID: 6127 RVA: 0x0008E778 File Offset: 0x0008C978
		public bool Equals(GraphicRequest other)
		{
			return this.graphicClass == other.graphicClass && this.path == other.path && this.maskPath == other.maskPath && this.shader == other.shader && this.drawSize == other.drawSize && this.color == other.color && this.colorTwo == other.colorTwo && this.graphicData == other.graphicData && this.renderQueue == other.renderQueue && this.shaderParameters == other.shaderParameters;
		}

		// Token: 0x060017F0 RID: 6128 RVA: 0x0008E83C File Offset: 0x0008CA3C
		public static bool operator ==(GraphicRequest lhs, GraphicRequest rhs)
		{
			return lhs.Equals(rhs);
		}

		// Token: 0x060017F1 RID: 6129 RVA: 0x0008E846 File Offset: 0x0008CA46
		public static bool operator !=(GraphicRequest lhs, GraphicRequest rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x04001067 RID: 4199
		public Type graphicClass;

		// Token: 0x04001068 RID: 4200
		public string path;

		// Token: 0x04001069 RID: 4201
		public string maskPath;

		// Token: 0x0400106A RID: 4202
		public Shader shader;

		// Token: 0x0400106B RID: 4203
		public Vector2 drawSize;

		// Token: 0x0400106C RID: 4204
		public Color color;

		// Token: 0x0400106D RID: 4205
		public Color colorTwo;

		// Token: 0x0400106E RID: 4206
		public GraphicData graphicData;

		// Token: 0x0400106F RID: 4207
		public int renderQueue;

		// Token: 0x04001070 RID: 4208
		public List<ShaderParameter> shaderParameters;
	}
}
