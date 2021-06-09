using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200084A RID: 2122
	public static class GenGeo
	{
		// Token: 0x0600352F RID: 13615 RVA: 0x00157128 File Offset: 0x00155328
		public static float AngleDifferenceBetween(float A, float B)
		{
			float num = A + 360f;
			float num2 = B + 360f;
			float num3 = 9999f;
			float num4 = A - B;
			if (num4 < 0f)
			{
				num4 *= -1f;
			}
			if (num4 < num3)
			{
				num3 = num4;
			}
			num4 = num - B;
			if (num4 < 0f)
			{
				num4 *= -1f;
			}
			if (num4 < num3)
			{
				num3 = num4;
			}
			num4 = A - num2;
			if (num4 < 0f)
			{
				num4 *= -1f;
			}
			if (num4 < num3)
			{
				num3 = num4;
			}
			return num3;
		}

		// Token: 0x06003530 RID: 13616 RVA: 0x000296CC File Offset: 0x000278CC
		public static float MagnitudeHorizontal(this Vector3 v)
		{
			return (float)Math.Sqrt((double)(v.x * v.x + v.z * v.z));
		}

		// Token: 0x06003531 RID: 13617 RVA: 0x000296F0 File Offset: 0x000278F0
		public static float MagnitudeHorizontalSquared(this Vector3 v)
		{
			return v.x * v.x + v.z * v.z;
		}

		// Token: 0x06003532 RID: 13618 RVA: 0x001571A0 File Offset: 0x001553A0
		public static bool LinesIntersect(Vector3 line1V1, Vector3 line1V2, Vector3 line2V1, Vector3 line2V2)
		{
			float num = line1V2.z - line1V1.z;
			float num2 = line1V1.x - line1V2.x;
			float num3 = num * line1V1.x + num2 * line1V1.z;
			float num4 = line2V2.z - line2V1.z;
			float num5 = line2V1.x - line2V2.x;
			float num6 = num4 * line2V1.x + num5 * line2V1.z;
			float num7 = num * num5 - num4 * num2;
			if (num7 == 0f)
			{
				return false;
			}
			float num8 = (num5 * num3 - num2 * num6) / num7;
			float num9 = (num * num6 - num4 * num3) / num7;
			return (num8 <= line1V1.x || num8 <= line1V2.x) && (num8 <= line2V1.x || num8 <= line2V2.x) && (num8 >= line1V1.x || num8 >= line1V2.x) && (num8 >= line2V1.x || num8 >= line2V2.x) && (num9 <= line1V1.z || num9 <= line1V2.z) && (num9 <= line2V1.z || num9 <= line2V2.z) && (num9 >= line1V1.z || num9 >= line1V2.z) && (num9 >= line2V1.z || num9 >= line2V2.z);
		}

		// Token: 0x06003533 RID: 13619 RVA: 0x001572E4 File Offset: 0x001554E4
		public static bool IntersectLineCircle(Vector2 center, float radius, Vector2 lineA, Vector2 lineB)
		{
			Vector2 lhs = center - lineA;
			Vector2 vector = lineB - lineA;
			float num = Vector2.Dot(vector, vector);
			float num2 = Vector2.Dot(lhs, vector) / num;
			if (num2 < 0f)
			{
				num2 = 0f;
			}
			else if (num2 > 1f)
			{
				num2 = 1f;
			}
			Vector2 vector2 = vector * num2 + lineA - center;
			return Vector2.Dot(vector2, vector2) <= radius * radius;
		}

		// Token: 0x06003534 RID: 13620 RVA: 0x00157350 File Offset: 0x00155550
		public static bool IntersectLineCircleOutline(Vector2 center, float radius, Vector2 lineA, Vector2 lineB)
		{
			bool flag = (lineA - center).sqrMagnitude <= radius * radius;
			bool flag2 = (lineB - center).sqrMagnitude <= radius * radius;
			return (!flag || !flag2) && GenGeo.IntersectLineCircle(center, radius, lineA, lineB);
		}

		// Token: 0x06003535 RID: 13621 RVA: 0x0015739C File Offset: 0x0015559C
		public static Vector3 RegularPolygonVertexPositionVec3(int polygonVertices, int vertexIndex)
		{
			Vector2 vector = GenGeo.RegularPolygonVertexPosition(polygonVertices, vertexIndex);
			return new Vector3(vector.x, 0f, vector.y);
		}

		// Token: 0x06003536 RID: 13622 RVA: 0x001573C8 File Offset: 0x001555C8
		public static Vector2 RegularPolygonVertexPosition(int polygonVertices, int vertexIndex)
		{
			if (vertexIndex < 0 || vertexIndex >= polygonVertices)
			{
				Log.Warning(string.Concat(new object[]
				{
					"Vertex index out of bounds. polygonVertices=",
					polygonVertices,
					" vertexIndex=",
					vertexIndex
				}), false);
				return Vector2.zero;
			}
			if (polygonVertices == 1)
			{
				return Vector2.zero;
			}
			return GenGeo.CalculatePolygonVertexPosition(polygonVertices, vertexIndex);
		}

		// Token: 0x06003537 RID: 13623 RVA: 0x00157428 File Offset: 0x00155628
		private static Vector2 CalculatePolygonVertexPosition(int polygonVertices, int vertexIndex)
		{
			float num = 6.2831855f / (float)polygonVertices * (float)vertexIndex;
			num += 3.1415927f;
			return new Vector3(Mathf.Cos(num), Mathf.Sin(num));
		}

		// Token: 0x06003538 RID: 13624 RVA: 0x00157460 File Offset: 0x00155660
		public static Vector2 InverseQuadBilinear(Vector2 p, Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
		{
			float num = (p0 - p).Cross(p0 - p2);
			float num2 = ((p0 - p).Cross(p1 - p3) + (p1 - p).Cross(p0 - p2)) / 2f;
			float num3 = (p1 - p).Cross(p1 - p3);
			float num4 = num2 * num2 - num * num3;
			if (num4 < 0f)
			{
				return new Vector2(-1f, -1f);
			}
			num4 = Mathf.Sqrt(num4);
			float num5;
			if (Mathf.Abs(num - 2f * num2 + num3) < 0.0001f)
			{
				num5 = num / (num - num3);
			}
			else
			{
				float num6 = (num - num2 + num4) / (num - 2f * num2 + num3);
				float num7 = (num - num2 - num4) / (num - 2f * num2 + num3);
				if (Mathf.Abs(num6 - 0.5f) < Mathf.Abs(num7 - 0.5f))
				{
					num5 = num6;
				}
				else
				{
					num5 = num7;
				}
			}
			float num8 = (1f - num5) * (p0.x - p2.x) + num5 * (p1.x - p3.x);
			float num9 = (1f - num5) * (p0.y - p2.y) + num5 * (p1.y - p3.y);
			if (Mathf.Abs(num8) < Mathf.Abs(num9))
			{
				return new Vector2(num5, ((1f - num5) * (p0.y - p.y) + num5 * (p1.y - p.y)) / num9);
			}
			return new Vector2(num5, ((1f - num5) * (p0.x - p.x) + num5 * (p1.x - p.x)) / num8);
		}
	}
}
