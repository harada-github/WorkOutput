using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace MySystem.ExcelToAsset
{
    public static class LoadExcelTestAssetData
    {
        private enum TestAssetDataEnum
        {
            Key = 0,
            Name,
            Hp,
            Attack,
            Defense,
            Speed
        }

        private const string SheetNameTestAssetData = "TestAssetData";
        private const string DirectoryPath = "Assets/Resources/ExcelToAsset";
        private const string AssetFilePath = DirectoryPath + "/TestAssetDatas.asset";
        private const string ExcelFilePath = DirectoryPath + "/TestAssetData.xlsm";


        [MenuItem("MySystem/UpdateAsset/TestAssetData")]
        /// <summary>
        /// assetファイルの更新
        /// </summary>
        private static void UpdateTestAssetData()
        {
            // ディレクトリチェック
            if (!Directory.Exists(DirectoryPath)) Directory.CreateDirectory(DirectoryPath);

            // Excelファイルチェック
            if (!System.IO.File.Exists(ExcelFilePath))
            {
                Debug.LogError("該当するExcelファイルが存在しません");
                return;
            }

            // Assetファイルチェック
            if (!System.IO.File.Exists(AssetFilePath))
            {
                // ファイルが無し→作成処理
                CreateTestAssetData();
            }
            else
            {
                // ファイルが存在→更新処理
                // Assetファイルを取得
                var asset = AssetDatabase.LoadAssetAtPath<TestAssetDatas>(AssetFilePath);

                // データの上書き
                asset = LoadExcel();

                // アセットのセーブ
                EditorUtility.SetDirty(asset);
                AssetDatabase.SaveAssets();

            }
        }

        /// <summary>
        /// assetファイルの作成
        /// </summary>
        private static void CreateTestAssetData()
        {
            // インスタンス作成
            var asset = ScriptableObject.CreateInstance<TestAssetDatas>();

            // データ作成
            asset = LoadExcel();

            // アセットファイルの作成
            AssetDatabase.CreateAsset(asset, AssetFilePath);
        }

        /// <summary>
        /// Excelからデータを取得してTestAssetDatasを作成
        /// </summary>
        private static TestAssetDatas LoadExcel()
        {
            // 初期化
            var testAssetDatas = new TestAssetDatas();
            testAssetDatas.data = new List<TestAssetData>();

            // Excelから1行ずつTestAssetDataを作成
            // Excelのファイルチェック
            if (Excel.Excel.TryRead(ExcelFilePath, out var result) == false)
            {
                return null;
            }

            var sheet = result.GetSheet(SheetNameTestAssetData);
            var cells = sheet.GetRowCells(0);
            var header = cells.ToDictionary(x => x.value, x => x.column);

            for (var i = 1; i < sheet.RowMax; i++) // 0 は文字列なので飛ばす
            {
                cells = sheet.GetRowCells(i);

                var testAssetdata = new TestAssetData
                (
                    cells[header[TestAssetDataEnum.Key.ToString()]].value,
                    cells[header[TestAssetDataEnum.Name.ToString()]].value,
                    LoadExcelBase.GetSafeInt(cells[header[TestAssetDataEnum.Hp.ToString()]].value),
                    LoadExcelBase.GetSafeInt(cells[header[TestAssetDataEnum.Attack.ToString()]].value),
                    LoadExcelBase.GetSafeInt(cells[header[TestAssetDataEnum.Defense.ToString()]].value),
                    LoadExcelBase.GetSafeFloat(cells[header[TestAssetDataEnum.Speed.ToString()]].value)
                );

                // 1行ずつデータを格納
                testAssetDatas.data.Add(testAssetdata);
            }

            return testAssetDatas;
        }
    }
}
