using System;
using System.Xml;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200008D RID: 141
	public class ShaderParameter
	{
		// Token: 0x0600050D RID: 1293 RVA: 0x0008AAA0 File Offset: 0x00088CA0
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
					Log.ErrorOnce(string.Format("Texture for {0} is not yet loaded; file may be invalid, or main thread may not have loaded it yet", this.name), 27929440, false);
				}
				mat.SetTexture(this.name, this.valueTex);
				break;
			default:
				return;
			}
		}

		// Token: 0x0600050E RID: 1294 RVA: 0x0008AB34 File Offset: 0x00088D34
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			if (xmlRoot.ChildNodes.Count != 1)
			{
				Log.Error("Misconfigured ShaderParameter: " + xmlRoot.OuterXml, false);
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

		// Token: 0x04000274 RID: 628
		[NoTranslate]
		private string name;

		// Token: 0x04000275 RID: 629
		private Vector4 value;

		// Token: 0x04000276 RID: 630
		private Texture2D valueTex;

		// Token: 0x04000277 RID: 631
		private ShaderParameter.Type type;

		// Token: 0x0200008E RID: 142
		private enum Type
		{
			// Token: 0x04000279 RID: 633
			Float,
			// Token: 0x0400027A RID: 634
			Vector,
			// Token: 0x0400027B RID: 635
			Matrix,
			// Token: 0x0400027C RID: 636
			Texture
		}
	}
}
