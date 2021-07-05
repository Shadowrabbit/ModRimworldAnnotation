using System;
using System.IO;
using System.Xml;

namespace Verse
{
	// Token: 0x0200032A RID: 810
	public class ScribeLoader
	{
		// Token: 0x06001713 RID: 5907 RVA: 0x000887C0 File Offset: 0x000869C0
		public void InitLoading(string filePath)
		{
			if (Scribe.mode != LoadSaveMode.Inactive)
			{
				Log.Error("Called InitLoading() but current mode is " + Scribe.mode);
				Scribe.ForceStop();
			}
			if (this.curParent != null)
			{
				Log.Error("Current parent is not null in InitLoading");
				this.curParent = null;
			}
			if (this.curPathRelToParent != null)
			{
				Log.Error("Current path relative to parent is not null in InitLoading");
				this.curPathRelToParent = null;
			}
			try
			{
				using (StreamReader streamReader = new StreamReader(filePath))
				{
					using (XmlTextReader xmlTextReader = new XmlTextReader(streamReader))
					{
						XmlDocument xmlDocument = new XmlDocument();
						xmlDocument.Load(xmlTextReader);
						this.curXmlParent = xmlDocument.DocumentElement;
					}
				}
				Scribe.mode = LoadSaveMode.LoadingVars;
			}
			catch (Exception ex)
			{
				Log.Error(string.Concat(new object[]
				{
					"Exception while init loading file: ",
					filePath,
					"\n",
					ex
				}));
				this.ForceStop();
				throw;
			}
		}

		// Token: 0x06001714 RID: 5908 RVA: 0x000888C8 File Offset: 0x00086AC8
		public void InitLoadingMetaHeaderOnly(string filePath)
		{
			if (Scribe.mode != LoadSaveMode.Inactive)
			{
				Log.Error("Called InitLoadingMetaHeaderOnly() but current mode is " + Scribe.mode);
				Scribe.ForceStop();
			}
			try
			{
				using (StreamReader streamReader = new StreamReader(filePath))
				{
					using (XmlTextReader xmlTextReader = new XmlTextReader(streamReader))
					{
						if (ScribeMetaHeaderUtility.ReadToMetaElement(xmlTextReader))
						{
							using (XmlReader xmlReader = xmlTextReader.ReadSubtree())
							{
								XmlDocument xmlDocument = new XmlDocument();
								xmlDocument.Load(xmlReader);
								XmlElement xmlElement = xmlDocument.CreateElement("root");
								xmlElement.AppendChild(xmlDocument.DocumentElement);
								this.curXmlParent = xmlElement;
								goto IL_81;
							}
							goto IL_7F;
							IL_81:
							goto IL_8D;
						}
						IL_7F:
						return;
					}
					IL_8D:;
				}
				Scribe.mode = LoadSaveMode.LoadingVars;
			}
			catch (Exception ex)
			{
				Log.Error(string.Concat(new object[]
				{
					"Exception while init loading meta header: ",
					filePath,
					"\n",
					ex
				}));
				this.ForceStop();
				throw;
			}
		}

		// Token: 0x06001715 RID: 5909 RVA: 0x000889E0 File Offset: 0x00086BE0
		public void FinalizeLoading()
		{
			if (Scribe.mode != LoadSaveMode.LoadingVars)
			{
				Log.Error("Called FinalizeLoading() but current mode is " + Scribe.mode);
				return;
			}
			try
			{
				Scribe.ExitNode();
				this.curXmlParent = null;
				this.curParent = null;
				this.curPathRelToParent = null;
				Scribe.mode = LoadSaveMode.Inactive;
				this.crossRefs.ResolveAllCrossReferences();
				this.initer.DoAllPostLoadInits();
			}
			catch (Exception arg)
			{
				Log.Error("Exception in FinalizeLoading(): " + arg);
				this.ForceStop();
				throw;
			}
		}

		// Token: 0x06001716 RID: 5910 RVA: 0x00088A74 File Offset: 0x00086C74
		public bool EnterNode(string nodeName)
		{
			if (this.curXmlParent != null)
			{
				XmlNode xmlNode = this.curXmlParent[nodeName];
				if (xmlNode == null && char.IsDigit(nodeName[0]))
				{
					xmlNode = this.curXmlParent.ChildNodes[int.Parse(nodeName)];
				}
				if (xmlNode == null)
				{
					return false;
				}
				this.curXmlParent = xmlNode;
			}
			this.curPathRelToParent = this.curPathRelToParent + "/" + nodeName;
			return true;
		}

		// Token: 0x06001717 RID: 5911 RVA: 0x00088AE4 File Offset: 0x00086CE4
		public void ExitNode()
		{
			if (this.curXmlParent != null)
			{
				this.curXmlParent = this.curXmlParent.ParentNode;
			}
			if (this.curPathRelToParent != null)
			{
				int num = this.curPathRelToParent.LastIndexOf('/');
				this.curPathRelToParent = ((num > 0) ? this.curPathRelToParent.Substring(0, num) : null);
			}
		}

		// Token: 0x06001718 RID: 5912 RVA: 0x00088B3C File Offset: 0x00086D3C
		public void ForceStop()
		{
			this.curXmlParent = null;
			this.curParent = null;
			this.curPathRelToParent = null;
			this.crossRefs.Clear(false);
			this.initer.Clear();
			if (Scribe.mode == LoadSaveMode.LoadingVars || Scribe.mode == LoadSaveMode.ResolvingCrossRefs || Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				Scribe.mode = LoadSaveMode.Inactive;
			}
		}

		// Token: 0x04000FFE RID: 4094
		public CrossRefHandler crossRefs = new CrossRefHandler();

		// Token: 0x04000FFF RID: 4095
		public PostLoadIniter initer = new PostLoadIniter();

		// Token: 0x04001000 RID: 4096
		public IExposable curParent;

		// Token: 0x04001001 RID: 4097
		public XmlNode curXmlParent;

		// Token: 0x04001002 RID: 4098
		public string curPathRelToParent;
	}
}
