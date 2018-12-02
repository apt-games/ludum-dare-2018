using UnityEngine;
using System;
using System.Collections;
using Random = UnityEngine.Random;

[RequireComponent(typeof (AudioSource))]
public class FlareAbility : BaseAbility
{
    public GameObject Prefab;

    [HideInInspector]
    private AudioSource _audioSource;

    public override void Use()
    {
        Debug.Log("Used Flare!");

        // make flare
        var flare = Instantiate(Prefab, transform.position, Quaternion.identity);

        var room = GameController.Instance.SelectedRoom;
        room.SetVisited();

        var target = room.transform.position + new Vector3(Random.Range(-0.05f, 0.05f), Random.Range(-0.05f, 0.05f), 0);

        foreach (Transform child in flare.transform)
            iTween.RotateBy(child.gameObject, new Vector3(0, 0, Random.Range(1f, 3f)), 2f);

        base.Use();

        Hashtable hashtable = new Hashtable() {{
            "time", 2f
        }, {
            "position", target
        }};

        iTween.MoveTo(flare, hashtable);

        _audioSource = GetComponent<AudioSource>();
        StartCoroutine(PlaySoundOnComplete(_audioSource));
    }

    private IEnumerator PlaySoundOnComplete(AudioSource audioSource) {
        yield return new WaitForSeconds(0.325f);

        Debug.Log("Yo COMPLETE");

        audioSource.volume = 2f;
        audioSource.Play();

        Debug.Log(audioSource);
    }

}
