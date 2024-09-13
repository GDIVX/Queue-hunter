using System.Collections;
using Game.Utility;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Core.SFX
{
    public class SfxManager : Singleton<SfxManager>
    {
        [SerializeField] private AudioSource prefab;

        public void PlaySoundAt(string sfxAddress, Vector3 position, float volume = 1f, float pitch = 1f)
        {
            Addressables.LoadAssetAsync<AudioClip>(sfxAddress).Completed += x => OnSfxLoad(x.Result, position);
        }

        private void OnSfxLoad(AudioClip clip, Vector3 position, float volume = 1f, float pitch = 1f)
        {
            AudioSource audioSource = CreateAudioSourceObject(position);

            audioSource.clip = clip;
            audioSource.volume = volume;
            audioSource.pitch = pitch;
            audioSource.Play();

            Destroy(audioSource, clip.length);
        }

        private AudioSource CreateAudioSourceObject(Vector3 position)
        {
            return GameObject.Instantiate(prefab, position, quaternion.identity);
        }
    }
}