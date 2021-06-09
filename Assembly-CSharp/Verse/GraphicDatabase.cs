using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004EA RID: 1258
	public static class GraphicDatabase
	{
		// Token: 0x06001F64 RID: 8036 RVA: 0x000FFB0C File Offset: 0x000FDD0C
		public static Graphic Get<T>(string path) where T : Graphic, new()
		{
			return GraphicDatabase.GetInner<T>(new GraphicRequest(typeof(T), path, ShaderDatabase.Cutout, Vector2.one, Color.white, Color.white, null, 0, null));
		}

		// Token: 0x06001F65 RID: 8037 RVA: 0x000FFB4C File Offset: 0x000FDD4C
		public static Graphic Get<T>(string path, Shader shader) where T : Graphic, new()
		{
			return GraphicDatabase.GetInner<T>(new GraphicRequest(typeof(T), path, shader, Vector2.one, Color.white, Color.white, null, 0, null));
		}

		// Token: 0x06001F66 RID: 8038 RVA: 0x000FFB88 File Offset: 0x000FDD88
		public static Graphic Get<T>(string path, Shader shader, Vector2 drawSize, Color color) where T : Graphic, new()
		{
			return GraphicDatabase.GetInner<T>(new GraphicRequest(typeof(T), path, shader, drawSize, color, Color.white, null, 0, null));
		}

		// Token: 0x06001F67 RID: 8039 RVA: 0x000FFBBC File Offset: 0x000FDDBC
		public static Graphic Get<T>(string path, Shader shader, Vector2 drawSize, Color color, int renderQueue) where T : Graphic, new()
		{
			return GraphicDatabase.GetInner<T>(new GraphicRequest(typeof(T), path, shader, drawSize, color, Color.white, null, renderQueue, null));
		}

		// Token: 0x06001F68 RID: 8040 RVA: 0x000FFBF0 File Offset: 0x000FDDF0
		public static Graphic Get<T>(string path, Shader shader, Vector2 drawSize, Color color, Color colorTwo) where T : Graphic, new()
		{
			return GraphicDatabase.GetInner<T>(new GraphicRequest(typeof(T), path, shader, drawSize, color, colorTwo, null, 0, null));
		}

		// Token: 0x06001F69 RID: 8041 RVA: 0x000FFC20 File Offset: 0x000FDE20
		public static Graphic Get<T>(string path, Shader shader, Vector2 drawSize, Color color, Color colorTwo, GraphicData data) where T : Graphic, new()
		{
			return GraphicDatabase.GetInner<T>(new GraphicRequest(typeof(T), path, shader, drawSize, color, colorTwo, data, 0, null));
		}

		// Token: 0x06001F6A RID: 8042 RVA: 0x0001BAE8 File Offset: 0x00019CE8
		public static Graphic Get(Type graphicClass, string path, Shader shader, Vector2 drawSize, Color color, Color colorTwo)
		{
			return GraphicDatabase.Get(graphicClass, path, shader, drawSize, color, colorTwo, null, null);
		}

		// Token: 0x06001F6B RID: 8043 RVA: 0x000FFC50 File Offset: 0x000FDE50
		public static Graphic Get(Type graphicClass, string path, Shader shader, Vector2 drawSize, Color color, Color colorTwo, GraphicData data, List<ShaderParameter> shaderParameters)
		{
			GraphicRequest graphicRequest = new GraphicRequest(graphicClass, path, shader, drawSize, color, colorTwo, data, 0, shaderParameters);
			if (graphicRequest.graphicClass == typeof(Graphic_Single))
			{
				return GraphicDatabase.GetInner<Graphic_Single>(graphicRequest);
			}
			if (graphicRequest.graphicClass == typeof(Graphic_Terrain))
			{
				return GraphicDatabase.GetInner<Graphic_Terrain>(graphicRequest);
			}
			if (graphicRequest.graphicClass == typeof(Graphic_Multi))
			{
				return GraphicDatabase.GetInner<Graphic_Multi>(graphicRequest);
			}
			if (graphicRequest.graphicClass == typeof(Graphic_Mote))
			{
				return GraphicDatabase.GetInner<Graphic_Mote>(graphicRequest);
			}
			if (graphicRequest.graphicClass == typeof(Graphic_Random))
			{
				return GraphicDatabase.GetInner<Graphic_Random>(graphicRequest);
			}
			if (graphicRequest.graphicClass == typeof(Graphic_Flicker))
			{
				return GraphicDatabase.GetInner<Graphic_Flicker>(graphicRequest);
			}
			if (graphicRequest.graphicClass == typeof(Graphic_Appearances))
			{
				return GraphicDatabase.GetInner<Graphic_Appearances>(graphicRequest);
			}
			if (graphicRequest.graphicClass == typeof(Graphic_StackCount))
			{
				return GraphicDatabase.GetInner<Graphic_StackCount>(graphicRequest);
			}
			try
			{
				return (Graphic)GenGeneric.InvokeStaticGenericMethod(typeof(GraphicDatabase), graphicRequest.graphicClass, "GetInner", new object[]
				{
					graphicRequest
				});
			}
			catch (Exception ex)
			{
				Log.Error(string.Concat(new object[]
				{
					"Exception getting ",
					graphicClass,
					" at ",
					path,
					": ",
					ex.ToString()
				}), false);
			}
			return BaseContent.BadGraphic;
		}

		// Token: 0x06001F6C RID: 8044 RVA: 0x000FFDE8 File Offset: 0x000FDFE8
		private static T GetInner<T>(GraphicRequest req) where T : Graphic, new()
		{
			req.color = req.color;
			req.colorTwo = req.colorTwo;
			Graphic graphic;
			if (!GraphicDatabase.allGraphics.TryGetValue(req, out graphic))
			{
				graphic = Activator.CreateInstance<T>();
				graphic.Init(req);
				GraphicDatabase.allGraphics.Add(req, graphic);
			}
			return (T)((object)graphic);
		}

		// Token: 0x06001F6D RID: 8045 RVA: 0x0001BAF9 File Offset: 0x00019CF9
		public static void Clear()
		{
			GraphicDatabase.allGraphics.Clear();
		}

		// Token: 0x06001F6E RID: 8046 RVA: 0x000FFE58 File Offset: 0x000FE058
		[DebugOutput("System", false)]
		public static void AllGraphicsLoaded()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("There are " + GraphicDatabase.allGraphics.Count + " graphics loaded.");
			int num = 0;
			foreach (Graphic graphic in GraphicDatabase.allGraphics.Values)
			{
				stringBuilder.AppendLine(num + " - " + graphic.ToString());
				if (num % 50 == 49)
				{
					Log.Message(stringBuilder.ToString(), false);
					stringBuilder = new StringBuilder();
				}
				num++;
			}
			Log.Message(stringBuilder.ToString(), false);
		}

		// Token: 0x0400160B RID: 5643
		private static Dictionary<GraphicRequest, Graphic> allGraphics = new Dictionary<GraphicRequest, Graphic>();
	}
}
