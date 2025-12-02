/*
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * All rights reserved.
 *
 * Licensed under the Oculus SDK License Agreement (the "License");
 * you may not use the Oculus SDK except in compliance with the License,
 * which is provided at the time of installation or download, or which
 * otherwise accompanies this software in either electronic or hard copy form.
 *
 * You may obtain a copy of the License at
 *
 * https://developer.oculus.com/licenses/oculussdk/
 *
 * Unless required by applicable law or agreed to in writing, the Oculus SDK
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using UnityEngine;
using System.Collections; // Coroutine (IEnumerator)
using Random = UnityEngine.Random;

namespace Oculus.Interaction
{
    /// <summary>
    /// Triggers audio clips. Place this component on a GameObject alongside a <cref="AudioSource" /> component.
    /// In an event wrapper, call AudioTrigger.PlayAudio() to trigger the audio.
    /// </summary>
    public class AudioTemple : MonoBehaviour
    {
        [SerializeField]
        private AudioSource _audioSource;

        /// <summary>
        /// A list of audio clips. The audio clip played will be randomly selected from the list.
        /// </summary>
        [Tooltip("Audio clip arrays with a value greater than 1 will have randomized playback.")]
        [SerializeField]
        private AudioClip[] _audioClips;

        // **[โค้ดใหม่]** ตัวแปรสำหรับกำหนดดีเลย์
        /// <summary>
        /// The delay in seconds between the end of a clip and the start of the next one.
        /// If this value is greater than 0 and Loop is enabled, a custom delayed loop will be used.
        /// </summary>
        [Tooltip("Delay (in seconds) between the end of a clip and the start of the next one. Set to 3 for 3-second delay loop.")]
        [SerializeField]
        private float _delayBetweenLoops = 5.0f;

        /// <summary>
        /// The default playback volume of the audio clip. Volume set here will override the volume set on the attached sound source component.
        /// </summary>
        [Tooltip("Volume set here will override the volume set on the attached sound source component.")]
        [Range(0f, 1f)]
        [SerializeField]
        private float _volume = 0.7f;
        public float Volume
        {
            get
            {
                return _volume;
            }
            set
            {
                _volume = value;
            }
        }

        /// <summary>
        /// A random range of volumes at which to play the audio clip. Check the 'Use Random Range' bool and adjust the min and max slider values for randomized volume level playback.
        /// </summary>
        [Tooltip("Check the 'Use Random Range' bool and adjust the min and max slider values for randomized volume level playback.")]
        [SerializeField]
        private MinMaxPair _volumeRandomization;
        public MinMaxPair VolumeRandomization
        {
            get
            {
                return _volumeRandomization;
            }
            set
            {
                _volumeRandomization = value;
            }
        }

        /// <summary>
        /// The default pitch of the audio clip. Pitch set here will override the volume set on the attached sound source component.
        /// </summary>
        [Tooltip("Pitch set here will override the volume set on the attached sound source component.")]
        [SerializeField]
        [Range(-3f, 3f)]
        [Space(10)]
        private float _pitch = 1f;
        public float Pitch
        {
            get
            {
                return _pitch;
            }
            set
            {
                _pitch = value;
            }
        }

        /// <summary>
        /// A random range of pitches at which to play the audio clip.
        /// Check the 'Use Random Range' bool and adjust the min and max slider values for randomized volume level playback.
        /// </summary>
        [Tooltip("Check the 'Use Random Range' bool and adjust the min and max slider values for randomized volume level playback.")]
        [SerializeField]
        private MinMaxPair _pitchRandomization;
        public MinMaxPair PitchRandomization
        {
            get
            {
                return _pitchRandomization;
            }
            set
            {
                _pitchRandomization = value;
            }
        }

        /// <summary>
        /// True by default. Set to false for sounds to bypass the spatializer plugin. Will override settings on attached audio source.
        /// </summary>
        [Tooltip("True by default. Set to false for sounds to bypass the spatializer plugin. Will override settings on attached audio source.")]
        [SerializeField]
        [Space(10)]
        private bool _spatialize = true;
        public bool Spatialize
        {
            get
            {
                return _spatialize;
            }
            set
            {
                _spatialize = value;
            }
        }

        /// <summary>
        /// False by default. Set to true to enable looping on this sound. Will override settings on attached audio source.
        /// </summary>
        [Tooltip("False by default. Set to true to enable looping on this sound. Will override settings on attached audio source.")]
        [SerializeField]
        private bool _loop = false;
        public bool Loop
        {
            get
            {
                return _loop;
            }
            set
            {
                _loop = value;
            }
        }

        /// <summary>
        /// 100% by default. Sets likelihood sample will actually play when called.
        /// </summary>
        [Tooltip("100% by default. Sets likelihood sample will actually play when called.")]
        [SerializeField]
        private float _chanceToPlay = 100;
        public float ChanceToPlay
        {
            get
            {
                return _chanceToPlay;
            }
            set
            {
                _chanceToPlay = value;
            }
        }

        /// <summary>
        /// If enabled, audio will play automatically when this gameobject is enabled.
        /// </summary>
        [Tooltip("If enabled, audio will play automatically when this gameobject is enabled.")]
        [SerializeField, Optional]
        private bool _playOnStart = false;

        private int _previousAudioClipIndex = -1;

        // **[โค้ดใหม่]** ตัวแปรสำหรับจัดการ Coroutine ของ Delayed Loop
        private Coroutine _loopRoutine;

        protected virtual void Start()
        {
            if (_audioSource == null)
            {
                _audioSource = gameObject.GetComponent<AudioSource>();
            }

            this.AssertField(_audioSource, nameof(_audioSource));
            this.AssertCollectionField(_audioClips, nameof(_audioClips));

            // Play audio on start if enabled
            if (_playOnStart)
            {
                PlayAudio();
            }
        }

        public void PlayAudio()
        {
            // Check if random chance is set
            float pick = Random.Range(0.0f, 100.0f);
            if (_chanceToPlay < 100 && pick > _chanceToPlay)
            {
                return;
            }

            // **[โค้ดใหม่]** หยุด Coroutine เดิมหากมีการเรียก PlayAudio ใหม่
            if (_loopRoutine != null)
            {
                StopCoroutine(_loopRoutine);
                _loopRoutine = null;
            }

            // Check if volume randomization is set
            if (_volumeRandomization.UseRandomRange == true)
            {
                _audioSource.volume = Random.Range(_volumeRandomization.Min, _volumeRandomization.Max);
            }
            else
            {
                _audioSource.volume = _volume;
            }

            // Check if pitch randomization is set
            if (_pitchRandomization.UseRandomRange == true)
            {
                _audioSource.pitch = Random.Range(_pitchRandomization.Min, _pitchRandomization.Max);
            }
            else
            {
                _audioSource.pitch = _pitch;
            }

            _audioSource.spatialize = _spatialize;

            // **
            bool useDelayedLoop = _loop && _delayBetweenLoops > 0;
            _audioSource.loop = _loop && !useDelayedLoop;

            _audioSource.clip = RandomClipWithoutRepeat();

            if (_audioSource.clip == null) return;

            if (useDelayedLoop)
            {
                // Coroutine
                _loopRoutine = StartCoroutine(DelayedLoopRoutine());
            }
            else
            {
                // loop
                _audioSource.Play();
            }
        }

        // **
        private IEnumerator DelayedLoopRoutine()
        {
            while (true)
            {
                // 1. เล่นเพลง: ต้องเลือกคลิปใหม่ทุกรอบ (เพื่อให้ RandomClipWithoutRepeat ทำงาน)
                _audioSource.clip = RandomClipWithoutRepeat();
                if (_audioSource.clip == null) break;

                _audioSource.Play();

                // 2. รอให้เพลงจบ: รอเป็นระยะเวลาเท่ากับความยาวของคลิป
                yield return new WaitForSeconds(_audioSource.clip.length);

                // 3. รอช่วงดีเลย์: รอตามระยะเวลาที่กำหนด
                yield return new WaitForSeconds(_delayBetweenLoops);
            }
            _loopRoutine = null; // ตั้งค่าเป็น null เมื่อจบการทำงาน
        }

        /// <summary>
        /// Choose a random clip without repeating the last clip
        /// </summary>
        private AudioClip RandomClipWithoutRepeat()
        {
            if (_audioClips.Length == 0) return null; // **

            if (_audioClips.Length == 1)
            {
                return _audioClips[0];
            }

            int randomOffset = Random.Range(1, _audioClips.Length);
            int index = (_previousAudioClipIndex + randomOffset) % _audioClips.Length;
            _previousAudioClipIndex = index;
            return _audioClips[index];
        }

        #region Inject

        public void InjectAllAudioTrigger(AudioSource audioSource, AudioClip[] audioClips)
        {
            InjectAudioSource(audioSource);
            InjectAudioClips(audioClips);
        }

        public void InjectAudioSource(AudioSource audioSource)
        {
            _audioSource = audioSource;
        }
        public void InjectAudioClips(AudioClip[] audioClips)
        {
            _audioClips = audioClips;
        }

        public void InjectOptionalPlayOnStart(bool playOnStart)
        {
            _playOnStart = playOnStart;
        }

        #endregion
    }

    [System.Serializable]
    public struct MinMaxPair
    {
        [SerializeField]
        private bool _useRandomRange;
        [SerializeField]
        private float _min;
        [SerializeField]
        private float _max;

        public bool UseRandomRange => _useRandomRange;
        public float Min => _min;
        public float Max => _max;
    }
}