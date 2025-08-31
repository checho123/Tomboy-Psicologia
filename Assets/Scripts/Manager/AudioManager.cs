using UnityEngine;

public class AudioManager : SingletonManager<AudioManager>
{
    private AudioSource m_AudioSource;

    static AudioManager()
    {
        EnablePersistence = false;
    }

    private void Start()
    {
        m_AudioSource = GetComponent<AudioSource>();
    }


    public void PlaySound(AudioClip clip)
    {
        m_AudioSource.PlayOneShot(clip);
    }

    public void StopSound() 
    {
        m_AudioSource.Stop();
    }

    public void PauseSound()
    {
        m_AudioSource.Pause();
    }
}
