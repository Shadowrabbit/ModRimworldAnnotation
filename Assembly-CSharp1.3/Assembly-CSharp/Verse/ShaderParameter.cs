using System;
using System.Xml;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000049 RID: 73
	public class ShaderParameter
	{
		// Token: 0x060003BE RID: 958 RVA: 0x00014A18 File Offset: 0x00012C18
		public void Apply(Material mat)
		{
			switch (this.type)
			{
			case ShaderParameter.Type.Float:
				mat.SetFloat(this.name, this.value.x);
				return;
			case ShaderParameter.Type.Vector:
				mat.SetVector(this.name, this.value);
				return;
			case ShaderParameter.Type.Matrix:
				break;
			case ShaderParameter.Type.Texture:
				if (this.valueTex == null)
				{
					Log.ErrorOnce(string.Format("Texture for {0} is not yet loaded; file may be invalid, or main thread may not have loaded it yet", this.name), 27929440);
				}
				mat.SetTexture(this.name, this.valueTex);
				break;
			default:
				return;
			}
		}

		// Token: 0x060003BF RID: 959 RVA: 0x00014AA8 File Offset: 0x00012CA8
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			if (xmlRoot.ChildNodes.Count != 1)
			{
				Log.Error("Misconfigured ShaderParameter: " + xmlRoot.OuterXml);
				return;
			}
			this.name = xmlRoot.Name;
			string valstr = xmlRoot.FirstChild.Value;
			if (!valstr.NullOrEmpty() && valstr[0] == '(')
			{
				this.value = ParseHelper.FromStringVector4Adaptive(valstr);
				this.type = ShaderParameter.Type.Vector;
				return;
			}
			if (!valstr.NullOrEmpty() && valstr[0] == '/')
			{
				LongEventHandler.ExecuteWhenFinished(delegate
				{
					this.valueTex = ContentFinder<Texture2D>.Get(valstr.TrimStart(new char[]
					{
						'/'
					}), true);
				});
				this.type = ShaderParameter.Type.Texture;
				return;
			}
			this.value = Vector4.one * ParseHelper.FromString<float>(valstr);
			this.type = ShaderParameter.Type.Float;
		}

		// Token: 0x04000113 RID: 275
		[NoTranslate]
		private string name;

		// Token: 0x04000114 RID: 276
		private Vector4 value;

		// Token: 0x04000115 RID: 277
		private Texture2D valueTex;

		// Token: 0x04000116 RID: 278
		private ShaderParameter.Type type;

		// Token: 0x020018A1 RID: 6305
		private enum Type
		{
			// Token: 0x04005E35 RID: 24117
			Float,
			// Token: 0x04005E36 RID: 24118
			Vector,
			// Token: 0x04005E37 RID: 24119
			Matrix,
			// Token: 0x04005E38 RID: 24120
			Texture
		}
	}
}
