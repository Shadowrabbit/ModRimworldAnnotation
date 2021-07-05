using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000346 RID: 838
	public static class GraphicDatabase
	{
		// Token: 0x060017F2 RID: 6130 RVA: 0x0008E854 File Offset: 0x0008CA54
		public static Graphic Get<T>(string path) where T : Graphic, new()
		{
			return GraphicDatabase.GetInner<T>(new GraphicRequest(typeof(T), path, ShaderDatabase.Cutout, Vector2.one, Color.white, Color.white, null, 0, null, null));
		}

		// Token: 0x060017F3 RID: 6131 RVA: 0x0008E894 File Offset: 0x0008CA94
		public static Graphic Get<T>(string path, Shader shader) where T : Graphic, new()
		{
			return GraphicDatabase.GetInner<T>(new GraphicRequest(typeof(T), path, shader, Vector2.one, Color.white, Color.white, null, 0, null, null));
		}

		// Token: 0x060017F4 RID: 6132 RVA: 0x0008E8D0 File Offset: 0x0008CAD0
		public static Graphic Get<T>(string path, Shader shader, Vector2 drawSize, Color color) where T : Graphic, new()
		{
			return GraphicDatabase.GetInner<T>(new GraphicRequest(typeof(T), path, shader, drawSize, color, Color.white, null, 0, null, null));
		}

		// Token: 0x060017F5 RID: 6133 RVA: 0x0008E904 File Offset: 0x0008CB04
		public static Graphic Get<T>(string path, Shader shader, Vector2 drawSize, Color color, int renderQueue) where T : Graphic, new()
		{
			return GraphicDatabase.GetInner<T>(new GraphicRequest(typeof(T), path, shader, drawSize, color, Color.white, null, renderQueue, null, null));
		}

		// Token: 0x060017F6 RID: 6134 RVA: 0x0008E938 File Offset: 0x0008CB38
		public static Graphic Get<T>(string path, Shader shader, Vector2 drawSize, Color color, Color colorTwo) where T : Graphic, new()
		{
			return GraphicDatabase.GetInner<T>(new GraphicRequest(typeof(T), path, shader, drawSize, color, colorTwo, null, 0, null, null));
		}

		// Token: 0x060017F7 RID: 6135 RVA: 0x0008E968 File Offset: 0x0008CB68
		public static Graphic Get<T>(string path, Shader shader, Vector2 drawSize, Color color, Color colorTwo, GraphicData data, string maskPath = null) where T : Graphic, new()
		{
			return GraphicDatabase.GetInner<T>(new GraphicRequest(typeof(T), path, shader, drawSize, color, colorTwo, data, 0, null, maskPath));
		}

		// Token: 0x060017F8 RID: 6136 RVA: 0x0008E99C File Offset: 0x0008CB9C
		public static Graphic Get(Type graphicClass, string path, Shader shader, Vector2 drawSize, Color color, Color colorTwo, string maskPath = null)
		{
			return GraphicDatabase.Get(graphicClass, path, shader, drawSize, color, colorTwo, null, null, maskPath);
		}

		// Token: 0x060017F9 RID: 6137 RVA: 0x0008E9BC File Offset: 0x0008CBBC
		public static Graphic Get(Type graphicClass, string path, Shader shader, Vector2 drawSize, Color color, Color colorTwo, GraphicData data, List<ShaderParameter> shaderParameters, string maskPath = null)
		{
			GraphicRequest graphicRequest = new GraphicRequest(graphicClass, path, shader, drawSize, color, colorTwo, data, 0, shaderParameters, maskPath);
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
				}));
			}
			return BaseContent.BadGraphic;
		}

		// Token: 0x060017FA RID: 6138 RVA: 0x0008EB54 File Offset: 0x0008CD54
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

		// Token: 0x060017FB RID: 6139 RVA: 0x0008EBC2 File Offset: 0x0008CDC2
		public static void Clear()
		{
			GraphicDatabase.allGraphics.Clear();
		}

		// Token: 0x060017FC RID: 6140 RVA: 0x0008EBD0 File Offset: 0x0008CDD0
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
					Log.Message(stringBuilder.ToString());
					stringBuilder = new StringBuilder();
				}
				num++;
			}
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x04001071 RID: 4209
		private static Dictionary<GraphicRequest, Graphic> allGraphics = new Dictionary<GraphicRequest, Graphic>();
	}
}
