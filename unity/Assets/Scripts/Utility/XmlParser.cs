using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml;

public class XmlParser 
{
	static XmlParser _xmlParser;
	public static XmlParser xmlParser
	{
		get
		{
			if (_xmlParser == null)
			{
				_xmlParser = new XmlParser();			
			}
			return _xmlParser;
		}
	}
	XmlDocument doc;

	public XmlParser()
	{
		doc = new XmlDocument();

		string path = "Locale/Strings";
		TextAsset source = Resources.Load<TextAsset>(path);

		if (source != null) doc.LoadXml(source.text);
		else Debug.LogWarning("WARNING: No Xml file found at " + path);
	}

	public string FindText(string path)
	{
		string text = doc.SelectSingleNode("Strings/" + path).InnerText;
		if (text != null) return text;
		else
		{
			Debug.LogWarning("WARNING: No text found at path location! path: Strings/" + path);
			return "Default Text";
		}
	}
}
