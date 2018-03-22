using UnityEngine;

namespace Datenshi.Scripts.Entities
{
    public class EntityMiscController : MonoBehaviour
    {
        public AudioSource EntityAudioSource;

        public void PlayAudioOneShot(AudioClip clip)
        {
            EntityAudioSource.PlayOneShot(clip);
        }
    }
}