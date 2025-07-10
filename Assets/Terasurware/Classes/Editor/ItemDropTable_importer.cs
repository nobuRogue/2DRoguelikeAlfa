using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;
using System.Xml.Serialization;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;

public class ItemDropTable_importer : AssetPostprocessor {
	private static readonly string filePath = "Assets/Resources/MasterData/ItemDropTable.xlsx";
	private static readonly string exportPath = "Assets/Resources/MasterData/ItemDropTable.asset";
	private static readonly string[] sheetNames = { "ItemDropTable", };
	
	static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
	{
		foreach (string asset in importedAssets) {
			if (!filePath.Equals (asset))
				continue;
				
			Entity_ItemDropTable data = (Entity_ItemDropTable)AssetDatabase.LoadAssetAtPath (exportPath, typeof(Entity_ItemDropTable));
			if (data == null) {
				data = ScriptableObject.CreateInstance<Entity_ItemDropTable> ();
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

					Entity_ItemDropTable.Sheet s = new Entity_ItemDropTable.Sheet ();
					s.name = sheetName;
				
					for (int i=1; i<= sheet.LastRowNum; i++) {
						IRow row = sheet.GetRow (i);
						ICell cell = null;
						
						Entity_ItemDropTable.Param p = new Entity_ItemDropTable.Param ();
						
					cell = row.GetCell(0); p.ID = (int)(cell == null ? 0 : cell.NumericCellValue);
					p.itemID = new int[4];
					cell = row.GetCell(1); p.itemID[0] = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(2); p.itemID[1] = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(3); p.itemID[2] = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(4); p.itemID[3] = (int)(cell == null ? 0 : cell.NumericCellValue);
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
