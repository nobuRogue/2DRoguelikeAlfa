using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;
using System.Xml.Serialization;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;

public class ItemData_importer : AssetPostprocessor {
	private static readonly string filePath = "Assets/Resources/MasterData/ItemData.xlsx";
	private static readonly string exportPath = "Assets/Resources/MasterData/ItemData.asset";
	private static readonly string[] sheetNames = { "ItemData", };
	
	static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
	{
		foreach (string asset in importedAssets) {
			if (!filePath.Equals (asset))
				continue;
				
			Entity_ItemData data = (Entity_ItemData)AssetDatabase.LoadAssetAtPath (exportPath, typeof(Entity_ItemData));
			if (data == null) {
				data = ScriptableObject.CreateInstance<Entity_ItemData> ();
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

					Entity_ItemData.Sheet s = new Entity_ItemData.Sheet ();
					s.name = sheetName;
				
					for (int i=1; i<= sheet.LastRowNum; i++) {
						IRow row = sheet.GetRow (i);
						ICell cell = null;
						
						Entity_ItemData.Param p = new Entity_ItemData.Param ();
						
					cell = row.GetCell(0); p.ID = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(2); p.nameID = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(3); p.category = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(5); p.actionID = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(6); p.burnID = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(7); p.lotID = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(8); p.MinValue = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(9); p.MaxValue = (int)(cell == null ? 0 : cell.NumericCellValue);
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
