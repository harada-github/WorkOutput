using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Scene.SoundTest
{
    public sealed class SoundTest : MonoBehaviour
    {
        private SoundManager _soundManager;
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _resetButton;
        [SerializeField] private Text _bgmPlayFileText;
        [SerializeField] private Text _sePlayFileText;
        [SerializeField] private Text _voicePlayFileText;
        [SerializeField] private InputField _bgmInputField;
        [SerializeField] private InputField _seInputField;
        [SerializeField] private InputField _voiceInputField;
        [SerializeField] private SoundTestScrollList _bgmScrollList;
        [SerializeField] private SoundTestScrollList _seScrollList;
        [SerializeField] private SoundTestScrollList _voiceScrollList;
        private const string InputFieldResetString = "--------------------------------";

        /// <summary>
        /// Start.
        /// </summary>
        private void Start()
        {
            // SoundManagerを取得
            _soundManager = SoundManager.instance;


            // リストの作成
            _bgmScrollList.SetSoundManager(_soundManager);
            _bgmScrollList.CreateViewList("bgm", _bgmPlayFileText);
            _bgmScrollList.SetViewList();
            _seScrollList.SetSoundManager(_soundManager);
            _seScrollList.CreateViewList("se", _sePlayFileText);
            _seScrollList.SetViewList();
            _voiceScrollList.SetSoundManager(_soundManager);
            _voiceScrollList.CreateViewList("voice", _voicePlayFileText);
            _voiceScrollList.SetViewList();


            // InputFieldのフィルター設定
            _bgmInputField.onValueChanged.AddListener(text => SoundFileNameFilter(_bgmScrollList, text));
            _seInputField.onValueChanged.AddListener(text => SoundFileNameFilter(_seScrollList, text));
            _voiceInputField.onValueChanged.AddListener(text => SoundFileNameFilter(_voiceScrollList, text));


            // 戻る・リセットボタンの設定
            _backButton.onClick.AddListener(() => SceneManager.LoadScene("Home"));
            _resetButton.onClick.AddListener(SetResetButton);
        }

        /// <summary>
        /// ResetButton関数.
        /// </summary>
        private void SetResetButton()
        {
            SoundManager.instance.StopBGM();
            SoundManager.instance.StopSE();
            SoundManager.instance.StopVOICE();
            _bgmInputField.text = String.Empty;
            _seInputField.text = String.Empty;
            _voiceInputField.text = String.Empty;
            _bgmPlayFileText.text = InputFieldResetString;
            _sePlayFileText.text = InputFieldResetString;
            _voicePlayFileText.text = InputFieldResetString;
        }

        /// <summary>
        /// フィルター機能.
        /// </summary>
        /// <param name="list">フィルターを適用したいScrollList</param>
        /// <param name="text">InputFieldで入力された文字列</param>
        private void SoundFileNameFilter(SoundTestScrollList list, string text)
        {
            list.SetViewList(text);
        }
    }
}
