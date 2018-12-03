using System.Collections;
﻿using UnityEngine;

public class StoneAbility : BaseAbility
{
    public GameObject Prefab;

    public AudioSource ThrowAudioSource;
    public AudioSource SmallHitAudioSource;

    public override void Use()
    {
        Debug.Log("Used Stone!");

        // make flare
        var stone = Instantiate(Prefab, transform.position, Quaternion.identity);

        var room = GameController.Instance.SelectedRoom;
        //room.SetVisited(true);

        var target = room.transform.position + new Vector3(Random.Range(-0.05f, 0.05f), Random.Range(-0.05f, 0.05f), 0);

        iTween.MoveTo(stone, target, 2f);

        foreach (Transform child in stone.transform)
            iTween.RotateBy(child.gameObject, new Vector3(0, 0, Random.Range(1f, 3f)), 2f);

        base.Use();

        ThrowAudioSource.Play();

        StartCoroutine(PlaySmallHitSoundOnComplete());
    }

    private IEnumerator PlaySmallHitSoundOnComplete() {
        yield return new WaitForSeconds(0.325f);

        SmallHitAudioSource.Play();
    }
}
