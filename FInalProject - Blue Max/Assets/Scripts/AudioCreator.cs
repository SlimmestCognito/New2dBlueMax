using UnityEngine;
using UnityEngine.Audio;

public class AudioCreator : MonoBehaviour
{
    public static AudioClip[] damageSFXsArray;
    public static AudioClip[] explosionSFXsArray;
    static AudioMixer mixer;

    private void Start()
    {
        damageSFXsArray = Resources.LoadAll<AudioClip>("Sounds/Damages");
        explosionSFXsArray = Resources.LoadAll<AudioClip>("Sounds/Explosions");
        mixer = Resources.Load("Mixer") as AudioMixer;
    }

    public static void CreateAudioSource(AudioClip c, float v, Transform t)
    {
        GameObject g = new GameObject("AudioSource");
        g.transform.position = t.position;
        AudioSource _as = g.AddComponent<AudioSource>();
        _as.outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[0]; //route it through sfx mixer
        _as.PlayOneShot(c, v); //play clip at volume
        Destroy(g, c.length);  //remove sound gameobject after clip ends
    }

}
