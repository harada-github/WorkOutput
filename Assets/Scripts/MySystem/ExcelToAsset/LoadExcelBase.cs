using UnityEngine;

namespace MySystem.ExcelToAsset
{
    public static class LoadExcelBase
    {
        public const char SeparatorColumn = ',';
        public const char SeparatorParam = '_';
        public const char SeparatorVariable = '|';

        /// <summary>
        /// stringをbyteに変換
        /// </summary>
        /// <param name="param"></param>
        public static byte GetSafeByte(string param)
        {
            if (byte.TryParse(param, out var result))
            {
                return result;
            }

            return byte.MaxValue;
        }

        /// <summary>
        /// stringをshortに変換
        /// </summary>
        /// <param name="param"></param>
        public static short GetSafeShort(string param)
        {
            if (short.TryParse(param, out var result))
            {
                return result;
            }

            return short.MaxValue;
        }

        /// <summary>
        /// stringをintに変換
        /// </summary>
        /// <param name="param"></param>
        public static int GetSafeInt(string param)
        {
            if (int.TryParse(param, out var result))
            {
                return result;
            }

            return int.MaxValue;
        }

        /// <summary>
        /// stringをuintに変換
        /// </summary>
        /// <param name="param"></param>
        public static uint GetSafeUint(string param)
        {
            if (uint.TryParse(param, out var result))
            {
                return result;
            }

            return uint.MaxValue;
        }

        public static float GetSafeFloat(string param)
        {
            if (float.TryParse(
                param,
                out var result))
            {
                return result;
            }

            return float.MaxValue;
        }

        /// <summary>
        /// stringをboolに変換
        /// </summary>
        /// <param name="param"></param>
        public static bool GetSafeBool(string param)
        {
            if (bool.TryParse(param, out var result))
            {
                return result;
            }

            return false;
        }

        /// <summary>
        /// stringをvector3に変換
        /// </summary>
        /// <param name="param"></param>
        public static Vector3 GetSafeVector3(string param)
        {
            try
            {
                string[] inputDatas = param.Split(SeparatorVariable);
                return new Vector3(GetSafeFloat(inputDatas[0]), GetSafeFloat(inputDatas[1]), GetSafeFloat(inputDatas[2]));
            }
            catch
            {
                Debug.LogError($"正しくない入力データがあります\ndata:{param}");
                return Vector3.zero;
            }
        }
    }
}
