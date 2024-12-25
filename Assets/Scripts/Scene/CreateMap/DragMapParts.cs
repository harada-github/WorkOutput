using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Scene.CreateMap
{
    public struct PartsConnectable
    {
        public bool isLeft;
        public bool isRight;
        public bool isUp;
        public bool isDown;
    }

    public sealed class DragMapParts : MonoBehaviour, IBeginDragHandler, IDragHandler, IDropHandler
    {
        private const int TileWidthCount = 5;
        private const int TileHeightCount = 5;
        public const int TileCount = TileWidthCount * TileHeightCount;

        [SerializeField] private CreateMap _createMap;
        [SerializeField] private RectTransform[] _tilePositions = new RectTransform[TileCount];
        [SerializeField] private Image _dragIconImage;
        [SerializeField] private RectTransform _rectTransform;
        public PartsConnectable PartsConnectableData;
        private int _tileNumber, _tileSize, _halfTileSize;
        private float _leftPos, _rightPos, _topPos, _bottomPos;
        private bool _isOnTile;
        private const int StartTileNumber = 0;
        private const int EndTileNumber = TileCount - 1;
        private const int CheckConnectableLeftUpNumber = 0;
        private const int CheckConnectableRightDownNumber = 4;
        private const bool NoneTileConnect = false;

        /// <summary>
        /// Awake
        /// </summary>
        private void Awake()
        {
            _tileNumber = 0;

            _tileSize = (int)_tilePositions[StartTileNumber].sizeDelta.x;
            _halfTileSize = _tileSize / 2;

            _leftPos = _tilePositions[StartTileNumber].position.x - _halfTileSize;
            _rightPos = _tilePositions[EndTileNumber].position.x + _halfTileSize;
            _topPos = _tilePositions[StartTileNumber].position.y + _halfTileSize;
            _bottomPos = _tilePositions[EndTileNumber].position.y - _halfTileSize;

            _isOnTile = false;
        }

        /// <summary>
        /// OnBeginDrag
        /// </summary>
        public void OnBeginDrag(PointerEventData eventData)
        {
            _createMap.DragMapPartsDatas[_tileNumber] = null;
            _createMap.RedTiles[_tileNumber].SetActive(false);
            RedTileCheck();
        }

        /// <summary>
        /// OnDrag
        /// </summary>
        public void OnDrag(PointerEventData eventData)
        {
            var PosX = eventData.position.x;
            var PosY = eventData.position.y;

            if (PosX > _leftPos && PosX < _rightPos && PosY < _topPos && PosY > _bottomPos)
            {
                _tileNumber = ((int)(PosX - _leftPos) / _tileSize) + ((int)(PosY - _topPos) / -_tileSize) * TileWidthCount;
                _rectTransform.position = _tilePositions[_tileNumber].position;
                _isOnTile = true;
            }
            else
            {
                _rectTransform.position = eventData.position;
                _isOnTile = false;
            }
        }

        /// <summary>
        /// OnDrop
        /// </summary>
        public void OnDrop(PointerEventData eventData)
        {
            if (_isOnTile)
            {
                if (_createMap.DragMapPartsDatas[_tileNumber] != null) Destroy(_createMap.DragMapPartsDatas[_tileNumber].gameObject);
                _createMap.DragMapPartsDatas[_tileNumber] = this;
                if (!CheckPartsConnectable(_tileNumber)) _createMap.RedTiles[_tileNumber].SetActive(true);
                RedTileCheck();
                SetPartsConnectable();
            }
            else
            {
                Destroy(this.gameObject);
            }
        }

        /// <summary>
        /// パーツの接続をセット
        /// </summary>
        private void SetPartsConnectable()
        {
            _createMap.DragMapPartsDatas[_tileNumber].PartsConnectableData = PartsConnectableData;
        }

        /// <summary>
        /// パーツが接続できるかの確認
        /// </summary>
        private bool CheckPartsConnectable(int tileNumber)
        {
            var leftTileNumber = tileNumber - 1;
            var rightTileNumber = tileNumber + 1;
            var upTileNumber = tileNumber - TileWidthCount;
            var downTileNumber = tileNumber + TileWidthCount;
            bool isConnect = true;

            // 配置マスの左マスをチェック
            if (tileNumber % TileWidthCount == CheckConnectableLeftUpNumber)
            {
                isConnect = NoneTileConnect == PartsConnectableData.isLeft;
            }
            else if (_createMap.DragMapPartsDatas[leftTileNumber] != null)
            {
                isConnect = _createMap.DragMapPartsDatas[leftTileNumber].PartsConnectableData.isRight == PartsConnectableData.isLeft;
            }
            if (!isConnect) return isConnect;

            // 配置マスの右マスをチェック
            if (tileNumber % TileWidthCount == CheckConnectableRightDownNumber)
            {
                isConnect = NoneTileConnect == PartsConnectableData.isRight;
            }
            else if (_createMap.DragMapPartsDatas[rightTileNumber] != null)
            {
                isConnect = _createMap.DragMapPartsDatas[rightTileNumber].PartsConnectableData.isLeft == PartsConnectableData.isRight;
            }
            if (!isConnect) return isConnect;

            // 配置マスの上マスをチェック
            if (tileNumber / TileWidthCount == CheckConnectableLeftUpNumber)
            {
                isConnect = NoneTileConnect == PartsConnectableData.isUp;
            }
            else if (_createMap.DragMapPartsDatas[upTileNumber] != null)
            {
                isConnect = _createMap.DragMapPartsDatas[upTileNumber].PartsConnectableData.isDown == PartsConnectableData.isUp;
            }
            if (!isConnect) return isConnect;

            // 配置マスの下マスをチェック
            if (tileNumber / TileWidthCount == CheckConnectableRightDownNumber)
            {
                isConnect = NoneTileConnect == PartsConnectableData.isDown;
            }
            else if (_createMap.DragMapPartsDatas[downTileNumber] != null)
            {
                isConnect = _createMap.DragMapPartsDatas[downTileNumber].PartsConnectableData.isUp == PartsConnectableData.isDown;
            }

            return isConnect;
        }

        /// <summary>
        /// アイコンの画像を設定
        /// </summary>
        public void SetDragIconImage(Image image)
        {
            _dragIconImage.sprite = image.sprite;
        }

        /// <summary>
        /// パーツの接続を設定
        /// </summary>
        public void SetPartsConnectableData(int index)
        {
            PartsConnectableData = _createMap.MapPartsConnectionLimit[index];
        }

        /// <summary>
        /// 赤いマップパーツがない時に保存・戻るをできないようにする
        /// </summary>
        private void RedTileCheck()
        {
            foreach (var data in _createMap.RedTiles)
            {
                if (data.activeSelf)
                {
                    _createMap.SaveButton.interactable = false;
                    _createMap.BackButton.interactable = false;
                    return;
                }
            }

            _createMap.SaveButton.interactable = true;
            _createMap.BackButton.interactable = true;
        }
    }
}