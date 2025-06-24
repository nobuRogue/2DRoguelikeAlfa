using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;
using System.Xml.Serialization;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;

public class MessageData_importer : AssetPostprocessor {
	private static readonly string filePath = "Assets/Resources/MasterData/MessageData.xlsx";
	private static readonly string exportPath = "Assets/Resources/MasterData/MessageData.asset";
	private static readonly string[] sheetNames = { "Menu","Character","Item","Log", };
	
	static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
	{
		foreach (string asset in importedAssets) {
			if (!filePath.Equals (asset))
				continue;
				
			Entity_MessageData data = (Entity_MessageData)AssetDatabase.LoadAssetAtPath (exportPath, typeof(Entity_MessageData));
			if (data == null) {
				data = ScriptableObject.CreateInstance<Entity_MessageData> ();
				AssetDatabase.CreateAsset ((ScriptableObject)data, exportPath);
				data.hideFlags = HideFlags.NotEditable;
			}
			
			data.sheets.Clear ();
			using (FileStream stream = File.Open (filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) {
				IWorkbook book = null;
				if (Path.GetExtension (filePath) == ".xls") {
					book = new HSSFWorkbook(stream);
				} else {
					book = new XSSFWorkbook(stream);
				}
				
				foreach(string sheetName in sheetNames) {
					ISheet sheet = book.GetSheet(sheetName);
					if( sheet == null ) {
						Debug.LogError("[QuestData] sheet not found:" + sheetName);
						continue;
					}

					Entity_MessageData.Sheet s = new Entity_MessageData.Sheet ();
					s.name = sheetName;
				
					for (int i=1; i<= sheet.LastRowNum; i++) {
						IRow row = sheet.GetRow (i);
						ICell cell = null;
						
						Entity_MessageData.Param p = new Entity_MessageData.Param ();
						
					cell = row.GetCell(0); p.ID = (int)(cell == null ? 0 : cell.NumericCellValue);
					p.text = new string[4];
					cell = row.GetCell(1); p.text[0] = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(2); p.text[1] = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(3); p.text[2] = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(4); p.text[3] = (cell == null ? "" : cell.StringCellValue);
						s.list.Add (p);
					}
					data.sheets.Add(s);
				}
			}

			ScriptableObject obj = AssetDatabase.LoadAssetAtPath (exportPath, typeof(ScriptableObject)) as ScriptableObject;
			EditorUtility.SetDirty (obj);
		}
	}
}
