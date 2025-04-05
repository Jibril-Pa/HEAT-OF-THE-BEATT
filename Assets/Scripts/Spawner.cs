using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{
    public GameObject[] drumNotePrefabs; 
    public Transform[] drumTargets;     

    private float bpm = 120f;
    private float spawnInterval;

    void Start()
    {
        spawnInterval = 60f / bpm;
        StartCoroutine(SpawnNotes());
    }

    IEnumerator SpawnNotes()
    {
        while (true)
        {
            int index = Random.Range(0, drumNotePrefabs.Length);

            
            Vector3 spawnPos = new Vector3(
                drumTargets[index].position.x,
                1.28f,
                5f
            );

            GameObject note = Instantiate(drumNotePrefabs[index], spawnPos, Quaternion.identity);
            StartCoroutine(MoveNoteToTarget(note.transform, drumTargets[index]));
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    IEnumerator MoveNoteToTarget(Transform note, Transform target)
    {
        float speed = 2f;
        while (note != null && Vector3.Distance(note.position, target.position) > 0.1f)
        {
            note.position = Vector3.MoveTowards(note.position, target.position, speed * Time.deltaTime);
            yield return null;
        }

        if (note != null)
            Destroy(note.gameObject);
    }
}