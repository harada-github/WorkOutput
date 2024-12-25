using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Scene.CreateMap
{
    public sealed class MapPartsIconView : MonoBehaviour
    {
        [SerializeField] private Text _nameText;
        [SerializeField] private Image _iconImage;
        [SerializeField] private Canvas _canvas;
        [SerializeField] private DragMapParts _dragMapParts;
        [SerializeField] private int _index;
        private DragMapParts _createObject;
        private EventTrigger.Entry _pointerDownEntry;
        private EventTrigger.Entry _dragEntry;

        /// <summary>
        /// Awake
        /// </summary>
        private void Awake()
        {
            // 押下時にDragMapPartsを生成
            _pointerDownEntry = new EventTrigger.Entry();
            _pointerDownEntry.eventID = EventTriggerType.PointerDown;
            _pointerDownEntry.callback.AddListener((data) => IconOnPointerDown());
            _iconImage.GetComponent<EventTrigger>().triggers.Add(_pointerDownEntry);

            // ドラッグ対象を変更して生成時からドラッグ可能にする
            _dragEntry = new EventTrigger.Entry();
            _dragEntry.eventID = EventTriggerType.Drag;
            _dragEntry.callback.AddListener((data) => IconOnDrag((PointerEventData)data));
            _iconImage.GetComponent<EventTrigger>().triggers.Add(_dragEntry);
        }

        /// <summary>
        /// アイコンを押したとき
        /// </summary>
        private void IconOnPointerDown()
        {
            _createObject = Instantiate(_dragMapParts, new Vector3(_iconImage.transform.position.x - Screen.width / 2, _iconImage.transform.position.y - Screen.height / 2, 0), Quaternion.identity);

            _createObject.transform.SetParent(_canvas.transform.transform, false);

            _createObject.SetDragIconImage(_iconImage);

            _createObject.SetPartsConnectableData(_index);

            _createObject.gameObject.SetActive(true);
        }

        /// <summary>
        /// アイコンをドラッグしたとき
        /// </summary>
        private void IconOnDrag(PointerEventData eventData)
        {
            eventData.pointerDrag = _createObject.gameObject;
        }

        /// <summary>
        /// オブジェクトがなくなったとき
        /// </summary>
        private void OnDestroy()
        {
            _iconImage.GetComponent<EventTrigger>().triggers.Remove(_pointerDownEntry);
            _iconImage.GetComponent<EventTrigger>().triggers.Remove(_dragEntry);
        }
    }
}