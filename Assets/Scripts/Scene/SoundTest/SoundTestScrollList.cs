using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scene.SoundTest
{
    public sealed class SoundTestScrollList : MonoBehaviour
    {
        public GameObject Content;
        [SerializeField] private SoundPlayButtonView _soundPlayButtonView;
        private List<SoundPlayButtonView> _viewList;
        private SoundManager _soundManager;


        /// <summary>
        /// SoundManagerの取得
        /// </summary>
        /// <param name="soundManager"></param>
        public void SetSoundManager(SoundManager soundManager)
        {
            _soundManager = soundManager;
        }

        /// <summary>
        /// ViewListの作成
        /// </summary>
        /// <param name="soundType">BGM・SE・VOICEのどのタイプか</param>
        /// <param name="playFileText">現在のSound名を表すText</param>
        public void CreateViewList(string soundType, Text playFileText)
        {
            _viewList = new List<SoundPlayButtonView>();
            var clips = GetSoundClips(soundType);

            foreach (var clip in clips)
            {
                // インスタンス生成
                SoundPlayButtonView view = new SoundPlayButtonView();
                view = Instantiate(_soundPlayButtonView);

                // ボタン・テキストの設定
                switch (soundType)
                {
                    case "bgm":
                        var bgmType = (SoundManager.BGM_Type)Enum.Parse(typeof(SoundManager.BGM_Type), clip);
                        view.ViewButton.onClick.AddListener(() =>
                        {
                            _soundManager.PlayBGM(bgmType);
                            playFileText.text = clip;
                        });
                        view.ViewText.text = Enum.GetName(typeof(SoundManager.BGM_Type), bgmType);
                        break;

                    case "se":
                        var seType = (SoundManager.SE_Type)Enum.Parse(typeof(SoundManager.SE_Type), clip);
                        view.ViewButton.onClick.AddListener(() =>
                        {
                            _soundManager.PlaySE(seType);
                            playFileText.text = clip;
                        });
                        view.ViewText.text = Enum.GetName(typeof(SoundManager.SE_Type), seType);
                        break;

                    case "voice":
                        var voiceType = (SoundManager.VOICE_Type)Enum.Parse(typeof(SoundManager.VOICE_Type), clip);
                        view.ViewButton.onClick.AddListener(() =>
                        {
                            _soundManager.PlayVOICE(voiceType);
                            playFileText.text = clip;
                        });
                        view.ViewText.text = Enum.GetName(typeof(SoundManager.VOICE_Type), voiceType);
                        break;
                }

                // リストへ追加
                _viewList.Add(view);
            }
        }

        /// <summary>
        /// ViewListの設定
        /// Contentを親に設定することでScrollViewに表示
        /// </summary>
        public void SetViewList()
        {
            if (_viewList == null) return;

            // Contentを親に設定
            foreach (var view in _viewList)
            {
                view.transform.parent = Content.transform;
            }
        }

        /// <summary>
        /// ViewListの設定
        /// Contentを親に設定することでScrollViewに表示
        /// </summary>
        /// <param name="filterText">検索したい文字列</param>
        public void SetViewList(string filterText)
        {
            if (_viewList == null) return;

            // 親をリセット
            foreach (var view in _viewList)
            {
                view.transform.parent = null;
            }

            // filterTextが含まれているviewのみContentを親に設定
            foreach (var view in _viewList)
            {
                if (!view.ViewText.text.Contains(filterText)) continue;

                view.transform.parent = Content.transform;
            }
        }

        /// <summary>
        /// Clipの取得
        /// </summary>
        /// <param name="soundType">BGM・SE・VOICEのどのタイプか</param>
        private List<string> GetSoundClips(string soundType)
        {
            var clips = new List<string>();

            // タイプに応じてClipを取得
            switch (soundType)
            {
                case "bgm":
                    foreach (var clip in _soundManager.bgmClips)
                    {
                        clips.Add(clip.name);
                    }
                    break;

                case "se":
                    foreach (var clip in _soundManager.seClips)
                    {
                        clips.Add(clip.name);
                    }
                    break;

                case "voice":
                    foreach (var clip in _soundManager.voiceClips)
                    {
                        clips.Add(clip.name);
                    }
                    break;
            }

            return clips;
        }
    }
}
