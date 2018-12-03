using UnityEngine;
using System;
using System.Collections;
using Random = UnityEngine.Random;

[RequireComponent(typeof (AudioSource))]
public class FlareAbility : BaseAbility
{
    public GameObject Prefab;

    public AudioSource ThrowAudioSource;
    public AudioSource SmallHitAudioSource;

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

        ThrowAudioSource.Play();

        iTween.MoveTo(flare, hashtable);

        StartCoroutine(PlaySmallHitSoundOnComplete());
    }

    private IEnumerator PlaySmallHitSoundOnComplete() {
        yield return new WaitForSeconds(0.325f);

        SmallHitAudioSource.Play();
    }

}
