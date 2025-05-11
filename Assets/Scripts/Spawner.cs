using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class NoteSpawner : MonoBehaviour
{
    public GameObject[] drumNotePrefabs;
    public Transform[] drumTargets;
    public List<string> noteQueue = new List<string>();

    public float bpm = 120f;
    private float noteInterval;
    private int currentNoteIndex = 0;

    public float maxScore = 100f;
    public float minScore = 0f;
    public float maxDistance = 2f;
    private float totalScore = 0f;

    public Text scoreText;
    public Text comboText;
    private int currentCombo = 0;
    private int highestCombo = 0;

    public Transform playerHead;
    public float uiDistance = 2f;
    public float uiHeight = 1.5f;

    public float startDelay = 0f;
    public bool autoStart = true;
    private bool hasStarted = false;
    private float songStartTime;

    private bool gameFinished = false;

    void Start()
    {
        noteInterval = 60f / bpm;
        CreateVRUI();
        UpdateScoreDisplay();
        UpdateComboDisplay();

        if (autoStart)
        {
            StartSpawning();
        }
    }

    void CreateVRUI()
    {
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            GameObject canvasObj = new GameObject("VRCanvas");
            canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;

            CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
            scaler.dynamicPixelsPerUnit = 100f;

            canvasObj.AddComponent<GraphicRaycaster>();
        }

        if (playerHead == null)
        {
            playerHead = Camera.main.transform;
        }

        if (playerHead != null)
        {
            canvas.transform.position = playerHead.position + playerHead.forward * uiDistance;
            canvas.transform.position = new Vector3(canvas.transform.position.x, playerHead.position.y + uiHeight, canvas.transform.position.z);
            canvas.transform.rotation = Quaternion.LookRotation(canvas.transform.position - playerHead.position);
            canvas.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        }

        GameObject panelObj = new GameObject("ScorePanel");
        panelObj.transform.SetParent(canvas.transform, false);
        Image panelImage = panelObj.AddComponent<Image>();
        panelImage.color = new Color(0, 0, 0, 0.7f);

        RectTransform panelRect = panelObj.GetComponent<RectTransform>();
        panelRect.sizeDelta = new Vector2(400, 200);
        panelRect.localPosition = Vector3.zero;
        panelRect.localRotation = Quaternion.identity;

        GameObject scoreObj = new GameObject("ScoreText");
        scoreObj.transform.SetParent(panelObj.transform, false);
        scoreText = scoreObj.AddComponent<Text>();
        scoreText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        scoreText.fontSize = 36;
        scoreText.color = Color.white;
        scoreText.alignment = TextAnchor.MiddleCenter;

        RectTransform scoreRect = scoreText.GetComponent<RectTransform>();
        scoreRect.sizeDelta = new Vector2(380, 40);
        scoreRect.localPosition = new Vector3(0, 40, 0);

        GameObject comboObj = new GameObject("ComboText");
        comboObj.transform.SetParent(panelObj.transform, false);
        comboText = comboObj.AddComponent<Text>();
        comboText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        comboText.fontSize = 36;
        comboText.color = Color.yellow;
        comboText.alignment = TextAnchor.MiddleCenter;

        RectTransform comboRect = comboText.GetComponent<RectTransform>();
        comboRect.sizeDelta = new Vector2(380, 40);
        comboRect.localPosition = new Vector3(0, -40, 0);
    }

    void Update()
    {
        if (playerHead != null)
        {
            Canvas canvas = FindObjectOfType<Canvas>();
            if (canvas != null)
            {
                canvas.transform.position = playerHead.position + playerHead.forward * uiDistance;
                canvas.transform.position = new Vector3(canvas.transform.position.x, playerHead.position.y + uiHeight, canvas.transform.position.z);
                canvas.transform.rotation = Quaternion.LookRotation(canvas.transform.position - playerHead.position);
            }
        }
    }

    public void StartSpawning()
    {
        if (!hasStarted)
        {
            hasStarted = true;
            songStartTime = Time.time;
            StartCoroutine(DelayedSpawn());
        }
    }

    IEnumerator DelayedSpawn()
    {
        yield return new WaitForSeconds(startDelay);
        StartCoroutine(SpawnNotes());
    }

    IEnumerator SpawnNotes()
    {
        while (currentNoteIndex < noteQueue.Count && !gameFinished)
        {
            string nextNote = noteQueue[currentNoteIndex];
            int noteIndex = GetNoteIndex(nextNote);
            int targetIndex = noteIndex;

            Vector3 spawnPos = new Vector3(
                drumTargets[targetIndex].position.x,
                drumTargets[targetIndex].position.y + 1f,
                drumTargets[targetIndex].position.z
            );

            GameObject note = Instantiate(drumNotePrefabs[noteIndex], spawnPos, Quaternion.identity);
            NoteController noteController = note.AddComponent<NoteController>();
            noteController.Initialize(drumTargets[targetIndex], this);

            currentNoteIndex++;
            yield return new WaitForSeconds(noteInterval);
        }

        if (currentNoteIndex >= noteQueue.Count)
        {
            gameFinished = true;
            EndGame();
        }
    }

    public float CalculateScore(float distance)
    {
        distance = Mathf.Clamp(distance, 0f, maxDistance);
        float score = maxScore * (1f - (distance / maxDistance));
        totalScore += score;

        if (score > 0)
        {
            currentCombo++;
            if (currentCombo > highestCombo)
            {
                highestCombo = currentCombo;
            }
        }
        else
        {
            currentCombo = 0;
        }

        UpdateScoreDisplay();
        UpdateComboDisplay();

        return score;
    }

    private void UpdateScoreDisplay()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score: {totalScore:F0}";
        }
    }

    private void UpdateComboDisplay()
    {
        if (comboText != null)
        {
            if (currentCombo > 1)
            {
                comboText.text = $"Combo: {currentCombo}x";
            }
            else
            {
                comboText.text = "";
            }
        }
    }

    int GetNoteIndex(string noteName)
    {
        switch (noteName.ToLower())
        {
            case "hi-hat2":
                return 0;
            case "hi-hat":
                return 1;
            case "snare":
                return 2;
            case "cymval":
                return 3;
            case "tom1":
                return 4;
            case "tom2":
                return 5;
            case "tom3":
                return 6;
            default:
                return 0;
        }
    }

    void EndGame()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Final Score: {totalScore:F0}\nHighest Combo: {highestCombo}x";
        }
    }
}

public class NoteController : MonoBehaviour
{
    private Transform target;
    private NoteSpawner spawner;
    private bool isHit = false;
    private BoxCollider noteCollider;

    public void Initialize(Transform targetTransform, NoteSpawner noteSpawner)
    {
        target = targetTransform;
        spawner = noteSpawner;

        if (noteCollider == null)
        {
            noteCollider = gameObject.AddComponent<BoxCollider>();
            noteCollider.isTrigger = true;
            noteCollider.size = new Vector3(0.5f, 0.5f, 0.5f);
        }
    }

    void Update()
    {
        if (!isHit)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, 2f * Time.deltaTime);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!isHit && other.CompareTag("DrumStick"))
        {
            isHit = true;
            float distance = Vector3.Distance(transform.position, target.position);
            float score = spawner.CalculateScore(distance);
            Destroy(gameObject);
        }
    }
}
