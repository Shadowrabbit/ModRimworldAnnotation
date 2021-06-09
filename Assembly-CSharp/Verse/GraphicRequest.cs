using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004E9 RID: 1257
	public struct GraphicRequest : IEquatable<GraphicRequest>
	{
		// Token: 0x06001F5E RID: 8030 RVA: 0x000FF978 File Offset: 0x000FDB78
		public GraphicRequest(Type graphicClass, string path, Shader shader, Vector2 drawSize, Color color, Color colorTwo, GraphicData graphicData, int renderQueue, List<ShaderParameter> shaderParameters)
		{
			this.graphicClass = graphicClass;
			this.path = path;
			this.shader = shader;
			this.drawSize = drawSize;
			this.color = color;
			this.colorTwo = colorTwo;
			this.graphicData = graphicData;
			this.renderQueue = renderQueue;
			this.shaderParameters = (shaderParameters.NullOrEmpty<ShaderParameter>() ? null : shaderParameters);
		}

		// Token: 0x06001F5F RID: 8031 RVA: 0x000FF9D8 File Offset: 0x000FDBD8
		public override int GetHashCode()
		{
			if (this.path == null)
			{
				this.path = BaseContent.BadTexPath;
			}
			return Gen.HashCombine<List<ShaderParameter>>(Gen.HashCombine<int>(Gen.HashCombine<GraphicData>(Gen.HashCombineStruct<Color>(Gen.HashCombineStruct<Color>(Gen.HashCombineStruct<Vector2>(Gen.HashCombine<Shader>(Gen.HashCombine<string>(Gen.HashCombine<Type>(0, this.graphicClass), this.path), this.shader), this.drawSize), this.color), this.colorTwo), this.graphicData), this.renderQueue), this.shaderParameters);
		}

		// Token: 0x06001F60 RID: 8032 RVA: 0x0001BABA File Offset: 0x00019CBA
		public override bool Equals(object obj)
		{
			return obj is GraphicRequest && this.Equals((GraphicRequest)obj);
		}

		// Token: 0x06001F61 RID: 8033 RVA: 0x000FFA5C File Offset: 0x000FDC5C
		public bool Equals(GraphicRequest other)
		{
			return this.graphicClass == other.graphicClass && this.path == other.path && this.shader == other.shader && this.drawSize == other.drawSize && this.color == other.color && this.colorTwo == other.colorTwo && this.graphicData == other.graphicData && this.renderQueue == other.renderQueue && this.shaderParameters == other.shaderParameters;
		}

		// Token: 0x06001F62 RID: 8034 RVA: 0x0001BAD2 File Offset: 0x00019CD2
		public static bool operator ==(GraphicRequest lhs, GraphicRequest rhs)
		{
			return lhs.Equals(rhs);
		}

		// Token: 0x06001F63 RID: 8035 RVA: 0x0001BADC File Offset: 0x00019CDC
		public static bool operator !=(GraphicRequest lhs, GraphicRequest rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x04001602 RID: 5634
		public Type graphicClass;

		// Token: 0x04001603 RID: 5635
		public string path;

		// Token: 0x04001604 RID: 5636
		public Shader shader;

		// Token: 0x04001605 RID: 5637
		public Vector2 drawSize;

		// Token: 0x04001606 RID: 5638
		public Color color;

		// Token: 0x04001607 RID: 5639
		public Color colorTwo;

		// Token: 0x04001608 RID: 5640
		public GraphicData graphicData;

		// Token: 0x04001609 RID: 5641
		public int renderQueue;

		// Token: 0x0400160A RID: 5642
		public List<ShaderParameter> shaderParameters;
	}
}
